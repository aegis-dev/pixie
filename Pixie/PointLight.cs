namespace Pixie
{
    public class PointLight
    {
        public long X { get; set; }
        public long Y { get; set; }
        public short Radius { get; set; }

        public PointLight(long x, long y, short radius)
        {
            X = x;
            Y = y;
            Radius = radius;
        }

        public Brightness GetBrightness(long x, long y)
        {
            float distance = Distance(x, y);
            float ratio = distance / Radius;
            if (ratio > 1.0f)
            {
                return Brightness.VeryDark;
            }
            else if (ratio > 0.8f)
            {
                return Brightness.Dark;
            }
            else if (ratio > 0.6f)
            {
                return Brightness.Dim;
            }
            return Brightness.Normal;
        }

        public float Distance(long x, long y)
        {
            return float.Sqrt((float)Math.Pow(X - x, 2) + (float)Math.Pow(Y - y, 2));
        }
    }
}
