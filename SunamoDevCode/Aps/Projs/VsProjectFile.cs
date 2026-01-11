// variables names: ok
namespace SunamoDevCode.Aps.Projs;

/// <summary>
/// In case when ProjectCollection wont work (like actually dont return any Compile elements)
/// </summary>
public class VsProjectFile
{
    public string file = null;
    XmlDocument xDocument = null;
    XmlNamespacesHolder holder = null;
    public bool IsCore
    {
        get; set;
    }
    XmlNamespaceManager nsmgr
    {
        get
        {
            return holder.nsmgr;
        }
    }
    public string Name
    {
        get
        {
            return Path.GetFileNameWithoutExtension(file);
        }
    }
    public
#if ASYNC
    async Task
#else
    void
#endif
        Load(string file, Dictionary<string, XmlDocument> dictToAvoidCollectionWasChanged)
    {
        this.file = file;
        if (dictToAvoidCollectionWasChanged == null)
        {
            var result =
#if ASYNC
                await
#endif
                XmlDocumentsCache.Get(file);
            if (MayExcHelper.MayExc(result.Exc))
            {
                return;
            }
            xDocument = result.Data;
        }
        else
        {
            if (dictToAvoidCollectionWasChanged.ContainsKey(file))
            {
                xDocument = dictToAvoidCollectionWasChanged[file];
            }
            else
            {
                xDocument = null;
            }
        }
        //xDocument = await XmlDocumentsCache.GetAsync(file);
    }
    public VsProjectFile()
    {
    }
    /// <summary>
    /// Before calling must check whether file is exists
    /// </summary>
    /// <param name="file"></param>
    public VsProjectFile(string file)
    {
        // async ctor nejde. proto to už nikdy nemůžu načítat v ctoru
        // už navždy budu užívat jen .net core
        Load(file, null).RunSynchronously();
        //var content = TF.ReadAllText(file);
        //IsCore = VsProjectsFileHelper.IsCore(content);
        //holder = new XmlNamespacesHolder();
        //holder.ParseAndRemoveNamespacesXmlDocument(content);
        //xDocument = XmlHelper.CreateXmlDocument(content);
        //XmlHelper.AddXmlNamespaces(holder.nsmgr);
    }
    public bool IsValidXml
    {
        get => xDocument != null;
    }
    #region TODO: working with XmlDocument which is not supported already
    //XmlDocument xd = null;
    //public VsProjectFile(string pathOrXml)
    //{
    //    xd = new XmlDocument();
    //    if (FS.ExistsFile(pathOrXml))
    //    {
    //        xd.Load(pathOrXml);
    //    }
    //    else
    //    {
    //        xd.LoadXml(pathOrXml);
    //    }
    //}
    //public VsProjectFile(XmlDocument xd, XmlNamespacesHolder holder)
    //{
    //    this.xd = xd;
    //    this.holder = holder;
    //}
    #endregion

    //public void SetToItemGroup(ItemGroups ig, List<XmlNode> old, List<XmlNode> n)
    //{
    //}
    public static XmlNode GetElementOfName(XmlNode element, string name)
    {
        return element.ChildNodes.First(name);
    }
    public List<XmlNode> ReturnAllItemGroup(ItemGroups ig)
    {
        var project = XmlHelper.GetElementOfName(xDocument, "Project");
        var itemGroups = XmlHelper.GetElementsOfName(project, "ItemGroup");
        List<XmlNode> xelements = new List<XmlNode>();
        foreach (var item in itemGroups)
        {
            xelements.AddRange(XmlHelper.GetElementsOfName(item, ig.ToString()));
        }
        return xelements;
    }
    public void Delete(List<XmlElement> compile)
    {
        foreach (var item in compile)
        {
            item.ParentNode?.RemoveChild(item);
        }
    }
    public void Save()
    {
        xDocument.Save(file);
    }
}