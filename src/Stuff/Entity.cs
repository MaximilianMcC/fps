using System.Numerics;

class Entity : Updatable
{
	// Position and rotation of the thing
	public Vector3 Position = Vector3.Zero;
	public Vector3 Velocity = Vector3.Zero;
	public Quaternion Rotation = Quaternion.Identity;
}