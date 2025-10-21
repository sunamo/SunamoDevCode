// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.Data;


public class ParseGlobalUsingsResult
{
    public List<string> GlobalUsings { get; set; } = new List<string>();
    public Dictionary<string, string> GlobalSymbols { get; set; } = new();
}
