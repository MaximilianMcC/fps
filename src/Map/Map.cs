
//! EVERYTHING IN HERE IS TEMPORARY
using System.Numerics;
using Raylib_cs;

class Map
{
	private static Texture2D containerTexture;
	private static Model containerModel;

	private static Texture2D lightTexture;
	private static Model lightModel;

	public static void Load()
	{
		containerTexture = Raylib.LoadTexture("./assets/texture/container.png");
		containerModel = Raylib.LoadModel("./assets/model/container.obj");
		Raylib.SetMaterialTexture(ref containerModel, 0, MaterialMapIndex.MATERIAL_MAP_ALBEDO, ref containerTexture);		

		lightTexture = Raylib.LoadTexture("./assets/texture/light.png");
		lightModel = Raylib.LoadModel("./assets/model/light.obj");
		Raylib.SetMaterialTexture(ref lightModel, 0, MaterialMapIndex.MATERIAL_MAP_ALBEDO, ref lightTexture);		

	}

	public static void Render()
	{
		Raylib.DrawModelEx(containerModel, new Vector3(10, 0, 10), Vector3.Zero, 0f, Vector3.One, Color.WHITE);
		Raylib.DrawModelEx(containerModel, new Vector3(10, 2.36f, 10), Vector3.Zero, 0f, Vector3.One, Color.WHITE);

		Raylib.DrawModelEx(lightModel, new Vector3(15, 0, 8), Vector3.Zero, 0f, Vector3.One, Color.WHITE);

	}

	public static void Unload()
	{
		Raylib.UnloadTexture(containerTexture);
		Raylib.UnloadModel(containerModel);

		Raylib.UnloadTexture(lightTexture);
		Raylib.UnloadModel(lightModel);
	}

}