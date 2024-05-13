

namespace SunamoDevCode;

public class SolutionFolderWithFiles : SolutionFolder
{
    public List<string> files = null;
    /// <summary>
    /// Without dot
    /// </summary>
    public Dictionary<string, List<string>> filesOfExtension = null;
    #region Filled in CheckSize()
    public Dictionary<int, long> filesAndSizes = null;
    public Dictionary<TypeOfExtensionDevCode, long> sizeOfExtensionTypes = null;
    /// <summary>
    /// All extensions is lower and without dot
    /// </summary>
    public Dictionary<string, long> sizeOfExtension = null;
    public long overallSize = 0;
    #endregion
    /// <summary>
    /// Is filled in method CreateFileInfoLiteObjects
    /// </summary>
    public Dictionary<string, List<FileInfoLite>> fileInfoLiteOfExtension = null;


    public SolutionFolderWithFiles(SolutionFolder sf)
    {
        countOfImages = sf.countOfImages;
        displayedText = sf.displayedText;
        fullPathFolder = sf.fullPathFolder;
        nameSolutionWithoutDiacritic = sf.nameSolutionWithoutDiacritic;

        filesAndSizes = new Dictionary<int, long>();
        sizeOfExtensionTypes = new Dictionary<TypeOfExtensionDevCode, long>();
        sizeOfExtension = new Dictionary<string, long>();

        files = Directory.GetFiles(fullPathFolder, AllStrings.asterisk, SearchOption.AllDirectories).ToList();
        filesOfExtension = new Dictionary<string, List<string>>();

        for (int i = 0; i < files.Count; i++)
        {
            var item = files[i];
            string ext = Path.GetExtension(item).TrimStart(AllChars.dot);
            DictionaryHelper.AddOrCreate(filesOfExtension, ext, item);
        }


    }

    public void CheckSize()
    {
        //for (int i = 0; i < files.Count; i++)
        //{
        //    var item = files[i];
        //    long fs = new FileInfo(item).Length;
        //    overallSize += fs;
        //    filesAndSizes.Add(i, fs);

        //    string ext = Path.GetExtension(item).TrimStart(AllChars.dot);
        //    TypeOfExtension extType = AllExtensionsHelper.FindTypeWithDot(ext);

        //    if (!sizeOfExtensionTypes.ContainsKey(extType))
        //    {
        //        sizeOfExtensionTypes.Add(extType, fs);
        //    }
        //    else
        //    {
        //        sizeOfExtensionTypes[extType] += fs;
        //    }

        //    if (!sizeOfExtension.ContainsKey(ext))
        //    {
        //        sizeOfExtension.Add(ext, fs);
        //    }
        //    else
        //    {
        //        sizeOfExtension[ext] += fs;
        //    }
        //}

        //displayedText += " (" + FS.GetSizeInAutoString(overallSize, ComputerSizeUnits.MB) + AllStrings.rb;
    }

    public void CreateFileInfoLiteObjects(string extensionWithoutDot, string item)
    {
        var fil = FileInfoLite.GetFIL(item);
        if (fileInfoLiteOfExtension.ContainsKey(extensionWithoutDot))
        {
            fileInfoLiteOfExtension[extensionWithoutDot].Add(fil);
        }
        else
        {
            List<FileInfoLite> l = new List<FileInfoLite>();
            l.Add(fil);
            fileInfoLiteOfExtension.Add(extensionWithoutDot, l);
        }
    }
}
