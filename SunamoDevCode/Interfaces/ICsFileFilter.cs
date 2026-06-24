namespace SunamoDevCode.Interfaces;

// Interface for filtering C# files
public interface ICsFileFilter
{
    // Gets filtered list of files
    List<string> GetFilesFiltered(string path, string searchPattern, SearchOption searchOption);
}
