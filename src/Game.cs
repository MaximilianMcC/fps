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
		Raylib.InitAudioDevice();
		Raylib.SetExitKey(KeyboardKey.Null);



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
		// Load the settings
		Settings.ReloadSettings();

		// Start/initialize everything
		Map.Load();
		Player.Start(500, 300);
	}

	private static void Update()
	{
		// Update everything
		Player.Update();
	}

	private static void Render()
	{
		//? No need to clear the screen because its done in the Map class
		//! Might not be the best idea to do it in the map class

		// ---------------------------------------------------
		Raylib.BeginTextureMode(Player.CameraOutput);
		Raylib.BeginMode3D(Player.Camera);

		// Render all the 3D stuff
		Map.Render();
		Raylib.DrawGrid(10, 1);

		Raylib.EndMode3D();
		Raylib.EndTextureMode();
		// ---------------------------------------------------

		// Actually put everything on the screen
		Player.Render();
	}

	private static void CleanUp()
	{
		Player.CleanUp();
		Map.CleanUp();

		// Close all the raylib stuff
		//! Make sure this is always done very last
		Raylib.CloseAudioDevice();
		Raylib.CloseWindow();
	}
}