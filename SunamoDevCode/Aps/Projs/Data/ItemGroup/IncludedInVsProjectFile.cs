namespace SunamoDevCode.Aps.Projs.Data.ItemGroup;

/// <summary>
/// Represents items included in a Visual Studio project file.
/// </summary>
public class IncludedInVsProjectFile
{
    /// <summary>
    /// Gets or sets the list of compile item groups.
    /// </summary>
    public List<CompileItemGroup> Compile { get; set; } = new List<CompileItemGroup>();
}