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
