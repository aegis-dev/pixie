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
