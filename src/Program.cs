using Raylib_cs;

class Program
{
	public static void Main(string[] args)
	{
		Raylib.SetTraceLogLevel(TraceLogLevel.Warning);
		Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
		Raylib.InitWindow(854, 480, "Bulletproof backpack");

		// Debug stuff
		Debug.Enabled = true;

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
			foreach (Updatable thing in Game.Things)
			{
				thing.Render3D();
				thing.RenderDebug3D();
			}
			Raylib.EndMode3D();

			// Draw all the 2D stuff
			foreach (Updatable thing in Game.Things)
			{
				thing.Render2D();
				thing.RenderDebug2D();
			}
			Debug.Draw();
			Raylib.EndDrawing();
		}

		// Unload everything
		Game.Things.ForEach(thing => thing.CleanUp());
		Raylib.CloseWindow();
	}
}