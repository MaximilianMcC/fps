using System.Numerics;
using Raylib_cs;

class Player
{
	// Position stuff
	public static Vector3 Position { get; set; }
	public static float Pitch { get; set; }
	public static float Yaw { get; set; }

	// Camera stuff
	public static Camera3D Camera;
	public static RenderTexture2D CameraOutput;
	private static Rectangle outputSource;

	// Dimensions
	private static float height = 1.65f;
	private static float eyeYFromTopOfHead = 0.15f;

	// Movement stuff
	private static Vector3 forwardDirection;
	private static Vector3 forward;
	private static Vector3 right;
	private static float walkForce;

	// Physics stuff
	private static Vector3 velocity;
	private static float mass = 62f;
	private static float frictionCoefficient = 0.1f;

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

		// Set forces based on weight
		//? Values from clyde💔 never forget🙏
		walkForce = mass * 1.2f;
		Console.WriteLine(mass);

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

	// TODO: If player hasn't moved don't do all this movement maths, but still do gravity and stuff
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
		velocity += movementForce / mass;

		// Apply friction to slow down the player overtime
		// and if their velocity is tiny then just stop them
		Vector3 friction = -frictionCoefficient * velocity;
		velocity += friction;
		if (velocity.LengthSquared() < 0.1f) velocity = Vector3.Zero;

		// Remove Y velocity
		// TODO: Remove this when adding gravity. If adding a noclip/spectator mode then remove everything related to Y, and the player will fly in direction they looking
		velocity.Y = 0;

		// Update the players position based on
		// their new velocity and also apply
		// delta time to make it frame independent
		Vector3 newPosition = Position;
		newPosition += velocity * Raylib.GetFrameTime();

		// TODO: Collision

		// Apply the final position
		Position = newPosition;
	}

}