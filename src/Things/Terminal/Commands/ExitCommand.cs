class ExitCommand : ICommand
{
	// Command information
	public string Name => "exit";
	public string[] RequiredArgs => null;
	public string[] OptionalArgs => null;

	// Stuff used by the help command
	public string DisplayName => "Exit";
	public string ShortDescription => "Exit the terminal";
	public string LongDescription => "Exit the terminal, and stop interacting with it.";
	public string ExampleUsage => "exit";

	public void Execute(string[] args, ref string output)
	{
		output += "not implemented!!";
	}
}