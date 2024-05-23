//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


namespace SunamoDevCode;

public class XHelper
{
    public static Dictionary<string, string> ns = new Dictionary<string, string>();

    public static IList<XElement> GetElementsOfNameWithAttrContains(XElement group, string tag, string attr, string value, bool caseSensitive = false)
    {
        return GetElementsOfNameWithAttrWorker(group, tag, attr, value, true, caseSensitive);
    }
    public static List<XElement> GetElementsOfNameWithAttrWorker(System.Xml.Linq.XElement xElement, string tag, string attr, string value, bool enoughIsContainsAttribute, bool caseSensitive)
    {
        List<XElement> vr = new List<XElement>();
        List<XElement> e = XHelper.GetElementsOfNameRecursive(xElement, tag);
        foreach (XElement item in e)
        {
            var attrValue = XHelper.Attr(item, attr);
            if (attrValue.Contains(value) /*SH.ContainsBoolBool(attrValue, value, enoughIsContainsAttribute, caseSensitive)*/)
            {
                vr.Add(item);
            }
        }

        return vr;
    }

    public static List<XElement> GetElementsOfNameRecursive(XElement node, string nazev)
    {
        List<XElement> vr = new List<XElement>();

        if (nazev.Contains(AllStrings.colon))
        {
            var (p, z) = SH.GetPartsByLocationNoOut(nazev, AllChars.colon);
            p = XHelper.ns[p];
            foreach (XElement item in node.DescendantsAndSelf())
            {
                if (item.Name.LocalName == z && item.Name.NamespaceName == p)
                {
                    vr.Add(item);
                }
            }
        }
        else
        {
            foreach (XElement item in node.DescendantsAndSelf())
            {
                if (item.Name.LocalName == nazev)
                {
                    vr.Add(item);
                }
            }
        }

        return vr;
    }

    public static void AddXmlNamespaces(XmlNamespaceManager nsmgr)
    {
        foreach (string item in nsmgr)
        {
            // Jak� je typ item, at nemus�m pou��vat slovn�k
            var v = nsmgr.LookupNamespace(item);
            if (!ns.ContainsKey(item))
            {
                ns.Add(item, v);
            }
        }
    }

    public static XElement GetElementOfNameWithAttr(XElement node, string nazev, string attr, string value)
    {

        if (nazev.Contains(AllStrings.colon))
        {
            var (p, z) = SH.GetPartsByLocationNoOut(nazev, AllChars.colon);
            p = XHelper.ns[p];
            foreach (XElement item in node.Elements())
            {
                if (item.Name.LocalName == z && item.Name.NamespaceName == p)
                {
                    if (Attr(item, attr) == value)
                    {
                        return item;
                    }
                }
            }
        }
        else
        {
            foreach (XElement item in node.DescendantsAndSelf())
            {
                if (item.Name.LocalName == nazev)
                {
                    if (Attr(item, attr) == value)
                    {
                        return item;
                    }
                }
            }
        }

        return null;
    }

    public static string Attr(XElement item, string attr)
    {
        XAttribute xa = item.Attribute(XName.Get(attr));
        if (xa != null)
        {
            return xa.Value;
        }

        return null;
    }

    public static XElement MakeAllElementsWithDefaultNs(XElement settings)
    {
        var ns2 = XHelper.ns[string.Empty];
        List<object> toInsert = new List<object>();
        // shift ALL elements in the settings document into the target namespace
        foreach (XElement e in settings.DescendantsAndSelf())
        {
            //e.Name =  e.Name.LocalName;
            e.Name = XName.Get(e.Name.LocalName, ns2);
        }

        //foreach (var e in settings.Attributes())
        //{
        //    //e.Name = XName.Get(e.Name.LocalName, ns2);
        //    toInsert.Add(e);
        //}
        //t
        var vr = new XElement(XName.Get(settings.Name.LocalName, ns2), settings.Attributes(), settings.Descendants());
        return vr;
    }

    public static bool IsRightTag(XElement xName, string nazev)
    {
        return IsRightTag(xName.Name, nazev);
    }

    public static bool IsRightTag(XName xName, string nazev)
    {

        var (p, z) = SH.GetPartsByLocationNoOut(nazev, AllChars.colon);
        p = XHelper.ns[p];
        if (xName.LocalName == z && xName.NamespaceName == p)
        {
            return true;
        }

        return false;
    }

    public static List<XElement> GetElementsOfName(XElement node, string nazev)
    {
        List<XElement> result = new List<XElement>();
        if (nazev.Contains(AllStrings.colon))
        {
            foreach (XElement item in node.Elements())
            {
                if (IsRightTag(item, nazev))
                {
                    result.Add(item);
                }
            }
        }
        else
        {
            foreach (XElement item in node.Elements())
            {
                if (item.Name.LocalName == nazev)
                {
                    result.Add(item);
                }
            }
        }

        return result;
    }

    public static bool IsRightTag(XElement xName, string localName, string namespaceName)
    {
        return IsRightTag(xName.Name, localName, namespaceName);
    }

    public static bool IsRightTag(XName xName, string localName, string namespaceName)
    {
        if (xName.LocalName == localName && xName.NamespaceName == namespaceName)
        {
            return true;
        }

        return false;
    }

    public static XElement GetElementOfName(XContainer node, string nazev)
    {

        if (nazev.Contains(AllStrings.colon))
        {
            var (p, z) = SH.GetPartsByLocationNoOut(nazev, AllChars.colon);
            p = XHelper.ns[p];
            foreach (XElement item in node.Elements())
            {
                if (IsRightTag(item, z, p))
                {
                }

                if (item.Name.LocalName == z && item.Name.NamespaceName == p)
                {
                    return item;
                }
            }
        }
        else
        {
            foreach (XElement item in node.Elements())
            {
                if (item.Name.LocalName == nazev)
                {
                    return item;
                }
            }
        }

        return null;
    }

    public static
#if ASYNC
    async Task<XDocument>
#else
XDocument
#endif
    CreateXDocument(string contentOrFn)
    {
        if (File.Exists(contentOrFn))
        {
            contentOrFn =
#if ASYNC
            await
#endif
            File.ReadAllTextAsync(contentOrFn);
        }

        var enB = Encoding.UTF8.GetBytes(contentOrFn).ToList();
        XDocument xd = null;
        using (MemoryStream oStream = new MemoryStream(enB.ToArray()))
        using (XmlReader oReader = XmlReader.Create(oStream))
        {
            xd = XDocument.Load(oReader);
        }

        return xd;
    }

    public static
#if ASYNC
    async Task<string>
#else
string
#endif
    FormatXml(string pathOrContent)
    {
        var xmlFormat = pathOrContent;
        if (File.Exists(pathOrContent))
        {
            xmlFormat =
#if ASYNC
            await
#endif
            TFSE.ReadAllText(pathOrContent);
        }

        XmlNamespacesHolder h = new XmlNamespacesHolder();
        XDocument doc = h.ParseAndRemoveNamespacesXDocument(xmlFormat);


        var formatted = doc.ToString();
        formatted = formatted.Replace(" xmlns=\"\"", string.Empty);
        //HReplace.ReplaceAll2(formatted, string.Empty, " xmlns=\"\"");
        if (File.Exists(pathOrContent))
        {
#if ASYNC
            await
#endif
            TFSE.WriteAllText(pathOrContent, formatted);
            //ThisApp.Success(sess.i18n(XlfKeys.ChangesSavedToFile));
            return null;
        }
        else
        {
            //ThisApp.Success(sess.i18n(XlfKeys.ChangesSavedToClipboard));
            return formatted;
        }
    }
}
