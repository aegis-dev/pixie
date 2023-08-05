
using System.Runtime.InteropServices;

namespace Pixie.Internal
{
    internal static class VertexAttributes
    {
        public const uint AttributesCount = 2;
        public const uint PositionAttributeId = 0;
        public const uint TextureCoordAttributeId = 1;

        public static readonly uint[] Attributes = new uint[(int)AttributesCount]
        {
            PositionAttributeId, TextureCoordAttributeId
        };
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct VertexData
    {
        public const uint PositionOffset = 0;
        public const uint TextureCoordOffset = sizeof(float) * 3;

        public Vec3 Position;
        public Vec2 TextureCoord;

        public VertexData(Vec3 position, Vec2 textCoord)
        {
            Position = position;
            TextureCoord = textCoord;
        }
    }
}
