namespace SunamoDevCode._public.SunamoGitBashBuilder;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public partial class GitBashBuilder : IGitBashBuilder
{
    /// <summary>
    /// Prepares a list of files into a simple git-compatible format relative to the repository root.
    /// </summary>
    /// <param name="solution">Solution folder name or path.</param>
    /// <param name="linesFiles">List of file references to prepare.</param>
    /// <param name="anyError">Output flag indicating if any files could not be found.</param>
    /// <param name="searchOnlyWithExtension">File extension filter (e.g., ".cs").</param>
    /// <param name="basePathIfA2SolutionsWontExistsOnFilesystem">Fallback base path when solution directory does not exist on disk.</param>
    /// <returns>List of prepared file paths, or null if any error occurred.</returns>
    public static List<string>? PrepareFilesToSimpleGitFormat(string solution, List<string> linesFiles, out bool anyError, string searchOnlyWithExtension, string basePathIfA2SolutionsWontExistsOnFilesystem)
    {
        searchOnlyWithExtension = searchOnlyWithExtension.TrimStart('*');
        anyError = false;
        string? pathSearchForFiles = null;
        if (Directory.Exists(solution))
        {
            pathSearchForFiles = solution;
        }
        else
        {
            pathSearchForFiles = Path.Combine(basePathIfA2SolutionsWontExistsOnFilesystem, solution);
        }

        string pathRepository = pathSearchForFiles;
        if (solution == "sunamo.cz")
        {
            pathSearchForFiles += "\"" + solution;
        }

        FS.WithEndSlash(ref pathRepository);
        var files = Directory.GetFiles(pathSearchForFiles, "*.*", System.IO.SearchOption.AllDirectories).ToList();
        files = files.Where(d => !d.Contains(@"\.git\")).ToList();
        CA.Replace(linesFiles, solution, string.Empty);
        CAChangeContent.ChangeContent1(null!, linesFiles, SHParts.RemoveAfterFirst, "-");
        CA.Trim(linesFiles);
        CAChangeContent.ChangeContent1(null!, linesFiles, FS.AddExtensionIfDontHave, searchOnlyWithExtension);
        CAChangeContent.ChangeContent<bool>(null!, linesFiles, FS.Slash, true);
        CAChangeContent.ChangeContent1(null!, linesFiles, SHTrim.TrimStart, "/");
        var linesFilesOnlyFilename = FS.OnlyNamesNoDirectEdit(linesFiles);
        anyError = false;
        List<string> filesToCommit = new List<string>();
        Dictionary<string, List<string>> dictPsychicallyExistsFiles = FS.GetDictionaryByFileNameWithExtension(files);
        CA.Replace(files, "\"", "/");
        pathRepository = FS.Slash(pathRepository, false);
        for (int i = 0; i < linesFiles.Count; i++)
        {
            var item = linesFilesOnlyFilename[i];
            var itemWithoutTrim = linesFiles[i];
#region Directory\*
            if (item[item.Length - 1] == '*')
            {
                item = itemWithoutTrim.TrimEnd('*');
                string itemWithoutTrimBackslashed = Path.Combine(pathRepository, FS.Slash(item, false));
                if (Directory.Exists(itemWithoutTrimBackslashed))
                {
                    filesToCommit.Add(item + "*");
                }
                else
                {
                    anyError = true;
                }
            }
#endregion
#region *File - add all files without specify root directory
            else if (item[0] == '*')
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
                    if (itemWithoutTrim.Contains("/"))
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

    /// <summary>
    /// Gets or sets the message text used when errors occur during file preparation.
    /// </summary>
    public static string SomeErrorsOccured { get; set; } = "SomeErrorsOccured";
#pragma warning disable
    public static string CreateGitCommandForFiles(string command, StringBuilder sb, List<string> linesFiles)
    {
        return null;
    }

#pragma warning restore
    /// <summary>
    /// Appends a cd (change directory) command to the script.
    /// </summary>
    /// <param name="key">Directory path to change to.</param>
    public void Cd(string key)
    {
        StringBuilder.AppendLine("cd " + SH.WrapWith(key, "\""));
    }

    /// <summary>
    /// Clears all accumulated commands.
    /// </summary>
    public void Clear()
    {
        StringBuilder.Clear();
    }

    /// <summary>
    /// Appends text followed by a space to the command script.
    /// </summary>
    /// <param name="text">Text to append.</param>
    public void Append(string text)
    {
        StringBuilder.Append(text + " ");
    }

    /// <summary>
    /// Appends a line of text to the command script.
    /// </summary>
    /// <param name="text">Text to append as a new line.</param>
    public void AppendLine(string text)
    {
        StringBuilder.AppendLine(text);
    }

    /// <summary>
    /// Appends an empty line to the command script.
    /// </summary>
    public void AppendLine()
    {
        StringBuilder.AppendLine();
    }

    /// <summary>
    /// Returns the complete command script as a string.
    /// </summary>
    /// <returns>The command script text.</returns>
    public override string ToString()
    {
        return StringBuilder.ToString();
    }
}