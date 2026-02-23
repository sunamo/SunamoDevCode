namespace SunamoDevCode.Helpers;

/// <summary>
/// Jednotné místo pro remove commentů odevšad
/// protože jsem chtěl odst # a nemohl jsem si vzpomenout kde se používají
///
///
/// </summary>
public class RemoveCommentsHelper
{
    /// <summary>
    /// Removes PowerShell comment lines (starting with #) from the given text.
    /// </summary>
    /// <param name="text">PowerShell script content to process.</param>
    /// <returns>Text with comment lines removed.</returns>
    public static string Powershell(string text)
    {
        var list = SHGetLines.GetLines(text);
        CA.Trim(list);
        CA.RemoveStartingWith("#", list);
        return string.Join(Environment.NewLine, list);
    }
}