class Utils
{
	//! Not using MathF.Pi because this smaller number is quicker
	private const float pi = 3.1415f;

	// Convert radians to degrees
	public static float RadiansToDegrees(float radians)
	{
		return radians * (180 / pi);
	}

	// Convert degrees to radians
	public static float DegreesToRadians(float degrees)
	{
		return degrees * (pi / 180);
	}
}