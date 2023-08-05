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

namespace Pixie.Internal
{
    internal struct ModelData
    {
        public readonly VertexData[] VertexData;
        public readonly int[] Indices;

        public ModelData() {
            VertexData = new VertexData[0];
            Indices = new int[0];
        }

        public ModelData(VertexData[] vertexData, int[] indices)
        {
            VertexData = vertexData;
            Indices = indices;
        }
    }
}
