namespace SunamoDevCode.Aps.Enums;

/// <summary>
/// Defines filtering criteria for solution names during search operations.
/// </summary>
public enum SolutionNameMustContains
{
    /// <summary>
    /// Solution name must contain specific text.
    /// </summary>
    Text,
    // Není možné použít, v první fázi se zjistí projekty které by mohli být potenciálně duplicitní. V druhé
    /// <summary>
    /// Solution name must have same name with diacritics variation.
    /// </summary>
    SameNameWithDiacritic,
    /// <summary>
    /// FS.GetNameWithoutSeries
    /// </summary>
    Serie,
    /// <summary>
    /// SH.EndsWithNumber, SHTrim.TrimTrailingNumbersAtEnd
    /// </summary>
    NumberAtEnd,
    // Berou se všechny projekty
    /// <summary>
    /// No filtering applied, all solutions are included.
    /// </summary>
    None
}