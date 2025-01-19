namespace SunamoDevCode.Aps.Projs;

using Microsoft.Extensions.Logging;
using SunamoDevCode.Aps.Projs;

public class ApsProjsHelper
{
    public static void ReplacePathWithCsproj(ILogger logger, List<string> projectsToAdd)
    {
        for (int i = 0; i < projectsToAdd.Count; i++)
        {
            var item = projectsToAdd[i];
            var csproj = ApsHelper.ci.GetCsprojsOnlyTopDirectory(logger, item);
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
    public static Type type = typeof(ApsProjsHelper);
    public static
#if ASYNC
    async Task<XmlDocument>
#else
    XmlDocument
#endif
         AbsoluteToRelativeCsProj(string x)
    {
        if (x == null)
        {
            return null;
        }
        var xd =
#if ASYNC
            await
#endif
        XmlDocumentsCache.Get(x);
        if (MayExcHelper.MayExc(xd.Exc))
        {
            return null;
        }
        var pr =
#if ASYNC
            await
#endif
        VsProjectsFileHelper.GetProjectReferences(x, null, UriKind.RelativeOrAbsolute);
        // Když soubor bude obsahovat např. git znaky, GetProjectReferencesAsync
        if (pr.projs != null)
        {
            if (pr.projs.Count != pr.nodes.Count)
            {
                ThrowEx.DifferentCountInLists("nodes", pr.nodes.Count, "projs", pr.projs.Count);
            }
            for (int i = 0; i < pr.nodes.Count; i++)
            {
                var z = pr.projs[i];
                var y = FS.IsAbsolutePath(z);
                if (DefaultPaths.IsIgnored(z))
                {
                    continue;
                }
                if (y)
                {
                    //z = DefaultPaths.ConvertToActualPlatform(z);
                    var rel = Path.GetRelativePath(x, z);
                    var node = pr.nodes[i];
                    XmlNode cloned = node.Clone();
                    XmlHelper.SetAttribute(cloned, "Include", rel);
                    node.ParentNode.ReplaceChild(cloned, node);
                }
            }
            await XmlDocumentsCache.Set(x, xd.Data);
        }
        return xd.Data;
    }
    #region Projs - kde pracuji s projekty absolutně nezávísle na existenci sln
    public static
#if ASYNC
    async Task
#else
    void
#endif
 SaveEmptyTestProject(string csproj)
    {
#if ASYNC
        await
#endif
        TF.WriteAllText(csproj, "<Project Sdk=\"Microsoft.NET.Sdk\"></Project>");
    }
    public static
#if ASYNC
    async Task
#else
    void
#endif
 SaveEmptyFullNetProject(string csProj, string projectName)
    {
        string c = SHFormat.Format4(
#if ASYNC
    await
#endif
 TF.ReadAllText(Path.Combine(DefaultPaths.eVsProjectsPinp, @"SunamoDevCode\Templates\FullNet.xml")), projectName);
#if ASYNC
        await
#endif
        TF.WriteAllText(csProj, c);
    }
    [Obsolete("Used MSBuildProject")]
    public static string RemoveSchema(string d)
    {
        return d; // d.Replace("<ItemGroup xmlns=\"" + MSBuildProject.Schema + "\">", "<ItemGroup>");
    }
    #endregion
    #region Uap
    public static void ChangeUapFramework(ILogger logger, SolutionFolder solutionFolder, string target, string min)
    {
        SolutionFolder.GetCsprojs(logger, solutionFolder);
        foreach (var item in solutionFolder.projectsGetCsprojs)
        {
            VsProjectsFileHelper.SetTargetFrameworksUap(item, target, min);
        }
    }
    #endregion
}