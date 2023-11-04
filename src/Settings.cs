using Raylib_cs;

class Settings
{
	// Keyboard movement stuff
	public static KeyboardKey Forwards = KeyboardKey.KEY_W;
	public static KeyboardKey Backwards = KeyboardKey.KEY_S;
	public static KeyboardKey Left = KeyboardKey.KEY_A;
	public static KeyboardKey Right = KeyboardKey.KEY_D;
	public static KeyboardKey Sprint = KeyboardKey.KEY_LEFT_CONTROL;
	public static KeyboardKey Jump = KeyboardKey.KEY_SPACE;
	
	// Mouse movement stuff
	public static float Sensitivity = 0.003f;
	public static float Fov = 60f;

	// Graphics/video stuff
	public static int MaxFps = 144;

	// Debug/other stuff
	public static KeyboardKey ToggleDebug =  KeyboardKey.KEY_GRAVE;

	// Crosshair settings
	// TODO: Maybe chuck in a separate struct or something
	public static Color CrosshairColor = new Color(255, 255, 255, 128);
	public static int CrosshairSize = 5;
	public static bool CrosshairCentreDot = true;
	public static int CrosshairLength = 4;
	public static int CrosshairSpacing = 2;
}