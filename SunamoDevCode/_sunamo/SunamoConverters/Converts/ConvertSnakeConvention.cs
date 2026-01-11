namespace SunamoDevCode._sunamo.SunamoConverters.Converts;

/// <summary>
/// Converter to snake_case convention
/// </summary>
internal class ConvertSnakeConvention
{
    /// <summary>
    /// Sanitizes string by removing invalid characters
    /// </summary>
    /// <param name="input">Input string to sanitize</param>
    /// <returns>Sanitized string</returns>
    static string Sanitize(string input)
    {
        var text = new StringBuilder(input.Replace("", "_").Replace("__", "_"));
        for (int i = text.Length - 1; i >= 0; i--)
        {
            var character = text[i];
            if (!char.IsLetter(character) && !char.IsDigit(character) && character != '_')
            {
                text = text.Remove(i, 1);
            }
        }
        return text.ToString();
    }

    /// <summary>
    /// Converts string to snake_case convention
    /// </summary>
    /// <param name="text">Input string to convert</param>
    /// <returns>String in snake_case convention</returns>
    internal static string ToConvention(string text)
    {
        var result = string.Concat(text.Select((character, index) => index > 0 && char.IsUpper(character) ? "_" + character.ToString() : character.ToString())).ToLower();
        return Sanitize(result);
    }
}
