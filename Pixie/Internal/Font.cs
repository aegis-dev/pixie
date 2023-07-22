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
            _glyphs.Add('a', Sprite.FromIndexedPNG(Resources.font_1));
            _glyphs.Add('b', Sprite.FromIndexedPNG(Resources.font_2));
            _glyphs.Add('c', Sprite.FromIndexedPNG(Resources.font_3));
            _glyphs.Add('d', Sprite.FromIndexedPNG(Resources.font_4));
            _glyphs.Add('e', Sprite.FromIndexedPNG(Resources.font_5));
            _glyphs.Add('f', Sprite.FromIndexedPNG(Resources.font_6));
            _glyphs.Add('g', Sprite.FromIndexedPNG(Resources.font_7));
            _glyphs.Add('h', Sprite.FromIndexedPNG(Resources.font_8));
            _glyphs.Add('i', Sprite.FromIndexedPNG(Resources.font_9));
            _glyphs.Add('j', Sprite.FromIndexedPNG(Resources.font_10));
            _glyphs.Add('k', Sprite.FromIndexedPNG(Resources.font_11));
            _glyphs.Add('l', Sprite.FromIndexedPNG(Resources.font_12));
            _glyphs.Add('m', Sprite.FromIndexedPNG(Resources.font_13));
            _glyphs.Add('n', Sprite.FromIndexedPNG(Resources.font_14));
            _glyphs.Add('o', Sprite.FromIndexedPNG(Resources.font_15));
            _glyphs.Add('p', Sprite.FromIndexedPNG(Resources.font_16));
            _glyphs.Add('q', Sprite.FromIndexedPNG(Resources.font_17));
            _glyphs.Add('r', Sprite.FromIndexedPNG(Resources.font_18));
            _glyphs.Add('s', Sprite.FromIndexedPNG(Resources.font_19));
            _glyphs.Add('t', Sprite.FromIndexedPNG(Resources.font_20));
            _glyphs.Add('u', Sprite.FromIndexedPNG(Resources.font_21));
            _glyphs.Add('v', Sprite.FromIndexedPNG(Resources.font_22));
            _glyphs.Add('w', Sprite.FromIndexedPNG(Resources.font_23));
            _glyphs.Add('x', Sprite.FromIndexedPNG(Resources.font_24));
            _glyphs.Add('y', Sprite.FromIndexedPNG(Resources.font_25));
            _glyphs.Add('z', Sprite.FromIndexedPNG(Resources.font_26));

            _glyphs.Add('A', Sprite.FromIndexedPNG(Resources.font_27));
            _glyphs.Add('B', Sprite.FromIndexedPNG(Resources.font_28));
            _glyphs.Add('C', Sprite.FromIndexedPNG(Resources.font_29));
            _glyphs.Add('D', Sprite.FromIndexedPNG(Resources.font_30));
            _glyphs.Add('E', Sprite.FromIndexedPNG(Resources.font_31));
            _glyphs.Add('F', Sprite.FromIndexedPNG(Resources.font_32));
            _glyphs.Add('G', Sprite.FromIndexedPNG(Resources.font_33));
            _glyphs.Add('H', Sprite.FromIndexedPNG(Resources.font_34));
            _glyphs.Add('I', Sprite.FromIndexedPNG(Resources.font_35));
            _glyphs.Add('J', Sprite.FromIndexedPNG(Resources.font_36));
            _glyphs.Add('K', Sprite.FromIndexedPNG(Resources.font_37));
            _glyphs.Add('L', Sprite.FromIndexedPNG(Resources.font_38));
            _glyphs.Add('M', Sprite.FromIndexedPNG(Resources.font_39));
            _glyphs.Add('N', Sprite.FromIndexedPNG(Resources.font_40));
            _glyphs.Add('O', Sprite.FromIndexedPNG(Resources.font_41));
            _glyphs.Add('P', Sprite.FromIndexedPNG(Resources.font_42));
            _glyphs.Add('Q', Sprite.FromIndexedPNG(Resources.font_43));
            _glyphs.Add('R', Sprite.FromIndexedPNG(Resources.font_44));
            _glyphs.Add('S', Sprite.FromIndexedPNG(Resources.font_45));
            _glyphs.Add('T', Sprite.FromIndexedPNG(Resources.font_46));
            _glyphs.Add('U', Sprite.FromIndexedPNG(Resources.font_47));
            _glyphs.Add('V', Sprite.FromIndexedPNG(Resources.font_48));
            _glyphs.Add('W', Sprite.FromIndexedPNG(Resources.font_49));
            _glyphs.Add('X', Sprite.FromIndexedPNG(Resources.font_50));
            _glyphs.Add('Y', Sprite.FromIndexedPNG(Resources.font_51));
            _glyphs.Add('Z', Sprite.FromIndexedPNG(Resources.font_52));

            _glyphs.Add('0', Sprite.FromIndexedPNG(Resources.font_53));
            _glyphs.Add('1', Sprite.FromIndexedPNG(Resources.font_54));
            _glyphs.Add('2', Sprite.FromIndexedPNG(Resources.font_55));
            _glyphs.Add('3', Sprite.FromIndexedPNG(Resources.font_56));
            _glyphs.Add('4', Sprite.FromIndexedPNG(Resources.font_57));
            _glyphs.Add('5', Sprite.FromIndexedPNG(Resources.font_58));
            _glyphs.Add('6', Sprite.FromIndexedPNG(Resources.font_59));
            _glyphs.Add('7', Sprite.FromIndexedPNG(Resources.font_60));
            _glyphs.Add('8', Sprite.FromIndexedPNG(Resources.font_61));
            _glyphs.Add('9', Sprite.FromIndexedPNG(Resources.font_62));

            _glyphs.Add('.', Sprite.FromIndexedPNG(Resources.font_63));
            _glyphs.Add(',', Sprite.FromIndexedPNG(Resources.font_64));
            _glyphs.Add('!', Sprite.FromIndexedPNG(Resources.font_65));
            _glyphs.Add('?', Sprite.FromIndexedPNG(Resources.font_66));
            _glyphs.Add(':', Sprite.FromIndexedPNG(Resources.font_67));
            _glyphs.Add(';', Sprite.FromIndexedPNG(Resources.font_68));
            _glyphs.Add('<', Sprite.FromIndexedPNG(Resources.font_69));
            _glyphs.Add('>', Sprite.FromIndexedPNG(Resources.font_70));
            _glyphs.Add('=', Sprite.FromIndexedPNG(Resources.font_71));
            _glyphs.Add('(', Sprite.FromIndexedPNG(Resources.font_72));
            _glyphs.Add(')', Sprite.FromIndexedPNG(Resources.font_73));
            _glyphs.Add('\'', Sprite.FromIndexedPNG(Resources.font_74));
            _glyphs.Add('%', Sprite.FromIndexedPNG(Resources.font_75));
            _glyphs.Add('$', Sprite.FromIndexedPNG(Resources.font_76));
            _glyphs.Add('&', Sprite.FromIndexedPNG(Resources.font_77));
            _glyphs.Add('#', Sprite.FromIndexedPNG(Resources.font_78));
            _glyphs.Add('"', Sprite.FromIndexedPNG(Resources.font_79));
            _glyphs.Add('-', Sprite.FromIndexedPNG(Resources.font_80));
            _glyphs.Add('+', Sprite.FromIndexedPNG(Resources.font_81));
            _glyphs.Add('_', Sprite.FromIndexedPNG(Resources.font_82));
            _glyphs.Add('{', Sprite.FromIndexedPNG(Resources.font_83));
            _glyphs.Add('}', Sprite.FromIndexedPNG(Resources.font_84));
            _glyphs.Add('*', Sprite.FromIndexedPNG(Resources.font_85));
            _glyphs.Add('/', Sprite.FromIndexedPNG(Resources.font_86));
            _glyphs.Add('`', Sprite.FromIndexedPNG(Resources.font_87));
            _glyphs.Add('^', Sprite.FromIndexedPNG(Resources.font_88));
            _glyphs.Add('|', Sprite.FromIndexedPNG(Resources.font_89));
            _glyphs.Add('~', Sprite.FromIndexedPNG(Resources.font_90));
            _glyphs.Add('[', Sprite.FromIndexedPNG(Resources.font_91));
            _glyphs.Add(']', Sprite.FromIndexedPNG(Resources.font_92));
            _glyphs.Add('@', Sprite.FromIndexedPNG(Resources.font_94));

            _nullGlyph = Sprite.FromIndexedPNG(Resources.font_93);
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
