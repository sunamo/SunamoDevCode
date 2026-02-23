namespace SunamoDevCode.Aps.Enums;

/// <summary>
/// Defines the state of saved strings based on how they were confirmed.
/// </summary>
public enum SavedStrings
{
    /// <summary>
    /// Automatically confirmed as yes.
    /// </summary>
    AutoYes,
    /// <summary>
    /// Manually confirmed as yes by user.
    /// </summary>
    ManuallyYes,
    /// <summary>
    /// Manually rejected by user.
    /// </summary>
    ManuallyNo,
    /// <summary>
    /// Automatically rejected.
    /// </summary>
    AutoNo
}