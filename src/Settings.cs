using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Raylib_cs;

class Settings
{
	// Settings file location
	private static readonly string settingsPath = "./config/settings.json";

	// Keyboard movement stuff
	public static KeyboardKey Forwards { get; set; } = KeyboardKey.W;
	public static KeyboardKey Backwards { get; set; } = KeyboardKey.S;
	public static KeyboardKey Left { get; set; } = KeyboardKey.A;
	public static KeyboardKey Right { get; set; } = KeyboardKey.D;
	public static KeyboardKey Jump { get; set; } = KeyboardKey.Space;

	// Mouse looking around stuff
	public static float Fov { get; set; } = 60f;
	public static float Sensitivity { get; set; } = 150f;


	// Write/save everything to the json
	public static void SaveSettings()
	{
		// Make a new json document to write the settings to
		JsonObject settingsJson = new JsonObject()
		{
			["forwards"] = (int)Forwards,
			["backwards"] = (int)Backwards,
			["left"] = (int)Left,
			["right"] = (int)Right,
			["jump"] = (int)Jump,

			["fov"] = (int)Fov,
			["sensitivity"] = (int)Sensitivity,
		};

		// If the json file doesn't exist then make it
		if (File.Exists(settingsPath) == false) Directory.CreateDirectory(Path.GetDirectoryName(settingsPath));
		
		// Write the json to the setting file
		File.WriteAllText(settingsPath, settingsJson.ToString());
	}

	// Load settings from the json
	public static void ReloadSettings()
	{
		// Check for if there is a settings file. If there
		// isn't then write a new one
		if (File.Exists(settingsPath) == false) SaveSettings();

		// Get all the settings from the settings json
		JsonDocument settingsJson = JsonDocument.Parse(File.ReadAllText(settingsPath));
		JsonElement settings = settingsJson.RootElement;

		// Update all the settings
		Forwards = (KeyboardKey)settings.GetProperty("forwards").GetInt32();
		Backwards = (KeyboardKey)settings.GetProperty("backwards").GetInt32();
		Left = (KeyboardKey)settings.GetProperty("left").GetInt32();
		Right = (KeyboardKey)settings.GetProperty("right").GetInt32();
		Jump = (KeyboardKey)settings.GetProperty("jump").GetInt32();

		Fov = settings.GetProperty("fov").GetInt32();
		Sensitivity = settings.GetProperty("sensitivity").GetInt32();
	}
}