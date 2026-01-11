namespace SunamoDevCode._sunamo.SunamoEnumsHelper;

internal class EnumHelper
{
    /// <summary>
    /// Parses string value to enum type.
    /// </summary>
    /// <typeparam name="T">Enum type to parse to.</typeparam>
    /// <param name="text">String value to parse.</param>
    /// <param name="defaultValue">Default value to return if parsing fails.</param>
    /// <param name="isReturningDefaultIfNull">Whether to return default value when input is null.</param>
    /// <returns>Parsed enum value or default value if parsing fails.</returns>
    internal static T Parse<T>(string text, T defaultValue, bool isReturningDefaultIfNull = false)
        where T : struct
    {
        if (isReturningDefaultIfNull)
        {
            return defaultValue;
        }
        T result;
        if (Enum.TryParse<T>(text, true, out result))
        {
            return result;
        }

        return defaultValue;
    }
}