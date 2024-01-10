namespace SunamoDevCode;

public class Boilerplate
{
    public static string CSharpCmd(string innerMain)
    {
        var c = @"using System;

{
    class Program
    {
        static void Main(String[] args)
        {
            {0}
        }
    }
}";

        StringBuilder sb = new StringBuilder();
        // If it not working, try Format3. Dont use any try-catch! 
        sb.AppendLine(SH.Format4(c, innerMain));

        return sb.ToString();
    }
    public static string CSharpClass(string addNamespacesLines, string className, string fields, string contentOfInitMethod)
    {
        var c = @"using System;
{0}


    public class {1}
    {
        {2}

        public static void Init()
        {
            {3}
        }
    }";

        StringBuilder sb = new StringBuilder();

        // If it not working, try Format3. Dont use any try-catch! 
        sb.AppendLine(SH.Format4(c, addNamespacesLines, className, fields, contentOfInitMethod));

        return sb.ToString();
    }
}
