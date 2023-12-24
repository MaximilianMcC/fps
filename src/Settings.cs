using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

public class Settings
{
	// Keyboard movement stuff
	public KeyboardKey Forwards { get; set; } = KeyboardKey.KEY_W;
	public KeyboardKey Backwards { get; set; } = KeyboardKey.KEY_S;
	public KeyboardKey Left { get; set; } = KeyboardKey.KEY_A;
	public KeyboardKey Right { get; set; } = KeyboardKey.KEY_D;
	public KeyboardKey Jump { get; set; } = KeyboardKey.KEY_SPACE;

	// 'special' movement stuff
	public KeyboardKey Sprint { get; set; } = KeyboardKey.KEY_LEFT_CONTROL;
	public KeyboardKey Crouch { get; set; } = KeyboardKey.KEY_LEFT_SHIFT;
	
	// Mouse movement stuff
	public float Sensitivity { get; set; } = 150f; //! If goes < 0 movement is reversed. 0 is no movement
	public float Fov { get; set; } = 60f;

	// Graphics/video stuff
	public int MaxFps { get; set; } = 144;

	// Debug/other stuff
	public KeyboardKey ToggleDebug { get; set; } =  KeyboardKey.KEY_GRAVE;

	// Crosshair settings
	// TODO: Re make crosshair settings to be stored in a single string like how cs:go does it and stuff to avoid all this stupid parsing stuff
	[JsonIgnore] public Color CrosshairColor { get; set; } = new Color(255, 255, 255, 128);
	public byte CrosshairColorRed { get; set; } = 255; // TODO: Don't have public
	public byte CrosshairColorGreen { get; set; } = 255; // TODO: Don't have public
	public byte CrosshairColorBlue { get; set; } = 255; // TODO: Don't have public
	public byte CrosshairColorAlpha { get; set; } = 128; // TODO: Don't have public
	public int CrosshairSize { get; set; } = 5;
	public bool CrosshairCentreDot { get; set; } = true;
	public int CrosshairLength { get; set; } = 4;
	public int CrosshairSpacing { get; set; } = 2;
}

public class SettingsManager
{
	public static Settings Settings { get; private set; }

	private const string settingsPath = "./config/";
	private const string settingsFile = "settings.json";
	private const string settingsFilePath = settingsPath + settingsFile;

	// Load/update settings
	public static void Load()
	{
		// Check for if there is a settings directory
		// If not create one
		if (Directory.Exists(settingsPath) == false)
			Directory.CreateDirectory(settingsPath);

		// Check for if there is a settings file.
		// If not create one, and add a heap of default values
		if (File.Exists(settingsFilePath) == false)
		{
			// Add in all of the default settings (the values that I have hardcoded into this class)
			Settings defaultSettings = new Settings();
			string defaultSettingsJson = JsonSerializer.Serialize(defaultSettings, new JsonSerializerOptions { WriteIndented = true } );

			// Make, and write the new settings file
			File.WriteAllText(settingsFilePath, defaultSettingsJson);
		}

		// Get/update the settings from the JSON
		string settingsJson = File.ReadAllText(settingsFilePath);
		Settings settings = JsonSerializer.Deserialize<Settings>(settingsJson);

		// Parse the colors
		// TODO: Do this automatically or another way somehow
		settings.CrosshairColor = new Color(settings.CrosshairColorRed, settings.CrosshairColorGreen, settings.CrosshairColorBlue, settings.CrosshairColorAlpha);

		// Assign the new settings
		Settings = settings;
	}

	// Save/update settings
	public static void Save()
	{
		// Parse the colors
		// TODO: Do this automatically or another way somehow
		Settings.CrosshairColorRed = Settings.CrosshairColor.r;
		Settings.CrosshairColorGreen = Settings.CrosshairColor.g;
		Settings.CrosshairColorBlue = Settings.CrosshairColor.b;
		Settings.CrosshairColorAlpha = Settings.CrosshairColor.a;

		// Serialize the current settings to JSON
		string currentSettings = JsonSerializer.Serialize(Settings);

		// Write to the settings file
		File.WriteAllText(settingsFilePath, currentSettings);
	}
}