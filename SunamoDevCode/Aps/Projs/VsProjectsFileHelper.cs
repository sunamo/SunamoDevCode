// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.Aps.Projs;

public partial class VsProjectsFileHelper
{
    static Type type = typeof(VsProjectsFileHelper);
    #region Bez používání externích knihoven
    /// <summary>
    /// Return paths of all csprojs
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static List<string> DuplicatedCsproj(ILogger logger, string path)
    {
        var result = FSGetFiles.GetFiles(logger, path, "*.csproj", true);
        var allCsproj = result.ToList();
        CAChangeContent.ChangeContent0(null, allCsproj, FS.GetFileName /*SHParts.RemoveAfterLast, "\\"*/);
        allCsproj.RemoveAll(d => d == "Runner.csproj");
        var dupl = CAG.GetDuplicities(allCsproj);
        if (dupl.Count > 0)
        {
            ThrowEx.Custom("Some csprojs have duplicated file names: " + SHJoin.JoinNL(dupl) + ". List was also copied to clipboard.");
        }
        return result;
    }
    #endregion
}