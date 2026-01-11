// variables names: ok
namespace SunamoDevCode.Aps.Projs.Data.ItemGroup;

/// <summary>
/// Represents a None item group in a Visual Studio project file.
/// </summary>
public class NoneItemGroup : ItemGroupElement
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NoneItemGroup"/> class.
    /// </summary>
    public NoneItemGroup() : base(string.Empty)
    {
    }

    /// <summary>
    /// Converts the None item group to an XML node.
    /// </summary>
    /// <param name="xmlDocument">The XML document to use for creating the node.</param>
    /// <returns>The XML node representing the None item group.</returns>
    public override XmlNode ToXml(XmlDocument xmlDocument)
    {
        XmlGenerator generator = new XmlGenerator();
        generator.WriteNonPairTagWithAttr(VsProjectItemTypes.None, "Include", Include);
        XmlNode node = XH.ReturnXmlNode(generator.ToString());
        return node;
    }
}