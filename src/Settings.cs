using System.Text.Json;
using Raylib_cs;

public class Settings
{
	// Keyboard movement stuff
	public KeyboardKey Forwards { get; protected set; } = KeyboardKey.KEY_W;
	public KeyboardKey Backwards { get; protected set; } = KeyboardKey.KEY_S;
	public KeyboardKey Left { get; protected set; } = KeyboardKey.KEY_A;
	public KeyboardKey Right { get; protected set; } = KeyboardKey.KEY_D;
	public KeyboardKey Sprint { get; protected set; } = KeyboardKey.KEY_LEFT_CONTROL;
	public KeyboardKey Jump { get; protected set; } = KeyboardKey.KEY_SPACE;
	
	// Mouse movement stuff
	public float Sensitivity { get; protected set; } = 0.003f;
	public float Fov { get; protected set; } = 60f;

	// Graphics/video stuff
	public int MaxFps { get; protected set; } = 144;

	// Debug/other stuff
	public KeyboardKey ToggleDebug { get; protected set; } =  KeyboardKey.KEY_GRAVE;

	// Crosshair settings
	public Color CrosshairColor { get; protected set; } = new Color(255, 255, 255, 128);
	public int CrosshairSize { get; protected set; } = 5;
	public bool CrosshairCentreDot { get; protected set; } = true;
	public int CrosshairLength { get; protected set; } = 4;
	public int CrosshairSpacing { get; protected set; } = 2;
}

public class SettingsManager
{
	public static Settings Settings { get; private set; }

	// Load/update settings
	public static void UpdateSettings()
	{
		const string settingsFilePath = "./config/settings.json";

		// Check for if there is a settings folder.
		// If not create one, and add a heap of default values
		if (!File.Exists(settingsFilePath))
		{
			// Add in all of the default settings (the values that I have hardcoded into this class)
			Settings defaultSettings = new Settings();
			string defaultSettingsJson = JsonSerializer.Serialize(defaultSettings, new JsonSerializerOptions { WriteIndented = true } );
			Console.WriteLine(defaultSettingsJson);

			// Make, and write the new settings file
			File.WriteAllText(settingsFilePath, defaultSettingsJson);
		}

		// Get/update the settings from the JSON
		string settingsJson = File.ReadAllText(settingsFilePath);
		Settings = JsonSerializer.Deserialize<Settings>(settingsJson);
	}
}