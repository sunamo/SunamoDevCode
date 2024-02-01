
namespace SunamoDevCode;

using SunamoCollectionsGeneric.Collections;
using TextCopy;

public class UnindexableHelper
{
    public static Unindexable unindexable = null;

    public static PpkOnDrive unindexablePathParts => unindexable.unindexablePathParts;
    public static PpkOnDrive unindexableFileNames => unindexable.unindexableFileNames;
    public static PpkOnDrive unindexablePathEnds => unindexable.unindexablePathEnds;
    public static PpkOnDrive unindexablePathStarts => unindexable.unindexablePathStarts;

    /// <summary>
    /// Into A1 insert SearchCodeElementsUC .ufp
    /// </summary>
    /// <param name="f"></param>
    public static void Load(UnindexableFilesPaths f)
    {
        unindexable = new Unindexable();

        ClipboardService.SetText(f.fileUnindexablePathParts);
        //PD.ShowMb(f.fileUnindexablePathParts);

        unindexable.unindexablePathParts = new PpkOnDrive(f.fileUnindexablePathParts);

        unindexable.unindexableFileNames = new PpkOnDrive(f.fileUnindexableFileNames);
        unindexable.unindexableFileNamesExactly = new PpkOnDrive(f.fileUnindexableFileNamesExactly);
        unindexable.unindexablePathEnds = new PpkOnDrive(f.fileUnindexablePathEnds);
        unindexable.unindexablePathStarts = new PpkOnDrive(f.fileUnindexablePathStarts);
    }

    public static bool IsToIndexedFolder(string d)
    {
#if DEBUG
        if (d == @"E:\vs\Projects\AllProjectsSearch\Aps.Projs\_\")
        {

        }
#endif

        if (unindexablePathStarts != null && unindexablePathParts != null)
        {
            if (unindexablePathParts.TrueForAll(e => !d.Contains(e)))
            {
                if (unindexablePathStarts.TrueForAll(e => !d.StartsWith(e)))
                {
                    return true;
                }
            }
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
            {
                if (unindexableFileNames.TrueForAll(e => !fn.Contains(e)))
                {
                    if (sci_IsIndexed == null)
                    {
                        return true;
                    }
                    else
                    {
                        return sci_IsIndexed(d);
                    }
                }
            }
        }
        else
        {
            return true;
        }
        return false;
    }
}
