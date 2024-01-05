
class ClsCommand : Command
{
	public static void Initialize()
	{
		CommandName = "cls";
		RequiredArgs = null;
		OptionalArgs = null;

		DisplayName = "Clear";
		Description = "Clears the screen";
	}

	public static new void Execute(string[] args, ref string output)
	{
		output = "";
	}
}