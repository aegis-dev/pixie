namespace Pixie
{
    public static class Utils
    {
        /// <summary>
        /// Returns text length in pixels. Doesn't work with multiline text.
        /// </summary>
        /// <param name="text">Text to measure length of</param>
        /// <returns>Text length</returns>
        public static int GetTextLengthInPixels(in string text)
        {
            // Char glyph width is 3 pixels and +1 for space between chars
            return (text.Length - 1) * 4;
        }
    }
}
