using Pixie.Internal;
using Silk.NET.OpenGL;
using System.Drawing;
using Font = Pixie.Internal.Font;
using Texture = Pixie.Internal.Texture;

namespace Pixie
{
    public class Renderer
    {
        public long CameraX { get; set; }
        public long CameraY { get; set; }
        public Brightness Brightness { get; set; }
        public uint FrameBufferWidth
        {
            get { return _frameBuffer.Width; }
        }

        public uint FrameBufferHeight
        {
            get { return _frameBuffer.Height; }
        }


        private readonly DataManager _dataManager;
        private readonly long _cameraOriginX;
        private readonly long _cameraOriginY;
        private byte _backgroundColorIndex;

        private Texture _paletteTexture;
        private FrameBuffer _frameBuffer;
        private GLRenderer _glRenderer;

        private const int UniformPaletteSizeLocation = 3;
        private const int UniformBackgroundColorIndexLocation = 4;

        internal Renderer(DataManager dataManager, FrameBuffer frameBuffer, GLRenderer gLRenderer)
        {
            _frameBuffer = frameBuffer;
            _dataManager = dataManager;
            _glRenderer = gLRenderer;

            CameraX = 0;
            CameraY = 0;
            Brightness = Brightness.Normal;
            _paletteTexture = LoadPaletteTexture(dataManager);
            _cameraOriginX = frameBuffer.Width / 2;
            _cameraOriginY = frameBuffer.Height / 2;
            _backgroundColorIndex = 1;


            // Init some uniforms that needs to be set once
            _glRenderer.Begin();
            _glRenderer.SetUniformInt(UniformPaletteSizeLocation, (int)_paletteTexture.Width);
            _glRenderer.End();
        }

        internal void RenderToScreen()
        {
            Texture bufferTexture = _frameBuffer.RenderToTexture(_dataManager);

            //TODO: not needed, right?
            _glRenderer.ClearScreen();

            _glRenderer.Begin();
            _glRenderer.SetUniformInt(UniformBackgroundColorIndexLocation, _backgroundColorIndex);
            _glRenderer.Render(_frameBuffer.FrameBufferQuad, bufferTexture, _paletteTexture);
            _glRenderer.End();
        }

        internal Texture LoadPaletteTexture(in DataManager dataManager)
        {
            IReadOnlyList<Color> palette = Palette.GetPalette();

            List<byte> paletteTextureData = new List<byte>();
            foreach(Color color in palette)
            {
                paletteTextureData.Add(color.R);
                paletteTextureData.Add(color.G);
                paletteTextureData.Add(color.B);
            }

            return dataManager.LoadTexture(paletteTextureData.ToArray(), (uint)palette.Count, 1, ImageMode.RGB, TextureUnit.Texture1);
        }

        public void ClearFrameBuffer()
        {
            _frameBuffer.Clear();
        }

        public void SetBackgroundColor(PixieColor color)
        {
            _backgroundColorIndex = (byte)color;
        }

        public void SetBackgroundColor(PixieColor color, Brightness brightness)
        {
            _backgroundColorIndex = (byte)((byte)color + ((byte)brightness * (byte)PixieColor.Count));
        }

        public void SetBackgroundColor(byte colorIndex)
        {
            _backgroundColorIndex = colorIndex;
        }

        public byte GetBackgroundColor()
        {
            return _backgroundColorIndex;
        }

        public void Point(long x, long y, byte colorIndex)
        {
            long xReal = x - CameraX + _cameraOriginX;
            long yReal = y - CameraY + _cameraOriginY;
            if (xReal < 0 || yReal < 0)
            {
                return;
            }

            _frameBuffer.SetPixel((uint)xReal, (uint)yReal, colorIndex);
        }

        public void Point(long x, long y, PixieColor color)
        {
            Point(x, y, (byte)color);
        }

        public void Point(long x, long y, PixieColor color, Brightness brightness)
        {
            Point(x, y, (byte)((byte)color + ((byte)brightness * (byte)PixieColor.Count)));
        }

        public void Point(long x, long y, PixieColor color, IReadOnlyList<PointLight> lights)
        {
            Brightness brightness = Brightness;
            foreach (PointLight light in lights)
            {
                if (brightness == Brightness.Normal)
                {
                    Point(x, y, color);
                    break;
                }
                Brightness tempBrightness = light.GetBrightness(x, y);
                if (BrightnessUtils.IsLighter(tempBrightness, brightness))
                {
                    brightness = tempBrightness;
                }
            }

            Point(x, y, color, brightness);
        }

        public void Circle(long originX, long originY, uint radius, PixieColor color)
        {
            Circle(originX, originY, radius, (byte)color);
        }

        public void Circle(long originX, long originY, uint radius, PixieColor color, Brightness brightness)
        {
            Circle(originX, originY, radius, (byte)((byte)color + ((byte)brightness * (byte)PixieColor.Count)));
        }

        public void Circle(long originX, long originY, uint radius, byte colorIndex)
        {
            long x = 0;
            long y = radius;
            long p = (5 - (long)radius * 4) / 4;

            CirclePoints(originX, originY, x, y, colorIndex);
            while (x < y)
            {
                x += 1;
                if (p < 0) {
                    p += 2 * x + 1;
                }
                else
                {
                    y -= 1;
                    p += 2 * (x - y) + 1;
                }
                CirclePoints(originX, originY, x, y, colorIndex);
            }
        }

        private void CirclePoints(long cx, long cy, long x, long y, byte colorIndex)
        {
            if (x == 0) {
                Point(cx, cy + y, colorIndex);
                Point(cx, cy - y, colorIndex);
                Point(cx + y, cy, colorIndex);
                Point(cx - y, cy, colorIndex);
            }
            else if (x == y) {
                Point(cx + x, cy + y, colorIndex);
                Point(cx - x, cy + y, colorIndex);
                Point(cx + x, cy - y, colorIndex);
                Point(cx - x, cy - y, colorIndex);
            }
            else if (x < y) {
                Point(cx + x, cy + y, colorIndex);
                Point(cx - x, cy + y, colorIndex);
                Point(cx + x, cy - y, colorIndex);
                Point(cx - x, cy - y, colorIndex);

                Point(cx + y, cy + x, colorIndex);
                Point(cx - y, cy + x, colorIndex);
                Point(cx + y, cy - x, colorIndex);
                Point(cx - y, cy - x, colorIndex);
            }
        }

        public void Line(long x1, long y1, long x2, long y2, PixieColor color)
        {
            Line(x1, x2, y1, y2, (byte)color);
        }

        public void Line(long x1, long y1, long x2, long y2, PixieColor color, Brightness brightness)
        {
            Line(x1, x2, y1, y2, (byte)((byte)color + ((byte)brightness * (byte)PixieColor.Count)));
        }

        public void Line(long x1, long y1, long x2, long y2, byte colorIndex)
        {
            long dx = x2 - x1;
            long dy = y2 - y1;
            long dx1 = Math.Abs(dx);
            long dy1 = Math.Abs(dy);
            long px = 2 * dy1 - dx1;
            long py = 2 * dx1 - dy1;
            if (dy1 <= dx1) {
                long x;
                long y;
                long xe;
                if (dx >= 0) {
                    x = x1;
                    y = y1;
                    xe = x2;
                }
                else
                {
                    x = x2;
                    y = y2;
                    xe = x1;
                }
                Point(x, y, colorIndex);

                while (x < xe) {
                    x += 1;
                    if (px < 0) {
                        px = px + 2 * dy1;
                    }
                    else
                    {
                        if ((dx < 0 && dy < 0) || (dx > 0 && dy > 0)) {
                            y = y + 1;
                        } 
                        else
                        {
                            y = y - 1;
                        }
                        px = px + 2 * (dy1 - dx1);
                    }
                    Point(x, y, colorIndex);
                }
            }
            else
            {
                long x;
                long y;
                long ye;
                if (dy >= 0) {
                    x = x1;
                    y = y1;
                    ye = y2;
                }
                else
                {
                    x = x2;
                    y = y2;
                    ye = y1;
                }
                Point(x, y, colorIndex);
                while (y < ye) {
                    y += 1;
                    if (py <= 0) {
                        py = py + 2 * dx1;
                    }
                    else
                    {
                        if ((dx < 0 && dy < 0) || (dx > 0 && dy > 0)) {
                            x = x + 1;
                        } 
                        else
                        {
                            x = x - 1;
                        }
                        py = py + 2 * (dx1 - dy1);
                    }
                    Point(x, y, colorIndex);
                }
            }
        }

        public void CircleFilled(long originX, long originY, uint radius, PixieColor color)
        {
            CircleFilled(originX, originY, radius, (byte)color);
        }

        public void CircleFilled(long originX, long originY, uint radius, PixieColor color, Brightness brightness)
        {
            CircleFilled(originX, originY, radius, (byte)((byte)color + ((byte)brightness * (byte)PixieColor.Count)));
        }

        public void CircleFilled(long originX, long originY, uint radius, byte colorIndex)
        {
            long x = 0;
            long y = radius;
            long p = (5 - (long)radius * 4) / 4;

            CirclePointsFilled(originX, originY, x, y, colorIndex);
            while (x < y)
            {
                x += 1;
                if (p < 0)
                {
                    p += 2 * x + 1;
                }
                else
                {
                    y -= 1;
                    p += 2 * (x - y) + 1;
                }
                CirclePointsFilled(originX, originY, x, y, colorIndex);
            }
        }

        private void CirclePointsFilled(long cx, long cy, long x, long y, byte colorIndex)
        {
            if (x == 0)
            {
                Line(cx, cy + y, cx, cy - y, colorIndex);
                Line(cx + y, cy, cx - y, cy, colorIndex);
            }
            else if (x == y)
            {
                Line(cx + x, cy + y, cx - x, cy + y, colorIndex);
                Line(cx + x, cy - y, cx - x, cy - y, colorIndex);
            }
            else if (x < y)
            {
                Line(cx + x, cy + y, cx - x, cy + y, colorIndex);
                Line(cx + x, cy - y, cx - x, cy - y, colorIndex);

                Line(cx + y, cy + x, cx - y, cy + x, colorIndex);
                Line(cx + y, cy - x, cx - y, cy - x, colorIndex);
            }
        }

        public void Rectangle(long x1, long y1, long x2, long y2, PixieColor color, Brightness brightness)
        {
            Rectangle(x1, y1, x2, y2, (byte)((byte)color + ((byte)brightness * (byte)PixieColor.Count)));
        }

        public void Rectangle(long x1, long y1, long x2, long y2, PixieColor color)
        {
            Rectangle(x1, y1, x2, y2, (byte)color);
        }

        public void Rectangle(long x1, long y1, long x2, long y2, byte colorIndex)
        {
            Line(x1, y1, x2, y1, colorIndex);
            Line(x1, y1, x1, y2, colorIndex);
            Line(x1, y2, x2, y2, colorIndex);
            Line(x2, y1, x2, y2, colorIndex);
        }

        public void RectangleFilled(long x1, long y1, long x2, long y2, PixieColor color, Brightness brightness)
        {
            RectangleFilled(x1, y1, x2, y2, (byte)((byte)color + ((byte)brightness * (byte)PixieColor.Count)));
        }

        public void RectangleFilled(long x1, long y1, long x2, long y2, PixieColor color)
        {
            RectangleFilled(x1, y1, x2, y2, (byte)color);
        }

        public void RectangleFilled(long x1, long y1, long x2, long y2, byte colorIndex)
        {
            long yMin = Math.Min(y1, y2);
            long yMax = Math.Max(y1, y2);

            for (long y = yMin; y <= yMax; ++y)
            {
                Line(x1, y, x2, y, colorIndex);
            }
        }

        public void Sprite(in Sprite sprite, long x, long y, bool flip)
        {
            if (sprite is null)
            {
                throw new ArgumentNullException(nameof(sprite));
            }

            uint spriteWidth = sprite.Width;
            uint spriteHeight = sprite.Height;
            for (uint spriteX = 0; spriteX < spriteWidth; ++spriteX)
            {
                for (uint spriteY = 0; spriteY < spriteHeight; ++spriteY)
                {
                    PixieColor color = sprite.ColorAt(spriteX, spriteY);
                    if (color == PixieColor.None)
                    {
                        continue;
                    }

                    if (flip)
                    {
                        Point(x + (spriteWidth - 1 - spriteX), y + spriteY, color);
                    }
                    else
                    {
                        Point(x + spriteX, y + spriteY, color);
                    }
                }
            }
        }

        public void Sprite(in Sprite sprite, long x, long y, bool flip, Brightness brightness)
        {
            uint spriteWidth = sprite.Width;
            uint spriteHeight = sprite.Height;
            for (uint spriteX = 0; spriteX < spriteWidth; ++spriteX)
            {
                for (uint spriteY = 0; spriteY < spriteHeight; ++spriteY)
                {
                    PixieColor color = sprite.ColorAt(spriteX, spriteY);
                    if (color == PixieColor.None)
                    {
                        continue;
                    }

                    if (flip)
                    {
                        Point(x + (spriteWidth - 1 - spriteX), y + spriteY, color, brightness);
                    }
                    else
                    {
                        Point(x + spriteX, y + spriteY, color, brightness);
                    }
                }
            }
        }

        public void Sprite(in Sprite sprite, long x, long y, bool flip, IReadOnlyList<PointLight> lights)
        {
            uint spriteWidth = sprite.Width;
            uint spriteHeight = sprite.Height;
            for (uint spriteX = 0; spriteX < spriteWidth; ++spriteX)
            {
                for (uint spriteY = 0; spriteY < spriteHeight; ++spriteY)
                {
                    PixieColor color = sprite.ColorAt(spriteX, spriteY);
                    if (color == PixieColor.None)
                    {
                        continue;
                    }

                    if (flip)
                    {
                        Point(x + (spriteWidth - 1 - spriteX), y + spriteY, color, lights);
                    }
                    else
                    {
                        Point(x + spriteX, y + spriteY, color, lights);
                    }
                }
            }
        }

        public void Text(string text, long x, long y, PixieColor color, Brightness brightness)
        {
            Text(text, x, y, (byte)((byte)color + ((byte)brightness * (byte)PixieColor.Count)));
        }

        public void Text(string text, long x, long y, PixieColor color)
        {
            Text(text, x, y, (byte)color);
        }

        public void Text(string text, long x, long y, byte colorIndex)
        {
            long xOffset = x;
            long yOffset = y;
            Font font = Font.GetFont();
            uint glyphWidth = font.GetGlyphWidth();
            uint glyphHeight = font.GetGlyphHeight();

            foreach (char ch in text)
            {
                if (ch == ' ')
                {
                    xOffset += glyphWidth + 1;
                    continue;
                }
                else if (ch == '\n')
                {
                    xOffset = x;
                    yOffset -= glyphHeight + 1;
                    continue;
                }

                Sprite glyph = font.GetGlyph(ch);

                for (uint glyphX = 0; glyphX < glyphWidth; ++glyphX)
                {
                    for (uint glyphY = 0; glyphY < glyphHeight; ++glyphY)
                    {
                        if (glyph.ColorAt(glyphX, glyphY) == PixieColor.None)
                        {
                            continue;
                        }
                        Point(xOffset + glyphX, yOffset + glyphY, colorIndex);
                    }
                }
                xOffset += glyphWidth + 1;
            }
        }

    }
}
