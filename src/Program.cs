using Raylib_cs;

class Program
{
	public static List<Thing> Things = new List<Thing>();

	public static void Main(string[] args)
	{
		Raylib.SetTraceLogLevel(TraceLogLevel.Warning);
		Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
		Raylib.InitWindow(854, 480, "Bulletproof backpack");
		Raylib.MaximizeWindow();

		// Start everything
		Things.ForEach(thing => thing.Start());

		while (Raylib.WindowShouldClose() == false)
		{
			// Update everything
			{
				Things.ForEach(thing => thing.Update());
			}

			Raylib.BeginDrawing();
			Raylib.ClearBackground(Color.Magenta);

			// Draw all the 3D stuff
			Raylib.BeginMode3D(Player.Camera);
			{
				Things.ForEach(thing => thing.RenderWorld());
			}
			Raylib.EndMode3D();

			// Draw all the 2D stuff
			{
				Things.ForEach(thing => thing.RenderScreen());
			}

			Raylib.EndDrawing();
		}

		// Unload everything
		Things.ForEach(thing => thing.CleanUp());
		Raylib.CloseWindow();
	}
}