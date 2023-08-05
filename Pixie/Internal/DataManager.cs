using Silk.NET.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;

namespace Pixie.Internal
{
    internal class DataManager
    {
        private readonly GL _gl;

        private readonly List<uint> _vaos = new List<uint>();
        private readonly List<uint> _vbos = new List<uint>();
        private readonly List<uint> _textures = new List<uint>();

        public DataManager(GL gl)
        {
            _gl = gl;
        }

        public Model LoadModel(in ModelData modelData)
        {
            uint vaoID = CreateVAO();

            BindIndicesBuffer(modelData.Indices);
            StoreVertexDataInAttributeList(in modelData.VertexData, BufferUsageARB.StaticDraw);
 
            UnbindVAO();

            return new Model(vaoID, modelData);
        }

        public unsafe Texture LoadTexture(in Bitmap textureImage, ImageMode imageMode, TextureUnit textureUnit)
        {
            BitmapData textureData = textureImage.LockBits(
                new Rectangle(0, 0, textureImage.Width, textureImage.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb
            );

            Texture texture = LoadTexture(textureData.Scan0.ToPointer(), (uint)textureData.Width, (uint)textureData.Height, imageMode, textureUnit);

            textureImage.UnlockBits(textureData);

            return texture;
        }

        public unsafe Texture LoadTexture(in byte[] imageData, uint width, uint height, ImageMode imageMode, TextureUnit textureUnit)
        {
            fixed(byte* data = imageData)
            {
                return LoadTexture(data, width, height, imageMode, textureUnit);
            }
        }

        public unsafe Texture LoadTexture(in void* data, uint width, uint height, ImageMode imageMode, TextureUnit textureUnit)
        {
            uint textureID = _gl.GenTexture();
            _gl.BindTexture(TextureTarget.Texture2D, textureID);

            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Nearest);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.Nearest);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.Repeat);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.Repeat);

            _gl.TexImage2D(
                TextureTarget.Texture2D,
                0,
                (int)imageMode,
                width,
                height,
                0,
                (GLEnum)imageMode,
                PixelType.UnsignedByte,
                data
            );

            _gl.BindTexture(TextureTarget.Texture2D, 0);

            _textures.Add(textureID);

            return new Texture(textureID, width, height, textureUnit, imageMode);
        }

        public unsafe void UpdateTexture(in byte[] data, uint width, uint height, in Texture texture)
        {
            _gl.BindTexture(TextureTarget.Texture2D, texture.TextureID);

            fixed (byte* dataPtr = data)
            {
                _gl.TexImage2D(
                   TextureTarget.Texture2D,
                   0,
                   (int)texture.ImageMode,
                   width,
                   height,
                   0,
                   (GLEnum)texture.ImageMode,
                   PixelType.UnsignedByte,
                   dataPtr
               );
            }

            _gl.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void CleanUp()
        {
            foreach (uint a in _vaos)
            {
                _gl.DeleteVertexArray(a);
            }
            foreach (uint a in _vbos)
            {
                _gl.DeleteBuffer(a);
            }
            foreach (uint a in _textures)
            {
                _gl.DeleteTexture(a);
            }
        }

        private uint CreateVAO()
        {
            uint vaoID = _gl.GenVertexArray();
            _vaos.Add(vaoID);
            _gl.BindVertexArray(vaoID);
            return vaoID;
        }

        private unsafe void StoreVertexDataInAttributeList(in VertexData[] vertexData, BufferUsageARB bufferUsage)
        {
            uint vboID = _gl.GenBuffer();
            _vbos.Add(vboID);
            _gl.BindBuffer(BufferTargetARB.ArrayBuffer, vboID);

            fixed (void* dataPtr = vertexData)
            {
                _gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(vertexData.Length * sizeof(VertexData)), dataPtr, bufferUsage);
            }

            _gl.VertexAttribPointer(VertexAttributes.PositionAttributeId, 3, VertexAttribPointerType.Float, false, (uint)sizeof(VertexData), (void*)VertexData.PositionOffset);
            _gl.VertexAttribPointer(VertexAttributes.TextureCoordAttributeId, 2, VertexAttribPointerType.Float, false, (uint)sizeof(VertexData), (void*)VertexData.TextureCoordOffset);

            _gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
        }

        private unsafe void StoreDataInAttributeList(uint attributeNumber, int coordinateSize, float[] data)
        {
            uint vboID = _gl.GenBuffer();
            _vbos.Add(vboID);
            _gl.BindBuffer(BufferTargetARB.ArrayBuffer, vboID);

            fixed (void* dataPtr = data)
            {
                _gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(data.Length * sizeof(float)), dataPtr, BufferUsageARB.StaticDraw);
            }

            _gl.VertexAttribPointer(attributeNumber, coordinateSize, VertexAttribPointerType.Float, false, 0, (void*)0);
            _gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
        }

        private void UnbindVAO()
        {
            _gl.BindVertexArray(0);
        }

        private unsafe void BindIndicesBuffer(int[] indices)
        {
            uint vboID = _gl.GenBuffer();
            _vbos.Add(vboID);
            _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, vboID);

            fixed (void* indicesPtr = indices)
            {
                _gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint)(indices.Length * sizeof(int)), indicesPtr, BufferUsageARB.StaticDraw);
            }
        }
    }
}
