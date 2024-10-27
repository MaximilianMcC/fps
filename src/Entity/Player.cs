using System.Numerics;
using Raylib_cs;

class Player : Entity
{
	public Camera3D Camera;
	private float EyeHeight = 1.7f;

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

		Camera.Position = Position + (Vector3.UnitY * EyeHeight);
		Camera.Target = Position + (Vector3.UnitY * EyeHeight) + Vector3.UnitZ;
	}

	public override void Update()
	{
		Position.X += 10 * Raylib.GetFrameTime();
		UpdateCamera();
	}

	public override void Render2D()
	{
		Raylib.DrawText($"{Camera.Position}\n\n{Camera.Target}\n\n\n\n{Position}", 10, 10, 35, Color.White);
	}
}
