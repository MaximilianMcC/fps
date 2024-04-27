using System.Numerics;
using Raylib_cs;

class Player
{
	// Position stuff
	public static Vector3 Position;
	public static float Pitch;
	public static float Yaw;

	// Camera stuff
	public static Camera3D Camera;
	public static RenderTexture2D CameraOutput;
	private static Rectangle outputSource;

	// Dimensions
	public static BoundingBox BoundingBox;
	private static float width = 0.5f;
	private static float height = 1.65f;
	private static float length = 0.4f;
	private static float eyeYFromTopOfHead = 0.15f;
	private static float legLength = height * 0.45f;
	private static float strideLength = legLength * 1.5f;

	// Physics stuff
	private static Vector3 velocity;
	private static float frictionCoefficient = 0.1f;

	// Movement stuff
	//? Force values from clyde💔 never forget🙏
	private static Vector3 forwardDirection;
	private static Vector3 forward;
	private static Vector3 right;
	private static bool onGround;

	// force stuff
	private static float mass = 62f / Map.Gravity;
	private static float jumpForce = mass * 0.4f;
	private static float walkForce = mass * 0.5f;

	// Sounds
	private static double lastTimeFootstepSoundPlayed;
	private static Sound[] footsteps;

	public static void Start(int renderWidth, int renderHeight)
	{
		// Setup the cameras render texture for getting output to the screen
		CameraOutput = Raylib.LoadRenderTexture(renderWidth, renderHeight);
		outputSource = new Rectangle(0f, 0f, renderWidth, -renderHeight);

		// Create the actual camera
		Camera = new Camera3D()
		{
			Position = Position + new Vector3(0, height - eyeYFromTopOfHead, 0),
			Target = Vector3.Zero,
			Up = Vector3.UnitY,
			FovY = Settings.Fov,
			Projection = CameraProjection.Perspective
		};

		// Create the bounding box for collisions and stuff
		// TODO: Maybe make the player a cylinder for better collisions or something
		BoundingBox = new BoundingBox(
			new Vector3(0, 0, 0),
			new Vector3(width, height, length)
		);


		// Load the footstep sounds
		footsteps = new Sound[]
		{
			Raylib.LoadSound("./assets/sound/footstep-1.ogg"),
			Raylib.LoadSound("./assets/sound/footstep-2.ogg"),
			Raylib.LoadSound("./assets/sound/footstep-3.ogg"),
			Raylib.LoadSound("./assets/sound/footstep-4.ogg"),
			Raylib.LoadSound("./assets/sound/footstep-5.ogg")
		};

		// Keep the players mouse in the centre of the screen
		// and hide it so they can look around normally
		Raylib.HideCursor();
		Raylib.DisableCursor();
	}

	public static void Update()
	{
		// Get player movement and mouse movement
		HeadMovement();
		BodyMovement();

		// Move and update the camera
		Camera.Position = Position + new Vector3(0, height - eyeYFromTopOfHead, 0);
		Camera.Target = Camera.Position + forwardDirection;
	}

	public static void Render()
	{
		// TODO: Only calculate destination when the window is resized
		Rectangle destination = new Rectangle(0f, 0f, Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
		Raylib.DrawTexturePro(CameraOutput.Texture, outputSource, destination, Vector2.Zero, 0f, Color.White);

		Raylib.DrawText($"Velocity: {velocity}\n\n\nPosition: {Position}\n\n\non ground rn {onGround}", 10, 10, 45, Color.White);
	}
	
	public static void CleanUp()
	{
		// Unload all the footstep sounds
		for (int i = 0; i < footsteps.Length; i++)
		{
			Raylib.UnloadSound(footsteps[i]);
		}
	}







	private static void HeadMovement()
	{
		// Get the mouse movement in degrees
		Vector2 mouseDelta = Raylib.GetMouseDelta() * Settings.Sensitivity;
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
		forward = Vector3.Transform(-Vector3.UnitZ, rotationNoPitch);
		right = Vector3.Transform(Vector3.UnitX, rotationNoPitch);

		// Assign the direction Forward for controlling camera rotation
		forwardDirection = Vector3.Transform(-Vector3.UnitZ, rotation);
	}

	private static void BodyMovement()
	{
		// Get the movement forces direction
		Vector3 movementForce = Vector3.Zero;

		// Forwards/backwards
		if (Raylib.IsKeyDown(Settings.Forwards)) movementForce += forward;
		if (Raylib.IsKeyDown(Settings.Backwards)) movementForce -= forward;

		// Left/right
		if (Raylib.IsKeyDown(Settings.Left)) movementForce -= right;
		if (Raylib.IsKeyDown(Settings.Right)) movementForce += right;

		// Apply speed to the direction and normalize
		if (movementForce != Vector3.Zero) movementForce = Vector3.Normalize(movementForce);
		movementForce *= walkForce;

		// Add the new force to the velocity
		// so the player actually moves
		//TODO: Figure out why divide by mass I forgot
		velocity.X += movementForce.X / mass;
		velocity.Z += movementForce.Z / mass;


		// Apply friction to slow down the player overtime
		// and if their velocity is tiny then just stop them
		// And don't put it on the Y because its 
		Vector3 friction = -frictionCoefficient * velocity;
		velocity.X += friction.X;
		velocity.Z += friction.Z;
		if (velocity.LengthSquared() < 0.1f) velocity *= new Vector3(0f, 1f, 0f);


		// Apply gravity to make the player fall
		// TODO: Make mass do something to this
		velocity.Y -= Map.Gravity * Raylib.GetFrameTime();


		// Check for if the player wants to jump
		if (Raylib.IsKeyDown(Settings.Jump) && onGround)
		{
			velocity.Y = jumpForce;
			onGround = false;
		}

		// Calculate the time between a footstep based on
		// the current speed and check for if we need to play
		// a footstep sound
		// TODO: Chuck in another method
		//! Length returns the real speed, but its very expensive
		//? the * 2 for the footstep timing isn't actually real, but it sounds better. If change speed then can remove it
		float timeBetweenFootstep = (strideLength / velocity.Length()) * 2;
		double currentTime = Raylib.GetTime();
		if ((currentTime - lastTimeFootstepSoundPlayed) >= timeBetweenFootstep && onGround)
		{
			// Update the timing stuff
			lastTimeFootstepSoundPlayed = currentTime;

			// Play a random footstep sound
			Raylib.PlaySound(footsteps[Raylib.GetRandomValue(0, footsteps.Length - 1)]);
		}


		// Update the players position based on
		// their new velocity and also apply
		// delta time to make it frame independent
		Vector3 newPosition = Position;
		newPosition += velocity * Raylib.GetFrameTime();

		// Get the ground position based on the players new position.
		// If the player is falling into the floor then that means
		// they would have landed. Reset all their variables.
		float groundY = Map.GetGroundY(newPosition);
		if (newPosition.Y < groundY)
		{
			velocity.Y = 0f;
			newPosition.Y = groundY;
			onGround = true;
		}

		// TODO: Collision with stuff in map

		// Apply the final position
		Position = newPosition;
	}

}