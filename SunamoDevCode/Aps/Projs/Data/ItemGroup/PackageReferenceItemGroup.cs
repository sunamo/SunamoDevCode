// variables names: ok
namespace SunamoDevCode.Aps.Projs.Data.ItemGroup;

/// <summary>
/// Represents a package reference item group (NuGet) in .NET Core and .NET Framework projects.
/// </summary>
public class PackageReferenceItemGroup : ItemGroupElement
{
    /// <summary>
    /// Gets or sets the version of the package reference.
    /// </summary>
    public string Version { get; set; } = null;

    /// <summary>
    /// Type information for runtime type checking.
    /// </summary>
    public static Type Type = typeof(PackageReferenceItemGroup);

    /// <summary>
    /// Initializes a new instance of the <see cref="PackageReferenceItemGroup"/> class.
    /// </summary>
    /// <param name="fullCsprojPath">Full path to the csproj file.</param>
    public PackageReferenceItemGroup(string fullCsprojPath) : base(fullCsprojPath)
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PackageReferenceItemGroup"/> class from an XML node.
    /// </summary>
    /// <param name="fullCsprojPath">Full path to the csproj file.</param>
    /// <param name="xmlElement">XML element containing the package reference data.</param>
    public PackageReferenceItemGroup(string fullCsprojPath, XmlNode xmlElement) : base(fullCsprojPath)
    {
        const string VersionAttributeName = "Version";
        Include = XmlHelper.Attr(xmlElement, "Include");
        Version = XmlHelper.InnerTextOfNode(xmlElement);
        if (Version == string.Empty)
        {
            Version = XmlHelper.Attr(xmlElement, VersionAttributeName);
        }
    }

    /// <summary>
    /// Converts the package reference item group to an XML node.
    /// </summary>
    /// <param name="xmlDocument">The XML document to use for creating the node.</param>
    /// <returns>The XML node representing the package reference item group, or null if not implemented.</returns>
    public override XmlNode ToXml(XmlDocument xmlDocument)
    {
        return null;
    }
}