namespace SunamoDevCode.SunamoSolutionsIndexer.Data.SolutionFoldersNs;

public class SolutionFoldersSerialize
{
    public List<SolutionFolderSerialize> sfs = new List<SolutionFolderSerialize>();

    public void Insert(int d, SolutionFolderSerialize sfsi)
    {
        if (sfsi == null)
        {
            return;
        }

        for (int i = 0; i < sfs.Count; i++)
        {
            if (sfs[i].fullPathFolder == sfsi.fullPathFolder)
            {
                sfs.RemoveAt(i);
                break;
            }
        }
        sfs.Insert(d, sfsi);
        if (sfs.Count > 10)
        {
            for (int i = 10; i < sfs.Count; i++)
            {
                sfs.RemoveAt(10);
            }
        }
        Update();
    }

    public event Action<List<SolutionFolderSerialize>> Updated;

    public void RemoveWithDisplayedText(string displayedText)
    {
        sfs.RemoveAll(d => d.displayedText == displayedText);
    }

    public void Update()
    {
        Updated(sfs);
    }

    /// <summary>
    /// if A2 and solution can't be found, save exception. Otherwise save in result null
    /// </summary>
    /// <param name="solutionNamesFounded"></param>
    /// <param name="canMissing"></param>
    public ResultWithExceptionDC<SolutionFoldersSerialize> GetWithName(List<string> solutionNamesFounded, bool canMissing)
    {
        ResultWithExceptionDC<SolutionFoldersSerialize> result = new ResultWithExceptionDC<SolutionFoldersSerialize>();
        result.Data = new SolutionFoldersSerialize();

        foreach (var item in solutionNamesFounded)
        {
            SolutionFolderSerialize solutionFolder = sfs.Find(d =>
            {
                if (d.nameSolution == item)
                {
                    return true;
                }
                return false;
            });

            if (solutionFolder == null)
            {
                if (!canMissing)
                {
                    result.Exc = Exceptions.ElementCantBeFound("", "solutionNamesFounded", item);
                }
            }
            else
            {
                result.Data.sfs.Add(solutionFolder);
            }
        }

        return result;
    }

    public void RemoveWithName(List<string> solutionNamesFounded)
    {
        int dex = -1;
        foreach (var item in solutionNamesFounded)
        {

            if ((dex = sfs.FindIndex(d => d.nameSolution == item)) != -1)
            {
                sfs.RemoveAt(dex);
            }



        }
    }
}
