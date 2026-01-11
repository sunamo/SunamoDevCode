// variables names: ok
namespace SunamoDevCode.CodeGenerator;

/// <summary>
/// C++ code generator for maps, vectors, and arrays.
/// </summary>
public class GeneratorCpp : GeneratorCodeAbstract
{
    /// <summary>
    /// Generates a C++ map with string keys and string values.
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation.</param>
    /// <param name="mapName">Name of the map variable.</param>
    /// <param name="nameCommentEnums">Dictionary of key-value pairs to add to the map.</param>
    public void MapStringString(int tabCount, string mapName, Dictionary<string, string> nameCommentEnums)
    {
        string typeName = "map<string, string>";
        NewVariable(tabCount, AccessModifiers.Private, typeName, mapName, true);
        foreach (var item in nameCommentEnums)
        {
            AppendLine(tabCount, mapName + ".insert({\"" + item.Key + "\", \"" + item.Value + "\"});");
        }
    }

    /// <summary>
    /// Creates a new variable declaration in C++.
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation.</param>
    /// <param name="accessModifier">Access modifier (not used in C++ the same way as C#).</param>
    /// <param name="typeName">Type name of the variable.</param>
    /// <param name="name">Name of the variable.</param>
    /// <param name="createInstance">Whether to create an instance (not used in current implementation).</param>
    private void NewVariable(int tabCount, AccessModifiers accessModifier, string typeName, string name, bool createInstance)
    {
        AddTab2(tabCount, " ");
        sb.AddItem(typeName);
        sb.AppendLine(name + ";");
    }

    /// <summary>
    /// Generates a C++ map with custom key and value types.
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation.</param>
    /// <param name="mapName">Name of the map variable.</param>
    /// <param name="keyType">Type of the map keys.</param>
    /// <param name="valueType">Type of the map values.</param>
    /// <param name="nameCommentEnums">Dictionary of key-value pairs to add to the map.</param>
    public void MapNonStringNonString(int tabCount, string mapName, string keyType, string valueType, Dictionary<string, string> nameCommentEnums)
    {
        string typeName = "map<" + keyType + ", " + valueType + ">";
        NewVariable(tabCount, AccessModifiers.Private, typeName, mapName, true);
        foreach (var item in nameCommentEnums)
        {
            AppendLine(tabCount, mapName + ".insert({" + item.Key + ", " + item.Value + "});");
        }
    }

    /// <summary>
    /// Generates a C++ vector with custom type.
    /// </summary>
    /// <param name="tabsCount">Number of tabs for indentation.</param>
    /// <param name="vectorName">Name of the vector variable.</param>
    /// <param name="customType">Custom type for vector elements.</param>
    /// <param name="dictionary">Dictionary of key-value pairs to initialize the vector.</param>
    public void VectorCustom(int tabsCount, string vectorName, string customType, Dictionary<string, string> dictionary)
    {
        string typeName = "vector<" + customType + ">";
        NewVariable(tabsCount, AccessModifiers.Private, typeName, vectorName, true);
        Append(tabsCount, vectorName + "=");
        Append(0, "{");
        foreach (var item in dictionary)
        {
            Append(0, "{\"" + item.Key + "\", \"" + item.Value + "\"}" + ",");
        }
        AppendLine(0, "};");
    }

    /// <summary>
    /// Generates a C++ array with custom type.
    /// </summary>
    /// <param name="tabsCount">Number of tabs for indentation.</param>
    /// <param name="arrayName">Name of the array variable.</param>
    /// <param name="customType">Custom type for array elements.</param>
    /// <param name="dictionary">Dictionary of key-value pairs to initialize the array.</param>
    public void Array(int tabsCount, string arrayName, string customType, Dictionary<string, string> dictionary)
    {
        AddTab2(tabsCount, " ");
        sb.AddItem(customType);
        Append(tabsCount, arrayName + "[" + dictionary.Count + "]=");
        Append(0, "{");
        foreach (var item in dictionary)
        {
            string separator = item.Key == dictionary.Last().Key ? "" : ",";
            Append(0, "{\"" + item.Key + "\", \"" + item.Value + "\"}" + separator);
        }
        AppendLine(0, "};");
    }
}