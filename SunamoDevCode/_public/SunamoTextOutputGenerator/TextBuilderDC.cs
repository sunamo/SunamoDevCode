namespace SunamoDevCode._public.SunamoTextOutputGenerator;

/// <summary>
/// Text builder that supports building strings using either a StringBuilder or a List of lines, with optional undo functionality.
/// </summary>
public class TextBuilderDC
{
    private static Type type = typeof(TextBuilderDC);
    private bool _canUndo = false;
    private int _lastIndex = -1;
    private string _lastText = "";
    /// <summary>
    /// The underlying StringBuilder used when not in list mode.
    /// </summary>
    public StringBuilder stringBuilder = null!;
    /// <summary>
    /// Gets or sets text to prepend before every non-whitespace append operation.
    /// </summary>
    public string prependEveryNoWhite { get; set; } = string.Empty;



    /// <summary>
    /// Gets or sets the list used for line-based building when in list mode.
    /// </summary>
    public List<string> list { get; set; } = null!;
    private bool _useList = false;
    /// <summary>
    /// Clears all accumulated text or lines.
    /// </summary>
    public void Clear()
    {
        if (_useList)
        {
            list.Clear();
        }
        else
        {
            stringBuilder.Clear();
        }
    }
    /// <summary>
    /// Creates a new TextBuilderDC instance with the specified mode.
    /// </summary>
    /// <param name="useList">If true, uses a list of lines instead of StringBuilder.</param>
    /// <returns>A new TextBuilderDC instance.</returns>
    public static TextBuilderDC Create(bool useList = false)
    {
        return new TextBuilderDC(useList);
    }







    /// <summary>
    /// Initializes a new TextBuilderDC using either a list or StringBuilder based on the parameter.
    /// </summary>
    /// <param name="useList">If true, uses a list of lines instead of StringBuilder.</param>
    public TextBuilderDC(bool useList = false)
    {
        _useList = useList;
        if (useList)
        {
            list = new List<string>();
        }
        else
        {
            stringBuilder = new StringBuilder();
        }
    }
    /// <summary>
    /// Gets or sets whether undo is enabled. Setting to false clears the undo state.
    /// </summary>
    public bool CanUndo
    {
        get
        {
            if (_useList)
            {
                return false;
            }
            return _canUndo;
        }
        set
        {
            _canUndo = value;
            if (!value)
            {
                _lastIndex = -1;
                _lastText = "";
            }
        }
    }
    private void UndoIsNotAllowed(string what)
    {
        ThrowEx.IsNotAllowed(what);
    }
    /// <summary>
    /// Undoes the last append operation by removing the last appended text from the StringBuilder.
    /// </summary>
    public void Undo()
    {
        if (_useList)
        {
            UndoIsNotAllowed("Undo");
        }
        if (_lastIndex != -1)
        {
            stringBuilder.Remove(_lastIndex, _lastText.Length);
        }
    }
    /// <summary>
    /// Appends text to the current line or to the StringBuilder.
    /// </summary>
    /// <param name="text">Text to append.</param>
    public void Append(string text)
    {
        if (_useList)
        {
            if (list.Count > 0)
            {
                list[list.Count - 1] += text;
            }
            else
            {
                list.Add(text);
            }
        }
        else
        {
            SetUndo(text);
            stringBuilder.Append(prependEveryNoWhite);
            stringBuilder.Append(text);
        }
    }
    private void SetUndo(string text)
    {
        if (_useList)
        {
            UndoIsNotAllowed("SetUndo");
        }
        if (CanUndo)
        {
            _lastIndex = stringBuilder.Length;
            _lastText = text;
        }
    }
    /// <summary>
    /// Appends the string representation of an object.
    /// </summary>
    /// <param name="text">Object whose string representation to append.</param>
    public void Append(object text)
    {
        string textString = text.ToString()!;
        SetUndo(textString);
        Append(textString);
    }
    /// <summary>
    /// Appends a new line to the output.
    /// </summary>
    public void AppendLine()
    {
        Append(Environment.NewLine);
    }
    /// <summary>
    /// Appends text followed by a new line.
    /// </summary>
    /// <param name="text">Text to append before the new line.</param>
    public void AppendLine(string text)
    {
        if (_useList)
        {
            list.Add(prependEveryNoWhite + text);
        }
        else
        {
            SetUndo(text);
            stringBuilder.Append(prependEveryNoWhite + text + Environment.NewLine);
        }
    }




    /// <summary>
    /// Returns the accumulated text as a single string.
    /// </summary>
    /// <returns>All accumulated text joined together.</returns>
    public override string ToString()
    {
        if (_useList)
        {
            return string.Join(Environment.NewLine, list);
        }
        else
        {
            return stringBuilder.ToString();
        }
    }
}