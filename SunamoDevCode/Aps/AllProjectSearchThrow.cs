// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.Aps;

public class AllProjectsSearchThrow
{
    static Type type = typeof(AllProjectsSearchThrow);
#pragma warning disable
    public static void IsNotValidProjectFile(object type, string methodName, string file)
#pragma warning restore
    {
        ThrowEx.Custom(file + " is not valid project file.");
    }
}
