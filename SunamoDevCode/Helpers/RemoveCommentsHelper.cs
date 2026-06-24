namespace SunamoDevCode.Helpers;

// Jednotné místo pro remove commentů odevšad
// protože jsem chtěl odst # a nemohl jsem si vzpomenout kde se používají
public class RemoveCommentsHelper
{
    // Removes PowerShell comment lines (starting with #) from the given text.
    public static string Powershell(string text)
    {
        var list = SHGetLines.GetLines(text);
        CA.Trim(list);
        CA.RemoveStartingWith("#", list);
        return string.Join(Environment.NewLine, list);
    }
}
