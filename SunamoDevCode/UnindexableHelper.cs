// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode;

public class UnindexableHelper
{
    public static Unindexable unindexable;

    public static PpkOnDriveDC unindexablePathParts => unindexable.unindexablePathParts;
    public static PpkOnDriveDC unindexableFileNames => unindexable.unindexableFileNames;
    public static PpkOnDriveDC unindexablePathEnds => unindexable.unindexablePathEnds;
    public static PpkOnDriveDC unindexablePathStarts => unindexable.unindexablePathStarts;

    /// <summary>
    ///     Into A1 insert SearchCodeElementsUC .ufp
    /// </summary>
    /// <param name="f"></param>
    public static void Load(UnindexableFilesPaths f)
    {
        unindexable = new Unindexable();

        //ClipboardService.SetText(f.fileUnindexablePathParts);
        //PD.ShowMb(f.fileUnindexablePathParts);

        unindexable.unindexablePathParts = new PpkOnDriveDC(f.fileUnindexablePathParts);

        unindexable.unindexableFileNames = new PpkOnDriveDC(f.fileUnindexableFileNames);
        unindexable.unindexableFileNamesExactly = new PpkOnDriveDC(f.fileUnindexableFileNamesExactly);
        unindexable.unindexablePathEnds = new PpkOnDriveDC(f.fileUnindexablePathEnds);
        unindexable.unindexablePathStarts = new PpkOnDriveDC(f.fileUnindexablePathStarts);
    }

    public static bool IsToIndexedFolder(string d)
    {


        if (unindexablePathStarts != null && unindexablePathParts != null)
        {
            if (unindexablePathParts.TrueForAll(e => !d.Contains(e)))
                if (unindexablePathStarts.TrueForAll(e => !d.StartsWith(e)))
                    return true;
        }
        else
        {
            return true;
        }

        return false;
    }

    public static bool IsToIndexed(string d, string fn, Func<string, bool> sci_IsIndexed)
    {
        if (unindexablePathEnds != null && unindexableFileNames != null)
        {
            //Checking for sth for which is checking in SourceCodeIndexerRoslyn.ProcessFile
            if (unindexablePathEnds.TrueForAll(e => !d.EndsWith(e)))
                if (unindexableFileNames.TrueForAll(e => !fn.Contains(e)))
                {
                    if (sci_IsIndexed == null)
                        return true;
                    return sci_IsIndexed(d);
                }
        }
        else
        {
            return true;
        }

        return false;
    }
}