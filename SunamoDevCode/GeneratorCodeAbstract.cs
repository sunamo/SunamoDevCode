// variables names: ok
namespace SunamoDevCode;

public abstract class GeneratorCodeAbstract
{
    /// <summary>
    ///     Use ToString() instead of public access
    /// </summary>
    protected string Final = "";

    protected InstantSB sb = new(" ");
    public XmlDoc xmlDoc;

    public GeneratorCodeAbstract()
    {
        xmlDoc = new XmlDoc(sb);
    }

    public void AddTab2(int tabCount, string text)
    {
        sb.AddItem(AddTab(tabCount, text));
    }

    /// <summary>
    /// EN: Ends a brace and adds a new line. After calling this method for elements like methods, properties, or constructors, it's recommended to also call sb.AppendLine().
    /// CZ: Ukončí složenou závorku a přidá nový řádek. Za voláním této metody pokud ukončuje nějaký celek jako jsou metody, vlastnosti nebo konstruktor je vhodné volat ještě sb.AppendLine().
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    public void EndBrace(int tabCount)
    {
        //sb.AppendLine();
        AddTab(tabCount);
        //sb.AppendLine();
        sb.AppendLine("}");
    }

    /// <summary>
    /// EN: Starts a brace. This is the only method here that adds a new line at the beginning.
    /// CZ: Přidá nový řádek, složenou závorku. Je to jediná zdejší metoda která na začátku přidává nový řádek.
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    public void StartBrace(int tabCount)
    {
        // Line always ending previous command
        //sb.AppendLine();
        AddTab(tabCount);
        sb.AppendLine("{");
        //sb.AppendLine();
    }

    public void StartParenthesis()
    {
        sb.AddItem("(");
    }

    public void EndParenthesis()
    {
        sb.AddItem(")");
    }

    public void AppendLine()
    {
        sb.AppendLine();
    }

    /// <summary>
    /// EN: Appends a formatted line with tabs
    /// CZ: Přidá formátovaný řádek s tabulátory
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="format">Format string or plain text</param>
    /// <param name="args">Format arguments</param>
    public void AppendLine(int tabCount, string format, params object[] args)
    {
        if (args.Length != 0)
            sb.AppendLine(AddTab(tabCount, string.Format(format, args)));
        else
            sb.AppendLine(AddTab(tabCount, format));
    }

    /// <summary>
    /// EN: Appends formatted text with tabs (without newline)
    /// CZ: Přidá formátovaný text s tabulátory (bez nového řádku)
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="format">Format string or plain text</param>
    /// <param name="args">Format arguments</param>
    public void Append(int tabCount, string format, params object[] args)
    {
        if (args.Length != 0)
            sb.AddItem(AddTab(tabCount, string.Format(format, args)));
        else
            sb.AddItem(AddTab(tabCount, format));
        var stringBuilderContent = sb.ToString();
    }

    /// <summary>
    /// EN: Assigns a value to a property/field, converting it to string
    /// CZ: Přiřadí hodnotu vlastnosti/poli, převádí na string
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="objectName">Name of the object</param>
    /// <param name="variableName">Name of the variable/property</param>
    /// <param name="value">Value to assign</param>
    /// <param name="addToHyphens">Whether to add quotes</param>
    public void AssignValue(int tabCount, string objectName, string variableName, object value, bool addToHyphens)
    {
        string valueString = null;
        if (value.GetType() == typeof(bool))
            valueString = value.ToString().ToLower();
        else
            valueString = value.ToString();
        AssignValue(tabCount, objectName, variableName, valueString, addToHyphens);
    }

    /// <summary>
    /// EN: Returns the generated code and resets the string builder
    /// CZ: Vrátí vygenerovaný kód a resetuje string builder
    /// </summary>
    /// <returns>Generated code</returns>
    public override string ToString()
    {
        var result = sb.ToString();
        sb = new InstantSB(" ");
        return result;
    }

    public void AddTab(int tabCount)
    {
        //tabCount += 1;
        for (var i = 0; i < tabCount; i++) sb.AddRaw("\t");
    }

    /// <summary>
    /// EN: Adds tabs to each line of the text
    /// CZ: Přidá tabulátory na začátek každého řádku textu
    /// </summary>
    /// <param name="tabCount">Number of tabs to add</param>
    /// <param name="text">Text to process</param>
    /// <returns>Text with tabs added</returns>
    public static string AddTab(int tabCount, string text)
    {
        var lines = SHGetLines.GetLines(text);
        for (var i = 0; i < lines.Count; i++)
        {
            lines[i] = lines[i].Trim();
            for (var tabIndex = 0; tabIndex < tabCount; tabIndex++) lines[i] = "\t" + lines[i];
        }

        var result = string.Join(Environment.NewLine, lines);
        return result;
    }
}