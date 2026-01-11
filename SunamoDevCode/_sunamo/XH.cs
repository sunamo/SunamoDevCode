// variables names: ok
namespace SunamoDevCode._sunamo;

/// <summary>
/// XML helper methods
/// </summary>
internal class XH
{
    /// <summary>
    /// Converts XML string to XmlNode
    /// </summary>
    /// <param name="xml">XML string to convert</param>
    /// <returns>First child node of the loaded XML document</returns>
    internal static XmlNode ReturnXmlNode(string xml)
    {
        XmlDocument xmlDocument = null;
        if (xmlDocument == null) xmlDocument = new XmlDocument();
        xmlDocument.PreserveWhitespace = true;
        xmlDocument.LoadXml(xml);
        return (XmlNode)xmlDocument.FirstChild;
    }
}