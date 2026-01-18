namespace SunamoDevCode._sunamo.SunamoBts;

/// <summary>
/// Basic Type System helper methods
/// </summary>
internal class BTS
{
    /// <summary>
    /// Replaces comma with dot in a string if specified
    /// </summary>
    /// <param name="text">Text to process</param>
    /// <param name="isReplacingCommaForDot">Whether to replace comma with dot</param>
    /// <returns>Processed text</returns>
    internal static string Replace(ref string text, bool isReplacingCommaForDot)
    {
        if (isReplacingCommaForDot) text = text.Replace(",", ".");

        return text;
    }

    internal static int LastInt = -1;
    internal static long LastLong = -1;
    internal static float LastFloat = -1;
    internal static double LastDouble = -1;

    /// <summary>
    /// Checks if string can be parsed as float
    /// </summary>
    /// <param name="text">Text to check</param>
    /// <param name="isReplacing">Whether to replace comma with dot</param>
    /// <returns>True if text is a valid float</returns>
    internal static bool IsFloat(string text, bool isReplacing = false)
    {
        if (text == null) return false;

        Replace(ref text, isReplacing);
        return float.TryParse(text.Replace(",", "."), out LastFloat);
    }

    /// <summary>
    /// Checks if string can be parsed as integer
    /// </summary>
    /// <param name="text">Text to check</param>
    /// <param name="isThrowingExceptionIfIsFloat">Whether to throw exception if value is float</param>
    /// <param name="isReplacingCommaForDot">Whether to replace comma with dot</param>
    /// <returns>True if text is a valid integer</returns>
    internal static bool IsInt(string text, bool isThrowingExceptionIfIsFloat = false, bool isReplacingCommaForDot = false)
    {
        if (text == null) return false;

        text = text.Replace(" ", "");
        Replace(ref text, isReplacingCommaForDot);


        var result = int.TryParse(text, out LastInt);
        if (!result)
            if (IsFloat(text))
                if (isThrowingExceptionIfIsFloat)
                    throw new Exception(text + " is float but is calling IsInt");

        return result;
    }

    /// <summary>
    /// Logical XOR operation
    /// </summary>
    /// <param name="first">First boolean value</param>
    /// <param name="isNegated">Whether to negate the result</param>
    /// <returns>XOR result</returns>
    internal static bool Is(bool first, bool isNegated)
    {
        if (isNegated)
        {
            return !first;
        }
        return first;
    }

    private const string Yes = "Yes";
    private const string No = "No";

    /// <summary>
    /// Converts boolean to string representation
    /// </summary>
    /// <param name="value">Boolean value to convert</param>
    /// <param name="isLowerCase">Whether to return lowercase result</param>
    /// <returns>String representation of boolean</returns>
    internal static string BoolToString(bool value, bool isLowerCase = false)
    {
        string result = null;
        if (value)
            result = Yes;
        else
        {
            result = No;
        }

        if (isLowerCase)
        {
            return result.ToLower();
        }
        return result;
    }

    /// <summary>
    /// Gets value from nullable boolean
    /// </summary>
    /// <param name="value">Nullable boolean</param>
    /// <returns>Value or default</returns>
    internal static bool GetValueOfNullable(bool? value)
    {
        return value.GetValueOrDefault();
    }
}