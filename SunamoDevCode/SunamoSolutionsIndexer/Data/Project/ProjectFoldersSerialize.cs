// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.SunamoSolutionsIndexer.Data.Project;

public class ProjectFoldersSerialize
{
    public List<ProjectFolderSerialize> projects = new List<ProjectFolderSerialize>();

    public ResultWithExceptionDC<ProjectFoldersSerialize> GetWithName(List<string> projectsNamesFounded, bool canMissing)
    {
        ResultWithExceptionDC<ProjectFoldersSerialize> result = new ResultWithExceptionDC<ProjectFoldersSerialize>();
        result.Data = new ProjectFoldersSerialize();

        foreach (var item in projectsNamesFounded)
        {
            ProjectFolderSerialize projectFolder = projects.Find(d =>
            {
                if (d.NameProject == item)
                {
                    return true;
                }

                return false;
            });

            if (projectFolder == null)
            {
                if (!canMissing)
                {
                    result.Exc = Exceptions.ElementCantBeFound("", "solutionNamesFounded", item);
                }
            }
            else
            {
                result.Data.projects.Add(projectFolder);
            }
        }

        return result;
    }

    public void RemoveWithName(List<string> projectNamesFounded)
    {
        int dex = -1;
        foreach (var item in projectNamesFounded)
        {
            if ((dex = projects.FindIndex(d => d.NameProject == item)) != -1)
            {
                projects.RemoveAt(dex);
            }
        }
    }
}
