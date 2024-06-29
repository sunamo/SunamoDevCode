



//using TextBuilder = SunamoTextBuilder.TextBuilder;

namespace SunamoDevCode;

public class VpsHelperDevCode
{
    public static bool IsVps
    {
        get
        {
            return IsVps;
        }
    }

    public static string path
    {
        get => path;
    }

    static PushSolutionsData pushSolutionsData = new PushSolutionsData();
    //public static PpkOnDrive list = new PpkOnDrive(AppData.ci.GetFile(AppFolders.Data, "SlnVps.txt"));
    //public static PpkOnDrive listMain = new PpkOnDrive(AppData.ci.GetFile(AppFolders.Data, "SlnVpsMain.txt"));
    public static PpkOnDriveDevCode listVpsNew = new PpkOnDriveDevCode(SolutionsIndexerPaths.listVpsNew);
    public static PpkOnDriveDevCode listSczAdmin64 = new PpkOnDriveDevCode(SolutionsIndexerPaths.listSczAdmin64);

    public static void PushAll(Func<List<string>, Task<List<List<string>>>> psInvoke)
    {
        pushSolutionsData.Set(false);
        PushPullAll(psInvoke);
    }

    private static string PushPullAll(Func<List<string>, Task<List<List<string>>>> psInvoke)
    {
        if (IsVps)
        {
            var folders = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly).ToList();
            bool release = true;
            string pushArgs = string.Empty;
            string commitMessage = sess.i18n(XlfKeys.FromVPS) + " " + DateTime.Today.ToShortDateString();

            var gitBashBuilder = new GitBashBuilder(new TextBuilder());
            var gitStatus = new GitBashBuilder(new TextBuilder());

            foreach (var item in folders)
            {
                GitHelper.PushSolution(release, gitBashBuilder, pushArgs, commitMessage, item, pushSolutionsData, gitStatus, psInvoke);
            }

            //ClipboardHelper.SetText(gitBashBuilder.ToString());
            return gitBashBuilder.ToString();
        }
        else
        {
            bool release = true;
            string pushArgs = string.Empty;
            string commitMessage = sess.i18n(XlfKeys.BeforePublishingToVPS) + " " + DateTime.Today.ToShortDateString();

            var gitBashBuilder = new GitBashBuilder(new TextBuilder());
            var gitStatus = new GitBashBuilder(new TextBuilder());
            foreach (var sln in listVpsNew)
            {
                var sln2 = SolutionsIndexerHelper.SolutionWithName(sln);
                var item = sln2.fullPathFolder;
                GitHelper.PushSolution(release, gitBashBuilder, pushArgs, commitMessage, item, pushSolutionsData, gitStatus, psInvoke);
            }

            //ClipboardHelper.SetText(gitBashBuilder.ToString());
            return gitBashBuilder.ToString();
        }
    }

    public static string pullAllResult = null;

    public static string PullAll(List<string> forMore = null)
    {
        if (IsVps)
        {
            var folders = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly).ToList();
            GitHelper.PowershellForPull(folders);
        }
        else
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
        }
        //ClipboardHelper.SetText(pullAllResult);
        return pullAllResult;
    }


}
