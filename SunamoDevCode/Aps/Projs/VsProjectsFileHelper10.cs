// variables names: ok
namespace SunamoDevCode.Aps.Projs;

/// <summary>
/// Use VsProjectFile, 
/// </summary>
public partial class VsProjectsFileHelper
{
    public static List<string> GetTokens(string relativePath)
    {
        var deli = "";
        if (relativePath.Contains("\""))
            deli = "\"";
        else if (relativePath.Contains("/")) deli = "/";
        else
        {
            ThrowEx.NotImplementedCase(relativePath);
        }
        return SHSplit.Split(relativePath, deli);
    }
    #region Use cacheProjectReferences
    public static Dictionary<string, ProjectReferences> cacheProjectReferences = new Dictionary<string, ProjectReferences>();
    /// <summary>
    /// Is working for .net core &amp; fw
    /// To call, must be installed Microsoft.Build
    ///
    /// Get full path in one csproj
    /// TO full tree use BuildProjectsDependencyTree
    /// </summary>
    /// <param name="csprojPath"></param>
    /// <returns></returns>
    //public async static Task<ProjectReferences> GetProjectReferencesAsync(string csprojPath, UriKind uri = UriKind.Absolute)
    public static
#if ASYNC
        async Task<ProjectReferences>
#else
            ProjectReferences
#endif
        GetProjectReferences(string csprojPath, Dictionary<string, XmlDocument> dictToAvoidCollectionWasChanged, UriKind uriKind = UriKind.Absolute)
    {
        // Not 
        //ThrowEx.FirstLetterIsNotUpper(csprojPath);
        csprojPath = SH.FirstCharUpper(csprojPath);
        if (cacheProjectReferences.ContainsKey(csprojPath))
        {
            return cacheProjectReferences[csprojPath];
        }
        if (!
FS.ExistsFile(csprojPath))
        {
            return new ProjectReferences();
        }
        VsProjectFile vs = new VsProjectFile();
#if ASYNC
        await
#endif
        vs.Load(csprojPath, dictToAvoidCollectionWasChanged);
        if (!vs.IsValidXml)
        {
            return new ProjectReferences();
        }
        var nodes = vs.ReturnAllItemGroup(ItemGroups.ProjectReference);
        var projectReferences = nodes.Select(d => XmlHelper.Attr(d, "Include")).ToList();
        var dr = FS.GetDirectoryName(csprojPath);
        if (uriKind == UriKind.Absolute)
        {
            CAChangeContent.ChangeContent(new ChangeContentArgsDC { }, projectReferences, FS.GetAbsolutePath2, dr);
        }
        else if (uriKind == UriKind.Relative)
        {
            CAChangeContent.ChangeContent(new ChangeContentArgsDC { SwitchFirstAndSecondArg = true }, projectReferences, null, dr, Path.GetRelativePath);
        }
        var pr = new ProjectReferences { Projects = projectReferences, Nodes = nodes };
        if (!cacheProjectReferences.ContainsKey(csprojPath))
        {
            cacheProjectReferences.Add(csprojPath, pr);
        }
        return pr;
    }
    #endregion
    public static async Task AddFilesToCsproj(SolutionFolder sln, string csprojpath, List<string> files)
    {
        List<string> containedFiles = new List<string>();
        var dir = FS.GetDirectoryName(csprojpath);
        VsProjectFile vs = new VsProjectFile(csprojpath);
        if (!vs.IsValidXml)
        {
            return;
        }
        var compile = vs.ReturnAllItemGroup(ItemGroups.Compile).Select(d => XmlHelper.Attr(d, "Include"));
        foreach (var item in compile)
        {
            containedFiles.Add(FS.GetAbsolutePath(dir, item));
        }
        foreach (var item in files)
        {
            if (!containedFiles.Contains(item))
            {
                CompileItemGroup c = new CompileItemGroup(csprojpath);
                var relativePathFromSolution = ApsHelper.Instance.GetRelativePathFromSolution(sln, item);
                var tokens = FS.GetTokens(relativePathFromSolution);
                tokens.RemoveAt(0);
                tokens.RemoveAt(0);
                c.Include = Path.Combine(tokens.ToArray());
                await AddItemGroupSdkStyle(csprojpath, ItemGroups.Compile, c, true);
            }
        }
    }
}