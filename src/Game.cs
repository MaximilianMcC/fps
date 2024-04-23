using System.Numerics;
using Raylib_cs;

class Game
{

	public static void Run()
	{
		Raylib.SetTraceLogLevel(TraceLogLevel.Warning);

		// Setup raylib
		Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
		Raylib.SetConfigFlags(ConfigFlags.AlwaysRunWindow);
		Raylib.InitWindow(854, 480, "Computer video game.");

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
		// Start/initialize everything
		Player.Start(500, 300);
	}

	private static void Update()
	{
		// Update everything
		Player.Update();
	}

	private static void Render()
	{
		// Clear the screen
		Raylib.ClearBackground(Color.Magenta);

		// ---------------------------------------------------
		Raylib.BeginTextureMode(Player.CameraOutput);
		Raylib.BeginMode3D(Player.Camera);

		//! debug
		Raylib.ClearBackground(Color.Green);

		// Render all the 3D stuff
		Raylib.DrawGrid(10, 1);

		Raylib.DrawCube(new Vector3(0, 1, 5), 1, 1, 1, Color.Red);
		Raylib.DrawCube(new Vector3(0, 1, -5), 1, 1, 1, Color.Red);
		Raylib.DrawCube(new Vector3(5, 1, 0), 1, 1, 1, Color.Red);
		Raylib.DrawCube(new Vector3(-5, 1, 0), 1, 1, 1, Color.Red);

		Raylib.EndMode3D();
		Raylib.EndTextureMode();
		// ---------------------------------------------------

		// Actually put everything on the screen
		Player.Render();
	}

	private static void CleanUp()
	{
		Player.CleanUp();

		// Close all the raylib stuff
		//! Make sure this is always done very last
		Raylib.CloseWindow();
	}
}