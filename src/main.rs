mod font;
mod patcher;
mod platform;
mod renderer;
mod widgets;
mod gui;

use anyhow::{Context, Result};
use std::env;
use std::fs;
use std::sync::Arc;

fn main() -> Result<()> {
    let args: Vec<String> = env::args().collect();

    if args.len() == 1 {
        // No arguments: launch the GUI.
        gui::start_gui();
        return Ok(());
    }

    if args[1] == "--help" || args[1] == "-h" || args.len() > 3 {
        eprintln!("Usage: {} <binary> [output]", args[0]);
        eprintln!("  Patches Flash enterprise, OOD, and PKCS#7 cert time checks in the given binary.");
        eprintln!("  Supports: ELF64 (Linux), MachO64 (macOS), PE32/PE64 (Windows DLL).");
        std::process::exit(1);
    }

    let input_path = &args[1];
    let output_path = if args.len() >= 3 {
        args[2].clone()
    } else {
        format!("{}.patched", input_path)
    };

    println!("[*] Reading binary: {}", input_path);
    let mut data = fs::read(input_path)
        .with_context(|| format!("Failed to read input file: {}", input_path))?;

    let filename = std::path::Path::new(input_path)
        .file_name()
        .and_then(|v| v.to_str())
        .unwrap_or(input_path)
        .to_string();

    let log_cb: patcher::LogCallback = Arc::new(|msg: String| {
        println!("{}", msg);
    });

    patcher::patch_binary(&mut data, &filename, &log_cb)?;

    println!("[*] Writing patched binary to: {}", output_path);
    fs::write(&output_path, &data)
        .with_context(|| format!("Failed to write output file: {}", output_path))?;

    // Preserve executable permissions on Unix
    #[cfg(unix)]
    {
        let perms = fs::metadata(input_path)?.permissions();
        fs::set_permissions(&output_path, perms)?;
    }

    println!("[*] Done!");
    Ok(())
}