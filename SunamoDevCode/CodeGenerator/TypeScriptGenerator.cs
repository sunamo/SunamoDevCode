namespace SunamoDevCode.CodeGenerator;

public class TypeScriptGenerator
{
    private StringBuilder stringBuilder = new StringBuilder();

    public void Interface(bool isExporting, string name, params TWithNameTDC<string>[] properties)
    {
        if (isExporting)
        {
            stringBuilder.Append("export ");
        }
        stringBuilder.Append("interface ");
        stringBuilder.AppendLine(name);
        stringBuilder.AppendLine("{");
        foreach (var item in properties)
        {
            stringBuilder.AppendLine(item.name + ": " + item.t + ";");
        }
        stringBuilder.AppendLine("}");
        stringBuilder.AppendLine();
    }

    public void Append(string text)
    {
        stringBuilder.Append(text);
    }

    public void AppendLine(string text)
    {
        stringBuilder.AppendLine(text);
    }

    public void ClassVariable(bool isExporting, TypesTs type, string name, string value)
    {
        if (isExporting)
        {
            stringBuilder.Append("export ");
        }

        stringBuilder.Append(type.ToString().TrimStart('_') + " ");

        stringBuilder.Append(name);
        stringBuilder.Append(" = ");
        if (type == TypesTs._string)
        {
            value = SH.WrapWithQm(value);
        }
        stringBuilder.Append(value);
        stringBuilder.Append(";");
    }

    public void ArrayWithKeyValueObjectStart(string itemType, params string[] values)
    {
        if (itemType == "string")
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = SH.WrapWithQm(values[i]);
            }
        }

        stringBuilder.AppendLine("const arr : " + itemType + "[] = [" + string.Join(", ", values) + "]");
    }

    public void ArrayWithKeyValueObjectEnd()
    {
        stringBuilder.AppendLine("];");
    }

    public override string ToString() => stringBuilder.ToString();

    private const string LetKeyword = "let";
    private const string ConstKeyword = "const";

    public void Const(string name, string type)
    {
        Field(ConstKeyword, name, type);
    }

    public void Let(string name, string type)
    {
        Field(LetKeyword, name, type);
    }

    private void Field(string keyword, string name, string type)
    {
        stringBuilder.AppendLine(keyword + " " + name + ":" + type + "= " + TypeScriptHelper.DefaultValueForType(type));
    }

    public void AppendLine()
    {
        stringBuilder.AppendLine();
    }
}
