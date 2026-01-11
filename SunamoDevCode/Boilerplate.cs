// variables names: ok
namespace SunamoDevCode;

/// <summary>
/// Provides boilerplate code templates for C# code generation
/// </summary>
public class Boilerplate
{
    /// <summary>
    /// Generates C# command-line program boilerplate
    /// </summary>
    /// <param name="innerMain">Main method content</param>
    public static string CSharpCmd(string innerMain)
    {
        var csharpTemplate = @"using System;

{
    class Program
    {
        static void Main(String[] args)
        {
            {0}
        }
    }
}";

        var stringBuilder = new StringBuilder();
        // If it not working, try Format3. Dont use any try-catch!
        stringBuilder.AppendLine(SHFormat.Format4(csharpTemplate, innerMain));

        return stringBuilder.ToString();
    }

    /// <summary>
    /// Generates C# class boilerplate with initialization method
    /// </summary>
    /// <param name="addNamespacesLines">Additional namespace using declarations</param>
    /// <param name="className">Name of the class</param>
    /// <param name="fields">Field declarations</param>
    /// <param name="contentOfInitMethod">Content of Init method</param>
    public static string CSharpClass(string addNamespacesLines, string className, string fields,
        string contentOfInitMethod)
    {
        var classTemplate = @"using System;
{0}


    public class {1}
    {
        {2}

        public static void Init()
        {
            {3}
        }
    }";

        var stringBuilder = new StringBuilder();

        // If it not working, try Format3. Dont use any try-catch!
        stringBuilder.AppendLine(SHFormat.Format4(classTemplate, addNamespacesLines, className, fields, contentOfInitMethod));

        return stringBuilder.ToString();
    }
}
