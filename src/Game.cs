using System.Numerics;
using Raylib_cs;

class Game
{
	// Game rending SettingsManager.Settings and stuff
	// TODO: Make 4:3
	// TODO: Mess around with resolution until get something good
	private static readonly int GAME_WIDTH = 1920;
	private static readonly int GAME_HEIGHT = 1080;

	// Render Texture
	private static RenderTexture2D cameraRenderTexture;

	//! debug
	private static Terminal terminal;

	// Things
	// TODO: Put in resource manager
	public static Player Player;

	// Other things
	private static bool paused = false;
	public static bool CanBePaused = true; //? Used for if escape key being used by another thingy

	public static void Run()
	{
		// Load all of the Settings
		SettingsManager.Load();

		// Make raylib window
		Raylib.InitWindow(GAME_WIDTH, GAME_HEIGHT, "fps gaem");
		Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
		Raylib.SetTargetFPS(SettingsManager.Settings.MaxFps);
		Raylib.InitAudioDevice();
		Raylib.SetExitKey(KeyboardKey.KEY_NULL);
		Raylib.DisableCursor();		

		// Setup the render texture
		cameraRenderTexture = Raylib.LoadRenderTexture(GAME_WIDTH, GAME_HEIGHT);

		// Game
		Start();
		while (!Raylib.WindowShouldClose())
		{
			Update();
			Render();
		}
		CleanUp();

		// Close the window when done
		Raylib.CloseAudioDevice();
		Raylib.CloseWindow();
	}



	private static void Start()
	{
		Player = new Player();

		// Add props
		//? Props are stuff with no logic, just simple 3D models.
		PropManager.Props.Add(new Prop("xyz", Vector3.Zero, Vector3.Zero, "./assets/xyz.obj", "./assets/xyz.png"));
		PropManager.StartThings();

		terminal = new Terminal();
		terminal.Position = new Vector3(0, 0, -1);
		terminal.Start();
	}

	private static void Update()
	{
		// Update stuff that can't be paused
		Debug.Terminal.Update();
		Debug.FPSGraph.Update();

		// Check for if the game is paused
		if (Raylib.IsKeyPressed(SettingsManager.Settings.Pause) && CanBePaused) 
		{
			// Toggle paused
			paused = !paused;

			// Enable/disable cursor
			if (paused) Raylib.EnableCursor();
			else Raylib.DisableCursor();
		}
		if (paused) return;

		// Update stuff that can be paused
		Player.Update();
		PropManager.UpdateThings();

		terminal.Update();

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
	private static void Render()
	{
		// Draw 3D camera output to the render texture
		Raylib.BeginTextureMode(cameraRenderTexture);
		Raylib.BeginMode3D(Player.Camera);
		Raylib.ClearBackground(Color.MAGENTA);
		
		// Draw 3D stuff
		Raylib.DrawGrid(10, 1);
		PropManager.RenderThings();

		terminal.Render();

		// Finish drawing 3D stuff to render texture
		Raylib.EndMode3D();
		Raylib.EndTextureMode();

		// Draw 2D stuff
		Raylib.BeginDrawing();
		Raylib.ClearBackground(Color.GREEN);

		// Draw the camera output
		Rectangle source = new Rectangle(0f, 0f, cameraRenderTexture.texture.width, -cameraRenderTexture.texture.height);
		Rectangle destination = new Rectangle(0f, 0f, Raylib.GetScreenWidth(), Raylib.GetScreenHeight()); //TODO: Make dynamic
		Raylib.DrawTexturePro(cameraRenderTexture.texture, source, destination, Vector2.Zero, 0f, Color.WHITE);

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

		if (Debug.DebugMode) Raylib.DrawText(Player.ToString(), 30, 450, 30, Color.WHITE);

		// Stop drawing everything
		Raylib.EndDrawing();
	}

	// When the game is ended/closed/quit
	private static void CleanUp()
	{
		PropManager.KillThings();
		terminal.Cleanup();
	}
}