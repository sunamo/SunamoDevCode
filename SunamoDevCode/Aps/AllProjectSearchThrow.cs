namespace SunamoDevCode.Aps;

public class AllProjectsSearchThrow
{
    public static void IsNotValidProjectFile(string filePath)
    {
        ThrowEx.Custom(filePath + " is not valid project file.");
    }
}
