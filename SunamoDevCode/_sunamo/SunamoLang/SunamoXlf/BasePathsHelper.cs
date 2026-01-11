namespace SunamoDevCode._sunamo.SunamoLang.SunamoXlf;

/// <summary>
/// Helper class for managing base paths used in localization.
/// </summary>
internal class BasePathsHelper
{
    // EN: Don't use this anymore. Pass in parameters to keep it as simple as possible.

    /// <summary>
    /// Path to the Visual Studio projects directory.
    /// </summary>
    internal static string VsProjects = null;

    /// <summary>
    /// Path for the actual platform being used.
    /// </summary>
    internal static string ActualPlatform;

    /// <summary>
    /// Path to the Visual Studio directory.
    /// </summary>
    internal static string Vs;

    /// <summary>
    /// Constant path for C:\repos directory.
    /// </summary>
    internal const string CRepos = @"C:\repos";

    /// <summary>
    /// Base path for main machine (MB): E:\vs\
    /// </summary>
    internal const string BpMb = @"E:\vs\";

    /// <summary>
    /// Base path for Q environment: C:\repos\_\
    /// </summary>
    internal const string BpQ = @"C:\repos\_\";

    /// <summary>
    /// Base path for VPS: C:\_\
    /// </summary>
    internal const string BpVps = @"C:\_\";

    /// <summary>
    /// Base path for BitBucket: D:\Documents\BitBucket\
    /// </summary>
    internal const string BpBb = @"D:\Documents\BitBucket\";
}