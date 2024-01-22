class TerminalOutput
{
	public string Output { get; private set; }
	public int MaxCharacters { get; private set; }

	public TerminalOutput(int maxCharacters)
	{
		MaxCharacters = maxCharacters;
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
		if (text.Length <= MaxCharacters) return text;

		// Split the text by the spaces so we can break on a space
		string[] words = text.Split(" ");

		// Loop through the text and add newlines
		int characters = 0;
		string wrappedText = "";
		for (int i = 0; i < words.Length; i++)
		{
			//  Check for if we need to insert a newline
			// TODO: Guard clause
			//? +1 is for a space
			if (characters + words[i].Length + 1 > MaxCharacters)
			{
				wrappedText += "\n";
				characters = 0;
			}

			//? again, +1 is for a space
			wrappedText += words[i] + " ";
			characters += words[i].Length + 1;
		}

		return wrappedText;
	}
}