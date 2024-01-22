class LoremCommand : ICommand
{
	// Command information
	public string Name => "lorem";
	public string[] RequiredArgs => null;
	public string[] OptionalArgs => new string[] { "count" };

	// Stuff used by the help command
	public string DisplayName => "Lorem Ipsum";
	public string ShortDescription => "Generate Lorem Ipsum";
	public string LongDescription => "Generate Lorem Ipsum. Mainly for debugging purposes.";
	public string ExampleUsage => "lorem 5";

	public void Execute(string[] args, ref TerminalOutput output)
	{
		// Get the count
		int count = 1;
		if (args.Length >= 1) int.TryParse(args[0], out count);

		// Print the lorem
		for (int i = 0; i < count; i++)
		{
			output.WriteLine("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed consequat lectus eget gravida sagittis. Aliquam erat volutpat. Integer facilisis eleifend sodales. Donec eleifend quam id turpis bibendum gravida vitae vitae mauris. Nulla facilisi. Donec lobortis ut nisi ut consectetur. Vivamus dignissim rhoncus nibh laoreet vehicula. Mauris mollis leo sit amet mauris efficitur, id euismod neque eleifend.\n");
			output.WriteLine("Aliquam rutrum semper justo, et volutpat nisl ultricies sit amet. Donec a lobortis orci, quis rhoncus arcu. Etiam in tristique urna, eu finibus neque. Vestibulum tristique ullamcorper sollicitudin. Phasellus iaculis sem eu nulla egestas, ac egestas mauris convallis. Vivamus cursus, magna non ornare ullamcorper, lacus arcu laoreet ligula, et consequat nulla tellus at mauris. Sed efficitur mauris a augue rutrum tristique. Etiam elit libero, fermentum tincidunt ante in, elementum placerat lacus. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec nec pretium ex, sit amet hendrerit turpis. Donec quis varius mi, tristique sollicitudin tellus.\n");
			output.WriteLine("Integer ut ipsum quis urna dapibus venenatis. Vivamus posuere leo magna, et pellentesque mauris aliquam et. Donec at mattis justo. Ut tempor laoreet tortor vitae cursus. Sed hendrerit nibh at eleifend tempor. Fusce pharetra sagittis libero, non sollicitudin nibh. Fusce vel erat massa. Nam consectetur pretium semper.\n");
		}
	}
}