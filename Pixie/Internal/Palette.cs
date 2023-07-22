using System.Drawing;

namespace Pixie.Internal
{
    internal static class Palette
    {
        // https://lospec.com/palette-list/retrocal-8
        // Sorter variant https://coolors.co/2f142f-2a584f-774448-c6505a-74a33f-6eb8a8-ee9c5d-fcffc0
        private static List<Color> GetMainColors()
        {
            return new List<Color>()
            {
                Color.FromArgb(0x2f142f),
                Color.FromArgb(0x2a584f),
                Color.FromArgb(0x774448),
                Color.FromArgb(0xc6505a),
                Color.FromArgb(0x74a33f),
                Color.FromArgb(0x6eb8a8),
                Color.FromArgb(0xee9c5d),
                Color.FromArgb(0xfcffc0),
            };
        }

        internal static List<Color> GetPalette()
        {
            List<Color> palette = new List<Color>();

            List<Color> mainColors = GetMainColors();

            // Normal colors
            foreach (Color mainColor in mainColors)
            {
                palette.Add(Color.FromArgb(
                    (byte)(mainColor.R * 0.7f),
                    (byte)(mainColor.G * 0.7f),
                    (byte)(mainColor.B * 0.7f)
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
