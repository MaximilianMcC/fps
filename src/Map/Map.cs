using System.Numerics;
using System.Runtime.CompilerServices;
using Raylib_cs;

class Map
{
	private static List<Prop> props;
	
	public static readonly float Gravity = 9.81f;

	public static void Load()
	{
		string texturePath = "./assets/texture/";
		string modelPath = "./assets/model/";

		// Create the props list to store
		// all of the props
		props = new List<Prop>();

		// Load in everything
		props.Add(new Prop(modelPath + "ground", texturePath + "pavement"));

		props.Add(new Prop(modelPath + "container", texturePath + "container-1"));
		props.Add(new Prop(modelPath + "container", texturePath + "container-2"));
		props.Add(new Prop(modelPath + "container", texturePath + "container-3"));

		props.Add(new Prop(modelPath + "toilet", texturePath + "toilet"));
	}

	public static void Render()
	{
		// Ground
		Raylib.DrawModel(props[0].Model, Vector3.Zero, 1f, Color.White);

		// Containers
		Raylib.DrawModel(props[1].Model, new Vector3(10, 0f, 0f), 1f, Color.White);
		Raylib.DrawModel(props[1].Model, new Vector3(10f, 2.6f, 0f), 1f, Color.White);
		Raylib.DrawModelEx(props[3].Model, new Vector3(10, 0, 10), Vector3.UnitY, 45f, Vector3.One, Color.White);
		Raylib.DrawModelEx(props[2].Model, new Vector3(10, 0, 5), Vector3.UnitY, 90f, Vector3.One, Color.White);

		// Toilet
		Raylib.DrawModelEx(props[4].Model, new Vector3(10, 0, 15), Vector3.UnitY, 45f, Vector3.One, Color.White);
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