namespace SunamoDevCode._sunamo.SunamoTextOutputGenerator;

/// <summary>
/// Text output generator for formatting structured text output.
/// </summary>
internal class TextOutputGenerator
{
    private readonly static string HeaderCharacter = "*";

    // EN: During NuGet conversion, I changed this to TextBuilderDC stringBuilder = TextBuilder.Create();
    // but that was probably a mistake, now in _sunamo I have Create() which returns null instead of using ctor
    // so I'm reverting it back.
    internal StringBuilder stringBuilder = new StringBuilder();

    #region Static texts
    #endregion
    #region Templates
    #endregion
    #region AppendLine
    /// <summary>
    /// Appends a StringBuilder content as a new line.
    /// </summary>
    /// <param name="text">The text builder content to append.</param>
    internal void AppendLine(StringBuilder text)
    {
        stringBuilder.AppendLine(text.ToString());
    }

    /// <summary>
    /// Appends text without adding a new line.
    /// </summary>
    /// <param name="text">The text to append.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Append(string text)
    {
        stringBuilder.Append(text);
    }

    /// <summary>
    /// Appends text with a new line.
    /// </summary>
    /// <param name="text">The text to append.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void AppendLine(string text)
    {
        stringBuilder.AppendLine(text);
    }
    #endregion
    #region Other adding methods
    /// <summary>
    /// Adds a header with empty lines before and after.
    /// </summary>
    /// <param name="headerText">The header text.</param>
    internal void Header(string headerText)
    {
        stringBuilder.AppendLine();
        AppendLine(headerText);
        stringBuilder.AppendLine();
    }
    #endregion
    /// <summary>
    /// Converts the generated output to a string.
    /// </summary>
    /// <returns>The complete generated text.</returns>
    public override string ToString()
    {
        var result = stringBuilder.ToString();
        return result;
    }
    #region List
    /// <summary>
    /// Adds a list with a header from StringBuilder.
    /// </summary>
    /// <param name="onlyStart">The start content.</param>
    /// <param name="headerText">The header text.</param>
    internal void ListSB(StringBuilder onlyStart, string headerText)
    {
        Header(headerText);
        AppendLine(onlyStart);
    }

    /// <summary>
    /// Adds a list of items with custom delimiter.
    /// </summary>
    /// <typeparam name="Value">Type of the list items.</typeparam>
    /// <param name="items">The list of items to output.</param>
    /// <param name="delimiter">Delimiter between items (default: CRLF).</param>
    /// <param name="whenNoEntries">Text to show when list is empty.</param>
    internal void List<Value>(IList<Value> items, string delimiter = "\r\n", string whenNoEntries = "")
    {
        if (items.Count == 0)
        {
            stringBuilder.AppendLine(whenNoEntries);
        }
        else
        {
            foreach (var item in items)
            {
                Append(item.ToString() + delimiter);
            }
        }
    }

    /// <summary>
    /// Adds a list of strings with a header.
    /// </summary>
    /// <param name="items">The list of items to output.</param>
    /// <param name="header">The header text.</param>
    internal void List(IList<string> items, string header)
    {
        List<string, string>(items, header, new TextOutputGeneratorArgs { headerWrappedEmptyLines = true, insertCount = false });
    }

    /// <summary>
    /// Adds a list with custom header type and formatting args.
    /// Use DictionaryHelper.CategoryParser for dictionary parsing.
    /// </summary>
    /// <typeparam name="Header">Type of the header (must be IEnumerable&lt;char&gt;).</typeparam>
    /// <typeparam name="Value">Type of the list items.</typeparam>
    /// <param name="items">The list of items to output.</param>
    /// <param name="header">The header text.</param>
    /// <param name="args">Arguments for formatting the output.</param>
    internal void List<Header, Value>(IList<Value> items, Header header, TextOutputGeneratorArgs args) where Header : IEnumerable<char>
    {
        if (args.headerWrappedEmptyLines)
        {
            stringBuilder.AppendLine();
        }
        stringBuilder.AppendLine(header + ":");
        if (args.headerWrappedEmptyLines)
        {
            stringBuilder.AppendLine();
        }
        List(items, args.delimiter, args.whenNoEntries);
    }
    #endregion
    #region Paragraph
    /// <summary>
    /// Adds a paragraph from StringBuilder with a header.
    /// </summary>
    /// <param name="wrongNumberOfParts">The StringBuilder containing the paragraph text.</param>
    /// <param name="header">The header text.</param>
    internal void Paragraph(StringBuilder wrongNumberOfParts, string header)
    {
        string text = wrongNumberOfParts.ToString().Trim();
        Paragraph(text, header);
    }

    /// <summary>
    /// Adds a paragraph with a header.
    /// For ordinary text use Append methods instead.
    /// </summary>
    /// <param name="text">The paragraph text.</param>
    /// <param name="header">The header text.</param>
    internal void Paragraph(string text, string header)
    {
        if (text != string.Empty)
        {
            stringBuilder.AppendLine(header + ":");
            stringBuilder.AppendLine(text);
            stringBuilder.AppendLine();
        }
    }
    #endregion
    #region Dictionary
    /// <summary>
    /// Adds a dictionary as categorized lists.
    /// </summary>
    /// <param name="dictionary">The dictionary with categories as keys and lists as values.</param>
    internal void Dictionary(Dictionary<string, List<string>> dictionary)
    {
        foreach (var item in dictionary)
        {
            List(item.Value, item.Key);
        }
    }
    #endregion
}