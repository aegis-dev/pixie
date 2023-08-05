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
