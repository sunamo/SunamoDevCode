namespace SunamoDevCode._sunamo.SunamoExtensions;

internal static class ListExtensions
{
    /// <summary>
    ///     Direct edit
    /// </summary>
    /// <param name="list"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    internal static List<string> LeadingRange(this List<string> list, IList<string> items)
    {
        for (var i = items.Count - 1; i >= 0; i--) list.Insert(0, items[i]);
        return list;
    }
}