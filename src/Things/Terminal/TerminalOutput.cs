class TerminalOutput
{
	public string Output { get; private set; }
	private int maxCharacters;

	public TerminalOutput(int maxCharacters)
	{
		this.maxCharacters = maxCharacters;
	}

	public void WriteLine(string text)
	{
		Output += WordWrap(text) + "\n";
	}

	public void Write(string text)
	{
		Output += WordWrap(text);
	}

	public void Clear()
	{
		Output = "";
	}

	private string WordWrap(string text)
	{
		// If the text is less than the max then quit early
		if (text.Length <= maxCharacters) return text;

		// Split the text by the spaces so we can break on a space
		string[] words = text.Split(" ");

		// Loop through the text and add newlines
		int characters = 0;
		string wrappedText = "";
		for (int i = 0; i < words.Length; i++)
		{
			//  Check for if we need to insert a newline
			// TODO: Guard clause
			if (characters >= maxCharacters)
			{
				Console.WriteLine(characters + "\t" + maxCharacters);
				wrappedText += "\n";
				characters = 0;
			}
			else characters += words[i].Length;

			wrappedText += words[i] + " ";
		}

		return wrappedText;
	}
}