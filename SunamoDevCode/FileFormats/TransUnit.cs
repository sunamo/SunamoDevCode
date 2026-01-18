namespace SunamoDevCode.FileFormats;

/// <summary>
/// Represents a trans-unit element in an XLF (XML Localization Interchange File Format) file.
/// </summary>
public class TransUnit
{
    /// <summary>
    /// Gets or sets the unique identifier for this translation unit.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this unit should be translated.
    /// </summary>
    public bool Translate { get; set; }

    /// <summary>
    /// Gets or sets the xml:space attribute value (typically "preserve").
    /// </summary>
    public string XmlSpace { get; set; }

    private string _source;

    /// <summary>
    /// Gets or sets the source text (original language).
    /// The setter automatically decodes, trims, and HTML-encodes the value.
    /// </summary>
    public string Source
    {
        get
        {
            return _source;
        }
        set
        {
            if (value != null)
            {
                value = SHNotTranslateAble.DecodeSlashEncodedString(value);
                value = HtmlAssistant.TrimInnerHtml(value);
                value = HtmlDocument.HtmlEncode(value);
            }
            _source = value;
        }
    }

    private string _target;

    /// <summary>
    /// Gets or sets the target text (translated language).
    /// The setter automatically decodes, trims, and HTML-encodes the value.
    /// </summary>
    public string Target
    {
        get
        {
            return _target;
        }
        set
        {
            value = SHNotTranslateAble.DecodeSlashEncodedString(value);
            value = HtmlAssistant.TrimInnerHtml(value);
            value = HtmlDocument.HtmlEncode(value);
            _target = value;
        }
    }

    /// <summary>
    /// The XML tag name for trans-unit elements.
    /// </summary>
    public const string TransUnitTagName = "trans-unit";

    /// <summary>
    /// Converts this trans-unit to an XML string representation.
    /// </summary>
    /// <param name="generator">XML generator to use for creating the XML.</param>
    /// <returns>XML string representation of the trans-unit.</returns>
    public string ToString(IXmlGeneratorDC generator)
    {
        generator.WriteTagWithAttrs(TransUnitTagName, "id", Id, "translate", BTS.BoolToString(Translate, true), "xml:space", "preserve");
        generator.WriteElement("source", Source);

        generator.WriteTagWithAttr("target", "state", "translated");
        generator.WriteRaw(Target);
        generator.TerminateTag("target");

        generator.TerminateTag(TransUnitTagName);

        return generator.ToString();
    }
}