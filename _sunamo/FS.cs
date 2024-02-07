using SunamoArgs;

namespace SunamoDevCode._sunamo;
internal class FS
{
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

    internal static void CreateUpfoldersPsysicallyUnlessThere(string nad)
    {
        CreateFoldersPsysicallyUnlessThere(Path.GetDirectoryName(nad));
    }

    internal static void CreateFoldersPsysicallyUnlessThere(string nad)
    {
        ThrowEx.IsNullOrEmpty("nad", nad);
        ThrowEx.IsNotWindowsPathFormat("nad", nad);


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

            // TODO: Tady to nefunguje pro UWP/UAP apps protoze nemaji pristup k celemu disku. Zjistit co to je UWP/UAP/... a jak v nem ziskat/overit jakoukoliv slozku na disku
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
            v = v.TrimEnd(AllCharsSE.bs) + AllCharsSE.bs;
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

        if (origS.Contains(AllCharsSE.slash) || origS.Contains(AllCharsSE.bs))
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

    internal static List<string> GetFiles(string projectFolder, string v, SearchOption topDirectoryOnly, GetFilesArgs getFilesArgs = null)
    {
        //throw new NotImplementedException();
        return Directory.GetFiles(projectFolder, v, topDirectoryOnly).ToList();
    }
}
