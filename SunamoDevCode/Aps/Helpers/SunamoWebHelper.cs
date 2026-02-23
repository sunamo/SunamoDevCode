namespace SunamoDevCode.Aps.Helpers;

/// <summary>
/// Provides helper methods for working with Sunamo web projects.
/// </summary>
public class SunamoWebHelper
{
    /// <summary>
    /// Gets a list of all Sunamo web project csproj paths from the solution folders.
    /// </summary>
    /// <param name="logger">Logger instance for logging operations.</param>
    /// <param name="getFileSettings">Settings for file retrieval.</param>
    /// <returns>List of csproj file paths that are web projects.</returns>
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