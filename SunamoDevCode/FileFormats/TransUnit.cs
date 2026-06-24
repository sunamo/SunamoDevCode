namespace SunamoDevCode.FileFormats;

public class TransUnit
{
    public string Id { get; set; } = null!;

    public bool Translate { get; set; }

    public string XmlSpace { get; set; } = null!;

    private string _source = null!;

    public string Source
    {
        get => _source;
        set
        {
            if (value is not null)
            {
                value = SHNotTranslateAble.DecodeSlashEncodedString(value);
                value = HtmlAssistant.TrimInnerHtml(value);
                value = HtmlDocument.HtmlEncode(value);
            }
            _source = value!;
        }
    }

    private string _target = null!;

    public string Target
    {
        get => _target;
        set
        {
            value = SHNotTranslateAble.DecodeSlashEncodedString(value);
            value = HtmlAssistant.TrimInnerHtml(value);
            value = HtmlDocument.HtmlEncode(value);
            _target = value;
        }
    }

    public const string TransUnitTagName = "trans-unit";

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
