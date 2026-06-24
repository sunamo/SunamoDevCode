namespace SunamoDevCode.CodeGenerator;

public class CSharpClassesGenerator
{
    public static Type Type = typeof(CSharpClassesGenerator);

    public static string Dictionary(string className, List<string> keys, Func<string> randomValue)
    {
        List<string> values = new List<string>();
        for (int i = 0; i < keys.Count; i++)
        {
            values.Add(randomValue());
        }

        return Dictionary(className, keys, values);
    }

    public static string Dictionary(string className, List<string> keys, List<string> values)
    {

        ThrowEx.DifferentCountInLists(nameof(keys), keys.Count, nameof(values), values.Count);


        CSharpGenerator generator = new CSharpGenerator();
        generator.StartClass(0, AccessModifiers.Private, false, className);
        generator.Field(1, AccessModifiers.Private, false, VariableModifiers.None, "Dictionary<string, string>", "dict", false, "new Dictionary<string, string>()");
        CSharpGenerator inner = new CSharpGenerator();
        for (int i = 0; i < keys.Count; i++)
        {
            inner.AppendLine(2, "dict.Add(\"{0}\", \"{1}\");", keys[i], values[i]);
        }
        generator.Ctor(1, ModifiersConstructor.Private, className, inner.ToString());
        generator.EndBrace(0);
        return generator.ToString();
    }

    public static string DictionaryPascalConvention(string className, List<string> list, bool switchKeysAndValues)
    {
        List<string> values = new List<string>();
        for (int i = 0; i < list.Count; i++)
        {
            values.Add(ConvertPascalConvention.ToConvention(list[i]));
        }

        if (switchKeysAndValues)
        {
            return Dictionary(className, values, list);
        }
        else
        {
            return Dictionary(className, list, values);
        }
    }
}
