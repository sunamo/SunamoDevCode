namespace SunamoDevCode.Aps.Projs;

/// <summary>
/// In case when ProjectCollection wont work (like actually dont return any Compile elements)
/// </summary>
public class VsProjectFile
{
    /// <summary>
    /// Path to the project file.
    /// </summary>
    public string file = null!;
    XmlDocument? xDocument = null;
    XmlNamespacesHolder? holder = null;
    /// <summary>
    /// Whether this is a .NET Core SDK-style project.
    /// </summary>
    public bool IsCore
    {
        get; set;
    }
    XmlNamespaceManager? nsmgr
    {
        get
        {
            return holder?.NamespaceManager!;
        }
    }
    /// <summary>
    /// Gets the project name (filename without extension).
    /// </summary>
    public string Name
    {
        get
        {
            return Path.GetFileNameWithoutExtension(file);
        }
    }
    /// <summary>
    /// Loads the XML document for the project file, using a cache dictionary if provided.
    /// </summary>
    /// <param name="file">Path to the project file.</param>
    /// <param name="dictToAvoidCollectionWasChanged">Optional dictionary cache to avoid collection-was-changed exceptions.</param>
    public
#if ASYNC
    async Task
#else
    void
#endif
        Load(string file, Dictionary<string, XmlDocument>? dictToAvoidCollectionWasChanged)
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
    /// <summary>
    /// Default constructor for VsProjectFile.
    /// </summary>
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
        //XmlHelper.AddXmlNamespaces(holder.NamespaceManager);
    }
    /// <summary>
    /// Whether the project XML was loaded successfully.
    /// </summary>
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
    /// <summary>
    /// Gets the first child element with the specified name from an XML node.
    /// </summary>
    /// <param name="element">Parent XML node to search.</param>
    /// <param name="name">Name of the child element to find.</param>
    /// <returns>First matching child node.</returns>
    public static XmlNode GetElementOfName(XmlNode element, string name)
    {
        return element.ChildNodes.First(name);
    }
    /// <summary>
    /// Returns all XML nodes matching the specified item group type from the project.
    /// </summary>
    /// <param name="ig">Item group type to search for (e.g. Compile, Reference).</param>
    /// <returns>List of matching XML nodes.</returns>
    public List<XmlNode> ReturnAllItemGroup(ItemGroups ig)
    {
        var project = XmlHelper.GetElementOfName(xDocument!, "Project")!;
        var itemGroups = XmlHelper.GetElementsOfName(project!, "ItemGroup");
        List<XmlNode> xelements = new List<XmlNode>();
        foreach (var item in itemGroups)
        {
            xelements.AddRange(XmlHelper.GetElementsOfName(item, ig.ToString()));
        }
        return xelements;
    }
    /// <summary>
    /// Deletes the specified XML elements from their parent nodes.
    /// </summary>
    /// <param name="compile">List of XML elements to remove.</param>
    public void Delete(List<XmlElement> compile)
    {
        foreach (var item in compile)
        {
            item.ParentNode?.RemoveChild(item);
        }
    }
    /// <summary>
    /// Saves the XML document back to the project file.
    /// </summary>
    public void Save()
    {
        xDocument!.Save(file);
    }
}