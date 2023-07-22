using Pixie;
using PixieExample;

GameContext context = new GameContext(128, 128, "Example", false);
context.RunGame(new TestScene());
