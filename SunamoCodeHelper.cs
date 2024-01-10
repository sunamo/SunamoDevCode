namespace SunamoDevCode;


public partial class SunamoDevCodeHelper
{
    public static void CopySolution(string slnFolder, string folderTo, Action<string> archive)
    {
        var l = FS.GetFiles(slnFolder, true);
        RemoveTemporaryFilesVS(l);
        RemoveGitFiles(l);

        var b = FS.GetDirectoryName(slnFolder);
        FS.WithEndSlash(ref b);
        FS.WithEndSlash(ref folderTo);

        var slnFolderTo = Path.Combine(folderTo, FS.GetFileName(slnFolder));
        FS.TryDeleteDirectory(slnFolderTo);

        foreach (var item in l)
        {
            var np = item.Replace(b, folderTo);
            FS.CreateUpfoldersPsysicallyUnlessThere(np);
            FS.CopyFile(item, np, FileMoveCollisionOption.DontManipulate);
        }

        archive(slnFolderTo);
        ThisApp.Info("Archive was created successfully, is important create archive because first open with VS because will create folders package,obj,bin");
    }

    public static void RemoveGitFiles(List<string> files, bool alsoGitFiles = true, bool alsoDownloadedFolders = false, bool alsoFoldersToDelete = false)
    {
        string wr = null;

        if (!alsoGitFiles)
        {
            wr = SH.WrapWith(VisualStudioTempFse.gitFolderName, AllStrings.bs);
            files.RemoveAll(d => d.Contains(wr));
        }

        if (!alsoDownloadedFolders)
        {
            foreach (var item in VisualStudioTempFse.foldersInSolutionDownloaded)
            {
                wr = SH.WrapWithBs(item);
                files.RemoveAll(d => d.Contains(wr));
            }
        }

        if (!alsoFoldersToDelete)
        {
            foreach (var item in VisualStudioTempFse.foldersInSolutionToDelete)
            {
                wr = SH.WrapWithBs(item);
                files.RemoveAll(d => d.Contains(wr));
            }
        }
    }

    public static void RemoveTemporaryFilesVS(List<string> files)
    {
        var list = VisualStudioTempFseWrapped.foldersInSolutionToDelete;

        // todo list je zde List<string>, chce jen string, později to analyzovat 

        //As foldersInProjectToDelete dont have contains WildCard, set false
        CA.RemoveWhichContainsList(files, list, false);
        list = VisualStudioTempFseWrapped.foldersInProjectToDelete;
        CA.RemoveWhichContainsList(files, list, false);
        list = VisualStudioTempFseWrapped.foldersAnywhereToDelete;
        CA.RemoveWhichContainsList(files, list, false);

        list = VisualStudioTempFseWrapped.foldersInSolutionDownloaded;
        CA.RemoveWhichContainsList(files, list, false);
        list = VisualStudioTempFseWrapped.foldersInProjectDownloaded;
        CA.RemoveWhichContainsList(files, list, false);
        list = VisualStudioTempFseWrapped.foldersAnywhereDownloaded;
        CA.RemoveWhichContainsList(files, list, false);

    }

    private static bool IsNameOfHtmlAttrValue(string between)
    {
        return AllHtmlAttrsValues.list.Contains(between.Trim());
    }

    private static bool IsNameOfHtmlAttr(string between)
    {
        return AllHtmlAttrs.list.Contains(between.Trim());
    }

    public static bool IsNameOfHtmlTag(string between, bool add)
    {
        string element = null;
        var startWithTag = CA.StartWith(AllHtmlTags.list, between, out element);
        startWithTag = element;
        if (startWithTag != null)
        {
            if (startWithTag == between)
            {
                add = true;
            }
            else
            {
                var remain = between.Substring(startWithTag.Length);
                add = BTS.IsInt(remain);
            }
        }

        return add;
    }

    /// <summary>
    /// A1 normal, not lower
    /// </summary>
    /// <param name = "between"></param>
    public static bool IsNameOfControl(string between)
    {
        var add = false;
        add = IsNameOfHtmlTag(between, add);
        if (!add)
        {
            add = IsNameOfHtmlAttr(between);
        }

        if (!add)
        {
            add = IsNameOfHtmlAttrValue(between);
        }

        if (!add)
        {
            int firstInt = -1;
            var i = 0;
            foreach (var item in between)
            {
                if (char.IsLower(item))
                {
                    if (firstInt != -1)
                    {
                        add = false;
                        break;
                    }
                }
                else if (char.IsNumber(item))
                {
                    if (firstInt == -1)
                    {
                        firstInt = i;
                    }
                }
                else
                {
                    add = false;
                    break;
                }

                i++;
            }

            string prefix = between;
            if (firstInt != -1)
            {
                prefix = between.Substring(0, firstInt);
            }

            add = SystemWindowsControls.IsShortcutOfControl(prefix);
        }

        return add;
    }
}
