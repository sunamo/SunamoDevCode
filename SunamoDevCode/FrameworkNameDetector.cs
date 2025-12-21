namespace SunamoDevCode;

public class FrameworkNameDetector
{
    public const string DefaultIdentifier = ".NETFramework";
    public const string DefaultFrameworkVersion = "v4.0";
    /// <summary>
    ///     Working only for .net fw, for sdk style return .net 4.0
    ///     Must return FrameworkName due to FubuCsProjFile
    /// </summary>
    /// <param name="project"></param>
    /// <returns></returns>
#pragma warning disable
    public static FrameworkName Detect(/*MSBuildProject*/ object project)
#pragma warning restore
    {
        throw new Exception(@"Už ani nevím odkud MSBuildProject je
Instaloval jsem tyto:
Microsoft.Build
Microsoft.CodeAnalysis
Microsoft.CodeAnalysis.Common
Microsoft.CodeAnalysis.CSharp
Microsoft.CodeAnalysis.Workspaces.Common
a ani v jednom. Navíc na netu taky od MS žádná zmínka. Asi už v době používání byl obsolete. udělat to tady z xml
Dole je kousek kódu:
var msb = new MSBuildProject();
        await msb.LoadAsync(path);
ale stejně na netu žádná zmínka
to jen znamená že to byla sračka a nikdo to nevyužíval, když se to nikde nevyužívalo");
        //var group = project.PropertyGroups.FirstOrDefault(x =>
        //    x.Properties.Any(p => p.Name.Contains("TargetFramework")));
        //var identifier = DefaultIdentifier;
        //var versionString = DefaultFrameworkVersion;
        //string profile = null;
        //if (group != null)
        //{
        //    // .NETFramework
        //    identifier = group.GetPropertyValue("TargetFrameworkIdentifier") ?? DefaultIdentifier;
        //    // 4.8
        //    versionString = group.GetPropertyValue("TargetFrameworkVersion") ?? DefaultFrameworkVersion;
        //    // SE
        //    profile = group.GetPropertyValue("TargetFrameworkProfile");
        //    var version = Version.Parse(versionString.Replace("v", "").Replace("V", ""));
        //    return new FrameworkName(identifier, version, profile);
        //}
        //return null; // DetectNetSdkVersion();
        return null;
    }
}