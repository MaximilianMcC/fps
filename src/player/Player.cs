using System.Numerics;
using Raylib_cs;

class Player
{
	// Camera and movement stuff
	public Camera3D Camera;
	public Vector3 Position { get; set; } = Vector3.Zero;
	public float Pitch { get; set; } = 0;
	public float Yaw { get; set; } = 0;

	// Physics stuff
	// TODO: Figure out why the player sinks into the ground (something to do with friction i think)
	public float Mass { get; private set; } = 62; //? kilograms
	public Vector3 Velocity { get; set; } = Vector3.Zero;
	private float frictionCoefficient = 0.1f;

	// Movement stuff
	public float Speed { get; private set; }
	private float walkForce;
	private float runForce;
	private Vector3 direction;
	private Vector3 right;

	// Make a new player
	public Player()
	{
		// Create the raylib camera
		Camera = new Camera3D
		{
			position = new Vector3(Position.X, Position.Y + 1, Position.Z),
			target = Vector3.Zero,
			up = Vector3.UnitY,
			fovy = SettingsManager.Settings.Fov,
			projection = CameraProjection.CAMERA_PERSPECTIVE
		};

		// Set the movement forces so that they are proportional to body weight
		//! I asked Clyde for average horizontal force applied when walking and they said 0.5 - 1.5
		walkForce = (float)(Mass * 1.2);
		runForce = (float)(Mass * 1.5);
	}

	// Update
	public void Update(float deltaTime)
	{
		// Move the player
		MouseMovement();
		KeyboardMovement();
		Speed = Velocity.Length();
	}

	// Looking around and stuff
	private void MouseMovement()
	{
		// Camera rotation
		Vector2 mouseDelta = Raylib.GetMouseDelta() * -SettingsManager.Settings.Sensitivity;
		Yaw += mouseDelta.X;
		Pitch += mouseDelta.Y;

		// Stop player from snapping their neck
		// TODO: Use correct values
		if (Pitch > 80.0f)  Pitch = 80.0f;
		else if (Pitch < -80.0f) Pitch = -80.0f;

		// Calculate target from pitch and yaw
		direction = new Vector3(
			(float)(Math.Cos(Pitch) * Math.Sin(Yaw)),
			(float)Math.Sin(Pitch),
			(float)(Math.Cos(Pitch) * Math.Cos(Yaw))
		);

		// Calculate right vector for moving in the direction that the player is looking
		right = new Vector3(
			(float)Math.Sin(Yaw - Math.PI / 2.0f),
			0,
			(float)Math.Cos(Yaw - Math.PI / 2.0f)
		);
	}

	private void KeyboardMovement()
	{
		// Check for if the player wants to run or walk
		float moveForce = walkForce;
		if (Raylib.IsKeyDown(SettingsManager.Settings.Sprint))
		{
			moveForce = runForce;
		}

		Vector3 force = Vector3.Zero;

		// Forwards and backwards keyboard input (W, S)
		if (Raylib.IsKeyDown(SettingsManager.Settings.Forwards)) force += direction;
		if (Raylib.IsKeyDown(SettingsManager.Settings.Backwards)) force -= direction;

		// Left and right/strafing keyboard input (A, D)
		if (Raylib.IsKeyDown(SettingsManager.Settings.Left)) force -= right;
		if (Raylib.IsKeyDown(SettingsManager.Settings.Right)) force += right;

		// Add the "speed" to the force applied and normalize
		if (force != Vector3.Zero) force = Vector3.Normalize(force);
		force *= moveForce;

		// Apply the force to the velocity
		Velocity += force / Mass;

		// Apply friction to slow down the player overtime
		Vector3 friction = -frictionCoefficient * Velocity;
		Velocity += friction;
		if (Velocity.LengthSquared() < 0.1f) Velocity = Vector3.Zero;

		//! Remove Y velocity
		// TODO: Remove this when adding gravity. If adding a noclip/spectator mode then remove everything related to Y, and the player will fly in direction they looking
		Velocity = new Vector3(Velocity.X, 0, Velocity.Z);

		// Update the players position based on the velocity
		Vector3 newPosition = Position;
		newPosition += Velocity * Raylib.GetFrameTime();
		Position = newPosition;

		// Move and update the camera
		Camera.position = newPosition;
		Camera.target = Camera.position + direction;
	}



	// Get a whole ton of rubbish info about the player
    public override string ToString()
    {
        return $"Speed: {Speed} thingys per delta time idk\nVelocity: {Velocity}\nYaw: {Yaw}\tPitch: {Pitch}\nMass: {Mass}meters (fat)";
    }

}