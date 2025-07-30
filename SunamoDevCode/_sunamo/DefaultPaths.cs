namespace SunamoDevCode._sunamo;

internal class DefaultPaths
{
    //private const string bpBb = @"D:\Documents\BitBucket\";
    internal static bool IsIgnored(string p, string bpBb)
    {
        if (p.StartsWith(bpBb)) return true;
        return false;
    }
}