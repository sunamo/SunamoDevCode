namespace SunamoDevCode;


/// <summary>
/// In Comparing
/// </summary>
public class TextBuilder : ITextBuilder
{
    private static Type type = typeof(TextBuilder);
    private bool _canUndo = false;
    private int _lastIndex = -1;
    private string _lastText = "";
    public StringBuilder sb = null;
    public string prependEveryNoWhite { get; set; } = string.Empty;
    /// <summary>
    /// For PowershellRunner
    /// </summary>
    public List<string> list { get; set; }
    private bool _useList = false;
    public void Clear()
    {
        if (_useList)
        {
            list.Clear();
        }
        else
        {
            sb.Clear();
        }
    }
    public static ITextBuilder Create(bool useList = false)
    {
        return new TextBuilder(useList);
    }
    /// <summary>
    /// Když někde nastavím na true, musím i zdůvodnit proč
    /// protože mi potom nefunguje sb.sb
    /// jako teď když jsem připojoval git do ps
    /// git počítal s sb ale ps s lines
    /// </summary>
    /// <param name="useList"></param>
    public TextBuilder(bool useList = false)
    {
        _useList = useList;
        if (useList)
        {
            list = new List<string>();
        }
        else
        {
            sb = new StringBuilder();
        }
    }
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
    public void Undo()
    {
        if (_useList)
        {
            UndoIsNotAllowed("Undo");
        }
        if (_lastIndex != -1)
        {
            sb.Remove(_lastIndex, _lastText.Length);
        }
    }
    public void Append(string s)
    {
        if (_useList)
        {
            if (list.Count > 0)
            {
                list[list.Count - 1] += s;
            }
            else
            {
                list.Add(s);
            }
        }
        else
        {
            SetUndo(s);
            sb.Append(prependEveryNoWhite);
            sb.Append(s);
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
            _lastIndex = sb.Length;
            _lastText = text;
        }
    }
    public void Append(object s)
    {
        string text = s.ToString();
        SetUndo(text);
        Append(text);
    }
    public void AppendLine()
    {
        Append(Environment.NewLine);
    }
    public void AppendLine(string s)
    {
        if (_useList)
        {
            list.Add(prependEveryNoWhite + s);
        }
        else
        {
            SetUndo(s);
            sb.Append(prependEveryNoWhite + s + Environment.NewLine);
        }
    }
    /// <summary>
    /// If is use List, join it with NL.
    /// Otherwise return sb
    /// </summary>
    public override string ToString()
    {
        if (_useList)
        {
            return string.Join(Environment.NewLine, list);
        }
        else
        {
            return sb.ToString();
        }
    }
}