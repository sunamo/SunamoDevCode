// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
// Instance variables refactored according to C# conventions
namespace SunamoDevCode;

public class Boilerplate
{
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