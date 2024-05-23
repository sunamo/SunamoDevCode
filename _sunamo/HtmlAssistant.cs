using HtmlAgilityPack;
using System.Net;

namespace SunamoDevCode;
public class HtmlAssistant
{
    public static string TrimInnerHtml(string value)
    {
        HtmlDocument hd = HtmlAgilityHelper.CreateHtmlDocument();
        hd.LoadHtml(value);
        foreach (var item in hd.DocumentNode.DescendantsAndSelf())
        {
            if (item.NodeType == HtmlNodeType.Element)
            {
                item.InnerHtml = item.InnerHtml.Trim();
            }
        }
        return hd.DocumentNode.OuterHtml;
    }
    public static string HtmlDecode(string v)
    {
        return WebUtility.HtmlDecode(v);
    }
}

