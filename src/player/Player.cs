using System.Numerics;
using Raylib_cs;

class Player
{
	// public Camera3D Camera;
	public Camera3D Camera;
	public Vector3 Position = Vector3.Zero;
	public float pitch = 0;
	public float yaw = 0;

	// Speed stuff
	private const float walkSpeed = 10f;
	private const float runSpeed = 15f;

	// Make a new player
	public Player()
	{
		// Create the raylib camera
		Camera = new Camera3D
		{
			position = new Vector3(Position.X, Position.Y + 1, Position.Z),
			target = Vector3.Zero,
			up = Vector3.UnitY,
			fovy = Settings.Fov,
			projection = CameraProjection.CAMERA_PERSPECTIVE
		};
	}

	// Update
	public void Update(float deltaTime)
	{
		// Movement(deltaTime);
		Movement(deltaTime);

		// Look at some random cube
		if (Raylib.IsKeyPressed(KeyboardKey.KEY_F9))
		{
			Console.WriteLine("Should eb able to se cube rn🙏🙏");
			yaw = -1.5f;
			pitch = 0;
			Camera.position = new Vector3(0, 1, 0);
		}
	}

	// Move the player
	// TODO: Maybe split mouse and keyboard stuff into two different methods for improved readability
	public void Movement(float deltaTime)
	{
		// Camera rotation
		Vector2 mouseDelta = Raylib.GetMouseDelta() * -Settings.Sensitivity;
		yaw += mouseDelta.X;
		pitch += mouseDelta.Y;

		// Stop player from snapping their neck
		if (pitch > 80.0f)  pitch = 80.0f;
		else if (pitch < -80.0f) pitch = -80.0f;

		// Calculate target from pitch and yaw
		Vector3 direction = new Vector3(
			(float)(Math.Cos(pitch) * Math.Sin(yaw)),
			(float)Math.Sin(pitch),
			(float)(Math.Cos(pitch) * Math.Cos(yaw))
		);

		// Calculate right vector for moving in the direction that the player is looking
		Vector3 right = new Vector3(
			(float)Math.Sin(yaw - Math.PI / 2.0f),
			0,
			(float)Math.Cos(yaw - Math.PI / 2.0f)
		);

		// Check for if the player wants to run or walk
		float speed = walkSpeed;
		if (Raylib.IsKeyDown(Settings.Sprint)) speed = runSpeed;

		// Keyboard movement input
		Vector3 newPosition = Position;

		// Forwards and backwards (W, S)
		if (Raylib.IsKeyDown(Settings.Forwards)) newPosition += direction;
		if (Raylib.IsKeyDown(Settings.Backwards)) newPosition -= direction;

		// Left and right/strafing (A, D)
		if (Raylib.IsKeyDown(Settings.Left)) newPosition -= right;
		if (Raylib.IsKeyDown(Settings.Right)) newPosition += right;

		// Apply speed and delta time. Also remove any Y movement to stop the player from flying
		// TODO: Reenable the Y thingy to make the player fly in a spectator mode or something
		// TODO: Normalize to keep same speed
		newPosition *= speed * deltaTime;
		newPosition.Y = 0;

		// Move and update the camera
		Camera.position += newPosition;
		Camera.target = Camera.position + direction;
	}





}