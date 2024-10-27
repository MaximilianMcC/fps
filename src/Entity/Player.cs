using System.Numerics;
using Raylib_cs;

class Player : Entity
{
	public Camera3D Camera;
	Quaternion rotation = Quaternion.Identity;
	public float Yaw;
	public float Pitch;

	private float eyeHeight = 1.7f;
	private float sensitivity = 250f;

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
		// Target.X = Horizontal rotation
		// Target.Y = Vertical rotation
		// Target.Z = Direction

		// Get the direction (Z) using the
		// rotation quaternion
		Vector3 direction = Vector3.Transform(Vector3.UnitZ, rotation);

		// Update the position, and the
		// target based on the position
		Camera.Position = Position + (Vector3.UnitY * eyeHeight);
		Camera.Target = Position + (Vector3.UnitY * eyeHeight) + direction;
	}

	private void Movement()
	{
		// Get what direction the player is looking at
		// and update the yaw and pitch accordingly
		//? Using 89 instead of 90 to avoid gimbal lock
		Vector2 mouseMovement = Raylib.GetMouseDelta() * sensitivity * Raylib.GetFrameTime();
		Yaw -= mouseMovement.X;
		Pitch = Math.Clamp(Pitch + mouseMovement.Y, -89f, 89f);

		// Update the quaternion rotation thingy based
		// on the newly updated yaw and pitch values
		// converted to radians 
		float yaw = Yaw * Raylib.DEG2RAD;
		float pitch = Pitch * Raylib.DEG2RAD;
		rotation = Quaternion.CreateFromYawPitchRoll(yaw, pitch, 0f);
	}

	public override void Update()
	{
		Movement();
		UpdateCamera();
	}

	public override void Render2D()
	{
		Raylib.DrawText($"{Camera.Position}\n\n{Camera.Target}\n\n\n\n{Position}", 10, 10, 35, Color.White);
	}
}
