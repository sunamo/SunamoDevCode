namespace SunamoDevCode.SunamoSolutionsIndexer.Data.SolutionFolderNs;

public partial class SolutionFolder : SolutionFolderSerialize, ISolutionFolder
{
    public new static Type Type { get; } = typeof(SolutionFolder);

    public static void GetCsprojs(ILogger logger, SolutionFolder solutionFolder, GetCsprojsArgs? args = null)
    {
        args ??= new GetCsprojsArgs();

        if (solutionFolder.ProjectsGetCsprojs == null || args.ForceReload)
        {
            var csprojs = new List<string>();
            var projectsFolder = SolutionsIndexerHelper.ProjectsInSolution(true, solutionFolder.FullPathFolder, false);
            foreach (var projectFolder in projectsFolder)
            {
                var files = FSGetFiles.GetFiles(logger, projectFolder, "*.csproj", SearchOption.TopDirectoryOnly, new GetFilesArgsDC { TrimA1AndLeadingBs = args.OnlyNames });
                foreach (var item in files)
                {
                    csprojs.Add(item);
                }
            }

            solutionFolder.ProjectsGetCsprojs = new List<string>(csprojs);
            solutionFolder.SourceOfProjects = SourceOfProjects.GetCsprojs;
        }
        else
        {
            if (solutionFolder.ProjectsGetCsprojs == null)
            {
                solutionFolder.ProjectsGetCsprojs = new List<string>();
            }
        }
    }

    public SolutionFolder(SolutionFolderSerialize source)
    {
        DisplayedText = source.DisplayedText;
        _FullPathFolder = source._FullPathFolder;
        _NameSolution = source._NameSolution;
        projectFolder = source.projectFolder;
        slnFullPath = source.slnFullPath;
        if (source.GetType() == Type)
        {
            var sourceSolutionFolder = (SolutionFolder)source;
            SlnNameWithoutExtension = sourceSolutionFolder.SlnNameWithoutExtension;
        }
    }

    public ProjectsTypes TypeProjectFolder { get; set; } = ProjectsTypes.None;

    public void UpdateModules(ILogger logger, PpkOnDriveDC toSelling)
    {
        if (toSelling != null)
        {
            ModulesSelling = SolutionsIndexerHelper.ModulesInSolution(logger, ProjectsInSolution, FullPathFolder, true, toSelling);
            ModulesNotSelling = SolutionsIndexerHelper.ModulesInSolution(logger, ProjectsInSolution, FullPathFolder, false, toSelling);
        }
    }

    public SolutionFolder()
    {
    }

    public SourceOfProjects SourceOfProjects { get; set; }

    private List<string> _projects = new List<string>();

    public List<string> ProjectsInSolution { get => _projects; set => _projects = value; }

    private List<string>? _projectsGetCsprojs = null;

    public List<string> ProjectsGetCsprojs
    {
        get => _projectsGetCsprojs!;
        set => _projectsGetCsprojs = value;
    }

    public List<string> ModulesSelling { get; set; } = new List<string>();

    public List<string> ModulesNotSelling { get; set; } = new List<string>();

    public string NameSolutionWithoutDiacritic { get; set; } = "";

    public int CountOfImages { get; set; } = 0;

    public bool InVsFolder { get; set; } = false;

    public RepositoryLocal Repository { get; set; }

    public string? SlnNameWithoutExtension { get; set; } = null;

    public override string ToString()
    {
        if (CountOfImages != 0)
        {
            return DisplayedText + " (" + CountOfImages.ToString() + " images)";
        }

        return DisplayedText;
    }

    public static bool operator>(SolutionFolder left, SolutionFolder right)
    {
        return left.CountOfImages > right.CountOfImages;
    }

    public static bool operator <(SolutionFolder left, SolutionFolder right)
    {
        return left.CountOfImages < right.CountOfImages;
    }
}
