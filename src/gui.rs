use crate::font::FontManager;
use crate::patcher;
use crate::patcher::LogCallback;
use crate::platform;
use crate::renderer::Renderer;
use crate::widgets;
use crate::widgets::{GradientButton, Label, LogView};

use minifb::{Key, MouseButton, MouseMode, Window, WindowOptions};
use std::sync::{Arc, Mutex};
use std::thread;

const VERSION: &str = "v2.0";
const WINDOW_TITLE: &str = "FlashPatch!";

const WIN_WIDTH: usize = 520;
const WIN_HEIGHT: usize = 390;

const BG_COLOR: u32 = Renderer::rgb(30, 30, 30);
const WHITE: u32 = Renderer::rgb(255, 255, 255);
const LINK_COLOR: u32 = Renderer::rgb(100, 160, 255);

/// Application state shared with the patching thread.
#[derive(Clone, Copy, PartialEq, Eq)]
enum AppState {
    Idle,
    Patching,
    Done,
}

pub fn start_gui() {
    let mut window = Window::new(
        WINDOW_TITLE,
        WIN_WIDTH,
        WIN_HEIGHT,
        WindowOptions {
            resize: false,
            ..WindowOptions::default()
        },
    )
    .expect("Failed to create window");
    window.set_target_fps(30);

    let fonts = FontManager::new();
    let mut renderer = Renderer::new(WIN_WIDTH, WIN_HEIGHT);

    // --- Layout ---
    let center_x = WIN_WIDTH as i32 / 2;

    // Title
    let title_label = Label {
        rect: widgets::Rect::new(center_x, 18, 0, 0),
        text: "FlashPatch!".to_string(),
        color: WHITE,
        font_size: 38.0,
        line_spacing: 0.0,
        visible: true,
        max_width: 0.0,
        centered: true,
    };

    let version_label = Label {
        rect: widgets::Rect::new(center_x + 105, 37, 0, 0),
        text: VERSION.to_string(),
        color: WHITE,
        font_size: 20.0,
        line_spacing: 0.0,
        visible: true,
        max_width: 0.0,
        centered: false,
    };

    // Description
    let desc_label = Label {
        rect: widgets::Rect::new(center_x, 68, 0, 0),
        text: "Play Adobe Flash Player games in the\nbrowser after January 12th, 2021.".to_string(),
        color: Renderer::rgb(200, 200, 200),
        font_size: 16.0,
        line_spacing: 2.0,
        visible: true,
        max_width: 0.0,
        centered: true,
    };

    // Buttons
    let mut patch_btn = GradientButton::new(center_x - 115, 120, 100, 42, "Patch");
    let mut patch_file_btn = GradientButton::new(center_x + 15, 120, 100, 42, "Patch File...");
    patch_file_btn.font_size = 14.0;

    // Link label
    let mut link_label = Label::new(center_x - 85, 172, "by darktohka - GitHub", 14.0);
    link_label.color = LINK_COLOR;

    // Log view (sized for the full window; invisible until patching starts)
    let mut log_view = LogView::new(10, 198, WIN_WIDTH as i32 - 20, WIN_HEIGHT as i32 - 208);

    // Shared state
    let state = Arc::new(Mutex::new(AppState::Idle));
    let log_lines = Arc::new(Mutex::new(Vec::new()));
    let log_lines_cb = Arc::clone(&log_lines);
    let log_cb: LogCallback = Arc::new(move |msg: String| {
        log_lines_cb.lock().unwrap().push(msg);
    });

    let mut prev_mouse_down = false;

    while window.is_open() && !window.is_key_down(Key::Escape) {
        let mouse_pos = window.get_mouse_pos(MouseMode::Clamp).unwrap_or((0.0, 0.0));
        let mx = mouse_pos.0 as i32;
        let my = mouse_pos.1 as i32;
        let mouse_down = window.get_mouse_down(MouseButton::Left);
        let mouse_released = prev_mouse_down && !mouse_down;
        prev_mouse_down = mouse_down;

        let current_state = *state.lock().unwrap();

        // Drain log lines from the patching thread
        {
            let mut lines = log_lines.lock().unwrap();
            for line in lines.drain(..) {
                log_view.add_line(line, &fonts);
            }
        }

        // Show log only once patching starts (i.e. there are log lines)
        log_view.visible = !log_view.lines.is_empty();

        // Handle scroll wheel on log view
        if log_view.visible && log_view.rect.contains(mx, my) {
            if let Some((_sx, sy)) = window.get_scroll_wheel() {
                let lines = if sy > 0.0 { -3 } else if sy < 0.0 { 3 } else { 0 };
                if lines != 0 {
                    log_view.scroll_by(lines);
                }
            }
        }

        // Handle text selection in log view
        log_view.handle_mouse(mx, my, mouse_down, mouse_released, &fonts);

        // Ctrl+C – copy selected text
        let ctrl = window.is_key_down(Key::LeftCtrl) || window.is_key_down(Key::RightCtrl);
        if ctrl && window.is_key_pressed(Key::C, minifb::KeyRepeat::No) {
            log_view.copy_selection();
        }
        // Ctrl+A – select all text in log view
        if ctrl && window.is_key_pressed(Key::A, minifb::KeyRepeat::No) && log_view.visible {
            log_view.select_all();
        }

        // Button states: only disable while an active patch job is running.
        let buttons_enabled = current_state != AppState::Patching;
        patch_btn.enabled = buttons_enabled && cfg!(target_os = "windows");
        patch_file_btn.enabled = buttons_enabled;

        // Update hover/pressed
        patch_btn.update(mx, my, mouse_down);
        patch_file_btn.update(mx, my, mouse_down);

        // Handle clicks
        if patch_btn.clicked(mx, my, mouse_released) {
            start_system_patch(state.clone(), log_cb.clone());
        }

        if patch_file_btn.clicked(mx, my, mouse_released) {
            start_file_patch(state.clone(), log_cb.clone());
        }

        if link_label.clicked(mx, my, mouse_released, &fonts) {
            let _ = open_url("https://github.com/darktohka/FlashPatch");
        }

        // --- Draw ---
        renderer.clear(BG_COLOR);

        title_label.draw(&mut renderer, &fonts);
        version_label.draw(&mut renderer, &fonts);
        desc_label.draw(&mut renderer, &fonts);

        patch_btn.draw(&mut renderer, &fonts);
        patch_file_btn.draw(&mut renderer, &fonts);

        link_label.draw(&mut renderer, &fonts);

        log_view.draw(&mut renderer, &fonts);

        window
            .update_with_buffer(&renderer.buffer, WIN_WIDTH, WIN_HEIGHT)
            .unwrap();
    }
}

/// Open a URL in the default browser.
fn open_url(url: &str) -> std::io::Result<()> {
    #[cfg(target_os = "windows")]
    {
        std::process::Command::new("cmd").args(["/c", "start", url]).spawn()?;
    }
    #[cfg(target_os = "macos")]
    {
        std::process::Command::new("open").arg(url).spawn()?;
    }
    #[cfg(target_os = "linux")]
    {
        std::process::Command::new("xdg-open").arg(url).spawn()?;
    }
    Ok(())
}

/// Start patching all system Flash binaries on a background thread (Windows).
fn start_system_patch(state: Arc<Mutex<AppState>>, log: LogCallback) {
    {
        let mut s = state.lock().unwrap();
        if *s == AppState::Patching {
            return;
        }
        *s = AppState::Patching;
    }

    thread::spawn(move || {
        do_system_patch(&log);
        *state.lock().unwrap() = AppState::Done;
    });
}

fn do_system_patch(log: &LogCallback) {
    push_log(log, "=== Starting system-wide Flash patch ===");

    // Disable telemetry
    {
        let mut telemetry_log = Vec::new();
        platform::disable_telemetry(&mut telemetry_log);
        for line in telemetry_log {
            push_log(log, &line);
        }
    }

    // Disable WoW64 redirection
    let redirection = platform::disable_wow64_redirection();

    // Find Flash binaries
    let binaries = platform::find_flash_binaries();
    if binaries.is_empty() {
        push_log(log, "No Flash Player binaries found on this system.");
        platform::enable_wow64_redirection(redirection);
        return;
    }

    push_log(log, &format!("Found {} Flash Player binary(ies).", binaries.len()));

    let mut patched_count = 0;
    let mut error_count = 0;

    for binary in &binaries {
        push_log(log, "");
        push_log(log, &format!("--- Processing: {} ---", binary.name));
        push_log(log, &format!("  Path: {}", binary.path.display()));

        // Take ownership
        {
            let mut ownership_log = Vec::new();
            platform::take_ownership(&binary.path, &mut ownership_log);
            for line in ownership_log {
                push_log(log, &line);
            }
        }

        // Read binary
        let data = match std::fs::read(&binary.path) {
            Ok(d) => d,
            Err(e) => {
                push_log(log, &format!("  ERROR: Cannot read file: {}", e));
                error_count += 1;
                continue;
            }
        };

        // Backup
        match platform::backup_file(&binary.path) {
            Ok(backup_path) => push_log(log, &format!("  Backup created: {}", backup_path.display())),
            Err(e) => push_log(log, &format!("  WARNING: Could not create backup: {}", e)),
        }

        // Patch
        let mut data = data;
        let filename = binary.path.file_name().unwrap_or_default().to_string_lossy().to_string();
        match patcher::patch_binary(&mut data, &filename, log) {
            Ok(()) => {
                match std::fs::write(&binary.path, &data) {
                    Ok(()) => {
                        push_log(log, "  Successfully wrote patched binary.");
                        patched_count += 1;
                    }
                    Err(e) => {
                        push_log(log, &format!("  ERROR: Cannot write patched file: {}", e));
                        error_count += 1;
                    }
                }
            }
            Err(e) => {
                push_log(log, &format!("  ERROR: Patching failed: {}", e));
                error_count += 1;
            }
        }
    }

    platform::enable_wow64_redirection(redirection);

    push_log(log, "");
    push_log(log, &format!("=== Patching complete: {} patched, {} errors ===", patched_count, error_count));
}

/// Start patching user-selected files on a background thread.
fn start_file_patch(state: Arc<Mutex<AppState>>, log: LogCallback) {
    // Open file dialog on the main thread (required by some platforms)
    let files = rfd::FileDialog::new()
        .set_title("Select Flash Player binaries to patch")
        .add_filter("All Files", &["*"])
        .add_filter("DLL/OCX Files", &["dll", "ocx"])
        .add_filter("Shared Libraries", &["so", "dylib"])
        .pick_files();

    let Some(paths) = files else {
        return;
    };

    if paths.is_empty() {
        return;
    }

    {
        let mut s = state.lock().unwrap();
        if *s == AppState::Patching {
            return;
        }
        *s = AppState::Patching;
    }

    thread::spawn(move || {
        do_file_patch(&paths, &log);
        *state.lock().unwrap() = AppState::Done;
    });
}

fn do_file_patch(paths: &[std::path::PathBuf], log: &LogCallback) {
    push_log(log, &format!("=== Patching {} selected file(s) ===", paths.len()));

    let mut patched_count = 0;
    let mut error_count = 0;

    for path in paths {
        let filename = path.file_name().unwrap_or_default().to_string_lossy().to_string();
        push_log(log, "");
        push_log(log, &format!("--- Processing: {} ---", filename));

        // Take ownership (Windows only, no-op on others)
        {
            let mut ownership_log = Vec::new();
            platform::take_ownership(path, &mut ownership_log);
            for line in ownership_log {
                push_log(log, &line);
            }
        }

        let data = match std::fs::read(path) {
            Ok(d) => d,
            Err(e) => {
                push_log(log, &format!("  ERROR: Cannot read file: {}", e));
                error_count += 1;
                continue;
            }
        };

        // Backup
        match platform::backup_file(path) {
            Ok(backup_path) => push_log(log, &format!("  Backup created: {}", backup_path.display())),
            Err(e) => push_log(log, &format!("  WARNING: Could not create backup: {}", e)),
        }

        // Patch
        let mut data = data;
        match patcher::patch_binary(&mut data, &filename, log) {
            Ok(()) => {
                match std::fs::write(path, &data) {
                    Ok(()) => {
                        push_log(log, "  Successfully wrote patched binary.");
                        patched_count += 1;
                    }
                    Err(e) => {
                        push_log(log, &format!("  ERROR: Cannot write patched file: {}", e));
                        error_count += 1;
                    }
                }
            }
            Err(e) => {
                push_log(log, &format!("  ERROR: Patching failed: {}", e));
                error_count += 1;
            }
        }

        // Preserve permissions on Unix
        #[cfg(unix)]
        {
            use std::os::unix::fs::PermissionsExt;
            if let Ok(metadata) = std::fs::metadata(path) {
                let mode = metadata.permissions().mode();
                if mode & 0o111 != 0 {
                    let _ = std::fs::set_permissions(path, std::fs::Permissions::from_mode(mode));
                }
            }
        }
    }

    push_log(log, "");
    push_log(log, &format!("=== Done: {} patched, {} errors ===", patched_count, error_count));
}

/// Helper to push a log line to the shared log callback.
fn push_log(cb: &LogCallback, msg: &str) {
    cb(msg.to_string());
}
