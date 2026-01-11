// variables names: ok
namespace SunamoDevCode;

public partial class CSharpGenerator : GeneratorCodeAbstract //, ICSharpGenerator
{
    /// <summary>
    /// EN: Generates a list initialization with specified generic type. Default: adds quotes to string values.
    /// CZ: Generuje inicializaci seznamu se zadaným generickým typem. Výchozí: přidává uvozovky k řetězcovým hodnotám.
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="genericType">Generic type parameter for the list (e.g., "string", "int")</param>
    /// <param name="listName">Name of the list variable</param>
    /// <param name="list">List of values to initialize with</param>
    /// <param name="args">Optional arguments for list generation (default: AddHyphens = true)</param>
    public void List(int tabCount, string genericType, string listName, List<string> list, CSharpGeneratorArgs? args = null)
    {
        if (args == null)
            args = new CSharpGeneratorArgs
            {
                AddHyphens = true
            };
        var cn = "List<" + genericType + ">";
        NewVariable(tabCount, AccessModifiers.Private, cn, listName, args);
        if (args.AddHyphens)
            for (var i = 0; i < list.Count; i++)
                list[i] = SH.WrapWithQm(list[i]);
        //list = CA.WrapWith(list, "\"");
        if (genericType == "string")
        {
            if (args.UseCA)
                AppendLine(tabCount, listName + " = new List<string>(@" + string.Join(',', list) + ");");
            else
                AppendLine(tabCount, listName + " = new List<string>(new string[] {" + string.Join(',', list) + "});");
        }
        else
        {
            if (args.UseCA)
                AppendLine(tabCount, listName + " = new List<" + genericType + ">(CA.ToEnumerable(" + string.Join(',', list) + "));");
            else
                AppendLine(tabCount, listName + " = new List<" + genericType + ">(new " + genericType + "[] }" + string.Join(',', list) + "});");
        }
    }

    /// <summary>
    /// EN: Generates a this.memberName reference
    /// CZ: Generuje odkaz na this.memberName
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="memberName">Name of the member to reference</param>
    public void This(int tabCount, string memberName)
    {
        Append(tabCount, "this." + memberName);
    }

    /// <summary>
    /// EN: Creates a new variable declaration. Args.CreateInstance determines whether to instantiate the object.
    /// CZ: Vytvoří deklaraci nové proměnné. Args.CreateInstance určuje, zda instancovat objekt.
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="accessModifier">Access modifier for the variable</param>
    /// <param name="className">Class name (type) of the variable</param>
    /// <param name="name">Name of the variable</param>
    /// <param name="args">Generator arguments (CreateInstance, etc.)</param>
    private void NewVariable(int tabCount, AccessModifiers accessModifier, string className, string name, CSharpGeneratorArgs args)
    {
        AddTab2(tabCount, "");
        WriteAccessModifiers(accessModifier);
        sb.AddItem(className);
        sb.AddItem(name);
        if (args.CreateInstance)
        {
            sb.EndLine(';');
            AppendLine();
            CreateInstance(className, name);
        }
        else
        {
            sb.AddItem("= null;");
        }

        sb.AppendLine();
    }

    /// <summary>
    /// EN: Generates object instantiation code (variableName = new ClassName();)
    /// CZ: Generuje kód pro vytvoření instance objektu (variableName = new ClassName();)
    /// </summary>
    /// <param name="className">Class name to instantiate</param>
    /// <param name="variableName">Variable name to assign to</param>
    private void CreateInstance(string className, string variableName)
    {
        sb.AddItem(variableName + " = new " + className + "();");
    }

    /// <summary>
    /// EN: Generates an enum with XML comments and optional attributes for each member
    /// CZ: Generuje enum s XML komentáři a volitelnými atributy pro každý člen
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="accessModifier">Access modifier for the enum</param>
    /// <param name="enumName">Name of the enum</param>
    /// <param name="enumItems">List of enum items with their properties</param>
    public void Enum(int tabCount, AccessModifiers accessModifier, string enumName, List<EnumItem> enumItems)
    {
        WriteAccessModifiers(accessModifier);
        AddTab(tabCount);
        sb.AddItem("enum " + enumName);
        StartBrace(tabCount);
        foreach (var item in enumItems)
        {
            XmlSummary(tabCount + 1, item.Comment);
            if (item.Attributes != null)
                foreach (var attribute in item.Attributes)
                    AppendAttribute(tabCount + 1, attribute.Key, attribute.Value);
            var hexValue = "";
            if (item.Hex != "")
                hexValue = "=" + item.Hex;
            AppendLine(tabCount + 1, item.Name + hexValue + ",");
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
    /// EN: Generates an assignment statement (objectName.variable = value). Args.AddHyphens determines whether to wrap value in quotes.
    /// CZ: Generuje přiřazovací příkaz (objectName.variable = value). Args.AddHyphens určuje, zda obalit hodnotu do uvozovek.
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="objectName">Name of the object</param>
    /// <param name="variableName">Name of the variable/property to assign to</param>
    /// <param name="value">Value to assign</param>
    /// <param name="args">Generator arguments (AddHyphens, etc.)</param>
    public void AssignValue(int tabCount, string objectName, string variableName, string value, CSharpGeneratorArgs args)
    {
        AddTab(tabCount);
        sb.AddItem(objectName + "." + variableName);
        sb.AddItem("=");
        if (args.AddHyphens)
            value = SH.WrapWith(value, "\"");
        sb.AddItem(value + ";");
        sb.AppendLine();
    }

    /// <summary>
    /// EN: Generates code to add multiple values to a collection via AddRange. Args.AddHyphens determines whether to wrap values in quotes.
    /// CZ: Generuje kód pro přidání více hodnot do kolekce pomocí AddRange. Args.AddHyphens určuje, zda obalit hodnoty do uvozovek.
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="objectName">Name of the object (can be null for static classes)</param>
    /// <param name="collectionName">Name of the collection to add to</param>
    /// <param name="type">Type of the array elements</param>
    /// <param name="values">Values to add to the collection</param>
    /// <param name="args">Generator arguments (AddHyphens, etc.)</param>
    public void AddValuesViaAddRange(int tabCount, string objectName, string collectionName, string type, List<string> values, CSharpGeneratorArgs args)
    {
        var objectPrefix = "";
        if (objectName != null)
            objectPrefix = objectName + ".";
        if (args.AddHyphens)
            //values = CA.WrapWith(values, "\"");
            for (var i = 0; i < values.Count; i++)
                values[i] = SH.WrapWithBs(values[i]);
        AddTab(tabCount);
        sb.AddItem(objectPrefix + collectionName + ".AddRange(new " + type + "[] { " + string.Join(',', values) + "});");
    }

    /// <summary>
    /// EN: Generates code to add multiple values to a collection via AddRange (Type overload). If you don't want to use object identifier (e.g., for static classes), pass null for objectName.
    /// CZ: Generuje kód pro přidání více hodnot do kolekce pomocí AddRange (Type přetížení). Pokud nechcete použít identifikátor objektu (např. u statické třídy), předejte null pro objectName.
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="objectName">Name of the object (can be null for static classes)</param>
    /// <param name="collectionName">Name of the collection to add to</param>
    /// <param name="type">Type of the array elements</param>
    /// <param name="values">Values to add to the collection</param>
    /// <param name="args">Generator arguments (AddHyphens, etc.)</param>
    public void AddValuesViaAddRange(int tabCount, string objectName, string collectionName, Type type, List<string> values, CSharpGeneratorArgs args)
    {
        AddValuesViaAddRange(tabCount, objectName, collectionName, type.FullName, values, args);
        sb.AppendLine();
    }

    /// <summary>
    /// EN: Generates a Dictionary with numeric key-value pairs. Args.AddingValue = true as default.
    /// CZ: Generuje Dictionary s číselnými páry klíč-hodnota. Args.AddingValue = true jako výchozí.
    /// </summary>
    /// <typeparam name="T">Type of dictionary keys</typeparam>
    /// <typeparam name="U">Type of dictionary values</typeparam>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="dictionaryName">Name of the dictionary variable</param>
    /// <param name="dictionary">Dictionary with key-value pairs</param>
    /// <param name="args">Generator arguments (AddingValue = true as default)</param>
    public void DictionaryNumberNumber<T, U>(int tabCount, string dictionaryName, Dictionary<T, U> dictionary, CSharpGeneratorArgs? args = null) where T : notnull where U : notnull
    {
        DictionaryFromDictionary(tabCount, dictionaryName, dictionary, args);
    }

    /// <summary>
    /// EN: Generates a Dictionary&lt;string, string&gt;. Args.AddingValue = true as default.
    /// CZ: Generuje Dictionary&lt;string, string&gt;. Args.AddingValue = true jako výchozí.
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="dictionaryName">Name of the dictionary variable</param>
    /// <param name="dictionary">Dictionary with string key-value pairs</param>
    /// <param name="args">Generator arguments (AddingValue = true as default)</param>
    public void DictionaryStringString(int tabCount, string dictionaryName, Dictionary<string, string> dictionary, CSharpGeneratorArgs? args = null)
    {
        DictionaryFromDictionary(tabCount, dictionaryName, dictionary, args);
    }

    /// <summary>
    /// EN: Generates a Dictionary&lt;string, List&lt;string&gt;&gt;. Args.CreateInstance = true as default.
    /// CZ: Generuje Dictionary&lt;string, List&lt;string&gt;&gt;. Args.CreateInstance = true jako výchozí.
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="dictionaryName">Name of the dictionary variable</param>
    /// <param name="dictionary">Dictionary with string keys and string list values</param>
    /// <param name="args">Generator arguments (CreateInstance = true as default)</param>
    public void DictionaryStringListString(int tabCount, string dictionaryName, Dictionary<string, List<string>> dictionary, CSharpGeneratorArgs? args = null)
    {
        if (args == null)
            args = new CSharpGeneratorArgs();
        var className = "Dictionary<string, List<string>>";
        NewVariable(tabCount, AccessModifiers.Private, className, dictionaryName, args);
        foreach (var item in dictionary)
        {
            var list = CAChangeContent.ChangeContent0(null, item.Value, SH.WrapWithQm);
            AppendLine(tabCount, dictionaryName + ".Add(\"" + item.Key + "\", new List<string>(" + string.Join(",", list) + "));");
        }
    }

    /// <summary>
    /// EN: Generates a Dictionary with keys and random values. Args.AddingValue = true as default.
    /// CZ: Generuje Dictionary s klíči a náhodnými hodnotami. Args.AddingValue = true jako výchozí.
    /// </summary>
    /// <typeparam name="Key">Type of dictionary keys</typeparam>
    /// <typeparam name="Value">Type of dictionary values</typeparam>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="dictionaryName">Name of the dictionary variable</param>
    /// <param name="keys">List of keys</param>
    /// <param name="randomValueGenerator">Function to generate random values</param>
    /// <param name="args">Generator arguments (AddingValue = true as default)</param>
    public void DictionaryFromRandomValue<Key, Value>(int tabCount, string dictionaryName, List<Key> keys, Func<Value> randomValueGenerator, CSharpGeneratorArgs? args = null)
    {
        if (args == null)
        {
            args = new CSharpGeneratorArgs();
            args.AddingValue = true;
        }

        var dict = new Dictionary<Key, Value>();
        for (var i = 0; i < keys.Count; i++)
            dict.Add(keys[i], randomValueGenerator.Invoke());
        DictionaryFromDictionary(tabCount, dictionaryName, dict, args);
    }

    /// <summary>
    /// EN: Generates a Dictionary from two lists (keys and values). Args.AddingValue = true was default.
    /// CZ: Generuje Dictionary ze dvou seznamů (klíče a hodnoty). Args.AddingValue = true byl výchozí.
    /// </summary>
    /// <typeparam name="Key">Type of dictionary keys</typeparam>
    /// <typeparam name="Value">Type of dictionary values</typeparam>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="dictionaryName">Name of the dictionary variable</param>
    /// <param name="keys">List of keys</param>
    /// <param name="values">List of values</param>
    /// <param name="args">Generator arguments</param>
    public void DictionaryFromTwoList<Key, Value>(int tabCount, string dictionaryName, List<Key> keys, List<Value> values, CSharpGeneratorArgs? args = null)
    {
        ThrowEx.DifferentCountInListsTU("keys", keys, "values", values);
        if (args == null)
            args = new CSharpGeneratorArgs();
        var dict = new Dictionary<Key, Value>();
        for (var i = 0; i < keys.Count; i++)
            dict.Add(keys[i], values[i]);
        DictionaryFromDictionary(tabCount, dictionaryName, dict, args);
    }
}
