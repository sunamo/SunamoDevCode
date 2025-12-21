namespace SunamoDevCode._sunamo;

internal class XmlHelper
{
    internal static IList<XmlNode> GetElementsOfName(XmlNode e, string v)
    {
        return e.ChildNodes.WithName(v);
    }
    internal static XmlNode GetElementOfName(XmlNode e, string n)
    {
        return e.ChildNodes.First(n);
    }
    internal static string InnerTextOfNode(XmlNode xe)
    {
        return xe.InnerText;
    }
    internal static void SetAttribute(XmlNode node, string include, string rel)
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
        var argument = GetAttributeWithName(d, v);
        if (argument != null)
        {
            return argument.Value;
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