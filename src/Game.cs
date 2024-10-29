class Game
{
	public static List<Updatable> Things = new List<Updatable>();
	public static Player Player;

	public static void SetUp()
	{
		// Make the player
		Player = new Player();
		Things.Add(Player);

		// Add crap idk
		Things.Add(new Grid());
		Things.Add(new Temp());
	}
}