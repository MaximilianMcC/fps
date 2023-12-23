using System.Numerics;
using Raylib_cs;

class Thing
{

	// General stuff
	public string Name { get; set; }

	// Positional stuff
	public Vector3 Position { get; set; }
	public Vector3 Rotation { get; set; }
	public Vector3 Scale { get; set; }

	// 3D stuff
	public Model Model { get; set; }
	private Texture2D[] textures;

	// Make a new thing
	public Thing(string name, Vector3 position, Vector3 rotation, string modelPath, params string[] texturePaths)
	{
		// Assign basic values
		Name = name;
		Position = position;
		Rotation = rotation;
		Scale = Vector3.One;

		// Load in the model
		//? The local model variable is created as you can't get a reference
		//? to a property because it's just a different way to writing a method
		//? (Not a real variable)
		Model model = Raylib.LoadModel(modelPath);

		// Load and apply the textures to the model
		textures = new Texture2D[texturePaths.Length];
		for (int i = 0; i < texturePaths.Length; i++)
		{
			// Load the texture and apply it to the model
			textures[i] = Raylib.LoadTexture(texturePaths[i]);
			Raylib.SetMaterialTexture(ref model, i, MaterialMapIndex.MATERIAL_MAP_ALBEDO, ref textures[i]);
		}

		// Set the final model, with all applied textures and whatnot
		Model = model;
	}


	// Runs a single time before Update();
	public void Start()
	{

	}

	// Runs every frame
	public void Update()
	{

	}

	// Runs after Update();
	public void Render()
	{
		// Draw the model
		Raylib.DrawModelEx(Model, Position, Rotation, 0f, Scale, Color.WHITE);
	}

	// Runs when you are done with using it
	// TODO: Rename to 'dispose' or 'finish' (age appropriate!!)
	public void Die()
	{
		// Unload the model
		Raylib.UnloadModel(Model);

		// Unload the textures
		foreach (Texture2D texture in textures)
		{
			Raylib.UnloadTexture(texture);
		}
	}

    public override string ToString()
    {
        return Name;
    }
}