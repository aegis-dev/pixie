using System.Drawing;

namespace Pixie.Internal
{
    internal static class Palette
    {
        // Original Pixie-8 palette
        // https://coolors.co/2b292d-596694-32c9fa-99ff24-ff3f0a-ff8c17-fff124-fff9e8
        internal static readonly Color Black = Color.FromArgb(0x2B292D);
        internal static readonly Color Violet = Color.FromArgb(0x596694);
        internal static readonly Color Blue = Color.FromArgb(0x32C9FA);
        internal static readonly Color Green = Color.FromArgb(0x99FF24);
        internal static readonly Color Red = Color.FromArgb(0xFF3F0A);
        internal static readonly Color Orange = Color.FromArgb(0xFF8C17);
        internal static readonly Color Yellow = Color.FromArgb(0xFFF124);
        internal static readonly Color White = Color.FromArgb(0xFFF9E8);

        private static List<Color> GetMainColors()
        {
            return new List<Color>()
            {
                Black,
                Violet,
                Blue,
                Green,
                Red,
                Orange,
                Yellow,
                White,
            };
        }

        internal static PixieColor ColorFromRgb(Color color)
        {
            if (color.R == Black.R && color.G == Black.G && color.B == Black.B)
            {
                return PixieColor.Black;
            }
            if (color.R == Violet.R && color.G == Violet.G && color.B == Violet.B)
            {
                return PixieColor.Violet;
            }
            if (color.R == Blue.R && color.G == Blue.G && color.B == Blue.B)
            {
                return PixieColor.Blue;
            }
            if (color.R == Green.R && color.G == Green.G && color.B == Green.B)
            {
                return PixieColor.Green;
            }
            if (color.R == Red.R && color.G == Red.G && color.B == Red.B)
            {
                return PixieColor.Red;
            }
            if (color.R == Orange.R && color.G == Orange.G && color.B == Orange.B)
            {
                return PixieColor.Orange;
            }
            if (color.R == Yellow.R && color.G == Yellow.G && color.B == Yellow.B)
            {
                return PixieColor.Yellow;
            }
            if (color.R == White.R && color.G == White.G && color.B == White.B)
            {
                return PixieColor.White;
            }

            return PixieColor.None;
        }

        internal static List<Color> GetPalette()
        {
            List<Color> palette = new List<Color>();

            List<Color> mainColors = GetMainColors();

            // Normal colors
            foreach (Color mainColor in mainColors)
            {
                palette.Add(Color.FromArgb(
                    (byte)(mainColor.R),
                    (byte)(mainColor.G),
                    (byte)(mainColor.B)
                ));
            }

            // Dim colors
            foreach (Color mainColor in mainColors)
            {
                palette.Add(Color.FromArgb(
                    (byte)(mainColor.R * 0.7f),
                    (byte)(mainColor.G * 0.7f),
                    (byte)(mainColor.B * 0.7f)
                ));
            }

            // Dark colors
            foreach (Color mainColor in mainColors)
            {
                palette.Add(Color.FromArgb(
                    (byte)(mainColor.R * 0.2f),
                    (byte)(mainColor.G * 0.2f),
                    (byte)(mainColor.B * 0.2f)
                ));
            }

            // Black colors
            foreach (Color mainColor in mainColors)
            {
                palette.Add(Color.FromArgb(
                 (byte)(mainColor.R * 0.1f),
                 (byte)(mainColor.G * 0.1f),
                 (byte)(mainColor.B * 0.1f)
             ));
            }

            return palette;
        }
    }
}
