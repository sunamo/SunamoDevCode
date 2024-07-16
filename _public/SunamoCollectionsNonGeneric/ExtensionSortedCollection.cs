namespace SunamoDevCode._public.SunamoCollectionsNonGeneric;

public class ExtensionSortedCollection
{
    public Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
    
    
    
    
    public ExtensionSortedCollection(params string[] d)
    {
        d.ToList().ForEach(fileName => AddOnlyFileName(fileName));
    }
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
    public void AddWholeFilePath(string filePath)
    {
        AddOnlyFileName(Path.GetFileName(filePath));
    }
}