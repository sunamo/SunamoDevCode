namespace SunamoDevCode.SunamoSolutionsIndexer.Data.SolutionFoldersNs;

public class SolutionFoldersSerialize
{
    public List<SolutionFolderSerialize> SolutionFolders = new List<SolutionFolderSerialize>();

    public void Insert(int index, SolutionFolderSerialize solutionFolder)
    {
        if (solutionFolder == null)
        {
            return;
        }

        for (int i = 0; i < SolutionFolders.Count; i++)
        {
            if (SolutionFolders[i].FullPathFolder == solutionFolder.FullPathFolder)
            {
                SolutionFolders.RemoveAt(i);
                break;
            }
        }
        SolutionFolders.Insert(index, solutionFolder);
        if (SolutionFolders.Count > 10)
        {
            for (int i = 10; i < SolutionFolders.Count; i++)
            {
                SolutionFolders.RemoveAt(10);
            }
        }
        Update();
    }

    public event Action<List<SolutionFolderSerialize>>? Updated;

    public void RemoveWithDisplayedText(string displayedText)
    {
        SolutionFolders.RemoveAll(folder => folder.DisplayedText == displayedText);
    }

    public void Update()
    {
        Updated?.Invoke(SolutionFolders);
    }

    public ResultWithExceptionDC<SolutionFoldersSerialize> GetWithName(List<string> solutionNamesToFind, bool isMissingAllowed)
    {
        var result = new ResultWithExceptionDC<SolutionFoldersSerialize>();
        result.Data = new SolutionFoldersSerialize();

        foreach (var solutionName in solutionNamesToFind)
        {
            SolutionFolderSerialize? solutionFolder = SolutionFolders.Find(folder =>
            {
                if (folder.NameSolution == solutionName)
                {
                    return true;
                }
                return false;
            });

            if (solutionFolder == null)
            {
                if (!isMissingAllowed)
                {
                    result.Exc = Exceptions.ElementCantBeFound("", "solutionNamesToFind", solutionName)!;
                }
            }
            else
            {
                result.Data.SolutionFolders.Add(solutionFolder);
            }
        }

        return result;
    }

    public void RemoveWithName(List<string> solutionNamesToRemove)
    {
        int index = -1;
        foreach (var solutionName in solutionNamesToRemove)
        {
            if ((index = SolutionFolders.FindIndex(folder => folder.NameSolution == solutionName)) != -1)
            {
                SolutionFolders.RemoveAt(index);
            }
        }
    }
}
