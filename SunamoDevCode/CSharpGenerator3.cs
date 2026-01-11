// variables names: ok
namespace SunamoDevCode;

public partial class CSharpGenerator : GeneratorCodeAbstract //, ICSharpGenerator
{
    /// <summary>
    /// EN: Generates a Dictionary from another dictionary. Args.AddingValue = true.
    /// CZ: Generuje Dictionary z jiného dictionary. Args.AddingValue = true.
    /// </summary>
    /// <typeparam name="Key">Type of dictionary keys</typeparam>
    /// <typeparam name="Value">Type of dictionary values</typeparam>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="dictionaryName">Name of the dictionary variable</param>
    /// <param name="dictionary">Source dictionary</param>
    /// <param name="args">Generator arguments (AddingValue = true)</param>
    public void DictionaryFromDictionary<Key, Value>(int tabCount, string dictionaryName, Dictionary<Key, Value> dictionary, CSharpGeneratorArgs? args = null) where Key : notnull
    {
        if (args == null)
            args = new CSharpGeneratorArgs();
        string valueType = null;
        if (dictionary.Count > 0)
            valueType = ConvertTypeShortcutFullName.ToShortcut(DictionaryHelper.GetFirstItemValue(dictionary).GetType().FullName);
        var className = "Dictionary<string, " + valueType + ">"; //
        args.CreateInstance = false;
        NewVariable(tabCount, AccessModifiers.Private, className, dictionaryName, args);
        AppendLine();
        CreateInstance(className, dictionaryName);
        if (args.AddingValue)
            GetDictionaryValuesFromDictionary(tabCount, dictionaryName, dictionary);
    }

    /// <summary>
    /// EN: Generates a Dictionary with List values from another dictionary. Default: addingValue = true.
    /// CZ: Generuje Dictionary s hodnotami List z jiného dictionary. Výchozí: addingValue = true.
    /// </summary>
    /// <typeparam name="Key">Type of dictionary keys</typeparam>
    /// <typeparam name="Value">Type of list values</typeparam>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="dictionaryName">Name of the dictionary variable</param>
    /// <param name="dictionary">Source dictionary</param>
    /// <param name="args">Generator arguments</param>
    public void DictionaryFromDictionaryInnerList<Key, Value>(int tabCount, string dictionaryName, Dictionary<Key, Value> dictionary, CSharpGeneratorArgs args) where Key : notnull
    {
        string valueType = null;
        if (dictionary.Count > 0)
            valueType = ConvertTypeShortcutFullName.ToShortcut(DictionaryHelper.GetFirstItemValue(dictionary).GetType().FullName);
        var className = "Dictionary<string, List<" + valueType + ">>"; //
        NewVariable(tabCount, AccessModifiers.Private, className, dictionaryName, args);
        AppendLine();
        CreateInstance(className, dictionaryName);
        if (args.AddingValue)
            GetDictionaryValuesFromDictionary(tabCount, dictionaryName, dictionary);
    }

    /// <summary>
    /// EN: Generates dictionary value assignments from random values
    /// CZ: Generuje přiřazení hodnot do dictionary z náhodných hodnot
    /// </summary>
    /// <typeparam name="Key">Type of dictionary keys</typeparam>
    /// <typeparam name="Value">Type of dictionary values</typeparam>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="dictionaryName">Name of the dictionary variable</param>
    /// <param name="keys">List of keys</param>
    /// <param name="randomValueGenerator">Function to generate random values</param>
    public void GetDictionaryValuesFromRandomValue<Key, Value>(int tabCount, string dictionaryName, List<Key> keys, Func<Value> randomValueGenerator) where Key : notnull
    {
        var dictionary = new Dictionary<Key, Value>();
        for (var i = 0; i < keys.Count; i++)
            dictionary.Add(keys[i], randomValueGenerator.Invoke());
        GetDictionaryValuesFromDictionary(tabCount, dictionaryName, dictionary);
    }

    /// <summary>
    /// EN: Generates dictionary value assignments from two lists. Args.SplitKeyWith allows splitting keys.
    /// CZ: Generuje přiřazení hodnot do dictionary ze dvou seznamů. Args.SplitKeyWith umožňuje rozdělení klíčů.
    /// </summary>
    /// <typeparam name="Key">Type of dictionary keys</typeparam>
    /// <typeparam name="Value">Type of dictionary values</typeparam>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="dictionaryName">Name of the dictionary variable</param>
    /// <param name="keys">List of keys</param>
    /// <param name="values">List of values</param>
    /// <param name="args">Generator arguments (SplitKeyWith, etc.)</param>
    public void GetDictionaryValuesFromTwoList<Key, Value>(int tabCount, string dictionaryName, List<Key> keys, List<Value> values, CSharpGeneratorArgs args) where Key : notnull
    {
        var shouldSplitKeys = false;
        string delimiter = null;
        if (args.SplitKeyWith != null)
        {
            if (typeof(Key) == Types.StringType)
            {
                shouldSplitKeys = true;
                delimiter = args.SplitKeyWith;
            }
            else
            {
                throw new Exception("App want to split key but key is not string");
            }
        }

        ThrowEx.DifferentCountInListsTU("keys", keys, "values", values);
        var dictionary = new Dictionary<Key, Value>();
        for (var i = 0; i < keys.Count; i++)
            if (shouldSplitKeys)
            {
                var splitKeys = keys[i].ToString().Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries); //SHSplit.Split(, delimiter);
                foreach (var item in splitKeys)
                    dictionary.Add((Key)(dynamic)item, values[i]);
            }
            else
            {
                dictionary.Add(keys[i], values[i]);
            }

        GetDictionaryValuesFromDictionary(tabCount, dictionaryName, dictionary);
    }

    /// <summary>
    /// EN: Generates dictionary value assignments from a dictionary with List values
    /// CZ: Generuje přiřazení hodnot do dictionary ze slovníku s hodnotami List
    /// </summary>
    /// <typeparam name="Key">Type of dictionary keys</typeparam>
    /// <typeparam name="Value">Type of list values</typeparam>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="dictionaryName">Name of the dictionary variable</param>
    /// <param name="dictionary">Source dictionary</param>
    /// <param name="args">Generator arguments (AlsoField, UseCA, etc.)</param>
    public void GetDictionaryValuesFromDictionaryInnerList<Key, Value>(int tabCount, string dictionaryName, Dictionary<Key, List<Value>> dictionary, CSharpGeneratorArgs args) where Key : notnull
    {
        Type keyType, valueType;
        Key firstKey;
        GetTKeyAndTValue(dictionary, out keyType, out valueType, out firstKey);
        IList valueList;
        string keyText = null;
        keyText = firstKey.ToString();
        if (args.AlsoField)
            Field(tabCount, AccessModifiers.Public, true, VariableModifiers.None, string.Format("Dictionary<{0}, List<{1}>>", keyType.Name, valueType.Name), dictionaryName, true);
        string valuesCSharp = null;
        foreach (var item in dictionary)
        {
            keyText = item.Key.ToString();
            valueList = item.Value;
            CSharpHelper.WrapWithQuote(keyType, ref keyText);
            var formattedValues = CSharpHelper.WrapWithQuoteList(valueType, valueList);
            if (args.UseCA)
                valuesCSharp = "[" + formattedValues + "]";
            else
                valuesCSharp = "new List<" + valueType.Name + ">(new ;" + valueType.Name + "[] {" + formattedValues + "})";
            AppendLine(tabCount, dictionaryName + ".Add(" + keyText + ", " + valuesCSharp + ");");
        }
    }

    /// <summary>
    /// EN: Extracts key and value types from a dictionary with list values
    /// CZ: Získá typy klíče a hodnoty ze slovníku s hodnotami typu list
    /// </summary>
    /// <typeparam name="Key">Type of dictionary keys</typeparam>
    /// <typeparam name="Value">Type of list values</typeparam>
    /// <param name="dictionary">Source dictionary</param>
    /// <param name="keyType">Output: key type</param>
    /// <param name="valueType">Output: value type</param>
    /// <param name="firstKey">Output: first key from dictionary</param>
    public static void GetTKeyAndTValue<Key, Value>(Dictionary<Key, List<Value>> dictionary, out Type keyType, out Type valueType, out Key firstKey) where Key : notnull
    {
        firstKey = default;
        var firstValue = default(Value);
        foreach (var item in dictionary)
        {
            firstKey = item.Key;
            firstValue = (Value)item.Value.FirstOrDefault();
            break;
        }

        keyType = firstKey.GetType();
        valueType = firstValue.GetType();
    }

    /// <summary>
    /// EN: Generates dictionary value assignments from a dictionary
    /// CZ: Generuje přiřazení hodnot do dictionary ze slovníku
    /// </summary>
    /// <typeparam name="Key">Type of dictionary keys</typeparam>
    /// <typeparam name="Value">Type of dictionary values</typeparam>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="dictionaryName">Name of the dictionary variable</param>
    /// <param name="dictionary">Source dictionary</param>
    public void GetDictionaryValuesFromDictionary<Key, Value>(int tabCount, string dictionaryName, Dictionary<Key, Value> dictionary) where Key : notnull
    {
        var firstKey = default(Key);
        var firstValue = default(Value);
        foreach (var item in dictionary)
        {
            firstKey = item.Key;
            firstValue = item.Value;
            break;
        }

        var keyType = firstKey.GetType();
        var valueType = firstValue.GetType();
        string valueText, keyText = null;
        valueText = firstValue.ToString();
        keyText = firstKey.ToString();
        foreach (var item in dictionary)
        {
            keyText = item.Key.ToString();
            valueText = item.Value.ToString();
            CSharpHelper.WrapWithQuote(keyType, ref keyText);
            CSharpHelper.WrapWithQuote(valueType, ref valueText);
            AppendLine(tabCount, dictionaryName + ".Add(" + keyText + ", " + valueText + ");");
        }
    }

    /// <summary>
    /// EN: Generates a Dictionary&lt;string, object&gt; with values. Args.AddingValue = true as default.
    /// CZ: Generuje Dictionary&lt;string, object&gt; s hodnotami. Args.AddingValue = true jako výchozí.
    /// </summary>
    /// <typeparam name="Value">Type of dictionary values</typeparam>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="dictionaryName">Name of the dictionary variable</param>
    /// <param name="dictionary">Source dictionary</param>
    /// <param name="args">Generator arguments (AddingValue = true as default)</param>
    public void DictionaryStringObject<Value>(int tabCount, string dictionaryName, Dictionary<string, Value> dictionary, CSharpGeneratorArgs args)
    {
        DictionaryFromDictionary(tabCount, dictionaryName, dictionary, args);
    }
}
