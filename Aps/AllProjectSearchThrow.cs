namespace SunamoDevCode.Aps;

public class AllProjectsSearchThrow
{
    static Type type = typeof(AllProjectsSearchThrow);
    public static void IsNotValidProjectFile(object type, string methodName, string file)
    {
        ThrowEx.Custom(file + " is not valid project file.");
    }
}
