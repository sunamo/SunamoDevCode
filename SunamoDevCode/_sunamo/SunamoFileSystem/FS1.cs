namespace SunamoDevCode._sunamo.SunamoFileSystem;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
internal partial class FS
{
    internal static bool CopyMoveFilePrepare(ref string item, ref string fileTo, FileMoveCollisionOptionDC co)
    {
        //var fileTo = fileTo2.ToString();
        item = @"\\?\" + item;
        MakeUncLongPath(ref fileTo);
        CreateUpfoldersPsysicallyUnlessThere(fileTo);
        // Toto tu je důležité, nevím který kokot to zakomentoval
        if (File.Exists(fileTo))
        {
            if (co == FileMoveCollisionOptionDC.AddFileSize)
            {
                var newFn = InsertBetweenFileNameAndExtension(fileTo, " " + new FileInfo(item).Length);
                if (File.Exists(newFn))
                {
                    File.Delete(item);
                    return true;
                }

                fileTo = newFn;
            }
            else if (co == FileMoveCollisionOptionDC.AddSerie)
            {
                var serie = 1;
                while (true)
                {
                    var newFn = InsertBetweenFileNameAndExtension(fileTo, " (" + serie + ")");
                    if (!File.Exists(newFn))
                    {
                        fileTo = newFn;
                        break;
                    }

                    serie++;
                }
            }
            else if (co == FileMoveCollisionOptionDC.DiscardFrom)
            {
                // Cant delete from because then is file deleting
                if (DeleteFileMaybeLocked != null)
                    DeleteFileMaybeLocked(item);
                else
                    File.Delete(item);
            }
            else if (co == FileMoveCollisionOptionDC.Overwrite)
            {
                if (DeleteFileMaybeLocked != null)
                    DeleteFileMaybeLocked(fileTo);
                else
                    File.Delete(fileTo);
            }
            else if (co == FileMoveCollisionOptionDC.LeaveLarger)
            {
                var fsFrom = new FileInfo(item).Length;
                var fsTo = new FileInfo(fileTo).Length;
                if (fsFrom > fsTo)
                    File.Delete(fileTo);
                else //if (fsFrom < fsTo)
                    File.Delete(item);
            }
            else if (co == FileMoveCollisionOptionDC.DontManipulate)
            {
                if (File.Exists(fileTo))
                    return false;
            }
            else if (co == FileMoveCollisionOptionDC.ThrowEx)
            {
                ThrowEx.Custom($"Directory {fileTo} already exists");
            }
        }

        return true;
    }

    internal static Action<string> DeleteFileMaybeLocked;
    internal static void MoveFile(string item, string fileTo, FileMoveCollisionOptionDC co)
    {
        if (CopyMoveFilePrepare(ref item, ref fileTo, co))
            try
            {
                item = MakeUncLongPath(item);
                fileTo = MakeUncLongPath(fileTo);
                if (co == FileMoveCollisionOptionDC.DontManipulate && File.Exists(fileTo))
                    return;
                File.Move(item, fileTo);
            }
            catch (Exception ex)
            {
            //ThisApp.Error(item + " : " + ex.Message);
            }
    }

    internal static Func<string, bool, List<Process>> fileUtilWhoIsLocking = null;
    internal static void CopyFile(string jsFiles, string v, bool terminateProcessIfIsInUsed = false)
    {
        try
        {
            File.Copy(jsFiles, v, true);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("because it is being used by another process") && terminateProcessIfIsInUsed)
            {
                if (fileUtilWhoIsLocking != null)
                {
                    var pr = fileUtilWhoIsLocking(jsFiles, true);
                    foreach (var item in pr)
                        item.Kill();
                }

                // Používá se i ve web, musel bych tam includovat spoustu metod
                //PH.ShutdownProcessWhichOccupyFileHandleExe(jsFiles);
                try
                {
                    File.Copy(jsFiles, v, true);
                }
                catch (Exception)
                {
                // Pokud se to ani na druhý pokus nepodaří, tak už to jebu
                }
            }
            else
            {
                throw;
            }
        }
    }

    private static void MoveOrCopy(string parameter, string to, FileMoveCollisionOptionDC co, bool move, string item)
    {
        var fileTo = to + item.Substring(parameter.Length);
        if (move)
            MoveFile(item, fileTo, co);
        else
            CopyFile(item, fileTo, co);
    }

    internal static void CopyFile(string item, string fileTo2, FileMoveCollisionOptionDC co)
    {
        var fileTo = fileTo2;
        if (CopyMoveFilePrepare(ref item, ref fileTo, co))
        {
            if (co == FileMoveCollisionOptionDC.DontManipulate && File.Exists(fileTo))
                return;
            File.Copy(item, fileTo);
        }
    }

    private static void CopyMoveAllFilesRecursively(ILogger logger, string parameter, string to, FileMoveCollisionOptionDC co, bool move, string mustContains, SearchOption so)
    {
        var files = FSGetFiles.GetFiles(logger, parameter, "*", so);
        if (!string.IsNullOrEmpty(mustContains))
        {
            foreach (var item in files)
                if (SH.IsContained(item, mustContains))
                {
                    MoveOrCopy(parameter, to, co, move, item);
                }
        }
        else
        {
            foreach (var item in files)
                MoveOrCopy(parameter, to, co, move, item);
        }
    }

    internal static void MoveAllRecursivelyAndThenDirectory(ILogger logger, string parameter, string to, FileMoveCollisionOptionDC co)
    {
        CopyMoveAllFilesRecursively(logger, parameter, to, co, true, null, SearchOption.TopDirectoryOnly);
        var dirs = Directory.GetDirectories(parameter, "*", SearchOption.AllDirectories);
        for (var i = dirs.Length - 1; i >= 0; i--)
            TryDeleteDirectory(dirs[i]);
        TryDeleteDirectory(parameter);
    }

    internal static Dictionary<string, List<string>> GetDictionaryByExtension(ILogger logger, string folder, string mask, SearchOption searchOption)
    {
        var extDict = new Dictionary<string, List<string>>();
        foreach (var item in FSGetFiles.GetFiles(logger, folder, mask, searchOption))
        {
            var ext = Path.GetExtension(item);
            var fn = Path.GetFileNameWithoutExtension(item).ToLower();
            if (fn == string.Empty)
            {
                fn = ext;
                ext = "";
            }

            DictionaryHelper.AddOrCreate(extDict, ext, fn);
        }

        return extDict;
    }

    internal static string AddUpfoldersToRelativePath(int i2, string file, char delimiter)
    {
        var jumpUp = ".." + delimiter;
        var stringBuilder = new StringBuilder();
        for (var i = 0; i < i2; i++)
            stringBuilder.Append(jumpUp);
        stringBuilder.Append(file);
        return stringBuilder.ToString();
    //return SHJoin.JoinTimes(i, jumpUp) + file;
    }

    internal static string MascFromExtension(string ext2 = "*")
    {
        if (char.IsLetterOrDigit(ext2[0]))
        {
            // For wildcard, dot (simply non letters) include .
            ext2 = "." + ext2;
        }

        if (!ext2.StartsWith("*"))
        {
            ext2 = "*" + ext2;
        }

        if (!ext2.StartsWith("*.") && ext2.StartsWith("."))
        {
            ext2 = "*." + ext2;
        }

        return ext2;
    }

    internal static void CreateUpfoldersPsysicallyUnlessThere(string nad)
    {
        CreateFoldersPsysicallyUnlessThere(Path.GetDirectoryName(nad));
    }

    internal static void CreateFoldersPsysicallyUnlessThere(string nad)
    {
        ThrowEx.IsNullOrEmpty("nad", nad);
        //ThrowEx.IsNotWindowsPathFormat("nad", nad);
        if (Directory.Exists(nad))
        {
            return;
        }

        List<string> slozkyKVytvoreni = new List<string>
        {
            nad
        };
        while (true)
        {
            nad = Path.GetDirectoryName(nad);
            if (Directory.Exists(nad))
            {
                break;
            }

            string kopia = nad;
            slozkyKVytvoreni.Add(kopia);
        }

        slozkyKVytvoreni.Reverse();
        foreach (string item in slozkyKVytvoreni)
        {
            string folder = item;
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }
    }
}