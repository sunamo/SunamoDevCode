// variables names: ok
namespace SunamoDevCode.Aps.Projs.Data.ItemGroup;

/// <summary>
/// Represents a reference item group (assembly reference) in a Visual Studio project file.
/// </summary>
public class ReferenceItemGroup : ItemGroupElement
{
    /// <summary>
    /// Type information for runtime type checking in ItemGroup processing.
    /// </summary>
    public static readonly Type Type = typeof(ReferenceItemGroup);

    /// <summary>
    /// Dictionary of parsed Include attribute parts (e.g., Version, Culture, PublicKeyToken).
    /// </summary>
    public Dictionary<string, string> IncludeParts { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// Relative path from project folder to DLL used as HintPath.
    /// Used later in AddItemGroup2 as HintPath.
    /// </summary>
    private string relativePathDllToProjectFolderHintPath = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReferenceItemGroup"/> class.
    /// </summary>
    /// <param name="fullPathDllInclude">Full path to the DLL for the Include attribute.</param>
    /// <param name="fullPathCsproj">Full path to the csproj file.</param>
    /// <param name="relativePathDllToProjectFolderHintPath">Relative path from project folder to DLL for HintPath.</param>
    public ReferenceItemGroup(string fullPathDllInclude, string fullPathCsproj, string relativePathDllToProjectFolderHintPath) : base(fullPathCsproj)
    {
        Include = fullPathDllInclude;
        this.relativePathDllToProjectFolderHintPath = relativePathDllToProjectFolderHintPath;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReferenceItemGroup"/> class from an XML node.
    /// </summary>
    /// <param name="fullPathCsproj">Full path to the csproj file.</param>
    /// <param name="xmlElement">XML element containing the reference data.</param>
    public ReferenceItemGroup(string fullPathCsproj, XmlNode xmlElement) : base(fullPathCsproj)
    {
        Include = XmlHelper.Attr(xmlElement, "Include");
        if (Include.Contains(","))
        {
            var includeParts = SHSplit.Split(Include, ",");
            CA.Trim(includeParts);
            foreach (var item in includeParts)
            {
                if (item.Contains("="))
                {
                    var parts = SHSplit.Split(item, "=");
                    IncludeParts.Add(parts[0], parts[1]);
                }
            }
        }
    }

    /// <summary>
    /// Gets the version from the parsed Include attribute parts.
    /// </summary>
    public string Version
    {
        get => FromInclude("Version");
    }

    /// <summary>
    /// Gets a value from the parsed Include attribute parts by key.
    /// </summary>
    /// <param name="key">The key to look up in the Include parts dictionary.</param>
    /// <returns>The value if found, otherwise an empty string.</returns>
    private string FromInclude(string key)
    {
        if (IncludeParts.ContainsKey(key))
        {
            return IncludeParts[key];
        }
        return string.Empty;
    }

    /// <summary>
    /// Converts the reference item group to an XML node.
    /// </summary>
    /// <param name="xmlDocument">The XML document to use for creating the node.</param>
    /// <returns>The XML node representing the reference item group.</returns>
    public override XmlNode ToXml(XmlDocument xmlDocument)
    {
        XmlGenerator generator = new XmlGenerator();
        generator.WriteTagWithAttrsCheckNull(VsProjectItemTypes.Reference, "Include", Include);
        generator.TerminateTag(VsProjectItemTypes.Reference);
        XmlNode node = XH.ReturnXmlNode(generator.ToString());
        return node;
    }
}
