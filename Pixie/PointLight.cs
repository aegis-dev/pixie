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

namespace Pixie
{
    public class PointLight
    {
        public long X { get; set; }
        public long Y { get; set; }
        public short Radius { get; set; }

        public PointLight(long x, long y, short radius)
        {
            X = x;
            Y = y;
            Radius = radius;
        }

        public Brightness GetBrightness(long x, long y)
        {
            float distance = Distance(x, y);
            float ratio = distance / Radius;
            if (ratio > 1.0f)
            {
                return Brightness.VeryDark;
            }
            else if (ratio > 0.8f)
            {
                return Brightness.Dark;
            }
            else if (ratio > 0.6f)
            {
                return Brightness.Dim;
            }
            return Brightness.Normal;
        }

        public float Distance(long x, long y)
        {
            return float.Sqrt((float)Math.Pow(X - x, 2) + (float)Math.Pow(Y - y, 2));
        }
    }
}
