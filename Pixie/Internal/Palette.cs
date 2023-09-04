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

namespace Pixie.Internal
{
    internal class Palette
    {
        public const int MaxColors = 256;
        public static readonly IReadOnlyList<Color> ColorPalette = GetPalette();

        private static List<Color> GetBasePalette()
        {
            // Original Pixie-8 palette
            // https://coolors.co/2b292d-596694-32c9fa-99ff24-ff3f0a-ff8c17-fff124-fff9e8
            return new List<Color>()
            {
                Color.FromArgb(0x2B292D),
                Color.FromArgb(0x596694),
                Color.FromArgb(0x32C9FA),
                Color.FromArgb(0x99FF24),
                Color.FromArgb(0xFF3F0A),
                Color.FromArgb(0xFF8C17),
                Color.FromArgb(0xFFF124),
                Color.FromArgb(0xFFF9E8),
            };
        }

        private static List<Color> GetBlackAndWhitePalette()
        {
            // Palette based on 
            // https://www.pixilart.com/palettes/black-and-white-55971
            return new List<Color>()
            {
                Color.FromArgb(0x1A1A1A),
                Color.FromArgb(0x242424),
                Color.FromArgb(0x363636),
                Color.FromArgb(0x525252),
                Color.FromArgb(0x787878),
                Color.FromArgb(0xADADAD),
                Color.FromArgb(0xE8E8E8),
                Color.FromArgb(0xFFFFFF),
            };
        }

        private static List<Color> GetCorruptedIcePalette()
        {
            // Palette based on 
            // https://www.pixilart.com/palettes/depths-unfathomed-40789
            // https://coolors.co/06131b-0f2c35-2b4e50-416d68-507f7a-739f99-96c0bb-cadedb
            return new List<Color>()
            {
                Color.FromArgb(0x06131B),
                Color.FromArgb(0x0F2C35),
                Color.FromArgb(0x2B4E50),
                Color.FromArgb(0x416D68),
                Color.FromArgb(0x507F7A),
                Color.FromArgb(0x739F99),
                Color.FromArgb(0x96C0BB),
                Color.FromArgb(0xCADEDB),
            };
        }

        internal static byte ColorFromRgb(Color color)
        {
            for (int colorIdx = 0; colorIdx < ColorPalette.Count; ++colorIdx)
            {
                Color paletteColor = ColorPalette[colorIdx];
                if (color.R == paletteColor.R && color.G == paletteColor.G && color.B == paletteColor.B)
                {
                    return (byte)(colorIdx + 1);
                }
            }

            return (byte)BaseColor.None;
        }

        private static List<Color> GetPalette()
        {
            List<Color> palette = new List<Color>();

            List<Color> baseColors = GetBasePalette();

            // Base palette
            foreach (Color color in baseColors)
            {
                palette.Add(Color.FromArgb(
                    color.R,
                    color.G,
                    color.B
                ));
            }

            // Base dim palette
            foreach (Color baseColor in baseColors)
            {
                palette.Add(Color.FromArgb(
                    (byte)(baseColor.R * 0.5f),
                    (byte)(baseColor.G * 0.5f),
                    (byte)(baseColor.B * 0.5f)
                ));
            }

            // Base dark palette
            foreach (Color baseColor in baseColors)
            {
                palette.Add(Color.FromArgb(
                    (byte)(baseColor.R * 0.2f),
                    (byte)(baseColor.G * 0.2f),
                    (byte)(baseColor.B * 0.2f)
                ));
            }

            // Black And White palette
            foreach (Color color in GetBlackAndWhitePalette())
            {
                palette.Add(Color.FromArgb(
                    color.R,
                    color.G,
                    color.B
                ));
            }

            // Corrupted Ice palette
            foreach (Color color in GetCorruptedIcePalette())
            {
                palette.Add(Color.FromArgb(
                    color.R,
                    color.G,
                    color.B
                ));
            }

#if DEBUG
            // Dump palette png for debugging
            Bitmap paletteTexture = new Bitmap(palette.Count, 1);
            for (int idx = 0; idx < palette.Count; ++idx)
            {
                paletteTexture.SetPixel(idx, 0, palette[idx]);
            }
            paletteTexture.Save("full_palette.png");
#endif

            return palette;
        }
    }
}
