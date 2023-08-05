using System.Runtime.InteropServices;

namespace Pixie.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct Vec2
    {
        public float X;
        public float Y;

        public Vec2(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
