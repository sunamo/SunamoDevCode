namespace SunamoDevCode.SunamoSolutionsIndexer.Data.Project;

public class ProjectFolderSerialize
{
    // TODO: Merge with SolutionInListBox

    SolutionFolderSerialize solution;
    string nameProject;
    /// <summary>
    /// Is filled in ctor
    /// </summary>
    string displayedText;

    public SolutionFolderSerialize Solution
    {
        get
        {
            return solution;
        }
    }
    public string NameProject
    {
        get
        {
            return nameProject;
        }
    }

    public ProjectFolderSerialize(SolutionFolderSerialize solution, string nameProject)
    {
        this.solution = solution;
        this.nameProject = nameProject;
        displayedText = SolutionsIndexerHelper.GetDisplayedSolutionName(Path.Combine(solution.fullPathFolder, nameProject));
    }

    public override string ToString()
    {
        return displayedText;
    }
}
