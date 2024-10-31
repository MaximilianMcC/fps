using System.Numerics;
using Raylib_cs;

class Player : Entity
{
	// Camera (kinda important)
	public Camera3D Camera;

	// Head stuff
	public float Yaw;
	public float Pitch;
	private float eyeHeight = 1.7f;
	private float sensitivity = 250f;

	// Actual moving stuff
	private bool freecam = false;
	private float freecamFlySpeed = 24f;
	private float walkingSpeed = 4.5f;
	private float runningSpeed = 8f;

	// TODO: Put this in the map or something
	private float gravity = 9.807f;
	private const float terminalVelocity = 55f;
	private Vector3 velocity;
	private bool useGravity = false;

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
		Vector3 direction = Vector3.Transform(Vector3.UnitZ, Rotation);

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
			//? Using 89 instead of 90 to avoid gimbal lock or something
			Vector2 mouseMovement = Raylib.GetMouseDelta() * sensitivity * Raylib.GetFrameTime();
			Yaw -= mouseMovement.X;
			Pitch = Math.Clamp(Pitch + mouseMovement.Y, -89f, 89f);
		}

		// Update the quaternion rotation thingy based
		// on the newly updated yaw and pitch values
		// converted to radians
		// TODO: Put inside mouse scope
		float yaw = Yaw * Raylib.DEG2RAD;
		float pitch = Pitch * Raylib.DEG2RAD;
		Rotation = Quaternion.CreateFromYawPitchRoll(yaw, pitch, 0f);

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

			// Get the direction vector depending on if we
			// are in freecam or not
			Quaternion rotation = freecam ? Rotation : Quaternion.CreateFromYawPitchRoll(yaw, 0f, 0f);

			// Apply the direction vector to the movement
			// TODO: Don't reuse this from UpdateCamera
			// TODO: Just plus this on in UpdateCamera or something
			Vector3 directionInput = new Vector3(xMovement, 0, yMovement);
			Vector3 direction = Vector3.Transform(directionInput, rotation);
			if (direction != Vector3.Zero) direction = Vector3.Normalize(direction);

			// Apply speed and whatnot to get the movement
			// TODO: Velocity based movement system
			Vector3 movement = (GetMovement() * direction) * Raylib.GetFrameTime();

			// Update the players position with the new movement
			Position += movement;
		}
	}

	// Get the speed and gravity and all that
	private Vector3 GetMovement()
	{
		// Default movement of nothing
		Vector3 movement = Vector3.One;

		// Check for if they're running or walking
		float speed = Raylib.IsKeyDown(KeyboardKey.LeftControl) ? runningSpeed : walkingSpeed;
		if (freecam) speed = freecamFlySpeed;
		movement *= speed;

		// Apply gravity and make sure they don't reach terminal velocity
		// TODO: Put this in the entity class
		// TODO: Don't make terminal velocity negative here
		velocity.Y -= (gravity * (useGravity ? 1 : 0)) * Raylib.GetFrameTime();
		if (velocity.Y > terminalVelocity) velocity.Y = -terminalVelocity;

		// Give back the new movement
		return movement;
	}

	public override void Update()
	{
		Movement();
		UpdateCamera();

		// Check for if they wanna toggle freecam (N)
		if (Raylib.IsKeyPressed(KeyboardKey.N)) freecam = !freecam;

		//! temp debug
		// Check for if they wanna toggle gravity (G)
		if (Raylib.IsKeyPressed(KeyboardKey.G))
		{
			useGravity = !useGravity;
			if (useGravity == false) velocity.Y = 0;
		}
	}

	// TODO: debug class and whatnot
	public override void Render2D()
	{
		string freecamStatus = freecam ? "yup" : "nah";
		Raylib.DrawText($"freecamming rn: {freecamStatus}", 10, 10, 35, Color.White);


		Debug.PrintVector3("velocity", velocity, 50);
		Debug.PrintBoolean("Gravity", useGravity, 150);

		Debug.PrintVector3("position", Position, 200);
	}
}