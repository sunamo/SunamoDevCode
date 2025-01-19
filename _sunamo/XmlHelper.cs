﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunamoDevCode._sunamo;
internal class XmlHelper
{
    public static void SetAttribute(XmlNode node, string include, string rel)
    {
        var xe = (XmlElement)node;
        if (xe != null)
        {
            xe.SetAttribute(include, rel);
            return;
        }

        // Working only when attribute
        var atrValue = Attr(node, include);
        if (atrValue == null)
        {
            var xa = node.OwnerDocument.CreateAttribute(include);
            node.Attributes.Append(xa);
        }

        node.Attributes[include].Value = rel;
    }
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
