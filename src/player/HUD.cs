using Raylib_cs;

class HUD
{
    public static void Render()
    {
        // Get the center of the screen with padding
        int centreX = (Raylib.GetScreenWidth() / 2) - (Settings.CrosshairSize / 2);
        int centreY = (Raylib.GetScreenHeight() / 2) - (Settings.CrosshairSize / 2);

        // Calculate spacing offset
        int spacingOffset = Settings.CrosshairSpacing * Settings.CrosshairSize;
		int crosshairLength = Settings.CrosshairLength * Settings.CrosshairSize;

        // Draw the crosshair center dot if wanted
        if (Settings.CrosshairCentreDot)
        {
            Raylib.DrawRectangle(centreX, centreY, Settings.CrosshairSize, Settings.CrosshairSize, Settings.CrosshairColor);
        }

        // Draw the top, bottom, left, and right of the plus part with spacing
        Raylib.DrawRectangle(centreX, centreY - crosshairLength - spacingOffset, Settings.CrosshairSize, crosshairLength, Settings.CrosshairColor);
        Raylib.DrawRectangle(centreX, centreY + Settings.CrosshairSize + spacingOffset, Settings.CrosshairSize, crosshairLength, Settings.CrosshairColor);
        Raylib.DrawRectangle(centreX - crosshairLength - spacingOffset, centreY, crosshairLength, Settings.CrosshairSize, Settings.CrosshairColor);
        Raylib.DrawRectangle(centreX + Settings.CrosshairSize + spacingOffset, centreY, crosshairLength, Settings.CrosshairSize, Settings.CrosshairColor);
    }
}
