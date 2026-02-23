namespace SunamoDevCode.Aps.Enums;

/// <summary>
/// Defines modes for repairing wrong characters in source code.
/// </summary>
public enum RepairWrongCharsInSourceCodeMode
{
    /// <summary>
    /// Repair incorrect letters by replacing them with correct ones.
    /// </summary>
    RepairLetters,
    /// <summary>
    /// Only check for wrong characters without repairing.
    /// </summary>
    CheckWrongChars
}