using Silk.NET.OpenGL;

namespace Pixie.Internal
{
    internal class FrameBuffer
    {
        public uint Width { get; }
        public uint Height { get; }
        public Model FrameBufferQuad { get;  }
        public Texture FrameBufferTexture { get; }

        private byte[] _frameBuffer;

        public FrameBuffer(in DataManager loader, uint width, uint height)
        {
            Width = width;
            Height = height;

            VertexData[] vertices = new VertexData[]
            {
                new VertexData(new Vec3(-1.0f,  -1.0f, 0.0f), new Vec2(0.0f, 0.0f)),
                new VertexData(new Vec3(-1.0f,   1.0f, 0.0f), new Vec2(0.0f, 1.0f)),
                new VertexData(new Vec3( 1.0f,   1.0f, 0.0f), new Vec2(1.0f, 1.0f)),
                new VertexData(new Vec3( 1.0f,  -1.0f, 0.0f), new Vec2(1.0f, 0.0f)),
            };

            int[] indices = new int[] {
                0, 1, 2,
                3, 0, 2
            };

            ModelData modelData = new ModelData(vertices, indices);

            FrameBufferQuad = loader.LoadModel(modelData);
            _frameBuffer = new byte[width * height];
            FrameBufferTexture = loader.LoadTexture(_frameBuffer, width, height, ImageMode.RED, TextureUnit.Texture0);
        }
      
        public Texture RenderToTexture(in DataManager dataManager)
        {
            dataManager.UpdateTexture(_frameBuffer, Width, Height, FrameBufferTexture);
            return FrameBufferTexture;
        }

        public void Clear()
        {
            _frameBuffer = new byte[_frameBuffer.Length];
        }

        public void SetPixel(uint x, uint y, byte colorIdx)
        {
            if (x >= Width || y >= Height)
            {
                return;
            }

            uint pixelOffset = y * Width + x;
            _frameBuffer[pixelOffset] = colorIdx;
        }
      
    }
}
