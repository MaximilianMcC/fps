
class HelpCommand : ICommand
{
	// Command information
	//! command isnt a required arg, but is just there so you can see its an available one
	public string Name => "help";
	public string[] RequiredArgs => new string[] { "command" };
	public string[] OptionalArgs => new string[] { "command" };

	// Stuff used by the help command
	public string DisplayName => "Help";
	public string ShortDescription => "Shows this menu.";
	public string LongDescription => "Learn about how to use different commands and arguments.";
	public string ExampleUsage => "help";

	// Command list
	List<ICommand> commandList;

	public HelpCommand(List<ICommand> commands)
	{
		commandList = commands;
	}

	public void Execute(string[] args, ref TerminalOutput output)
	{
		// Check for if they want to show a single command
		if (args.Length >= 1)
		{
			// Loop through all commands until we get the requested one
			foreach (ICommand command in commandList)
			{
				if (command.Name == args[0])
				{
					ShowSingleCommand(command, ref output);
					return;
				}
			}
		}

		// Show all commands
		ShowAllCommands(ref output);
	}

	private void ShowSingleCommand(ICommand command, ref TerminalOutput output)
	{
		// Print a title and description
		output.WriteLine($"---- {command.DisplayName.ToUpper()} COMMAND ----------------\n" +
			$"{command.LongDescription}");

		// Print the required and optional args if there are any
		output.WriteLine($"Required Args: {GetArgs(command.RequiredArgs)}");
		output.WriteLine($"Optional Args: {GetArgs(command.OptionalArgs)}");

		// Print the example usage
		output.WriteLine($"Example usage: {command.ExampleUsage}");
	}

	private void ShowAllCommands(ref TerminalOutput output)
	{
		// Show a header
		output.WriteLine("---- COMMAND LIST ----------------");

		// Get the longest label for calculating whitespace for the separator
		// TODO: Don't do this because it's looping over the same arrays heaps of time (bad)
		int separatorX = 0;
		foreach (ICommand command in commandList)
		{
			int length = GenerateLabel(command).Length;
			if (length > separatorX) separatorX = length;
		}

		// Loop through all commands and print their details
		foreach (ICommand command in commandList)
		{
			// Calculate whitespace for the bar/separator
			string label = GenerateLabel(command);
			string whitespace = new string(' ', separatorX - label.Length);
			output.WriteLine($"{label}{whitespace} | {command.ShortDescription}");
		}
	}


	// Make a label thingy
	//? Only in a method so it can be measured
	private string GenerateLabel(ICommand command)
	{
		// Only return the name if there are no args
		string label = $"{command.Name} ";
		if (command.RequiredArgs == null) return label;

		// Return the label and args if they exist
		label += GetArgs(command.RequiredArgs);
		return label;
	}

	// Generate args from an array
	private string GetArgs(string[] args)
	{
		string output = "";

		// Check for if there are any args
		if (args == null) return "None";

		// Print all of the args and also remove the last space
		foreach (string arg in args) output += $"<{arg}> ";
		return output.Remove(output.Length - 1);
	}
}