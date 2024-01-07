using System.Numerics;
using Raylib_cs;

class Player
{
	// Camera and movement stuff
	public Camera3D Camera;
	public Vector3 Position { get; set; } = new Vector3(0, 0, 0);
	public float Pitch { get; set; } = 0;
	public float Yaw { get; set; } = 0;

	// Dimensions and stuff
	public float Height { get; private set; }
	public bool Crouching { get; set; } = false;
	private float standingHeight = 1.75f; //? Meter
	private float crouchingHeight = 1f; //? Meter
	private float eyeYFromTopOfHead = 0.15f; //? Meter

	// Physics stuff
	// TODO: Figure out why the player sinks into the ground (something to do with friction i think)
	public float Mass { get; private set; } = 62; //? kilograms
	public Vector3 Velocity { get; set; } = Vector3.Zero;
	private float frictionCoefficient = 0.1f;

	// Movement stuff
	public float Speed { get; private set; }
	private float crouchForce;
	private float walkForce;
	private float runForce;
	private Vector3 forward;
	private Vector3 forwardDirection;
	private Vector3 right;

	// Interaction stuff
	public bool MovementLocked = false;
	public bool CameraLocked = false;
	public Ray Ray;
	public RayCollision RayCollision;

	// Make a new player
	public Player()
	{
		// Set the player height
		Height = standingHeight;

		// Create the raylib camera
		Camera = new Camera3D
		{
			position = new Vector3(Position.X, Position.Y + (Height - eyeYFromTopOfHead), Position.Z),
			target = Vector3.Zero,
			up = Vector3.UnitY,
			fovy = SettingsManager.Settings.Fov,
			projection = CameraProjection.CAMERA_PERSPECTIVE
		};

		// Set the movement forces so that they are proportional to body weight
		//! I asked Clyde for average horizontal force applied when walking and they said 0.5 - 1.5
		walkForce = Mass * 1.2f;
		runForce = Mass * 1.5f;
		crouchForce = Mass * 0.4f;
	}

	// Update
	public void Update()
	{
		// Move the player if we're allowed to
		if (!CameraLocked) MouseMovement();
		if (!MovementLocked) KeyboardMovement();
		

		// Other stuff
		// TODO: Don't check for if camera locked again
		if (!CameraLocked) Zoom();

		// Move and update the camera
		Camera.position = new Vector3(Position.X, Position.Y + (Height - eyeYFromTopOfHead), Position.Z);
		Camera.target = Camera.position + forwardDirection;

		// Set the speed
		// TODO: Don't do every frame because getting length is expensive
		Speed = Velocity.Length();
	}

	// Looking around and stuff
	private void MouseMovement()
	{
		// Get the mouse movement in degrees
		Vector2 mouseDelta = Raylib.GetMouseDelta() * SettingsManager.Settings.Sensitivity;
		float mouseX = mouseDelta.X / Raylib.GetScreenWidth();
		float mouseY = mouseDelta.Y / Raylib.GetScreenHeight();

		// Update the pitch and yaw values
		Pitch -= mouseY;
		Yaw -= mouseX;

		// Clamp the pitch from -90 to 90 in degrees
		// and keep yaw between 0 and 360 degrees
		// TODO: Remove gimbal lock
		Pitch = Math.Clamp(Pitch, -90f, 90f);
		Yaw %= 360f;

		// Convert into a quaternion to make working with rotations easier
		Quaternion rotation = Quaternion.CreateFromYawPitchRoll(Utils.DegreesToRadians(Yaw), Utils.DegreesToRadians(Pitch), 0);
		Quaternion rotationNoPitch = Quaternion.CreateFromYawPitchRoll(Utils.DegreesToRadians(Yaw), 0, 0);

		// Calculate target for moving forwards and right
		// No pitch is used to stop the player from speeding up/slowing down depending on pitch
		forward = Vector3.Transform(new Vector3(0, 0, -1), rotationNoPitch);
		right = Vector3.Transform(new Vector3(1, 0, 0), rotationNoPitch);

		// Assign the direction Forward for controlling camera rotation
		forwardDirection = Vector3.Transform(new Vector3(0, 0, -1), rotation);
	}


	private void KeyboardMovement()
	{
		// Check for if the player wants to crouch
		if (Raylib.IsKeyDown(SettingsManager.Settings.Crouch))
		{
			Crouching = true;
			Height = crouchingHeight;
		}
		else
		{
			Crouching = false;
			Height = standingHeight;
		}

		// Check for if the player wants to run or walk
		float moveForce = walkForce;
		if (Raylib.IsKeyDown(SettingsManager.Settings.Sprint) && !Crouching)
		{
			moveForce = runForce;
		}
		else if (Crouching) moveForce = crouchForce;

		Vector3 force = Vector3.Zero;

		// Forwards and backwards keyboard input (W, S)
		// TODO: Make walking backwards slower
		if (Raylib.IsKeyDown(SettingsManager.Settings.Forwards)) force += forward;
		if (Raylib.IsKeyDown(SettingsManager.Settings.Backwards)) force -= forward;

		// Left and right/strafing keyboard input (A, D)
		if (Raylib.IsKeyDown(SettingsManager.Settings.Left)) force -= right;
		if (Raylib.IsKeyDown(SettingsManager.Settings.Right)) force += right;

		// Add the "speed" to the force applied and normalize
		if (force != Vector3.Zero) force = Vector3.Normalize(force);
		force *= moveForce;

		// Apply the force to the velocity
		Velocity += force / Mass;

		// Apply friction to slow down the player overtime
		// TODO: Use a different friction for the Y component (air resistance)
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
	}

	private void Zoom()
	{
		// Check for if the player is holding down the zoom button
		// TODO: Lerp it
		if (Raylib.IsKeyDown(SettingsManager.Settings.Zoom))
		{
			// Zoom in a bit
			// TODO: Add zoom level to settings
			Camera.fovy = SettingsManager.Settings.Fov / 2;
		}
		else
		{
			// TODO: Don't set every frame
			Camera.fovy = SettingsManager.Settings.Fov;
		}
	}


	// Get a whole ton of rubbish info about the player
	public override string ToString()
	{
		return $"Speed: {Speed} thingys per delta time idk" +
			$"\nVelocity: {Velocity}" +
			$"\nYaw: {Yaw}\tPitch: {Pitch}" +
			$"\nMass: {Mass} meters (fat)" +
			$"\nCrouching: {Crouching}" +
			$"\n\nPosition: {Position}" + 
			$"\nCamera Position: {Camera.position}" +
			$"\n\nKeyboard movement Locked: {MovementLocked}" +
			$"\nMouse movement Locked: {CameraLocked}";
	}

}