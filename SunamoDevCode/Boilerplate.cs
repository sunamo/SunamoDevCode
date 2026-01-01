namespace SunamoDevCode;

/// <summary>
/// EN: Provides boilerplate code templates for C# code generation
/// CZ: Poskytuje šablony boilerplate kódu pro generování C# kódu
/// </summary>
public class Boilerplate
{
    /// <summary>
    /// EN: Generates C# command-line program boilerplate
    /// CZ: Generuje boilerplate pro C# konzolový program
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
    /// EN: Generates C# class boilerplate with initialization method
    /// CZ: Generuje boilerplate pro C# třídu s inicializační metodou
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