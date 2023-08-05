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

using Silk.NET.OpenGL;

namespace Pixie.Internal
{
    internal class ShaderProgram
    {
        private readonly GL _gl;

        private readonly uint _programID;
        private readonly uint _vertexShaderID;
        private readonly uint _fragmentShaderID;

        public ShaderProgram(in GL gl, in string vertexSource, in string fragmentSource)
        {
            _gl = gl;

            _vertexShaderID = LoadShaderFromSource(vertexSource, ShaderType.VertexShader);
            _fragmentShaderID = LoadShaderFromSource(fragmentSource, ShaderType.FragmentShader);
            _programID = _gl.CreateProgram();
            _gl.AttachShader(_programID, _vertexShaderID);
            _gl.AttachShader(_programID, _fragmentShaderID);
            _gl.LinkProgram(_programID);

            {
                string programInfo;
                _gl.GetProgramInfoLog(_programID, out programInfo);
                if (programInfo.Length != 0)
                {
                    Console.WriteLine("SHADER PROGRAM: " + programInfo);
                    //TODO: throw
                }
            }

            _gl.ValidateProgram(_programID);

            {
                string programInfo;
                _gl.GetProgramInfoLog(_programID, out programInfo);
                if (programInfo.Length != 0)
                {
                    Console.WriteLine("SHADER PROGRAM: " + programInfo);
                    //TODO: throw
                }
            }
        }

        ~ShaderProgram()
        {
            CleanUp();
        }

        public void Enable()
        {
            _gl.UseProgram(_programID);
        }

        public void Disable()
        {
            _gl.UseProgram(0);
        }

        public void CleanUp()
        {
            Disable();
            _gl.DetachShader(_programID, _vertexShaderID);
            _gl.DetachShader(_programID, _fragmentShaderID);
            _gl.DeleteShader(_vertexShaderID);
            _gl.DeleteShader(_fragmentShaderID);
            _gl.DeleteProgram(_programID);
        }

        public int GetUniformLocation(in string uniformName)
        {
            return _gl.GetUniformLocation(_programID, uniformName);
        }

        public void SetUniformInt(int location, int value)
        {
            _gl.Uniform1(location, value);
        }

        private uint LoadShaderFromSource(in string source, ShaderType st)
        {
            uint shaderID = _gl.CreateShader(st);
            _gl.ShaderSource(shaderID, source);
            _gl.CompileShader(shaderID);

            return shaderID;
        }
    }
}
