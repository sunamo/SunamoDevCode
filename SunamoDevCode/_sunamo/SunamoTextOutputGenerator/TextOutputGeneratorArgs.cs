// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode._sunamo.SunamoTextOutputGenerator;


internal class TextOutputGeneratorArgs
{
    internal bool headerWrappedEmptyLines = true;
    internal bool insertCount = false;
    internal string whenNoEntries = "No entries";
    internal string delimiter = Environment.NewLine;
    internal TextOutputGeneratorArgs()
    {
    }
    internal TextOutputGeneratorArgs(bool headerWrappedEmptyLines, bool insertCount)
    {
        this.headerWrappedEmptyLines = headerWrappedEmptyLines;
        this.insertCount = insertCount;
    }
}