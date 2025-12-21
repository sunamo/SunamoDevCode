namespace SunamoDevCode;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public partial class CSharpGenerator : GeneratorCodeAbstract //, ICSharpGenerator
{
    private static Type type = typeof(CSharpGenerator);
    //public int Length => sb.Length;
    public void StartClass(int tabCount, AccessModifiers _public, bool _static, string className, params string[] derive)
    {
        AddTab(tabCount);
        PublicStatic(_public, _static);
        sb.AddItem(" class " + className);
        if (derive.Length != 0)
        {
            sb.AddItem(":");
            for (var i = 0; i < derive.Length - 1; i++)
                sb.AddItem(derive[i] + ",");
            sb.AddItem(derive[derive.Length - 1]);
        }

        StartBrace(tabCount);
    }

    private void PublicStatic(AccessModifiers _public, bool _static)
    {
        WriteAccessModifiers(_public);
        if (_static)
            sb.AddItem("static");
    }

    private void WriteAccessModifiers(AccessModifiers _public)
    {
        var methodName = "WriteAccessModifiers";
        if (_public == AccessModifiers.Public)
        {
            sb.AddItem("public");
        }
        else if (_public == AccessModifiers.Protected)
        {
            sb.AddItem("protected");
        }
        else if (_public == AccessModifiers.Private)
        {
        //sb.AddItem("private");
        }
        else if (_public == AccessModifiers.Internal)
        {
            sb.AddItem("public");
        }
        else
        {
            ThrowEx.NotImplementedCase(_public);
        }
    }

    public void EndRegion(int tabCount)
    {
        AppendLine(tabCount, "#endregion");
    }

    public void Region(int tabCount, string v)
    {
        AppendLine(tabCount, "#region " + v);
    }

    public void Attribute(int tabCount, string name, string attrs)
    {
        AddTab(tabCount);
        sb.AppendLine("[" + name + "(" + attrs + ")]");
    }

    public static List<EnumItem> CreateEnumItemsFromList(List<string> list)
    {
        var ei = new List<EnumItem>(list.Count);
        foreach (var item in list)
            ei.Add(new EnumItem { Name = item });
        return ei;
    }

    public void Field(int tabCount, AccessModifiers _public, bool _static, VariableModifiers variableModifiers, string type, string name, bool addHyphensToValue, string value)
    {
        var oio = ObjectInitializationOptions.Original;
        if (addHyphensToValue)
            oio = ObjectInitializationOptions.Hyphens;
        Field(tabCount, _public, _static, variableModifiers, type, name, oio, value);
    }

    /// <summary>
    ///     Pokud do A2 zadáš Private tak se jednoduše žádný modifikátor nepřidá - to proto že se může jednat o vnitřek metody
    ///     atd.
    ///     A1 se bude ignorovat pokud v A7 bude NewAssign
    ///     Do A8 value se nesmí vkládal null, program by havaroval
    ///     If A7 NewAssign, put into A8 string.Empty => constructor will be empty
    ///     If A7 Original, put into A8 part after =
    /// </summary>
    /// <param name = "tabCount"></param>
    /// <param name = "_public"></param>
    /// <param name = "_static"></param>
    /// <param name = "variableModifiers"></param>
    /// <param name = "type"></param>
    /// <param name = "name"></param>
    /// <param name = "oio"></param>
    /// <param name = "value"></param>
    public void Field(int tabCount, AccessModifiers _public, bool _static, VariableModifiers variableModifiers, string type, string name, ObjectInitializationOptions oio, string value)
    {
        AddTab(tabCount);
        ModificatorsField(_public, _static, variableModifiers);
        ReturnTypeName(type, name);
        sb.AddItem("=");
        if (oio == ObjectInitializationOptions.Hyphens)
            value = "\"" + value + "\"";
        else if (oio == ObjectInitializationOptions.NewAssign)
            value = "new " + type + "()";
        var text = value + ";";
        // zde mi to nevysvětlitelně - dokud jsem v AddItem tak hodnotu má
        // jen co se z něj dostanu tak je empty
        // dělá to i když vložím jiný text
        // nefungovalo to ani když jsem vložil přímoo do sb bez InstantSb
        sb.AddItem(text);
        //}
        sb.AppendLine();
    }

    public void Field(int tabCount, AccessModifiers _public, bool _static, VariableModifiers variableModifiers, string type, string name, bool defaultValue)
    {
        AddTab(tabCount);
        ModificatorsField(_public, _static, variableModifiers);
        ReturnTypeName(type, name);
        DefaultValue(type, defaultValue);
        sb.RemoveEndDelimiter();
        sb.AddItem(";");
        sb.AppendLine();
    //this.sb.AddItem(sb.ToString());
    }

    /// <summary>
    ///     Use method CSharpHelper.DefaultValueForType
    /// </summary>
    /// <param name = "type"></param>
    /// <param name = "defaultValue"></param>
    private void DefaultValue(string type, bool defaultValue)
    {
        if (defaultValue)
        {
            sb.AddItem("=");
            sb.AddItem(CSharpHelperSunamo.DefaultValueForType(type, ConvertTypeShortcutFullName.ToShortcut));
        }
    }

    public static List<string> AddIntoClass(List<string> contentFileNew, List<string> insertedLines, out int classIndex, string ns)
    {
        // index line with class
        classIndex = -1;
        // whether im after {
        var cl = false;
        var lsf = false;
        for (var i = 0; i < contentFileNew.Count; i++)
            if (!cl)
            {
                // can be public / public partial
                if (contentFileNew[i].Contains(" class "))
                {
                    contentFileNew[i] = contentFileNew[i].Replace(ns + "Page", "Page");
                    classIndex = i;
                    cl = true;
                }
            }
            else if (cl && !lsf)
            {
                if (contentFileNew[i].Contains("{"))
                    lsf = true;
            }
            else if (cl && lsf)
            {
                if (contentFileNew[i].Contains("}"))
                {
                    contentFileNew.InsertRange(i, insertedLines);
                    break;
                }
            }

        return contentFileNew;
    }

    public void Namespace(string ns)
    {
        sb.AddItem("namespace" + " " + ns);
        sb.AppendLine();
        sb.AddItem("{");
        sb.AppendLine();
    }

    private void ModificatorsField(AccessModifiers _public, bool _static, VariableModifiers variableModifiers)
    {
        WriteAccessModifiers(_public);
        if (variableModifiers == VariableModifiers.Mapped)
        {
            sb.AddItem("const");
        }
        else
        {
            if (_static && variableModifiers == VariableModifiers.ReadOnly)
            {
                sb.AddItem("const");
            }
            else
            {
                if (_static)
                    sb.AddItem("static");
                if (variableModifiers == VariableModifiers.ReadOnly)
                    sb.AddItem("readonly");
            }
        }
    }

    /// <summary>
    ///     NI
    /// </summary>
    /// <param name = "tableName"></param>
    /// <param name = "inner"></param>
    /// <param name = "args"></param>
    public void Ctor(int tabCount, ModifiersConstructor mc, string ctorName, string inner, params string[] args)
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
        StartBrace(tabCount);
        //sb.AppendLine();
        Append(tabCount + 1, inner);
        EndBrace(tabCount - 2);
        sb.AppendLine();
    }
}