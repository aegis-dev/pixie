namespace Pixie
{
    public abstract class Scene
    {
        public abstract void OnStart(Renderer renderer);

        public abstract Scene OnUpdate(GameState state, Renderer renderer, Input input, float deltaTime);

        public abstract void OnRender(Renderer renderer, Input input, float deltaTime);

        public abstract void OnDestroy();
    }
}
