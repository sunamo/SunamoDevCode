namespace Aps.Projs._;

/// <summary>
/// Only commented
/// </summary>
public partial class VsProjectsFileHelper
{
    #region Working direct from XML
    //public static void AddFilesToUnitTestProject(FileInfo[] files, string measureBaseDirPath, string measureDataDirSuffix)
    //{
    //    var unitTestProjectPath = measureBaseDirPath + _unitTestProjectFile;
    //    var unitTestProjectFile = XDocument.Load(unitTestProjectPath);
    //    var itemGroup = unitTestProjectFile.Nodes()
    //                      .OfType<XElement>()
    //                      .DescendantNodes()
    //                      .OfType<XElement>().First(xy => xy.Name.LocalName == "ItemGroup");

    //    foreach (var fileInfo in files)
    //    {
    //        var xelem = AddProjectContent(measureDataDirSuffix + fileInfo.Name, unitTestProjectFile);
    //        itemGroup.Add(xelem);
    //    }
    //    unitTestProjectFile.Save(unitTestProjectPath);
    //}

    //public static void AddFileToUnitTestProject(string pathToAdd, string measureBaseDirPath, string measureDataDir)
    //{
    //    var unitTestProjectPath = measureBaseDirPath + _unitTestProjectFile;
    //    var unitTestProjectFile = XDocument.Load(unitTestProjectPath);
    //    var itemGroup =
    //    unitTestProjectFile.Nodes()
    //                       .OfType<XElement>()
    //                       .DescendantNodes()
    //                       .OfType<XElement>().First(xy => xy.Name.LocalName == "ItemGroup");
    //    var xelem = AddProjectContent(pathToAdd, unitTestProjectFile);
    //    itemGroup.Add(xelem);
    //    unitTestProjectFile.Save(unitTestProjectPath);
    //}

    ///// <summary>
    ///// Add Content - exists many others like Reference(used also in .NET Core, Compile, and so)
    ///// </summary>
    ///// <param name="pathToAdd"></param>
    ///// <param name="doc"></param>
    ///// <returns></returns>
    //private static XElement AddProjectContent(string pathToAdd, XDocument doc, ItemGroups itemGroup)
    //{
    //    /*
    //     * Reference:
    //     * Include - fnwoe dll
    //     * <HintPath> - relative
    //     * 
    //     * Folder:
    //     * Include - relative path
    //     * 
    //     * Content:
    //     * Include = rp
    //     * <CopyToOutputDirectory> - Always,...
    //     */
    //    XNamespace rootNamespace = doc.Root.Name.NamespaceName;
    //    var xelem = new XElement(rootNamespace + itemGroup.ToString());

    //    if (itemGroup == ItemGroups.Content)
    //    {
    //        xelem.Add(new XAttribute("Include", pathToAdd));
    //        xelem.Add(new XElement(rootNamespace + "CopyToOutputDirectory", "Always"));
    //    }
    //    else if (itemGroup == ItemGroups.Compile)
    //    {

    //    }
    //    else if (itemGroup == ItemGroups.Reference)
    //    {
    //        xelem.Add(new XAttribute("Include", pathToAdd));
    //    }

    //    return xelem;
    //} 
    #endregion

}
