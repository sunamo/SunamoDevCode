namespace SunamoDevCode.ToNetCore.research;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public partial class MoveToNet5
{
    private 
#if ASYNC
    async Task
#else
    void 
#endif
    ReplaceOrRemoveFile(Func<string, string> addReplace, string element, List<string> refe, string pathCsproj, string newRefe = null)
    {
        bool replace = newRefe != null;
        refe2 = "</" + element + ">";
#if DEBUG
        if (pathCsproj.EndsWith(@"sunamo.web.csproj"))
        {
            ThisApp.check = true;
        }
#endif
        var xd = 
#if ASYNC
    await
#endif
        XmlDocumentsCache.Get(pathCsproj);
        if (MayExcHelper.MayExc(xd.Exc))
        {
            return;
        }

        var tf = xd.Data.OuterXml;
        string n = tf;
        int dx = -1;
        int dx2 = -1;
        bool combine = false;
        foreach (var r in refe)
        {
            combine = false;
            dx = -1;
            dx2 = -1;
            if (replace)
            {
                n = SHReplace.ReplaceWithIndex(n, "<" + element + " Include=\"" + r + "\" />", string.Empty, ref dx);
                n = SHReplace.ReplaceWithIndex(n, "<" + element + " Include=\"" + r + "\"/>", string.Empty, ref dx);
                n = SHReplace.ReplaceWithIndex(n, "<" + element + " Include=\"" + r + "\"></" + element + ">", string.Empty, ref dx);
            }
            else
            {
                n = n.Replace("<" + element + " Include=\"" + r + "\" />", string.Empty);
                n = n.Replace(GetReferenceShortest(element, r), string.Empty);
                n = n.Replace("<" + element + " Include=\"" + r + "\"></" + element + ">", string.Empty);
            }

            if (dx == -1)
            {
                string toFind = ReferenceLongest(element, r);
                dx = n.IndexOf(toFind);
                combine = true;
            }

            if (dx != -1 && combine)
            {
                n = n.Remove(dx, ReferenceLongest(element, r).Length);
                dx2 = n.IndexOf(refe2, dx);
                n = n.Remove(dx2, refe2.Length);
            }

            if (dx != -1)
            {
                if (replace)
                {
                    var refe3 = GetReferenceShortest(element, newRefe);
                    n = n.Insert(dx, refe3);
                }
            }
        }

        if (addReplace != null)
        {
            n = addReplace(n);
        }

        if (n != tf)
        {
#if DEBUG
            var list =
#if ASYNC
    await
#endif
 TF.ReadAllLines(pathCsproj);
            var nf = FS.InsertBetweenFileNameAndExtension(pathCsproj, "_b");

#if ASYNC
            await
#endif
            TF.WriteAllLines(nf, list);
#endif
            await XmlDocumentsCache.Set(pathCsproj, n);
#if ASYNC
            await
#endif
            TF.WriteAllText(pathCsproj, n);
        }

        ThisApp.check = false;
    }

    private static string ReferenceLongest(string element, string r)
    {
        return "<" + element + " Include=\"" + r + "\">";
    }

    private static string GetReferenceShortest(string element, string r)
    {
        return "<" + element + " Include=\"" + r + "\"/>";
    }

    /// <summary>
    /// Don't run, wait whether will be really needed to run
    /// it could do more harm than good
    /// </summary>
    private 
#if ASYNC
    async Task
#else
    void 
#endif
    CommentAssemblyInfoCsFiles(ILogger logger)
    {
        var data = WebAndNonWebProjects(logger);
        var lc = "//";
        foreach (var item in data.Item2)
        {
            var d2 = FS.GetDirectoryName(item);
            var ass = FSGetFiles.GetFiles(logger, d2, "AssemblyInfo.cs", true);
            foreach (var item2 in ass)
            {
                var list = 
#if ASYNC
    await
#endif
                TF.ReadAllLines(item2);
                if (!list.All(r => r.StartsWith(lc)))
                {
                    CA.StartingWith(lc, list);
                    await TF.WriteAllLines(item2, list);
                }
            }
        }
    }
}