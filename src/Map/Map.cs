using System.Numerics;
using Raylib_cs;

class Map
{
	private static List<Prop> props;
	
	// TODO: Make 9.81
	public static readonly float Gravity = 9.81f;
	// public static readonly float Gravity = f;


	public static void Load()
	{
		string texturePath = "./assets/texture/";
		string modelPath = "./assets/model/";

		// Create the props list to store
		// all of the props
		props = new List<Prop>();

		// Load in everything
		props.Add(new Prop(modelPath + "ground", texturePath + "pavement"));

	}

	public static void Render()
	{
		// Ground
		Raylib.DrawModel(props[0].Model, Vector3.Zero, 1f, Color.White);
	}

	public static void CleanUp()
	{
		// Unload everything
		foreach (Prop prop in props) prop.Unload();
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
		// Load everything
		Texture = Raylib.LoadTexture(texturePath + ".png");
		Model = Raylib.LoadModel(modelPath + ".obj");

		// Apply the texture to the model
		Raylib.SetMaterialTexture(ref Model, 0, MaterialMapIndex.Albedo, ref Texture);
	}

	public void Unload()
	{
		Raylib.UnloadTexture(Texture);
		Raylib.UnloadModel(Model);
	}
}