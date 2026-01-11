namespace SunamoDevCode._sunamo.SunamoThisApp;

/// <summary>
/// Application-level status and messaging helper.
/// </summary>
internal class ThisApp
{
    /// <summary>
    /// Name of the event log for this application.
    /// </summary>
    internal static string EventLogName;

    /// <summary>
    /// Name of the application.
    /// </summary>
    internal static string Name;

    /// <summary>
    /// Check flag for application state.
    /// </summary>
    internal static bool Check;

    /// <summary>
    /// Logs a success message.
    /// </summary>
    /// <param name="message">The message format string.</param>
    /// <param name="args">Format arguments for the message.</param>
    internal static void Success(string message, params string[] args)
    {
        SetStatus(TypeOfMessageShared.Success, message, args);
    }

    /// <summary>
    /// Logs an informational message.
    /// </summary>
    /// <param name="message">The message format string.</param>
    /// <param name="args">Format arguments for the message.</param>
    internal static void Info(string message, params string[] args)
    {
        SetStatus(TypeOfMessageShared.Information, message, args);
    }

    /// <summary>
    /// Logs an error message.
    /// </summary>
    /// <param name="message">The message format string.</param>
    /// <param name="args">Format arguments for the message.</param>
    internal static void Error(string message, params string[] args)
    {
        SetStatus(TypeOfMessageShared.Error, message, args);
    }

    /// <summary>
    /// Logs an appeal message.
    /// </summary>
    /// <param name="message">The message format string.</param>
    /// <param name="args">Format arguments for the message.</param>
    internal static void Appeal(string message, params string[] args)
    {
        SetStatus(TypeOfMessageShared.Appeal, message, args);
    }

    /// <summary>
    /// Sets the application status with a formatted message.
    /// </summary>
    /// <param name="messageType">The type of message.</param>
    /// <param name="status">The status message format string.</param>
    /// <param name="args">Format arguments for the message.</param>
    internal static void SetStatus(TypeOfMessageShared messageType, string status, params string[] args)
    {
        var formattedMessage = string.Format(status, args);
        if (formattedMessage.Trim() != string.Empty)
        {
            if (StatusSetted == null)
            {
                // For unit tests - no handler attached
            }
            else
            {
                StatusSetted(messageType, formattedMessage);
            }
        }
    }

    /// <summary>
    /// Event fired when a status message is set.
    /// </summary>
    internal static event Action<TypeOfMessageShared, string> StatusSetted;
}