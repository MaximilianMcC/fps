// A thing is a 3D model basically.
class ThingManager
{
	public static List<Thing> Things = new List<Thing>();

	public static void StartThings()
	{
		foreach (Thing thing in Things) thing.Start();
	}

	public static void UpdateThings()
	{
		foreach (Thing thing in Things) thing.Update();
	}

	public static void RenderThings()
	{
		foreach (Thing thing in Things) thing.Render();
	}

	public static void KillThings()
	{
		foreach (Thing thing in Things) thing.Die();
	}
}