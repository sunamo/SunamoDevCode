namespace SunamoDevCode.Aps.Projs.Data.ItemGroup;

/// <summary>
/// Base class for item group elements in a Visual Studio project file.
/// </summary>
public abstract class ItemGroupElement
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ItemGroupElement"/> class.
    /// </summary>
    /// <param name="fullPath">Full path to the csproj file.</param>
    public ItemGroupElement(string fullPath)
    {
        this.FullPath = fullPath;
    }

    /// <summary>
    /// Gets or sets the full path to the csproj file.
    /// Must exist. From it and relative structure, other properties are filled.
    /// </summary>
    public string FullPath { get; set; }

    /// <summary>
    /// Gets or sets the Include attribute value.
    /// </summary>
    public string Include { get; set; }

    /// <summary>
    /// Converts the item group element to an XML node.
    /// </summary>
    /// <param name="xmlDocument">The XML document to use for creating the node.</param>
    /// <returns>The XML node representing the item group element.</returns>
    public abstract XmlNode ToXml(XmlDocument xmlDocument);
}