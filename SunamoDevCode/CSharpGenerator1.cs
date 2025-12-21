namespace SunamoDevCode;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public partial class CSharpGenerator : GeneratorCodeAbstract //, ICSharpGenerator
{
    /// <summary>
    ///     Do A1 byly uloženy v pořadí typ, název, typ, název
    ///     Statický konstruktor zde nevytvoříte
    /// </summary>
    /// <param name = "tableName"></param>
    /// <param name = "autoAssing"></param>
    /// <param name = "args"></param>
    public void Ctor(int tabCount, ModifiersConstructor mc, string ctorName, bool autoAssing, bool isBase, params string[] args)
    {
        AddTab(tabCount);
        var sb2 = new StringBuilder(mc.ToString());
        sb2[0] = char.ToLower(sb2[0]);
        sb.AddItem(sb2.ToString());
        sb.AddItem(ctorName);
        StartParenthesis();
        var nazevParams = new List<string>(args.Length / 2);
        for (var i = 0; i < args.Length; i++)
        {
            sb.AddItem(args[i]);
            var nazevParam = args[++i];
            nazevParams.Add(nazevParam);
            if (i != args.Length - 1)
                sb.AddItem(nazevParam + ",");
            else
                sb.AddItem(nazevParam);
        }

        EndParenthesis();
        if (!isBase)
            if (nazevParams.Count > 0)
                sb.AddItem(": base(" + string.Join(',', nazevParams.ToArray()) + ")");
        StartBrace(tabCount);
        if (autoAssing && isBase)
            foreach (var item in nazevParams)
            {
                This(tabCount, item);
                sb.AddItem("=");
                sb.AddItem(item + ";");
                sb.AppendLine();
            }

        EndBrace(tabCount);
        sb.AppendLine();
    }

    /// <summary>
    ///     _get, _set can be string or bool
    /// </summary>
    /// <param name = "tabCount"></param>
    /// <param name = "_public"></param>
    /// <param name = "_static"></param>
    /// <param name = "returnType"></param>
    /// <param name = "name"></param>
    /// <param name = "_get"></param>
    /// <param name = "_set"></param>
    /// <param name = "field"></param>
    public void Property(int tabCount, AccessModifiers _public, bool _static, string returnType, string name, object _get, object _set, string field, bool shortGet, bool shortSet)
    {
#region MyRegion
        AddTab(tabCount);
        PublicStatic(_public, _static);
#endregion
        ReturnTypeName(returnType, name);
        AddTab(tabCount);
        if (shortGet && shortSet)
            sb.AddItem("{");
        else
            StartBrace(tabCount);
        var settedGet = !(_get == null || _get.ToString() == false.ToString());
        if (settedGet)
        {
            if (shortGet)
                throw new Exception("Can't be set shortGet and _get in one time");
            var text = _get.ToString();
            AddTab(tabCount + 1);
            sb.AddItem("get");
            StartBrace(tabCount + 1);
            AddTab(tabCount + 2);
            if (text == true.ToString())
                sb.AddItem("return " + field + ";");
            else
                sb.AddItem(text);
            sb.AppendLine();
            EndBrace(tabCount + 1);
        }

        var settedSet = !(_set == null || _set.ToString() == false.ToString());
        if (settedSet)
        {
            if (shortSet)
                throw new Exception("Can't be set shortSet and _set in one time");
            AddTab(tabCount + 1);
            sb.AddItem("set");
            StartBrace(tabCount + 1);
            AddTab(tabCount + 2);
            var text = _set.ToString();
            if (text == true.ToString())
                sb.AddItem(field + " = value;");
            else
                sb.AddItem(text);
            sb.AppendLine();
            EndBrace(tabCount + 1);
        }

        if (shortGet)
        {
            if (settedSet)
                throw new Exception("Can't be set shortGet and _get in one time");
            sb.AddItem("get;");
        }

        if (shortSet)
        {
            if (settedGet)
                throw new Exception("Can't be set shortGet and _get in one time");
            sb.AddItem("set;");
        }

        EndBrace(tabCount);
        sb.AppendLine();
    }

    /// <summary>
    ///     A6 inner již musí býy odsazené pro tuto metod
    /// </summary>
    /// <param name = "_public"></param>
    /// <param name = "_static"></param>
    /// <param name = "returnType"></param>
    /// <param name = "name"></param>
    /// <param name = "inner"></param>
    /// <param name = "args"></param>
    public void Method(int tabCount, AccessModifiers _public, bool _static, string returnType, string name, string inner, string args)
    {
        AddTab(tabCount);
        PublicStatic(_public, _static);
        ReturnTypeName(returnType, name);
        StartParenthesis();
        sb.AddItem(args);
        EndParenthesis();
        AppendLine();
        StartBrace(tabCount);
        AddTab(tabCount + 1);
        sb.AddItem(inner);
        sb.AppendLine();
        EndBrace(tabCount);
        sb.AppendLine();
    }

    private void ReturnTypeName(string returnType, string name)
    {
        sb.AddItem(returnType);
        sb.AddItem(name);
    }

    public void Method(int tabCount, string header, string inner)
    {
        AddTab(tabCount);
        sb.AddItem(header);
        StartBrace(tabCount);
        //AddTab(tabCount + 1);
        sb.AddItem(inner);
        sb.AppendLine("");
        EndBrace(tabCount);
        sb.AppendLine();
    }

    /// <param name = "usings"></param>
    public void Using(string usings)
    {
        if (!usings.StartsWith("using "))
            usings = "using " + usings + ";";
        else if (!usings.Trim().EndsWith(";"))
            usings += ";";
        sb.AddItem(usings);
        sb.AppendLine();
    }

    /// <summary>
    ///     Pokud chceš nový řádek bez jakéhokoliv textu, zadej například 2, ""
    ///     Nepoužívej na to metodu jen text tabCount, protože ji pak IntelliSense nevidělo.
    /// </summary>
    /// <param name = "tabCount"></param>
    /// <param name = "p"></param>
    /// <param name = "p2"></param>
    /// <summary>
    ///     Automaticky doplní počáteční závorku
    /// </summary>
    /// <param name = "podminka"></param>
    public void If(int tabCount, string podminka)
    {
        AddTab(tabCount);
        sb.AppendLine("if(" + podminka + ")");
        StartBrace(tabCount);
    }

    /// <summary>
    ///     Automaticky doplní počáteční závorku
    /// </summary>
    public void Else(int tabCount)
    {
        AddTab(tabCount);
        sb.AppendLine("else");
        StartBrace(tabCount);
    }

    public void EnumWithComments(int tabCount, AccessModifiers _public, string nameEnum, Dictionary<string, string> nameCommentEnums)
    {
        WriteAccessModifiers(_public);
        AddTab(tabCount);
        sb.AddItem("enum " + nameEnum);
        StartBrace(tabCount);
        foreach (var item in nameCommentEnums)
        {
            XmlSummary(tabCount + 1, item.Value);
            AppendLine(tabCount + 1, item.Key + ",");
        }

        EndBrace(tabCount);
    }

    private void AppendAttribute(int tabCount, string name, string inParentheses)
    {
        var zav = "";
        if (inParentheses != null)
            zav = "(" + inParentheses + ")";
        AppendLine(tabCount, "[" + name + zav + "]");
    }
}