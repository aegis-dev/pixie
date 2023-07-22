using System.Drawing;

namespace Pixie.Internal
{
    internal static class Palette
    {
        // https://lospec.com/palette-list/retrocal-8
        // Sorter variant https://coolors.co/2f142f-2a584f-774448-c6505a-74a33f-6eb8a8-ee9c5d-fcffc0
        internal static readonly Color Purple = Color.FromArgb(0x2f142f);
        internal static readonly Color Green = Color.FromArgb(0x2a584f);
        internal static readonly Color Brown = Color.FromArgb(0x774448);
        internal static readonly Color Red = Color.FromArgb(0xc6505a);
        internal static readonly Color Salad = Color.FromArgb(0x74a33f);
        internal static readonly Color Teal = Color.FromArgb(0x6eb8a8);
        internal static readonly Color Yellow = Color.FromArgb(0xee9c5d);
        internal static readonly Color White = Color.FromArgb(0xfcffc0);

        private static List<Color> GetMainColors()
        {
            return new List<Color>()
            {
                Purple,
                Green,
                Brown,
                Red,
                Salad,
                Teal,
                Yellow,
                White,
            };
        }

        internal static PixieColor ColorFromRgb(Color color)
        {
            if (color.R == Purple.R && color.G == Purple.G && color.B == Purple.B)
            {
                return PixieColor.Purple;
            }
            if (color.R == Green.R && color.G == Green.G && color.B == Green.B)
            {
                return PixieColor.Green;
            }
            if (color.R == Brown.R && color.G == Brown.G && color.B == Brown.B)
            {
                return PixieColor.Brown;
            }
            if (color.R == Red.R && color.G == Red.G && color.B == Red.B)
            {
                return PixieColor.Red;
            }
            if (color.R == Salad.R && color.G == Salad.G && color.B == Salad.B)
            {
                return PixieColor.Salad;
            }
            if (color.R == Teal.R && color.G == Teal.G && color.B == Teal.B)
            {
                return PixieColor.Teal;
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
