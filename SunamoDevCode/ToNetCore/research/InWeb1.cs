// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.ToNetCore.research;
public partial class MoveToNet5
{
    /// <summary>
    /// 1 = sdk style, not netstandard2.0
    /// 2 = sdk style, netstandard2.0
    /// 3 = non sdk style
    /// </summary>
    /// <param name = "appendHeaderForWeb"></param>
    /// <returns></returns>
    public 
#if ASYNC
    async Task<FindProjectsWhichIsSdkStyleResult>
#else
    FindProjectsWhichIsSdkStyleResult 
#endif
    FindProjectsWhichIsSdkStyle(ILogger logger, bool appendHeaderForWeb, bool web = true)
    {
        List<string> csprojSdkStyleList = new List<string>();
        List<string> netstandardList = new List<string>();
        List<string> nonCsprojSdkStyleList = new List<string>();
        if (appendHeaderForWeb)
        {
            csprojSdkStyleList.Add("Web but in SDK style:");
        }

        var temp = WebAndNonWebProjects(logger);
        List<string> lines = null;
        if (web)
        {
            lines = temp.Item1;
        }
        else
        {
            lines = temp.Item2;
        }

        foreach (var item2 in lines)
        {
            if (Ignored.IsIgnored(item2))
            {
                continue;
            }

            var item = item2;
            var tu = 
#if ASYNC
    await
#endif
            SunamoCsprojHelper.IsProjectCsprojSdkStyleIsCore( /*ref*/item);
            if (tu.isProjectCsprojSdkStyleIsCore)
            {
                if (tu.isNetstandard)
                {
                    netstandardList.Add(item);
                }
                else
                {
                    csprojSdkStyleList.Add(item);
                }
            }
            else
            {
                nonCsprojSdkStyleList.Add(item);
            }
        }

        return new FindProjectsWhichIsSdkStyleResult
        {
            csprojSdkStyleList = csprojSdkStyleList,
            netstandardList = netstandardList,
            nonCsprojSdkStyleList = nonCsprojSdkStyleList
        };
    }

    public 
#if ASYNC
    async Task<string>
#else
    void 
#endif
    WebProjectsWhichIsNotSdkStyle(ILogger logger)
    {
        var u = 
#if ASYNC
    await
#endif
        FindProjectsWhichIsSdkStyle(logger, true);
        return SHJoin.JoinNL(u.nonCsprojSdkStyleList);
    }

    string nameProject = null;
    public async void ReplaceProjectReferenceForWeb(ILogger logger, string name, string ns)
    {
        Console.WriteLine("Solution old & new must be in same root folder");
        name = SHTrim.TrimEnd(name, ".web");
        ns = SHTrim.TrimEnd(ns, ".web");
        nameProject = name;
        string old = @"..\..\" + ns + @"\" + name + @"\" + name + ".csproj";
        string nuova = @"..\..\" + SolutionNameFor(ns) + @"\" + name + @".web\" + name + ".web.csproj";
        var temp = WebAndNonWebProjects(logger);
        foreach (var item in temp.Item1)
        {
            Console.WriteLine(item);
            //DebugLogger.Instance.WriteLine(item);
            //var dx =
            await ReplaceOrRemoveFile(WithWebEnd, ElementsItemGroup.ProjectReference, [old], item, nuova);
        //if (dx != -1)
        //{
        //    //break;
        //}
        }
    }

    string WithWebEnd(string text)
    {
        return text.Replace("<Name>" + nameProject + "</Name>", "<Name>" + nameProject + ".web</Name>");
    }

    private string SolutionNameFor(string name)
    {
        if (name == "PlatformIndependentNuGetPackages")
        {
            return "sunamo.webWithoutDep";
        }

        return name + ".web";
    }
}