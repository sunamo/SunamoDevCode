namespace SunamoDevCode.Aps.Projs;

public partial class VsProjectsFileHelper
{
    /// <summary>
    /// Loads XML content into an XmlDocument and initializes namespace manager and key nodes.
    /// </summary>
    /// <param name="itemGroups">The type of ItemGroup to work with.</param>
    /// <param name="xmlDocument">The XmlDocument to load content into.</param>
    /// <param name="content">The XML content string to parse.</param>
    /// <param name="namespaceManager">Output: XML namespace manager for XPath queries.</param>
    /// <param name="itemGroup">Output: The first ItemGroup node found in the project.</param>
    /// <param name="parent">Output: Parent node (initialized as null).</param>
    /// <param name="project">Output: The root Project node.</param>
    private static void LoadXml(ItemGroups itemGroups, XmlDocument xmlDocument, string content, out XmlNamespaceManager namespaceManager, out XmlNode itemGroup, out XmlNode parent, out XmlNode project)
    {
        xmlDocument.PreserveWhitespace = true;
        xmlDocument.LoadXml(content);
        namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
        string wanted = itemGroups.ToString();
        itemGroup = xmlDocument.SelectSingleNode("//Project/ItemGroup", namespaceManager);
        parent = null;
        project = xmlDocument.SelectSingleNode("//Project", namespaceManager);
    }
}