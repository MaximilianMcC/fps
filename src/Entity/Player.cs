using System.Numerics;
using Raylib_cs;

class Player : Entity
{
	public Camera3D Camera;

	// Head stuff
	public float Yaw;
	public float Pitch;
	Quaternion rotation = Quaternion.Identity;

	// Actual moving stuff
	bool freecam = false;
	float speed = 45f;

	private float eyeHeight = 1.7f;
	private float sensitivity = 250f;

	public override void Start()
	{
		// Create the camera
		Camera = new Camera3D()
		{
			Up = Vector3.UnitY,
			FovY = 65f,
			Projection = CameraProjection.Perspective
		};
		UpdateCamera();

		// Turn off the mouse
		Raylib.DisableCursor();
	}

	private void UpdateCamera()
	{
		// Get the direction (Z) using the
		// rotation quaternion thingy
		Vector3 direction = Vector3.Transform(Vector3.UnitZ, rotation);

		// Update the position, and the
		// target based on the position
		Camera.Position = Position + (Vector3.UnitY * eyeHeight);
		Camera.Target = Position + (Vector3.UnitY * eyeHeight) + direction;
	}

	private void Movement()
	{
		// Mouse movement/looking around
		{
			// Get what direction the player is looking at
			// and update the yaw and pitch accordingly
			//? Using 89 instead of 90 to avoid gimbal lock
			Vector2 mouseMovement = Raylib.GetMouseDelta() * sensitivity * Raylib.GetFrameTime();
			Yaw -= mouseMovement.X;
			Pitch = Math.Clamp(Pitch + mouseMovement.Y, -89f, 89f);

			// Update the quaternion rotation thingy based
			// on the newly updated yaw and pitch values
			// converted to radians 
			float yaw = Yaw * Raylib.DEG2RAD;
			float pitch = Pitch * Raylib.DEG2RAD;
			rotation = Quaternion.CreateFromYawPitchRoll(yaw, pitch, 0f);
		}

		// Keyboard movement/moving around
		{
			// Capture all the left/right movement
			int xMovement = 0;
			if (Raylib.IsKeyDown(KeyboardKey.A)) xMovement += 1;
			if (Raylib.IsKeyDown(KeyboardKey.D)) xMovement += -1;

			// Capture all the forwards/backwards movement
			int yMovement = 0;
			if (Raylib.IsKeyDown(KeyboardKey.W)) yMovement += 1;
			if (Raylib.IsKeyDown(KeyboardKey.S)) yMovement += -1;

			// Get a matrix just with yaw/x rotation so that
			// our speed isn't affected by the heads pitch
			float yaw = Yaw * Raylib.DEG2RAD;
			Quaternion yawRotation = Quaternion.CreateFromYawPitchRoll(yaw, 0f, 0f);

			// Check for what matrix we use depending if 
			// we're using freecam or not
			// TODO: Don't calculate the yaw only rotation if its not being used
			Quaternion movementRotation = freecam ? rotation : yawRotation;

			// Apply the direction vector to the movement and
			// normalize it so its the same for all directions
			Vector3 directionInput = new Vector3(xMovement, 0, yMovement);
			Vector3 direction = Vector3.Transform(directionInput, movementRotation);
			Vector3.Normalize(direction);

			// Apply speed and whatnot to get the movement
			// TODO: Velocity based movement system
			Vector3 movement = (direction * speed) * Raylib.GetFrameTime();

			// Update the players position with the new movement
			Position += movement;
		}
	}

	public override void Update()
	{
		Movement();
		UpdateCamera();

		// Check for if they wanna toggle freecam (N)
		if (Raylib.IsKeyPressed(KeyboardKey.N)) freecam = !freecam;
	}

	public override void Render2D()
	{
		string freecamStatus = freecam ? "yup" : "nah";
		Raylib.DrawText($"freecamming rn: {freecamStatus}", 10, 10, 35, Color.White);
	}
}
