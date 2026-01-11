// variables names: ok
namespace SunamoDevCode._sunamo;

/// <summary>
/// Helper methods for XML operations
/// </summary>
internal class XmlHelper
{
    /// <summary>
    /// Gets all child elements with the specified name
    /// </summary>
    /// <param name="node">Parent element to search in</param>
    /// <param name="name">Name of elements to find</param>
    /// <returns>List of matching XML nodes</returns>
    internal static IList<XmlNode> GetElementsOfName(XmlNode node, string name)
    {
        return node.ChildNodes.WithName(name);
    }

    /// <summary>
    /// Gets the first child element with the specified name
    /// </summary>
    /// <param name="node">Parent element to search in</param>
    /// <param name="name">Name of element to find</param>
    /// <returns>First matching XML node</returns>
    internal static XmlNode GetElementOfName(XmlNode node, string name)
    {
        return node.ChildNodes.First(name);
    }

    /// <summary>
    /// Gets the inner text of an XML node
    /// </summary>
    /// <param name="node">Node to get text from</param>
    /// <returns>Inner text of the node</returns>
    internal static string InnerTextOfNode(XmlNode node)
    {
        return node.InnerText;
    }

    /// <summary>
    /// Sets an attribute value on an XML node
    /// </summary>
    /// <param name="node">Node to set attribute on</param>
    /// <param name="attributeName">Name of the attribute</param>
    /// <param name="attributeValue">Value to set</param>
    internal static void SetAttribute(XmlNode node, string attributeName, string attributeValue)
    {
        var element = (XmlElement)node;
        if (element != null)
        {
            element.SetAttribute(attributeName, attributeValue);
            return;
        }
        // Working only when attribute
        var attributeValueExisting = Attr(node, attributeName);
        if (attributeValueExisting == null)
        {
            var newAttribute = node.OwnerDocument.CreateAttribute(attributeName);
            node.Attributes.Append(newAttribute);
        }
        node.Attributes[attributeName].Value = attributeValue;
    }

    /// <summary>
    /// Gets the value of an attribute
    /// </summary>
    /// <param name="node">Node to get attribute from</param>
    /// <param name="attributeName">Name of the attribute</param>
    /// <returns>Attribute value or null if not found</returns>
    internal static string Attr(XmlNode node, string attributeName)
    {
        var argument = GetAttributeWithName(node, attributeName);
        if (argument != null)
        {
            return argument.Value;
        }
        return null;
    }

    internal static XmlAttribute FoundedNode = null;

    /// <summary>
    /// Gets an attribute with the specified name
    /// </summary>
    /// <param name="node">Node to search in</param>
    /// <param name="attributeName">Name of the attribute to find</param>
    /// <returns>Matching attribute or null if not found</returns>
    internal static XmlNode GetAttributeWithName(XmlNode node, string attributeName)
    {
        foreach (XmlAttribute attribute in node.Attributes)
        {
            if (attribute.Name == attributeName)
            {
                FoundedNode = attribute;
                return attribute;
            }
        }
        return null;
    }
}
