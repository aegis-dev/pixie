using Pixie.Properties;
using Silk.NET.OpenGL;
using System.Drawing;
using System.Text;

namespace Pixie.Internal
{
    internal class GLRenderer
    {
        private readonly GL _gl;
        private readonly ShaderProgram _shader;

        public GLRenderer(in GL gl)
        {
            _gl = gl;
            _shader = new ShaderProgram(
                gl, 
                Encoding.Default.GetString(Resources.vertex_shader), 
                Encoding.Default.GetString(Resources.fragment_shader)
            );

            _gl.Disable(EnableCap.DepthTest);
            //_gl.Disable(EnableCap.Blend);
            //_gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            _gl.ClearColor(Color.Black);
        }

        public void Begin()
        {
            _shader.Enable();
        }

        public void End()
        {
            _shader.Disable();
        }

        public void ClearScreen()
        {
            _gl.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
        }

        public int GetUniformLocation(in string uniformName)
        {
            return _shader.GetUniformLocation(uniformName);
        }

        public void SetUniformInt(int location, int value)
        {
            _shader.SetUniformInt(location, value);
        }

        public unsafe void Render(in Model model, in Texture texture, in Texture palette)
        {
            _gl.BindVertexArray(model.VaoID);

            foreach (uint attributeId in VertexAttributes.Attributes)
            {
                _gl.EnableVertexAttribArray(attributeId);
            }

            _gl.ActiveTexture(texture.TextureUnit);
            _gl.BindTexture(GLEnum.Texture2D, texture.TextureID);

            _gl.ActiveTexture(palette.TextureUnit);
            _gl.BindTexture(GLEnum.Texture2D, palette.TextureID);

            _gl.DrawElements(GLEnum.Triangles, (uint)model.ModelData.Indices.Length, DrawElementsType.UnsignedInt, null);

            foreach (uint attributeId in VertexAttributes.Attributes)
            {
                _gl.DisableVertexAttribArray(attributeId);
            }

            _gl.BindVertexArray(0);
        }
    }
}
