using System.Numerics;
using Raylib_cs;

class Player : Entity
{
	// Camera (kinda important)
	public Camera3D Camera;

	// Head stuff
	public float Yaw;
	public float Pitch;
	private float sensitivity = 250f;

	// Moving stuff
	private bool freecam = false;
	private float freecamFlySpeed = 24f;
	private float walkingSpeed = 4.5f;
	private float runningSpeed = 8f;

	// Body proportion stuff
	private float eyeHeight = 1.7f;
	private float mass = 85f;

	// TODO: Put this in the map or something
	private float gravity = 9.807f;
	private const float terminalVelocity = 55f;
	private float friction = 0.1f;

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

			// Get the direction that we're gonna move
			// TODO: Don't reuse this from UpdateCamera
			// TODO: Just plus this on in UpdateCamera or something
			Vector3 directionInput = new Vector3(xMovement, 0, yMovement);
			Vector3 direction = Vector3.Transform(directionInput, rotation);
			if (direction != Vector3.Zero) direction = Vector3.Normalize(direction);

			// Update the players position with the new movement
			UpdateVelocity(direction);
			Position += Velocity * Raylib.GetFrameTime();
		}
	}

	// Get the speed and gravity and all that
	// TODO: Velocity based movement system
	private void UpdateVelocity(Vector3 direction)
	{
		// Check for if they're running, walking, or
		// in freecam and use that as the target speed
		float targetSpeed = Raylib.IsKeyDown(KeyboardKey.LeftControl) ? runningSpeed : walkingSpeed;
		if (freecam) targetSpeed = freecamFlySpeed;

		// Get the target Velocity that we're
		// gonna go up to overtime (Velocity)
		// and the force required to get to it
		Vector3 targetVelocity = direction * targetSpeed;
		Vector3 force = (targetVelocity - Velocity) * mass;

		// Add the force to the Velocity to
		// actually make the player move
		Velocity += force * Raylib.GetFrameTime();

		// Add friction so the player can
		// stop eventually
		float deltaFriction = 1 - (friction * Raylib.GetFrameTime());
		Velocity *= deltaFriction;
	}

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