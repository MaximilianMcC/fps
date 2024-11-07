using Raylib_cs;

class InputManager
{
	// Movement keys
	public static readonly KeyboardKey Forwards = KeyboardKey.W;
	public static readonly KeyboardKey Backwards = KeyboardKey.S;
	public static readonly KeyboardKey Left = KeyboardKey.A;
	public static readonly KeyboardKey Right = KeyboardKey.D;

	// Fancy movement stuff
	public static readonly KeyboardKey Sprint = KeyboardKey.LeftControl;
	public static readonly KeyboardKey Jump = KeyboardKey.Space;
	public static readonly KeyboardKey ToggleFreecam = KeyboardKey.N;

	// Mouse stuff
	//? sensitivity scaler is used to let us use a bigger number for sensitivity idk
	private const float sensitivityScaler = 0.000135f;
	public static readonly float Sensitivity = 250f * sensitivityScaler;

	// Other stuff
	public static readonly KeyboardKey ToggleDebug = KeyboardKey.Grave;




	public static float HorizontalInput()
	{
		// Add/subtract from the direction thingy so that
		// the two things can cancel each other out if needed
		float direction = 0;
		if (Raylib.IsKeyDown(Left)) direction++;
		if (Raylib.IsKeyDown(Right)) direction--;

		return direction;
	}

	public static float VerticalInput()
	{
		// Add/subtract from the direction thingy so that
		// the two things can cancel each other out if needed
		float direction = 0;
		if (Raylib.IsKeyDown(Forwards)) direction++;
		if (Raylib.IsKeyDown(Backwards)) direction--;

		return direction;
	}
}