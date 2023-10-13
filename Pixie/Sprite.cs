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
using System.Drawing;

namespace Pixie
{
    public class Sprite : IReadOnlySprite
    {
        public uint Width { get; }
        public uint Height { get; }

        private List<List<byte>> _pixels;

        public Sprite(uint width, uint height)
        {
            this.Width = width;
            this.Height = height;

            List<List<byte>> pixels = new List<List<byte>>();
            for (long x = 0; x < Width; ++x)
            {
                List<byte> column = new List<byte>();
                for (long y = Height - 1; y >= 0; --y)
                {
                    column.Add((byte)PixieColor.None);
                }
                pixels.Add(column);
            }
            this._pixels = pixels;
        }

        public Sprite(uint width, uint height, List<List<byte>> pixels) 
        { 
            this.Width = width;
            this.Height = height;
            this._pixels = pixels;
        }

        public static Sprite FromBitmapPNG(in Bitmap bitmap)
        {
            List<List<byte>> pixels = new List<List<byte>>();


            for (long x = 0; x < bitmap.Width; ++x)
            {
                List<byte> column = new List<byte>();
                for (long y = bitmap.Height - 1; y >= 0; --y)
                {
                    Color rgbColor = bitmap.GetPixel((int)x, (int)y);
                    column.Add(Palette.ColorFromRgb(rgbColor));
                }
                pixels.Add(column);
            }

            return new Sprite((uint)bitmap.Width, (uint)bitmap.Height, pixels);
        }

        public byte GetColorAt(uint x, uint y)
        {
            if (x >= Width || y >= Height) {
                return (byte)PixieColor.None;
            }

            return _pixels[(int)x][(int)y];
        }

        public void SetColorAt(uint x, uint y, byte color)
        {
            if (x >= Width || y >= Height)
            {
                return;
            }

            _pixels[(int)x][(int)y] = color;
        }

        public void SetColorAt(uint x, uint y, PixieColor color)
        {
            SetColorAt(x, y, (byte)color);
        }
    }
}
