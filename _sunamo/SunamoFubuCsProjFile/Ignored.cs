namespace SunamoDevCode;


internal class Ignored
{
    internal class StartWith
    {
        internal const string uap = @"\_Uap\";
        internal const string archived = @"_Archived";
        internal const string mixin = @"_Mixin";
    }
    internal class EndsWith
    {
        internal const string vcxProj = ".vcxproj";
    }
    internal static bool IsIgnored(string path)
    {
        if (path == null) return true;
        return path.Contains(StartWith.archived) || path.Contains(StartWith.uap) || path.Contains(StartWith.mixin) ||
               path.EndsWith(EndsWith.vcxProj);
    }
}