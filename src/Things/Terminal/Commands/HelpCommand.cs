
class HelpCommand : Command
{
	public static void Initialize()
	{
		CommandName = "help";
		RequiredArgs = null;
		OptionalArgs = new string[] { "command" };

		DisplayName = "Help";
		Description = "Prints a list of available commands.";
	}


	public static new void Execute(string[] args, ref string output)
	{
		output += "---- COMMAND LIST ------------\n";
	}
}