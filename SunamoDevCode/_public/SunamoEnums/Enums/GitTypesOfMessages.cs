namespace SunamoDevCode._public.SunamoEnums.Enums;

/// <summary>
/// Defines the severity types of Git command output messages.
/// </summary>
public enum GitTypesOfMessages
{
    /// <summary>
    /// Warning level message from Git.
    /// </summary>
    warning,
    /// <summary>
    /// Error level message from Git.
    /// </summary>
    error,
    /// <summary>
    /// Fatal level message from Git indicating critical failure.
    /// </summary>
    fatal
}