// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode;

public class UnindexableFilesPaths
{
    public string fileUnindexableFileNames;
    public string fileUnindexableFileNamesExactly;
    public string fileUnindexablePathEnds;
    public string fileUnindexablePathParts;
    public string fileUnindexablePathStarts;

    public UnindexableFilesPaths(string p)
    {
        var f = UnindexableFilesNames.ci;
        fileUnindexablePathParts = p + f.fileUnindexablePathParts;
        fileUnindexableFileNames = p + f.fileUnindexableFileNames;
        fileUnindexableFileNamesExactly = p + f.fileUnindexableFileNamesExactly;
        fileUnindexablePathEnds = p + f.fileUnindexablePathEnds;
        fileUnindexablePathStarts = p + f.fileUnindexablePathStarts;
    }
}