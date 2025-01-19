

public partial class MoveToNet5
{
    public static List<string> linesFromDontReplaceReferencesIn = null;

    /// <summary>
    /// Dont use XmlDocumentsCache 
    /// </summary>
    /// <returns></returns>
    public Tuple<List<string>, List<string>> WebAndNonWebProjects(bool withCsprojs = true)
    {
       return ApsHelper.WebAndNonWebProjects(withCsprojs);
    }

    public Tuple<List<string>, List<string>> WebAndNonWebSlns()
    {
        List<string> webProjects = new List<string>();
        List<string> notWebProjects = new List<string>();

        foreach (var item in FoldersWithSolutions.fwss)
        {
            var s = item.Solutions(RepositoryLocal.Vs17);
            foreach (var sln in s)
            {
               var slnFullPathFolder = sln.fullPathFolder;
                    if (ApsHelper.IsWeb(slnFullPathFolder))
                    {
                        webProjects.Add(slnFullPathFolder);
                    }
                    else
                    {
                        notWebProjects.Add(slnFullPathFolder);
                    }
            }
        }

        return new Tuple<List<string>, List<string>>(webProjects, notWebProjects);
    }

    
}
