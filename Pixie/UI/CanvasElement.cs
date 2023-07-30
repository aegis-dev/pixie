namespace Pixie.UI
{
    public abstract class CanvasElement
    {
        public bool Enabled { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public uint Width { get; set; }
        public uint Height { get; set; }
        public CanvasElement? Parent { get; set; }
        public List<CanvasElement> Children { get; set; }

        public event Action<uint, uint>? OnHover;
        public event Action<uint, uint>? OnClick;

        public abstract void OnUpdate(in Renderer renderer, in Input input, float deltaTime);
        public abstract void OnRender(in Renderer renderer, long x, long y);

        protected CanvasElement(int positionX, int positionY, uint width, uint height)
        {
            Enabled = true;
            PositionX = positionX;
            PositionY = positionY;
            Width = width;
            Height = height;
            Parent = null;
            Children = new List<CanvasElement>();
        }

        public int GetAbsolutePositionX()
        {
            int parentPosX = Parent?.GetAbsolutePositionX() ?? 0;
            return parentPosX + PositionX;
        }

        public int GetAbsolutePositionY()
        {
            int parentPosY = Parent?.GetAbsolutePositionY() ?? 0;
            return parentPosY + PositionY;
        }

        public void AddChildElement(in CanvasElement element)
        {
            element.Parent = this;
            Children.Add(element);
        }

        // Attach CanvasElement to the same parent and position it below
        public void AttachNeighbourElementBelow(in CanvasElement element)
        {
            Parent.AddChildElement(element);
            element.PositionX += PositionX;
            element.PositionY += PositionY - (int)element.Height;
        }

        // Attach CanvasElement to the same parent and position it above
        public void AttachNeighbourElementAbove(in CanvasElement element)
        {
            Parent.AddChildElement(element);
            element.PositionX += PositionX;
            element.PositionY += PositionY + (int)Height;
        }

        // Attach CanvasElement to the same parent and position it to the right side
        public void AttachNeighbourElementToRight(in CanvasElement element)
        {
            Parent.AddChildElement(element);
            element.PositionX += PositionX + (int)Width;
            element.PositionY += PositionY;
        }
        // Attach CanvasElement to the same parent and position it to the left side
        public void AttachNeighbourElementToLeft(in CanvasElement element)
        {
            Parent.AddChildElement(element);
            element.PositionX += PositionX - (int)element.Width;
            element.PositionY += PositionY;
        }
    }
}
