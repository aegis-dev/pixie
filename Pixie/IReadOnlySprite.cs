namespace Pixie
{
    public interface IReadOnlySprite
    {
        public uint Width { get; }
        public uint Height { get; }

        public PixieColor GetColorAt(uint x, uint y);
    }
}
