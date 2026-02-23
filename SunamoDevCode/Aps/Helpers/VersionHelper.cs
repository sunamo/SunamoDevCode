namespace SunamoDevCode.Aps.Helpers;

/// <summary>
/// Provides helper methods for version string formatting.
/// </summary>
public class VersionHelper
{
    /// <summary>
    /// Removes trailing ".0" parts from a version string representation.
    /// </summary>
    /// <param name="version">Version to format.</param>
    /// <returns>Version string with trailing zero parts removed.</returns>
    public static string RemovePartsWhichIsZero(Version version)
    {
        const string dotZero = ".0";

        var text = version.ToString();
        while (text.EndsWith(dotZero))
        {
            text = SHTrim.Trim(text, dotZero);
        }
        return text;
    }
}