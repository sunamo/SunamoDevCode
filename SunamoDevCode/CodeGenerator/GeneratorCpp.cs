namespace SunamoDevCode.CodeGenerator;

public class GeneratorCpp : GeneratorCodeAbstract
{
    public void MapStringString(int tabCount, string mapName, Dictionary<string, string> nameCommentEnums)
    {
        string typeName = "map<string, string>";
        NewVariable(tabCount, typeName, mapName);
        foreach (var item in nameCommentEnums)
        {
            AppendLine(tabCount, mapName + ".insert({\"" + item.Key + "\", \"" + item.Value + "\"});");
        }
    }

    private void NewVariable(int tabCount, string typeName, string name)
    {
        AddTab2(tabCount, " ");
        sb.AddItem(typeName);
        sb.AppendLine(name + ";");
    }

    public void MapNonStringNonString(int tabCount, string mapName, string keyType, string valueType, Dictionary<string, string> nameCommentEnums)
    {
        string typeName = "map<" + keyType + ", " + valueType + ">";
        NewVariable(tabCount, typeName, mapName);
        foreach (var item in nameCommentEnums)
        {
            AppendLine(tabCount, mapName + ".insert({" + item.Key + ", " + item.Value + "});");
        }
    }

    public void VectorCustom(int tabsCount, string vectorName, string customType, Dictionary<string, string> dictionary)
    {
        string typeName = "vector<" + customType + ">";
        NewVariable(tabsCount, typeName, vectorName);
        Append(tabsCount, vectorName + "=");
        Append(0, "{");
        foreach (var item in dictionary)
        {
            Append(0, "{\"" + item.Key + "\", \"" + item.Value + "\"}" + ",");
        }
        AppendLine(0, "};");
    }

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
