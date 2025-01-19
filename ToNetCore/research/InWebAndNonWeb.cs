

public partial class MoveToNet5
{
    /// <summary>
    /// Nesmírně užitečné, zjistí mi projekty které užívám jak ve web tak non web
    /// Čili když něco bude i ve web musím mu nechat netstandard
    /// </summary>
    /// <returns></returns>
    public bool TableWebAndNonWeb()
    {
        var v = WebAndNonWebProjects();
        var web = v.Item1;
        var nonWeb = v.Item2;

        var webFn = FS.OnlyNamesWithoutExtensionCopy(web);
        var nonWebFn = FS.OnlyNamesWithoutExtensionCopy(nonWeb);

        CA.Replace(webFn, ".web", string.Empty);

        var both = CAG.CompareList(webFn, nonWebFn);

        var dx = webFn.Where(d => d.Contains("desktop"));
        var dx2 = nonWebFn.Where(d => d.Contains("desktop"));

        int dxWeb, dxNonWeb;
        List<Tuple<ProjFw, ProjFw>> toTable = new List<Tuple<ProjFw, ProjFw>>();

        foreach (var item in both)
        {
            dxWeb = webFn.IndexOf(item);
            dxNonWeb = nonWebFn.IndexOf(item);

            var f1 = web[dxWeb];
            var f2 = nonWeb[dxNonWeb];

            //ProjFw pfWeb = new ProjFw { path = f1, t = FrameworkNameDetector.Detect(f1). };

        }

        return false;
    }

    public
#if ASYNC
    async Task
#else
    void  
#endif
 RestoreFromBackup()
    {
        CmdApp.openAndWaitForChangeContentOfInputFile = true;
        CmdApp.WaitForSaving(ProgramShared.inputFile, PHWin.Code);

        var k =
#if ASYNC
    await
#endif
 TF.ReadAllLines(ProgramShared.inputFile);
        foreach (var item in k)
        {
            var old = item + AllExtensions.old;
            if (FS.ExistsFile(old))
            {
                FS.MoveFile(old, item, FileMoveCollisionOption.Overwrite);
            }
            else
            {
                Console.WriteLine("Doesn't exists: " + old);
            }
        }
    }

    public void ListOfAllWebAndNonWeb()
    {
        var t = WebAndNonWebProjects(false);
        TextOutputGenerator tog = new TextOutputGenerator();
        tog.List(t.Item1, "Web");
        tog.List(t.Item2, "NonWeb");

        Output = tog.ToString();
        OutputOpen();
    }

    public
#if ASYNC
    async Task
#else
    void  
#endif
 ReplaceUnneedReferencesInCsprojsNotSdKStyle(bool web = true)
    {
        StringBuilder sb = new StringBuilder();
        var f =
#if ASYNC
    await
#endif
 FindProjectsWhichIsSdkStyle(false, web);
        foreach (var item in f.nonCsprojSdkStyleList)
        {
            if (item.EndsWith("_b.csproj"))
            {
                continue;
            }
            ReplaceUnneedReferencesInCsprojs(item);
            sb.AppendLine(GenerateTryConvert(item));
        }
        var sbs = sb.ToString();
        Output = sbs;
        OutputOpen();
    }

    public string GenerateTryConvert(string p)
    {
        // poslední verze try-convert je 0.9.232202 a ta funguje na .NET 5. Proto musím přidávat ty 2 parametry
        return @"try-convert  --target-framework net5.0 -m 'C:\Program Files\dotnet\sdk\5.0.100\' -w '" + p + "'";
    }

    public
#if ASYNC
    async Task<string>
#else
    void  
#endif
 GetAllTargetFrameworks()
    {
        List<string> hasMoreTargetFrameworkElements = new List<string>();

        Dictionary<string, List<string>> ls = new Dictionary<string, List<string>>();

        var d = WebAndNonWebProjects(true);

        foreach (var item in d.Item2)
        {
            var c =
#if ASYNC
    await
#endif
 TF.ReadAllText(item);

            var s = SH.GetTextBetweenSimple(c, ChangeProjects.start, ChangeProjects.end, false);

            if (s != null)
            {
                DictionaryHelper.AddOrCreate(ls, s, item);
            }

            if (SH.OccurencesOfStringIn(c, ChangeProjects.start) > 1)
            {
                hasMoreTargetFrameworkElements.Add(item);
            }
        }

        TextOutputGenerator tog = new TextOutputGenerator();
        tog.Dictionary(ls);
        tog.List(hasMoreTargetFrameworkElements, "hasMoreTargetFrameworkElements");

        return tog.ToString();
    }
}
