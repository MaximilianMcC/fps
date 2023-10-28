using System.Numerics;
using Raylib_cs;

class Game
{

	// Game rending settings and stuff (4:3)
	// TODO: Maybe make resolution a bit bigger.
	private const int GAME_WIDTH = 250;
	private const int GAME_HEIGHT = 188;
	private RenderTexture2D display;



	public void Run()
	{
		// Make raylib window
		Raylib.InitWindow(GAME_WIDTH, GAME_HEIGHT, "fps gaem");
		Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
		Raylib.SetExitKey(KeyboardKey.KEY_NULL);
		Raylib.SetTargetFPS(144);

		// Create the render texture for drawing everything on so we can rescale game
		display = Raylib.LoadRenderTexture(GAME_WIDTH, GAME_HEIGHT);
		Raylib.SetWindowSize(1000, 750);
		

		// Main game loop
		while (!Raylib.WindowShouldClose())
		{
			// Update everything
			Update();

			// Draw everything
			Render();
		}
	}

	private void Update()
	{
		if (Raylib.IsKeyPressed(KeyboardKey.KEY_GRAVE))
		{
			Console.WriteLine("!");
		}
	}

	private void Render()
	{
		// Draw to the render texture
		Raylib.BeginTextureMode(display);
		Raylib.ClearBackground(Color.MAGENTA);

		// Temporary stuff for figuring out scale thingy
		Raylib.DrawText("fps gaem", 10, 10, 50, Color.BLACK);
		Raylib.DrawCircle(50, 23, 25, Color.BROWN);
		Raylib.DrawCircle(310, 140, 25, Color.BROWN);
		Raylib.DrawLine(0, 0, GAME_WIDTH, GAME_HEIGHT, Color.YELLOW);

		Raylib.EndTextureMode();

		// Actually draw to the screen
		Raylib.BeginDrawing();
		Raylib.DrawTexturePro(display.texture,
			new Rectangle(0, 0, GAME_WIDTH, -GAME_HEIGHT),
			new Rectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight()),
			new Vector2(0, 0),
			0, Color.WHITE
		);
		Raylib.EndDrawing();
	}

}