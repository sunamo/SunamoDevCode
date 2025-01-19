using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunamoDevCode._sunamo;
internal class XmlHelper
{
    internal static string Attr(XmlNode d, string v)
    {
        var a = GetAttributeWithName(d, v);
        if (a != null)
        {
            return a.Value;
        }
        return null;
    }
    internal static XmlAttribute foundedNode = null;
    internal static XmlNode GetAttributeWithName(XmlNode item, string p)
    {
        foreach (XmlAttribute item2 in item.Attributes)
        {
            if (item2.Name == p)
            {
                foundedNode = item2;
                return item2;
            }
        }

        return null;
    }
}
