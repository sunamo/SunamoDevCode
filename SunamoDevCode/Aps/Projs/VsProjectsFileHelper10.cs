namespace SunamoDevCode.Aps.Projs;

/// <summary>
/// Use VsProjectFile, 
/// </summary>
public partial class VsProjectsFileHelper
{
    /// <summary>
    /// Splits a relative path into tokens using detected delimiter (backslash, forward slash, or quotes).
    /// </summary>
    /// <param name="relativePath">Relative path to tokenize.</param>
    /// <returns>List of path tokens.</returns>
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
    /// <summary>
    /// Cache of project references indexed by csproj path.
    /// </summary>
    public static Dictionary<string, ProjectReferences> cacheProjectReferences = new Dictionary<string, ProjectReferences>();
    /// <summary>
    /// Gets project references from a csproj file. Works for .NET Core and Framework.
    /// Requires Microsoft.Build. For full dependency tree, use BuildProjectsDependencyTree.
    /// </summary>
    /// <param name="csprojPath">Path to the csproj file.</param>
    /// <param name="dictToAvoidCollectionWasChanged">Dictionary cache for XML documents to avoid collection-changed exceptions.</param>
    /// <param name="uriKind">Whether to return absolute or relative paths.</param>
    /// <returns>Project references found in the csproj.</returns>
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
        var projectReferences = nodes.Select(d => XmlHelper.Attr(d, "Include")!).ToList();
        var dr = FS.GetDirectoryName(csprojPath);
        if (uriKind == UriKind.Absolute)
        {
            CAChangeContent.ChangeContent(new ChangeContentArgsDC { }, projectReferences, FS.GetAbsolutePath2, dr);
        }
        else if (uriKind == UriKind.Relative)
        {
            CAChangeContent.ChangeContent(new ChangeContentArgsDC { SwitchFirstAndSecondArg = true }, projectReferences, null!, dr, Path.GetRelativePath);
        }
        var pr = new ProjectReferences { Projects = projectReferences, Nodes = nodes };
        if (!cacheProjectReferences.ContainsKey(csprojPath))
        {
            cacheProjectReferences.Add(csprojPath, pr);
        }
        return pr;
    }
    #endregion
    /// <summary>
    /// Adds missing files to a csproj as Compile items by comparing with already included files.
    /// </summary>
    /// <param name="sln">Solution folder containing the project.</param>
    /// <param name="csprojpath">Path to the csproj file.</param>
    /// <param name="files">List of absolute file paths to add.</param>
    public static async Task AddFilesToCsproj(SolutionFolder sln, string csprojpath, List<string> files)
    {
        List<string> containedFiles = new List<string>();
        var dir = FS.GetDirectoryName(csprojpath);
        VsProjectFile vs = new VsProjectFile(csprojpath);
        if (!vs.IsValidXml)
        {
            return;
        }
        var compile = vs.ReturnAllItemGroup(ItemGroups.Compile).Select(d => XmlHelper.Attr(d, "Include")!);
        foreach (var item in compile)
        {
            containedFiles.Add(FS.GetAbsolutePath(dir, item!));
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