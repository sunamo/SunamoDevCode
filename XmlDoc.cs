namespace SunamoDevCode;

public class XmlDoc
{
    private readonly InstantSB sb;

    public XmlDoc(InstantSB sb)
    {
        this.sb = sb;
    }

    public void SummaryStart()
    {
        Prefix("<summary>");
    }

    private void Prefix(string v)
    {
        v = v.Replace("\r", "");
        var p = v.Split('\n');

        foreach (var item in p) sb.AppendLine("/// " + item);
    }

    public void SummaryEnd(bool appendLine = true)
    {
        Prefix("</summary>");
        if (appendLine) sb.AppendLine();
    }

    public void Raw(string r)
    {
        Prefix(r);
    }
}