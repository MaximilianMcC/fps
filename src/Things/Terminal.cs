using System.Numerics;
using Raylib_cs;

class Terminal : Thing
{
	private Model terminalModel;
	private Model screenModel;
	private Texture2D terminalTexture;

	private RenderTexture2D screen;
	private RenderTexture2D displayScreen; //? used to flip the image
	private float screenWidth = 400;
	private float screenHeight = 300;
	private float count = 0;

	public Terminal() : base("Terminal")
	{
		// Load in all of the required assets
		terminalModel = Raylib.LoadModel("./assets/terminal.obj");
		terminalTexture = Raylib.LoadTexture("./assets/terminal.png");
		screenModel = Raylib.LoadModel("./assets/terminal-screen.obj");

		Raylib.SetMaterialTexture(ref terminalModel, 0, MaterialMapIndex.MATERIAL_MAP_ALBEDO, ref terminalTexture);

		// Make the render textures for the screen
		screen = Raylib.LoadRenderTexture((int)screenWidth, (int)screenHeight);
		displayScreen = Raylib.LoadRenderTexture((int)screenWidth, (int)screenHeight);
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
		Raylib.DrawModelEx(terminalModel, Position, Rotation, 0f, Scale, Color.WHITE);
		Raylib.DrawModelEx(screenModel, Position, Rotation, 0f, Scale, Color.WHITE);

		// Draw everything to the screen
		Raylib.BeginTextureMode(screen);
		Raylib.ClearBackground(Color.BLACK);
		DrawScreen();
		Raylib.EndTextureMode();

		// Draw everything onto the display screen so that it can be rendered not upside-down
		//? This is because OpenGL doesn't have 0,0 at top-left (kys)
		Raylib.BeginTextureMode(displayScreen);
		Raylib.ClearBackground(Color.BLUE);
		Raylib.DrawTexture(screen.texture, 0, 0, Color.WHITE);
		Raylib.EndTextureMode();

		// Update the screen with the render texture
		Raylib.SetMaterialTexture(ref screenModel, 0, MaterialMapIndex.MATERIAL_MAP_ALBEDO, ref displayScreen.texture);
	}

	public override void Cleanup()
	{
		Raylib.UnloadModel(terminalModel);
		Raylib.UnloadTexture(terminalTexture);
		Raylib.UnloadModel(screenModel);

		Raylib.UnloadRenderTexture(screen);
		Raylib.UnloadRenderTexture(displayScreen);
	}





	private void DrawScreen()
	{
		Raylib.DrawText($"Count: {count}", 10, 10, 50, Color.GREEN);
	}
}