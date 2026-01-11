// variables names: ok
namespace SunamoDevCode.MsBuild.Values;

/// <summary>
/// Framework versions and platform constants for MSBuild projects
/// </summary>
public class ProjectFrameworks
{
    /// <summary>
    /// List of Universal Windows Platform framework versions
    /// </summary>
    public static readonly List<string> UapFwVersions = new List<string>(["10.0.10240.0", "10.0.10586.0", "10.0.14393.0", "10.0.15063.0", "10.0.16299.0", "10.0.17134.1", "10.0.17763.0", "10.0.18362.0"]);

    /// <summary>
    /// List of .NET Framework versions
    /// </summary>
    public static readonly List<string> NetFwVersions = new List<string>(["v2.0", "v3.0", "v3.5", "v4.5.2", "v4.6", "v4.6.1", "v4.6.2", "v4.7", "v4.7.1", "v4.7.2", "v4.8"]);

    /// <summary>
    /// Target platform version property name
    /// </summary>
    public const string TargetPlatformVersion = "TargetPlatformVersion";

    /// <summary>
    /// Target platform minimum version property name
    /// </summary>
    public const string TargetPlatformMinVersion = "TargetPlatformMinVersion";
}