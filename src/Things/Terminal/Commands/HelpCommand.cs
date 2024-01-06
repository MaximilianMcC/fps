class HelpCommand : ICommand
{
	// Command information
	public string Name => "help";
	public string[] RequiredArgs => null;
	public string[] OptionalArgs => new string[] { "command" };

	// Stuff used by the help command
	public string DisplayName => "Help";
	public string ShortDescription => "Shows this menu.";
	public string LongDescription => "Learn about how to use different commands and arguments.";

	// Command list
	List<ICommand> commandList;

	public HelpCommand(List<ICommand> commands)
	{
		commandList = commands;
	}

	public void Execute(string[] args, ref string output)
	{
		output += "---- COMMAND LIST ----------------\n";
	}
}