using Silk.NET.Input;

namespace Pixie
{
    public abstract class Scene
    {
        public abstract void OnStart(Renderer renderer);

        public abstract Scene OnUpdate(Renderer renderer, IKeyboard keyboard, IMouse mouse, float deltaTime);

        public abstract void OnRender(Renderer renderer, float deltaTime);

        public abstract void OnDestroy();
    }
}
