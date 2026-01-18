namespace SunamoDevCode.Aps.Projs.Data.ItemGroup;

/// <summary>
/// Represents a project reference item group in a Visual Studio project file.
/// </summary>
public class ProjectReferenceItemGroup : ItemGroupElement
{
    /// <summary>
    /// Type information for runtime type checking.
    /// </summary>
    public static Type Type = typeof(ProjectReferenceItemGroup);

    /// <summary>
    /// Gets or sets the project GUID.
    /// </summary>
    public string Project { get; set; } = null;

    /// <summary>
    /// Gets or sets the project name.
    /// </summary>
    public string Name { get; set; } = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectReferenceItemGroup"/> class.
    /// </summary>
    /// <param name="fullPath">Full path to the csproj file.</param>
    public ProjectReferenceItemGroup(string fullPath) : base(fullPath)
    {
    }

    /// <summary>
    /// Converts the project reference item group to an XML node.
    /// </summary>
    /// <param name="xmlDocument">The XML document to use for creating the node.</param>
    /// <returns>The XML node representing the project reference item group.</returns>
    public override XmlNode ToXml(XmlDocument xmlDocument)
    {
        // .NET Framework also adds Name and ProjectGuid elements, but they are not required and it works the same without them. Tested with rebuild.
        var xmlContent = "<" + ItemGroups.ProjectReference + " Include=\"" + Include + "\" />";
        XmlNode node = XH.ReturnXmlNode(xmlContent.ToString());
        return node;
    }
}