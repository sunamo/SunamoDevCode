namespace SunamoDevCode;


public class AllHtmlAttrs
{
    //
    public static List<string> list = null;
    public static void Initialize()
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