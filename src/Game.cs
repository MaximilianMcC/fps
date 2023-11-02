using System.Numerics;
using Raylib_cs;

class Game
{
	// Game rending settings and stuff
	// TODO: Make 4:3
	// TODO: Mess around with resolution until get something good
	private const int GAME_WIDTH = 1920;
	private const int GAME_HEIGHT = 1080;
	private RenderTexture2D display;

	// Things
	Player player;
	Model monitor;

	// Other things
	private bool paused = false;


	public void Run()
	{
		Console.WriteLine("Run");

		// Make raylib window
		Raylib.InitWindow(GAME_WIDTH, GAME_HEIGHT, "fps gaem");
		Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
		Raylib.SetTargetFPS(144);
		Raylib.SetExitKey(KeyboardKey.KEY_NULL);
		Raylib.DisableCursor();

		// Create the render texture for drawing everything on so we can rescale game
		display = Raylib.LoadRenderTexture(GAME_WIDTH, GAME_HEIGHT);


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

		// Load a model
		Model model = Raylib.LoadModel("./assets/monitor.obj");
		
		// Add the texture
		Texture2D texture = Raylib.LoadTexture("./assets/monitor.png");
		Raylib.SetMaterialTexture(ref model, 0, MaterialMapIndex.MATERIAL_MAP_ALBEDO, ref texture);
		
		monitor = model;
	}

	private void Update()
	{
		// Get delta time
		float deltaTime = Raylib.GetFrameTime();

		// Update stuff that can't be paused
		Debug.Terminal.Update();

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



		// Raylib.DrawModel(monitor, Vector3.Zero, 1f, Color.WHITE);
		Raylib.DrawModel(monitor, Vector3.Zero, 1f, Color.WHITE);
		Raylib.DrawCube(new Vector3(-5, 0, 0), 1.0f, 1.0f, 1.0f, Color.RED); // Draw a red cube at the center
		Raylib.DrawCubeWires(new Vector3(-5, 0, 0), 1.1f, 1.1f, 1.1f, Color.DARKGREEN); // Draw a red cube at the center

		

		// Draw 2D stuff
		Raylib.EndMode3D();
		Debug.Terminal.Render();

		// Stop drawing everything
		Raylib.EndDrawing();
	}


}