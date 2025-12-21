namespace SunamoDevCode.Aps.Projs.Data.ItemGroup;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public class CompileItemGroup : ItemGroupElement
{
    public string Link = null;
    /// <summary>
    /// Use in checking whether exists already, not for simple adding
    /// </summary>
    public string FullPathFile = null;
    /// <summary>
    /// A1 - To csproj. Must exists. From it and relative structure is filled other properties
    /// </summary>
    /// <param name="_FullPath"></param>
    public CompileItemGroup(string _FullPath) : base(_FullPath)
    {
    }
    public override XmlNode ToXml(XmlDocument xd)
    {
        throw new Exception("Make it with ");
        //XmlGenerator x = new XmlGenerator();
        //x.WriteTagWithAttrsCheckNull(VsProjectItemTypes.Compile, "Include", Include, "Link", Link);
        //x.TerminateTag(VsProjectItemTypes.Compile);
        //XmlNode xn = XH.ReturnXmlNode(x.ToString(), xd);
        //return xn;
    }
}