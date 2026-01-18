namespace SunamoDevCode.Aps.Helpers;

public class SunamoWebHelper
{
    public static async Task<List<string>> ListOfSunamoWebProjects(ILogger logger, GetFileSettings getFileSettings)
    {
        List<string> csprojs = new List<string>();
        foreach (var item in ApsMainWindow.Instance.Fwss)
        {
            foreach (var sln in item.GetSolutions(RepositoryLocal.Vs17))
            {
                if (await ApsHelper.Instance.IsWebProject(logger, sln, getFileSettings))
                {
                    SolutionFolder.GetCsprojs(logger, sln);
                    csprojs.AddRange(sln.ProjectsGetCsprojs);
                }
            }
        }
        return csprojs;
    }
}