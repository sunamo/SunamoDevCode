// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.Aps.Enums;

public enum SolutionNameMustContains
{
    Text,
    // Není možné použít, v první fázi se zjistí projekty které by mohli být potenciálně duplicitní. V druhé 
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
    None
}