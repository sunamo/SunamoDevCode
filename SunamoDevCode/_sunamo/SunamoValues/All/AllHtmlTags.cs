// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode._sunamo.SunamoValues.All;


/// <summary>
/// Must be in shared due to HtmlTextWriterTag in System.Web
/// All is lower
/// </summary>
internal class AllHtmlTags
{
    /// <summary>
    /// Sorted from longest to shortest due to comparing and finding right string
    /// </summary>
    internal static List<string> list = null;
    static List<string> withLeftArrow;
    internal static List<string> WithLeftArrow
    {
        get
        {
            if (withLeftArrow == null)
            {
                Initialize();
                withLeftArrow = new List<string>(list.Count);
                for (int i = 0; i < list.Count; i++)
                {
                    withLeftArrow.Add("<" + list[i] + " ");
                }
            }
            return withLeftArrow;
        }
    }
    internal static void Initialize()
    {
        if (list == null)
        {
            list = new List<string>();
            foreach (var item in Enum.GetNames(typeof(HtmlTextWriterTag)))
            {
                list.Add(item.ToLower());
            }
            //list.Sort(new SunamoComparerICompare.StringLength.Desc(SunamoComparer.StringLength.Instance));
        }
    }
}