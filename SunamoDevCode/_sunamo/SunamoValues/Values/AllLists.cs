namespace SunamoDevCode._sunamo.SunamoValues.Values;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
internal partial class AllLists
{
    // In key is long name, in value letter
    internal static Dictionary<string, string>? htmlEntitiesDict = null;
    // When entity have more names, there is just one
    // In key is letter, in value long name
    internal static Dictionary<string, string> htmlEntitiesFullNames = null!;
}