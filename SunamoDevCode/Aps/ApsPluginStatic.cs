namespace SunamoDevCode.Aps;

public class ApsPluginStatic
{
    public bool Cmd
    {
        get
        {
            return ApsMainWindow.Instance.cmd;
        }
    }

    public static FoldersWithSolutionsList Fwss
    {
        get
        {
            return FoldersWithSolutions.Fwss;
        }
    }

    public static RepositoryLocal UsedRepository
    {
        set => FoldersWithSolutions.UsedRepository = value;
        get => FoldersWithSolutions.UsedRepository;
    }
}
