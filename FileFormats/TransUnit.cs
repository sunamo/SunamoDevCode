namespace SunamoDevCode.FileFormats;

public class TransUnit
{
    public string id;
    public bool translate;
    public string xml_space;


    private string _source;

    public string source
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
    public string target
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

    public const string tTransUnit = "trans-unit";

    public string ToString(IXmlGeneratorDC g)
    {
        //XmlGenerator g = new XmlGenerator();
        g.WriteTagWithAttrs(tTransUnit, "id", id, "translate", BTS.BoolToString(translate, true), "xml:space", "preserve");
        g.WriteElement("source", source);

        g.WriteTagWithAttr("target", "state", "translated");
        g.WriteRaw(target);
        g.TerminateTag("target");

        g.TerminateTag(tTransUnit);

        return g.ToString();
    }
}
