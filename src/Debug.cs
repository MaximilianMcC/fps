using System.Numerics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
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

	// FPS counter
	// TODO: Make memory graph also
	public class FPSGraph
	{
		// TODO: Make length lower/higher for more/less detail
		// TODO: Stop crazy red -> green spike at start
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
		}

		public static void Render()
		{
			// Check for if debug mode is enabled, then draw the fps
			if (!DebugMode) return;

			const int width = 300;
			const int height = 200;
			const int padding = 20;

			// TODO: Make draggable
			// TODO: Add fancy scrolling grid pattern in back
			// Draw box
			Raylib.DrawRectangle(10, 10, width + padding, height + padding, new Color(13, 25, 38, 255));

			// Draw FPS text
			Raylib.DrawText($"FPS: {Raylib.GetFPS()}", 10 + padding, height - padding, 30, Color.BEIGE);

			// Calculate the scale factor for drawing
			float scaleX = (float)width / (previousFPS.Length - 1);
			float scaleY = (float)height / Settings.MaxFps;

			for (int i = 0; i < previousFPS.Length - 1; i++)
			{
				// Get the start, and end positions for the line
				Vector2 start = new Vector2(i * scaleX + padding, height + padding - previousFPS[i] * scaleY);
				Vector2 end = new Vector2((i + 1) * scaleX + padding, height + padding - previousFPS[i + 1] * scaleY);

				// Change color based on fps relational to max fps
				Color color = new Color(131, 255, 8, 255); //? green
				if (previousFPS[i] <= Settings.MaxFps / 2) color = new Color(255, 131, 8, 255); //? orange
				if (previousFPS[i] <= Settings.MaxFps / 4) color = new Color(255, 8, 131, 255); //? red

				// Draw it on the graph
				Raylib.DrawLineEx(start, end, 2, color);
			}
		}
	}

	//! test remove later trust
	public class Test
	{
		private Window window;

		public Test()
		{
			// Make the window
			window = new Window("Test", new Vector2(500, 500), new Vector2(400, 300), true);
		}

		public void Update()
		{
			window.Update();
		}

		public void Render()
		{
			window.Render();
		}
	}









	protected class Window
	{
		private int windowX;
		private int windowY;
		private int windowWidth;
		private int windowHeight;
		private const int padding = 10;

		private const int titleHeight = 50;
		private string titleText;

		private bool draggable;
		private bool beingDragged;
		private Vector2 dragOffset;

		protected readonly static Color Green = new Color(131, 255, 8, 255);
		protected readonly static Color Red = new Color(255, 8, 131, 255);
		protected readonly static Color Orange = new Color(255, 131, 8, 255);
		protected readonly static Color Purple = new Color(131, 8, 255, 255);
		protected readonly static Color Gray = new Color(156, 156, 156, 255);
		protected readonly static Color Tan = new Color(206, 145, 120, 255);

		// Make a new window
		public Window(string title, Vector2 startPosition, Vector2 size, bool canBeDragged)
		{
			windowX = (int)startPosition.X;
			windowY = (int)startPosition.Y;

			windowWidth = (int)size.X;
			windowHeight = (int)size.Y;

			// Set values
			titleText = title;
			draggable = canBeDragged;
		}

		// Update everything
		public void Update()
		{
			// TODO: Check for if debug mode is enabled.

			// Check for if we can drag the window
			if (!draggable) return;

			// Check for if they are clicking on the title bar and dragging it
			Rectangle titleRectangle = new Rectangle(windowX, windowY, windowWidth, titleHeight);
			if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), titleRectangle) && Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT))
			{
				// Setup dragging
				if (!beingDragged)
				{
					beingDragged = true;

					// Get the drag offset and whatnot
					Vector2 mouse = Raylib.GetMousePosition();
					dragOffset = mouse - new Vector2(windowX, windowY);
				}
				
				// Update the X and Y of the terminal to move it
				// TODO: Move everything at once
				windowX = ((int)Raylib.GetMousePosition().X) - ((int)dragOffset.X);
				windowY = ((int)Raylib.GetMousePosition().Y) - ((int)dragOffset.Y);

			}
			else beingDragged = false;
		}

		// Draw the window on the screen
		public void Render()
		{
			// Check for if debug mode is enabled
			if (!DebugMode) return;

			// Draw the background and title
			Raylib.DrawRectangle(windowX, windowY, windowWidth, windowHeight, new Color(13, 25, 38, 255));
			Raylib.DrawRectangle(windowX, windowY, windowWidth, titleHeight, new Color(54, 54, 54, 255));
			Raylib.DrawText(titleText, windowX + padding, windowY + (padding / 2), 35, Color.LIGHTGRAY);
		}
	}
}
