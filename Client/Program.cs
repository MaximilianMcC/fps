using Raylib_cs;
using Shared;

class Program
{
	public static void Main(string[] args)	
	{
		Console.WriteLine("Kia ora (client)");

		Raylib.SetTraceLogLevel(TraceLogLevel.Warning);
		Raylib.InitWindow(854, 480, "Marl Multiplayer Game (mmg)");

		while (!Raylib.WindowShouldClose())
		{
			Raylib.BeginDrawing();
			Raylib.ClearBackground(Color.Magenta);



			Raylib.EndDrawing();
		}

		Raylib.CloseWindow();
	}
}