namespace SunamoDevCode.Aps.Projs.Data.ItemGroup;

/// <summary>
/// For nuget in core and fw
/// </summary>
public class PackageReferenceItemGroup : ItemGroupElement
{
    public string Version = null;
    public static Type type = typeof(PackageReferenceItemGroup);
    public PackageReferenceItemGroup(string FullCsprojPath) : base(FullCsprojPath)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="FullCsprojPath"></param>
    /// <param name="xe"></param>
    public PackageReferenceItemGroup(string FullCsprojPath, XmlNode xe) : base(FullCsprojPath)
    {
        const string Version2 = "Version";
        Include = XmlHelper.Attr(xe, "Include");
        Version = XmlHelper.InnerTextOfNode(xe);
        if (Version == string.Empty)
        {
            Version = XmlHelper.Attr(xe, Version2);
        }
    }

    public override XmlNode ToXml(XmlDocument xd)
    {
        return null;
    }
}
