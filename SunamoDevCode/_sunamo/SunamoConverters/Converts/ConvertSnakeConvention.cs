namespace SunamoDevCode._sunamo.SunamoConverters.Converts;

internal class ConvertSnakeConvention
{
    static string Sanitize(string t)
    {
        var text = new StringBuilder(t.Replace("", "_").Replace("__", "_"));
        for (int i = text.Length - 1; i >= 0; i--)
        {
            var ch = text[i];
            if (!char.IsLetter(ch) && !char.IsDigit(ch) && ch != '_')
            {
                text = text.Remove(i, 1);
            }
        }
        return text.ToString();
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
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(char.ToLowerInvariant(text[0]));
        for (int i = 1; i < text.Length; ++i)
        {
            char character = text[i];
            if (char.IsUpper(character))
            {
                stringBuilder.Append('_');
                stringBuilder.Append(char.ToLowerInvariant(character));
            }
            else
            {
                stringBuilder.Append(character);
            }
        }
        var result = stringBuilder.ToString().Replace("", "_");
        return result;
    }
}