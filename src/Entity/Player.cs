using System.Numerics;
using Raylib_cs;

class Player : Entity
{
	// Camera (kinda important)
	public Camera3D Camera;

	// Head stuff
	public float Yaw;
	public float Pitch;

	// Moving stuff
	private bool freecam = false;
	private float freecamFlySpeed = 24f;
	private float walkingSpeed = 4.5f;
	private float runningSpeed = 8f;

	// Body proportion stuff
	private float eyeHeight = 1.7f;
	private float mass = 85f;

	// TODO: Put this in the map or something
	private float gravity = 9.81f;
	private const float terminalVelocity = 55f;
	private float friction = 0.5f;

	//! get rid of this fully idk
	private Vector3 previousPosition;

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
			Vector2 mouseMovement = Raylib.GetMouseDelta() * InputManager.Sensitivity;
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
			// Get the movement input
			Vector3 inputDirection = new Vector3(
				InputManager.HorizontalInput(),
				0,
				InputManager.VerticalInput()
			);

			// TODO: Use length squared
			// Normalize it
			if (inputDirection.Length() > 0) inputDirection = Vector3.Normalize(inputDirection);

			// Get the movement direction vectors
			Vector3 forwards = Vector3.Transform(Vector3.UnitZ, Rotation);
			Vector3 right = Vector3.Cross(Camera.Up, forwards);

			// Combine the forwards and right vectors to get
			// the direction that the player has to move in
			Vector3 direction = (forwards * inputDirection.Z) + (right * inputDirection.X);		

			// Get the new velocity to apply rn
			//! temp Y thing
			Vector3 newVelocity = direction * walkingSpeed * new Vector3(1, 0, 1);

			// Apply velocity, and friction
			Velocity += newVelocity;
			Velocity.X -= Velocity.X * friction;
			Velocity.Z -= Velocity.Z * friction;

			// If the velocity is low as then just set it to 0
			if (Velocity.LengthSquared() < 0.1f) Velocity = Vector3.Zero;

			// Update the position
			Position += Velocity * Raylib.GetFrameTime();
		}
	}

	// TODO: Remove
	//! dodgy
	private float GetSpeed()
	{
		Vector3 deltaPosition = Position - previousPosition;
		float speed = deltaPosition.Length() / Raylib.GetFrameTime();
		previousPosition = Position;

		return speed;
	}

	public override void Update()
	{
		Movement();
		UpdateCamera();

		// Check for if they wanna toggle freecam (N)
		if (Raylib.IsKeyPressed(KeyboardKey.N))
		{
			freecam = !freecam;
			// if (freecam) Velocity.Y = 0;
		}
	}

	// TODO: debug class and whatnot
	public override void Render2D()
	{
		Debug.PrintBoolean("freecam", freecam, 10);



		Debug.PrintVector3("Velocity", Velocity, 50);
		Debug.PrintVector3("position", Position, 200);

		Debug.PrintFloat("Speed", GetSpeed(), 500, 2);
	}
}