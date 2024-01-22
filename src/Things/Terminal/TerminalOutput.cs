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

		// Loop through the text and add newlines before the text
		// TODO: Make it only wrap on spaces
		for (int i = 0; i < text.Length; i += maxCharacters)
		{
			text = text.Insert(i, "\n");
		}
		return text;
	}
}