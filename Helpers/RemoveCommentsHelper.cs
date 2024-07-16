namespace SunamoDevCode.Helpers;
/// <summary>
/// Jednotné místo pro remove commentů odevšad
/// protože jsem chtěl odst # a nemohl jsem si vzpomenout kde se používají
///
///
/// </summary>
public class RemoveCommentsHelper
{
    public static string Powershell(string s)
    {
        var l = SHGetLines.GetLines(s);
        _sunamo.SunamoCollections.CA.Trim(l);
        _sunamo.SunamoCollections.CA.RemoveStartingWith("#", l);
        return string.Join(Environment.NewLine, l);
    }
}
