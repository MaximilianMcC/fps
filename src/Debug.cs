using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using Raylib_cs;

public class Debug
{
	public static bool DebugMode { get; set; } = true;
	public static bool LogInConsole { get; set; } = false;




	public class Terminal
	{
		public static void Update()
		{
			// Toggle debug mode
			if (Raylib.IsKeyPressed(SettingsManager.Settings.ToggleDebug)) DebugMode = !DebugMode;
		}

		public static void Render()
		{

		}
	}



	// FPS counter
	// TODO: Make a memory counter/graph
	public class FPSGraph
	{
		// Make the window
		//! Its prolly a bad idea to make it in here but idk it works
		private static Window window = new Window("fram per secnsd", new Vector2(10, 10), new Vector2(300, 200), true);

		// TODO: Stop crazy spike at start
		private static int[] previousFPS = new int[512];

		public static void Update()
		{
			// Get the current FPS
			int currentFPS = Raylib.GetFPS();

			// Shift everything in the array along, removing the first value and adding the
			// current FPS to the last value to make it have a scrolling effect
			for (int i = 0; i < previousFPS.Length - 1; i++)
			{
				previousFPS[i] = previousFPS[i + 1];
			}
			previousFPS[previousFPS.Length - 1] = currentFPS;

			window.Update();
		}

		public static void Render()
		{
			// Check for if debug mode is enabled, then draw the fps
			if (!DebugMode) return;
			window.Render();

			// Positional stuff
			const int padding = 20;
			const int padding2 = padding * 2;
			float x, y;

			// Calculate the scale factor for drawing the graph
			float scaleX = (float)(window.Width - padding2) / (previousFPS.Length - 1);
			float scaleY = (float)(window.BodyHeight - padding2) / SettingsManager.Settings.MaxFps;

			// Draw the graph
			for (int i = 0; i < previousFPS.Length - 1; i++)
			{
				// Calculate the position for the start line position
				x = window.X + (i * scaleX) + padding;
				y = window.Y + window.Height - padding - (previousFPS[i] * scaleY);
				Vector2 start = new Vector2(x, y);

				// Calculate the position for the end line position
				x = window.X + ((i + 1) * scaleX) + padding;
				y = window.Y + window.Height - padding - (previousFPS[i + 1] * scaleY);
				Vector2 end = new Vector2(x, y);

				// Change color based on fps relational to max fps
				Color color = Colors.Green;
				if (previousFPS[i] <= SettingsManager.Settings.MaxFps / 2) color = Colors.Orange;
				if (previousFPS[i] <= SettingsManager.Settings.MaxFps / 5) color = Colors.Red;

				// Draw the line/plot the current fps
				Raylib.DrawLineEx(start, end, 2, color);
			}

			// Draw the FPS text
			//? 35 is font size
			x = window.X + padding;
			y = (window.Y + window.Height) - 35 - padding;
			Raylib.DrawText($"FPS: {Raylib.GetFPS()}", (int)x, (int)y, 35, Color.BEIGE);
		}
	}

	// TODO: Make resizable
	protected class Window
	{
		public int X;
		public int Y;
		public int Width;
		public int Height;

		private const int padding = 10;
		public int BodyY;
		public int BodyHeight;

		public const int TitleHeight = 45;
		private string titleText;

		private bool draggable;
		private bool beingDragged;
		private Vector2 dragOffset;

		// Make a new window
		public Window(string title, Vector2 startPosition, Vector2 size, bool canBeDragged)
		{
			X = (int)startPosition.X;
			Y = (int)startPosition.Y;

			Width = (int)size.X;
			Height = (int)size.Y;

			BodyY = Y + TitleHeight;
			BodyHeight = Height - TitleHeight;

			// Set values
			titleText = title;
			draggable = canBeDragged;
		}

		// Update everything
		public void Update()
		{
			// Check for if debug mode is enabled.
			if (!DebugMode) return;

			// Check for if we can drag the window
			if (!draggable) return;

			// Check for if they are clicking on the title bar and dragging it
			Rectangle titleRectangle = new Rectangle(X, Y, Width, TitleHeight);
			if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), titleRectangle) && Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT))
			{
				// Setup dragging
				if (!beingDragged)
				{
					beingDragged = true;

					// Get the drag offset
					Vector2 mouse = Raylib.GetMousePosition();
					dragOffset = mouse - new Vector2(X, Y);
				}
				
				// Update the X and Y of the window to move it
				X = ((int)Raylib.GetMousePosition().X) - ((int)dragOffset.X);
				Y = ((int)Raylib.GetMousePosition().Y) - ((int)dragOffset.Y);
				BodyY = Y + TitleHeight;

			}
			else beingDragged = false;
		}

		// Draw the window on the screen
		public void Render()
		{
			// Check for if debug mode is enabled
			if (!DebugMode) return;

			// Draw the background and title
			Raylib.DrawRectangle(X, Y, Width, Height, new Color(13, 25, 38, 255));
			Raylib.DrawRectangle(X, Y, Width, TitleHeight, new Color(54, 54, 54, 255));
			Raylib.DrawText(titleText, X + padding, Y + (padding / 2), 35, Color.LIGHTGRAY);
		}
	}



	struct Colors
	{
		public readonly static Color Green = new Color(131, 255, 8, 255);
		public readonly static Color Red = new Color(255, 8, 131, 255);
		public readonly static Color Orange = new Color(255, 131, 8, 255);
		public readonly static Color Purple = new Color(131, 8, 255, 255);
		public readonly static Color Gray = new Color(156, 156, 156, 255);
		public readonly static Color Tan = new Color(206, 145, 120, 255);
		public readonly static Color Blue = new Color(8, 131, 255, 255);
	}
}
