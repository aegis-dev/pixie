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

        public event Action<int, int>? OnHover;
        public event Action<int, int>? OnClick;

        public abstract void OnUpdate(in Renderer renderer, in Input input, float deltaTime);
        public abstract void OnRender(in Renderer renderer, long x, long y);

        internal void InvokeOnHover(int x, int y)
        {
            OnHover?.Invoke(x, y);
        }

        internal void InvokeOnClick(int x, int y)
        {
            OnClick?.Invoke(x, y);
        }

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
