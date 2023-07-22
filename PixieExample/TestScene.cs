using Pixie;
using Silk.NET.Input;

namespace PixieExample
{
    internal class TestScene : Scene
    {
        private Random random = new Random();

        public override void OnStart(Renderer renderer)
        {
            renderer.CameraX = 64;
            renderer.CameraY = 64;
        }

        public override Scene OnUpdate(Renderer renderer, IKeyboard keyboard, IMouse mouse, float deltaTime)
        {
            return null;
        }

        public override void OnRender(Renderer renderer, float deltaTime)
        {
            renderer.ClearFrameBuffer();
            for (int i = 0; i < 100; ++i)
            {
                renderer.Point(random.Next(0, 127), random.Next(0, 127), (byte)random.Next(0, (int)PixieColor.Count));
            }
        }

        public override void OnDestroy()
        {

        }
    }
}
