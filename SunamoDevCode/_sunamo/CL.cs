// variables names: ok
namespace SunamoDevCode._sunamo;

/// <summary>
/// Console helper class for user interaction
/// </summary>
internal class CL
{
    /// <summary>
    /// Creates prompt text asking user to enter value
    /// </summary>
    /// <param name="prompt">Text to display without ending dot</param>
    /// <param name="appendAfterEnter">Whether to append "Enter" before the text</param>
    /// <param name="returnWhenIsNotNull">Text to return if not null, otherwise build prompt</param>
    /// <returns>Formatted prompt text</returns>
    internal static string AskForEnter(string prompt, bool appendAfterEnter,
        string returnWhenIsNotNull)
    {
        if (returnWhenIsNotNull == null)
        {
            if (appendAfterEnter)
                prompt = FromKey("Enter") + " " + prompt + " ";
            prompt +=
                ". " + FromKey("ForExitEnter") + " -1. Is possible enter also nothing - just enter";
            return prompt;
        }
        return returnWhenIsNotNull;
    }

    /// <summary>
    /// Gets localized text for the given key
    /// </summary>
    /// <param name="key">Localization key</param>
    /// <returns>Localized text (currently returns the key itself)</returns>
    private static string FromKey(string key)
    {
        return key;
    }

    /// <summary>
    /// Prompts user to type multiple lines of text until they enter -1 or a specific termination text
    /// </summary>
    /// <param name="prompt">Prompt text to display to user</param>
    /// <param name="anotherPossibleAftermOne">Additional possible termination strings</param>
    /// <returns>All entered lines combined and trimmed</returns>
    internal static string UserMustTypeMultiLine(string prompt, params string[] anotherPossibleAftermOne)
    {
        string line = null;
        Information(AskForEnter(prompt, true, ""));
        StringBuilder stringBuilder = new();
        while ((line = Console.ReadLine()) != null)
        {
            if (line == "-1") break;
            stringBuilder.AppendLine(line);
            if (anotherPossibleAftermOne.Contains(line)) break;
        }
        var result = stringBuilder.ToString().Trim();
        return result;
    }

    /// <summary>
    /// Writes information message to console
    /// </summary>
    /// <param name="value">Message to write</param>
    private static void Information(string value)
    {
        Console.WriteLine(value);
    }

    /// <summary>
    /// Writes a line to console
    /// </summary>
    /// <param name="message">Message to write</param>
    internal static void WriteLine(string message)
    {
        Console.WriteLine(message);
    }
}