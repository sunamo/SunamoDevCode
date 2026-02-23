namespace SunamoDevCode;

/// <summary>
/// Provides methods for generating JSON content using snake_case naming convention.
/// </summary>
public class GenerateJson
{
    /// <summary>
    /// Creates a dictionary mapping snake_case keys to original values.
    /// </summary>
    /// <param name="list">List of values to convert to snake_case keys.</param>
    /// <returns>Dictionary with snake_case keys and original values.</returns>
    public static Dictionary<string, string> WithSnakeConventionDict(List<string> list)
    {
        var dictionary = new Dictionary<string, string>();
        foreach (var item in list) dictionary.Add(ConvertSnakeConvention.ToConvention(item), item);
        return dictionary;
    }

    /// <summary>
    /// Generates a JSON-formatted string with snake_case keys mapped to original values.
    /// </summary>
    /// <param name="list">List of values to include in the JSON output.</param>
    /// <returns>JSON-formatted string.</returns>
    public static string WithSnakeConvention(List<string> list)
    {
        var dictionary = WithSnakeConventionDict(list);
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("{");
        foreach (var item in dictionary) stringBuilder.AppendLine($"{SH.WrapWithQm(item.Key) + ": " + SH.WrapWithQm(item.Value)},");
        stringBuilder.AppendLine("}");

        return stringBuilder.ToString();
    }
}