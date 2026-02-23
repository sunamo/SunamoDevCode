namespace SunamoDevCode._public.SunamoInterfaces.Interfaces;

/// <summary>
/// Interface for generating XML content with various tag and attribute writing capabilities.
/// </summary>
public interface IXmlGeneratorDC
{
    /// <summary>
    /// Appends a new line to the XML output.
    /// </summary>
    void AppendLine();
    /// <summary>
    /// Writes the end of an XML comment block.
    /// </summary>
    void EndComment();
    /// <summary>
    /// Inserts text at the specified index position.
    /// </summary>
    /// <param name="index">Position to insert at</param>
    /// <param name="text">Text to insert</param>
    void Insert(int index, string text);
    /// <summary>
    /// Returns the current length of the XML output.
    /// </summary>
    /// <returns>Length of the output</returns>
    int Length();
    /// <summary>
    /// Writes the start of an XML comment block.
    /// </summary>
    void StartComment();
    /// <summary>
    /// Writes a closing tag for the specified element.
    /// </summary>
    /// <param name="tagName">Name of the tag to close</param>
    void TerminateTag(string tagName);
    /// <summary>
    /// Returns the current XML output as a string.
    /// </summary>
    /// <returns>String representation of the XML output</returns>
    string ToString();
    /// <summary>
    /// Writes a CDATA section with the specified content.
    /// </summary>
    /// <param name="innerCData">Content for the CDATA section</param>
    void WriteCData(string innerCData);
    /// <summary>
    /// Writes an XML element with inner text content.
    /// </summary>
    /// <param name="elementName">Name of the element</param>
    /// <param name="inner">Inner text content</param>
    void WriteElement(string elementName, string inner);
    /// <summary>
    /// Writes a self-closing XML tag without attributes.
    /// </summary>
    /// <param name="tagName">Name of the tag</param>
    void WriteNonPairTag(string tagName);
    /// <summary>
    /// Writes a self-closing XML tag with two attributes.
    /// </summary>
    /// <param name="tagName">Name of the tag</param>
    /// <param name="attr1Name">First attribute name</param>
    /// <param name="attr1Value">First attribute value</param>
    /// <param name="attr2Name">Second attribute name</param>
    /// <param name="attr2Value">Second attribute value</param>
    void WriteNonPairTagWith2Attrs(string tagName, string attr1Name, string attr1Value, string attr2Name, string attr2Value);
    /// <summary>
    /// Writes a self-closing XML tag with one attribute.
    /// </summary>
    /// <param name="tagName">Name of the tag</param>
    /// <param name="attrName">Attribute name</param>
    /// <param name="attrValue">Attribute value</param>
    void WriteNonPairTagWithAttr(string tagName, string attrName, string attrValue);
    /// <summary>
    /// Writes a self-closing XML tag with multiple attributes, optionally appending null values.
    /// </summary>
    /// <param name="appendNull">Whether to append null attribute values</param>
    /// <param name="tagName">Name of the tag</param>
    /// <param name="attributes">Attribute name-value pairs</param>
    void WriteNonPairTagWithAttrs(bool appendNull, string tagName, params string[] attributes);
    /// <summary>
    /// Writes a self-closing XML tag with attributes from a list.
    /// </summary>
    /// <param name="tag">Name of the tag</param>
    /// <param name="args">List of attribute name-value pairs</param>
    void WriteNonPairTagWithAttrs(string tag, List<string> args);
    /// <summary>
    /// Writes a self-closing XML tag with attributes from params array.
    /// </summary>
    /// <param name="tag">Name of the tag</param>
    /// <param name="args">Attribute name-value pairs</param>
    void WriteNonPairTagWithAttrs(string tag, params string[] args);
    /// <summary>
    /// Writes raw XML content directly to the output.
    /// </summary>
    /// <param name="rawContent">Raw XML content to write</param>
    void WriteRaw(string rawContent);
    /// <summary>
    /// Writes an opening XML tag.
    /// </summary>
    /// <param name="tagName">Name of the tag</param>
    void WriteTag(string tagName);
    /// <summary>
    /// Writes a tag with namespace manager configuration using an RSS object.
    /// </summary>
    /// <param name="rss">RSS object for namespace configuration</param>
    /// <param name="nsmgr">XML namespace manager</param>
    /// <param name="namespaceUri">Namespace URI</param>
    /// <param name="prefix">Namespace prefix</param>
    void WriteTagNamespaceManager(object rss, XmlNamespaceManager nsmgr, string namespaceUri, string prefix);
    /// <summary>
    /// Writes a tag with namespace manager and attribute arguments.
    /// </summary>
    /// <param name="tagName">Name of the tag</param>
    /// <param name="nsmgr">XML namespace manager</param>
    /// <param name="args">Additional arguments</param>
    void WriteTagNamespaceManager(string tagName, XmlNamespaceManager nsmgr, params string[] args);
    /// <summary>
    /// Writes a tag with two attributes.
    /// </summary>
    /// <param name="tagName">Name of the tag</param>
    /// <param name="attr1Name">First attribute name</param>
    /// <param name="attr1Value">First attribute value</param>
    /// <param name="attr2Name">Second attribute name</param>
    /// <param name="attr2Value">Second attribute value</param>
    void WriteTagWith2Attrs(string tagName, string attr1Name, string attr1Value, string attr2Name, string attr2Value);
    /// <summary>
    /// Writes a tag with one attribute, optionally skipping empty or null values.
    /// </summary>
    /// <param name="tag">Name of the tag</param>
    /// <param name="attributeName">Attribute name</param>
    /// <param name="attributeValue">Attribute value</param>
    /// <param name="skipEmptyOrNull">Whether to skip writing if value is empty or null</param>
    void WriteTagWithAttr(string tag, string attributeName, string attributeValue, bool skipEmptyOrNull = false);
    /// <summary>
    /// Writes a tag with attributes from a list.
    /// </summary>
    /// <param name="tagName">Name of the tag</param>
    /// <param name="attributes">List of attribute name-value pairs</param>
    void WriteTagWithAttrs(string tagName, List<string> attributes);
    /// <summary>
    /// Writes a tag with attributes from params array.
    /// </summary>
    /// <param name="tagName">Name of the tag</param>
    /// <param name="attributes">Attribute name-value pairs</param>
    void WriteTagWithAttrs(string tagName, params string[] attributes);
    /// <summary>
    /// Writes a tag with attributes, checking for null values.
    /// </summary>
    /// <param name="tagName">Name of the tag</param>
    /// <param name="attributes">Attribute name-value pairs</param>
    void WriteTagWithAttrsCheckNull(string tagName, params string[] attributes);
    /// <summary>
    /// Writes the standard XML declaration header.
    /// </summary>
    void WriteXmlDeclaration();
}