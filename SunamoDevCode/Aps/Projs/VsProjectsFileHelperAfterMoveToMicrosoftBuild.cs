// variables names: ok
namespace SunamoDevCode.Aps.Projs;

/// <summary>
/// EN: Methods that were moved to SunamoMicrosoftBuild.
/// They could not remain here due to transitive dependencies.
/// CZ: Metody které se přesunuly do SunamoMicrosoftBuild.
/// Zde nemohly zůstat kvůli transitive deps.
/// </summary>
public partial class VsProjectsFileHelper
{
    /// <summary>
    /// Adds a new ItemGroup to the project XML.
    /// This method has been moved to SunamoMicrosoftBuild.
    /// </summary>
    /// <param name="csprojPath">Path to the .csproj file.</param>
    /// <param name="xmlDocument">XML document representing the project.</param>
    /// <param name="namespaceManager">XML namespace manager for XPath queries.</param>
    /// <param name="itemGroup">The ItemGroup XML node.</param>
    /// <param name="project">The Project XML node.</param>
    /// <returns>The newly added ItemGroup node.</returns>
    /// <exception cref="NotImplementedException">This method has been moved to another assembly.</exception>
    private static XmlNode AddNewItemGroup(string csprojPath, XmlDocument xmlDocument, XmlNamespaceManager namespaceManager, XmlNode itemGroup, XmlNode project)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Replaces project template parameters in the content.
    /// This method has been moved to SunamoMicrosoftBuild.
    /// </summary>
    /// <param name="content">The content string to modify.</param>
    /// <param name="templateParameter">The template parameter type.</param>
    /// <param name="value">The value to replace with.</param>
    /// <exception cref="NotImplementedException">This method has been moved to another assembly.</exception>
    private static void ReplaceProjectTemplateParameter(ref string content, VsProjectTemplateParameters templateParameter, object value)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Sets the target frameworks for UAP (Universal Application Platform) projects.
    /// This method has been moved to SunamoMicrosoftBuild.
    /// </summary>
    /// <param name="csprojPath">Path to the .csproj file.</param>
    /// <param name="targetFramework">The target framework version.</param>
    /// <param name="minFramework">The minimum framework version.</param>
    /// <exception cref="NotImplementedException">This method has been moved to another assembly.</exception>
    public static void SetTargetFrameworksUap(string csprojPath, string targetFramework, string minFramework)
    {
        throw new NotImplementedException();
    }
}