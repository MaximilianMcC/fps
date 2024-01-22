class LoremCommand : ICommand
{
	// Command information
	public string Name => "lorem";
	public string[] RequiredArgs => null;
	public string[] OptionalArgs => new string[] { "count" };

	// Stuff used by the help command
	public string DisplayName => "Lorem Ipsum";
	public string ShortDescription => "Generate Lorem Ipsum";
	public string LongDescription => "Generate Lorem Ipsum. Mainly for debugging purposes.";
	public string ExampleUsage => "lorem 5";

	public void Execute(string[] args, ref string output)
	{
		// Get the count
		int count = int.Parse(args[0]);
		
		// Print the lorem
		for (int i = 0; i < count; i++)
		{
			output += "Lorem ipsum dolor sit amet, \nconsectetur adipiscing elit. Quisque et vestibulum\npurus, eu pellentesque orci. Duis pharetra\nvenenatis vehicula. Duis ac tortor\nut neque gravida cursus. Morbi nibh ante, dignissim nec\ndolor dictum, porttitor lacinia leo. Quisque enim\njusto, pretium quis velit non, vehicula laoreet ex\nDonec congue metus ac tortor volutpat, ut sagittis\nex pellentesque. Nunc velit mauris, mattis in pharetra at,\nfinibus sed urna. Integer fermentum, tellus eu malesuada\nconsequat, sem libero commodo mi, id sagittis\nelit lacus eget eros. Aenean sollicitudin placerat consectetur.\nDuis vestibulum ipsum dignissim mi pretium\nvestibulum. Fusce a lorem lacus. Nulla in eros dui.";
		}
	}
}