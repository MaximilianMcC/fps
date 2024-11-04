using Raylib_cs;

class Program
{
	public static void Main(string[] args)
	{
		Raylib.SetTraceLogLevel(TraceLogLevel.Warning);
		Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
		Raylib.InitWindow(854, 480, "Bulletproof backpack");

		// Start everything
		Game.SetUp();
		Game.Things.ForEach(thing => thing.Start());

		while (Raylib.WindowShouldClose() == false)
		{
			// Update everything
			Game.Things.ForEach(thing => thing.Update());
			Debug.Update();

			// Start drawing
			Raylib.BeginDrawing();
			Raylib.ClearBackground(Color.Magenta);

			// Draw all the 3D stuff
			Raylib.BeginMode3D(Game.Player.Camera);
			Game.Things.ForEach(thing => thing.Render3D());
			Raylib.EndMode3D();

			// Draw all the 2D stuff
			Game.Things.ForEach(thing => thing.Render2D());
			Debug.Draw();
			Raylib.EndDrawing();
		}

		// Unload everything
		Game.Things.ForEach(thing => thing.CleanUp());
		Raylib.CloseWindow();
	}
}