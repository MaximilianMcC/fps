using System.Numerics;
using Raylib_cs;

class Debug
{
	public static bool Enabled = false;

	public static void Update()
	{
		// Check for if we wanna toggle debug mode
		if (Raylib.IsKeyPressed(InputManager.ToggleDebug)) Enabled = !Enabled;
	}

	public static void Draw()
	{
		
	}
}
