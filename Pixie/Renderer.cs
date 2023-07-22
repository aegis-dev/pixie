using Pixie.Internal;
using Pixie.Properties;
using Silk.NET.OpenGL;
using System.Drawing;
using Texture = Pixie.Internal.Texture;

namespace Pixie
{
    public class Renderer
    {
        public long CameraX { get; set; }
        public long CameraY { get; set; }
        public Brightness Brightness { get; set; }

        private readonly DataManager _dataManager;
        private readonly long _cameraOriginX;
        private readonly long _cameraOriginY;
        private byte _backgroundColorIndex;

        private Texture _paletteTexture;
        private FrameBuffer _frameBuffer;
        private GLRenderer _glRenderer;

        private const int UniformPaletteSizeLocation = 3;
        private const int UniformBackgroundColorIndexLocation = 4;

        internal Renderer(DataManager dataManager, FrameBuffer frameBuffer, GLRenderer gLRenderer)
        {
            _frameBuffer = frameBuffer;
            _dataManager = dataManager;
            _glRenderer = gLRenderer;

            CameraX = 0;
            CameraY = 0;
            Brightness = Brightness.Normal;
            _paletteTexture = LoadPaletteTexture(dataManager);
            _cameraOriginX = frameBuffer.Width / 2;
            _cameraOriginY = frameBuffer.Height / 2;
            _backgroundColorIndex = 1;


            // Init some uniforms that needs to be set once
            _glRenderer.Begin();
            _glRenderer.SetUniformInt(UniformPaletteSizeLocation, (int)_paletteTexture.Width);
            _glRenderer.End();
        }

        internal void RenderToScreen()
        {
            Texture bufferTexture = _frameBuffer.RenderToTexture(_dataManager);

            //TODO: not needed, right?
            _glRenderer.ClearScreen();

            _glRenderer.Begin();
            _glRenderer.SetUniformInt(UniformBackgroundColorIndexLocation, _backgroundColorIndex);
            _glRenderer.Render(_frameBuffer.FrameBufferQuad, bufferTexture, _paletteTexture);
            _glRenderer.End();
        }

        internal Texture LoadPaletteTexture(in DataManager dataManager)
        {
            List<Color> palette = Palette.GetPalette();

            List<byte> paletteTextureData = new List<byte>();
            foreach(Color color in palette)
            {
                paletteTextureData.Add(color.R);
                paletteTextureData.Add(color.G);
                paletteTextureData.Add(color.B);
            }

            return dataManager.LoadTexture(paletteTextureData.ToArray(), (uint)palette.Count, 1, ImageMode.RGB, TextureUnit.Texture1);
        }

        public void ClearFrameBuffer()
        {
            _frameBuffer.Clear();
        }

        public void SetBackgroundColor(PixieColor color)
        {
            _backgroundColorIndex = (byte)color;
        }

        public void SetBackgroundColor(PixieColor color, Brightness brightness)
        {
            _backgroundColorIndex = (byte)((byte)color + ((byte)brightness * (byte)PixieColor.Count));
        }

        public void SetBackgroundColor(byte colorIndex)
        {
            _backgroundColorIndex = colorIndex;
        }

        public byte GetBackgroundColor()
        {
            return _backgroundColorIndex;
        }

        public void Point(long x, long y, byte colorIndex)
        {
            long xReal = x - CameraX + _cameraOriginX;
            long yReal = y - CameraY + _cameraOriginY;
            if (xReal < 0 || yReal < 0)
            {
                return;
            }

            _frameBuffer.SetPixel((uint)xReal, (uint)yReal, colorIndex);
        }

        public void Point(long x, long y, PixieColor color)
        {
            Point(x, y, (byte)color);
        }

        public void Point(long x, long y, PixieColor color, Brightness brightness)
        {
            Point(x, y, (byte)((byte)color + ((byte)brightness * (byte)PixieColor.Count)));
        }

        public void Point(long x, long y, PixieColor color, List<PointLight> lights)
        {
            Brightness brightness = Brightness;
            foreach (PointLight light in lights)
            {
                if (brightness == Brightness.Normal)
                {
                    Point(x, y, color);
                    break;
                }
                Brightness tempBrightness = light.GetBrightness(x, y);
                if (BrightnessUtils.IsLighter(tempBrightness, brightness))
                {
                    brightness = tempBrightness;
                }
            }

            Point(x, y, color, brightness);
        }

        // TODO: finish other drawing utils
    }
}
