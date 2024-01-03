// A prop is a 3D model basically.
class PropManager
{
	public static List<Prop> Props = new List<Prop>();

	public static void StartThings()
	{
		foreach (Prop prop in Props) prop.Start();
	}

	public static void UpdateThings()
	{
		foreach (Prop prop in Props) prop.Update();
	}

	public static void RenderThings()
	{
		foreach (Prop prop in Props) prop.Render();
	}

	public static void KillThings()
	{
		foreach (Prop prop in Props) prop.Die();
	}
}