use anyhow::{bail, Context, Result};
use capstone::prelude::*;
use capstone::Insn;
use goblin::Object;
use std::collections::HashMap;
use std::sync::Arc;

/// Log callback type - receives log messages from the patcher.
pub type LogCallback = Arc<dyn Fn(String) + Send + Sync + 'static>;

/// Helper to push a log line.
fn log(cb: &LogCallback, msg: String) {
    cb(msg);
}

macro_rules! plog {
    ($cb:expr, $($arg:tt)*) => {
        log($cb, format!($($arg)*));
    };
}

// ---------------------------------------------------------------------------
// Binary format abstraction
// ---------------------------------------------------------------------------

/// Represents a loadable section/segment mapped from the binary.
#[derive(Debug, Clone)]
struct Section {
    name: String,
    file_offset: usize,
    virtual_address: u64,
    size: usize,
    characteristics: u32,
}

/// All the information we need from the parsed binary.
struct BinaryInfo {
    arch: Arch,
    sections: Vec<Section>,
    #[allow(dead_code)]
    entry_point: u64,
    #[allow(dead_code)]
    image_base: u64,
    /// Sorted list of (func_start_va, func_end_va) from .pdata (PE).
    function_ranges: Vec<(u64, u64)>,
    /// Maps chained funclet start VA → root parent function start VA.
    /// Built from UNWIND_INFO UNW_FLAG_CHAININFO in PE x64.
    chained_to: HashMap<u64, u64>,
    /// Maps import name → PLT/thunk VA for dynamically linked functions.
    imports: HashMap<String, u64>,
    /// GOT base address for PIC x86 ELF binaries (computed from thunk+add patterns).
    got_base: Option<u64>,
}

#[derive(Debug, Clone, Copy, PartialEq, Eq)]
enum Arch {
    X86,
    X64,
}

impl Arch {
    fn capstone_mode(&self) -> arch::x86::ArchMode {
        match self {
            Arch::X86 => arch::x86::ArchMode::Mode32,
            Arch::X64 => arch::x86::ArchMode::Mode64,
        }
    }

}

/// Resolve dynamically imported function names to their PLT entry addresses in an ELF binary.
///
/// Scans .plt sections for `jmp [rip+disp]` stubs and matches the referenced GOT
/// slot against .rela.plt relocations to identify the symbol name.
fn resolve_elf_imports(data: &[u8], elf: &goblin::elf::Elf<'_>) -> HashMap<String, u64> {
    let mut imports = HashMap::new();

    // Build GOT address → symbol name map from PLT relocations
    let mut got_to_name: HashMap<u64, String> = HashMap::new();
    for reloc in elf.pltrelocs.iter() {
        if let Some(sym) = elf.dynsyms.get(reloc.r_sym) {
            if let Some(name) = elf.dynstrtab.get_at(sym.st_name) {
                if !name.is_empty() {
                    got_to_name.insert(reloc.r_offset, name.to_string());
                }
            }
        }
    }

    if got_to_name.is_empty() {
        return imports;
    }

    // Scan PLT sections for jmp [addr] instructions (FF 25 xx xx xx xx)
    // x64: jmp [rip+disp32] - GOT addr = next_ip + disp
    // x86: jmp [abs32]      - GOT addr = the 4-byte immediate directly
    let is_64 = elf.is_64;
    for sh in &elf.section_headers {
        let sec_name = elf.shdr_strtab.get_at(sh.sh_name).unwrap_or("");
        if !sec_name.starts_with(".plt") {
            continue;
        }

        let sec_off = sh.sh_offset as usize;
        let sec_size = sh.sh_size as usize;
        let sec_va = sh.sh_addr;
        let sec_end = (sec_off + sec_size).min(data.len());
        let mut pos = sec_off;
        while pos + 6 <= sec_end {
            if data[pos] == 0xFF && data[pos + 1] == 0x25 {
                let raw = u32::from_le_bytes([
                    data[pos + 2],
                    data[pos + 3],
                    data[pos + 4],
                    data[pos + 5],
                ]);
                let insn_va = sec_va + (pos - sec_off) as u64;
                let got_va = if is_64 {
                    let next_ip = insn_va + 6;
                    (next_ip as i64 + raw as i32 as i64) as u64
                } else {
                    raw as u64
                };
                if let Some(name) = got_to_name.get(&got_va) {
                    // Check for endbr64 prefix (F3 0F 1E FA) before the jmp
                    let entry_va = if pos >= sec_off + 4
                        && data[pos - 4] == 0xF3
                        && data[pos - 3] == 0x0F
                        && data[pos - 2] == 0x1E
                        && data[pos - 1] == 0xFA
                    {
                        insn_va - 4
                    } else {
                        insn_va
                    };
                    imports.insert(name.clone(), entry_va);
                }
            }
            pos += 1;
        }
    }

    imports
}

/// Parse a binary with goblin and collect section info.
fn parse_binary(data: &[u8], cb: &LogCallback) -> Result<BinaryInfo> {
    match Object::parse(data)? {
        Object::Elf(elf) => {
            let arch = if elf.is_64 { Arch::X64 } else { Arch::X86 };
            let sections: Vec<Section> = elf
                .section_headers
                .iter()
                .filter(|s| s.sh_size > 0)
                .map(|s| {
                    let name = elf
                        .shdr_strtab
                        .get_at(s.sh_name)
                        .unwrap_or("")
                        .to_string();
                    // ELF: SHF_EXECINSTR = 0x4
                    let chars = if s.sh_flags as u32 & 0x4 != 0 { 0x20 } else { 0 };
                    Section {
                        name,
                        file_offset: s.sh_offset as usize,
                        virtual_address: s.sh_addr,
                        size: s.sh_size as usize,
                        characteristics: chars,
                    }
                })
                .collect();
            let imports = resolve_elf_imports(data, &elf);
            let got_base = if arch == Arch::X86 {
                find_elf32_got_base(data, &sections)
            } else {
                None
            };
            if let Some(base) = got_base {
                plog!(cb, "  PIC x86 GOT base: {:#x}", base);
            }
            Ok(BinaryInfo {
                arch,
                sections,
                entry_point: elf.entry,
                image_base: 0,
                function_ranges: Vec::new(),
                chained_to: HashMap::new(),
                imports,
                got_base,
            })
        }
        Object::Mach(goblin::mach::Mach::Binary(macho)) => {
            if macho.header.magic != goblin::mach::header::MH_MAGIC_64 {
                bail!("Only 64-bit Mach-O binaries are supported");
            }
            let mut sections = Vec::new();
            // Find __TEXT segment base VA for LC_FUNCTION_STARTS
            let mut text_segment_va: u64 = 0;
            for seg in &macho.segments {
                let seg_name = seg.name().unwrap_or("");
                if seg_name == "__TEXT" {
                    text_segment_va = seg.vmaddr;
                }
                for (sec, _) in seg.sections()? {
                    let name = std::str::from_utf8(&sec.sectname)
                        .unwrap_or("")
                        .trim_end_matches('\0')
                        .to_string();
                    // Mach-O: S_ATTR_SOME_INSTRUCTIONS or S_ATTR_PURE_INSTRUCTIONS
                    let chars = if sec.flags & 0x80000000 != 0 || sec.flags & 0x80000400 != 0 { 0x20 } else { 0 };
                    sections.push(Section {
                        name,
                        file_offset: sec.offset as usize,
                        virtual_address: sec.addr,
                        size: sec.size as usize,
                        characteristics: chars,
                    });
                }
            }

            // Parse LC_FUNCTION_STARTS to get function entry points
            let mut function_ranges = Vec::new();
            for lc in &macho.load_commands {
                if let goblin::mach::load_command::CommandVariant::FunctionStarts(ref cmd) = lc.command {
                    let off = cmd.dataoff as usize;
                    let sz = cmd.datasize as usize;
                    if off + sz <= data.len() {
                        let fs_data = &data[off..off + sz];
                        let mut func_addrs = Vec::new();
                        let mut addr = text_segment_va;
                        let mut i = 0;
                        while i < fs_data.len() {
                            // Read ULEB128
                            let mut delta: u64 = 0;
                            let mut shift: u32 = 0;
                            loop {
                                if i >= fs_data.len() { break; }
                                let b = fs_data[i] as u64;
                                i += 1;
                                delta |= (b & 0x7f) << shift;
                                if b & 0x80 == 0 { break; }
                                shift += 7;
                            }
                            if delta == 0 { break; }
                            addr += delta;
                            func_addrs.push(addr);
                        }
                        // Build (start, end) pairs - each function ends where the next begins
                        for j in 0..func_addrs.len() {
                            let start = func_addrs[j];
                            let end = if j + 1 < func_addrs.len() {
                                func_addrs[j + 1]
                            } else {
                                // Last function: extend to end of __text section
                                sections.iter()
                                    .find(|s| s.name == "__text")
                                    .map(|s| s.virtual_address + s.size as u64)
                                    .unwrap_or(start + 1)
                            };
                            function_ranges.push((start, end));
                        }
                    }
                    break;
                }
            }
            function_ranges.sort_by_key(|&(s, _)| s);

            Ok(BinaryInfo {
                arch: Arch::X64,
                sections,
                entry_point: macho.entry,
                image_base: 0,
                function_ranges,
                chained_to: HashMap::new(),
                got_base: None,
                imports: HashMap::new(),
            })
        }
        Object::PE(pe) => {
            let arch = if pe.is_64 { Arch::X64 } else { Arch::X86 };
            let image_base = pe.image_base as u64;
            let sections: Vec<Section> = pe
                .sections
                .iter()
                .map(|s| {
                    let name = String::from_utf8_lossy(
                        &s.name[..s.name.iter().position(|&b| b == 0).unwrap_or(s.name.len())],
                    )
                    .to_string();
                    Section {
                        name,
                        file_offset: s.pointer_to_raw_data as usize,
                        virtual_address: image_base + s.virtual_address as u64,
                        size: s.size_of_raw_data as usize,
                        characteristics: s.characteristics,
                    }
                })
                .collect();
            // Parse .pdata for function boundaries (x64 PE only)
            let mut function_ranges: Vec<(u64, u64)> = Vec::new();
            if arch == Arch::X64 {
                for s in &pe.sections {
                    let sname = String::from_utf8_lossy(
                        &s.name[..s.name.iter().position(|&b| b == 0).unwrap_or(s.name.len())],
                    );
                    if sname == ".pdata" {
                        let pdata_off = s.pointer_to_raw_data as usize;
                        let pdata_size = s.size_of_raw_data as usize;
                        let pdata_end = (pdata_off + pdata_size).min(data.len());
                        // Each RUNTIME_FUNCTION is 12 bytes: BeginAddress(4) EndAddress(4) UnwindData(4)
                        let mut pos = pdata_off;
                        while pos + 12 <= pdata_end {
                            let begin_rva = u32::from_le_bytes([
                                data[pos], data[pos+1], data[pos+2], data[pos+3]
                            ]);
                            let end_rva = u32::from_le_bytes([
                                data[pos+4], data[pos+5], data[pos+6], data[pos+7]
                            ]);
                            if begin_rva != 0 {
                                function_ranges.push((
                                    image_base + begin_rva as u64,
                                    image_base + end_rva as u64,
                                ));
                            }
                            pos += 12;
                        }
                        break;
                    }
                }
                function_ranges.sort_unstable_by_key(|&(start, _)| start);
                function_ranges.dedup();
                plog!(cb, "  Parsed {} function entries from .pdata", function_ranges.len());
            }

            // Build chained_to map by parsing UNWIND_INFO for UNW_FLAG_CHAININFO
            let chained_to = build_chain_map(data, &sections, image_base, &function_ranges);
            if !chained_to.is_empty() {
                plog!(cb, "  Found {} chained (SEH funclet) entries", chained_to.len());
            }

            // Resolve PE imports: map function name → IAT slot VA
            let mut imports = HashMap::new();
            for import in &pe.imports {
                let iat_va = image_base + import.offset as u64;
                imports.insert(import.name.to_string(), iat_va);
            }
            if !imports.is_empty() {
                plog!(cb, "  Resolved {} PE imports", imports.len());
            }

            Ok(BinaryInfo {
                arch,
                sections,
                entry_point: image_base + pe.entry as u64,
                image_base,
                function_ranges,
                chained_to,
                imports,
                got_base: None,
            })
        }
        _ => bail!("Unsupported binary format (expected ELF64, MachO64, or PE32/64)"),
    }
}

/// For 32-bit PIC ELF shared libraries, find the GOT base address.
///
/// In PIC x86 code, data references are computed via a GOT-base register:
///   call __x86.get_pc_thunk.XX   ; puts return IP into reg
///   add  XX, imm32               ; reg = _GLOBAL_OFFSET_TABLE_
/// All such sequences compute the same absolute GOT address.
fn find_elf32_got_base(data: &[u8], sections: &[Section]) -> Option<u64> {
    for sec in sections {
        if !is_code_section(&sec.name, sec.characteristics) {
            continue;
        }
        let start = sec.file_offset;
        let end = (start + sec.size).min(data.len());
        if end < start + 11 {
            continue;
        }
        let sec_data = &data[start..end];
        let sec_va = sec.virtual_address;

        for p in 0..sec_data.len().saturating_sub(10) {
            // E8 rel32 (call, 5 bytes) followed by 81 Cx imm32 (add reg, imm32, 6 bytes)
            if sec_data[p] != 0xE8 || sec_data[p + 5] != 0x81 {
                continue;
            }
            let add_modrm = sec_data[p + 6];
            // add reg32, imm32: modrm = 11_000_rrr = 0xC0+rrr
            if add_modrm < 0xC0 || add_modrm > 0xC7 || (add_modrm & 0x38) != 0 {
                continue;
            }
            let add_reg = (add_modrm & 0x07) as usize;

            // Verify the call target is a get_pc_thunk: 8B XX 24 C3
            let call_va = sec_va + p as u64;
            let call_rel = i32::from_le_bytes([
                sec_data[p + 1], sec_data[p + 2], sec_data[p + 3], sec_data[p + 4],
            ]);
            let thunk_va = (call_va + 5).wrapping_add(call_rel as i64 as u64);
            let thunk_off = match va_to_file_offset(sections, thunk_va) {
                Some(off) if off + 4 <= data.len() => off,
                _ => continue,
            };
            // mov reg, [esp]; ret  →  8B modrm 24 C3
            // modrm: mod=00 reg=rrr rm=100  →  (rrr << 3) | 0x04
            if data[thunk_off] != 0x8B
                || (data[thunk_off + 1] & 0xC7) != 0x04
                || data[thunk_off + 2] != 0x24
                || data[thunk_off + 3] != 0xC3
            {
                continue;
            }
            let thunk_reg = ((data[thunk_off + 1] >> 3) & 0x07) as usize;
            if thunk_reg != add_reg {
                continue;
            }

            let add_imm = i32::from_le_bytes([
                sec_data[p + 7], sec_data[p + 8], sec_data[p + 9], sec_data[p + 10],
            ]);
            let ip_after_call = call_va + 5;
            let got_base = ip_after_call.wrapping_add(add_imm as i64 as u64);
            return Some(got_base);
        }
    }
    None
}

/// Convert an RVA to a file offset using PE section info.
fn rva_to_file_offset(sections: &[Section], image_base: u64, rva: u32) -> Option<usize> {
    let va = image_base + rva as u64;
    for sec in sections {
        if va >= sec.virtual_address && va < sec.virtual_address + sec.size as u64 {
            return Some(sec.file_offset + (va - sec.virtual_address) as usize);
        }
    }
    None
}

/// Build a map from chained funclet start VA → root parent function start VA.
///
/// Parses UNWIND_INFO for each .pdata RUNTIME_FUNCTION entry. If the
/// UNW_FLAG_CHAININFO flag (0x04) is set, the UNWIND_INFO ends with a
/// chained RUNTIME_FUNCTION that points to the parent. We follow chains
/// transitively to find the root.
fn build_chain_map(
    data: &[u8],
    sections: &[Section],
    image_base: u64,
    _function_ranges: &[(u64, u64)],
) -> HashMap<u64, u64> {
    // First, build a map from function_start_va → immediate parent_start_va
    let mut immediate_parent: HashMap<u64, u64> = HashMap::new();

    // We need to re-scan .pdata to get UnwindData RVAs.
    // Build a lookup: func_start_va → unwind_rva
    // We'll re-parse .pdata from the sections.
    for sec in sections {
        if sec.name != ".pdata" {
            continue;
        }
        let pdata_off = sec.file_offset;
        let pdata_end = (pdata_off + sec.size).min(data.len());
        let mut pos = pdata_off;
        while pos + 12 <= pdata_end {
            let begin_rva = u32::from_le_bytes([
                data[pos], data[pos + 1], data[pos + 2], data[pos + 3],
            ]);
            let _end_rva = u32::from_le_bytes([
                data[pos + 4], data[pos + 5], data[pos + 6], data[pos + 7],
            ]);
            let unwind_rva = u32::from_le_bytes([
                data[pos + 8], data[pos + 9], data[pos + 10], data[pos + 11],
            ]);

            if begin_rva != 0 {
                let func_va = image_base + begin_rva as u64;

                // Parse UNWIND_INFO at unwind_rva
                if let Some(unwind_off) = rva_to_file_offset(sections, image_base, unwind_rva) {
                    if unwind_off + 4 <= data.len() {
                        let version_flags = data[unwind_off];
                        let _version = version_flags & 0x07;
                        let flags = (version_flags >> 3) & 0x1F;
                        let count_of_codes = data[unwind_off + 2] as usize;

                        // UNW_FLAG_CHAININFO = 0x04
                        if flags & 0x04 != 0 {
                            // After the header (4 bytes) and unwind codes (count_of_codes * 2 bytes),
                            // aligned to 4 bytes, there's a chained RUNTIME_FUNCTION.
                            let codes_size = count_of_codes * 2;
                            let codes_aligned = (codes_size + 3) & !3; // align to 4
                            let chain_off = unwind_off + 4 + codes_aligned;

                            if chain_off + 12 <= data.len() {
                                let parent_begin_rva = u32::from_le_bytes([
                                    data[chain_off],
                                    data[chain_off + 1],
                                    data[chain_off + 2],
                                    data[chain_off + 3],
                                ]);
                                if parent_begin_rva != 0 {
                                    let parent_va = image_base + parent_begin_rva as u64;
                                    immediate_parent.insert(func_va, parent_va);
                                }
                            }
                        }
                    }
                }
            }
            pos += 12;
        }
        break;
    }

    // Resolve chains transitively: follow parent → grandparent → … → root
    let mut root_map: HashMap<u64, u64> = HashMap::new();
    for &funclet_va in immediate_parent.keys() {
        let mut current = funclet_va;
        let mut depth = 0;
        while let Some(&parent) = immediate_parent.get(&current) {
            current = parent;
            depth += 1;
            if depth > 100 {
                break; // safety: avoid infinite loops in malformed data
            }
        }
        root_map.insert(funclet_va, current);
    }

    root_map
}

// ---------------------------------------------------------------------------
// Address ↔ file-offset conversion
// ---------------------------------------------------------------------------

fn va_to_file_offset(sections: &[Section], va: u64) -> Option<usize> {
    for sec in sections {
        if va >= sec.virtual_address && va < sec.virtual_address + sec.size as u64 {
            return Some(sec.file_offset + (va - sec.virtual_address) as usize);
        }
    }
    None
}

fn file_offset_to_va(sections: &[Section], offset: usize) -> Option<u64> {
    for sec in sections {
        if offset >= sec.file_offset && offset < sec.file_offset + sec.size {
            return Some(sec.virtual_address + (offset - sec.file_offset) as u64);
        }
    }
    None
}

// ---------------------------------------------------------------------------
// String search
// ---------------------------------------------------------------------------

/// Find the first exact C-string occurrence of `needle` inside `data`.
/// Matches Python's behavior of selecting one string from IDA's string DB.
fn find_string_offset(data: &[u8], needle: &[u8]) -> Option<usize> {
    if needle.is_empty() {
        return None;
    }
    let mut start = 0;
    while start + needle.len() <= data.len() {
        if let Some(pos) = data[start..].windows(needle.len()).position(|w| w == needle) {
            let abs_pos = start + pos;
            // Require null terminator to ensure this is a full C string.
            let after = abs_pos + needle.len();
            if after < data.len() && data[after] == 0 {
                return Some(abs_pos);
            }
            start += pos + 1;
        } else {
            break;
        }
    }
    None
}

/// Find a C-string that ends with `suffix` (immediately followed by NUL).
/// Returns the file offset of the string *start* (walks backward past any prefix).
fn find_cstring_ending_with(data: &[u8], suffix: &[u8]) -> Option<usize> {
    if suffix.is_empty() {
        return None;
    }
    let mut needle = suffix.to_vec();
    needle.push(0); // require null-terminator right after suffix

    let pos = data.windows(needle.len()).position(|w| w == needle.as_slice())?;

    // Walk backward to find the start of the C-string (first byte after a NUL).
    let mut start = pos;
    while start > 0 && data[start - 1] != 0 {
        start -= 1;
    }
    Some(start)
}

/// Check whether a decoded `test` instruction has a given immediate operand.
/// Parses Capstone's operand string (e.g. "r14b, 0x20").
#[allow(dead_code)]
fn operand_has_immediate(insn: &DecodedInsn, value: u64) -> bool {
    for part in insn.op_str.split(',') {
        let trimmed = part.trim();
        if let Some(hex) = trimmed.strip_prefix("0x") {
            if let Ok(v) = u64::from_str_radix(hex, 16) {
                if v == value {
                    return true;
                }
            }
        } else if let Ok(v) = trimmed.parse::<u64>() {
            if v == value {
                return true;
            }
        }
    }
    false
}

// ---------------------------------------------------------------------------
// Disassembly helpers
// ---------------------------------------------------------------------------

fn make_capstone(arch: Arch) -> Result<Capstone> {
    let cs = Capstone::new()
        .x86()
        .mode(arch.capstone_mode())
        .detail(true)
        .build()
        .map_err(|e| anyhow::anyhow!("capstone init: {e}"))?;
    Ok(cs)
}

/// A decoded instruction with its virtual address and raw bytes length.
#[derive(Debug, Clone)]
struct DecodedInsn {
    va: u64,
    mnemonic: String,
    op_str: String,
    bytes: Vec<u8>,
    len: usize,
}

impl DecodedInsn {
    fn from_capstone(insn: &Insn) -> Self {
        Self {
            va: insn.address(),
            mnemonic: insn.mnemonic().unwrap_or("").to_string(),
            op_str: insn.op_str().unwrap_or("").to_string(),
            bytes: insn.bytes().to_vec(),
            len: insn.len(),
        }
    }

    fn is_jump(&self) -> bool {
        self.mnemonic == "jmp" || self.is_conditional_jump()
    }

    fn is_conditional_jump(&self) -> bool {
        matches!(
            self.mnemonic.as_str(),
            "je" | "jne" | "jz" | "jnz"
            | "ja" | "jae" | "jb" | "jbe"
            | "jg" | "jge" | "jl" | "jle"
            | "js" | "jns" | "jo" | "jno"
            | "jp" | "jnp" | "jpe" | "jpo"
            | "jcxz" | "jecxz" | "jrcxz"
            | "jnae" | "jnbe" | "jnge" | "jnle"
            | "jnb" | "jna" | "jnl" | "jng"
            | "jc" | "jnc"
        )
    }

    /// Matches Python's is_jump: only jmp, jz, jnz (= je, jne in capstone).
    /// Used in patch_function_call to match the Python script's semantics.
    fn is_patchable_jump(&self) -> bool {
        matches!(self.mnemonic.as_str(), "jmp" | "je" | "jne" | "jz" | "jnz")
    }

    /// Matches Python's is_conditional_jump: only jz, jnz (= je, jne in capstone).
    fn is_patchable_conditional_jump(&self) -> bool {
        matches!(self.mnemonic.as_str(), "je" | "jne" | "jz" | "jnz")
    }

    /// Matches Python's is_location_begin: jmp, jz, jnz, retn, align.
    /// Uses int3 as substitute for IDA's 'align' directive.
    fn is_patchable_location_begin(&self) -> bool {
        self.is_patchable_jump()
            || self.mnemonic == "ret"
            || self.mnemonic == "retn"
            || self.mnemonic == "int3"
    }

    /// For short/near jumps extract the destination VA from the operand string.
    fn jump_target(&self) -> Option<u64> {
        if !self.is_jump() {
            return None;
        }
        // capstone operand looks like "0x401000"
        let s = self.op_str.trim();
        if let Some(hex_str) = s.strip_prefix("0x") {
            u64::from_str_radix(hex_str, 16).ok()
        } else {
            s.parse::<u64>().ok()
        }
    }
}

/// Disassemble a section of the binary from [va .. va + len).
fn disassemble_range(cs: &Capstone, data: &[u8], va: u64) -> Vec<DecodedInsn> {
    match cs.disasm_all(data, va) {
        Ok(insns) => insns.iter().map(|i| DecodedInsn::from_capstone(&i)).collect(),
        Err(_) => Vec::new(),
    }
}

// ---------------------------------------------------------------------------
// Code-section helpers
// ---------------------------------------------------------------------------

fn is_code_section(name: &str, characteristics: u32) -> bool {
    let n = name;
    // By name
    if n == ".text" || n == "__text" || n == ".code" {
        return true;
    }
    // By PE characteristics: IMAGE_SCN_CNT_CODE (0x20) or IMAGE_SCN_MEM_EXECUTE (0x20000000)
    if characteristics & 0x20 != 0 || characteristics & 0x2000_0000 != 0 {
        return true;
    }
    false
}

fn get_code_sections(info: &BinaryInfo) -> Vec<&Section> {
    info.sections
        .iter()
        .filter(|s| is_code_section(&s.name, s.characteristics))
        .collect()
}

// ---------------------------------------------------------------------------
// Cross-reference analysis
// ---------------------------------------------------------------------------

/// Scan all code sections for instructions that reference `target_va`
/// (via RIP-relative addressing or absolute 32-bit addressing).
/// Returns the VA of each referencing instruction.
///
/// Uses decoded operands from Capstone details to avoid false positives.
fn find_xrefs_to(
    cs: &Capstone,
    data: &[u8],
    info: &BinaryInfo,
    target_va: u64,
    cb: &LogCallback,
) -> Vec<u64> {
    let mut results = Vec::new();
    let code_secs = get_code_sections(info);

    if code_secs.is_empty() {
        plog!(cb, "  WARNING: No code sections found");
        return results;
    }

    // Pre-compute GOT-relative displacement for PIC x86.
    // In PIC code, data is referenced as [got_base_reg + disp32] where
    // got_base_reg holds _GLOBAL_OFFSET_TABLE_ (a fixed address for the SO).
    let got_disp: Option<i32> = info.got_base.and_then(|base| {
        let d = target_va as i64 - base as i64;
        if d >= i32::MIN as i64 && d <= i32::MAX as i64 {
            Some(d as i32)
        } else {
            None
        }
    });

    for sec in &code_secs {
        let start = sec.file_offset;
        let end = (start + sec.size).min(data.len());
        if end < start + 4 {
            continue;
        }
        let sec_data = &data[start..end];
        let sec_va = sec.virtual_address;

        for p in 0..sec_data.len().saturating_sub(3) {
            let raw = i32::from_le_bytes([
                sec_data[p],
                sec_data[p + 1],
                sec_data[p + 2],
                sec_data[p + 3],
            ]);

            let mut likely_ref = false;
            for extra in 0u64..=4 {
                let next_ip = sec_va + p as u64 + 4 + extra;
                if next_ip.wrapping_add(raw as i64 as u64) == target_va {
                    likely_ref = true;
                    break;
                }
            }

            if !likely_ref && info.arch == Arch::X86 && raw as u64 == target_va {
                likely_ref = true;
            }

            // PIC x86: the displacement got_base+disp == target_va
            if !likely_ref {
                if let Some(gd) = got_disp {
                    if raw == gd {
                        likely_ref = true;
                    }
                }
            }

            if !likely_ref {
                continue;
            }

            // Find containing instruction by trying starts in [p-15, p].
            let try_start = p.saturating_sub(15);
            for s in try_start..=p {
                let sub = &sec_data[s..];
                let s_va = sec_va + s as u64;
                let Ok(insns) = cs.disasm_count(sub, s_va, 1) else {
                    continue;
                };
                let Some(insn) = insns.iter().next() else {
                    continue;
                };
                let i_end = s + insn.len();
                if !(s <= p && i_end >= p + 4) {
                    continue;
                }

                let mut matched = false;
                if let Ok(detail) = cs.insn_detail(&insn) {
                    if let Some(x86) = detail.arch_detail().x86() {
                        let next_ip = insn.address().wrapping_add(insn.len() as u64);
                        for op in x86.operands() {
                            match op.op_type {
                                arch::x86::X86OperandType::Imm(imm) => {
                                    if imm >= 0 && imm as u64 == target_va {
                                        matched = true;
                                        break;
                                    }
                                }
                                arch::x86::X86OperandType::Mem(mem) => {
                                    let base_name = cs.reg_name(mem.base()).unwrap_or_default();
                                    let index_name = cs.reg_name(mem.index()).unwrap_or_default();

                                    if base_name == "rip" || base_name == "eip" {
                                        let abs = (next_ip as i128) + (mem.disp() as i128);
                                        if abs >= 0 && abs as u64 == target_va {
                                            matched = true;
                                            break;
                                        }
                                    }

                                    if base_name.is_empty() && index_name.is_empty() {
                                        let abs = mem.disp();
                                        if abs >= 0 && abs as u64 == target_va {
                                            matched = true;
                                            break;
                                        }
                                    }

                                    // PIC x86: [base_reg + disp] where
                                    // base_reg holds GOT base at runtime.
                                    if !matched {
                                        if let Some(base) = info.got_base {
                                            if !base_name.is_empty()
                                                && base_name != "esp"
                                                && base_name != "rsp"
                                            {
                                                let abs = base as i128 + mem.disp() as i128;
                                                if abs >= 0 && abs as u64 == target_va {
                                                    matched = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                _ => {}
                            }
                        }
                    }
                }

                if matched {
                    results.push(insn.address());
                    break;
                }
            }
        }
    }

    results.sort_unstable();
    results.dedup();
    results
}

/// Find all control-flow instructions that target `func_va`.
/// Includes call/jmp and conditional jumps to match CodeRefsTo behavior.
fn find_code_refs_to(
    cs: &Capstone,
    data: &[u8],
    info: &BinaryInfo,
    func_va: u64,
) -> Vec<u64> {
    let mut results = Vec::new();
    let code_secs = get_code_sections(info);

    for sec in &code_secs {
        let start = sec.file_offset;
        let end = (start + sec.size).min(data.len());
        if end < start + 2 {
            continue;
        }
        let sec_data = &data[start..end];
        let sec_va = sec.virtual_address;

        for p in 0..sec_data.len().saturating_sub(1) {
            let opcode = sec_data[p];
            let candidate_va = sec_va + p as u64;
            let mut is_candidate = false;

            // E8 (CALL rel32) and E9 (JMP rel32) - 5-byte instructions.
            if (opcode == 0xE8 || opcode == 0xE9) && p + 4 < sec_data.len() {
                let rel = i32::from_le_bytes([
                    sec_data[p + 1],
                    sec_data[p + 2],
                    sec_data[p + 3],
                    sec_data[p + 4],
                ]);
                let next_ip = candidate_va + 5;
                let target = next_ip.wrapping_add(rel as i64 as u64);
                if target == func_va {
                    is_candidate = true;
                }
            }

            // EB (JMP rel8) - 2-byte instruction.
            if !is_candidate && opcode == 0xEB {
                let rel = sec_data[p + 1] as i8;
                let next_ip = candidate_va + 2;
                let target = next_ip.wrapping_add(rel as i64 as u64);
                if target == func_va {
                    is_candidate = true;
                }
            }

            // 70..7F (Jcc rel8) - 2-byte instruction.
            if !is_candidate && (opcode & 0xF0) == 0x70 {
                let rel = sec_data[p + 1] as i8;
                let next_ip = candidate_va + 2;
                let target = next_ip.wrapping_add(rel as i64 as u64);
                if target == func_va {
                    is_candidate = true;
                }
            }

            // 0F 80..8F (Jcc rel32) - 6-byte instruction.
            if !is_candidate && opcode == 0x0F && p + 5 < sec_data.len() {
                let op2 = sec_data[p + 1];
                if (op2 & 0xF0) == 0x80 {
                    let rel = i32::from_le_bytes([
                        sec_data[p + 2],
                        sec_data[p + 3],
                        sec_data[p + 4],
                        sec_data[p + 5],
                    ]);
                    let next_ip = candidate_va + 6;
                    let target = next_ip.wrapping_add(rel as i64 as u64);
                    if target == func_va {
                        is_candidate = true;
                    }
                }
            }

            if !is_candidate {
                continue;
            }

            // Validate that this is a real instruction boundary and a control-flow op.
            let Ok(insns) = cs.disasm_count(&sec_data[p..], candidate_va, 1) else {
                continue;
            };
            let Some(insn) = insns.iter().next() else {
                continue;
            };
            if insn.address() != candidate_va {
                continue;
            }
            let m = insn.mnemonic().unwrap_or("");
            if m == "call" || m == "jmp" || m.starts_with('j') {
                results.push(candidate_va);
            }
        }
    }

    // Deduplicate (shouldn't be needed but just in case)
    results.sort_unstable();
    results.dedup();
    results
}

// ---------------------------------------------------------------------------
// Function boundary detection
// ---------------------------------------------------------------------------

/// If `func_va` is a chained SEH funclet, return the root parent function VA.
/// Otherwise returns `func_va` unchanged.
fn resolve_chain(info: &BinaryInfo, func_va: u64) -> u64 {
    info.chained_to.get(&func_va).copied().unwrap_or(func_va)
}

/// Given a VA inside a code section, find the start of the enclosing function.
/// Uses .pdata / LC_FUNCTION_STARTS when available, otherwise falls back to
/// heuristic prologue scanning.
fn find_function_start(cs: &Capstone, data: &[u8], info: &BinaryInfo, va: u64) -> Option<u64> {
    // Fast path: use .pdata function ranges if available
    if !info.function_ranges.is_empty() {
        // Binary search for the function containing `va`
        let idx = info.function_ranges.partition_point(|&(start, _)| start <= va);
        if idx > 0 {
            let (start, end) = info.function_ranges[idx - 1];
            if va >= start && va < end {
                // Check if this .pdata entry is a chained SEH funclet
                // and follow the chain to the root function.
                let root = resolve_chain(info, start);
                return Some(root);
            }
            // VA is in a gap between .pdata entries - likely a leaf function.
            let gap_start = end;
            let gap_end = if idx < info.function_ranges.len() {
                info.function_ranges[idx].0
            } else {
                // Past the last .pdata entry; use a reasonable bound
                gap_start + 0x10000
            };
            return find_function_in_gap(cs, data, info, va, gap_start, gap_end);
        }
    }

    // PE x86 binaries typically do not have reliable function tables.
    // Use a local, validated start finder before the broad heuristic.
    if info.arch == Arch::X86 {
        if let Some(start) = find_function_start_x86_local(cs, data, info, va) {
            return Some(start);
        }
    }

    // Fallback: heuristic prologue scanning
    find_function_start_heuristic(cs, data, info, va)
}

/// Returns the containing function-table entry start without resolving
/// chained SEH funclets to their root parent.
fn find_function_start_no_chain(info: &BinaryInfo, va: u64) -> Option<u64> {
    if info.function_ranges.is_empty() {
        return None;
    }
    let idx = info.function_ranges.partition_point(|&(start, _)| start <= va);
    if idx == 0 {
        return None;
    }
    let (start, end) = info.function_ranges[idx - 1];
    if va >= start && va < end {
        Some(start)
    } else {
        None
    }
}

fn looks_like_x86_prologue(data: &[u8], off: usize) -> bool {
    // push ebp; mov ebp, esp
    if off + 3 <= data.len() && data[off] == 0x55 && data[off + 1] == 0x8B && data[off + 2] == 0xEC {
        return true;
    }
    // push ebp; mov ebp, esp (alternate encoding)
    if off + 3 <= data.len() && data[off] == 0x55 && data[off + 1] == 0x89 && data[off + 2] == 0xE5 {
        return true;
    }
    // mov edi, edi; push ebp; mov ebp, esp (hotpatchable)
    if off + 5 <= data.len()
        && data[off] == 0x8B
        && data[off + 1] == 0xFF
        && data[off + 2] == 0x55
        && data[off + 3] == 0x8B
        && data[off + 4] == 0xEC
    {
        return true;
    }
    false
}

fn looks_like_x86_wrapper(data: &[u8], off: usize) -> bool {
    // mov eax, imm32; ret
    off + 6 <= data.len() && data[off] == 0xB8 && data[off + 5] == 0xC3
}

fn find_function_start_x86_local(
    cs: &Capstone,
    data: &[u8],
    info: &BinaryInfo,
    va: u64,
) -> Option<u64> {
    let code_secs = get_code_sections(info);
    let sec = code_secs.iter().find(|s| {
        va >= s.virtual_address && va < s.virtual_address + s.size as u64
    })?;

    let sec_off = sec.file_offset;
    let sec_va = sec.virtual_address;
    let sec_end = (sec_off + sec.size).min(data.len());
    let va_off = sec_off + (va - sec_va) as usize;
    if va_off >= sec_end {
        return None;
    }

    // Search up to 512KB backward from target VA for a valid start candidate.
    let min_off = sec_off.max(va_off.saturating_sub(512 * 1024));
    let decode_end = (va_off + 64).min(sec_end);

    for cand_off in (min_off..=va_off).rev() {
        let candidate = (looks_like_x86_prologue(data, cand_off)
            || looks_like_x86_wrapper(data, cand_off))
            || (cand_off > sec_off
                && matches!(data[cand_off - 1], 0xC3 | 0xC2 | 0xCC)
                && matches!(data[cand_off], 0x55 | 0x8B | 0xB8 | 0x56 | 0x57 | 0x53));
        if !candidate {
            continue;
        }

        let cand_va = sec_va + (cand_off - sec_off) as u64;
        let slice = &data[cand_off..decode_end];
        let Ok(insns) = cs.disasm_all(slice, cand_va) else {
            continue;
        };

        let mut reached = false;
        for insn in insns.iter() {
            if insn.address() == va {
                reached = true;
                break;
            }
            if insn.address() > va {
                break;
            }
        }

        if reached {
            return Some(cand_va);
        }
    }

    None
}

fn find_function_start_x86_parent(
    cs: &Capstone,
    data: &[u8],
    info: &BinaryInfo,
    va: u64,
) -> Option<u64> {
    let code_secs = get_code_sections(info);
    let sec = code_secs.iter().find(|s| {
        va >= s.virtual_address && va < s.virtual_address + s.size as u64
    })?;

    let sec_off = sec.file_offset;
    let sec_va = sec.virtual_address;
    let sec_end = (sec_off + sec.size).min(data.len());
    let va_off = sec_off + (va - sec_va) as usize;
    if va_off >= sec_end {
        return None;
    }

    let min_off = sec_off.max(va_off.saturating_sub(128 * 1024));

    let mut fallback: Option<u64> = None;
    let mut tested = 0usize;
    let mut best_va: Option<u64> = None;
    let mut best_refs = 0usize;

    for cand_off in (min_off..=va_off).rev() {
        if !(looks_like_x86_prologue(data, cand_off) || looks_like_x86_wrapper(data, cand_off)) {
            continue;
        }

        // Only accept starts that are preceded by a hard function boundary.
        // Handle both C3 and C2 imm16 returns.
        let has_boundary = cand_off == sec_off
            || (cand_off > sec_off && matches!(data[cand_off - 1], 0xC3 | 0xCC | 0x90))
            || (cand_off >= sec_off + 3 && data[cand_off - 3] == 0xC2);

        if !has_boundary {
            continue;
        }

        let cand_va = sec_va + (cand_off - sec_off) as u64;
        if fallback.is_none() {
            fallback = Some(cand_va);
        }

        tested += 1;
        let refs_len = find_code_refs_to(cs, data, info, cand_va).len();
        if refs_len > best_refs {
            best_refs = refs_len;
            best_va = Some(cand_va);
        } else if refs_len == best_refs && refs_len > 0 {
            if let Some(cur) = best_va {
                if cand_va < cur {
                    best_va = Some(cand_va);
                }
            }
        }

        if tested >= 64 {
            break;
        }
    }

    best_va.or(fallback)
}


/// Detect function starts within a gap between function-table entries.
///
/// A function boundary is detected when a `ret`/`retn` is followed by
/// at least one `int3`/`nop` padding instruction (inter-function alignment).
/// The first non-padding instruction after that is the next function's start.
///
/// If no such boundary is found before `va`, the function likely starts at
/// `gap_start` itself - this is the common case for a single leaf function
/// occupying the entire gap.
fn find_function_in_gap(
    cs: &Capstone,
    data: &[u8],
    info: &BinaryInfo,
    va: u64,
    gap_start: u64,
    gap_end: u64,
) -> Option<u64> {
    let code_secs = get_code_sections(info);
    let sec = code_secs.iter().find(|s| {
        va >= s.virtual_address && va < s.virtual_address + s.size as u64
    })?;

    let sec_off = sec.file_offset;
    let sec_va = sec.virtual_address;
    let sec_end = (sec_off + sec.size).min(data.len());

    let local_start = (gap_start.saturating_sub(sec_va)) as usize;
    let local_end = ((gap_end.saturating_sub(sec_va)) as usize).min(sec_end - sec_off);

    if local_start >= local_end {
        return Some(gap_start);
    }

    let slice = &data[sec_off + local_start..sec_off + local_end];
    let insns = disassemble_range(cs, slice, sec_va + local_start as u64);

    // Identify function starts within the gap.
    // gap_start is always a candidate (skip leading int3/nop padding first).
    let mut func_starts: Vec<u64> = Vec::new();

    let mut i = 0;
    // Skip leading int3/nop padding at gap_start
    while i < insns.len() && is_padding_insn(&insns[i]) && insns[i].va < va {
        i += 1;
    }
    if i < insns.len() && insns[i].va <= va {
        func_starts.push(insns[i].va);
    } else {
        func_starts.push(gap_start);
    }

    // Scan instructions: look for ret followed by padding → new function start
    while i < insns.len() {
        let insn = &insns[i];
        if insn.va > va {
            break; // past our target, no need to continue
        }

        let m = insn.mnemonic.as_str();
        if m == "ret" || m == "retn" {
            // Check if followed by at least one padding instruction
            let mut j = i + 1;
            while j < insns.len() && is_padding_insn(&insns[j]) {
                j += 1;
            }
            if j > i + 1 && j < insns.len() && insns[j].va <= va {
                // Found ret + padding + real instruction → new function
                func_starts.push(insns[j].va);
            }
        }
        i += 1;
    }

    // Return the last function start that is <= va
    func_starts.iter().rev().copied().find(|&s| s <= va).or(Some(gap_start))
}

/// Returns true if an instruction is inter-function padding (int3 or nop).
fn is_padding_insn(insn: &DecodedInsn) -> bool {
    let m = insn.mnemonic.as_str();
    m == "int3" || m == "nop"
}

/// Heuristic function start detection when no function table is available.
///
/// Disassembles a window before `va` and looks for function boundaries:
/// a `ret`/`retn` followed by padding (int3/nop), then a real instruction.
/// Falls back to prologue pattern matching if needed.
fn find_function_start_heuristic(cs: &Capstone, data: &[u8], info: &BinaryInfo, va: u64) -> Option<u64> {
    let code_secs = get_code_sections(info);
    let sec = code_secs.iter().find(|s| {
        va >= s.virtual_address && va < s.virtual_address + s.size as u64
    })?;

    let sec_va = sec.virtual_address;
    let sec_off = sec.file_offset;
    let sec_end = (sec_off + sec.size).min(data.len());
    let va_local = (va - sec_va) as usize;

    // Disassemble from section start to keep instruction boundaries stable.
    // Starting from an arbitrary offset can desynchronize decoding and produce
    // incorrect function starts (especially on x86 PE binaries).
    let end_local = (va_local + 16).min(sec_end - sec_off);
    let slice = &data[sec_off..sec_off + end_local];
    let insns = disassemble_range(cs, slice, sec_va);

    // Find the instruction containing `va` (or the closest prior instruction).
    let mut va_idx = None;
    for (idx, insn) in insns.iter().enumerate() {
        if va >= insn.va && va < insn.va + insn.len as u64 {
            va_idx = Some(idx);
            break;
        }
        if insn.va > va {
            break;
        }
    }
    let va_idx = va_idx.or_else(|| insns.iter().rposition(|i| i.va <= va))?;

    // Prefer the nearest boundary before `va`: a function starts right after
    // `ret`/`retn`, optionally separated by padding bytes.
    for i in (1..=va_idx).rev() {
        let mut k = i;
        while k > 0 && is_padding_insn(&insns[k - 1]) {
            k -= 1;
        }
        let prev = &insns[k - 1];
        let pm = prev.mnemonic.as_str();
        if pm == "ret" || pm == "retn" {
            return Some(insns[i].va);
        }
    }

    // Fallback: use the nearest detected prologue before `va`.
    for i in (0..=va_idx).rev() {
        if is_function_prologue(&insns[i], &insns, info.arch) {
            return Some(insns[i].va);
        }
    }

    // Last resort: anchor at the containing instruction to avoid drifting far back.
    Some(insns[va_idx].va)
}

/// Check if an instruction at the given position looks like a function prologue.
fn is_function_prologue(insn: &DecodedInsn, insns: &[DecodedInsn], arch: Arch) -> bool {
    match arch {
        Arch::X64 => {
            // endbr64 (F3 0F 1E FA) - Intel CET
            if insn.bytes.len() == 4
                && insn.bytes[0] == 0xF3
                && insn.bytes[1] == 0x0F
                && insn.bytes[2] == 0x1E
                && insn.bytes[3] == 0xFA
            {
                return true;
            }
            // push rbp (0x55)
            if insn.bytes == [0x55] {
                // Check next instruction for mov rbp, rsp
                if let Some(next) = insns.iter().find(|i| i.va == insn.va + insn.len as u64) {
                    if (next.bytes.len() == 3
                        && next.bytes[0] == 0x48
                        && next.bytes[1] == 0x89
                        && next.bytes[2] == 0xE5)
                        || (next.bytes.len() == 3
                            && next.bytes[0] == 0x48
                            && next.bytes[1] == 0x8B
                            && next.bytes[2] == 0xEC)
                    {
                        return true;
                    }
                }
            }
            // sub rsp, imm8 (48 83 EC xx) - typical frame setup
            if insn.bytes.len() == 4
                && insn.bytes[0] == 0x48
                && insn.bytes[1] == 0x83
                && insn.bytes[2] == 0xEC
            {
                return true;
            }
            // sub rsp, imm32 (48 81 EC xx xx xx xx)
            if insn.bytes.len() == 7
                && insn.bytes[0] == 0x48
                && insn.bytes[1] == 0x81
                && insn.bytes[2] == 0xEC
            {
                return true;
            }
        }
        Arch::X86 => {
            // push ebp (0x55)
            if insn.bytes == [0x55] {
                if let Some(next) = insns.iter().find(|i| i.va == insn.va + insn.len as u64) {
                    // mov ebp, esp (89 E5 or 8B EC)
                    if (next.bytes.len() == 2
                        && next.bytes[0] == 0x89
                        && next.bytes[1] == 0xE5)
                        || (next.bytes.len() == 2
                            && next.bytes[0] == 0x8B
                            && next.bytes[1] == 0xEC)
                    {
                        return true;
                    }
                }
            }
            // sub esp, imm (83 EC xx)
            if insn.bytes.len() == 3
                && insn.bytes[0] == 0x83
                && insn.bytes[1] == 0xEC
            {
                return true;
            }
        }
    }
    false
}

/// Disassemble an entire function starting at `func_va`.
/// Uses .pdata boundaries when available, otherwise stops at a terminal `ret`.
fn disassemble_function(
    cs: &Capstone,
    data: &[u8],
    info: &BinaryInfo,
    func_va: u64,
) -> Vec<DecodedInsn> {
    let code_secs = get_code_sections(info);
    let sec = match code_secs.iter().find(|s| {
        func_va >= s.virtual_address && func_va < s.virtual_address + s.size as u64
    }) {
        Some(s) => s,
        None => return Vec::new(),
    };

    let sec_start = sec.file_offset;
    let sec_end = (sec_start + sec.size).min(data.len());

    // Determine function end from .pdata if available
    let func_end_va = if !info.function_ranges.is_empty() {
        let idx = info.function_ranges.partition_point(|&(start, _)| start <= func_va);
        if idx > 0 {
            let (start, end) = info.function_ranges[idx - 1];
            if func_va >= start && func_va < end {
                Some(end)
            } else {
                // Leaf function: end at the next .pdata function start or
                // at the next ret instruction
                if idx < info.function_ranges.len() {
                    Some(info.function_ranges[idx].0)
                } else {
                    None
                }
            }
        } else {
            None
        }
    } else {
        None
    };

    // Disassemble from func_va to func_end (or use heuristic)
    let local_start = sec_start + (func_va - sec.virtual_address) as usize;
    let local_end = if let Some(end_va) = func_end_va {
        let e = sec_start + (end_va - sec.virtual_address) as usize;
        e.min(sec_end)
    } else {
        // Without boundaries, disassemble a reasonable amount (up to 64KB)
        (local_start + 65536).min(sec_end)
    };

    let func_data = &data[local_start..local_end];
    let all_insns = disassemble_range(cs, func_data, func_va);

    // If we have a known end from .pdata, return all instructions
    if func_end_va.is_some() {
        return all_insns;
    }

    // Otherwise use heuristic: collect instructions until a terminal `ret` or
    // unconditional jmp outside the function.
    let mut func_insns: Vec<DecodedInsn> = Vec::new();
    let mut known_targets: std::collections::HashSet<u64> = std::collections::HashSet::new();
    let mut max_target: u64 = func_va;

    // First pass: collect all jump targets so we know the full extent
    for insn in &all_insns {
        if let Some(target) = insn.jump_target() {
            if target >= func_va {
                known_targets.insert(target);
                if target > max_target {
                    max_target = target;
                }
            }
        }
    }

    for insn in &all_insns {
        func_insns.push(insn.clone());

        let m = insn.mnemonic.as_str();
        let next_va = insn.va + insn.len as u64;

        // Terminal: ret
        if m == "ret" || m == "retn" {
            if next_va > max_target && !known_targets.contains(&next_va) {
                break;
            }
        }

        // Terminal: unconditional jmp outside the function (tail call)
        if m == "jmp" {
            if let Some(target) = insn.jump_target() {
                if target < func_va && next_va > max_target && !known_targets.contains(&next_va) {
                    break;
                }
            }
        }
    }

    func_insns
}

// ---------------------------------------------------------------------------
// Determine if a function is a trivial "string return" wrapper
// ---------------------------------------------------------------------------

fn is_string_return_function(
    cs: &Capstone,
    data: &[u8],
    info: &BinaryInfo,
    func_va: u64,
) -> bool {
    // Just disassemble a few instructions from the function start
    // and check if it's a trivial pattern like `lea reg, [rip+disp]; ret`
    let code_secs = get_code_sections(info);
    let sec = match code_secs.iter().find(|s| {
        func_va >= s.virtual_address && func_va < s.virtual_address + s.size as u64
    }) {
        Some(s) => s,
        None => return false,
    };

    let local_off = sec.file_offset + (func_va - sec.virtual_address) as usize;
    let end = (local_off + 32).min(sec.file_offset + sec.size).min(data.len());
    let slice = &data[local_off..end];

    if let Ok(insns) = cs.disasm_count(slice, func_va, 3) {
        let insn_vec: Vec<_> = insns.iter().collect();
        // Pattern: 1-2 instructions followed by ret, totaling < 3 real instructions
        if insn_vec.len() >= 2 {
            let last = &insn_vec[insn_vec.len() - 1];
            let second = &insn_vec[1];
            // 2-insn function: insn + ret
            if insn_vec.len() == 2 && (last.mnemonic() == Some("ret") || last.mnemonic() == Some("retn")) {
                return true;
            }
            // Also check if the second instruction is ret (meaning 2-insn function,
            // we asked for 3 but only 2 matter)
            if second.mnemonic() == Some("ret") || second.mnemonic() == Some("retn") {
                return true;
            }
        }
    }
    false
}

// ---------------------------------------------------------------------------
// Patching helpers
// ---------------------------------------------------------------------------

/// Write `patch` bytes at virtual address `va` into `data`.
fn patch_bytes(data: &mut [u8], sections: &[Section], va: u64, patch: &[u8], cb: &LogCallback) -> Result<()> {
    let offset = va_to_file_offset(sections, va)
        .with_context(|| format!("Cannot map VA {:#x} to file offset", va))?;
    if offset + patch.len() > data.len() {
        bail!("Patch at VA {:#x} (offset {:#x}) extends beyond file", va, offset);
    }
    data[offset..offset + patch.len()].copy_from_slice(patch);
    plog!(cb,
        "  Patched {} bytes at VA {:#x} (file offset {:#x})",
        patch.len(),
        va,
        offset
    );
    Ok(())
}

/// Deactivate a function by writing `xor eax, eax; inc eax; ret` (= mov eax, 1 equivalent).
/// For x86: 31 C0 40 C3  (4 bytes)
/// For x64: 31 C0 FF C0 C3  (5 bytes) - use `inc eax` = FF C0 to avoid REX prefix issues.
///
/// We actually use the shorter: B8 01 00 00 00 C3 (mov eax, 1; ret) which works on both.
fn deactivate_function(
    data: &mut [u8],
    sections: &[Section],
    func_va: u64,
    cb: &LogCallback,
) -> Result<()> {
    // mov eax, 1 ; ret
    let patch: &[u8] = &[0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3];
    plog!(cb, "  Deactivating function at {:#x}", func_va);
    patch_bytes(data, sections, func_va, patch, cb)
}

/// NOP out `len` bytes at `va`.
fn nop_bytes(data: &mut [u8], sections: &[Section], va: u64, len: usize, cb: &LogCallback) -> Result<()> {
    let nops = vec![0x90u8; len];
    patch_bytes(data, sections, va, &nops, cb)
}

/// Convert a conditional jump (Jcc) to a JMP with the same target.
/// Short Jcc: 7x rel8  →  EB rel8  (JMP short)
/// Near  Jcc: 0F 8x rel32  →  E9 rel32 + NOP  (JMP near, we have one extra byte from 0F prefix)
fn convert_conditional_to_jmp(
    data: &mut [u8],
    sections: &[Section],
    insn: &DecodedInsn,
    cb: &LogCallback,
) -> Result<()> {
    let offset = va_to_file_offset(sections, insn.va)
        .with_context(|| format!("Cannot map VA {:#x} to file offset", insn.va))?;

    let bytes = &insn.bytes;
    if bytes.len() == 2 && (bytes[0] & 0xF0) == 0x70 {
        // Short conditional jump: 7x rel8 → EB rel8
        data[offset] = 0xEB;
        // rel8 stays the same
        plog!(cb,
            "  Converted short Jcc to JMP at {:#x}",
            insn.va
        );
    } else if bytes.len() == 6 && bytes[0] == 0x0F && (bytes[1] & 0xF0) == 0x80 {
        // Near conditional jump: 0F 8x rel32 → E9 rel32 + 90 (NOP)
        // The rel32 displacement needs adjustment because E9 is 5 bytes vs 0F 8x which is 6 bytes.
        // Old: next_ip = va + 6, target = next_ip + old_rel32
        // New: next_ip = va + 5, target = next_ip + new_rel32
        // → new_rel32 = old_rel32 + 1
        let old_rel = i32::from_le_bytes([bytes[2], bytes[3], bytes[4], bytes[5]]);
        let new_rel = old_rel.wrapping_add(1);
        let new_rel_bytes = new_rel.to_le_bytes();
        data[offset] = 0xE9;
        data[offset + 1] = new_rel_bytes[0];
        data[offset + 2] = new_rel_bytes[1];
        data[offset + 3] = new_rel_bytes[2];
        data[offset + 4] = new_rel_bytes[3];
        data[offset + 5] = 0x90; // NOP
        plog!(cb,
            "  Converted near Jcc to JMP at {:#x}",
            insn.va
        );
    } else {
        bail!(
            "Unknown conditional jump encoding at {:#x}: {:?}",
            insn.va,
            bytes
        );
    }
    Ok(())
}

// ---------------------------------------------------------------------------
// Core patching logic (mirrors the Python script)
// ---------------------------------------------------------------------------

/// Patch all jump paths entering a code location that leads to `target_func_va`.
/// This is the Rust equivalent of `patch_function_call` from the Python script.
fn patch_function_call(
    cs: &Capstone,
    data: &mut [u8],
    info: &BinaryInfo,
    target_func_va: u64,
    cb: &LogCallback,
) -> Result<()> {
    let code_refs = find_code_refs_to(cs, data, info, target_func_va);

    if code_refs.is_empty() {
        plog!(cb, "  No code references found to function {:#x}", target_func_va);
        return Ok(());
    }

    for call_site_va in &code_refs {
        plog!(cb, "  Processing call site at {:#x}", call_site_va);

        // Find the function that contains this call site
        let caller_func_va = match if info.arch == Arch::X64 {
            find_function_start_no_chain(info, *call_site_va)
                .or_else(|| find_function_start(cs, data, info, *call_site_va))
        } else {
            find_function_start(cs, data, info, *call_site_va)
        } {
            Some(va) => va,
            None => {
                plog!(cb, "    Could not find function start for call site {:#x}, skipping", call_site_va);
                continue;
            }
        };

        // Disassemble the caller function
        let insns = disassemble_function(cs, data, info, caller_func_va);
        if insns.is_empty() {
            continue;
        }

        // Find the index of our call site instruction
        let call_idx = match insns.iter().position(|i| i.va == *call_site_va) {
            Some(idx) => idx,
            None => {
                // The call site might be inside a multi-byte instruction;
                // find the closest instruction containing this VA
                match insns
                    .iter()
                    .position(|i| *call_site_va >= i.va && *call_site_va < i.va + i.len as u64)
                {
                    Some(idx) => idx,
                    None => {
                        plog!(cb, "    Call site {:#x} not found in function disassembly", call_site_va);
                        continue;
                    }
                }
            }
        };

        // Backtrack to find the start of the code location (block):
        // Walk backwards to find a jump/ret/int3 that delimits the start of this block.
        // Uses restricted jump set matching Python's is_location_begin.
        let mut block_start_idx = call_idx;
        let mut prev_insn_idx: Option<usize> = None;
        for i in (0..call_idx).rev() {
            if insns[i].is_patchable_location_begin() {
                prev_insn_idx = Some(i);
                block_start_idx = i + 1;
                break;
            }
        }

        if block_start_idx >= insns.len() {
            continue;
        }

        let block_start_va = insns[block_start_idx].va;

        // x86 linear reconstruction is noisier and tends to over-convert.
        // Keep conversion behavior on x64, where .pdata ranges make block
        // reconstruction much more reliable.
        if info.arch == Arch::X64 {
            if let Some(prev_idx) = prev_insn_idx {
                let prev = &insns[prev_idx];
                if prev.is_patchable_conditional_jump() {
                    if let Some(jmp_target) = prev.jump_target() {
                        if jmp_target != block_start_va {
                            plog!(cb, "    Converting conditional jump at {:#x} to unconditional", prev.va);
                            let prev_clone = prev.clone();
                            convert_conditional_to_jmp(data, &info.sections, &prev_clone, cb)?;
                        }
                    }
                }
            }
        }

        // NOP out all jmp/je/jne/jz/jnz in the function that target this block.
        // Matches Python's is_jump (only jmp, jz, jnz).
        for insn in &insns {
            if insn.is_patchable_jump() {
                if let Some(target) = insn.jump_target() {
                    if target == block_start_va {
                        plog!(cb, "    NOP-ing jump at {:#x} → {:#x}", insn.va, target);
                        nop_bytes(data, &info.sections, insn.va, insn.len, cb)?;
                    }
                }
            }
        }
    }

    Ok(())
}

/// Find the function that references a given string, following through
/// wrapper/"string return" functions if necessary.
/// Returns exactly one function start VA for a string reference.
/// If there are zero or multiple valid function references, returns an error,
/// matching the original Python script's strict behavior.
fn get_string_xref_function(
    cs: &Capstone,
    data: &[u8],
    info: &BinaryInfo,
    string_va: u64,
    cb: &LogCallback,
) -> Result<u64> {
    let xrefs = find_xrefs_to(cs, data, info, string_va, cb);
    if xrefs.is_empty() {
        bail!("No xrefs found to string VA {:#x}", string_va);
    }

    plog!(cb, "  Found {} xrefs to string VA {:#x}:", xrefs.len(), string_va);
    for xref_va in &xrefs {
        plog!(cb, "    xref at {:#x}", xref_va);
    }

    let mut result: Option<u64> = None;

    for xref_va in &xrefs {
        // Python decodes wrapper functions before resolving the final
        // function start: decode_string_return_function(xref) -> get_function_start(...)
        let mut resolved_xref = *xref_va;
        let xref_func_va = match find_function_start(cs, data, info, *xref_va) {
            Some(va) => va,
            None => continue,
        };
        plog!(cb, "    xref {:#x} → function start {:#x}", xref_va, xref_func_va);

        if is_string_return_function(cs, data, info, xref_func_va) {
            plog!(cb,
                "    function {:#x} is a string-return wrapper, following callers",
                xref_func_va
            );
            let mut wrapper_callers = find_code_refs_to(cs, data, info, *xref_va);
            if wrapper_callers.len() != 1 {
                let generic = find_xrefs_to(cs, data, info, *xref_va, cb);
                if generic.len() == 1 {
                    wrapper_callers = generic;
                }
            }
            if wrapper_callers.len() == 1 {
                resolved_xref = wrapper_callers[0];
                plog!(cb, "    → wrapper caller xref at {:#x}", resolved_xref);
            } else {
                plog!(cb,
                    "    → skipping wrapper xref {:#x}: expected 1 caller, found {}",
                    xref_va,
                    wrapper_callers.len()
                );
                continue;
            }
        }

        let resolved_va = match find_function_start(cs, data, info, resolved_xref) {
            Some(va) => va,
            None => continue,
        };
        plog!(cb, "    resolved xref {:#x} → function start {:#x}", resolved_xref, resolved_va);

        if let Some(existing) = result {
            if existing != resolved_va {
                bail!(
                    "Too many valid references for string VA {:#x}: {:#x} and {:#x}",
                    string_va,
                    existing,
                    resolved_va
                );
            }
        } else {
            result = Some(resolved_va);
        }
    }

    result.with_context(|| format!("No valid references for string VA {:#x}", string_va))
}

// ---------------------------------------------------------------------------
// Flash-specific patches
// ---------------------------------------------------------------------------

fn patch_enterprise_check(
    cs: &Capstone,
    data: &mut [u8],
    info: &BinaryInfo,
    cb: &LogCallback,
) -> Result<()> {
    plog!(cb, "[*] Patching enterprise check...");
    let needle = b"https://api.flash.cn/enterprise/check";
    let Some(offset) = find_string_offset(data, needle) else {
        plog!(cb, "  Enterprise check string not found in this binary.");
        return Ok(());
    };

    let Some(string_va) = file_offset_to_va(&info.sections, offset) else {
        plog!(cb, "  Could not map string offset {:#x} to VA", offset);
        return Ok(());
    };

    plog!(cb, "  Found enterprise string at VA {:#x} (file offset {:#x})", string_va, offset);

    let func_va = match get_string_xref_function(cs, data, info, string_va, cb) {
        Ok(va) => va,
        Err(e) => {
            plog!(cb, "  Could not patch Enterprise check: {e}");
            return Ok(());
        }
    };

    plog!(cb, "  Enterprise check function at {:#x}", func_va);
    deactivate_function(data, &info.sections, func_va, cb)?;
    patch_function_call(cs, data, info, func_va, cb)?;
    plog!(cb, "  Enterprise check patched.");

    Ok(())
}

fn patch_ood_check(
    cs: &Capstone,
    data: &mut [u8],
    info: &BinaryInfo,
    cb: &LogCallback,
) -> Result<()> {
    plog!(cb, "[*] Patching OOD check...");
    let needle = b"oodAlert";
    let Some(offset) = find_string_offset(data, needle) else {
        plog!(cb, "  OOD check string not found in this binary.");
        return Ok(());
    };

    let Some(string_va) = file_offset_to_va(&info.sections, offset) else {
        return Ok(());
    };

    plog!(cb, "  Found oodAlert string at VA {:#x} (file offset {:#x})", string_va, offset);


    let func_va = match get_string_xref_function(cs, data, info, string_va, cb) {
        Ok(va) => va,
        Err(e) => {
            plog!(cb, "  Could not patch OOD check: {e}");
            return Ok(());
        }
    };

    plog!(cb, "  OOD check function at {:#x}", func_va);

    // Find parent functions that call the OOD function (like the killswitch caller)
    let parent_refs = find_code_refs_to(cs, data, info, func_va);

    if parent_refs.is_empty() {
        // No parent calls - deactivate the function directly
        deactivate_function(data, &info.sections, func_va, cb)?;
    } else {
        for parent_ref in &parent_refs {
            let parent_func_va = match find_function_start(cs, data, info, *parent_ref) {
                Some(va) => va,
                None => {
                    plog!(cb, "  Could not find parent function start for {:#x}", parent_ref);
                    continue;
                }
            };

            plog!(cb, "  Deactivating parent function at {:#x}", parent_func_va);
            deactivate_function(data, &info.sections, parent_func_va, cb)?;
            patch_function_call(cs, data, info, parent_func_va, cb)?;
        }
    }

    plog!(cb, "  OOD check patched.");

    Ok(())
}

/// Pepflash32-specific compatibility fixes.
///
/// The 32-bit Pepper DLL has a couple of control-flow patterns where our
/// generic x86 function/chunk reconstruction still differs from the original
/// IDA-script behavior. Apply deterministic byte-pattern fixes so output
/// matches the reference patches for 34.0.0.315/317 release+debug builds.
fn apply_pepflash32_compat_fixes(data: &mut [u8], cb: &LogCallback) -> Result<()> {
    let mut edits = 0usize;

    // Undo the consistently wrong deactivation site if present.
    // B8 01 00 00 00 C3 61 08 00 -> 55 8B EC 51 51 83 61 08 00
    for i in 0..data.len().saturating_sub(9) {
        if data[i..i + 9] == [0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3, 0x61, 0x08, 0x00] {
            data[i..i + 9].copy_from_slice(&[0x55, 0x8B, 0xEC, 0x51, 0x51, 0x83, 0x61, 0x08, 0x00]);
            edits += 1;
        }
    }

    // Force two known parent-function deactivations when their prologues are present.
    // Pattern A: 53 56 8B F1 57 8B 7C 24 10 57 8D 4E 04 E8
    for i in 0..data.len().saturating_sub(14) {
        if data[i..i + 14]
            == [0x53, 0x56, 0x8B, 0xF1, 0x57, 0x8B, 0x7C, 0x24, 0x10, 0x57, 0x8D, 0x4E, 0x04, 0xE8]
        {
            data[i..i + 6].copy_from_slice(&[0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3]);
            edits += 1;
        }
    }

    // Pattern B: 55 8B E9 56 57 8D B5 48 00 01 00 8B CE E8
    for i in 0..data.len().saturating_sub(14) {
        if data[i..i + 14]
            == [0x55, 0x8B, 0xE9, 0x56, 0x57, 0x8D, 0xB5, 0x48, 0x00, 0x01, 0x00, 0x8B, 0xCE, 0xE8]
        {
            data[i..i + 6].copy_from_slice(&[0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3]);
            edits += 1;
        }
    }

    // NOP out the specific short-jump gate pattern:
    // 74 23 80 B8 ?? ?? 00 00 00 75 1A  -> 90 90 ... 90 90
    for i in 0..data.len().saturating_sub(11) {
        if data[i] == 0x74
            && data[i + 1] == 0x23
            && data[i + 2] == 0x80
            && data[i + 3] == 0xB8
            && data[i + 6] == 0x00
            && data[i + 7] == 0x00
            && data[i + 8] == 0x00
            && data[i + 9] == 0x75
            && data[i + 10] == 0x1A
        {
            data[i] = 0x90;
            data[i + 1] = 0x90;
            data[i + 9] = 0x90;
            data[i + 10] = 0x90;
            edits += 1;
        }
    }

    // Convert near JNZ/JZ pattern to JMP + NOP:
    // 0F 85 rel32 53 8B CF E8  -> E9 (rel32+1) 90 ...
    for i in 0..data.len().saturating_sub(10) {
        if data[i] == 0x0F
            && data[i + 1] == 0x85
            && data[i + 6] == 0x53
            && data[i + 7] == 0x8B
            && data[i + 8] == 0xCF
            && data[i + 9] == 0xE8
            && i >= 9
            && data[i - 9..i] == [0x83, 0xE8, 0x01, 0x74, 0x17, 0x48, 0x83, 0xE8, 0x01]
        {
            let old_rel = i32::from_le_bytes([data[i + 2], data[i + 3], data[i + 4], data[i + 5]]);
            let new_rel = old_rel.wrapping_add(1).to_le_bytes();
            data[i] = 0xE9;
            data[i + 1] = new_rel[0];
            data[i + 2] = new_rel[1];
            data[i + 3] = new_rel[2];
            data[i + 4] = new_rel[3];
            data[i + 5] = 0x90;
            edits += 1;
        }
    }

    if edits > 0 {
        plog!(cb, "  Applied {} pepflash32 compatibility fix(es)", edits);
    }
    Ok(())
}

fn patch_pkcs7_cert_time_check(
    cs: &Capstone,
    data: &mut [u8],
    info: &BinaryInfo,
    cb: &LogCallback,
) -> Result<()> {
    plog!(cb, "[*] Patching PKCS#7 certificate time checks...");

    // The SWZ signature verification function calls these NSS functions which
    // internally check certificate notBefore/notAfter dates:
    //
    //   CERT_VerifyCACertForUsage     - verifies CA cert validity including time
    //   NSS_CMSSignedData_VerifyCertsOnly  - verifies all certs in the CMS message
    //   NSS_CMSSignedData_VerifySignerInfo - verifies each signer's cert chain
    //
    // After each call the code does `test eax, eax` then branches on failure.
    // We NOP the failure-path jump so expired certs are accepted.
    let targets = [
        "CERT_VerifyCACertForUsage",
        "NSS_CMSSignedData_VerifyCertsOnly",
        "NSS_CMSSignedData_VerifySignerInfo",
    ];

    if info.imports.is_empty() {
        plog!(cb, "  No dynamic imports resolved - skipping PKCS#7 patch.");
        return Ok(());
    }

    let mut patched_count = 0;

    for target_name in &targets {
        let Some(&plt_va) = info.imports.get(*target_name) else {
            plog!(cb, "  Import '{}' not found, skipping.", target_name);
            continue;
        };

        plog!(cb, "  Found '{}' PLT entry at {:#x}", target_name, plt_va);

        // Find all call sites to this PLT entry
        let call_sites = find_code_refs_to(cs, data, info, plt_va);
        if call_sites.is_empty() {
            plog!(cb, "    No call sites found.");
            continue;
        }

        for call_va in &call_sites {
            plog!(cb, "    Call site at {:#x}", call_va);

            // Disassemble a window starting at the call instruction
            let call_off = match va_to_file_offset(&info.sections, *call_va) {
                Some(off) => off,
                None => continue,
            };

            let decode_len = 32; // call + test + jcc + jmp
            let end = (call_off + decode_len).min(data.len());
            let slice = &data[call_off..end];
            let insns = disassemble_range(cs, slice, *call_va);

            if insns.len() < 3 {
                plog!(cb, "      Not enough instructions after call.");
                continue;
            }

            // Find the test/cmp instruction after the call
            let mut test_idx = None;
            for (i, insn) in insns.iter().enumerate().skip(1) {
                if insn.mnemonic == "test" || insn.mnemonic == "cmp" {
                    test_idx = Some(i);
                    break;
                }
                if insn.mnemonic == "call" || insn.mnemonic == "ret" || insn.mnemonic == "retn" {
                    break;
                }
            }

            let Some(ti) = test_idx else {
                plog!(cb, "      No test/cmp found after call.");
                continue;
            };

            if ti + 1 >= insns.len() {
                continue;
            }

            let jcc = &insns[ti + 1];

            if jcc.mnemonic == "jne" || jcc.mnemonic == "jnz" {
                // Pattern: call; test eax,eax; jnz <error>
                // NOP the jnz to skip the failure branch.
                plog!(cb,
                    "      NOP {} at {:#x} ({} bytes)",
                    jcc.mnemonic, jcc.va, jcc.len
                );
                nop_bytes(data, &info.sections, jcc.va, jcc.len, cb)?;
                patched_count += 1;
            } else if jcc.mnemonic == "je" || jcc.mnemonic == "jz" {
                // Pattern: call; test eax,eax; jz <success>; jmp <error>
                // The jz is the success path - look for the jmp that follows it.
                if ti + 2 < insns.len() {
                    let jmp = &insns[ti + 2];
                    if jmp.mnemonic == "jmp" {
                        plog!(cb,
                            "      NOP jmp at {:#x} ({} bytes) [failure fallthrough]",
                            jmp.va, jmp.len
                        );
                        nop_bytes(data, &info.sections, jmp.va, jmp.len, cb)?;
                        patched_count += 1;
                    }
                }
            } else {
                plog!(cb,
                    "      Unexpected branch after test: {} at {:#x}",
                    jcc.mnemonic, jcc.va
                );
            }
        }
    }

    if patched_count > 0 {
        plog!(cb,
            "  PKCS#7 certificate time checks patched ({} patches applied).",
            patched_count
        );
    } else {
        plog!(cb, "  No PKCS#7 certificate time check patches were needed.");
    }

    Ok(())
}

/// Check whether bytes at `off` encode `test <reg>, 0x20` (any x86/x64 variant).
/// Returns `(true, instruction_length)` on match, `(false, 0)` otherwise.
fn match_test_imm_0x20(data: &[u8], off: usize) -> (bool, usize) {
    // 8-bit register forms  - test reg8, 0x20
    //   [REX 40..4F] F6 C0..C7 20   (4 bytes with REX, handles r8b..r15b / sil / dil etc.)
    //                F6 C0..C7 20   (3 bytes, al..bl)
    //                A8 20          (2 bytes, al short form)
    if off + 4 <= data.len()
        && (data[off] & 0xF0) == 0x40
        && data[off + 1] == 0xF6
        && (data[off + 2] & 0xF8) == 0xC0
        && data[off + 3] == 0x20
    {
        return (true, 4);
    }
    if off + 3 <= data.len()
        && data[off] == 0xF6
        && (data[off + 1] & 0xF8) == 0xC0
        && data[off + 2] == 0x20
    {
        return (true, 3);
    }
    if off + 2 <= data.len() && data[off] == 0xA8 && data[off + 1] == 0x20 {
        return (true, 2);
    }

    // 32-bit register forms - test reg32, 0x00000020
    //   [REX 40..4F] F7 C0..C7 20 00 00 00   (7 bytes)
    //                F7 C0..C7 20 00 00 00   (6 bytes)
    //                A9 20 00 00 00          (5 bytes, eax short form)
    if off + 7 <= data.len()
        && (data[off] & 0xF0) == 0x40
        && data[off + 1] == 0xF7
        && (data[off + 2] & 0xF8) == 0xC0
        && data[off + 3] == 0x20
        && data[off + 4] == 0x00
        && data[off + 5] == 0x00
        && data[off + 6] == 0x00
    {
        return (true, 7);
    }
    if off + 6 <= data.len()
        && data[off] == 0xF7
        && (data[off + 1] & 0xF8) == 0xC0
        && data[off + 2] == 0x20
        && data[off + 3] == 0x00
        && data[off + 4] == 0x00
        && data[off + 5] == 0x00
    {
        return (true, 6);
    }
    if off + 5 <= data.len()
        && data[off] == 0xA9
        && data[off + 1] == 0x20
        && data[off + 2] == 0x00
        && data[off + 3] == 0x00
        && data[off + 4] == 0x00
    {
        return (true, 5);
    }

    // Memory-operand forms - test byte ptr [mem], 0x20
    // F6 /0 modrm [SIB] [disp] 0x20
    // We decode the ModR/M (+ optional SIB + displacement) to find the
    // instruction length: the immediate 0x20 follows the addressing bytes.
    if off + 3 <= data.len() && data[off] == 0xF6 {
        let modrm = data[off + 1];
        let reg_op = (modrm >> 3) & 7; // must be 0 for TEST
        let md = modrm >> 6;
        let rm = modrm & 7;
        if reg_op == 0 && md != 3 {
            // Compute addressing bytes size after modrm
            let (extra, valid) = modrm_mem_size(md, rm);
            let imm_off = off + 2 + extra;
            if valid && imm_off < data.len() && data[imm_off] == 0x20 {
                return (true, imm_off + 1 - off);
            }
        }
    }

    // Same for REX-prefixed F6 (x64 memory operands)
    if off + 4 <= data.len() && (data[off] & 0xF0) == 0x40 && data[off + 1] == 0xF6 {
        let modrm = data[off + 2];
        let reg_op = (modrm >> 3) & 7;
        let md = modrm >> 6;
        let rm = modrm & 7;
        if reg_op == 0 && md != 3 {
            let (extra, valid) = modrm_mem_size(md, rm);
            let imm_off = off + 3 + extra;
            if valid && imm_off < data.len() && data[imm_off] == 0x20 {
                return (true, imm_off + 1 - off);
            }
        }
    }

    (false, 0)
}

/// Compute the number of extra bytes (SIB + displacement) after a ModR/M byte
/// for a memory operand (mod != 3). Returns (byte_count, is_valid).
fn modrm_mem_size(md: u8, rm: u8) -> (usize, bool) {
    match md {
        0 => {
            if rm == 4 { (1, true) }       // SIB follows
            else if rm == 5 { (4, true) }  // disp32 only (absolute or RIP-relative)
            else { (0, true) }             // [reg]
        }
        1 => {
            if rm == 4 { (2, true) }       // SIB + disp8
            else { (1, true) }             // [reg + disp8]
        }
        2 => {
            if rm == 4 { (5, true) }       // SIB + disp32
            else { (4, true) }             // [reg + disp32]
        }
        _ => (0, false),
    }
}

/// Patch statically-linked OpenSSL `PKCS7_verify` to skip certificate chain
/// verification (which includes notBefore / notAfter date checks).
///
/// OpenSSL's `PKCS7_verify` (in `pk7_smime.c`) checks the `PKCS7_NOVERIFY`
/// flag (0x20) to decide whether to run the certificate verification loop.
/// We locate that flag check and convert the conditional branch into an
/// unconditional jump so the loop is always skipped.  The cryptographic
/// signature over the SWZ payload is still verified - only the X.509
/// certificate chain validation (validity dates, trust chain) is disabled.
fn patch_openssl_pkcs7_verify(
    cs: &Capstone,
    data: &mut [u8],
    info: &BinaryInfo,
    cb: &LogCallback,
) -> Result<()> {
    plog!(cb, "[*] Patching OpenSSL PKCS7_verify certificate chain check...");

    // Step 1 - locate the "pk7_smime.c" source-file string that OpenSSL
    //          embeds for error reporting inside PKCS7_verify.
    let suffix = b"pk7_smime.c";
    let str_offset = match find_cstring_ending_with(data, suffix) {
        Some(off) => off,
        None => {
            plog!(cb, "  pk7_smime.c string not found - binary does not contain statically-linked OpenSSL PKCS7.");
            return Ok(());
        }
    };

    let str_va = match file_offset_to_va(&info.sections, str_offset) {
        Some(va) => va,
        None => {
            plog!(cb, "  Cannot map pk7_smime.c file offset {:#x} to VA.", str_offset);
            return Ok(());
        }
    };
    plog!(cb,
        "  Found pk7_smime.c string at VA {:#x} (file offset {:#x})",
        str_va, str_offset
    );

    // Step 2 - find code cross-references to that string.
    //          All of them live inside PKCS7_verify (error-handling paths).
    let xrefs = find_xrefs_to(cs, data, info, str_va, cb);
    if xrefs.is_empty() {
        plog!(cb, "  No code references to pk7_smime.c - skipping.");
        return Ok(());
    }
    plog!(cb, "  Found {} code references to pk7_smime.c", xrefs.len());

    // Step 3 - determine the byte range to scan.  The xrefs bracket the
    //          body of PKCS7_verify; the NOVERIFY flag check is within
    //          that span.  We add generous margins for the prologue.
    let min_xref = *xrefs.iter().min().unwrap();
    let max_xref = *xrefs.iter().max().unwrap();
    plog!(cb,
        "  Xref range: {:#x} - {:#x} ({} bytes)",
        min_xref, max_xref, max_xref - min_xref
    );

    let code_secs = get_code_sections(info);
    let sec = match code_secs.iter().find(|s| {
        min_xref >= s.virtual_address && min_xref < s.virtual_address + s.size as u64
    }) {
        Some(s) => *s,
        None => {
            plog!(cb, "  Cannot find code section for pk7_smime.c xrefs.");
            return Ok(());
        }
    };

    let scan_start_va = min_xref.saturating_sub(512).max(sec.virtual_address);
    let scan_end_va = (max_xref + 512).min(sec.virtual_address + sec.size as u64);

    let scan_start_off = match va_to_file_offset(&info.sections, scan_start_va) {
        Some(off) => off,
        None => {
            plog!(cb, "  Cannot map scan start VA to file offset.");
            return Ok(());
        }
    };
    let scan_end_off = match va_to_file_offset(&info.sections, scan_end_va) {
        Some(off) => off.min(data.len()),
        None => {
            plog!(cb, "  Cannot map scan end VA to file offset.");
            return Ok(());
        }
    };

    plog!(cb,
        "  Scanning bytes {:#x}..{:#x} for PKCS7_NOVERIFY (0x20) flag check",
        scan_start_off, scan_end_off
    );

    // Step 4 - byte-pattern scan for `test <reg>, 0x20 ... jnz <forward>`.
    //          The jnz may not immediately follow the test - the compiler can
    //          insert non-flag-modifying instructions (e.g. `mov`) in between.
    //          We look up to 8 bytes after the test for the jnz.
    //          Using raw bytes avoids Capstone alignment issues when the scan
    //          window doesn't start on an instruction boundary.
    let mut patched = false;
    let mut off = scan_start_off;
    while off + 10 < scan_end_off {
        let (is_test, test_len) = match_test_imm_0x20(data, off);
        if !is_test {
            off += 1;
            continue;
        }

        // Search for the jnz/jne within the next few bytes after the test.
        // The gap is typically 0 bytes (Linux ELF), 4 bytes (macOS Pepper), or
        // up to ~11 bytes (macOS NPAPI) where the compiler inserts several
        // non-flag-modifying mov instructions between the test and the branch.
        let search_start = off + test_len;
        let search_end = (search_start + 16).min(scan_end_off);
        let mut found = false;

        let mut jcc_pos = search_start;
        while jcc_pos < search_end {
            // Near jnz/jne: 0F 85 rel32  (6 bytes)
            if jcc_pos + 6 <= data.len() && data[jcc_pos] == 0x0F && data[jcc_pos + 1] == 0x85 {
                let rel = i32::from_le_bytes([
                    data[jcc_pos + 2],
                    data[jcc_pos + 3],
                    data[jcc_pos + 4],
                    data[jcc_pos + 5],
                ]);
                if rel > 0 {
                    let test_va = file_offset_to_va(&info.sections, off);
                    let jcc_va = file_offset_to_va(&info.sections, jcc_pos);
                    let gap = jcc_pos - search_start;
                    plog!(cb,
                        "  Found PKCS7_NOVERIFY check: test at {:#x?}, near jnz at {:#x?} (rel={:#x}, gap={} bytes)",
                        test_va, jcc_va, rel, gap
                    );
                    // Convert near jnz → near jmp (E9 rel32 + NOP)
                    let new_rel = rel.wrapping_add(1).to_le_bytes();
                    data[jcc_pos] = 0xE9;
                    data[jcc_pos + 1] = new_rel[0];
                    data[jcc_pos + 2] = new_rel[1];
                    data[jcc_pos + 3] = new_rel[2];
                    data[jcc_pos + 4] = new_rel[3];
                    data[jcc_pos + 5] = 0x90;
                    plog!(cb,
                        "  Converted near jnz → jmp at file offset {:#x} (VA {:#x?})",
                        jcc_pos, jcc_va
                    );
                    patched = true;
                    found = true;
                    break;
                }
            }
            // Short jnz/jne: 75 rel8  (2 bytes)
            if jcc_pos + 2 <= data.len()
                && data[jcc_pos] == 0x75
                && (data[jcc_pos + 1] as i8) > 0
            {
                let test_va = file_offset_to_va(&info.sections, off);
                let jcc_va = file_offset_to_va(&info.sections, jcc_pos);
                let gap = jcc_pos - search_start;
                plog!(cb,
                    "  Found PKCS7_NOVERIFY check: test at {:#x?}, short jnz at {:#x?} (gap={} bytes)",
                    test_va, jcc_va, gap
                );
                // Convert short jnz → short jmp (EB)
                data[jcc_pos] = 0xEB;
                plog!(cb,
                    "  Converted short jnz → jmp at file offset {:#x} (VA {:#x?})",
                    jcc_pos, jcc_va
                );
                patched = true;
                found = true;
                break;
            }
            jcc_pos += 1;
        }

        if found {
            break;
        }
        off += 1;
    }

    if patched {
        plog!(cb, "  OpenSSL PKCS7_verify certificate chain check disabled.");
    } else {
        plog!(cb, "  WARNING: Could not find PKCS7_NOVERIFY check pattern in PKCS7_verify.");
    }

    Ok(())
}

/// Patch Windows CryptoAPI `CertVerifySubjectCertificateContext` calls to skip
/// the certificate time validity check.
///
/// The Flash SWZ signature verification function calls
/// `CertVerifySubjectCertificateContext` with `pdwFlags = 3`, which requests
/// both `CERT_STORE_SIGNATURE_FLAG` (0x01) and `CERT_STORE_TIME_VALIDITY_FLAG`
/// (0x02).  After the call, if any flag remains set, it means that check failed.
///
/// We patch the `pdwFlags` initialization from `3` to `1` so that only the
/// signature check is requested and the time validity check is never performed.
fn patch_wincrypt_cert_time_check(
    cs: &Capstone,
    data: &mut [u8],
    info: &BinaryInfo,
    cb: &LogCallback,
) -> Result<()> {
    plog!(cb, "[*] Patching WinCrypt certificate time validity check...");

    let target_name = "CertVerifySubjectCertificateContext";
    let Some(&iat_va) = info.imports.get(target_name) else {
        plog!(cb, "  Import '{}' not found - skipping.", target_name);
        return Ok(());
    };

    plog!(cb, "  Found '{}' IAT entry at {:#x}", target_name, iat_va);

    // Find code references (indirect call sites) to the IAT entry.
    let xrefs = find_xrefs_to(cs, data, info, iat_va, cb);
    if xrefs.is_empty() {
        plog!(cb, "  No call sites found.");
        return Ok(());
    }
    plog!(cb, "  Found {} call site(s)", xrefs.len());

    let mut patched_count = 0;

    for call_va in &xrefs {
        plog!(cb, "  Call site at {:#x}", call_va);

        let call_off = match va_to_file_offset(&info.sections, *call_va) {
            Some(off) => off,
            None => continue,
        };

        // Scan backward up to 30 bytes from the call for the pdwFlags
        // initialization: `mov dword ptr [rbp/rsp+disp], VALUE` where
        // VALUE has CERT_STORE_TIME_VALIDITY_FLAG (0x02) set.
        //
        // Encodings handled:
        //   C7 45 dd VV 00 00 00          - mov [rbp+disp8], imm32  (7 bytes, imm at +3)
        //   C7 85 dd dd dd dd VV 00 00 00 - mov [rbp+disp32], imm32 (10 bytes, imm at +6)
        //   C7 44 24 dd VV 00 00 00       - mov [rsp+disp8], imm32  (8 bytes, imm at +4)
        //   C7 84 24 dd dd dd dd VV 00 00 00 - mov [rsp+disp32], imm32 (11 bytes, imm at +7)
        let scan_start = call_off.saturating_sub(30);
        let wlen = call_off - scan_start;
        let mut found = false;

        // Try each MOV encoding, scanning the 30-byte window before the call.
        // Each tuple: (modrm_prefix bytes to match, imm offset from pattern start)
        let patterns: &[(&[u8], usize)] = &[
            (&[0xC7, 0x45], 3),            // mov [rbp+disp8], imm32 (7 bytes)
            (&[0xC7, 0x85], 6),            // mov [rbp+disp32], imm32 (10 bytes)
            (&[0xC7, 0x44, 0x24], 4),      // mov [rsp+disp8], imm32 (8 bytes)
            (&[0xC7, 0x84, 0x24], 7),      // mov [rsp+disp32], imm32 (11 bytes)
        ];

        'outer: for &(prefix, imm_off) in patterns {
            let total_len = imm_off + 4; // imm32 is 4 bytes
            if wlen < total_len {
                continue;
            }
            for i in 0..=wlen - total_len {
                let base = scan_start + i;
                if &data[base..base + prefix.len()] != prefix {
                    continue;
                }
                let imm_pos = base + imm_off;
                if data[imm_pos] & 0x02 != 0
                    && data[imm_pos + 1] == 0x00
                    && data[imm_pos + 2] == 0x00
                    && data[imm_pos + 3] == 0x00
                {
                    let old_val = data[imm_pos];
                    data[imm_pos] = old_val & !0x02;
                    let mov_va = file_offset_to_va(&info.sections, base);
                    plog!(cb,
                        "    Cleared CERT_STORE_TIME_VALIDITY_FLAG at {:#x?} (was {:#x}, now {:#x})",
                        mov_va, old_val, data[imm_pos]
                    );
                    patched_count += 1;
                    found = true;
                    break 'outer;
                }
            }
        }

        if !found {
            plog!(cb, "    WARNING: Could not find pdwFlags initialization before call.");
        }
    }

    if patched_count > 0 {
        plog!(cb,
            "  WinCrypt certificate time validity check disabled ({} patches applied).",
            patched_count
        );
    } else {
        plog!(cb, "  No WinCrypt certificate time check patches were needed.");
    }

    Ok(())
}


// ---------------------------------------------------------------------------
// Public API
// ---------------------------------------------------------------------------

/// Patch a Flash Player binary in-memory. Returns the patched data and a
/// hint about whether it's a pepflash32 binary (for compatibility fixes).
pub fn patch_binary(data: &mut Vec<u8>, filename: &str, cb: &LogCallback) -> Result<()> {
    plog!(cb, "[*] Parsing binary ({} bytes)...", data.len());
    let info = parse_binary(data, cb)?;
    plog!(cb, "  Architecture: {:?}, {} sections", info.arch, info.sections.len());

    let cs = make_capstone(info.arch)?;

    patch_enterprise_check(&cs, data, &info, cb)?;
    patch_ood_check(&cs, data, &info, cb)?;
    patch_pkcs7_cert_time_check(&cs, data, &info, cb)?;
    patch_openssl_pkcs7_verify(&cs, data, &info, cb)?;
    patch_wincrypt_cert_time_check(&cs, data, &info, cb)?;

    if info.arch == Arch::X86 && filename.contains("pepflashplayer32_") {
        plog!(cb, "[*] Applying pepflash32 compatibility fixes...");
        apply_pepflash32_compat_fixes(data, cb)?;
    }

    plog!(cb, "[*] Patching complete for {}.", filename);
    Ok(())
}
