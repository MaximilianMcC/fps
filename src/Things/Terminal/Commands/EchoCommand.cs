
class EchoCommand : Command
{
	public static void Initialize()
	{
		CommandName = "echo";
		RequiredArgs = new string[] { "text" };
		OptionalArgs = null;

		DisplayName = "Echo";
		Description = "Echoes back arguments";
	}

	//? new is kinda like override for static stuff
	public static new void Execute(string[] args, ref string output)
	{
		output += string.Join(' ', args) + "\n";
	}
}