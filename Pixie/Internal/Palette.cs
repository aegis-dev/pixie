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

#if DEBUG
//#define DUMP_PALETTES
#endif

using System.Drawing;

namespace Pixie.Internal
{
    internal class Palette
    {
        public const int MaxColors = 256;
        public static readonly IReadOnlyList<Color> ColorPalette = GetPalette();

        private static List<Color> GetPalette()
        {
            // https://www.pixilart.com/palettes/pixie-32-77487
            List<Color> palette = new List<Color>()
            {
                Color.FromArgb(0x000000),
                Color.FromArgb(0x5B5B5B),
                Color.FromArgb(0x989898),
                Color.FromArgb(0xFEFEFE),
                Color.FromArgb(0x5B0D0E),
                Color.FromArgb(0x97151A),
                Color.FromArgb(0xE43B44),
                Color.FromArgb(0xE96267),

                Color.FromArgb(0xA34406),
                Color.FromArgb(0xF46609),
                Color.FromArgb(0xF77622),
                Color.FromArgb(0xF9924E),
                Color.FromArgb(0xB76F01),
                Color.FromArgb(0xF49401),
                Color.FromArgb(0xFEAE34),
                Color.FromArgb(0xFEBE5D),

                Color.FromArgb(0x377E28),
                Color.FromArgb(0x49A835),
                Color.FromArgb(0x63C74D),
                Color.FromArgb(0x82D271),
                Color.FromArgb(0x005B83),
                Color.FromArgb(0x0079AF),
                Color.FromArgb(0x0099DB),
                Color.FromArgb(0x16B7FF),

                Color.FromArgb(0x003283),
                Color.FromArgb(0x0042AF),
                Color.FromArgb(0x0055DB),
                Color.FromArgb(0x166EFF),
                Color.FromArgb(0x53288A),
                Color.FromArgb(0x6F36B8),
                Color.FromArgb(0x8C5BCF),
                Color.FromArgb(0xA47CD9),
            };

#if DUMP_PALETTES
            DumpColorPalette(palette, "pixie_palette.png");
#endif

            return palette;
        }

        internal static byte ColorFromRgb(Color color)
        {
            if (color.R == 0 && color.G == 0 && color.B == 0 && color.A == 0)
            {
                return (byte)PixieColor.None;
            }

            for (int colorIdx = 0; colorIdx < ColorPalette.Count; ++colorIdx)
            {
                Color paletteColor = ColorPalette[colorIdx];
                if (color.R == paletteColor.R && color.G == paletteColor.G && color.B == paletteColor.B)
                {
                    return (byte)(colorIdx + 1);
                }
            }

            return (byte)PixieColor.None;
        }

#if DUMP_PALETTES
        private static void DumpColorPalette(List<Color> palette, string fileName)
        {
            Bitmap paletteTexture = new Bitmap(palette.Count, 1);
            for (int idx = 0; idx < palette.Count; ++idx)
            {
                Color colorWithAplha = Color.FromArgb(255, palette[idx]);
                paletteTexture.SetPixel(idx, 0, colorWithAplha);
            }
            paletteTexture.Save(fileName);
        }

#endif
    }
}
