namespace SunamoDevCode._sunamo.SunamoValues.All;

internal class AllHtmlAttrsValues
{
    static bool initialized = false;
    internal static List<string> list = new List<string>();
    internal static void Init()
    {
        if (!initialized)
        {
            initialized = true;
            var d = typeof(HtmlAttrValue).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly);
            foreach (var item in d)
            {
                list.Add(item.GetValue(null).ToString());
            }
            //list.Sort(new SunamoComparerICompare.StringLength.Desc(SunamoComparer.StringLength.Instance));
        }
    }
}
