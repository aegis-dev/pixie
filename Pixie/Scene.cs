namespace Pixie
{
    public abstract class Scene
    {
        public abstract void OnStart(in Renderer renderer);

        public abstract Scene? OnUpdate(in GameState state, in Renderer renderer, in Input input, float deltaTime);

        public abstract void OnRender(in Renderer renderer, in Input input, float deltaTime);

        public abstract void OnDestroy();
    }
}
