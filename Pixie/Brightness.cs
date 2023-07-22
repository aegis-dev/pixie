namespace Pixie
{
    public enum Brightness
    {
        Normal = 0,
        Dim,
        Dark,
        VeryDark,
    }

    public static class BrightnessUtils
    {
        public static bool IsLighter(Brightness lhs, Brightness rhs)
        {
            return lhs < rhs;
        }

        public static bool IsDarker(Brightness lhs, Brightness rhs)
        {
            return !IsLighter(lhs, rhs);
        }
    }
}
