using Pixie;
using Pixie.UI;

namespace PixieExample
{
    public class UIQuad : CanvasElement
    {
        public PixieColor Color { get; set; }

        public UIQuad(int positionX, int positionY, uint width, uint height, PixieColor color) : base(positionX, positionY, width, height)
        {
            Width = width;
            Height = height;
            Color = color;
        }

        public override void OnUpdate(in Renderer renderer, in Input input, float deltaTime)
        {  }

        public override void OnRender(in Renderer renderer, long x, long y)
        {
            renderer.RectangleFilled(x, y, x + Width, y + Height, Color);
        }
    }
}
