using System.Numerics;
using Raylib_cs;

class Game
{
	// Game rending SettingsManager.Settings and stuff
	// TODO: Make 4:3
	// TODO: Mess around with resolution until get something good
	private const int GAME_WIDTH = 1920;
	private const int GAME_HEIGHT = 1080;

	// Things
	// TODO: Put in resource manager
	Player player;

	// Other things
	private bool paused = false;

	public void Run()
	{
		// Load all of the Settings
		SettingsManager.UpdateSettings();

		// Make raylib window
		Raylib.InitWindow(GAME_WIDTH, GAME_HEIGHT, "fps gaem");
		Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
		Raylib.SetTargetFPS(SettingsManager.Settings.MaxFps);
		Raylib.SetExitKey(KeyboardKey.KEY_NULL);
		Raylib.DisableCursor();

		//! F12 takes a screenshot and idk if there is a way to rebind

		// Game
		Start();
		while (!Raylib.WindowShouldClose())
		{
			Update();
			Render();
		}
		CleanUp();

		// Close the window when done
		Raylib.CloseWindow();
	}



	private void Start()
	{
		player = new Player();


		// Do some things
		ThingManager.Things.Add(new Thing("CRT Monitor", Vector3.Zero, Vector3.Zero, "./assets/crt.obj", new string[] { "./assets/crt.png" }));
		ThingManager.StartThings();
	}

	private void Update()
	{
		// Get delta time
		float deltaTime = Raylib.GetFrameTime();


		// Update stuff that can't be paused
		Debug.Terminal.Update();
		Debug.FPSGraph.Update();

		// Check for if the game is paused
		if (Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE)) 
		{
			// Toggle paused
			paused = !paused;

			// Enable/disable cursor
			if (paused) Raylib.EnableCursor();
			else Raylib.DisableCursor();
		}
		if (paused) return;

		// Update stuff that can be paused
		player.Update(deltaTime);
		ThingManager.UpdateThings();





		// TODO: Put somewhere else
		// Check for if they want to use full screen
		// TODO: Remove fullscreen and add borderless window
		if (Raylib.IsKeyPressed(KeyboardKey.KEY_F11))
		{
			// Set the width and height of the program to be the monitor size
			// so when switching to fullscreen the entire pc doesn't die
			int width = Raylib.GetMonitorWidth(Raylib.GetCurrentMonitor());
			int height = Raylib.GetMonitorHeight(Raylib.GetCurrentMonitor());
			Raylib.SetWindowSize(width, height);

			// Toggle fullscreen
			// TODO: Make it so that the window doesn't minimize when alt+tabbing
			Raylib.ToggleFullscreen();
		}

	}

	// TODO: Render in 4:3 squished
	private void Render()
	{
		// Draw 3D stuff
		Raylib.BeginDrawing();
		Raylib.BeginMode3D(player.Camera);
		Raylib.ClearBackground(Color.MAGENTA);
		
		Raylib.DrawGrid(10, 1);
		ThingManager.RenderThings();

		// Draw 2D stuff
		Raylib.EndMode3D();

		// If we're paused then darken everything by putting
		// a semi-transparent rectangle on the screen
		if (paused)
		{
			// TODO: Don't make a new color every frame
			Raylib.DrawRectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight(), new Color(32, 32, 32, 128));
		}

		Debug.Terminal.Render();
		Debug.FPSGraph.Render();
		HUD.Render();

		if (Debug.DebugMode) Raylib.DrawText(player.ToString(), 30, 450, 30, Color.WHITE);

		// Stop drawing everything
		Raylib.EndDrawing();
	}

	// When the game is ended/closed/quit
	private void CleanUp()
	{
		ThingManager.KillThings();
	}
}