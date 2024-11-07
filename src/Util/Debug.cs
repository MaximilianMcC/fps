using System.Numerics;
using Raylib_cs;

class Debug
{
	public static bool Enabled = false;
	private static StringWriter debugOutput;

	public static void StartSnaggingOutput()
	{
		// Redirect the console output to a
		// string thingy so we can steal it
		// and print it nicely in raylib
		debugOutput = new StringWriter();
		Console.SetOut(debugOutput);
	}

	public static void Update()
	{
		// Check for if we wanna toggle debug mode
		if (Raylib.IsKeyPressed(InputManager.ToggleDebug)) Enabled = !Enabled;

		// Draw the fps
		Console.WriteLine(Raylib.GetFPS() + "\n");
	}

	public static void Draw()
	{
		// Only show the stuff if we need to 
		if (Enabled == false) return;

		// Get all of the text that we've printed
		// then clear it for the next frame
		string debugText = debugOutput.ToString();
		debugOutput.GetStringBuilder().Clear();

		// Make the newlines actually a new line
		//! Raylib issue. Remove when fixed
		debugText = debugText.Replace("\n", "\n\n");

		// Draw the output with raylib
		Raylib.DrawText(debugText, 10, 10, 25, Color.White);
	}
}
