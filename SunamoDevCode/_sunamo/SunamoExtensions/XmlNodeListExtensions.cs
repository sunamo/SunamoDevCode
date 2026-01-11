namespace SunamoDevCode._sunamo.SunamoExtensions;

internal static class XmlNodeListExtensions
{
    #region For easy copy from XmlNodeListExtensions.cs
    /// <summary>
    /// Returns first XML node with specified name.
    /// </summary>
    /// <param name="nodeList">XML node list to search in.</param>
    /// <param name="nodeName">Name of the node to find.</param>
    /// <returns>First matching node or null if not found.</returns>
    internal static XmlNode First(this XmlNodeList nodeList, string nodeName)
    {
        foreach (XmlNode item in nodeList)
            if (item.Name == nodeName)
                return item;
        return null;
    }

    /// <summary>
    /// Returns all XML nodes with specified name.
    /// </summary>
    /// <param name="nodeList">XML node list to search in.</param>
    /// <param name="nodeName">Name of the nodes to find.</param>
    /// <returns>List of all matching nodes.</returns>
    internal static List<XmlNode> WithName(this XmlNodeList nodeList, string nodeName)
    {
        var result = new List<XmlNode>();
        foreach (XmlNode item in nodeList)
            if (item.Name == nodeName)
                result.Add(item);
        return result;
    }
    #endregion
}