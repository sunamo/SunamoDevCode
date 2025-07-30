namespace SunamoDevCode._sunamo.SunamoConverters.Converts;

internal class ConvertSnakeConvention
{
    static string Sanitize(string t)
    {
        var s = new StringBuilder(t.Replace("", "_").Replace("__", "_"));
        for (int i = s.Length - 1; i >= 0; i--)
        {
            var ch = s[i];
            if (!char.IsLetter(ch) && !char.IsDigit(ch) && ch != '_')
            {
                s = s.Remove(i, 1);
            }
        }
        return s.ToString();
    }
    internal static string ToConvention(string text)
    {
        var rz = string.Concat(text.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        return Sanitize(rz);
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }
        if (text.Length < 2)
        {
            return text;
        }
        var sb = new StringBuilder();
        sb.Append(char.ToLowerInvariant(text[0]));
        for (int i = 1; i < text.Length; ++i)
        {
            char c = text[i];
            if (char.IsUpper(c))
            {
                sb.Append('_');
                sb.Append(char.ToLowerInvariant(c));
            }
            else
            {
                sb.Append(c);
            }
        }
        var r = sb.ToString().Replace("", "_");
        return r;
    }
}