using System.Numerics;
using Raylib_cs;

class Map
{	
	public static readonly float Gravity = 9.81f;
	
	private static readonly Color skyColor = Color.Black;

	// Props
	private static Prop ground;
	private static Prop fence;
	private static Prop boxTruck;
	private static Prop container1;
	private static Prop container2;
	private static Prop toilet;


	public static void Load()
	{
		ground = new Prop("ground", "pavement");
		fence = new Prop("fence", "chain-link-fence");
		boxTruck = new Prop("truck", "box-truck");
		container1 = new Prop("container", "container-2");
		container2 = new Prop("container", "container-3");
		toilet = new Prop("toilet", "toilet");
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

		container2.Render(new Vector3(0, 0, -6), Vector3.UnitY, 0f);
		container1.Render(new Vector3(0, 2.5f, -6), Vector3.UnitY, 0f);
		toilet.Render(new Vector3(-2f, 0f, -8f), Vector3.UnitY, 0f);

		// Truck
		boxTruck.Render(Vector3.Zero, Vector3.UnitZ, 0f, true);


		Raylib.DrawBoundingBox(Player.BoundingBox, Color.Green);
	}

	public static void CleanUp()
	{
		// Unload everything
		ground.Unload();
		fence.Unload();
		boxTruck.Unload();
		container1.Unload();
		container2.Unload();
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
	private BoundingBox[] boundingBoxes;

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
		
		// Get its bounding box
		unsafe
		{
			// Loop through every mesh in the model and get its bounding box
			boundingBoxes = new BoundingBox[Model.MeshCount];
			for (int i = 0; i < boundingBoxes.Length; i++)
			{
				boundingBoxes[i] = Raylib.GetMeshBoundingBox(Model.Meshes[i]);
			}
		}
	}

	// TODO: Maybe use DrawMeshInstanced
	// TODO: Make a Debug.ShowMesh variable toggle thingy to view the meshes
	public void Render(Vector3 position, Vector3 rotationAxis, float rotationAmount, bool showCollision = false)
	{
		// Render the model
		Raylib.DrawModelEx(Model, position, rotationAxis, rotationAmount, Vector3.One, Color.White);

		if (showCollision)
		{
			// Loop through every mesh (thing) in the model
			// and draw its bounding box
			for (int i = 0; i < Model.MeshCount; i++)
			{
				Raylib.DrawBoundingBox(boundingBoxes[i], Color.Green);
			}
		}
	}

	// Check for collision against the player
	public bool CollidingWithPlayer()
	{
		for (int i = 0; i < boundingBoxes.Length; i++)
		{
			if (Raylib.CheckCollisionBoxes(Player.BoundingBox, boundingBoxes[i])) return true;
		}
		
		return false;
	}

	public void Unload()
	{
		Raylib.UnloadTexture(Texture);
		Raylib.UnloadModel(Model);
	}
}