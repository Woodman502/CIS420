using System;
using System.Drawing;

public static class ColorHelper {

    /// <summary>
    /// Get a random hex color
    /// </summary>
    /// <returns>Random hex color</returns>
    public static string GetRandomColor() {
        var random = new Random();
        int r = random.Next(50, 255);
        int g = random.Next(50, 255);
        int b = random.Next(50, 255);

        return ColorTranslator.ToHtml(Color.FromArgb(r, g, b));
    }

    /// <summary>
    /// Checks whether an html hex color isn't too dark
    /// </summary>
    /// <param name="htmlColor">The hex color to check</param>
    /// <returns>true if the color is bright enough, false otherwise</returns>
    public static bool IsValidColor(string htmlColor) {
        // Bright enough means that the
        // color is valid and it's r+g+b are greater than 150

        try {
            Color c = ColorTranslator.FromHtml(htmlColor);
            return ((c.R + c.G + c.B) >= 150);
        } catch {
            return false;
        }
    }

    /// <summary>
    /// Returns a darker color for use as a border
    /// </summary>
    /// <param name="hex">The color to make darker</param>
    /// <returns>A darker hex color than the color passed</returns>
    public static string GetBorderFromHtml(string hex) {

        // I want this to throw an exception on invalid
        // hex values. It should only be used from the
        // developers side, so that should be safe.

        Color color = ColorTranslator.FromHtml(hex);

        // I should convert to HSV or HSL and then lower V/L
        // These methods aren't in .net. Instead i'm lowering each
        // R,G, and B value

        int newR = (int)(color.R * .5);
        int newG = (int)(color.G * .5);
        int newB = (int)(color.B * .5);

        Color darker = Color.FromArgb(newR, newG, newB);
        return ColorTranslator.ToHtml(darker);
    }
}