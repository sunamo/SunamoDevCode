namespace SunamoDevCode._sunamo.SunamoXml;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
internal partial class XHelper
{
    internal static 
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
            File.ReadAllTextAsync(pathOrContent);
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
            File.WriteAllTextAsync(pathOrContent, formatted);
            //ThisApp.Success(Translate.FromKey(XlfKeys.ChangesSavedToFile));
            return null;
        }
        else
        {
            //ThisApp.Success(Translate.FromKey(XlfKeys.ChangesSavedToClipboard));
            return formatted;
        }
    }
}