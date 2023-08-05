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

using Pixie.Properties;

namespace Pixie.Internal
{
    internal class Font
    {
        private static readonly Object _lock = new Object();
        private static Font _instance;

        private readonly Dictionary<char, Sprite> _glyphs = new Dictionary<char, Sprite>();
        private readonly Sprite _nullGlyph;

        private Font()
        {
            _glyphs.Add('a', Sprite.FromBitmapPNG(Resources.font_1));
            _glyphs.Add('b', Sprite.FromBitmapPNG(Resources.font_2));
            _glyphs.Add('c', Sprite.FromBitmapPNG(Resources.font_3));
            _glyphs.Add('d', Sprite.FromBitmapPNG(Resources.font_4));
            _glyphs.Add('e', Sprite.FromBitmapPNG(Resources.font_5));
            _glyphs.Add('f', Sprite.FromBitmapPNG(Resources.font_6));
            _glyphs.Add('g', Sprite.FromBitmapPNG(Resources.font_7));
            _glyphs.Add('h', Sprite.FromBitmapPNG(Resources.font_8));
            _glyphs.Add('i', Sprite.FromBitmapPNG(Resources.font_9));
            _glyphs.Add('j', Sprite.FromBitmapPNG(Resources.font_10));
            _glyphs.Add('k', Sprite.FromBitmapPNG(Resources.font_11));
            _glyphs.Add('l', Sprite.FromBitmapPNG(Resources.font_12));
            _glyphs.Add('m', Sprite.FromBitmapPNG(Resources.font_13));
            _glyphs.Add('n', Sprite.FromBitmapPNG(Resources.font_14));
            _glyphs.Add('o', Sprite.FromBitmapPNG(Resources.font_15));
            _glyphs.Add('p', Sprite.FromBitmapPNG(Resources.font_16));
            _glyphs.Add('q', Sprite.FromBitmapPNG(Resources.font_17));
            _glyphs.Add('r', Sprite.FromBitmapPNG(Resources.font_18));
            _glyphs.Add('s', Sprite.FromBitmapPNG(Resources.font_19));
            _glyphs.Add('t', Sprite.FromBitmapPNG(Resources.font_20));
            _glyphs.Add('u', Sprite.FromBitmapPNG(Resources.font_21));
            _glyphs.Add('v', Sprite.FromBitmapPNG(Resources.font_22));
            _glyphs.Add('w', Sprite.FromBitmapPNG(Resources.font_23));
            _glyphs.Add('x', Sprite.FromBitmapPNG(Resources.font_24));
            _glyphs.Add('y', Sprite.FromBitmapPNG(Resources.font_25));
            _glyphs.Add('z', Sprite.FromBitmapPNG(Resources.font_26));

            _glyphs.Add('A', Sprite.FromBitmapPNG(Resources.font_27));
            _glyphs.Add('B', Sprite.FromBitmapPNG(Resources.font_28));
            _glyphs.Add('C', Sprite.FromBitmapPNG(Resources.font_29));
            _glyphs.Add('D', Sprite.FromBitmapPNG(Resources.font_30));
            _glyphs.Add('E', Sprite.FromBitmapPNG(Resources.font_31));
            _glyphs.Add('F', Sprite.FromBitmapPNG(Resources.font_32));
            _glyphs.Add('G', Sprite.FromBitmapPNG(Resources.font_33));
            _glyphs.Add('H', Sprite.FromBitmapPNG(Resources.font_34));
            _glyphs.Add('I', Sprite.FromBitmapPNG(Resources.font_35));
            _glyphs.Add('J', Sprite.FromBitmapPNG(Resources.font_36));
            _glyphs.Add('K', Sprite.FromBitmapPNG(Resources.font_37));
            _glyphs.Add('L', Sprite.FromBitmapPNG(Resources.font_38));
            _glyphs.Add('M', Sprite.FromBitmapPNG(Resources.font_39));
            _glyphs.Add('N', Sprite.FromBitmapPNG(Resources.font_40));
            _glyphs.Add('O', Sprite.FromBitmapPNG(Resources.font_41));
            _glyphs.Add('P', Sprite.FromBitmapPNG(Resources.font_42));
            _glyphs.Add('Q', Sprite.FromBitmapPNG(Resources.font_43));
            _glyphs.Add('R', Sprite.FromBitmapPNG(Resources.font_44));
            _glyphs.Add('S', Sprite.FromBitmapPNG(Resources.font_45));
            _glyphs.Add('T', Sprite.FromBitmapPNG(Resources.font_46));
            _glyphs.Add('U', Sprite.FromBitmapPNG(Resources.font_47));
            _glyphs.Add('V', Sprite.FromBitmapPNG(Resources.font_48));
            _glyphs.Add('W', Sprite.FromBitmapPNG(Resources.font_49));
            _glyphs.Add('X', Sprite.FromBitmapPNG(Resources.font_50));
            _glyphs.Add('Y', Sprite.FromBitmapPNG(Resources.font_51));
            _glyphs.Add('Z', Sprite.FromBitmapPNG(Resources.font_52));

            _glyphs.Add('0', Sprite.FromBitmapPNG(Resources.font_53));
            _glyphs.Add('1', Sprite.FromBitmapPNG(Resources.font_54));
            _glyphs.Add('2', Sprite.FromBitmapPNG(Resources.font_55));
            _glyphs.Add('3', Sprite.FromBitmapPNG(Resources.font_56));
            _glyphs.Add('4', Sprite.FromBitmapPNG(Resources.font_57));
            _glyphs.Add('5', Sprite.FromBitmapPNG(Resources.font_58));
            _glyphs.Add('6', Sprite.FromBitmapPNG(Resources.font_59));
            _glyphs.Add('7', Sprite.FromBitmapPNG(Resources.font_60));
            _glyphs.Add('8', Sprite.FromBitmapPNG(Resources.font_61));
            _glyphs.Add('9', Sprite.FromBitmapPNG(Resources.font_62));

            _glyphs.Add('.', Sprite.FromBitmapPNG(Resources.font_63));
            _glyphs.Add(',', Sprite.FromBitmapPNG(Resources.font_64));
            _glyphs.Add('!', Sprite.FromBitmapPNG(Resources.font_65));
            _glyphs.Add('?', Sprite.FromBitmapPNG(Resources.font_66));
            _glyphs.Add(':', Sprite.FromBitmapPNG(Resources.font_67));
            _glyphs.Add(';', Sprite.FromBitmapPNG(Resources.font_68));
            _glyphs.Add('<', Sprite.FromBitmapPNG(Resources.font_69));
            _glyphs.Add('>', Sprite.FromBitmapPNG(Resources.font_70));
            _glyphs.Add('=', Sprite.FromBitmapPNG(Resources.font_71));
            _glyphs.Add('(', Sprite.FromBitmapPNG(Resources.font_72));
            _glyphs.Add(')', Sprite.FromBitmapPNG(Resources.font_73));
            _glyphs.Add('\'', Sprite.FromBitmapPNG(Resources.font_74));
            _glyphs.Add('%', Sprite.FromBitmapPNG(Resources.font_75));
            _glyphs.Add('$', Sprite.FromBitmapPNG(Resources.font_76));
            _glyphs.Add('&', Sprite.FromBitmapPNG(Resources.font_77));
            _glyphs.Add('#', Sprite.FromBitmapPNG(Resources.font_78));
            _glyphs.Add('"', Sprite.FromBitmapPNG(Resources.font_79));
            _glyphs.Add('-', Sprite.FromBitmapPNG(Resources.font_80));
            _glyphs.Add('+', Sprite.FromBitmapPNG(Resources.font_81));
            _glyphs.Add('_', Sprite.FromBitmapPNG(Resources.font_82));
            _glyphs.Add('{', Sprite.FromBitmapPNG(Resources.font_83));
            _glyphs.Add('}', Sprite.FromBitmapPNG(Resources.font_84));
            _glyphs.Add('*', Sprite.FromBitmapPNG(Resources.font_85));
            _glyphs.Add('/', Sprite.FromBitmapPNG(Resources.font_86));
            _glyphs.Add('`', Sprite.FromBitmapPNG(Resources.font_87));
            _glyphs.Add('^', Sprite.FromBitmapPNG(Resources.font_88));
            _glyphs.Add('|', Sprite.FromBitmapPNG(Resources.font_89));
            _glyphs.Add('~', Sprite.FromBitmapPNG(Resources.font_90));
            _glyphs.Add('[', Sprite.FromBitmapPNG(Resources.font_91));
            _glyphs.Add(']', Sprite.FromBitmapPNG(Resources.font_92));
            _glyphs.Add('@', Sprite.FromBitmapPNG(Resources.font_94));

            _nullGlyph = Sprite.FromBitmapPNG(Resources.font_93);
        }

        public static Font GetFont()
        {
            lock(_lock)
            {
                if (_instance == null)
                {
                    _instance = new Font();
                }
                return _instance;
            }
        }

        public Sprite GetGlyph(char ch)
        {
            Sprite glyph;
            if (!_glyphs.TryGetValue(ch, out glyph))
            {
                return _nullGlyph;
            }
            return glyph;
        }

        public uint GetGlyphWidth()
        {
            return _nullGlyph.Width;
        }

        public uint GetGlyphHeight()
        {
            return _nullGlyph.Height;
        }
    }
}
