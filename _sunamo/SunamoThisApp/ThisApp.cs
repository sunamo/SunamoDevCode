namespace SunamoDevCode._sunamo.SunamoThisApp;

internal class ThisApp
{
    internal static string EventLogName;
    internal static string Name;
    //internal static LangsShared l;
    internal static void Success(string v, params string[] o)
    {
        SetStatus(TypeOfMessageShared.Success, v, o);
    }
    internal static void Info(string v, params string[] o)
    {
        SetStatus(TypeOfMessageShared.Information, v, o);
    }
    internal static void Error(string v, params string[] o)
    {
        SetStatus(TypeOfMessageShared.Error, v, o);
    }
    internal static void Warning(string v, params string[] o)
    {
        SetStatus(TypeOfMessageShared.Warning, v, o);
    }
    internal static void Ordinal(string v, params string[] o)
    {
        SetStatus(TypeOfMessageShared.Ordinal, v, o);
    }
    internal static void Appeal(string v, params string[] o)
    {
        SetStatus(TypeOfMessageShared.Appeal, v, o);
    }
    internal static void SetStatus(TypeOfMessageShared st, string status, params string[] args)
    {
        var format = /*string.Format*/ string.Format(status, args);
        if (format.Trim() != string.Empty)
        {
            if (StatusSetted == null)
            {
                // For unit tests
                //////////DebugLogger.Instance.WriteLine(st + ": " + format);
            }
            else
            {
                StatusSetted(st, format);
            }
        }
    }
    internal static event Action<TypeOfMessageShared, string> StatusSetted;
}