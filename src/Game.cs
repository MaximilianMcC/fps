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

	// Other things
	private bool debugMode = true;


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
	}

	private void Update()
	{
		// Get delta time
		float deltaTime = Raylib.GetFrameTime();

		player.Update(deltaTime);

		// Debug thingy
		if (Raylib.IsKeyPressed(KeyboardKey.KEY_GRAVE))
		{
			// Toggle debug mode
			debugMode = !debugMode;
		}
		if (Raylib.IsKeyPressed(KeyboardKey.KEY_SLASH))
		{
			// copy debug info to clipboard
			Raylib.SetClipboardText($"Captured at {DateTime.Now}\n\nPosition: {player.Camera.position}\nTarget: {player.Camera.target}");
		}
	}

	// TODO: Render in 4:3 squished
	private void Render()
	{		
		// Raylib.BeginDrawing();

		// Draw 3D stuff
		Raylib.BeginDrawing();
		Raylib.BeginMode3D(player.Camera);
		Raylib.ClearBackground(Color.MAGENTA);

		Raylib.DrawCube(new Vector3(-5, 0, 0), 1.0f, 1.0f, 1.0f, Color.RED); // Draw a red cube at the center
		Raylib.DrawCubeWires(new Vector3(-5, 0, 0), 1.1f, 1.1f, 1.1f, Color.DARKGREEN); // Draw a red cube at the center


		Raylib.EndMode3D();

		Raylib.DrawFPS(10, 10);
		if (debugMode)
		{
			Raylib.DrawText($"Position: {player.Camera.position}\n Target: {player.Camera.target}", 10, 50, 25, Color.BLACK);
		}

		Raylib.EndDrawing();
	}


}