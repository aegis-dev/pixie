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

using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Maths;
using Window = Silk.NET.Windowing.Window;

using Pixie.Internal;

namespace Pixie
{
    public class GameContext
    {
        private readonly IWindow _window;
        private GL _gl;

        private readonly uint _bufferWidth;
        private readonly uint _bufferHeight;
        private DataManager _dataManager;
        private Renderer _renderer;
        private Input _input;
        private Scene _scene;

        public GameContext(uint bufferWidth, uint bufferHeight, string title, bool fullscrean)
        {
            if (bufferWidth % 4 != 0)
            {
                throw new ArgumentException("Buffer width must be 4 pixel aligned");
            }
            if (bufferWidth < bufferHeight)
            {
                throw new ArgumentException("Buffer height can't be smaller than width");
            }

            var options = WindowOptions.Default;
            options.Title = title;
            options.WindowBorder = WindowBorder.Fixed;
            if (fullscrean)
            {
                options.WindowState = WindowState.Fullscreen;
            }

            _bufferWidth = bufferWidth;
            _bufferHeight = bufferHeight;

            _window = Window.Create(options);

            _window.Load += OnLoad;
            _window.Update += OnUpdate;
            _window.Render += OnRender;
            _window.Closing += OnUnload;
        }

        public void RunGame(Scene scene)
        {
            _scene = scene;
            _window.Run();
        }

        public void StopGame()
        {
            _window.Close();
        }

        private void OnLoad()
        {
            _gl = GL.GetApi(_window);

            if (_window.WindowState == WindowState.Fullscreen)
            {
                _window.Size = new Vector2D<int>(
                   _window.VideoMode.Resolution.Value.X,
                   _window.VideoMode.Resolution.Value.Y
               );
            }
            else
            {
                _window.Size = new Vector2D<int>(
                    (int)(_window.VideoMode.Resolution.Value.X * 0.9f),
                    (int)(_window.VideoMode.Resolution.Value.Y * 0.9f)
                );
            }

            uint displayWidth = (uint)_window.Size.X;
            uint displayHeight = (uint)_window.Size.Y;

            float modifier = (float)displayHeight / (float)_bufferHeight;

            uint viewportWidth = (uint)(_bufferWidth * modifier);
            uint viewportHeight = displayHeight;

            _gl.Viewport(
                (int)(displayWidth - viewportWidth) / 2,
                0,
                viewportWidth,
                viewportHeight
            );

            _dataManager = new DataManager(_gl);
            _renderer = new Renderer(
                _dataManager, 
                new FrameBuffer(_dataManager, _bufferWidth, _bufferHeight), 
                new GLRenderer(_gl)
            );
            _input = new Input(
                _window.CreateInput(), 
                _bufferWidth, 
                _bufferHeight, 
                displayWidth, 
                displayHeight
            );

            _scene.OnStart(_renderer);
        }

        private void OnUnload()
        {
            _scene.OnDestroy();
            _dataManager.CleanUp();
        }

        private void OnUpdate(double deltaTime)
        {
            GameState state = new GameState();
            Scene? scene = _scene.OnUpdate(state, _renderer, _input, (float)deltaTime);

            if (state.ShouldShutDown())
            {
                _scene.OnDestroy();
                _window.Close();
                return;
            }

            if (scene != null)
            {
                if (_scene != null)
                {
                    _scene.OnDestroy();
                }
                _renderer.CameraX = 0;
                _renderer.CameraY = 0;
                _renderer.SetBackgroundColor(BaseColor.Black);

                _scene = scene;
                _scene.OnStart(_renderer);
            } 
            else
            {
                _scene.OnRender(_renderer, _input, (float)deltaTime);
            }

            _input.UpdateLastStates();
        }

        private void OnRender(double deltaTime)
        {
            _renderer.RenderToScreen();
        }
    }
}
