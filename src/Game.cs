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
	Model monitor;
	Model wall;

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
	}



	private void Start()
	{
		player = new Player();

		// Load a model and texture
		Model model = Raylib.LoadModel("./assets/monitor.obj");
		Texture2D texture = Raylib.LoadTexture("./assets/monitor.png");
		Raylib.SetMaterialTexture(ref model, 0, MaterialMapIndex.MATERIAL_MAP_ALBEDO, ref texture);
		monitor = model;

		// Load wall model
		// TODO: Make asset/resource manager
		//! Don't hardcode
		wall = Raylib.LoadModel("./assets/wall.obj");
		Texture2D wallTexture = Raylib.LoadTexture("./assets/dev-texture-128.png");
		Raylib.SetMaterialTexture(ref wall, 0, MaterialMapIndex.MATERIAL_MAP_ALBEDO, ref wallTexture);
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
	}

	// TODO: Render in 4:3 squished
	private void Render()
	{
		// Draw 3D stuff
		Raylib.BeginDrawing();
		Raylib.BeginMode3D(player.Camera);
		Raylib.ClearBackground(Color.MAGENTA);


		// Raylib.DrawModel(monitor, new Vector3(5, 0, 3), 1f, Color.WHITE);
		// Raylib.DrawCube(new Vector3(-5, 0, 0), 1.0f, 1.0f, 1.0f, Color.RED); // Draw a red cube at the center
		// Raylib.DrawCubeWires(new Vector3(-5, 0, 0), 1.1f, 1.1f, 1.1f, Color.DARKGREEN); // Draw a red cube at the center
		
		Raylib.DrawGrid(10, 1);
		
		// Draw crappy map thing
		//! Bad to hardcode map
		// TODO: Remove and use map editor
		Raylib.DrawModelEx(wall, new Vector3(0, 0, 0), new Vector3(0, 0, 0), 0, new Vector3(1, 1, 1), Color.WHITE);
		Raylib.DrawModelEx(wall, new Vector3(0, 1, 0), new Vector3(0, 0, 0), 0, new Vector3(1, 1, 1), Color.WHITE);

		Raylib.DrawModelEx(wall, new Vector3(1, 0, 0), new Vector3(0, 0, 0), 0, new Vector3(1, 1, 1), Color.WHITE);
		Raylib.DrawModelEx(wall, new Vector3(1, 1, 0), new Vector3(0, 0, 0), 0, new Vector3(1, 1, 1), Color.WHITE);

		Raylib.DrawModelEx(wall, new Vector3(2, 0, 0), new Vector3(0, -90, 0), -90, new Vector3(1, 1, 1), Color.BLUE);
		Raylib.DrawModelEx(wall, new Vector3(2, 1, 0), new Vector3(0, -90, 0), -90, new Vector3(1, 1, 1), Color.BLUE);


		// Draw 2D stuff
		Raylib.EndMode3D();
		Debug.Terminal.Render();
		Debug.FPSGraph.Render();
		HUD.Render();

		if (Debug.DebugMode) Raylib.DrawText(player.ToString(), 30, 450, 30, Color.WHITE);

		// Stop drawing everything
		Raylib.EndDrawing();
	}


}