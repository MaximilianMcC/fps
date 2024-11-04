using System.Numerics;
using Raylib_cs;

class Debug
{
	private static int targetFpsIndex;
	private static int[] targetFpsses = new int[] { 10, 30, 60, 100 };

	// TODO: Make a 3D visualization where you can see the 
	public static void PrintVector3(string vectorName, Vector3 value, int y)
	{
		Raylib.DrawText($"{vectorName} X: {value.X}\n\n{vectorName} Y: {value.Y}\n\n{vectorName} Z: {value.Z}", 10, y, 20, Color.White);
	}

	public static void PrintBoolean(string booleanName, bool value, int y)
	{
		Raylib.DrawText($"{booleanName}: {value}", 10, y, 20, Color.White);
	}

	public static void PrintFloat(string floatName, float value, int y, int precision = 16)
	{
		Raylib.DrawText($"{floatName}: {value.ToString("F" + precision)}", 10, y, 20, Color.White);
	}

	public static void Update()
	{
		if (Raylib.IsKeyPressed(KeyboardKey.T))
		{
			targetFpsIndex++;
			if (targetFpsIndex >= targetFpsses.Length) targetFpsIndex = 0;
			Raylib.SetTargetFPS(targetFpsses[targetFpsIndex]);
		}
	}

	public static void Draw()
	{
		PrintFloat("target fps", targetFpsses[targetFpsIndex], 320, 0);
		PrintFloat("fps", Raylib.GetFPS(), 350, 0);

	}
}
