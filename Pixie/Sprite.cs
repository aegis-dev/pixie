using Pixie.Internal;
using System.Drawing;

namespace Pixie
{
    public class Sprite
    {
        public uint Width { get; }
        public uint Height { get; }
        public IReadOnlyList<IReadOnlyList<PixieColor>> Pixels { get; }

        public Sprite(uint width, uint height, IReadOnlyList<IReadOnlyList<PixieColor>> pixels) 
        { 
            this.Width = width;
            this.Height = height;
            this.Pixels = pixels;
        }

        public static Sprite FromBitmapPNG(in Bitmap bitmap)
        {
            List<List<PixieColor>> pixels = new List<List<PixieColor>>();


            for (long x = 0; x < bitmap.Width; ++x)
            {
                List<PixieColor> column = new List<PixieColor>();
                for (long y = bitmap.Height - 1; y >= 0; --y)

                {
                    Color rgbColor = bitmap.GetPixel((int)x, (int)y);
                    column.Add(Palette.ColorFromRgb(rgbColor));
                }
                pixels.Add(column);
            }

            return new Sprite((uint)bitmap.Width, (uint)bitmap.Height, pixels);
        }

        public PixieColor ColorAt(uint x, uint y)
        {
            if (x >= Width || y >= Height) {
                return PixieColor.None;
            }

            return Pixels[(int)x][(int)y];
        }
    }
}
