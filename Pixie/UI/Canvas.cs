namespace Pixie.UI
{
    public class Canvas
    {
        public uint Width {
            get { return _bounds.Width; }
        }
        public uint Height
        {
            get { return _bounds.Height; }
        }

        private readonly CanvasElement _bounds;

        public Canvas(in Renderer renderer)
        {
            _bounds = new CanvasBounds(renderer.FrameBufferWidth, renderer.FrameBufferHeight);
        }

        public void AddChildElement(in CanvasElement element)
        {
            _bounds.AddChildElement(element);
        }

        public void OnUpdate(in Renderer renderer, in Input input, float deltaTime)
        {
            foreach (CanvasElement child in _bounds.Children)
            {
                UpdateElement(child, renderer, input, deltaTime);
            }
        }

        private void UpdateElement(in CanvasElement element, in Renderer renderer, in Input input, float deltaTime)
        {
            element.OnUpdate(renderer, input, deltaTime);
            foreach (CanvasElement child in element.Children)
            {
                UpdateElement(child, renderer, input, deltaTime);
            }
            // TODO: onClick onHover events
        }

        public void Render(in Renderer renderer)
        {
            foreach (CanvasElement child in _bounds.Children)
            {
                RenderElement(child, renderer);
            }
        }

        private void RenderElement(in CanvasElement element, in Renderer renderer)
        {
            if (element.Enabled)
            {
                long absPositionX = renderer.CameraX - (renderer.FrameBufferWidth / 2) + element.GetAbsolutePositionX();
                long absPositionY = renderer.CameraY - (renderer.FrameBufferHeight / 2) + element.GetAbsolutePositionY();
                element.OnRender(renderer, absPositionX, absPositionY);

                foreach (CanvasElement child in element.Children)
                {
                    RenderElement(child, renderer);
                }
            }
        }
    }
}
