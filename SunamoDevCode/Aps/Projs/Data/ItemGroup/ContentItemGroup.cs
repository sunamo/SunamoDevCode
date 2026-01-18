namespace SunamoDevCode.Aps.Projs.Data.ItemGroup;

/// <summary>
/// Represents a content item group in a Visual Studio project file.
/// </summary>
public class ContentItemGroup : ItemGroupElement
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ContentItemGroup"/> class.
    /// </summary>
    /// <param name="fullPath">Full path to the csproj file.</param>
    public ContentItemGroup(string fullPath) : base(fullPath)
    {

    }

    /// <summary>
    /// Converts the content item group to an XML node.
    /// </summary>
    /// <param name="xmlDocument">The XML document to use for creating the node.</param>
    /// <returns>The XML node representing the content item group, or null if not implemented.</returns>
    public override XmlNode ToXml(XmlDocument xmlDocument)
    {
        return null;
    }
}