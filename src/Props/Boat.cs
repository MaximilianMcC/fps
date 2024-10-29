using System.Numerics;
using Raylib_cs;

class Temp : Prop
{
	private Model model;

	public override void Start()
	{
		// Load the model and texture
		// model = Raylib.LoadModel("./assets/testAutomated.glb");
		model = AssetManager.LoadGlbModel("./assets/testAutomated.glb");
	}

	// Render the model
	public override void Render3D()
	{
		Raylib.DrawModel(model, Vector3.Zero, 1f, Color.White);
	}

	public override void CleanUp()
	{
		Raylib.UnloadModel(model);
	}
}