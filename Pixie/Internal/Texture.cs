using Silk.NET.OpenGL;

namespace Pixie.Internal
{
    public class Texture
    {
        public uint Width { get; }
        public uint Height { get; }
        public uint TextureID { get; }
        public TextureUnit TextureUnit { get; }
        public ImageMode ImageMode { get; }

        public Texture(uint id, uint width, uint height, TextureUnit textureUnit, ImageMode imageMode)
        {
            TextureID = id;
            TextureUnit = textureUnit;
            ImageMode = imageMode;
            Width = width;
            Height = height;
        }
    }
}
