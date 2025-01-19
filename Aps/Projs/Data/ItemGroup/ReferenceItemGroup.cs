namespace SunamoDevCode.Aps.Projs.Data.ItemGroup;
public class ReferenceItemGroup : ItemGroupElement
{
    public static Type type = typeof(ReferenceItemGroup);
    public Dictionary<string, string> IncludeParts = new Dictionary<string, string>();
    public ReferenceItemGroup(string _FullPathDllInclude, string _FullPathCsproj, string _RelativePathDllToProjectFolderHintPath) : base(_FullPathCsproj)
    {
        Include = _FullPathDllInclude;
        this._RelativePathDllToProjectFolderHintPath = _RelativePathDllToProjectFolderHintPath;
    }
    public ReferenceItemGroup(string FullCsprojPath, XmlNode xe) : base(FullCsprojPath)
    {
        Include = XmlHelper.Attr(xe, "Include");
        if (Include.Contains(","))
        {
            var IncludeParts2 = SHSplit.SplitMore(Include, ",");
            CA.Trim(IncludeParts2);
            foreach (var item in IncludeParts2)
            {
                if (item.Contains("="))
                {
                    var sp = SHSplit.SplitMore(item, "=");
                    IncludeParts.Add(sp[0], sp[1]);
                }
            }
        }
    }
    public string Version
    {
        get => FromInclude("Version");
    }
    private string FromInclude(string v)
    {
        if (IncludeParts.ContainsKey(v))
        {
            return IncludeParts[v];
        }
        return string.Empty;
    }
    #region MyRegion
    /// <summary>
    /// Využívá se poté dále v AddItemGroup2 jako HintPath
    /// </summary>
    public string _RelativePathDllToProjectFolderHintPath = null;
    /// <summary>
    /// Využívá se poté dále v AddItemGroup2 kde se s pomocí Path.GetFileNameWithoutExtension převádí na include Include
    /// To znamená že musí mít vlastní příponu, aby se nepoužila např. Web ze System.Web
    /// </summary>
    //public string _FullPathDllInclude = null;
    #endregion
    public override XmlNode ToXml(XmlDocument xd)
    {
        XmlGenerator x = new XmlGenerator();
        x.WriteTagWithAttrsCheckNull(VsProjectItemTypes.Reference, "Include", Include);
        x.TerminateTag(VsProjectItemTypes.Reference);
        XmlNode xn = XH.ReturnXmlNode(x.ToString(), xd);
        return xn;
    }
}