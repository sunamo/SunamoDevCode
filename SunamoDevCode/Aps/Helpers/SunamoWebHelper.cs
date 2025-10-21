// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.Aps.Helpers;

public class SunamoWebHelper
{
    public static async Task<List<string>> ListOfSunamoWebProjects(ILogger logger, GetFileSettings getFileSettings)
    {
        List<string> csprojs = new List<string>();
        foreach (var item in ApsMainWindow.Instance.fwss)
        {
            foreach (var sln in item.Solutions(RepositoryLocal.Vs17))
            {
                if (await ApsHelper.ci.IsWebProject(logger, sln, getFileSettings))
                {
                    SolutionFolder.GetCsprojs(logger, sln);
                    csprojs.AddRange(sln.projectsGetCsprojs);
                }
            }
        }
        return csprojs;
    }
}