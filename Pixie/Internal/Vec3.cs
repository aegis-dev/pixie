using System.Numerics;
using System.Runtime.InteropServices;

namespace Pixie.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct Vec3
    {
        public float X;
        public float Y;
        public float Z;

        public Vec3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3 Clone()
        {
            return new Vector3(X, Y, Z);
        }
    }
}
