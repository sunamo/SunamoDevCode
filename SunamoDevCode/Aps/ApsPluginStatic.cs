namespace SunamoDevCode.Aps;

public class ApsPluginStatic
{
    public bool cmd
    {
        get
        {
            return ApsMainWindow.Instance.cmd;
        }
    }
    public static FoldersWithSolutionsList fwss
    {
        get
        {
            return FoldersWithSolutions.fwss;
        }
    }

    public static RepositoryLocal usedRepository
    {
        set => FoldersWithSolutions.usedRepository = value;
        get => FoldersWithSolutions.usedRepository;
    }



    //compareInCheckBoxListUC
}