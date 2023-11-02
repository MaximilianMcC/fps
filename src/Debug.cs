using System.Numerics;
using Raylib_cs;

public class Debug
{
	public static bool DebugMode { get; set; } = false;
	public static bool LogInConsole { get; set; } = false;

	public static void Log(string text)
	{
		Message message = new Message();
		Terminal.Chat.Add(message);
	}


	// Debug terminal/console
	public class Terminal
	{
		public static List<Message> Chat = new List<Message>();

		// Terminal size and position settings
		// TODO: Make terminal able to be dragged around and resized
		private static int terminalWidth = 600;
		private static int terminalHeight = 500;
		private static int terminalX = 500;
		private static int terminalY = 500;
		private static int titleHeight = 50;
		private static int padding = 20;

		// Dragging stuff
		private static bool dragging = false;
		private static Vector2 dragOffset = Vector2.Zero;

		// Terminal colors
		// TODO: Remove from here and straight up hardcode
		public readonly static Color Green = new Color(131, 255, 8, 255);
		public readonly static Color Red = new Color(255, 8, 131, 255);
		public readonly static Color Orange = new Color(255, 131, 8, 255);
		public readonly static Color Purple = new Color(131, 8, 255, 255);
		public readonly static Color Gray = new Color(156, 156, 156, 255);
		public readonly static Color Tan = new Color(206, 145, 120, 255);
	

		public static void Update()
		{
			// Check for if the terminal needs to be toggled
			if (Raylib.IsKeyPressed(Settings.ToggleDebug)) DebugMode = !DebugMode;
			if (!DebugMode) return;

			// Check for if they are clicking on the title bar and dragging it
			Rectangle titleRectangle = new Rectangle(terminalX, terminalY, terminalWidth, titleHeight);
			if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), titleRectangle) && Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT))
			{
				// Setup dragging
				if (!dragging)
				{
					dragging = true;

					// Get the drag offset and whatnot
					Vector2 mouse = Raylib.GetMousePosition();
					dragOffset = mouse - new Vector2(terminalX, terminalY);
				}
				
				// Update the X and Y of the terminal to move it
				terminalX = ((int)Raylib.GetMousePosition().X) - ((int)dragOffset.X);
				terminalY = ((int)Raylib.GetMousePosition().Y) - ((int)dragOffset.Y);
			}
			else dragging = false;
		}

		public static void Render()
		{ 
			// Check for if debug mode is enabled, then draw the terminal
			if (!DebugMode) return;

			// Draw a shadow and background
			Raylib.DrawRectangle(terminalX, terminalY, terminalWidth + 5, terminalHeight + 5, new Color(0, 0, 0, 128));
			Raylib.DrawRectangle(terminalX, terminalY, terminalWidth, terminalHeight, new Color(13, 25, 38, 255));

			// Draw a title bar and title text
			Raylib.DrawRectangle(terminalX, terminalY, terminalWidth, titleHeight, new Color(54, 54, 54, 255));
			Raylib.DrawText("DEBUG TERMINAL (real)", terminalX + padding, terminalY + (padding / 2), 35, Color.LIGHTGRAY);

			// Draw the text onto the terminal
			int y = terminalY + titleHeight + padding;
			foreach (Message currentMessage in Chat)
			{
				int x = terminalX + padding;
				foreach (KeyValuePair<string, Color> word in currentMessage.Content)
				{
					Raylib.DrawText(word.Key, x, y, 25, word.Value);
					x += Raylib.MeasureText(word.Key, 25);
				}
			}
		}
	}


	// TODO: Take out of struct and just chuck ctor in method
	public struct Message
	{
		public Dictionary<string, Color> Content;

		public Message(string[] words)
		{
			Content = new Dictionary<string, Color>();

			// Set the time
			// words = new string[words.Length + 1];
			// words[words.Length] = DateTime.Now.ToString("hh:mm:ss.fff");

			// Loop over all of the words
			bool currentlyWritingString = false;
			foreach (string currentWord in words)
			{
				// Convert the current word to string and save color
				Color color = Color.WHITE;
				string word = currentWord.ToString();

				// Apply colors depending on entire known words
				// TODO: Use switch
				if (word == "True") color = Terminal.Green;
				if (word == "False") color = Terminal.Red;

				// Apply colors depending on known symbols
				// TODO: Use switch
				// TODO: If getting slow, don't use .Contains and switch to doing manually
				if (word.Contains("0123456789")) color = Terminal.Purple;
				if (word.Contains(".,:")) color = Terminal.Orange;
				if (word.Contains("[]()")) color = Terminal.Gray;

				// String thingy
				if (word.StartsWith("\"") || word.StartsWith("'")) currentlyWritingString = true;
				if (currentlyWritingString) color = Terminal.Tan;
				if (currentlyWritingString && word.StartsWith("\"") || word.StartsWith("'")) currentlyWritingString = false;

				// Add the current word to the message
				Content.Add(word, color);
			}

		}
	}
}