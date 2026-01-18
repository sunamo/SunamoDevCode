namespace SunamoDevCode;

/// <summary>
/// Helper class for generating XML documentation comments.
/// </summary>
public class XmlDoc
{
    private readonly InstantSB _stringBuilder;

    /// <summary>
    /// Initializes a new instance of the XmlDoc class.
    /// </summary>
    /// <param name="stringBuilder">The string builder to write XML documentation to.</param>
    public XmlDoc(InstantSB stringBuilder)
    {
        _stringBuilder = stringBuilder;
    }

    /// <summary>
    /// Appends the opening summary tag.
    /// </summary>
    public void SummaryStart()
    {
        Prefix("<summary>");
    }

    /// <summary>
    /// Prefixes each line of text with XML comment marker.
    /// </summary>
    /// <param name="text">The text to prefix with XML comment markers.</param>
    private void Prefix(string text)
    {
        text = text.Replace("\r", "");
        var lines = text.Split('\n');

        foreach (var line in lines) _stringBuilder.AppendLine("/// " + line);
    }

    /// <summary>
    /// Appends the closing summary tag.
    /// </summary>
    /// <param name="appendLine">Whether to append an additional blank line after the summary tag.</param>
    public void SummaryEnd(bool appendLine = true)
    {
        Prefix("</summary>");
        if (appendLine) _stringBuilder.AppendLine();
    }

    /// <summary>
    /// Appends raw XML documentation text.
    /// </summary>
    /// <param name="text">The raw XML documentation text to append.</param>
    public void Raw(string text)
    {
        Prefix(text);
    }
}