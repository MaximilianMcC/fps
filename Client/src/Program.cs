using Raylib_cs;
using Shared;

namespace Client;
class Program
{
	public static void Main(string[] args)	
	{
		Logger.Log("Kia ora!");

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