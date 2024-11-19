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

	// Speeds
	private float walkVelocity = 3f;
	private float runVelocity = 5f;
	private float freecamVelocity = 10f;

	// Physics stuff
	private float frictionCoefficient = 0.001f;
	private float acceleration = 1000f;
	private float maxVelocity = 10f;

	//! Temp
	private Texture2D hands;

	public override void Start()
	{
		hands = AssetManager.LoadTexture("./assets/hands.png");

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

		// Check for if we wanna run/walk
		// TODO: Maybe decrease friction for freecam
		if (freecam) maxVelocity = freecamVelocity;
		else maxVelocity = Raylib.IsKeyDown(InputManager.Sprint) ? runVelocity : walkVelocity;

		// Actually move, then update the position
		ApplyMovementForces(direction);
		Position += Velocity * Raylib.GetFrameTime();
	}

	private void ApplyMovementForces(Vector3 direction)
	{
		// Only apply velocity if we're below the target velocity
		float speed = Velocity.Length();
		if (speed < maxVelocity)
		{
			// Apply acceleration based on the movement direction
			Velocity += (acceleration * direction) * Raylib.GetFrameTime();
		}

		// If the velocity is tiny as and we're not
		// moving then just kill it yk
		if (speed < 0.1f && direction == Vector3.Zero) Velocity = Vector3.Zero;

		// Apply friction
		//? Delta time isn't applied here because it is already applied previously
		// TODO: Remove/bake the `1 -` thingy
		Velocity *= (1 - frictionCoefficient);
	}

	public override void Update()
	{
		Move();
		UpdateCamera();

		// Check for freecam toggle
		if (Raylib.IsKeyPressed(KeyboardKey.N)) freecam = !freecam;
	}

	public override void Render2D()
	{
		float handHeight = Raylib.GetScreenHeight() / 1.5f;

		Raylib.DrawTexturePro(
			hands,
			new Rectangle(0, 0, hands.Width, hands.Height),
			new Rectangle(0, Raylib.GetScreenHeight() - handHeight, Raylib.GetScreenWidth(), handHeight),
			Vector2.Zero,
			0f,
			Color.White
		);
	}

	public override void RenderDebug2D()
	{
		// Display debug info
		Raylib.DrawText($"Position: {Position}\n\nVelocity: {Velocity}\n\nSpeed: {Velocity.Length()} m/s\n\n\n\n\nfreecam: {freecam}", 10, 10, 30, Color.White);
	}
}