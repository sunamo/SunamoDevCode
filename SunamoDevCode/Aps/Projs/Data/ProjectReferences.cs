// variables names: ok
namespace SunamoDevCode.Aps.Projs.Data;

/// <summary>
/// Contains project references with their XML nodes
/// </summary>
public class ProjectReferences
{
    /// <summary>
    /// List of project names or paths
    /// </summary>
    public List<string> Projects { get; set; }

    /// <summary>
    /// XML nodes representing the project references
    /// </summary>
    public List<XmlNode> Nodes { get; set; }
}