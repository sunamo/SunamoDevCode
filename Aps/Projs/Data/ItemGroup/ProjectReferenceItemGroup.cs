namespace SunamoDevCode.Aps.Projs.Data.ItemGroup;

public class ProjectReferenceItemGroup : ItemGroupElement
{
    public static Type type = typeof(ProjectReferenceItemGroup);
    public string Project = null;
    public string Name = null;
    public ProjectReferenceItemGroup(string _FullPath) : base(_FullPath)
    {
    }
    public override XmlNode ToXml(XmlDocument xd)
    {
        // .net fw vkládá ještě elementy Name a ProjectGuid. Nicméně ty nemusím ani vyplňovat a fungovalo by to úplně stejně. Zkoušeno s rebuildem.
        var x = "<" + ItemGroups.ProjectReference + " Include=\"" + Include + "\" />";
        XmlNode xn = XH.ReturnXmlNode(x.ToString(), xd);
        return xn;
    }
}