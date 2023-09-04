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

using System.Drawing;

namespace Pixie
{
    public class SpriteSheet
    {
        public uint Size 
        {
            get { return (uint)_sprites.Count; }
        }

        private readonly IReadOnlyList<Sprite> _sprites;

        internal SpriteSheet(IReadOnlyList<Sprite> sprites)
        {
            _sprites = sprites;
        }

        public static SpriteSheet FromBitmapPNG(in Bitmap bitmap, uint spriteWidth, uint spriteHeight)
        {
            Sprite spriteSheetRaw = Sprite.FromBitmapPNG(bitmap);

            if (spriteSheetRaw.Width % spriteWidth != 0)
            {
                throw new ArgumentException(string.Format("Sprite sheet width is not multiple of {0}", spriteWidth));
            }
            if (spriteSheetRaw.Height % spriteHeight != 0)
            {
                throw new ArgumentException(string.Format("Sprite sheet height is not multiple of {0}", spriteHeight));
            }

            if (spriteSheetRaw.Width < spriteWidth)
            {
                throw new ArgumentException(string.Format("Sprite sheet width {0} is smaller than sprite size {1}", spriteSheetRaw.Width, spriteWidth));
            }
            if (spriteSheetRaw.Height < spriteHeight)
            {
                throw new ArgumentException(string.Format("Sprite sheet height {0} is smaller than sprite size {1}", spriteSheetRaw.Height, spriteHeight));
            }

            uint columns = spriteSheetRaw.Width / spriteWidth;
            uint rows = spriteSheetRaw.Height / spriteHeight;

            List<Sprite> sprites = new List<Sprite>();

            for (int row = (int)rows - 1; row >= 0; --row)
            {
                for (int column = 0; column < columns; ++column)
                {
                    uint spriteOffsetX = (uint)column * spriteWidth;
                    uint spriteOffsetY = (uint)row * spriteHeight;

                    List<List<byte>> pixels = new List<List<byte>>();
                    for (uint x = 0; x < spriteWidth; ++x)
                    {
                        List<byte> pixelColumn = new List<byte>();
                        for (uint y = 0; y < spriteHeight; ++y)
                        {
                            byte pixel = spriteSheetRaw.GetColorAt(spriteOffsetX + x, spriteOffsetY + y);
                            pixelColumn.Add(pixel);
                        }
                        pixels.Add(pixelColumn);
                    }

                    sprites.Add(new Sprite(spriteWidth, spriteHeight, pixels));
                }
            }

            return new SpriteSheet(sprites);
        }

        public Sprite? GetSpriteAtIndex(uint index)
        {
            if (index >= _sprites.Count)
            {
                return null;
            }
            return _sprites[(int)index];
        }
    }
}
