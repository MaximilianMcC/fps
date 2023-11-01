using System.Numerics;
using Raylib_cs;

class Player
{
	// public Camera3D Camera;
	public Camera3D Camera;
	public Vector3 Position = Vector3.Zero;

	// Make a new player
	public Player()
	{
		// Create the raylib camera
		Camera = new Camera3D
		{
			position = new Vector3(Position.X, Position.Y + 1, Position.Z),
			target = Position + Vector3.UnitZ,
			up = Vector3.UnitY,
			fovy = Settings.Fov,
			projection = CameraProjection.CAMERA_PERSPECTIVE
		};
	
		//! debug
		// Settings.Sensitivity = 0.00001f;
	}

	// Update
	public void Update(float deltaTime)
	{
		Movement(deltaTime);

		if (Raylib.IsKeyPressed(KeyboardKey.KEY_F9))
		{
			Console.WriteLine("Should eb able to se cube rn🙏🙏");
			Camera.target = new Vector3(-463999f, -150332f, 0f);
			Camera.position = new Vector3(0, 1, 0);
		}
	}

	private void Movement(float deltaTime)
	{
		float speed = 0.00001f;

		// Looking around with mouse
		Vector2 mouseDelta = Raylib.GetMouseDelta() / Settings.Sensitivity;
		Camera.target.X += mouseDelta.X;
		Camera.target.Y -= mouseDelta.Y;


		// Forwards/backwards movement
		if (Raylib.IsKeyDown(Settings.Forwards)) Camera.position += (Camera.target * speed) * deltaTime;
		if (Raylib.IsKeyDown(Settings.Backwards)) Camera.position -= (Camera.target * speed) * deltaTime;

		// Left/right movement (strafing)
		if (Raylib.IsKeyDown(Settings.Left)) Camera.position -= (Vector3.Cross(Camera.target, Camera.up) * speed) * deltaTime;
		if (Raylib.IsKeyDown(Settings.Right)) Camera.position += (Vector3.Cross(Camera.target, Camera.up) * speed) * deltaTime;


		// Move with mouse
		Camera.position -= Vector3.Cross(Camera.target, Camera.up) * ((mouseDelta.X / Settings.Sensitivity) * deltaTime);
	}


}