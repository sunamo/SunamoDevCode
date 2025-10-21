// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.Aps.Data;

public class SolutionFolderDetailedInfo : SolutionFolder
{
    // TODO: whole was moved to Aps.Statistic. use interface and to initialize delegate

    public int overallFiles = 0;

    public SolutionFolderDetailedInfo(SolutionFolder sf)
    {
        countOfImages = sf.countOfImages;
        displayedText = sf.displayedText;
        fullPathFolder = sf.fullPathFolder;
        nameSolutionWithoutDiacritic = sf.nameSolutionWithoutDiacritic;
        #region Commented due to move to Aps.Statistic
        //stats = new StatisticSolution(sf.fullPathFolder);
        //overallFiles = (stats.files.Sum(d => d.filesCount) + stats.codeElements.Sum(d => d.filesCount)); 
        #endregion

        displayedText += " (" + overallFiles.ToString() + ")";
    }
}

class SolutionFolderDetailedInfoComparerBySourceFiles : IComparer<SolutionFolderDetailedInfo>
{
    public int Compare(SolutionFolderDetailedInfo x, SolutionFolderDetailedInfo y)
    {
        //  -1 protože to chci sestupně
        return x.overallFiles.CompareTo(y.overallFiles) * -1;
    }
}