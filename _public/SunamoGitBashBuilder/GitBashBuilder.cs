namespace SunamoDevCode._public.SunamoGitBashBuilder;

public class GitBashBuilder : IGitBashBuilder
{
    public void Pull()
    {
        Git("pull");
        AppendLine();
    }
    #region Git commands
    /// <summary>
    /// 11-9 the repoUrl attribute has been removed because it is fully replaceable with args
    /// 
    /// </summary>
    /// <param name="args"></param>
    public void Clone(string args)
    {
        Git("clone " + args);
        AppendLine();
    }

    public void Commit(bool addAllUntrackedFiles, string commitMessage)
    {
        ThrowEx.IsNullOrWhitespace("commitMessage", commitMessage);
        Git("commit ");
        if (addAllUntrackedFiles)
        {
            Append("-a");
        }
        if (!string.IsNullOrWhiteSpace(commitMessage))
        {
            Append("-m " + SH.WrapWithQm(commitMessage));
        }
        AppendLine();
    }
    public void Push(bool force)
    {
        Git("push");
        if (force)
        {
            Append("-f");
        }
        AppendLine();
    }
    public void Push(string arg)
    {
        Git("push");
        Append(arg);
        AppendLine();
    }




    public void Init()
    {
        Git("init");
        AppendLine();
    }
    public void Add(string v)
    {
        Git("add");
        Append(v);
        AppendLine();
    }
    public void Config(string v)
    {
        Git("config");
        Append(v);
        AppendLine();
    }









    public void Clean(string v)
    {
        Git("clean");
        Arg(v);
        AppendLine();
    }
    public static string GitStatic(StringBuilder sb, string remainCommand)
    {
        sb.Append("git " + remainCommand);
        return sb.ToString();
    }

    private void Git(string remainCommand)
    {
        if (remainCommand[remainCommand.Length - 1] != ' ')
        {
            remainCommand += " ";
        }
        sb.Append((GitForDebug ? "GitForDebug " : "git ") + remainCommand);
    }
    #endregion
    private void Arg(string v)
    {
        Append(AllStrings.dash + v);
    }
    public void Remote(string arg)
    {
        Git("remote");
        Append(arg);
        AppendLine();
    }
    public void Status()
    {
        Git("status");
        AppendLine();
    }
    public void Fetch(string s = Consts.se)
    {
        Git("fetch " + s);
        AppendLine();
    }
    public void Merge(string v)
    {
        Git("merge " + v);
        AppendLine();
    }
    public void AddNewRemote(string s)
    {
        Remote("add origin " + s);
        Fetch("origin");
        Checkout("-b master --track origin/master");
        AppendLine("vsGitIgnoreGitHub");
        AppendLine("gaacipuu");
    }
    public void Checkout(string arg)
    {
        Git("checkout");
        AppendLine(arg);
    }

    private static Type type = typeof(GitBashBuilder);
    public TextBuilderDC sb = null;





    public GitBashBuilder(TextBuilderDC sb)
    {
        this.sb = sb;

    }
    public bool GitForDebug = false;
    public List<string> Commands { get => SHGetLines.GetLines(ToString()); }

    public static string CreateGitAddForFiles(StringBuilder sb, List<string> linesFiles)
    {
        return CreateGitCommandForFiles("add", sb, linesFiles);
    }















    public static string GenerateCommandForGit(object tlb, string solution, List<string> linesFiles, out bool anyError, string searchOnlyWithExtension, string command, string basePathIfA2SolutionsWontExistsOnFilesystem)
    {
        var filesToCommit = GitBashBuilder.PrepareFilesToSimpleGitFormat(tlb, solution, linesFiles, out anyError, searchOnlyWithExtension, basePathIfA2SolutionsWontExistsOnFilesystem);
        if (filesToCommit == null || filesToCommit.Count == 0)
        {
            return "";
        }
        string result = GitBashBuilder.CreateGitCommandForFiles(command, new StringBuilder(), filesToCommit);

        return result;
    }






    public static string CheckoutWithExtension(string folder, string typedExt, List<string> files, string basePathIfA2SolutionsWontExistsOnFilesystem, TextBuilderDC ci)
    {
        ThrowEx.IsNull("typedExt", typedExt);
        GitBashBuilder bashBuilder = new GitBashBuilder(ci);
        bool anyError = false;
        var filesToCommit = GitBashBuilder.PrepareFilesToSimpleGitFormat(null, folder, files, out anyError, typedExt, basePathIfA2SolutionsWontExistsOnFilesystem);
        if (filesToCommit == null)
        {

        }

        string result = GitBashBuilder.GenerateCommandForGit(null, folder, files, out anyError, typedExt, "checkout", basePathIfA2SolutionsWontExistsOnFilesystem);
        return result;
    }











    public static List<string> PrepareFilesToSimpleGitFormat(object tlb, string solution, List<string> linesFiles, out bool anyError, string searchOnlyWithExtension, string basePathIfA2SolutionsWontExistsOnFilesystem)
    {
        searchOnlyWithExtension = searchOnlyWithExtension.TrimStart(AllChars.asterisk);
        anyError = false;


        string pathSearchForFiles = null;
        if (Directory.Exists(solution))
        {
            pathSearchForFiles = solution;
        }
        else
        {
            pathSearchForFiles = Path.Combine(basePathIfA2SolutionsWontExistsOnFilesystem, solution);
        }
        string pathRepository = pathSearchForFiles;
        if (solution == Consts.Cz)
        {

            pathSearchForFiles += AllStrings.bs + solution;
        }

        FS.WithEndSlash(ref pathRepository);
        var files = Directory.GetFiles(pathSearchForFiles, "*.*", System.IO.SearchOption.AllDirectories).ToList();
        files = files.Where(d => !d.Contains(@"\.git\")).ToList();
        CA.Replace(linesFiles, solution, string.Empty);
        CAChangeContent.ChangeContent1(null, linesFiles, SHParts.RemoveAfterFirst, AllStrings.swd);
        CA.Trim(linesFiles);
        CAChangeContent.ChangeContent1(null, linesFiles, FS.AddExtensionIfDontHave, searchOnlyWithExtension);
        CAChangeContent.ChangeContent<bool>(null, linesFiles, FS.Slash, true);
        CAChangeContent.ChangeContent1(null, linesFiles, SHTrim.TrimStart, AllStrings.slash);
        var linesFilesOnlyFilename = FS.OnlyNamesNoDirectEdit(linesFiles);
        anyError = false;
        List<string> filesToCommit = new List<string>();

        Dictionary<string, List<string>> dictPsychicallyExistsFiles = FS.GetDictionaryByFileNameWithExtension(files);
        CA.Replace(files, AllStrings.bs, AllStrings.slash);
        pathRepository = FS.Slash(pathRepository, false);

        for (int i = 0; i < linesFiles.Count; i++)
        {
            var item = linesFilesOnlyFilename[i];

            var itemWithoutTrim = linesFiles[i];
            #region Directory\*
            if (item[item.Length - 1] == AllChars.asterisk)
            {
                item = itemWithoutTrim.TrimEnd(AllChars.asterisk);
                string itemWithoutTrimBackslashed = Path.Combine(pathRepository, FS.Slash(item, false));
                if (Directory.Exists(itemWithoutTrimBackslashed))
                {
                    filesToCommit.Add(item + AllStrings.asterisk);
                }
                else
                {
                    anyError = true;

                }
            }
            #endregion
            #region *File - add all files without specify root directory
            else if (item[0] == AllChars.asterisk)
            {
                string file = item.Substring(1);
                if (dictPsychicallyExistsFiles.ContainsKey(file))
                {
                    foreach (var item2 in dictPsychicallyExistsFiles[file])
                    {
                        filesToCommit.Add(FS.Slash(item2.Replace(pathRepository, string.Empty), true));
                    }
                }
            }
            #endregion
            #region Exactly defined file
            else
            {
                var fullPath = item;
                item = Path.GetFileName(item);
                #region File isnt in dict => Dont exists
                if (!dictPsychicallyExistsFiles.ContainsKey(item))
                {
                    anyError = true;

                }
                #endregion
                else
                {
                    string itemWithoutTrimBackslashed = Path.Combine(pathRepository, FS.Slash(itemWithoutTrim, false));
                    #region Add as relative file
                    if (itemWithoutTrim.Contains(AllStrings.slash))
                    {
                        if (File.Exists(itemWithoutTrimBackslashed))
                        {
                            filesToCommit.Add(itemWithoutTrim.Replace(pathRepository, string.Empty));
                        }
                        else
                        {
                            anyError = true;

                        }
                    }
                    #endregion
                    #region Add file in root of repository
                    else
                    {
                        if (dictPsychicallyExistsFiles[item].Count == 1)
                        {
                            filesToCommit.Add(FS.Slash(dictPsychicallyExistsFiles[item][0].Replace(pathRepository, string.Empty), true));
                        }
                        else
                        {
                            anyError = true;

                        }
                    }
                    #endregion
                }
            }
            #endregion
        }
        if (anyError)
        {

            return null;
        }
        return filesToCommit;
    }
    public static string xSomeErrorsOccured = "SomeErrorsOccured";
    public static string CreateGitCommandForFiles(string command, StringBuilder sb, List<string> linesFiles)
    {
        return null;

    }
    public void Cd(string key)
    {
        sb.AppendLine("cd " + SH.WrapWith(key, AllStrings.qm));
    }
    public void Clear()
    {
        sb.Clear();
    }
    public void Append(string text)
    {
        sb.Append(text);
    }
    public void AppendLine(string text)
    {
        sb.AppendLine(text);
    }
    public void AppendLine()
    {
        sb.AppendLine();
    }
    public override string ToString()
    {
        return sb.ToString();
    }
}