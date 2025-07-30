namespace SunamoDevCode.SunamoSolutionsIndexer;

public class VpsHelperDevCode
{
    static PushSolutionsData pushSolutionsData = new PushSolutionsData();
    //public static PpkOnDrive list = new PpkOnDrive(AppData.ci.GetFile(AppFolders.Data, "SlnVps.txt"));
    //public static PpkOnDrive listMain = new PpkOnDrive(AppData.ci.GetFile(AppFolders.Data, "SlnVpsMain.txt"));
    public static PpkOnDriveDC listVpsNew = new PpkOnDriveDC(SolutionsIndexerPaths.listVpsNew);
    public static PpkOnDriveDC listSczAdmin64 = new PpkOnDriveDC(SolutionsIndexerPaths.listSczAdmin64);

    public static async Task PushAll(Func<List<string>, Task<List<List<string>>>> psInvoke)
    {
        pushSolutionsData.Set(false);
        await PushPullAll(psInvoke);
    }

    private static async Task<string> PushPullAll(Func<List<string>, Task<List<List<string>>>> psInvoke)
    {

        bool release = true;
        string pushArgs = string.Empty;
        string commitMessage = Translate.FromKey(XlfKeys.BeforePublishingToVPS) + " " + DateTime.Today.ToShortDateString();

        var gitBashBuilder = new GitBashBuilder(new TextBuilderDC());
        var gitStatus = new GitBashBuilder(new TextBuilderDC());
        foreach (var sln in listVpsNew)
        {
            var sln2 = SolutionsIndexerHelper.SolutionWithName(sln);
            var item = sln2.fullPathFolder;
            await GitHelper.PushSolution(release, gitBashBuilder, pushArgs, commitMessage, item, pushSolutionsData, gitStatus, psInvoke);
        }

        //ClipboardHelper.SetText(gitBashBuilder.ToString());
        return gitBashBuilder.ToString();
    }

    public static string pullAllResult = null;

    public static string PullAll()
    {

        // pokračovat s přidáním forMore in close

        List<string> paths = new List<string>();

        foreach (var item in listVpsNew)
        {
            var sln = SolutionsIndexerHelper.SolutionWithName(item);

            if (sln != null)
            {
                paths.Add(sln.fullPathFolder);
            }
            else
            {
                //ThisApp.Warning(item + " solution was not found");
            }
        }

        pullAllResult = GitHelper.PowershellForPull(paths);

        //ClipboardHelper.SetText(pullAllResult);
        return pullAllResult;
    }
}