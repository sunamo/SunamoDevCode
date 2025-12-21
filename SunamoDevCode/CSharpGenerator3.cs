namespace SunamoDevCode;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public partial class CSharpGenerator : GeneratorCodeAbstract //, ICSharpGenerator
{
    /// <summary>
    ///     a: addingValue = true
    /// </summary>
    /// <typeparam name = "Key"></typeparam>
    /// <typeparam name = "Value"></typeparam>
    /// <param name = "tabCount"></param>
    /// <param name = "nameDictionary"></param>
    /// <param name = "dict"></param>
    /// <param name = "addingValue"></param>
    public void DictionaryFromDictionary<Key, Value>(int tabCount, string nameDictionary, Dictionary<Key, Value> dict, CSharpGeneratorArgs arg = null)
    {
        if (arg == null)
            arg = new CSharpGeneratorArgs();
        string valueType = null;
        if (dict.Count > 0)
            valueType = ConvertTypeShortcutFullName.ToShortcut(DictionaryHelper.GetFirstItemValue(dict).GetType().FullName);
        var cn = "Dictionary<string, " + valueType + ">"; //
        arg.createInstance = false;
        NewVariable(tabCount, AccessModifiers.Private, cn, nameDictionary, arg);
        AppendLine();
        CreateInstance(cn, nameDictionary);
        if (arg.addingValue)
            GetDictionaryValuesFromDictionary(tabCount, nameDictionary, dict);
    }

    /// <summary>
    ///     default addingValue = true
    /// </summary>
    /// <typeparam name = "Key"></typeparam>
    /// <typeparam name = "Value"></typeparam>
    /// <param name = "tabCount"></param>
    /// <param name = "nameDictionary"></param>
    /// <param name = "dict"></param>
    /// <param name = "addingValue"></param>
    public void DictionaryFromDictionaryInnerList<Key, Value>(int tabCount, string nameDictionary, Dictionary<Key, Value> dict, CSharpGeneratorArgs a)
    {
        string valueType = null;
        if (dict.Count > 0)
            valueType = ConvertTypeShortcutFullName.ToShortcut(DictionaryHelper.GetFirstItemValue(dict).GetType().FullName);
        var cn = "Dictionary<string, List<" + valueType + ">>"; //
        NewVariable(tabCount, AccessModifiers.Private, cn, nameDictionary, a);
        AppendLine();
        CreateInstance(cn, nameDictionary);
        if (a.addingValue)
            GetDictionaryValuesFromDictionary(tabCount, nameDictionary, dict);
    }

    public void GetDictionaryValuesFromRandomValue<Key, Value>(int tabCount, string nameDictionary, List<Key> keys, Func<Value> randomValue)
    {
        var dict = new Dictionary<Key, Value>();
        for (var i = 0; i < keys.Count; i++)
            dict.Add(keys[i], randomValue.Invoke());
        GetDictionaryValuesFromDictionary(tabCount, nameDictionary, dict);
    }

    /// <summary>
    ///     a: splitKeyWith
    /// </summary>
    /// <typeparam name = "Key"></typeparam>
    /// <typeparam name = "Value"></typeparam>
    /// <param name = "tabCount"></param>
    /// <param name = "nameDictionary"></param>
    /// <param name = "keys"></param>
    /// <param name = "values"></param>
    /// <param name = "a"></param>
    public void GetDictionaryValuesFromTwoList<Key, Value>(int tabCount, string nameDictionary, List<Key> keys, List<Value> values, CSharpGeneratorArgs a)
    {
        var split = false;
        string text = null;
        if (a.splitKeyWith != null)
        {
            if (typeof(Key) == Types.tString)
            {
                split = true;
                text = a.splitKeyWith;
            }
            else
            {
                throw new Exception("App want to split key but key is not string");
            }
        }

        ThrowEx.DifferentCountInListsTU("keys", keys, "values", values);
        var dict = new Dictionary<Key, Value>();
        for (var i = 0; i < keys.Count; i++)
            if (split)
            {
                var splitted = keys[i].ToString().Split(new[] { text }, StringSplitOptions.RemoveEmptyEntries); //SHSplit.Split(, text);
                foreach (var item in splitted)
                    dict.Add((Key)(dynamic)item, values[i]);
            }
            else
            {
                dict.Add(keys[i], values[i]);
            }

        GetDictionaryValuesFromDictionary(tabCount, nameDictionary, dict);
    }

    public void GetDictionaryValuesFromDictionaryInnerList<Key, Value>(int tabCount, string nameDictionary, Dictionary<Key, List<Value>> dict, CSharpGeneratorArgs a)
    {
        Type tKey, tValue;
        Key key;
        GetTKeyAndTValue(dict, out tKey, out tValue, out key);
        IList valueS;
        string keyS = null;
        keyS = key.ToString();
        if (a.alsoField)
            Field(tabCount, AccessModifiers.Public, true, VariableModifiers.None, string.Format("Dictionary<{0}, List<{1}>>", tKey.Name, tValue.Name), nameDictionary, true);
        string valuesCs = null;
        foreach (var item in dict)
        {
            keyS = item.Key.ToString();
            valueS = item.Value;
            CSharpHelper.WrapWithQuote(tKey, ref keyS);
            var valueS2 = CSharpHelper.WrapWithQuoteList(tValue, valueS);
            if (a.useCA)
                valuesCs = "[" + valueS2 + "]";
            else
                valuesCs = "new List<" + tValue.Name + ">(new ;" + tValue.Name + "[] {" + valueS2 + "})";
            AppendLine(tabCount, nameDictionary + ".Add(" + keyS + ", " + valuesCs + ");");
        }
    }

    public static void GetTKeyAndTValue<Key, Value>(Dictionary<Key, List<Value>> dict, out Type tKey, out Type tValue, out Key key)
    {
        key = default;
        var value = default(Value);
        foreach (var item in dict)
        {
            key = item.Key;
            value = (Value)item.Value.FirstOrDefault();
            break;
        }

        tKey = key.GetType();
        tValue = value.GetType();
    }

    public void GetDictionaryValuesFromDictionary<Key, Value>(int tabCount, string nameDictionary, Dictionary<Key, Value> dict)
    {
        var key = default(Key);
        var value = default(Value);
        foreach (var item in dict)
        {
            key = item.Key;
            value = item.Value;
            break;
        }

        var tKey = key.GetType();
        var tValue = value.GetType();
        string valueS, keyS = null;
        valueS = value.ToString();
        keyS = key.ToString();
        foreach (var item in dict)
        {
            keyS = item.Key.ToString();
            valueS = item.Value.ToString();
            CSharpHelper.WrapWithQuote(tKey, ref keyS);
            CSharpHelper.WrapWithQuote(tValue, ref valueS);
            AppendLine(tabCount, nameDictionary + ".Add(" + keyS + ", " + valueS + ");");
        }
    }

    /// <summary>
    ///     a: addingValue = true as default
    /// </summary>
    /// <typeparam name = "Value"></typeparam>
    /// <param name = "tabCount"></param>
    /// <param name = "nameDictionary"></param>
    /// <param name = "dict"></param>
    /// <param name = "a"></param>
    public void DictionaryStringObject<Value>(int tabCount, string nameDictionary, Dictionary<string, Value> dict, CSharpGeneratorArgs a)
    {
        DictionaryFromDictionary(tabCount, nameDictionary, dict, a);
    }
}