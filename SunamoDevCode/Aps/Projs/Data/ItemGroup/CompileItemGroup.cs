// variables names: ok
namespace SunamoDevCode.Aps.Projs.Data.ItemGroup;

public class CompileItemGroup : ItemGroupElement
{
    /// <summary>
    /// Link attribute for the compile item
    /// </summary>
    public string Link = null;

    /// <summary>
    /// Full path to the file - use in checking whether exists already, not for simple adding
    /// </summary>
    public string FullPathFile = null;

    /// <summary>
    /// Initializes a new instance of CompileItemGroup
    /// A1 - To csproj. Must exists. From it and relative structure is filled other properties
    /// </summary>
    /// <param name="fullPath">Full path to the file</param>
    public CompileItemGroup(string fullPath) : base(fullPath)
    {
    }

    /// <summary>
    /// Converts the compile item group to XML node
    /// </summary>
    /// <param name="xmlDocument">XML document to create the node in</param>
    /// <returns>XML node representing this compile item</returns>
    public override XmlNode ToXml(XmlDocument xmlDocument)
    {
        throw new Exception("Make it with ");
    }
}