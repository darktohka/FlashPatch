use ab_glyph::{point, Font, FontRef, ScaleFont};
use crate::renderer::Renderer;

/// Manages loaded fonts and provides text measurement / rendering.
pub struct FontManager {
    regular: FontRef<'static>,
}

impl FontManager {
    /// Create a `FontManager` using the bundled Liberation Sans font.
    pub fn new() -> Self {
        let regular = FontRef::try_from_slice(include_bytes!(
            "../resources/liberation-sans.regular.ttf"
        ))
        .expect("Failed to parse bundled Liberation Sans font");
        Self { regular }
    }

    /// Measure the width (in pixels) of `text` at the given `size` (in px).
    pub fn measure_text(&self, text: &str, size: f32) -> (f32, f32) {
        let scaled = self.regular.as_scaled(size);
        let mut width: f32 = 0.0;
        let height = scaled.height();

        let mut last_glyph_id = None;

        for ch in text.chars() {
            let glyph_id = scaled.glyph_id(ch);
            if let Some(prev) = last_glyph_id {
                width += scaled.kern(prev, glyph_id);
            }
            width += scaled.h_advance(glyph_id);
            last_glyph_id = Some(glyph_id);
        }

        (width, height)
    }

    /// Draw `text` onto the renderer at (x, y) with the given pixel size and colour.
    pub fn draw_text(
        &self,
        renderer: &mut Renderer,
        x: i32,
        y: i32,
        text: &str,
        size: f32,
        color: u32,
    ) {
        let scaled = self.regular.as_scaled(size);
        let ascent = scaled.ascent();
        let mut cursor_x: f32 = 0.0;
        let mut last_glyph_id = None;

        for ch in text.chars() {
            let glyph_id = scaled.glyph_id(ch);
            if let Some(prev) = last_glyph_id {
                cursor_x += scaled.kern(prev, glyph_id);
            }

            let glyph = glyph_id.with_scale_and_position(
                size,
                point(x as f32 + cursor_x, y as f32 + ascent),
            );

            if let Some(outlined) = self.regular.outline_glyph(glyph) {
                let bounds = outlined.px_bounds();
                outlined.draw(|gx, gy, coverage| {
                    let px = bounds.min.x as i32 + gx as i32;
                    let py = bounds.min.y as i32 + gy as i32;
                    let alpha = (coverage * 255.0) as u8;
                    if alpha > 0 {
                        renderer.blend_pixel(px, py, color, alpha);
                    }
                });
            }

            cursor_x += scaled.h_advance(glyph_id);
            last_glyph_id = Some(glyph_id);
        }
    }

    /// Draw multiline text, splitting on '\n' and word-wrapping at `max_width`.
    pub fn draw_text_multiline(
        &self,
        renderer: &mut Renderer,
        x: i32,
        y: i32,
        text: &str,
        size: f32,
        color: u32,
        line_spacing: f32,
        max_width: f32,
    ) -> f32 {
        let scaled = self.regular.as_scaled(size);
        let line_height = scaled.height() + line_spacing;
        let mut cy = y as f32;

        for line in text.split('\n') {
            if max_width > 0.0 {
                let wrapped = self.word_wrap(line, size, max_width);
                for wl in &wrapped {
                    self.draw_text(renderer, x, cy as i32, wl, size, color);
                    cy += line_height;
                }
            } else {
                self.draw_text(renderer, x, cy as i32, line, size, color);
                cy += line_height;
            }
        }

        cy - y as f32
    }

    /// Measure the total height of multiline text with word-wrapping.
    pub fn measure_text_multiline(
        &self,
        text: &str,
        size: f32,
        line_spacing: f32,
        max_width: f32,
    ) -> (f32, f32) {
        let scaled = self.regular.as_scaled(size);
        let line_height = scaled.height() + line_spacing;
        let mut total_lines = 0usize;
        let mut overall_max_w: f32 = 0.0;

        for line in text.split('\n') {
            if max_width > 0.0 {
                let wrapped = self.word_wrap(line, size, max_width);
                for wl in &wrapped {
                    let (lw, _) = self.measure_text(wl, size);
                    if lw > overall_max_w {
                        overall_max_w = lw;
                    }
                }
                total_lines += wrapped.len();
            } else {
                let (lw, _) = self.measure_text(line, size);
                if lw > overall_max_w {
                    overall_max_w = lw;
                }
                total_lines += 1;
            }
        }

        (overall_max_w, line_height * total_lines as f32)
    }

    pub fn word_wrap(&self, line: &str, size: f32, max_width: f32) -> Vec<String> {
        if line.is_empty() {
            return vec![String::new()];
        }

        let mut result = Vec::new();
        let mut current = String::new();
        let mut current_w: f32 = 0.0;
        let space_w = self.measure_text(" ", size).0;

        for word in line.split_whitespace() {
            let (ww, _) = self.measure_text(word, size);

            if current.is_empty() {
                current = word.to_string();
                current_w = ww;
            } else if current_w + space_w + ww <= max_width {
                current.push(' ');
                current.push_str(word);
                current_w += space_w + ww;
            } else {
                result.push(current);
                current = word.to_string();
                current_w = ww;
            }
        }

        result.push(current);
        result
    }

    /// Return the character index (0-based) whose position is closest to `target_x`
    /// pixels from the start of `text` rendered at `size`.
    pub fn char_index_at_x(&self, text: &str, size: f32, target_x: f32) -> usize {
        let scaled = self.regular.as_scaled(size);
        let mut x: f32 = 0.0;
        let mut last_glyph_id = None;

        for (i, ch) in text.chars().enumerate() {
            let glyph_id = scaled.glyph_id(ch);
            if let Some(prev) = last_glyph_id {
                x += scaled.kern(prev, glyph_id);
            }
            let advance = scaled.h_advance(glyph_id);
            if target_x < x + advance / 2.0 {
                return i;
            }
            x += advance;
            last_glyph_id = Some(glyph_id);
        }

        text.chars().count()
    }

    /// Measure the pixel width of the first `char_count` characters of `text`.
    pub fn measure_text_chars(&self, text: &str, size: f32, char_count: usize) -> f32 {
        let scaled = self.regular.as_scaled(size);
        let mut width: f32 = 0.0;
        let mut last_glyph_id = None;

        for (i, ch) in text.chars().enumerate() {
            if i >= char_count {
                break;
            }
            let glyph_id = scaled.glyph_id(ch);
            if let Some(prev) = last_glyph_id {
                width += scaled.kern(prev, glyph_id);
            }
            width += scaled.h_advance(glyph_id);
            last_glyph_id = Some(glyph_id);
        }

        width
    }
}
