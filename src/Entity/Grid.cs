using System.Numerics;
using Raylib_cs;

class Grid : Entity
{
	public override void Render3D()
	{
		Raylib.DrawGrid(10, 1f);
		// Raylib.DrawCubeV(new Vector3(0, 0, 10), Vector3.One, Color.Orange);
		// Raylib.DrawCubeV(new Vector3(0, 0, -10), Vector3.One, Color.Orange);
		// Raylib.DrawCubeV(new Vector3(10, 0, 0), Vector3.One, Color.Orange);
		// Raylib.DrawCubeV(new Vector3(-10, 0, 0), Vector3.One, Color.Orange);
	}
}