using System.Numerics;
using Raylib_cs;

class Player
{
	//TODO: Add getters and setters
	public Camera3D Camera;
	public Vector3 Position = Vector3.Zero;
	public Vector3 Direction = Vector3.Zero;

	// Movement values
	public bool Running { get; private set; }
	private const float walkSpeed = 10;
	private const float runSpeed = 15;

	// Mouse options and whatnot
	private const float sensitivity = 0.05f;
	private const float fov = 60f;

	public Player()
	{
		Camera = new Camera3D
		{
			position = new Vector3(-1, 1, 0),
			target = new Vector3(0, 2, 0),
			up = new Vector3(0, 1, 0),
			fovy = fov,
			projection = CameraProjection.CAMERA_PERSPECTIVE
		};
	}

	public void Update(float deltaTime)
	{
		Movement(deltaTime);
	}

	public void Movement(float deltaTime)
	{
		// Check for if they are walking or running
		float speed = walkSpeed;
		Running = false;
		if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL))
		{
			speed = runSpeed;
			Running = true;
		}

		// Get movement input for going forwards and back (W, S)
		float forwardsBackMovement = 0;
		if (Raylib.IsKeyDown(KeyboardKey.KEY_W)) forwardsBackMovement = 1;
		if (Raylib.IsKeyDown(KeyboardKey.KEY_S)) forwardsBackMovement = -1;

		// Get movement input for going left and right (A, D)
		float leftRightMovement = 0;
		if (Raylib.IsKeyDown(KeyboardKey.KEY_A)) leftRightMovement = 1;
		if (Raylib.IsKeyDown(KeyboardKey.KEY_D)) leftRightMovement = -1;

		// Turn the movement into a direction and normalize it if needed
		Vector3 moveDirection = new Vector3(forwardsBackMovement, 0, leftRightMovement);
		if (moveDirection.Length() > 1.0f) moveDirection = Vector3.Normalize(moveDirection);
		
		// Calculate the new movement position based on where the player is looking
		Vector3 newPosition = Position + (moveDirection * speed) * deltaTime;
		Vector3 forward = Vector3.Normalize(Camera.target - Position);
		Vector3 right = Vector3.Cross(Camera.up, forward);
		// newPosition = newPosition + right * leftRightMovement * speed * deltaTime;
		newPosition = newPosition + right * leftRightMovement * speed * deltaTime;
		Position = newPosition;

		// Calculate the new look direction
		Vector2 mouseDelta = Raylib.GetMouseDelta();
		Camera.target += new Vector3(-mouseDelta.X * sensitivity, -mouseDelta.Y * sensitivity, 0);
		
		// Update the cameras position and direction
		Camera.position = Position;
	}



}