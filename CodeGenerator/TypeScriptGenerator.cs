namespace SunamoDevCode.CodeGenerator;


public class TypeScriptGenerator
{
    StringBuilder sb = new StringBuilder();

    public void Interface(bool export, string name, params TWithNameTDC<string>[] vars)
    {
        if (export)
        {
            sb.Append("export ");
        }
        sb.Append("interface ");
        sb.AppendLine(name);
        sb.AppendLine(AllStrings.lcub);
        foreach (var item in vars)
        {
            sb.AppendLine(item.name + ": " + item.t + AllStrings.sc);
        }
        sb.AppendLine(AllStrings.rcub);
        sb.AppendLine();
    }

    public void Append(string v)
    {
        sb.Append(v);
    }

    public void AppendLine(string v)
    {
        sb.AppendLine(v);
    }

    public void ClassVariable(bool export, TypesTs type, string name, string value)
    {
        if (export)
        {
            sb.Append("export ");
        }

        sb.Append(type.ToString().TrimStart(AllChars.lowbar) + " ");

        sb.Append(name);
        sb.Append(" = ");
        if (type == TypesTs._string)
        {
            value = SH.WrapWithQm(value);
        }
        sb.Append(value);
        sb.Append(AllStrings.sc);
    }

    public void ArrayWithKeyValueObjectStart(string innerType, params string[] values)
    {
        sb.AppendLine("const arr : " + innerType + "[] = [");
    }

    public void ArrayWithKeyValueObjectEnd()
    {
        sb.AppendLine("];");
    }

    public override string ToString()
    {
        return sb.ToString();
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
        sb.AppendLine(keyword + " " + item + ":" + type + "= " + TypeScriptHelper.DefaultValueForType(type));
    }

    public void AppendLine()
    {
        sb.AppendLine();
    }
}
