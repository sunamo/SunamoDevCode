namespace SunamoDevCode._sunamo.SunamoFileSystem;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
internal partial class FS
{
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

    internal static bool TryDeleteDirectory(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            return true;
        }

        try
        {
            Directory.Delete(directoryPath, true);
            return true;
        }
        catch (Exception ex)
        {
        // EN: It's try so don't know what this is doing here
        // CZ: Je to try takže nevím co tu dělá tohle
        }

        var files = GetFiles(directoryPath, "*", SearchOption.AllDirectories);
        foreach (var filePath in files)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
        }

        try
        {
            Directory.Delete(directoryPath, true);
            return true;
        }
        catch (Exception ex)
        {
        }

        return false;
    }

    internal static string WithEndSlash(string path)
    {
        return WithEndSlash(ref path);
    }

    /// <summary>
    ///     Usage: Exceptions.FileWasntFoundInDirectory
    /// </summary>
    /// <param name = "v"></param>
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
            string parameter = Path.GetDirectoryName(origS);
            return Path.Combine(parameter, fn + whatInsert + e);
        }

        return fn + whatInsert + e;
    }

    /// <summary>
    /// No direct edit
    /// </summary>
    /// <param name = "files2"></param>
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
    /// <param name = "files"></param>
    internal static List<string> OnlyNamesNoDirectEdit(List<string> files2)
    {
        List<string> files = new List<string>(files2.Count);
        for (int i = 0; i < files2.Count; i++)
        {
            files.Add(Path.GetFileName(files2[i]));
        }

        return files;
    }

    internal static List<string> GetFiles(string projectFolder, string v, SearchOption topDirectoryOnly /*, GetFilesArgsDC getFilesArgs = null*/)
    {
        //ThrowEx.NotImplementedMethod();
        return Directory.GetFiles(projectFolder, v, topDirectoryOnly).ToList();
    }

    internal static string GetDirectoryName(string csp)
    {
        var data = Path.GetDirectoryName(csp);
        return FS.WithEndSlash(data);
    }

    internal static string GetFileName(string fullPathFolder)
    {
        return FS.GetFileName(fullPathFolder);
    }

    internal static bool ExistsFile(string r)
    {
        return File.Exists(r);
    }

    internal static bool ExistsDirectory(string parameter)
    {
        return Directory.Exists(parameter);
    }
}