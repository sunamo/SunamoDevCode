namespace SunamoDevCode.FileFormats;

/// <summary>
/// Represents data from an XLF (XML Localization Interchange File Format) file including trans-units and metadata.
/// </summary>
public class XlfData
{
    /// <summary>
    /// Gets or sets the file path to the XLF file.
    /// </summary>
    public string Path { get; set; } = null;

    /// <summary>
    /// Gets or sets the group XML element.
    /// </summary>
    public XElement Group { get; set; } = null;

    /// <summary>
    /// Gets or sets the XML document.
    /// </summary>
    public XDocument XmlDocument { get; set; } = null;

    /// <summary>
    /// Gets or sets the list of trans-unit XML elements.
    /// </summary>
    public List<XElement> TransUnits { get; set; } = null;

    /// <summary>
    /// Gets or sets the list of all translation unit IDs.
    /// </summary>
    public List<string> AllIds { get; set; } = null;

    /// <summary>
    /// Fills the AllIds list with IDs from all trans-units.
    /// </summary>
    public void FillIds()
    {
        AllIds = new List<string>(TransUnits.Count);

        foreach (var item in TransUnits)
        {
            AllIds.Add(XmlLocalisationInterchangeFileFormat.Id(item));
        }
    }
}