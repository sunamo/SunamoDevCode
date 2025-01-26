namespace SunamoDevCode._sunamo.SunamoExtensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class XmlNodeListExtensions
{
    #region For easy copy from XmlNodeListExtensions.cs


    internal static XmlNode First(this XmlNodeList e, string n)
    {
        foreach (XmlNode item in e)
            if (item.Name == n)
                return item;
        return null;
    }

    internal static List<XmlNode> WithName(this XmlNodeList e, string n)
    {
        var result = new List<XmlNode>();
        foreach (XmlNode item in e)
            if (item.Name == n)
                result.Add(item);
        return result;
    }

    #endregion
}