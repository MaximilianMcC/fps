using Raylib_cs;

class Game
{
	public static void Run()
	{
		Raylib.SetTraceLogLevel(TraceLogLevel.Warning);

		// Setup raylib
		Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
		Raylib.SetConfigFlags(ConfigFlags.AlwaysRunWindow);
		Raylib.InitWindow(854, 480, "My very first video game");

		// Load everything
		Start();
		while (!Raylib.WindowShouldClose())
		{
			// Update everything
			Update();
			
			// Draw everything
			Raylib.BeginDrawing();
			Render();
			Raylib.EndDrawing();
		}
		// Unload everything
		CleanUp();
	}

	private static void Start()
	{

	}

	private static void Update()
	{

	}

	private static void Render()
	{
		// Clear the screen
		Raylib.ClearBackground(Color.Magenta);

		Raylib.DrawText("wow", 10, 10, 25, Color.White);
	}

	private static void CleanUp()
	{
		
		// Close all the raylib stuff
		//! Make sure this is always done very last
		Raylib.CloseWindow();
	}
}