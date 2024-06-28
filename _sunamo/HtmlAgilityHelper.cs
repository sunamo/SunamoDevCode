namespace SunamoDevCode;
internal class HtmlAgilityHelper
{
    internal static HtmlDocument CreateHtmlDocument(CreateHtmlDocumentInitData d = null)
    {
        HtmlDocument hd = new HtmlDocument();

        hd.OptionOutputOriginalCase = true;
        // false - i přesto mi tag ukončený na / převede na </Page>. Musí se ještě tagy jež nechci ukončovat vymazat z HtmlAgilityPack.HtmlNode.ElementsFlags.Remove("form"); před načetním XML https://html-agility-pack.net/knowledge-base/7104652/htmlagilitypack-close-form-tag-automatically
        hd.OptionAutoCloseOnEnd = false;
        hd.OptionOutputAsXml = false;
        hd.OptionFixNestedTags = false;
        //when OptionCheckSyntax = false, raise NullReferenceException in Load/LoadHtml
        //hd.OptionCheckSyntax = false;
        return hd;
    }
}
