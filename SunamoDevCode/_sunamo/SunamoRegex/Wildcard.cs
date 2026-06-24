namespace SunamoDevCode._sunamo.SunamoRegex;

internal class Wildcard : Regex
{
    internal static Regex CreateInstance(string pattern) => new Regex(WildcardToRegex(pattern));

    internal Wildcard(string pattern)
    : base(WildcardToRegex(pattern))
    {
    }

    internal Wildcard(string pattern, RegexOptions options)
    : base(WildcardToRegex(pattern), options)
    {
    }

    internal static string WildcardToRegex(string pattern) =>
        "^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
}
