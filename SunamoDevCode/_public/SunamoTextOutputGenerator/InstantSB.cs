namespace SunamoDevCode._public.SunamoTextOutputGenerator;

/// <summary>
/// String builder that automatically separates appended items with a specified delimiter.
/// </summary>
public class InstantSB
{
    /// <summary>
    /// Gets or sets the underlying StringBuilder used for building output.
    /// </summary>
    public StringBuilder StringBuilder { get; set; } = new StringBuilder();
    private string _tokensDelimiter;
    /// <summary>
    /// Initializes a new InstantSB with the specified delimiter between items.
    /// </summary>
    /// <param name="delimiter">Delimiter string to insert between items.</param>
    public InstantSB(string delimiter)
    {
        _tokensDelimiter = delimiter;
    }
    /// <summary>
    /// Gets the current length of the accumulated output.
    /// </summary>
    public int Length => StringBuilder.Length;
    /// <summary>
    /// Returns the accumulated output as a string.
    /// </summary>
    /// <returns>The built string.</returns>
    public override string ToString()
    {
        string result = StringBuilder.ToString();
        return result;
    }




    /// <summary>
    /// Adds a value followed by the delimiter, skipping if the value equals the delimiter or is empty.
    /// </summary>
    /// <param name="value">Value to add.</param>
    public void AddItem(string value)
    {
        string text = value.ToString();
        if (text != _tokensDelimiter && text != "")
        {
            StringBuilder.Append(text + _tokensDelimiter);
        }
    }
    /// <summary>
    /// Adds the string representation of an object without any delimiter.
    /// </summary>
    /// <param name="content">Object whose string representation to append.</param>
    public void AddRaw(object content)
    {
        StringBuilder.Append(content.ToString());
    }

    /// <summary>
    /// Adds multiple items, each followed by the delimiter.
    /// </summary>
    /// <param name="items">Items to add.</param>
    public void AddItems(params string[] items)
    {
        foreach (var item in items)
        {
            AddItem(item);
        }
    }




    /// <summary>
    /// Adds content without delimiter, typically used at the end of a line.
    /// </summary>
    /// <param name="content">Content to append.</param>
    public void EndLine(object content)
    {
        string text = content.ToString()!;
        if (text != _tokensDelimiter && text != "")
        {
            StringBuilder.Append(text);
        }
    }




    /// <summary>
    /// Appends text followed by a newline.
    /// </summary>
    /// <param name="text">Text to append.</param>
    public void AppendLine(string text)
    {
        EndLine(text + Environment.NewLine);
    }
    /// <summary>
    /// Appends a newline.
    /// </summary>
    public void AppendLine()
    {
        EndLine(Environment.NewLine);
    }
    /// <summary>
    /// Removes the trailing delimiter from the accumulated output.
    /// </summary>
    public void RemoveEndDelimiter()
    {
        StringBuilder.Remove(StringBuilder.Length - _tokensDelimiter.Length, _tokensDelimiter.Length);
    }
    /// <summary>
    /// Clears all accumulated output.
    /// </summary>
    public void Clear()
    {
        StringBuilder.Clear();
    }
}