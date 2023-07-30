using Pixie;
using Pixie.UI;
using PixieExample.Properties;
using Silk.NET.Input;

namespace PixieExample
{
    internal class TestScene : Scene
    {
        private Random _random = new Random();
        private SpriteSheet _spriteSheet = SpriteSheet.FromBitmapPNG(Resources.test, 8, 8);

        private Canvas _canvas;

        public override void OnStart(in Renderer renderer)
        {
            renderer.CameraX = 64;
            renderer.CameraY = 64;

            _canvas = new Canvas(renderer);

            UIQuad centerQuad = new UIQuad(64 - 8, 64 - 8, 16, 16, PixieColor.Red);
            _canvas.AddChildElement(centerQuad);

            UIQuad left = new UIQuad(-5,0, 16, 16, PixieColor.Green);
            centerQuad.AttachNeighbourElementToLeft(left);

            UIQuad right = new UIQuad(5, 0, 16, 16, PixieColor.Green);
            centerQuad.AttachNeighbourElementToRight(right);

            UIQuad top = new UIQuad(0, 5, 16, 16, PixieColor.Green);
            centerQuad.AttachNeighbourElementAbove(top);

            UIQuad bottom = new UIQuad(0, -5, 16, 16, PixieColor.Green);
            centerQuad.AttachNeighbourElementBelow(bottom);


            UIQuad leftCorner = new UIQuad(0, (int)_canvas.Height - 16, 16, 16, PixieColor.Red);
            _canvas.AddChildElement(leftCorner);
            UIQuad rightCorner = new UIQuad((int)_canvas.Width - 16, 0, 16, 16, PixieColor.Red);
            _canvas.AddChildElement(rightCorner);
        }

        public override Scene? OnUpdate(in GameState state, in Renderer renderer, in Input input, float deltaTime)
        {
            if (input.KeyPressed(Key.Escape))
            {
                state.ShutDown();
            }

            _canvas.OnUpdate(renderer, input, deltaTime);

            return null;
        }

        public override void OnRender(in Renderer renderer, in Input input, float deltaTime)
        {
            renderer.ClearFrameBuffer();

            //for (int x = 1; x <= 32; ++x)
            //{
            //    renderer.Point(x - 1, 0, (byte)x);
            //}

            //Sprite sprite = _spriteSheet.GetSpriteAtIndex(3);
            //renderer.Sprite(sprite, 0, 64, false);
            //renderer.Circle(0, 64, 3, PixieColor.Red);

            //renderer.Circle(renderer.CameraX + input.MouseX, renderer.CameraY + input.MouseY, 5, PixieColor.Red);

            ////renderer.CircleFilled(64, 64, 9, PixieColor.Red);
            ////renderer.RectangleFilled(10, 10, 32, 33, PixieColor.Red);
            ////renderer.RectangleFilled(64, 65, 32, 33, PixieColor.Green);

            ////renderer.Sprite(sprite, 64, 64, false);
            ////renderer.Sprite(sprite, 20, 20, true);

            //renderer.Text("Hello World!\nWelcome!", 64, 64, PixieColor.Red);

            //if (input.KeyPressed(Key.Space))
            //{
            //    Console.WriteLine("Space");
            //}

            _canvas.Render(renderer);
        }

        public override void OnDestroy()
        {

        }
    }
}
