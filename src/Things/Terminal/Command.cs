interface ICommand
{
	// Command information
	public string Name { get; }
	public string[] RequiredArgs { get; }
	public string[] OptionalArgs { get; }

	// Stuff used by the help command
	public string DisplayName { get; }
	public string ShortDescription { get; }
	public string LongDescription { get; }
	public string ExampleUsage { get; }

	// Run the command
	public void Execute(string[] args, ref string output);
}