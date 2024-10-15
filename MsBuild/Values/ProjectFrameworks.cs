namespace SunamoDevCode.MsBuild.Values;
public class ProjectFrameworks
{
    public static List<string> uapFwVersions = new List<string>(["10.0.10240.0", "10.0.10586.0", "10.0.14393.0", "10.0.15063.0", "10.0.16299.0", "10.0.17134.1", "10.0.17763.0", "10.0.18362.0"]);
    public static List<string> netFwVersions = new List<string>(["v2.0", "v3.0", "v3.5", "v4.5.2", "v4.6", "v4.6.1", "v4.6.2", "v4.7", "v4.7.1", "v4.7.2", "v4.8"]);

    public const string TargetPlatformVersion = "TargetPlatformVersion";
    public const string TargetPlatformMinVersion = "TargetPlatformMinVersion";
}