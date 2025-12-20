// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode._sunamo.SunamoFileSystem;
internal partial class FS
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
        return !String.IsNullOrWhiteSpace(path) && path.IndexOfAny(System.IO.Path.GetInvalidPathChars()) == -1 && Path.IsPathRooted(path) && !Path.GetPathRoot(path).Equals(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal);
    }

    internal static string GetAbsolutePath2(string relativePath, string dir)
    {
        var ap = GetAbsolutePath(dir, relativePath);
        return Path.GetFullPath(ap);
    }

    internal static void FileToDirectory(ref string dir)
    {
        if (!dir.EndsWith("\""))
            dir = GetDirectoryName(dir);
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

    internal static bool IsWindowsPathFormat(string argValue)
    {
        if (string.IsNullOrWhiteSpace(argValue))
            return false;
        var badFormat = false;
        if (argValue.Length < 3)
            return badFormat;
        if (!char.IsLetter(argValue[0]))
            badFormat = true;
        if (char.IsLetter(argValue[1]))
            badFormat = true;
        if (argValue.Length > 2)
            if (argValue[1] != '\\' && argValue[2] != '\\')
                badFormat = true;
        return !badFormat;
    }

    internal static string FirstCharUpper(ref string result)
    {
        if (IsWindowsPathFormat(result))
            result = SH.FirstCharUpper(result);
        return result;
    }

    internal static string FirstCharUpper(string nazevPP, bool only = false)
    {
        if (nazevPP != null)
        {
            var substring = nazevPP.Substring(1);
            if (only)
                substring = substring.ToLower();
            return nazevPP[0].ToString().ToUpper() + substring;
        }

        return null;
    }

    internal static string GetFullPath(string vr)
    {
        var result = Path.GetFullPath(vr);
        FirstCharUpper(ref result);
        return result;
    }

    private static string CombineWorker(bool FirstCharUpper, bool file, params string[] s)
    {
        for (var i = 0; i < s.Length; i++)
            s[i] = s[i].TrimStart('\\');
        //s = CA.TrimStartChar('\\', s.ToList()).ToArray();
        var result = Path.Combine(s);
        if (result[2] != '\\')
            result = result.Insert(2, "\"");
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
        else if (relativePath.Contains("/"))
            deli = "/";
        else
        {
            ThrowEx.NotImplementedCase(relativePath);
        }

        return SHSplit.Split(relativePath, deli);
    }

    internal static List<string> OnlyNamesWithoutExtensionCopy(List<string> p2)
    {
        var parameter = new List<string>(p2.Count);
        //CA.InitFillWith(parameter, p2.Count);
        for (var i = 0; i < p2.Count; i++)
            parameter[i] = Path.GetFileNameWithoutExtension(p2[i]);
        return parameter;
    }

    internal static string ReplaceDirectoryThrowExceptionIfFromDoesntExists(string parameter, string folderWithProjectsFolders, string folderWithTemporaryMovedContentWithoutBackslash)
    {
        parameter = SH.FirstCharUpper(parameter);
        folderWithProjectsFolders = SH.FirstCharUpper(folderWithProjectsFolders);
        folderWithTemporaryMovedContentWithoutBackslash = SH.FirstCharUpper(folderWithTemporaryMovedContentWithoutBackslash);
        if (!ThrowEx.NotContains(parameter, folderWithProjectsFolders))
            // Here can never accomplish when exc was throwed
            return parameter;
        // Here can never accomplish when exc was throwed
        return parameter.Replace(folderWithProjectsFolders, folderWithTemporaryMovedContentWithoutBackslash);
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
}