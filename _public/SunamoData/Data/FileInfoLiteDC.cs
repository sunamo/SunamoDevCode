namespace SunamoDevCode;


public class FileInfoLiteDC
{
    
    
    
    public string Path = null;
    
    
    
    public string Name = null;
    public string FileName
    {
        get
        {
            return Name;
        }
    }
    public long Size = 0;
    public long Length
    {
        get
        {
            return Size;
        }
    }
    public string Directory = null;
    public FileInfoLiteDC()
    {
    }
    public FileInfoLiteDC(string Directory, string FileName, long Length)
    {
        this.Directory = Directory;
        Name = FileName;
        Size = Length;
    }
    public static FileInfoLiteDC GetFIL(FileInfo item2)
    {
        FileInfoLiteDC fil = new FileInfoLiteDC();
        fil.Name = item2.Name;
        fil.Path = item2.FullName;
        fil.Directory = item2.DirectoryName;
        fil.Size = item2.Length;
        return fil;
    }
    public static FileInfoLiteDC GetFIL(string file)
    {
        FileInfo item2 = new FileInfo(file);
        return GetFIL(item2);
    }
}