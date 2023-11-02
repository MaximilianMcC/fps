using System.Numerics;
using Raylib_cs;

public class Debug
{

	public enum LogType
	{
		INFO,
		WARN,
		ERROR,
		SPECIAL
	}

	// Log out a bool
	public static void Float(string text, LogType type)
	{

	}

	// TODO: Add overloads for heaps of data that all convert to string in their own way and print
	// Log out a string
	public static void Log(string text, LogType type)
	{

	}



	// Debug terminal/console
	public class Terminal
	{
		public static bool DebugMode { get; set; } = true;

		// Terminal size and position settings
		// TODO: Make terminal able to be dragged around and resized
		private static int terminalWidth = 600;
		private static int terminalHeight = 500;
		private static int terminalX = 10;
		private static int terminalY = 10;
		private static int padding = 20;
	

		public static void Update()
		{
			// Check for if the terminals needs to be toggled
			if (Raylib.IsKeyPressed(Settings.ToggleDebug)) DebugMode = !DebugMode;
		}

		public static void Render()
		{
			// Check for if debug mode is enabled, then draw the terminal
			if (!DebugMode) return;

			int titleHeight = 50;

			// Draw a shadow and background
			Raylib.DrawRectangle(terminalX, terminalY, terminalWidth + 5, terminalHeight + 5, new Color(0, 0, 0, 128));
			Raylib.DrawRectangle(terminalX, terminalY, terminalWidth, terminalHeight, new Color(13, 25, 38, 255));

			// Draw a title bar and title text
			Raylib.DrawRectangle(terminalX, terminalY, terminalWidth, titleHeight, new Color(54, 54, 54, 255));
			Raylib.DrawText("DEBUG TERMINAL", terminalX + padding, padding, 35, Color.LIGHTGRAY);
		}
	}
}