namespace Pixie.UI
{
    internal class CanvasBounds : CanvasElement
    {
        public CanvasBounds(uint width, uint height) : base(0, 0, width, height)
        { }

        public override void OnRender(in Renderer renderer, long x, long y)
        { }

        public override void OnUpdate(in Renderer renderer, in Input input, float deltaTime)
        { }
    }
}
