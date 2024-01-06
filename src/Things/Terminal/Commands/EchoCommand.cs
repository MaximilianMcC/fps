class EchoCommand : ICommand
{
	// Command information
	public string Name => "echo";
	public string[] RequiredArgs => new string[] { "text" };
	public string[] OptionalArgs => null;

	// Stuff used by the help command
	public string DisplayName => "Echo";
	public string ShortDescription => "Displays provided text.";
	public string LongDescription => "Displays provided text.";
	public string ExampleUsage => "echo Hello, World!";

	public void Execute(string[] args, ref string output)
	{
		// Give back the text
		output += string.Join(' ', args) + "\n";
	}
}