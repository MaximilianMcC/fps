class Command
{
	// Info
	public static string CommandName { get; protected set; }
	public static string[] RequiredArgs { get; protected set; }
	public static string[] OptionalArgs { get; protected set; }

	// Crap for the help command
	public static string DisplayName { get; protected set; }
	public static string Description { get; protected set; }

	// Command (returns the output)
	// TODO: Put ref to output as arg
	public static void Execute(string[] args, ref string output)
	{
		
	}
}