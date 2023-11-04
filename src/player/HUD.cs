using Raylib_cs;

class HUD
{
    public static void Render()
    {
        // Get the center of the screen with padding
        int centreX = (Raylib.GetScreenWidth() / 2) - (SettingsManager.Settings.CrosshairSize / 2);
        int centreY = (Raylib.GetScreenHeight() / 2) - (SettingsManager.Settings.CrosshairSize / 2);

        // Calculate spacing offset
        int spacingOffset = SettingsManager.Settings.CrosshairSpacing * SettingsManager.Settings.CrosshairSize;
		int crosshairLength = SettingsManager.Settings.CrosshairLength * SettingsManager.Settings.CrosshairSize;

        // Draw the crosshair center dot if wanted
        if (SettingsManager.Settings.CrosshairCentreDot)
        {
            Raylib.DrawRectangle(centreX, centreY, SettingsManager.Settings.CrosshairSize, SettingsManager.Settings.CrosshairSize, SettingsManager.Settings.CrosshairColor);
        }

        // Draw the top, bottom, left, and right of the plus part with spacing
        Raylib.DrawRectangle(centreX, centreY - crosshairLength - spacingOffset, SettingsManager.Settings.CrosshairSize, crosshairLength, SettingsManager.Settings.CrosshairColor);
        Raylib.DrawRectangle(centreX, centreY + SettingsManager.Settings.CrosshairSize + spacingOffset, SettingsManager.Settings.CrosshairSize, crosshairLength, SettingsManager.Settings.CrosshairColor);
        Raylib.DrawRectangle(centreX - crosshairLength - spacingOffset, centreY, crosshairLength, SettingsManager.Settings.CrosshairSize, SettingsManager.Settings.CrosshairColor);
        Raylib.DrawRectangle(centreX + SettingsManager.Settings.CrosshairSize + spacingOffset, centreY, crosshairLength, SettingsManager.Settings.CrosshairSize, SettingsManager.Settings.CrosshairColor);
    }
}
