namespace SunamoDevCode._sunamo.SunamoConverters.Converts;

/// <summary>
/// Converter to PascalCase convention
/// </summary>
internal class ConvertPascalConvention
{
    /// <summary>
    /// Converts string to PascalCase convention
    /// Will include numbers
    /// hello world = HelloWorld
    /// Hello world = HelloWorld
    /// helloWorld = HelloWorld
    /// </summary>
    /// <param name="input">Input string to convert</param>
    /// <returns>String in PascalCase convention</returns>
    internal static string ToConvention(string input)
    {
        StringBuilder stringBuilder = new StringBuilder();
        bool shouldCapitalizeNext = false;
        foreach (char character in input)
        {
            if (shouldCapitalizeNext)
            {
                if (char.IsUpper(character))
                {
                    shouldCapitalizeNext = false;
                    stringBuilder.Append(character);
                    continue;
                }
                else if (char.IsLower(character))
                {
                    shouldCapitalizeNext = false;
                    stringBuilder.Append(char.ToUpper(character));
                    continue;
                }
                else if (char.IsDigit(character))
                {
                    shouldCapitalizeNext = true;
                    stringBuilder.Append(character);
                    continue;
                }
                else
                {
                    continue;
                }
            }
            if (char.IsUpper(character))
            {
                stringBuilder.Append(character);
            }
            else if (char.IsLower(character))
            {
                stringBuilder.Append(character);
            }
            else if (char.IsDigit(character))
            {
                stringBuilder.Append(character);
            }
            else
            {
                shouldCapitalizeNext = true;
            }
        }
        var result = stringBuilder.ToString().Trim();
        StringBuilder resultBuilder = new StringBuilder(result);
        resultBuilder[0] = char.ToUpper(resultBuilder[0]);
        return result;
    }
}
