namespace SunamoDevCode._sunamo;

internal class XH
{
    public static XmlNode ReturnXmlNode(string xml, XmlDocument xdoc2 = null)
    {
        XmlDocument xdoc = null;
        //XmlTextReader xtr = new XmlTextReader(
        if (xdoc == null) xdoc = new XmlDocument();
        xdoc.PreserveWhitespace = true;
        xdoc.LoadXml(xml);
        //xdoc.Load(soubor);
        return (XmlNode)xdoc.FirstChild;
    }
}