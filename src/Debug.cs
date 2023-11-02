using System.Numerics;
using Raylib_cs;

public class Debug
{
	public static bool DebugMode { get; set; } = true;
	public static bool LogInConsole { get; set; } = false;

	public enum LogType
	{
		INFO,
		WARN,
		ERROR,
		SPECIAL
	}

	// Log out a bool
	public static void Log(bool value, LogType type)
	{

	}

	// TODO: Add overloads for heaps of data that all convert to string in their own way and print
	// Log out a string
	public static void Log(string text, LogType type)
	{
		if (LogInConsole) Console.WriteLine(text);
	}



	// Debug terminal/console
	public class Terminal
	{
		// Terminal size and position settings
		// TODO: Make terminal able to be dragged around and resized
		private static int terminalWidth = 600;
		private static int terminalHeight = 500;
		private static int terminalX = 500;
		private static int terminalY = 500;
		private static int titleHeight = 50;

		private static int padding = 20;
		private static int roundness = 10;
		private static int segments = 20;

		// Dragging stuff
		private static bool dragging = false;
		private static Vector2 dragOffset = Vector2.Zero;

		// Terminal colors
		// TODO: Make const or something
		private static Color green = new Color(131, 255, 8, 255);
		private static Color red = new Color(255, 8, 131, 255);
		private static Color orange = new Color(255, 131, 8, 255);
		private static Color purple = new Color(131, 8, 255, 255);
	

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
		}
	}
}