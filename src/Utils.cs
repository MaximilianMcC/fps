using System.Numerics;

class Utils
{
	// Convert radians to degrees
	public static float RadiansToDegrees(float radians)
	{
		return radians * (180 / MathF.PI);
	}

	// Convert degrees to radians
	public static float DegreesToRadians(float degrees)
	{
		return degrees * (MathF.PI / 180);
	}

	// Get the distance between two coordinates
	//? This doesn't use the square root, so it's faster but less precise
	public static float GetDistance(Vector3 firstPoint, Vector3 secondPoint)
	{
		float x = MathF.Pow(secondPoint.X - firstPoint.X, 2);
		float y = MathF.Pow(secondPoint.Y - firstPoint.Y, 2);
		float z = MathF.Pow(secondPoint.Z - firstPoint.Z, 2);
		return x + y + z;
	}

}