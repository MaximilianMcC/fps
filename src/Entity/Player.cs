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
	private static float height = 1.65f;
	private static float eyeYFromTopOfHead = 0.15f;

	// Physics stuff
	private static Vector3 velocity;
	private static float mass = 62f;
	private static float frictionCoefficient = 0.1f;

	// Movement stuff
	//? Force values from clyde💔 never forget🙏
	private static Vector3 forwardDirection;
	private static Vector3 forward;
	private static Vector3 right;
	private static bool onGround;
	private static float walkForce = mass * 1.2f;
	private static float jumpForce = mass * 0.04f;

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
			FovY = 60f,
			Projection = CameraProjection.Perspective
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

	}







	private static void HeadMovement()
	{
		// Get the mouse movement in degrees
		Vector2 mouseDelta = Raylib.GetMouseDelta() * 150;
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
		if (Raylib.IsKeyDown(KeyboardKey.W)) movementForce += forward;
		if (Raylib.IsKeyDown(KeyboardKey.S)) movementForce -= forward;

		// Left/right
		if (Raylib.IsKeyDown(KeyboardKey.A)) movementForce -= right;
		if (Raylib.IsKeyDown(KeyboardKey.D)) movementForce += right;

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
		Vector3 friction = -frictionCoefficient * velocity;
		velocity.X += friction.X;
		velocity.Z += friction.Z;
		if (velocity.LengthSquared() < 0.1f) velocity *= new Vector3(0f, 1f, 0f);


		//! debug
		if (Raylib.IsKeyPressed(KeyboardKey.Up)) Position.Y	= 100f;


		// Apply gravity to make the player fall
		// TODO: See if delta time is actually required here
		// TODO: Make mass do something to this
		velocity.Y -= Map.Gravity * Raylib.GetFrameTime();


		// Check for if the player wants to jump
		if (Raylib.IsKeyPressed(KeyboardKey.Space) && onGround)
		{
			Console.WriteLine("jumping rn (eligible)");
			velocity.Y = jumpForce;
			onGround = false;
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