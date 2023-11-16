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
}