using Pixie;
using PixieExample.Properties;
using Silk.NET.Input;

namespace PixieExample
{
    internal class TestScene : Scene
    {
        private Random random = new Random();
        private Sprite sprite = Sprite.FromIndexedPNG(Resources.test);

        public override void OnStart(Renderer renderer)
        {
            renderer.CameraX = 64;
            renderer.CameraY = 64;
        }

        public override Scene OnUpdate(Renderer renderer, Input input, float deltaTime)
        {
            return null;
        }

        public override void OnRender(Renderer renderer, Input input, float deltaTime)
        {
            renderer.ClearFrameBuffer();

            for (int x = 1; x <= 32; ++x)
            {
                renderer.Point(x - 1, 0, (byte)x);
            }

            renderer.Circle(renderer.CameraX + input.MouseX, renderer.CameraY + input.MouseY, 5, PixieColor.Red);

            //renderer.CircleFilled(64, 64, 9, PixieColor.Red);
            //renderer.RectangleFilled(10, 10, 32, 33, PixieColor.Red);
            //renderer.RectangleFilled(64, 65, 32, 33, PixieColor.Green);

            //renderer.Sprite(sprite, 64, 64, false);
            //renderer.Sprite(sprite, 20, 20, true);

            renderer.Text("Hello World!\nWelcome!", 64, 64, PixieColor.Red);

            if (input.KeyPressed(Key.Space))
            {
                Console.WriteLine("Space");
            }

            // TODO: input and spritesheets
        }

        public override void OnDestroy()
        {

        }
    }
}
