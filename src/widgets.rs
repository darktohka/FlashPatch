use crate::font::FontManager;
use crate::renderer::Renderer;

/// A rectangle region on screen.
#[derive(Clone, Copy, Debug, Default)]
pub struct Rect {
    pub x: i32,
    pub y: i32,
    pub w: i32,
    pub h: i32,
}

impl Rect {
    pub const fn new(x: i32, y: i32, w: i32, h: i32) -> Self {
        Self { x, y, w, h }
    }

    pub fn contains(&self, px: i32, py: i32) -> bool {
        px >= self.x && px < self.x + self.w && py >= self.y && py < self.y + self.h
    }
}

/// A gradient button.
pub struct GradientButton {
    pub rect: Rect,
    pub text: String,
    pub font_size: f32,
    pub color1: u32,
    pub color2: u32,
    pub back_color: u32,
    pub fore_color: u32,
    pub hover_alpha: f64,
    pub disable_alpha: f64,
    pub enabled: bool,
    pub visible: bool,
    pub hovered: bool,
    pub pressed: bool,
}

impl GradientButton {
    pub fn new(x: i32, y: i32, w: i32, h: i32, text: &str) -> Self {
        Self {
            rect: Rect::new(x, y, w, h),
            text: text.to_string(),
            font_size: 16.0,
            color1: Renderer::rgb(118, 118, 118),
            color2: Renderer::rgb(81, 81, 81),
            back_color: Renderer::rgb(0, 0, 0),
            fore_color: Renderer::rgb(227, 227, 227),
            hover_alpha: 0.875,
            disable_alpha: 0.644,
            enabled: true,
            visible: true,
            hovered: false,
            pressed: false,
        }
    }

    pub fn update(&mut self, mx: i32, my: i32, mouse_down: bool) {
        if !self.visible || !self.enabled {
            self.hovered = false;
            self.pressed = false;
            return;
        }
        self.hovered = self.rect.contains(mx, my);
        self.pressed = self.hovered && mouse_down;
    }

    pub fn clicked(&self, mx: i32, my: i32, mouse_released: bool) -> bool {
        self.visible && self.enabled && self.rect.contains(mx, my) && mouse_released
    }

    pub fn draw(&self, renderer: &mut Renderer, fonts: &FontManager) {
        if !self.visible {
            return;
        }

        let (mut c1, mut c2, mut bg, mut fg) =
            (self.color1, self.color2, self.back_color, self.fore_color);

        if !self.enabled {
            c1 = dim_color(c1, self.disable_alpha);
            c2 = dim_color(c2, self.disable_alpha);
            bg = dim_color(bg, self.disable_alpha);
            fg = dim_color(fg, self.disable_alpha);
        } else if self.pressed {
            c1 = dim_color(c1, self.hover_alpha);
            c2 = dim_color(c2, self.hover_alpha);
        } else if !self.hovered {
            c1 = dim_color(c1, self.hover_alpha);
            c2 = dim_color(c2, self.hover_alpha);
        }

        let r = self.rect;
        renderer.fill_gradient_v(r.x, r.y, r.w, r.h, c1, c2);
        renderer.draw_rect(r.x, r.y, r.w, r.h, bg);

        let font_size = self.font_size;
        let (tw, th) = fonts.measure_text(&self.text, font_size);
        let tx = r.x + ((r.w as f32 - tw) / 2.0) as i32;
        let ty = r.y + ((r.h as f32 - th) / 2.0) as i32;

        fonts.draw_text(renderer, tx + 1, ty + 1, &self.text, font_size, bg);
        fonts.draw_text(renderer, tx, ty, &self.text, font_size, fg);
    }
}

fn dim_color(c: u32, alpha: f64) -> u32 {
    let r = (((c >> 16) & 0xFF) as f64 * alpha) as u8;
    let g = (((c >> 8) & 0xFF) as f64 * alpha) as u8;
    let b = ((c & 0xFF) as f64 * alpha) as u8;
    Renderer::rgb(r, g, b)
}

/// Simple label for drawing text.
pub struct Label {
    pub rect: Rect,
    pub text: String,
    pub color: u32,
    pub font_size: f32,
    pub line_spacing: f32,
    pub visible: bool,
    pub max_width: f32,
    pub centered: bool,
}

impl Label {
    pub fn new(x: i32, y: i32, text: &str, font_size: f32) -> Self {
        Self {
            rect: Rect::new(x, y, 0, 0),
            text: text.to_string(),
            color: Renderer::rgb(245, 245, 245),
            font_size,
            line_spacing: 2.0,
            visible: true,
            max_width: 0.0,
            centered: false,
        }
    }

    pub fn draw(&self, renderer: &mut Renderer, fonts: &FontManager) {
        if !self.visible || self.text.is_empty() {
            return;
        }
        if self.centered {
            let (tw, _th) = fonts.measure_text(&self.text, self.font_size);
            let cx = self.rect.x - (tw / 2.0) as i32;
            fonts.draw_text(renderer, cx, self.rect.y, &self.text, self.font_size, self.color);
        } else {
            fonts.draw_text_multiline(
                renderer,
                self.rect.x,
                self.rect.y,
                &self.text,
                self.font_size,
                self.color,
                self.line_spacing,
                self.max_width,
            );
        }
    }

    pub fn clicked(&self, mx: i32, my: i32, mouse_released: bool, fonts: &FontManager) -> bool {
        if !self.visible || !mouse_released {
            return false;
        }
        let (tw, th) = fonts.measure_text_multiline(
            &self.text,
            self.font_size,
            self.line_spacing,
            self.max_width,
        );
        let r = Rect::new(self.rect.x, self.rect.y, tw as i32 + 5, th as i32);
        r.contains(mx, my)
    }
}

/// A display row in the log view (may be a sub-line of a wrapped source line).
struct DisplayLine {
    source_idx: usize,
    text: String,
}

/// A log view that displays scrolling text lines with selectable, word-wrapped text.
pub struct LogView {
    pub rect: Rect,
    pub lines: Vec<String>,
    display_lines: Vec<DisplayLine>,
    pub font_size: f32,
    pub color: u32,
    pub bg_color: u32,
    pub visible: bool,
    pub scroll_offset: usize,
    // Selection state: positions are (display_line_index, char_index).
    sel_anchor: Option<(usize, usize)>,
    sel_cursor: Option<(usize, usize)>,
    selecting: bool,
    sel_highlight: u32,
}

impl LogView {
    pub fn new(x: i32, y: i32, w: i32, h: i32) -> Self {
        Self {
            rect: Rect::new(x, y, w, h),
            lines: Vec::new(),
            display_lines: Vec::new(),
            font_size: 14.0,
            color: Renderer::rgb(200, 200, 200),
            bg_color: Renderer::rgb(15, 15, 15),
            visible: false,
            scroll_offset: 0,
            sel_anchor: None,
            sel_cursor: None,
            selecting: false,
            sel_highlight: Renderer::rgb(51, 102, 178),
        }
    }

    fn wrap_width(&self) -> f32 {
        (self.rect.w - 8) as f32
    }

    pub fn add_line(&mut self, line: String, fonts: &FontManager) {
        let source_idx = self.lines.len();
        let max_w = self.wrap_width();

        if max_w > 0.0 {
            let wrapped = fonts.word_wrap(&line, self.font_size, max_w);
            for wl in wrapped {
                self.display_lines.push(DisplayLine { source_idx, text: wl });
            }
        } else {
            self.display_lines.push(DisplayLine { source_idx, text: line.clone() });
        }

        self.lines.push(line);

        // Auto-scroll to bottom
        let max_lines = self.max_visible_lines();
        if self.display_lines.len() > max_lines {
            self.scroll_offset = self.display_lines.len() - max_lines;
        }
    }

    pub fn scroll_by(&mut self, delta: i32) {
        let max_lines = self.max_visible_lines();
        if self.display_lines.len() <= max_lines {
            self.scroll_offset = 0;
            return;
        }
        let max_offset = self.display_lines.len() - max_lines;
        if delta < 0 {
            self.scroll_offset = self.scroll_offset.saturating_sub((-delta) as usize);
        } else {
            self.scroll_offset = (self.scroll_offset + delta as usize).min(max_offset);
        }
    }

    fn max_visible_lines(&self) -> usize {
        let line_h = self.font_size + 2.0;
        ((self.rect.h as f32) / line_h).floor() as usize
    }

    /// Convert a screen (mx, my) to a (display_line_index, char_index).
    fn hit_test(&self, mx: i32, my: i32, fonts: &FontManager) -> (usize, usize) {
        let r = self.rect;
        let line_h = self.font_size + 2.0;
        let rel_y = (my - r.y - 2).max(0) as f32;
        let vis_line = (rel_y / line_h).floor() as usize;
        let abs_line = (self.scroll_offset + vis_line).min(self.display_lines.len().saturating_sub(1));

        let rel_x = (mx - r.x - 4).max(0) as f32;
        let char_idx = if abs_line < self.display_lines.len() {
            fonts.char_index_at_x(&self.display_lines[abs_line].text, self.font_size, rel_x)
        } else {
            0
        };
        (abs_line, char_idx)
    }

    /// Normalise anchor/cursor so that start <= end.
    fn selection_range(&self) -> Option<((usize, usize), (usize, usize))> {
        let a = self.sel_anchor?;
        let c = self.sel_cursor?;
        if a <= c { Some((a, c)) } else { Some((c, a)) }
    }

    /// Handle mouse events for text selection.  Call once per frame.
    pub fn handle_mouse(
        &mut self,
        mx: i32,
        my: i32,
        mouse_down: bool,
        mouse_released: bool,
        fonts: &FontManager,
    ) {
        if !self.visible {
            return;
        }

        let inside = self.rect.contains(mx, my);

        // Mouse pressed inside – start a new selection
        if mouse_down && inside && !self.selecting {
            let pos = self.hit_test(mx, my, fonts);
            self.sel_anchor = Some(pos);
            self.sel_cursor = Some(pos);
            self.selecting = true;
        }

        // Dragging – update cursor
        if self.selecting && mouse_down {
            let clamped_y = my.max(self.rect.y).min(self.rect.y + self.rect.h - 1);
            let pos = self.hit_test(mx, clamped_y, fonts);
            self.sel_cursor = Some(pos);
        }

        // Mouse released – stop dragging
        if mouse_released && self.selecting {
            self.selecting = false;
        }

        // Click outside clears selection
        if mouse_down && !inside && !self.selecting {
            self.sel_anchor = None;
            self.sel_cursor = None;
        }
    }

    /// Select all text in the log.
    pub fn select_all(&mut self) {
        if self.display_lines.is_empty() {
            return;
        }
        self.sel_anchor = Some((0, 0));
        let last = self.display_lines.len() - 1;
        self.sel_cursor = Some((last, self.display_lines[last].text.chars().count()));
    }

    /// Return the currently selected text (may be empty).
    /// Consecutive display lines from the same source line are joined with a space;
    /// different source lines are separated by newlines.
    pub fn selected_text(&self) -> String {
        let Some((start, end)) = self.selection_range() else {
            return String::new();
        };
        if start == end {
            return String::new();
        }
        let (sl, sc) = start;
        let (el, ec) = end;

        let mut result = String::new();
        let mut prev_source: Option<usize> = None;

        for di in sl..=el {
            if di >= self.display_lines.len() {
                break;
            }
            let dl = &self.display_lines[di];
            let chars: Vec<char> = dl.text.chars().collect();
            let c_start = if di == sl { sc } else { 0 };
            let c_end = if di == el { ec.min(chars.len()) } else { chars.len() };
            let slice: String = chars[c_start..c_end].iter().collect();

            if let Some(prev) = prev_source {
                if prev == dl.source_idx {
                    result.push(' ');
                } else {
                    result.push('\n');
                }
            }
            result.push_str(&slice);
            prev_source = Some(dl.source_idx);
        }
        result
    }

    /// Copy the current selection to the system clipboard.
    pub fn copy_selection(&self) {
        let text = self.selected_text();
        if text.is_empty() {
            return;
        }
        if let Ok(mut clip) = arboard::Clipboard::new() {
            let _ = clip.set_text(text);
        }
    }

    pub fn has_selection(&self) -> bool {
        self.selection_range()
            .map(|(a, b)| a != b)
            .unwrap_or(false)
    }

    pub fn draw(&self, renderer: &mut Renderer, fonts: &FontManager) {
        if !self.visible {
            return;
        }
        let r = self.rect;
        renderer.fill_rect(r.x, r.y, r.w, r.h, self.bg_color);
        renderer.draw_rect(r.x, r.y, r.w, r.h, Renderer::rgb(60, 60, 60));

        let line_h = self.font_size + 2.0;
        let max_lines = self.max_visible_lines();
        let start = self.scroll_offset;
        let end = (start + max_lines).min(self.display_lines.len());

        let sel = self.selection_range();
        let text_x = r.x + 4;

        let mut cy = r.y + 2;
        for i in start..end {
            let line = &self.display_lines[i].text;

            // Draw selection highlight for this line if applicable
            if let Some(((sl, sc), (el, ec))) = sel {
                if i >= sl && i <= el {
                    let chars_len = line.chars().count();
                    let c_start = if i == sl { sc } else { 0 };
                    let c_end = if i == el { ec.min(chars_len) } else { chars_len };

                    if c_start < c_end {
                        let x_start = fonts.measure_text_chars(line, self.font_size, c_start);
                        let x_end = fonts.measure_text_chars(line, self.font_size, c_end);
                        renderer.fill_rect(
                            text_x + x_start as i32,
                            cy,
                            (x_end - x_start).ceil() as i32,
                            line_h as i32,
                            self.sel_highlight,
                        );
                    }
                }
            }

            fonts.draw_text(
                renderer,
                text_x,
                cy,
                line,
                self.font_size,
                self.color,
            );
            cy += line_h as i32;
        }
    }
}
