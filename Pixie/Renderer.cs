//
// Copyright © 2023  Egidijus Lileika
//
// This file is part of Pixie - Framework for 2D game development
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
//

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
            _paletteTexture = LoadPaletteTexture(dataManager);
            _cameraOriginX = frameBuffer.Width / 2;
            _cameraOriginY = frameBuffer.Height / 2;
            _backgroundColorIndex = 1;


            // Init some uniforms that needs to be set once
            _glRenderer.Begin();
            _glRenderer.SetUniformInt(UniformPaletteSizeLocation, Palette.ColorPalette.Count);
            _glRenderer.End();
        }

        internal void RenderToScreen()
        {
            Texture bufferTexture = _frameBuffer.RenderToTexture(_dataManager);

            _glRenderer.Begin();
            _glRenderer.SetUniformInt(UniformBackgroundColorIndexLocation, _backgroundColorIndex);
            _glRenderer.Render(_frameBuffer.FrameBufferQuad, bufferTexture, _paletteTexture);
            _glRenderer.End();
        }

        internal Texture LoadPaletteTexture(in DataManager dataManager)
        {
            List<Color> palette = new List<Color>(Palette.ColorPalette);
            if (palette.Count > Palette.MaxColors)
            {
                throw new Exception("Palette exceeding color maximum");
            }

            // Pad palette with transparent colors
            int padColors = Palette.MaxColors - palette.Count;
            for (int idx = 0; idx < padColors; ++idx)
            {
                palette.Add(Color.FromArgb(0, 0, 0, 0));
            }

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

        public void Circle(long originX, long originY, uint radius, PixieColor color)
        {
            Circle(originX, originY, radius, (byte)color);
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
            Line(x1, y1, x2, y2, (byte)color);
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

        public void Sprite(in IReadOnlySprite? sprite, long x, long y, bool flip)
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
                    byte color = sprite.GetColorAt(spriteX, spriteY);
                    if (color == (byte)PixieColor.None)
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

        public void Sprite(in IReadOnlySprite? sprite, long x, long y, float angle)
        {
            if (sprite is null)
            {
                throw new ArgumentNullException(nameof(sprite));
            }

            float angleRadians = MathF.PI * angle / 180.0f;

            float cosTheta = MathF.Cos(angleRadians);
            float sinTheta = MathF.Sin(angleRadians);

            uint spriteWidth = sprite.Width;
            uint spriteHeight = sprite.Height;

            long newWidth = (long)(Math.Abs(cosTheta) * spriteWidth + Math.Abs(sinTheta) * spriteHeight);
            long newHeight = (long)(Math.Abs(cosTheta) * spriteHeight + Math.Abs(sinTheta) * spriteWidth);

            long widthOffset = (newWidth - spriteWidth) / 2;
            long heightOffset = (newHeight - spriteHeight) / 2;

            for (long spriteX = 0; spriteX < newWidth; ++spriteX)
            {
                for (long spriteY = 0; spriteY < newHeight; ++spriteY)
                {
                    long sampleX = (long)((spriteX - newWidth / 2) * cosTheta + (spriteY - newHeight / 2) * sinTheta + spriteWidth / 2);
                    long sampleY = (long)((-spriteX + newWidth / 2) * sinTheta + (spriteY - newHeight / 2) * cosTheta + spriteHeight / 2);

                    byte color = sprite.GetColorAt((uint)sampleX, (uint)sampleY);
                    if (color == (byte)PixieColor.None)
                    {
                        continue;
                    }

                    Point(x - widthOffset + spriteX, y - heightOffset + spriteY, color);
                }
            }
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
                        if (glyph.GetColorAt(glyphX, glyphY) == (byte)PixieColor.None)
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
