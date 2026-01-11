// variables names: ok
namespace SunamoDevCode.Helpers;

/// <summary>
/// Jednotné místo pro remove commentů odevšad
/// protože jsem chtěl odst # a nemohl jsem si vzpomenout kde se používají
///
///
/// </summary>
public class RemoveCommentsHelper
{
    public static string Powershell(string text)
    {
        var list = SHGetLines.GetLines(text);
        CA.Trim(list);
        CA.RemoveStartingWith("#", list);
        return string.Join(Environment.NewLine, list);
    }
}