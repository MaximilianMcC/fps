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

		// Connect to the server
		// TODO: Put this in start method or something idk
		// TODO: Get port and ip from args or ui
		const int Port = 54321;
		const string ip = "127.0.0.1";
		Networker.Network(Port, ip, "test");

		while (!Raylib.WindowShouldClose())
		{
			Raylib.BeginDrawing();
			Raylib.ClearBackground(Color.Magenta);





			Raylib.EndDrawing();
		}

		Raylib.CloseWindow();
	}
}