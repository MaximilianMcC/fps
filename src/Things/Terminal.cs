using System.Numerics;
using Raylib_cs;

class Terminal : Thing
{
	// 3D stuff
	private Model terminalModel;
	private Model screenModel;
	private Texture2D terminalTexture;

	// Displaying screen stuff
	private RenderTexture2D screen;
	private RenderTexture2D displayScreen; //? used to flip the image
	private Font font;
	private float screenWidth = 400;
	private float screenHeight = 300;
	private readonly Color backgroundColor = new Color(34, 8, 11, 255);
	private readonly Color foregroundColor = new Color(179, 100, 0, 255);

	// Screen stuff
	private string output;
	private string prompt = ">";
	private string input = "";
	private int caretIndex = 0;



	public Terminal() : base("Terminal")
	{
		// Load in all of the required assets
		terminalModel = Raylib.LoadModel("./assets/terminal.obj");
		terminalTexture = Raylib.LoadTexture("./assets/terminal.png");
		screenModel = Raylib.LoadModel("./assets/terminal-screen.obj");

		// Add the textures to the terminal
		Raylib.SetMaterialTexture(ref terminalModel, 0, MaterialMapIndex.MATERIAL_MAP_ALBEDO, ref terminalTexture);

		// Make the render textures for the screen
		screen = Raylib.LoadRenderTexture((int)screenWidth, (int)screenHeight);
		displayScreen = Raylib.LoadRenderTexture((int)screenWidth, (int)screenHeight);

		// Load the terminal font
		// TODO: Do in an asset manager 
		// TODO: Decide on a font
		font = Raylib.LoadFont("./assets/font/BigBlue_Terminal_437TT.TTF");
	}

	public override void Start()
	{
		// Add the default starting text thingy to the terminal
		output += "Type 'help' for a list of commands...\n\n";

		//! debug
		input = "echo Hello, world!";
	}

	public override void Update()
	{
		// if (Raylib.IsKeyPressed(KeyboardKey.KEY_KP_ADD)) count++;
		// else if (Raylib.IsKeyPressed(KeyboardKey.KEY_KP_SUBTRACT)) count--;
	}

	// TODO: Flip 3d order and screen order so the screen is drawn first, then the 3d models
	public override void Render()
	{
		// Draw everything in the game world thingy
		Raylib.DrawModelEx(terminalModel, Position, Rotation, 0f, Scale, Color.WHITE);
		Raylib.DrawModelEx(screenModel, Position, Rotation, 0f, Scale, Color.WHITE);

		// Draw everything to the screen
		Raylib.BeginTextureMode(screen);
		Raylib.ClearBackground(Color.MAROON);
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





	// TODO: Add a scanline shader or something
	private void DrawScreen()
	{
		// Define constants for drawing
		// TODO: Put some of this stuff up where the colors are
		const float padding = 10;
		const float padding2 = padding * 2;
		const float paddingHalf = padding / 2;
		const float thickness = 3f;
		float fontSize = 10f;
		float fontSpacing = fontSize / 10;

		// Draw the background
		Raylib.ClearBackground(backgroundColor);

		// Add a border around everything
		// TODO: Use box-drawing characters
		Raylib.DrawRectangleLinesEx(new Rectangle(padding, padding, screenWidth - padding2, screenHeight - padding2), thickness, foregroundColor);

		// Draw the output stream thingy
		// TODO: Do some maths to make it scroll as characters are added
		Raylib.DrawTextEx(font, output, new Vector2(padding2, padding2), fontSize, fontSpacing, foregroundColor);

		// Draw a line separating the input section
		// TODO: Could hardcode all these values, but doing dynamically for now
		//! Will break if font isn't monospace
		float y = screenHeight - padding2 - fontSize;
		Raylib.DrawLineEx(new Vector2(padding, y), new Vector2(screenWidth - padding, y), thickness, foregroundColor);

		// Draw the input text prompt thing
		// TODO: Make the caret blink
		y += paddingHalf;
		Raylib.DrawTextEx(font, (prompt + input), new Vector2(padding2, y), fontSize, fontSpacing, foregroundColor);

		// Draw the caret
		// TODO: Toggle for box-shape and line-shape caret for if people don't know how to use box caret (noob)
		//! Will break if font isn't monospace
		// TODO: Invert color where caret is
	}
}