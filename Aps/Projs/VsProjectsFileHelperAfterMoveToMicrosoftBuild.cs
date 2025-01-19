
namespace SunamoDevCode.Aps.Projs;
/// <summary>
/// Metodoy které se přesunuli do SunamoMicrosoftBuild
/// Zde nemohli zůstat kvůli transitive deps
///
/// </summary>
public partial class VsProjectsFileHelper
{
    private static XmlNode AddNewItemGroup(string pathCsproj, XmlDocument xd, XmlNamespaceManager nsmgr, XmlNode itemGroup, XmlNode project)
    {
        throw new NotImplementedException();
    }
    private static void ReplaceProjectTemplateParameter(ref string c, VsProjectTemplateParameters guid1, object guid)
    {
        throw new NotImplementedException();
    }
    public static void SetTargetFrameworksUap(string item, string target, string min)
    {
        throw new NotImplementedException();
    }
}