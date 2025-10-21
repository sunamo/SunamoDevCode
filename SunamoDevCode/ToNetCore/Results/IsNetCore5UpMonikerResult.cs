// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.ToNetCore.Results;

/// <summary>
/// https://learn.microsoft.com/en-us/dotnet/standard/frameworks
/// </summary>
public class IsNetCore5UpMonikerResult
{
    public string targetFramework;
    public string platformTfm;

    public override string ToString()
    {
        return targetFramework + platformTfm;
    }
}
