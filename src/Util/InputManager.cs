using Raylib_cs;

class InputManager
{
	// Movement keys
	public readonly KeyboardKey Forwards = KeyboardKey.W;
	public readonly KeyboardKey Backwards = KeyboardKey.S;
	public readonly KeyboardKey Left = KeyboardKey.A;
	public readonly KeyboardKey Right = KeyboardKey.D;

	// Fancy movement stuff
	public readonly KeyboardKey Sprint = KeyboardKey.LeftControl;
	public readonly KeyboardKey ToggleFreecam = KeyboardKey.N;

	// Mouse stuff
	public readonly float Sensitivity = 250f;
}