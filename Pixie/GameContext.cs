using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Maths;
using Window = Silk.NET.Windowing.Window;

using Pixie.Internal;
using Silk.NET.SDL;

namespace Pixie
{
    public class GameContext
    {
        private readonly IWindow _window;
        private GL _gl;

        private uint _bufferWidth;
        private uint _bufferHeight;
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
            Scene scene = _scene.OnUpdate(state, _renderer, _input, (float)deltaTime);

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
                _renderer.SetBackgroundColor(PixieColor.Purple);

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
