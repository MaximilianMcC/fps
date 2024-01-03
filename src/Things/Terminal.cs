using System.Numerics;
using Raylib_cs;

class Terminal : Thing
{
	private Model terminalModel;
	private Model screenModel;
	private Model debugModel;
	private Texture2D terminalTexture;
	private Texture2D debugTexture;

	private RenderTexture2D screen;
	private float screenWidth = 400;
	private float screenHeight = 300;
	private float count = 0;

	public Terminal() : base("Terminal")
	{
		// Load in all of the required assets
		terminalModel = Raylib.LoadModel("./assets/terminal.obj");
		terminalTexture = Raylib.LoadTexture("./assets/terminal.png");
		Raylib.SetMaterialTexture(ref terminalModel, 0, MaterialMapIndex.MATERIAL_MAP_ALBEDO, ref terminalTexture);
		screenModel = Raylib.LoadModel("./assets/terminal-screen.obj");
		debugModel = Raylib.LoadModel("./assets/wall.obj");

		//! debug
		debugTexture = Raylib.LoadTexture("./assets/dev-texture-128.png");

		// Make the render texture for the screen
		screen = Raylib.LoadRenderTexture((int)screenWidth, (int)screenHeight);
	}

	public override void Update()
	{
		if (Raylib.IsKeyPressed(KeyboardKey.KEY_KP_ADD)) count++;
		else if (Raylib.IsKeyPressed(KeyboardKey.KEY_KP_SUBTRACT)) count--;
	}

	// TODO: Flip 3d order and screen order so the screen is drawn first, then the 3d models
	public override void Render()
	{
		// Draw everything in the game world thingy
		// Raylib.DrawModelEx(terminalModel, Position, Rotation, 0f, Scale, Color.WHITE);
		// TODO: Don't use -Scale.Y
		//! hack. Do properly
		Raylib.DrawModelEx(screenModel, Position, Rotation, 0f, Scale, Color.WHITE);
		Raylib.DrawModelEx(debugModel, Position, Rotation, 0f, Scale, Color.WHITE);

		// Draw everything to the screen
		Raylib.BeginTextureMode(screen);
		Raylib.ClearBackground(Color.BLACK);
		DrawScreen();
		Raylib.EndTextureMode();

		// Flip the render texture on the Y
		//? This is because openGl doesn't have 0,0 at top-left (kys)
		Texture2D screenTexture = screen.texture;
		Image screenImage = Raylib.LoadImageFromTexture(screenTexture);
		Raylib.ImageFlipVertical(ref screenImage);

		screenTexture = Raylib.LoadTextureFromImage(screenImage);
		Raylib.UnloadImage(screenImage);
		

		// Update the screen with the render texture
		Raylib.SetMaterialTexture(ref screenModel, 0, MaterialMapIndex.MATERIAL_MAP_ALBEDO, ref screenTexture);
		Raylib.SetMaterialTexture(ref debugModel, 0, MaterialMapIndex.MATERIAL_MAP_ALBEDO, ref screen.texture);

		//! do rn because this is extremely bad for performance
		// TODO: Unload screenTexture
	}

	public override void Cleanup()
	{
		Raylib.UnloadModel(terminalModel);
		Raylib.UnloadTexture(terminalTexture);

		Raylib.UnloadModel(screenModel);
		Raylib.UnloadRenderTexture(screen);
	}





	private void DrawScreen()
	{
		Raylib.DrawText($"Count: {count}", 10, 10, 50, Color.GREEN);
	}
}