using Raylib_cs;

class Program
{
	public static void Main(string[] args)
	{


		// Make raylib window
		Raylib.InitWindow(800, 400, "fps gaem");


		// Main game loop
		while (!Raylib.WindowShouldClose())
		{
			// Update everything
			Update();

			// Draw everything
			Render();
		}
	}

	private static void Update()
	{

	}

	private static void Render()
	{
		Raylib.BeginDrawing();
		Raylib.ClearBackground(Color.MAGENTA);

		Raylib.DrawText("fps game", 10, 10, 10, Color.BLACK);

		Raylib.EndDrawing();
	}
}