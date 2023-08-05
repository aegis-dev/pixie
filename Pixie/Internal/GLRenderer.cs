//
// Copyright © 2023  Egidijus Lileika
//
// This file is part of Pixie - Framework for 2D game development
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
//

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
            _gl.Disable(EnableCap.Blend);
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

            // TODO: technically we can iterate over list of textures and bind them one by one like we do with vertex attributes
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
