namespace SunamoDevCode;

/// <summary>
/// Helper class for System.Windows controls shortcuts and names
/// </summary>
public static class SystemWindowsControls
{
    private static readonly Type type = typeof(SystemWindowsControls);
    private static bool s_initialized;
    private static readonly Dictionary<string, List<string>> s_controls = new();
    private static EmbeddedResourcesH? s_embeddedResourcesH;

    private static Dictionary<string, string> controlsShortLong;

    /// <summary>
    /// Initializes the controls short to long mapping dictionary
    /// </summary>
    public static void InitControlsShortLong()
    {
        Init();

        if (controlsShortLong == null)
        {
            controlsShortLong = new Dictionary<string, string>();

            foreach (var item in s_controls)
            foreach (var shortcut in item.Value)
                controlsShortLong.Add(shortcut, item.Key);
        }
    }

    /// <summary>
    /// Initializes the embedded resources and controls dictionary
    /// </summary>
    public static void Init()
    {
        if (!s_initialized)
        {
            s_initialized = true;

            s_embeddedResourcesH = new EmbeddedResourcesH(type.Assembly, "SunamoDevCode");
        }
    }

    /// <summary>
    /// Checks if the given text starts with any control shortcut
    /// </summary>
    /// <param name="text">Text to check</param>
    /// <returns>True if text starts with a control shortcut</returns>
    public static bool StartingWithShortcutOfControl(string text)
    {
        foreach (var item in s_controls)
        foreach (var shortcut in item.Value)
            if (shortcut.Length > 2)
                if (text.StartsWith(shortcut))
                    return true;

        return false;
    }

    /// <summary>
    /// Checks if the given text is a control shortcut
    /// </summary>
    /// <param name="text">Text to check</param>
    /// <returns>True if text is a control shortcut</returns>
    public static bool IsShortcutOfControl(string text)
    {
        foreach (var item in s_controls)
        foreach (var shortcut in item.Value)
            if (shortcut == text)
                return true;

        return false;
    }

    /// <summary>
    /// Checks if the given text is a control name
    /// </summary>
    /// <param name="text">Text to check</param>
    /// <returns>True if text is a control name</returns>
    public static bool IsNameOfControl(string text)
    {
        return s_controls.ContainsKey(text);
    }
}