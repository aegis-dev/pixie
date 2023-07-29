using Silk.NET.Input;
using System.Numerics;

namespace Pixie
{
    public class Input
    {
        public enum State
        {
            Up,
            Down
        }

        public long MouseX { 
            get { return (long)_mouseX; }
        }
        public long MouseY { 
            get { return (long)_mouseY; }
        }

        private readonly IKeyboard _keyboard;
        private readonly IMouse _mouse;
        private readonly Dictionary<Key, State> _lastKeyState = new Dictionary<Key, State>();
        private readonly Dictionary<MouseButton, State> _lastButtonState = new Dictionary<MouseButton, State>();

        private readonly float _mouseSensitivityX;
        private readonly float _mouseSensitivityY;
        private readonly float _maxMouseX;
        private readonly float _minMouseX;
        private readonly float _maxMouseY;
        private readonly float _minMouseY;
        private float _mouseX = 0;
        private float _mouseY = 0;
        private float _lastRawMouseX = 0;
        private float _lastRawMouseY = 0;

        internal Input(IInputContext inputContext, uint screenBufferWidth, uint screenBufferHeight, uint screenWidth, uint screenHeight)
        {
            
            _keyboard = inputContext.Keyboards.Count > 0 ? inputContext.Keyboards[0] : null;
            _mouse = inputContext.Mice.Count > 0 ? inputContext.Mice[0] : null;

            _maxMouseX = screenBufferWidth / 2.0f;
            _minMouseX = -screenBufferWidth / 2.0f;
            _maxMouseY = screenBufferHeight / 2.0f;
            _minMouseY = -screenBufferHeight / 2.0f;
            _mouseSensitivityX = screenBufferWidth / (float)screenWidth;
            _mouseSensitivityY = screenBufferHeight / (float)screenHeight;

            _mouse.MouseMove += OnMouseMove;
            _mouse.Cursor.CursorMode = CursorMode.Disabled;
        }

        public State GetKeyState(Key key)
        {
            return (_keyboard?.IsKeyPressed(key) ?? false) ? State.Down : State.Up;
        }

        public State GetLastKeyState(Key key)
        {
            State state;
            if (!_lastKeyState.TryGetValue(key, out state))
            {
                return State.Up;
            }
            return state;
        }

        public bool KeyPressed(Key key)
        {
            State lastState = GetLastKeyState(key);
            State currentState = GetKeyState(key);
            return currentState == State.Down && lastState != currentState;
        }

        public State GetButtonState(MouseButton button)
        {
            return (_mouse?.IsButtonPressed(button) ?? false) ? State.Down : State.Up;
        }

        public State GetLastButtonState(MouseButton button)
        {
            State state;
            if (!_lastButtonState.TryGetValue(button, out state))
            {
                return State.Up;
            }
            return state;
        }

        public bool ButtonPressed(MouseButton button)
        {
            State lastState = GetLastButtonState(button);
            State currentState = GetButtonState(button);
            return currentState == State.Down && lastState != currentState;
        }

        internal void UpdateLastStates()
        {
            foreach (Key key in Enum.GetValues(typeof(Key)))
            {
                if (key == Key.Unknown)
                {
                    continue;
                }
                _lastKeyState[key] = GetKeyState(key);
            }

            foreach (MouseButton button in Enum.GetValues(typeof(MouseButton)))
            {
                if (button == MouseButton.Unknown)
                {
                    continue;
                }
                _lastButtonState[button] = GetButtonState(button);
            }
        }

        private void OnMouseMove(IMouse mouse, Vector2 position)
        {
            float mouseDeltaX = position.X - _lastRawMouseX;
            float mouseDeltaY = position.Y - _lastRawMouseY;
            _lastRawMouseX = position.X;
            _lastRawMouseY = position.Y;

            float normalizedRelX = mouseDeltaX * _mouseSensitivityX;
            float normalizedRelY = mouseDeltaY * _mouseSensitivityY;

            float mouseX = _mouseX;
            float mouseY = _mouseY;

            mouseX += normalizedRelX;
            mouseX = Math.Min(mouseX, _maxMouseX);
            mouseX = Math.Max(mouseX, _minMouseX);

            mouseY -= normalizedRelY;
            mouseY = Math.Min(mouseY, _maxMouseY);
            mouseY = Math.Max(mouseY, _minMouseY);

            _mouseX = mouseX;
            _mouseY = mouseY;
        }
    }
}
