// variables names: ok
namespace SunamoDevCode.CodeGenerator;

/// <summary>
/// Generator for C# classes. Generation of single element is in CSharpHelper.
/// </summary>
public class CSharpClassesGenerator
{
    /// <summary>
    /// Type information for runtime type checking.
    /// </summary>
    public static Type Type = typeof(CSharpClassesGenerator);

    /// <summary>
    /// Generates a dictionary class with random values.
    /// </summary>
    /// <param name="className">Name of the class to generate.</param>
    /// <param name="keys">List of dictionary keys.</param>
    /// <param name="randomValue">Function that generates random values.</param>
    /// <returns>Generated C# code as string.</returns>
    public static string Dictionary(string className, List<string> keys, Func<string> randomValue)
    {
        List<string> values = new List<string>();
        for (int i = 0; i < keys.Count; i++)
        {
            values.Add(randomValue());
        }

        return Dictionary(className, keys, values);
    }

    /// <summary>
    /// Generates a dictionary class with specified keys and values.
    /// </summary>
    /// <param name="className">Name of the class to generate.</param>
    /// <param name="keys">List of dictionary keys.</param>
    /// <param name="values">List of dictionary values.</param>
    /// <returns>Generated C# code as string.</returns>
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

    /// <summary>
    /// Generates a dictionary class with Pascal convention conversion.
    /// </summary>
    /// <param name="className">Name of the class to generate.</param>
    /// <param name="list">List of items to convert and use as keys or values.</param>
    /// <param name="switchKeysAndValues">If true, switches keys and values in the dictionary.</param>
    /// <returns>Generated C# code as string.</returns>
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