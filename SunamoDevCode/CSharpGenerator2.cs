namespace SunamoDevCode;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public partial class CSharpGenerator : GeneratorCodeAbstract //, ICSharpGenerator
{
    /// <summary>
    ///     addHyphens = true
    /// </summary>
    /// <param name = "tabCount"></param>
    /// <param name = "genericType"></param>
    /// <param name = "listName"></param>
    /// <param name = "list"></param>
    /// <param name = "a"></param>
    public void List(int tabCount, string genericType, string listName, List<string> list, CSharpGeneratorArgs a = null)
    {
        if (a == null)
            a = new CSharpGeneratorArgs
            {
                addHyphens = true
            };
        var cn = "List<" + genericType + ">";
        NewVariable(tabCount, AccessModifiers.Private, cn, listName, a);
        if (a.addHyphens)
            for (var i = 0; i < list.Count; i++)
                list[i] = SH.WrapWithQm(list[i]);
        //list = CA.WrapWith(list, "\"");
        if (genericType == "string")
        {
            if (a.useCA)
                AppendLine(tabCount, listName + " = new List<string>(@" + string.Join(',', list) + ");");
            else
                AppendLine(tabCount, listName + " = new List<string>(new string[] {" + string.Join(',', list) + "});");
        }
        else
        {
            if (a.useCA)
                AppendLine(tabCount, listName + " = new List<" + genericType + ">(CA.ToEnumerable(" + string.Join(',', list) + "));");
            else
                AppendLine(tabCount, listName + " = new List<" + genericType + ">(new " + genericType + "[] }" + string.Join(',', list) + "});");
        }
    }

    public void This(int tabCount, string item)
    {
        Append(tabCount, "this." + item);
    }

    /// <summary>
    ///     a: createInstance
    /// </summary>
    /// <param name = "tabCount"></param>
    /// <param name = "_public"></param>
    /// <param name = "cn"></param>
    /// <param name = "name"></param>
    /// <param name = "a"></param>
    private void NewVariable(int tabCount, AccessModifiers _public, string cn, string name, CSharpGeneratorArgs a)
    {
        AddTab2(tabCount, "");
        WriteAccessModifiers(_public);
        sb.AddItem(cn);
        sb.AddItem(name);
        if (a.createInstance)
        {
            sb.EndLine(';');
            AppendLine();
            CreateInstance(cn, name);
        }
        else
        {
            sb.AddItem("= null;");
        }

        sb.AppendLine();
    }

    private void CreateInstance(string className, string variableName)
    {
        sb.AddItem(variableName + " = new " + className + "();");
    }

    public void Enum(int tabCount, AccessModifiers _public, string nameEnum, List<EnumItem> enumItems)
    {
        WriteAccessModifiers(_public);
        AddTab(tabCount);
        sb.AddItem("enum " + nameEnum);
        StartBrace(tabCount);
        foreach (var item in enumItems)
        {
            XmlSummary(tabCount + 1, item.Comment);
            if (item.Attributes != null)
                foreach (var item2 in item.Attributes)
                    AppendAttribute(tabCount + 1, item2.Key, item2.Value);
            var hex = "";
            if (item.Hex != "")
                hex = "=" + item.Hex;
            AppendLine(tabCount + 1, item.Name + hex + ",");
        }

        EndBrace(tabCount);
    }

    private void XmlSummary(int tabCount, string summary)
    {
        if (!string.IsNullOrEmpty(summary))
        {
            AppendLine(tabCount, "/// <summary>");
            AppendLine(tabCount, "/// " + summary);
            AppendLine(tabCount, "/// </summary>");
        }
    }

    /// <summary>
    ///     A4 nepřidává do uvozovek
    /// </summary>
    /// <param name = "tabCount"></param>
    /// <param name = "objectName"></param>
    /// <param name = "variable"></param>
    /// <param name = "value"></param>
    public void AssignValue(int tabCount, string objectName, string variable, string value, CSharpGeneratorArgs a)
    {
        AddTab(tabCount);
        sb.AddItem(objectName + "." + variable);
        sb.AddItem("=");
        if (a.addHyphens)
            value = SH.WrapWith(value, "\"");
        sb.AddItem(value + ";");
        sb.AppendLine();
    }

    /// <summary>
    ///     a: addHyphens
    /// </summary>
    /// <param name = "tabCount"></param>
    /// <param name = "timeObjectName"></param>
    /// <param name = "v"></param>
    /// <param name = "type"></param>
    /// <param name = "whereIsUsed2"></param>
    /// <param name = "a"></param>
    public void AddValuesViaAddRange(int tabCount, string timeObjectName, string v, string type, List<string> whereIsUsed2, CSharpGeneratorArgs a)
    {
        var objectIdentificator = "";
        if (timeObjectName != null)
            objectIdentificator = timeObjectName + ".";
        if (a.addHyphens)
            //whereIsUsed2 = CA.WrapWith(whereIsUsed2, "\"");
            for (var i = 0; i < whereIsUsed2.Count; i++)
                whereIsUsed2[i] = SH.WrapWithBs(whereIsUsed2[i]);
        AddTab(tabCount);
        sb.AddItem(objectIdentificator + v + ".AddRange(new " + type + "[] { " + string.Join(',', whereIsUsed2) + "});");
    }

    /// <summary>
    ///     Pokud nechceš použít identifikátor objektu(například u statické třídy), vlož do A2 null
    /// </summary>
    /// <param name = "tabCount"></param>
    /// <param name = "timeObjectName"></param>
    /// <param name = "v"></param>
    /// <param name = "type"></param>
    /// <param name = "whereIsUsed2"></param>
    /// <param name = "wrapToHyphens"></param>
    public void AddValuesViaAddRange(int tabCount, string timeObjectName, string v, Type type, List<string> whereIsUsed2, CSharpGeneratorArgs a)
    {
        AddValuesViaAddRange(tabCount, timeObjectName, v, type.FullName, whereIsUsed2, a);
        sb.AppendLine();
    }

    /// <summary>
    ///     a: addingValue = true as default
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    /// <typeparam name = "U"></typeparam>
    /// <param name = "tabCount"></param>
    /// <param name = "nameDictionary"></param>
    /// <param name = "nameCommentEnums"></param>
    public void DictionaryNumberNumber<T, U>(int tabCount, string nameDictionary, Dictionary<T, U> nameCommentEnums, CSharpGeneratorArgs a = null)
    {
        DictionaryFromDictionary(tabCount, nameDictionary, nameCommentEnums, a);
    }

    /// <summary>
    ///     a: addingValue = true as default
    /// </summary>
    /// <param name = "tabCount"></param>
    /// <param name = "nameDictionary"></param>
    /// <param name = "nameCommentEnums"></param>
    public void DictionaryStringString(int tabCount, string nameDictionary, Dictionary<string, string> nameCommentEnums, CSharpGeneratorArgs a = null)
    {
        DictionaryFromDictionary(tabCount, nameDictionary, nameCommentEnums, a);
    }

    /// <summary>
    ///     createInstance = true as default
    /// </summary>
    /// <param name = "tabCount"></param>
    /// <param name = "nameDictionary"></param>
    /// <param name = "result"></param>
    /// <param name = "a"></param>
    public void DictionaryStringListString(int tabCount, string nameDictionary, Dictionary<string, List<string>> result, CSharpGeneratorArgs a = null)
    {
        if (a == null)
            a = new CSharpGeneratorArgs();
        var cn = "Dictionary<string, List<string>>";
        NewVariable(tabCount, AccessModifiers.Private, cn, nameDictionary, a);
        foreach (var item in result)
        {
            var list = CAChangeContent.ChangeContent0(null, item.Value, SH.WrapWithQm);
            AppendLine(tabCount, nameDictionary + ".Add(\"" + item.Key + "\", new List<string>(" + string.Join(",", list) + "));");
        }
    }

    /// <summary>
    ///     a: addingValue = true as default
    /// </summary>
    /// <typeparam name = "Key"></typeparam>
    /// <typeparam name = "Value"></typeparam>
    /// <param name = "tabCount"></param>
    /// <param name = "nameDictionary"></param>
    /// <param name = "keys"></param>
    /// <param name = "randomValue"></param>
    /// <param name = "addingValue"></param>
    public void DictionaryFromRandomValue<Key, Value>(int tabCount, string nameDictionary, List<Key> keys, Func<Value> randomValue, CSharpGeneratorArgs a = null)
    {
        if (a == null)
        {
            a = new CSharpGeneratorArgs();
            a.addingValue = true;
        }

        var dict = new Dictionary<Key, Value>();
        for (var i = 0; i < keys.Count; i++)
            dict.Add(keys[i], randomValue.Invoke());
        DictionaryFromDictionary(tabCount, nameDictionary, dict, a);
    }

    /// <summary>
    ///     a: addingValue = true was default
    /// </summary>
    /// <typeparam name = "Key"></typeparam>
    /// <typeparam name = "Value"></typeparam>
    /// <param name = "tabCount"></param>
    /// <param name = "nameDictionary"></param>
    /// <param name = "keys"></param>
    /// <param name = "values"></param>
    /// <param name = "addingValue"></param>
    public void DictionaryFromTwoList<Key, Value>(int tabCount, string nameDictionary, List<Key> keys, List<Value> values, CSharpGeneratorArgs a = null)
    {
        ThrowEx.DifferentCountInListsTU("keys", keys, "values", values);
        if (a == null)
            a = new CSharpGeneratorArgs();
        var dict = new Dictionary<Key, Value>();
        for (var i = 0; i < keys.Count; i++)
            dict.Add(keys[i], values[i]);
        DictionaryFromDictionary(tabCount, nameDictionary, dict, a);
    }
}