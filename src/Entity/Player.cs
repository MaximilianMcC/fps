using System.Numerics;
using Raylib_cs;


class Player : Entity
{
	// Camera (important)
	public Camera3D Camera;

	// Movement variables
	private float yaw, pitch;
	private bool freecam = false;
	private Quaternion headRotation;

	// Body proportions and physics properties
	private float height = 1.75f;
	private float eyeHeight = 1.645f;
	private float mass = 57.5f;

	// Physics stuff
	private float frictionCoefficient = 0.001f;
	private float acceleration = 100f;

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
		// Get the direction that we're looking
		Vector3 direction = Vector3.Transform(Vector3.UnitZ, headRotation);

		// Get the current camera position
		// and update the camera accordingly
		Vector3 cameraPosition = Position + (Vector3.UnitY * eyeHeight);
		Camera.Position = cameraPosition;
		Camera.Target = cameraPosition + direction;
	}

	private void Move()
	{
		// Mouse movement/looking around
		HandleMouseLook();

		// Keyboard movement/moving around
		HandleKeyboardMovement();
	}

	private void HandleMouseLook()
	{
		// Get the new yaw and pitch (look around)
		Vector2 mouseMovement = Raylib.GetMouseDelta() * InputManager.Sensitivity;
		yaw -= mouseMovement.X;
		pitch = Math.Clamp(pitch + mouseMovement.Y, -89f, 89f);

		// Update the quaternion rotation
		float yawRadians = yaw * Raylib.DEG2RAD;
		float pitchRadians = pitch * Raylib.DEG2RAD;
		headRotation = Quaternion.CreateFromYawPitchRoll(yawRadians, pitchRadians, 0f);
		Rotation = Quaternion.CreateFromYawPitchRoll(yawRadians, (freecam ? pitchRadians: 0f), 0f);
	}

	private void HandleKeyboardMovement()
	{
		// Get the movement input and normalize it
		Vector3 inputDirection = InputManager.GetDirectionInput();		
		if (inputDirection.LengthSquared() > 0) inputDirection = Vector3.Normalize(inputDirection);

		// Create direction vectors based on player rotation
		Vector3 forwards = Vector3.Transform(Vector3.UnitZ, Rotation);
		Vector3 right = Vector3.Cross(Camera.Up, forwards);

		// Combine the directions to get the final movement direction
		Vector3 direction = (forwards * inputDirection.Z) + (right * inputDirection.X);

		// Actually move, then update the position
		ApplyMovementForces(direction);
		Position += Velocity * Raylib.GetFrameTime();
	}

	private void ApplyMovementForces(Vector3 direction)
	{
		// Apply acceleration
		Velocity += (acceleration * direction) * Raylib.GetFrameTime();

		// Apply friction
		// TODO: Remove/bake the `1 -` thingy
		Velocity *= 1 - frictionCoefficient;

		// If the velocity is tiny as and we're not
		// moving then just kill it yk
		float speed = Velocity.LengthSquared();
		if (speed < 0.1f && direction == Vector3.Zero) Velocity = Vector3.Zero;
	}

	public override void Update()
	{
		Move();
		UpdateCamera();

		// Check for freecam toggle
		if (Raylib.IsKeyPressed(KeyboardKey.N)) freecam = !freecam;
	}

	public override void RenderDebug2D()
	{
		// Display debug info
		Raylib.DrawText($"Position: {Position}\n\nVelocity: {Velocity}\n\nSpeed: {Velocity.Length()} m/s\n\n\n\n\nfreecam: {freecam}", 10, 10, 30, Color.White);
	}
}
