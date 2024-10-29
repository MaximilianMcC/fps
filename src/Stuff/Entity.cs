using System.Numerics;

class Entity : Updatable
{
	// Position and rotation of the thing
	public Vector3 Position = Vector3.Zero;
	public Quaternion Rotation = Quaternion.Identity;
}