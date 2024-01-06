class ClsCommand : ICommand
{
	// Command information
	public string Name => "cls";
	public string[] RequiredArgs => null;
	public string[] OptionalArgs => null;

	// Stuff used by the help command
	public string DisplayName => "Clear";
	public string ShortDescription => "Clears the screen.";
	public string LongDescription => "Clears the screen.";

	public void Execute(string[] args, ref string output)
	{
		// Completely reset the output
		output = "";
	}
}