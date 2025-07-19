namespace SunamoDevCode._sunamo.SunamoFileSystem;

internal class FS
{



    internal static void DeleteFoldersWhichNotContains(string v, string folder, IList<string> v2)
    {
        var f = Directory.GetDirectories(v, folder, SearchOption.AllDirectories).ToList();
        for (int i = f.Count - 1; i >= 0; i--)
        {
            if (CA.ReturnWhichContainsIndexes(f[i], v2).Count != 0)
            {
                f.RemoveAt(i);
            }
        }
        foreach (var item in f)
        {
            //FS.DeleteF
        }
    }

    internal static bool IsAbsolutePath(string path)
    {
        return !String.IsNullOrWhiteSpace(path)
            && path.IndexOfAny(System.IO.Path.GetInvalidPathChars()) == -1
            && Path.IsPathRooted(path)
            && !Path.GetPathRoot(path).Equals(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal);
    }

    internal static string GetAbsolutePath2(string relativePath, string dir)
    {
        var ap = GetAbsolutePath(dir, relativePath);
        return Path.GetFullPath(ap);
    }

    internal static void FileToDirectory(ref string dir)
    {
        if (!dir.EndsWith("\"")) dir = GetDirectoryName(dir);
    }

    internal static string GetAbsolutePath(string dir, string relativePath)
    {


        FileToDirectory(ref dir);

        var ds = "./";
        var dds = "../";

        var dds2 = 0;
        while (true)
            if (relativePath.StartsWith(ds))
            {
                relativePath = relativePath.Substring(ds.Length);
            }
            else if (relativePath.StartsWith(dds))
            {
                dds2++;
                relativePath = relativePath.Substring(dds.Length);
            }
            else
            {
                break;
            }

        var tokens = GetTokens(relativePath);
        tokens = tokens.Skip(dds2).ToList();
        tokens.Insert(0, dir);

        var vr = Combine(tokens.ToArray());
        vr = GetFullPath(vr);
        return vr;
    }

    #region FirstCharUpper

    internal static bool IsWindowsPathFormat(string argValue)
    {
        if (string.IsNullOrWhiteSpace(argValue)) return false;

        var badFormat = false;

        if (argValue.Length < 3) return badFormat;

        if (!char.IsLetter(argValue[0])) badFormat = true;

        if (char.IsLetter(argValue[1])) badFormat = true;

        if (argValue.Length > 2)
            if (argValue[1] != '\\' && argValue[2] != '\\')
                badFormat = true;

        return !badFormat;
    }



    internal static string FirstCharUpper(ref string result)
    {
        if (IsWindowsPathFormat(result)) result = SH.FirstCharUpper(result);

        return result;
    }

    internal static string FirstCharUpper(string nazevPP, bool only = false)
    {
        if (nazevPP != null)
        {
            var sb = nazevPP.Substring(1);
            if (only) sb = sb.ToLower();

            return nazevPP[0].ToString().ToUpper() + sb;
        }

        return null;
    }

    #endregion

    internal static string GetFullPath(string vr)
    {
        var result = Path.GetFullPath(vr);
        FirstCharUpper(ref result);
        return result;
    }

    private static string CombineWorker(bool FirstCharUpper, bool file, params string[] s)
    {
        for (var i = 0; i < s.Length; i++) s[i] = s[i].TrimStart('\\');
        //s = CA.TrimStartChar('\\', s.ToList()).ToArray();
        var result = Path.Combine(s);
        if (result[2] != '\\') result = result.Insert(2, "\"");
        if (FirstCharUpper)
            result = SH.FirstCharUpper(ref result);
        else
            result = SH.FirstCharUpper(ref result);
        if (!file)
            // Cant return with end slash becuase is working also with files
            WithEndSlash(ref result);
        return result;
    }

    internal static string Combine(params string[] s)
    {
        //return Path.Combine(s);
        return CombineWorker(true, false, s);
    }

    internal static List<string> GetTokens(string relativePath)
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

    internal static List<string> OnlyNamesWithoutExtensionCopy(List<string> p2)
    {
        var p = new List<string>(p2.Count);
        //CA.InitFillWith(p, p2.Count);
        for (var i = 0; i < p2.Count; i++) p[i] = Path.GetFileNameWithoutExtension(p2[i]);
        return p;
    }
    internal static string ReplaceDirectoryThrowExceptionIfFromDoesntExists(string p, string folderWithProjectsFolders,
        string folderWithTemporaryMovedContentWithoutBackslash)
    {
        p = SH.FirstCharUpper(p);
        folderWithProjectsFolders = SH.FirstCharUpper(folderWithProjectsFolders);
        folderWithTemporaryMovedContentWithoutBackslash =
            SH.FirstCharUpper(folderWithTemporaryMovedContentWithoutBackslash);

        if (!ThrowEx.NotContains(p, folderWithProjectsFolders))
            // Here can never accomplish when exc was throwed
            return p;

        // Here can never accomplish when exc was throwed
        return p.Replace(folderWithProjectsFolders, folderWithTemporaryMovedContentWithoutBackslash);
    }


    internal static string MakeUncLongPath(ref string path)
    {
        if (!path.StartsWith(@"\\?\"))
        {
            // V ASP.net mi vrátilo u každé directory.exists false. Byl jsem pod ApplicationPoolIdentity v IIS a bylo nastaveno Full Control pro IIS AppPool\DefaultAppPool
#if !ASPNET
            //  asp.net / vps nefunguje, ve windows store apps taktéž, NECHAT TO TRVALE ZAKOMENTOVANÉ
            // v asp.net toto způsobí akorát zacyklení, IIS začne vyhazovat 0xc00000fd, pak už nejde načíst jediná stránka
            //path = @"\\?\" + path;
#endif
        }

        return path;
    }
    internal static string MakeUncLongPath(string path)
    {
        return MakeUncLongPath(ref path);
    }

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
                if (File.Exists(fileTo)) return false;
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

                if (co == FileMoveCollisionOptionDC.DontManipulate && File.Exists(fileTo)) return;
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
                    foreach (var item in pr) item.Kill();
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

    private static void MoveOrCopy(string p, string to, FileMoveCollisionOptionDC co, bool move, string item)
    {
        var fileTo = to + item.Substring(p.Length);
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
            if (co == FileMoveCollisionOptionDC.DontManipulate && File.Exists(fileTo)) return;

            File.Copy(item, fileTo);
        }
    }


    private static void CopyMoveAllFilesRecursively(ILogger logger, string p, string to, FileMoveCollisionOptionDC co, bool move,
        string mustContains, SearchOption so)
    {
        var files = FSGetFiles.GetFiles(logger, p, "*", so);
        if (!string.IsNullOrEmpty(mustContains))
        {
            foreach (var item in files)
                if (SH.IsContained(item, mustContains))
                {

                    MoveOrCopy(p, to, co, move, item);
                }
        }
        else
        {
            foreach (var item in files) MoveOrCopy(p, to, co, move, item);
        }
    }

    internal static void MoveAllRecursivelyAndThenDirectory(ILogger logger, string p, string to, FileMoveCollisionOptionDC co)
    {
        CopyMoveAllFilesRecursively(logger, p, to, co, true, null, SearchOption.TopDirectoryOnly);
        var dirs = Directory.GetDirectories(p, "*", SearchOption.AllDirectories);
        for (var i = dirs.Length - 1; i >= 0; i--) TryDeleteDirectory(dirs[i]);
        TryDeleteDirectory(p);
    }
    internal static Dictionary<string, List<string>> GetDictionaryByExtension(ILogger logger, string folder, string mask,
        SearchOption searchOption)
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
        var sb = new StringBuilder();
        for (var i = 0; i < i2; i++) sb.Append(jumpUp);
        sb.Append(file);
        return sb.ToString();
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

    internal static string Slash(string path, bool slash)
    {
        string result = null;
        if (slash)
        {
            result = path.Replace("\"", "/"); //SHReplace.ReplaceAll2(path, "/", "\"");
        }
        else
        {
            result = path.Replace("/", "\""); //SHReplace.ReplaceAll2(path, "\"", "/");
        }

        SH.FirstCharUpper(ref result);
        return result;
    }
    internal static Dictionary<string, List<string>> GetDictionaryByFileNameWithExtension(List<string> files)
    {
        Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
        foreach (var item in files)
        {
            string filename = Path.GetFileName(item);
            DictionaryHelper.AddOrCreateIfDontExists<string, string>(result, filename, item);
        }

        return result;
    }

    internal static string AddExtensionIfDontHave(string file, string ext)
    {
        // For *.* and git paths {dir}/*
        if (file[file.Length - 1] == '*')
        {
            return file;
        }
        if (Path.GetExtension(file) == string.Empty)
        {
            return file + ext;
        }

        return file;
    }

    internal static bool TryDeleteDirectory(string v)
    {
        if (!Directory.Exists(v))
        {
            return true;
        }

        try
        {
            Directory.Delete(v, true);
            return true;
        }
        catch (Exception ex)
        {

            // Je to try takže nevím co tu dělá tohle a
            //ThrowEx.FolderCannotBeDeleted(v, ex);
            //var result = InvokePs(v);
            //if (result.Count > 0)
            //{
            //    return false;
            //}
        }

        var files = GetFiles(v, "*", SearchOption.AllDirectories);
        foreach (var item in files)
        {
            File.SetAttributes(item, FileAttributes.Normal);
        }

        try
        {
            Directory.Delete(v, true);
            return true;
        }
        catch (Exception ex)
        {
        }

        return false;
    }

    internal static string WithEndSlash(string v)
    {
        return WithEndSlash(ref v);
    }

    /// <summary>
    ///     Usage: Exceptions.FileWasntFoundInDirectory
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    internal static string WithEndSlash(ref string v)
    {
        if (v != string.Empty)
        {
            v = v.TrimEnd('\\') + '\\';
        }

        SH.FirstCharUpper(ref v);
        return v;
    }



    internal static string InsertBetweenFileNameAndExtension(string orig, string whatInsert)
    {
        //return InsertBetweenFileNameAndExtension<string, string>(orig, whatInsert, null);

        // Cesta by se zde hodila kvůli FS.CiStorageFile
        // nicméně StorageFolder nevím zda se používá, takže to bude umět i bez toho

        var origS = orig.ToString();

        string fn = Path.GetFileNameWithoutExtension(origS);
        string e = Path.GetExtension(origS);

        if (origS.Contains('/') || origS.Contains('\\'))
        {
            string p = Path.GetDirectoryName(origS);

            return Path.Combine(p, fn + whatInsert + e);
        }
        return fn + whatInsert + e;
    }

    /// <summary>
    /// No direct edit
    /// </summary>
    /// <param name="files2"></param>
    /// <returns></returns>
    internal static List<string> OnlyNamesNoDirectEdit(String[] files2)
    {
        var tl = files2.ToList();
        return OnlyNamesNoDirectEdit(tl);
    }

    /// <summary>
    /// No direct edit
    /// Returns with extension
    /// POZOR: Na rozdíl od stejné metody v sunamo tato metoda vrací úplně nové pole a nemodifikuje A1
    /// </summary>
    /// <param name="files"></param>
    internal static List<string> OnlyNamesNoDirectEdit(List<string> files2)
    {
        List<string> files = new List<string>(files2.Count);
        for (int i = 0; i < files2.Count; i++)
        {
            files.Add(Path.GetFileName(files2[i]));
        }
        return files;
    }

    internal static List<string> GetFiles(string projectFolder, string v, SearchOption topDirectoryOnly/*, GetFilesArgsDC getFilesArgs = null*/)
    {
        //ThrowEx.NotImplementedMethod();
        return Directory.GetFiles(projectFolder, v, topDirectoryOnly).ToList();
    }

    internal static string GetDirectoryName(string csp)
    {
        var d = Path.GetDirectoryName(csp);
        return FS.WithEndSlash(d);
    }

    internal static string GetFileName(string fullPathFolder)
    {
        return FS.GetFileName(fullPathFolder);
    }

    internal static bool ExistsFile(string r)
    {
        return File.Exists(r);
    }

    internal static bool ExistsDirectory(string p)
    {
        return Directory.Exists(p);
    }
}