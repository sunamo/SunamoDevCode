namespace SunamoDevCode._sunamo;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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