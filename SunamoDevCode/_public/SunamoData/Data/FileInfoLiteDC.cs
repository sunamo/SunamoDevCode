// variables names: ok
namespace SunamoDevCode._public.SunamoData.Data;

/// <summary>
/// Lightweight file information class
/// CZ: Odlehčená třída pro informace o souboru
/// </summary>
public class FileInfoLiteDC
{
    /// <summary>
    /// Full path to the file
    /// CZ: Plná cesta k souboru
    /// </summary>
    public string Path { get; set; } = null;

    /// <summary>
    /// Name of the file with extension
    /// CZ: Název souboru s příponou
    /// </summary>
    public string Name { get; set; } = null;

    /// <summary>
    /// Gets the file name (alias for Name)
    /// CZ: Získá název souboru (alias pro Name)
    /// </summary>
    public string FileName
    {
        get
        {
            return Name;
        }
    }

    /// <summary>
    /// Size of the file in bytes
    /// CZ: Velikost souboru v bajtech
    /// </summary>
    public long Size { get; set; } = 0;

    /// <summary>
    /// Gets the file length (alias for Size)
    /// CZ: Získá délku souboru (alias pro Size)
    /// </summary>
    public long Length
    {
        get
        {
            return Size;
        }
    }

    /// <summary>
    /// Directory containing the file
    /// CZ: Adresář obsahující soubor
    /// </summary>
    public string Directory { get; set; } = null;

    /// <summary>
    /// Initializes a new instance of FileInfoLiteDC
    /// CZ: Inicializuje novou instanci FileInfoLiteDC
    /// </summary>
    public FileInfoLiteDC()
    {
    }

    /// <summary>
    /// Initializes a new instance of FileInfoLiteDC with specified values
    /// CZ: Inicializuje novou instanci FileInfoLiteDC se zadanými hodnotami
    /// </summary>
    /// <param name="directory">Directory containing the file</param>
    /// <param name="fileName">Name of the file</param>
    /// <param name="length">Size of the file in bytes</param>
    public FileInfoLiteDC(string directory, string fileName, long length)
    {
        this.Directory = directory;
        Name = fileName;
        Size = length;
    }

    /// <summary>
    /// Creates FileInfoLiteDC from FileInfo instance
    /// CZ: Vytvoří FileInfoLiteDC z instance FileInfo
    /// </summary>
    /// <param name="fileInfo">FileInfo instance to convert</param>
    /// <returns>FileInfoLiteDC instance</returns>
    public static FileInfoLiteDC GetFIL(FileInfo fileInfo)
    {
        FileInfoLiteDC fileInfoLite = new FileInfoLiteDC();
        fileInfoLite.Name = fileInfo.Name;
        fileInfoLite.Path = fileInfo.FullName;
        fileInfoLite.Directory = fileInfo.DirectoryName;
        fileInfoLite.Size = fileInfo.Length;
        return fileInfoLite;
    }

    /// <summary>
    /// Creates FileInfoLiteDC from file path
    /// CZ: Vytvoří FileInfoLiteDC z cesty k souboru
    /// </summary>
    /// <param name="filePath">Path to the file</param>
    /// <returns>FileInfoLiteDC instance</returns>
    public static FileInfoLiteDC GetFIL(string filePath)
    {
        FileInfo fileInfo = new FileInfo(filePath);
        return GetFIL(fileInfo);
    }
}