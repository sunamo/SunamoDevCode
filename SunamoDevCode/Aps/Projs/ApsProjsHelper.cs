// variables names: ok
namespace SunamoDevCode.Aps.Projs;

/// <summary>
/// Helper class for working with All Projects Search (APS) project files.
/// </summary>
public class ApsProjsHelper
{
    /// <summary>
    /// Type of the ApsProjsHelper class for reflection purposes.
    /// </summary>
    public static Type Type = typeof(ApsProjsHelper);

    /// <summary>
    /// Replaces folder paths with their corresponding .csproj file paths.
    /// </summary>
    /// <param name="logger">Logger for error/info messages.</param>
    /// <param name="projectsToAdd">List of project paths to replace.</param>
    public static void ReplacePathWithCsproj(ILogger logger, List<string> projectsToAdd)
    {
        for (int i = 0; i < projectsToAdd.Count; i++)
        {
            var item = projectsToAdd[i];
            var csproj = ApsHelper.Instance.GetCsprojsOnlyTopDirectory(logger, item);
            if (csproj.Count > 0)
            {
                projectsToAdd[i] = csproj[0];
            }
            else
            {
                projectsToAdd[i] = "Folder " + item + " doesn't have csproj";
            }
        }
    }
    /// <summary>
    /// Converts absolute project reference paths to relative paths in a .csproj file.
    /// </summary>
    /// <param name="csprojPath">Path to the .csproj file.</param>
    /// <param name="basePath">Base path to use for path calculations.</param>
    /// <returns>The modified XML document, or null if the file doesn't exist or has errors.</returns>
    public static
#if ASYNC
    async Task<XmlDocument>
#else
    XmlDocument
#endif
         AbsoluteToRelativeCsProj(string csprojPath, string basePath)
    {
        if (csprojPath == null)
        {
            return null;
        }
        var xmlDocumentResult =
#if ASYNC
            await
#endif
        XmlDocumentsCache.Get(csprojPath);
        if (MayExcHelper.MayExc(xmlDocumentResult.Exc))
        {
            return null;
        }
        var projectReferences =
#if ASYNC
            await
#endif
        VsProjectsFileHelper.GetProjectReferences(csprojPath, null, UriKind.RelativeOrAbsolute);
        // When file contains e.g. git characters, GetProjectReferencesAsync
        if (projectReferences.Projects != null)
        {
            if (projectReferences.Projects.Count != projectReferences.Nodes.Count)
            {
                ThrowEx.DifferentCountInLists("nodes", projectReferences.Nodes.Count, "projs", projectReferences.Projects.Count);
            }
            for (int i = 0; i < projectReferences.Nodes.Count; i++)
            {
                var projectPath = projectReferences.Projects[i];
                var isAbsolutePath = FS.IsAbsolutePath(projectPath);
                if (DefaultPaths.IsIgnored(projectPath, basePath))
                {
                    continue;
                }
                if (isAbsolutePath)
                {
                    var relativePath = Path.GetRelativePath(csprojPath, projectPath);
                    var node = projectReferences.Nodes[i];
                    XmlNode clonedNode = node.Clone();
                    XmlHelper.SetAttribute(clonedNode, "Include", relativePath);
                    node.ParentNode.ReplaceChild(clonedNode, node);
                }
            }
            await XmlDocumentsCache.Set(csprojPath, xmlDocumentResult.Data);
        }
        return xmlDocumentResult.Data;
    }
    #region Projs - working with projects absolutely independently of .sln existence
    /// <summary>
    /// Creates an empty SDK-style test project file.
    /// </summary>
    /// <param name="csprojPath">Path where the .csproj file will be created.</param>
    public static
#if ASYNC
    async Task
#else
    void
#endif
 SaveEmptyTestProject(string csprojPath)
    {
#if ASYNC
        await
#endif
        TF.WriteAllText(csprojPath, "<Project Sdk=\"Microsoft.NET.Sdk\"></Project>");
    }

    /// <summary>
    /// Creates an empty Full .NET Framework project file from a template.
    /// </summary>
    /// <param name="csprojPath">Path where the .csproj file will be created.</param>
    /// <param name="projectName">Name of the project.</param>
    /// <param name="projectsBasePath">Base path to the projects directory containing templates.</param>
    public static
#if ASYNC
    async Task
#else
    void
#endif
 SaveEmptyFullNetProject(string csprojPath, string projectName, string projectsBasePath)
    {
        string content = SHFormat.Format4(
#if ASYNC
    await
#endif
 TF.ReadAllText(Path.Combine(projectsBasePath, @"SunamoDevCode\Templates\FullNet.xml")), projectName);
#if ASYNC
        await
#endif
        TF.WriteAllText(csprojPath, content);
    }

    /// <summary>
    /// Removes XML schema from content.
    /// </summary>
    /// <param name="content">The XML content to process.</param>
    /// <returns>The content with schema removed.</returns>
    [Obsolete("Used MSBuildProject")]
    public static string RemoveSchema(string content)
    {
        return content;
    }
    #endregion
    #region Uap
    /// <summary>
    /// Changes the UAP (Universal Application Platform) framework version for all projects in a solution.
    /// </summary>
    /// <param name="logger">Logger for error/info messages.</param>
    /// <param name="solutionFolder">The solution folder containing projects to update.</param>
    /// <param name="targetFramework">The target framework version.</param>
    /// <param name="minFramework">The minimum framework version.</param>
    public static void ChangeUapFramework(ILogger logger, SolutionFolder solutionFolder, string targetFramework, string minFramework)
    {
        SolutionFolder.GetCsprojs(logger, solutionFolder);
        foreach (var item in solutionFolder.ProjectsGetCsprojs)
        {
            VsProjectsFileHelper.SetTargetFrameworksUap(item, targetFramework, minFramework);
        }
    }
    #endregion
}