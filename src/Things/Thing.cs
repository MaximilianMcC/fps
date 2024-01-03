using System.Numerics;

class Thing
{
	public string Name { get; set; }
	public Vector3 Position { get; set; }
	public Vector3 Rotation { get; set; }
	public Vector3 Scale { get; set; }

	public Thing(string name)
	{
		// Default values
		Name = name;
		Position = Vector3.Zero;
		Rotation = Vector3.Zero;
		Scale = Vector3.One;
	}

	public virtual void Start()
	{
		
	}

	public virtual void Update()
	{

	}

	public virtual void Render()
	{
		
	}

	public virtual void Cleanup()
	{
		
	}
}