namespace SunamoDevCode;

using FileMs = File;

/// <summary>
/// Writes C# files with proper formatting: file-scoped namespace, sorted usings, and trimmed whitespace.
/// </summary>
public class TFCsFormat
{
    private static readonly List<string> classCodeElements = new()
        { "class ", "interface ", "enum ", "struct ", "delegate " };

    private static List<string> OnlyToFirst(List<string> lines)
    {
        var toFirstCodeElement = new List<string>();

        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            if (classCodeElements.Any(element => line.Contains(element)))
            {
                for (var previousIndex = i - 1; previousIndex >= 0; previousIndex--)
                    if (lines[previousIndex].StartsWith("//"))
                        i--;
                    else
                        break;

                toFirstCodeElement = lines.Take(i).ToList();

                for (var j = toFirstCodeElement.Count - 1; j >= 0; j--) lines.RemoveAt(0);

                break;
            }
        }

        return toFirstCodeElement;
    }

    /// <summary>
    /// Synchronously writes text to a C# file with proper formatting.
    /// </summary>
    /// <param name="filePath">Path to the file.</param>
    /// <param name="content">Text content to write.</param>
    public static void WriteAllTextSync(string filePath, string content)
    {
        WriteAllText(filePath, content).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Synchronously writes lines to a C# file with proper formatting.
    /// </summary>
    /// <param name="filePath">Path to the file.</param>
    /// <param name="lines">Lines to write.</param>
    public static void WriteAllLinesSync(string filePath, IEnumerable<string> lines)
    {
        WriteAllLines(filePath, lines).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Asynchronously writes text to a C# file with proper formatting (file-scoped namespace, sorted usings).
    /// </summary>
    /// <param name="filePath">Path to the file.</param>
    /// <param name="content">Text content to write.</param>
    public static async Task WriteAllText(string filePath, string content)
    {
        if (!filePath.EndsWith(".cs") || filePath.EndsWith("GlobalUsings.cs"))
        {
            await FileMs.WriteAllTextAsync(filePath, content);
            return;
        }

        var lines = SHGetLines.GetLines(content);
        await WriteAllLines(filePath, lines);
    }

    /// <summary>
    /// Asynchronously writes lines to a C# file with proper formatting (file-scoped namespace, sorted usings).
    /// </summary>
    /// <param name="filePath">Path to the file.</param>
    /// <param name="lines">Lines to write.</param>
    public static async Task WriteAllLines(string filePath, IEnumerable<string> lines)
    {
        if (!filePath.EndsWith(".cs") || filePath.EndsWith("GlobalUsings.cs"))
        {
            await FileMs.WriteAllLinesAsync(filePath, lines);
            return;
        }

        var mutableLines = lines.ToList();

        var toFirstCodeElement = OnlyToFirst(mutableLines);

        var usings = new List<string>();
        var ns = string.Empty;

        foreach (var item in toFirstCodeElement)
            if (item.StartsWith("using "))
                usings.Add(item);
            else if (item.StartsWith("namespace "))
                ns = item;

        ns = ns.TrimEnd(';') + ";";
        if (ns == string.Empty)
        {
            // todo doplnit ns
        }

        if (usings.Count != 0) usings.Add("");

        var wasBlockScopedNs = !ns.EndsWith(";");

        if (wasBlockScopedNs)
        {
            ThrowEx.Custom("Block scoped namespace is not allowed.");
        }

        usings.Insert(0, ns);
        usings.Insert(1, "");

        TrimWhiteSpaceRowFromEnd(mutableLines);

        if (wasBlockScopedNs)
        {

            if (mutableLines[mutableLines.Count - 1] == "}")
            {
                mutableLines.RemoveAt(mutableLines.Count - 1);
            }
            else
            {
                throw new Exception("The last line is not }");
            }
        }

        usings.AddRange(mutableLines);

        await FileMs.WriteAllTextAsync(filePath, SHJoin.JoinNL(usings));
    }

    /// <summary>
    /// Removes trailing whitespace-only lines from the end of the list.
    /// </summary>
    /// <param name="lines">List of lines to trim.</param>
    public static void TrimWhiteSpaceRowFromEnd(List<string> lines)
    {
        for (int i = lines.Count - 1; i >= 0; i--)
        {
            if (!string.IsNullOrWhiteSpace(lines[i]))
            {
                break;
            }
            lines.RemoveAt(i);
        }
    }
}