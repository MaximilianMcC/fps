using System.Numerics;
using Raylib_cs;

class Chel : Entity
{
	private Model cel;
	private float rotation;

	public override void Start()
	{
		cel = AssetManager.LoadGlbModel("./assets/chel.glb");
		Position.Y = 0.5f;
	}

	override public void Update()
	{
		// Spin
		const float rotationSpeed = 25f;
		rotation += rotationSpeed * Raylib.GetFrameTime();

		// Floating up and down
		const float hoverHeight = 0.3f;
		const float hoverSpeed = 3f;
		Position.Y = 0.5f + MathF.Sin((float)Raylib.GetTime() * hoverSpeed) * hoverHeight;
	}

	public override void Render3D()
	{
		Raylib.DrawModelEx(cel, Position, Vector3.UnitY, rotation, Vector3.One, Color.White);
	}


	public override void CleanUp()
	{
		Raylib.UnloadModel(cel);
	}
}