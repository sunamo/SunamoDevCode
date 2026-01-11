// variables names: ok
namespace SunamoDevCode.CodeGenerator;

/// <summary>
/// Generator for TypeScript code.
/// </summary>
public class TypeScriptGenerator
{
    private StringBuilder stringBuilder = new StringBuilder();

    /// <summary>
    /// Generates a TypeScript interface.
    /// </summary>
    /// <param name="isExporting">If true, adds the export keyword.</param>
    /// <param name="name">Name of the interface.</param>
    /// <param name="properties">Array of properties with their types.</param>
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

    /// <summary>
    /// Appends text to the generated code.
    /// </summary>
    /// <param name="text">Text to append.</param>
    public void Append(string text)
    {
        stringBuilder.Append(text);
    }

    /// <summary>
    /// Appends a line of text to the generated code.
    /// </summary>
    /// <param name="text">Text to append.</param>
    public void AppendLine(string text)
    {
        stringBuilder.AppendLine(text);
    }

    /// <summary>
    /// Generates a TypeScript class variable declaration.
    /// </summary>
    /// <param name="isExporting">If true, adds the export keyword.</param>
    /// <param name="type">Type of the variable.</param>
    /// <param name="name">Name of the variable.</param>
    /// <param name="value">Initial value for the variable.</param>
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

    /// <summary>
    /// Generates the start of a TypeScript array with key-value objects.
    /// </summary>
    /// <param name="itemType">Type of array items.</param>
    /// <param name="values">Initial values for the array.</param>
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

    /// <summary>
    /// Generates the end of a TypeScript array with key-value objects.
    /// </summary>
    public void ArrayWithKeyValueObjectEnd()
    {
        stringBuilder.AppendLine("];");
    }

    /// <summary>
    /// Returns the generated TypeScript code as a string.
    /// </summary>
    /// <returns>The generated TypeScript code.</returns>
    public override string ToString()
    {
        return stringBuilder.ToString();
    }

    private const string LetKeyword = "let";
    private const string ConstKeyword = "const";

    /// <summary>
    /// Generates a const field declaration.
    /// </summary>
    /// <param name="name">Name of the field.</param>
    /// <param name="type">Type of the field.</param>
    public void Const(string name, string type)
    {
        Field(ConstKeyword, name, type);
    }

    /// <summary>
    /// Generates a let field declaration.
    /// </summary>
    /// <param name="name">Name of the field.</param>
    /// <param name="type">Type of the field.</param>
    public void Let(string name, string type)
    {
        Field(LetKeyword, name, type);
    }

    /// <summary>
    /// Generates a field declaration with specified keyword.
    /// </summary>
    /// <param name="keyword">Keyword (let or const).</param>
    /// <param name="name">Name of the field.</param>
    /// <param name="type">Type of the field.</param>
    private void Field(string keyword, string name, string type)
    {
        stringBuilder.AppendLine(keyword + " " + name + ":" + type + "= " + TypeScriptHelper.DefaultValueForType(type));
    }

    /// <summary>
    /// Appends an empty line to the generated code.
    /// </summary>
    public void AppendLine()
    {
        stringBuilder.AppendLine();
    }
}