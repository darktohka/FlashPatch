/// Software renderer operating on a `Vec<u32>` pixel buffer (0x00RRGGBB).
/// All drawing is done in-memory; the buffer is presented via minifb.
pub struct Renderer {
    pub width: usize,
    pub height: usize,
    pub buffer: Vec<u32>,
}

impl Renderer {
    pub fn new(width: usize, height: usize) -> Self {
        Self {
            width,
            height,
            buffer: vec![0; width * height],
        }
    }

    /// Pack r, g, b into the minifb pixel format (0x00RRGGBB).
    #[inline]
    pub const fn rgb(r: u8, g: u8, b: u8) -> u32 {
        ((r as u32) << 16) | ((g as u32) << 8) | (b as u32)
    }

    /// Clear the entire buffer to a single colour.
    pub fn clear(&mut self, color: u32) {
        self.buffer.fill(color);
    }

    /// Set a single pixel (bounds-checked).
    #[inline]
    pub fn set_pixel(&mut self, x: i32, y: i32, color: u32) {
        if x >= 0 && y >= 0 && (x as usize) < self.width && (y as usize) < self.height {
            self.buffer[y as usize * self.width + x as usize] = color;
        }
    }

    /// Alpha-blend a single pixel. `alpha` is 0..=255.
    #[inline]
    pub fn blend_pixel(&mut self, x: i32, y: i32, color: u32, alpha: u8) {
        if x < 0 || y < 0 || (x as usize) >= self.width || (y as usize) >= self.height {
            return;
        }
        let idx = y as usize * self.width + x as usize;
        let dst = self.buffer[idx];
        self.buffer[idx] = alpha_blend(dst, color, alpha);
    }

    /// Fill a solid rectangle.
    pub fn fill_rect(&mut self, x: i32, y: i32, w: i32, h: i32, color: u32) {
        let x0 = x.max(0) as usize;
        let y0 = y.max(0) as usize;
        let x1 = ((x + w) as usize).min(self.width);
        let y1 = ((y + h) as usize).min(self.height);

        for row in y0..y1 {
            let start = row * self.width + x0;
            let end = row * self.width + x1;
            self.buffer[start..end].fill(color);
        }
    }

    /// Draw a 1px rectangle outline.
    pub fn draw_rect(&mut self, x: i32, y: i32, w: i32, h: i32, color: u32) {
        for dx in 0..w {
            self.set_pixel(x + dx, y, color);
            self.set_pixel(x + dx, y + h - 1, color);
        }
        for dy in 0..h {
            self.set_pixel(x, y + dy, color);
            self.set_pixel(x + w - 1, y + dy, color);
        }
    }

    /// Fill a rectangle with a vertical linear gradient from `color1` (top) to `color2` (bottom).
    pub fn fill_gradient_v(&mut self, x: i32, y: i32, w: i32, h: i32, color1: u32, color2: u32) {
        if h <= 0 {
            return;
        }
        let (r1, g1, b1) = unpack(color1);
        let (r2, g2, b2) = unpack(color2);

        for dy in 0..h {
            let t = dy as f32 / (h - 1).max(1) as f32;
            let r = lerp_u8(r1, r2, t);
            let g = lerp_u8(g1, g2, t);
            let b = lerp_u8(b1, b2, t);
            let c = Self::rgb(r, g, b);
            self.fill_rect(x, y + dy, w, 1, c);
        }
    }
}

#[inline]
fn unpack(c: u32) -> (u8, u8, u8) {
    (
        ((c >> 16) & 0xFF) as u8,
        ((c >> 8) & 0xFF) as u8,
        (c & 0xFF) as u8,
    )
}

#[inline]
fn lerp_u8(a: u8, b: u8, t: f32) -> u8 {
    (a as f32 + (b as f32 - a as f32) * t).round() as u8
}

#[inline]
fn alpha_blend(dst: u32, src: u32, alpha: u8) -> u32 {
    let (sr, sg, sb) = unpack(src);
    let (dr, dg, db) = unpack(dst);
    let a = alpha as u16;
    let inv = 255 - a;
    let r = ((sr as u16 * a + dr as u16 * inv) / 255) as u8;
    let g = ((sg as u16 * a + dg as u16 * inv) / 255) as u8;
    let b = ((sb as u16 * a + db as u16 * inv) / 255) as u8;
    Renderer::rgb(r, g, b)
}
