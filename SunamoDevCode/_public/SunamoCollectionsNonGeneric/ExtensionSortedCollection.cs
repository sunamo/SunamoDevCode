namespace SunamoDevCode._public.SunamoCollectionsNonGeneric;

/// <summary>
/// Collection that sorts file names by their extension into a dictionary.
/// </summary>
public class ExtensionSortedCollection
{
    /// <summary>
    /// Dictionary mapping lowercase file extensions to lists of lowercase file names without extensions.
    /// </summary>
    public Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();

    /// <summary>
    /// Creates a new collection from the given file names, sorting them by extension.
    /// </summary>
    /// <param name="d">File names to add.</param>
    public ExtensionSortedCollection(params string[] d)
    {
        d.ToList().ForEach(fileName => AddOnlyFileName(fileName));
    }
    /// <summary>
    /// Adds a file name (without path) to the collection, indexed by its extension.
    /// </summary>
    /// <param name="fileName">File name to add.</param>
    public void AddOnlyFileName(string fileName)
    {
        string key = Path.GetExtension(fileName).ToLower();
        string value = Path.GetFileNameWithoutExtension(fileName).ToLower();
        if (dictionary.ContainsKey(key))
        {
            if (!dictionary[key].Contains(value))
            {
                dictionary[key].Add(value);
            }
        }
        else
        {
            List<string> ad = new List<string>();
            ad.Add(value);
            dictionary.Add(key, ad);
        }
    }
    /// <summary>
    /// Adds a file by its full path, extracting only the file name for sorting.
    /// </summary>
    /// <param name="filePath">Full file path to add.</param>
    public void AddWholeFilePath(string filePath)
    {
        AddOnlyFileName(Path.GetFileName(filePath));
    }
}