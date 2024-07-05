namespace SunamoDevCode._sunamo.SunamoValues.All;


internal class AllHtmlAttrs
{
    //
    internal static List<string> list = null;
    internal static void Initialize()
    {
        if (list == null)
        {
            list = new List<string>();
            foreach (var item in Enum.GetNames(typeof(HtmlTextWriterAttribute)))
            {
                list.Add(item.ToLower());
            }
            //list.Sort(new SunamoComparerICompare.StringLength.Desc(SunamoComparer.StringLength.Instance));
        }
    }
}