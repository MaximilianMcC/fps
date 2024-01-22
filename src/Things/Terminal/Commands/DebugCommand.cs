class TestCommand : ICommand
{
	// Command information
	public string Name => "test";
	public string[] RequiredArgs => null;
	public string[] OptionalArgs => null;

	// Stuff used by the help command
	public string DisplayName => "Test";
	public string ShortDescription => "debugging and testing crap";
	public string LongDescription => "debugging and testing crap";
	public string ExampleUsage => "test";

	public void Execute(string[] args, ref TerminalOutput output)
	{
		// Test max character
		output.WriteLine(new string('M', output.MaxCharacters));
	}
}