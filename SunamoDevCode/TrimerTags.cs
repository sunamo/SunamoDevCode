namespace SunamoDevCode;

/// <summary>
/// Utility for trimming wrapping HTML tags (b, i, u) from text content.
/// </summary>
public static class TrimerTags
{
    private static List<string> tagsWrapping = null!;
    private static List<string> tagsWrappingUpper = null!;

    /// <summary>
    /// Trims the outermost wrapping HTML tag from the given HTML string, recording removed tags.
    /// </summary>
    /// <param name="html">HTML text to trim.</param>
    /// <param name="fromStart">StringBuilder to record the opening tag removed from the start.</param>
    /// <param name="fromEnd">StringBuilder to record the closing tag removed from the end.</param>
    /// <returns>HTML with the outermost wrapping tag removed.</returns>
    public static string TrimWrappingTag(string html, StringBuilder fromStart, StringBuilder fromEnd)
    {
        fromStart.Clear();
        fromEnd.Clear();

        html = html.Trim();
        if (!html.StartsWith("<")) return html;

        var changed = false;

        html = TrimmingWrappingTags(html, tagsWrapping, fromStart, fromEnd, ref changed);
        if (!changed) html = TrimmingWrappingTags(html, tagsWrappingUpper, fromStart, fromEnd, ref changed);

        return html;
    }

    private static string TrimmingWrappingTags(string html, List<string> tagsWrapping, StringBuilder fromStart,
        StringBuilder fromEnd, ref bool changed)
    {
        string? endingTag = null;

        for (var i = 0; i < tagsWrapping.Count; i++)
        {
            var item = tagsWrapping[i];
            if (html.StartsWith(item))
            {
                endingTag = item.Replace("<", "<" + "/");

                if (!html.EndsWith(endingTag)) continue;

                html = SHTrim.TrimStart(html, item);

                html = SHTrim.TrimEnd(html, endingTag);

                fromStart.Append(item);
                fromEnd.Append(endingTag);
                changed = true;
                break;
            }
        }

        return html;
    }

    /// <summary>
    /// Initializes the wrapping tag lists with common formatting tags (i, b, u) in both cases.
    /// </summary>
    public static void InitTagsWrapping()
    {
        tagsWrapping = new List<string>(["i", "b", "u"]);
        tagsWrappingUpper = new List<string>(tagsWrapping.Count);
        foreach (var item in tagsWrapping) tagsWrappingUpper.Add(item.ToUpper());

        WrapWithBracket(tagsWrapping);
        WrapWithBracket(tagsWrappingUpper);
    }

    private static void WrapWithBracket(List<string> tagsWrapping)
    {
        for (var i = 0; i < tagsWrapping.Count; i++) tagsWrapping[i] = "<" + tagsWrapping[i] + ">";
    }
}