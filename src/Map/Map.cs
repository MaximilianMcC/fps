using System.Numerics;
using Raylib_cs;

class Map
{	
	public static readonly float Gravity = 9.81f;
	private static readonly Color skyColor = Color.Black;

	// Props
	private static Prop ground;
	private static Prop fence;


	public static void Load()
	{
		ground = new Prop("ground", "pavement");
		fence = new Prop("fence", "chain-link-fence");
	}

	public static void Render()
	{
		// Draw the sky
		// TODO: Use a skybox
		//! also doing normal clear screen thing so cant remove
		Raylib.ClearBackground(skyColor);

		// Ground and fence
		ground.Render(Vector3.Zero, Vector3.UnitY, 0f);
		
		const float length = 3f;
		for (int i = 0; i < 10; i++)
		{
			fence.Render(new Vector3(5f - (i * length), 0f, -10f), Vector3.UnitY, 0f);
		}
	}

	public static void CleanUp()
	{
		// Unload everything
		ground.Unload();
		fence.Unload();
	}

	public static float GetGroundY(Vector3 positionOnMap)
	{
		// TODO: Make this work with ground and stuff
		return 0f;
	}
}


class Prop
{
	public Texture2D Texture;
	public Model Model;

	public Prop(string modelPath, string texturePath)
	{
		// Paths
		const string texturesPath = "./assets/texture/";
		const string modelsPath = "./assets/model/";

		// Load everything
		Texture = Raylib.LoadTexture(texturesPath + texturePath + ".png");
		Model = Raylib.LoadModel(modelsPath + modelPath + ".obj");

		// Apply the texture to the model
		Raylib.SetMaterialTexture(ref Model, 0, MaterialMapIndex.Albedo, ref Texture);
	}

	// TODO: Maybe use DrawMeshInstanced
	// TODO: Make a Debug.ShowMesh variable toggle thingy to view the meshes
	public void Render(Vector3 position, Vector3 rotationAxis, float rotationAmount)
	{
		Raylib.DrawModelEx(Model, position, rotationAxis, rotationAmount, Vector3.One, Color.White);
	}

	// TODO: Collision method

	public void Unload()
	{
		Raylib.UnloadTexture(Texture);
		Raylib.UnloadModel(Model);
	}
}