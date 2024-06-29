namespace SunamoDevCode;


public class FileInfoLiteDC
{
    /// <summary>
    /// Plná cesta k souboru
    /// </summary>
    internal string Path = null;
    /// <summary>
    /// Název souboru bez cesty s příponou a sériemi
    /// </summary>
    internal string Name = null;
    internal string FileName
    {
        get
        {
            return Name;
        }
    }
    internal long Size = 0;
    internal long Length
    {
        get
        {
            return Size;
        }
    }
    internal string Directory = null;
    internal FileInfoLiteDC()
    {
    }
    internal FileInfoLiteDC(string Directory, string FileName, long Length)
    {
        this.Directory = Directory;
        Name = FileName;
        Size = Length;
    }
    internal static FileInfoLiteDC GetFIL(FileInfo item2)
    {
        FileInfoLiteDC fil = new FileInfoLiteDC();
        fil.Name = item2.Name;
        fil.Path = item2.FullName;
        fil.Directory = item2.DirectoryName;
        fil.Size = item2.Length;
        return fil;
    }
    internal static FileInfoLiteDC GetFIL(string file)
    {
        FileInfo item2 = new FileInfo(file);
        return GetFIL(item2);
    }
}