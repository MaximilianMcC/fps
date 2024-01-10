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
	private float screenWidth = 1280;
	private float screenHeight = 960;
	private readonly Color backgroundColor = new Color(34, 8, 11, 255);
	private readonly Color foregroundColor = new Color(179, 100, 0, 255);

	// Sounds
	private const int keyboardSoundsCount = 5;
	private Sound[] keyboardSounds;

	// Terminal input and output typing stuff
	private string output;
	private string prompt = ">";
	private string input = "";
	private int caretIndex = 0;
	private const double caretBlinkTime = 0.5f; //? seconds
	private double lastCaretBlinkTime;
	private bool caretCurrentlyVisible;
	private bool caretCanBlink = false;
	private int historyIndex;
	private List<string> history;
	private int autocompleteIndex;
	private string autocompleteInput = "";

	// Actual terminal data
	private List<ICommand> commands;

	// Interaction
	private bool beingUsed;
	private float UseRadius = 3f;



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
		// font = Raylib.LoadFont("./assets/font/BigBlue_Terminal_437TT.TTF");
		font = Raylib.LoadFont("./assets/font/bedstead-extended.otf");

		// Load in all of the sounds
		// TODO: Don't hardcode idk
		keyboardSounds = new Sound[keyboardSoundsCount];
		for (int i = 0; i < keyboardSoundsCount; i++)
		{
			Sound currentSound = Raylib.LoadSound($"./assets/audio/sound/key-stroke-{i + 1}.wav");
			keyboardSounds[i] = currentSound;
		}

		// Setup all of the commands
		//! Help command must be added last and separately
		commands = new List<ICommand>()
		{
			new EchoCommand(),
			new ClsCommand(),
			new ExitCommand()
		};
		commands.Add(new HelpCommand(commands));
	}

	public override void Start()
	{
		// Add the default starting text thingy to the terminal
		output += "Type 'help' for a list of commands...\n\n";
		lastCaretBlinkTime = Raylib.GetTime();
		history = new List<string>();

		// TODO: Lock input and snap camera when use terminal to stop moving around when typing
	}

	public override void Update()
	{
		// Handle the terminals keyboard and whatnot
		//? Keyboard is ran before CheckInteraction to stop player typing in the interact key
		if (beingUsed) Keyboard();

		// Check for if the player is using the terminal
		// TODO: Don't make it so that there is a specific button. If they are within range and looking at the correct area then put them in it
		CheckInteraction();
	}
 
	private void CheckInteraction()
	{
		// Check for if the player wants to exit
		// TODO: Stop the menu from popping up when pressing idk
		// TODO: Maybe have to use exit command or something to exit
		if (Raylib.IsKeyPressed(SettingsManager.Settings.Exit))
		{
			beingUsed = false;
			caretCanBlink = false;
			Game.CanBePaused = true;

			// Let the player move again
			Game.Player.CameraLocked = false;
			Game.Player.MovementLocked = false;
		}

		// Check for if the player wants to use the terminal
		// TODO: Make sure the player is looking at the terminal also
		if (Raylib.IsKeyPressed(SettingsManager.Settings.Use))
		{
			// Player must be within the use radius
			if (Utils.GetDistance(Position, Game.Player.Position) >= UseRadius) return;

			beingUsed = true;
			caretCanBlink = true;
			Game.CanBePaused = false;

			// Lock the player movement
			Game.Player.CameraLocked = true;
			Game.Player.MovementLocked = true;
		}
	}

	private void Keyboard()
	{
		bool keyPressed = false;

		// Check for if any special keys are pressed
		// TODO: Use switch
		//! Could be very bad for performance
		if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER) || Raylib.IsKeyPressed(KeyboardKey.KEY_KP_ENTER))
		{
			// Run the command if we typed something in it
			// then add it to history
			if (input != "")
			{
				RunCommand(input);

				history.Add(input);
				historyIndex++;
			}

			// Clear the input stuff for next time
			input = "";
			caretIndex = 0;
			autocompleteIndex = 0;
			autocompleteInput = "";
			keyPressed = true;
		}
		else if (Raylib.IsKeyPressed(KeyboardKey.KEY_BACKSPACE))
		{
			// Decrease the caret index and remove the character at the index
			if (caretIndex > 0)
			{
				caretIndex--;
				input = input.Remove(caretIndex, 1);
			}

			keyPressed = true;
		}
		else if (Raylib.IsKeyPressed(KeyboardKey.KEY_DELETE))
		{
			// Remove the character before the caret
			if (caretIndex < input.Length) input = input.Remove(caretIndex, 1);
			
			keyPressed = true;
		}
		else if (Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT))
		{
			// Move the caret backwards to the left
			caretIndex--;
			if (caretIndex < 0) caretIndex = 0;

			keyPressed = true;
		}
		else if (Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT))
		{
		// Move the caret forwards to the right
			caretIndex++;
			if (caretIndex > input.Length) caretIndex = input.Length;

			keyPressed = true;
		}
		else if (Raylib.IsKeyPressed(KeyboardKey.KEY_END))
		{
			// Go to the end of the input
			caretIndex = input.Length;

			keyPressed = true;
		}
		else if (Raylib.IsKeyPressed(KeyboardKey.KEY_HOME))
		{
			// Go to the beginning of the input
			caretIndex = 0;

			keyPressed = true;
		}
		else if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP))
		{
			// Decrease the index to go up the list
			historyIndex--;
			if (historyIndex < 0) historyIndex = 0;

			// Set the index to the current history thingy
			input = history[historyIndex];
			caretIndex = input.Length;

			keyPressed = true;
		}
		else if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN))
		{
			// Increase the index to go down the list
			historyIndex++;
			if (historyIndex >= history.Count) historyIndex = history.Count - 1;

			// Set the index to the current history thingy
			input = history[historyIndex];
			caretIndex = input.Length;

			keyPressed = true;
		}
		else if (Raylib.IsKeyPressed(KeyboardKey.KEY_TAB))
		{
			// Check for if we're typing in a command, or arguments.
			// Autocomplete can only work on commands.
			if (input.Contains(" ")) return;

			// If we aren't looking at autocomplete input then
			// preserve the current input so we don't use the previous
			// autocompleted text to search for the next autocomplete
			if (autocompleteInput == "") autocompleteInput = input;

			// Get all of the commands that start with the input. If there are
			// none the return. Also play the sound now for incase we return
			ICommand[] autocompleteCommands = commands.Where(command => command.Name.StartsWith(autocompleteInput)).ToArray();
			keyPressed = true;
			if (autocompleteCommands.Length == 0) return;

			// Set the text to the input
			input = autocompleteCommands[autocompleteIndex].Name;
			caretIndex = autocompleteCommands[autocompleteIndex].Name.Length;

			// Increase the index for next time and loop it
			autocompleteIndex++;
			if (autocompleteIndex >= autocompleteCommands.Count()) autocompleteIndex = 0;

		}
		else
		{
			// Get the normal key stuff
			// TODO: Add emojis to font because this will pick up on them
			int keyboardInput = Raylib.GetCharPressed();
			if (keyboardInput == 0) return;

			// Add the input to the input at the caret position
			string newInput = char.ConvertFromUtf32(keyboardInput);
			input = input.Insert(caretIndex, newInput);

			// Update key stuff
			caretIndex++;
			keyPressed = true;
		}
		
		// Play a random keyboard sound when a key is pressed
		// TODO: Make keyboard model that moves when press keys. Each individual key doesn't have to move, but maybe it can just go down a bit or something
		if (keyPressed)
		{
			// Get a random sound
			//? idk if its better to use built in C# rand, or Raylib but imma use raylib
			Sound keySound = keyboardSounds[Raylib.GetRandomValue(0, keyboardSoundsCount - 1)];

			// Play the sound
			Raylib.PlaySound(keySound);
		}
	}

	public override void Render()
	{
		// Draw everything in the game world thingy
		// TODO: Flip 3d order and screen order so the screen is drawn first, then the 3d models
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

		for (int i = 0; i < keyboardSoundsCount; i++) Raylib.UnloadSound(keyboardSounds[i]);
	}





	// TODO: Add a scanline shader or something
	// TODO: Auto word-wrap, and scrolling
	private void DrawScreen()
	{
		// Define constants for drawing
		// TODO: Put some of this stuff up where the colors are
		const float padding = 50;
		const float padding2 = padding * 2;
		const float paddingHalf = padding / 2;
		const float thickness = 5f;
		float fontSize = 20f;
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

		// Check for if we can draw the caret or not
		double currentTime = Raylib.GetTime();
		double elapsedTime = currentTime - lastCaretBlinkTime;
		if (elapsedTime > caretBlinkTime)
		{
			caretCurrentlyVisible = !caretCurrentlyVisible;
			lastCaretBlinkTime = currentTime;
		}

		// Draw the caret if its currently visible
		// TODO: Toggle for box-shape and line-shape caret for if people don't know how to use box caret (noob)
		// TODO: Invert color where caret is for box
		//! just doing caret for now because its easier (easy)
		if (caretCanBlink && caretCurrentlyVisible)
		{
			float x = (padding2 + Raylib.MeasureTextEx(font, prompt, fontSize, fontSpacing).X) + Raylib.MeasureTextEx(font, input.Substring(0, caretIndex), fontSize, fontSpacing).X;
			Raylib.DrawRectangleRec(new Rectangle(x, y, fontSpacing, fontSize), foregroundColor);
		}
	}

	// TODO: Make a fancy command class where it has a description and stuff for in the help section and the actual command logic can be written in a method
	private void RunCommand(string commandInput)
	{
		// Split everything, and check for if we even added an arg
		// TODO: Check for if multiple commands were entered at once (example: cls && help)
		string command = commandInput.Split(" ")[0].Trim().ToLower();
		string[] args = commandInput.Split(" ").Skip(1).ToArray(); //? skip removes first arg (command)

		// Check for what command it is
		// TODO: Add command to rebind keys in settings
		// TODO: Add command to change terminal theme (amber, green, white, etc) and toggle caret type
		foreach (ICommand currentCommand in commands)
		{
			if (command == currentCommand.Name)
			{
				currentCommand.Execute(args, ref output);
				return;
			}
		}

		// If the command didn't run give a error message
		output += $"Unknown command '{command}'.\nPlease use 'help' for a list of commands.\n";
	}
}