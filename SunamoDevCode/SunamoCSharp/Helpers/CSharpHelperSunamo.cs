namespace SunamoDevCode.SunamoCSharp.Helpers;

public class CSharpHelperSunamo
{
    public static string IsAllCsprojAndSlnRightInHiearchy(string path, TextOutputGeneratorDC tog)
    {
        var csproj = Directory.GetFiles(path, "*.csproj", SearchOption.AllDirectories).ToList();
        var sln = Directory.GetFiles(path, "*.sln", SearchOption.AllDirectories).ToList();

        //List<string> foldersWithSlnOk = new List<string>();
        List<string> foldersWithSlnKo = new List<string>();

        for (int i = sln.Count - 1; i >= 0; i--)
        {
            var item = sln[i];


            if (!IsOnlyInSpecialOrProjectFolders(sln[i]))
            {
                foldersWithSlnKo.Add(sln[i]);
                sln.RemoveAt(i);
            }
        }

        //sln = CAChangeContent.ChangeContent0(null, sln, Path.GetDirectoryName);

        for (int i = 0; i < sln.Count; i++)
        {
            sln[i] = Path.GetDirectoryName(sln[i]);
        }

        for (int i = csproj.Count - 1; i >= 0; i--)
        {
            var csprojFolder = Path.GetDirectoryName(csproj[i]);
            var slnFolder = Path.GetDirectoryName(csprojFolder);

            if (sln.Contains(slnFolder))
            {
                csproj.RemoveAt(i);
            }
        }



        tog.List(foldersWithSlnKo, "Sln in wrong folder:");
        tog.List(csproj, "Csproj in wrong folder:");

        return tog.ToString();
    }

    private static bool IsOnlyInSpecialOrProjectFolders(string filePath)
    {
        filePath = Path.GetDirectoryName(filePath);

        while (true)
        {
            filePath = Path.GetDirectoryName(filePath);

            if (filePath.Length < 4)
            {
                return false;
            }

            var fn = Path.GetFileName(filePath);

            if (fn.EndsWith("Projects"))
            {
                return true;
            }

            if (!fn.StartsWith("_"))
            {
                return false;
            }
        }

        return false;
    }

    public static FromToList DetectFromToString(string text)
    {
        List<int> oc = null;// SH.ReturnOccurencesOfString(text, "\"");
        for (int i = oc.Count - 1; i >= 0; i--)
        {
            if (text[oc[i] - 1] == '\\')
            {
                oc.RemoveAt(i);
            }
        }

        ThrowEx.HasOddNumberOfElements("oc", oc);

        var ft = new FromToList();
        for (int i = 0; i < oc.Count; i++)
        {
            ft.c.Add(new FromToDC(oc[i], oc[++i]));
        }
        return ft;
    }

    public static void IndentAsPreviousLine(List<string> lines)
    {
        string indentPrevious = string.Empty;
        string line = null;
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < lines.Count; i++)
        {
            line = lines[i];
            if (line.Length > 0)
            {
                if (!char.IsWhiteSpace(line[0]))
                {
                    lines[i] = indentPrevious + lines[i];
                }
                else
                {
                    StringBuilder sb2 = new StringBuilder();
                    foreach (var item in line)
                    {
                        if (char.IsWhiteSpace(item))
                        {
                            sb2.Append(item);
                        }
                        else
                        {
                            break;
                        }
                    }
                    indentPrevious = stringBuilder.ToString();
                    //indentPrevious = SH.GetWhitespaceFromBeginning(stringBuilder, line);
                }
            }
        }
    }
    public static bool IsInterface(string item)
    {
        if (item[0] == 'I')
        {
            if (char.IsUpper(item[1]))
            {
                return true;
            }
        }
        return false;
    }

    public static string ReplaceNulled(string text)
    {
        return text.Replace("(null)", string.Empty).Trim();
    }

    public static string ShortcutForControl(string name)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (var item in name)
        {
            if (char.IsUpper(item))
            {
                stringBuilder.Append(item.ToString().ToLower());
            }
        }
        return stringBuilder.ToString();
    }
    /// <summary>
    /// Its not compatible with default operator
    /// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/default-values
    /// Nonsense, cant type too many different output types to T.
    /// Must cast manually
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    /// <param name = "t"></param>
    public static object DefaultValueForTypeT<T>(T t, Func<string, string> ConvertTypeShortcutFullNameToShortcut)
    {
        var type = t.GetType().FullName;
        if (type.Contains("."))
        {
            type = ConvertTypeShortcutFullNameToShortcut(type);
        }
        #region Same seria as in Types
        switch (type)
        {
            case "string":
                return string.Empty;
            case "bool":
                return false;
            case "float":
            case "double":
            case "int":
            case "long":
            case "short":
            case "decimal":
            case "sbyte":
                return -1;
            case "byte":
            case "ushort":
            case "uint":
            case "ulong":
                return 0;
            case "DateTime":
                // Původně tu bylo MinValue kvůli SQLite ale dohodl jsem se že SQLite už nebudu používat a proto si ušetřím v kódu práci text MSSQL
                return "new(1900, 1, 1)";
            case "byte[]":
                // Podporovaný typ pouze v desktopových aplikacích, kde není lsožka sbf
                return null;
            case "Guid":
                return Guid.Empty;
            case "char":
                throw new Exception("Nepodporovan\u00FD typ");
                break;
        }
        #endregion
        throw new Exception("Nepodporovan\u00FD typ");
        return null;
    }

    static Type type = typeof(CSharpHelperSunamo);
    public static string DefaultValueForType(string type, Func<string, string> ConvertTypeShortcutFullNameToShortcut)
    {
        if (type.Contains("."))
        {
            type = ConvertTypeShortcutFullNameToShortcut(type);
        }
        switch (type)
        {
            case "string":
                return "\"" + "\"";
            case "bool":
                return "false";
            case "float":
            case "double":
            case "int":
            case "long":
            case "short":
            case "decimal":
            case "sbyte":
                return "-1";
            case "byte":
            case "ushort":
            case "uint":
            case "ulong":
                return "0";
            case "DateTime":
                // Původně tu bylo MinValue kvůli SQLite ale dohodl jsem se že SQLite už nebudu používat a proto si ušetřím v kódu práci text MSSQL 
                return "SqlServerHelper.DateTimeMinVal";
            case "byte[]":
                // Podporovaný typ pouze v desktopových aplikacích, kde není lsožka sbf
                return "null";
            case "Guid":
                return "Guid.Empty";
            case "char":
                throw new Exception("Nepodporovan\u00FD typ");
                break;
            default:
                // For types like Dictionary<int,int>
                return "new " + type + "()";
        }
        //throw new Exception("Nepodporovan\u00FD typ");
        return null;
    }

    public static string ReplaceReadonlyToConst(string arg)
    {
        arg = arg.Replace("static readonly", "const");
        arg = arg.Replace("readonly", "const");
        return arg;
    }
}