namespace SunamoDevCode.CodeGenerator;

public class TypeScriptGenerator
{
    StringBuilder stringBuilder = new StringBuilder();

    public void Interface(bool export, string name, params TWithNameTDC<string>[] vars)
    {
        if (export)
        {
            stringBuilder.Append("export ");
        }
        stringBuilder.Append("interface ");
        stringBuilder.AppendLine(name);
        stringBuilder.AppendLine("{");
        foreach (var item in vars)
        {
            stringBuilder.AppendLine(item.name + ": " + item.t + ";");
        }
        stringBuilder.AppendLine("}");
        stringBuilder.AppendLine();
    }

    public void Append(string v)
    {
        stringBuilder.Append(v);
    }

    public void AppendLine(string v)
    {
        stringBuilder.AppendLine(v);
    }

    public void ClassVariable(bool export, TypesTs type, string name, string value)
    {
        if (export)
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

    public void ArrayWithKeyValueObjectStart(string innerType, params string[] values)
    {
        if (innerType == "string")
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = SH.WrapWithQm(values[i]);
            }
        }

        stringBuilder.AppendLine("const arr : " + innerType + "[] = [" + string.Join(", ", values) + "]");
    }

    public void ArrayWithKeyValueObjectEnd()
    {
        stringBuilder.AppendLine("];");
    }

    public override string ToString()
    {
        return stringBuilder.ToString();
    }

    const string let = "let";
    const string const_ = "const";

    public void Const(string item, string type)
    {
        Field(const_, item, type);
    }

    public void Let(string item, string type)
    {
        Field(let, item, type);
    }

    void Field(string keyword, string item, string type)
    {
        stringBuilder.AppendLine(keyword + " " + item + ":" + type + "= " + TypeScriptHelper.DefaultValueForType(type));
    }

    public void AppendLine()
    {
        stringBuilder.AppendLine();
    }
}