namespace SunamoDevCode.SunamoSolutionsIndexer.Data.SolutionFolderNs;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public partial class SolutionFolder : SolutionFolderSerialize, ISolutionFolder
{
    public static Type type = typeof(SolutionFolder);
    /// <summary>
    /// Return csproj full paths in subfolders of A1 (one depth)
    /// Must use as A1 SolutionFolder, coz in CreateSolutionFolder is filled projects variable
    ///
    /// From every folder is taked all csproj => even only file name is shown is good keep also upfolder
    ///
    /// A2 whether return only names to csproj files without path
    /// </summary>
    /// <param name = "sf"></param>
    /// <param name = "onlyNames"></param>
    public static void GetCsprojs(ILogger logger, SolutionFolder sf, GetCsprojsArgs a = null)
    {
        if (a == null)
        {
            a = new GetCsprojsArgs();
        }

        // && sf.projectsGetCsprojs.Count == 0 - for better performance, when will have zero, its not VS sln
        if (sf.projectsGetCsprojs == null || a.forceReload)
        {
#if DEBUG

            if (sf.fullPathFolder.TrimEnd('\\') == @"\monoConsoleSqlClient")
            {

            }
#endif
            List<string> csprojs = new List<string>();
            var projectsFolder = SolutionsIndexerHelper.ProjectsInSolution(true, sf.fullPathFolder, false);
            foreach (var projectFolder in projectsFolder)
            {
                var files = FSGetFiles.GetFiles(logger, projectFolder, "*.csproj", SearchOption.TopDirectoryOnly, new GetFilesArgsDC { _trimA1AndLeadingBs = a.onlyNames });
                foreach (var item in files)
                {
                    csprojs.Add(item);
                }
            }

#if DEBUG
            if (sf.fullPathFolder.TrimEnd('\\') == @"\monoConsoleSqlClient")
            {

            }
#endif
            sf.projectsGetCsprojs = new List<string>(csprojs);
            sf.SourceOfProjects = SourceOfProjects.GetCsprojs;
        }
        else
        {
            if (sf.projectsGetCsprojs == null)
            {
                sf.projectsGetCsprojs = new List<string>();
            }
        }
    }

    public SolutionFolder(SolutionFolderSerialize t)
    {
        displayedText = t.displayedText;
        _fullPathFolder = t._fullPathFolder;
        _nameSolution = t._nameSolution;
        projectFolder = t.projectFolder;
        slnFullPath = t.slnFullPath;
        if (t.GetType() == type)
        {
            var t2 = (SolutionFolder)t;
            slnNameWoExt = t2.slnNameWoExt;
        }
    }

    /// <summary>
    /// C# Projects
    /// PHP PHP_Projects etc.
    /// </summary>
    public ProjectsTypes typeProjectFolder { get; set; } = ProjectsTypes.None;

    public void UpdateModules(ILogger logger, PpkOnDriveDC toSelling)
    {
        if (toSelling != null)
        {
            modulesSelling = SolutionsIndexerHelper.ModulesInSolution(logger, projectsInSolution, fullPathFolder, true, toSelling);
            modulesNotSelling = SolutionsIndexerHelper.ModulesInSolution(logger, projectsInSolution, fullPathFolder, false, toSelling);
        }
    }

    public SolutionFolder()
    {
    }

    /// <summary>
    /// SolutionFolder.GetCsprojs. SolutionsIndexerHelper.ProjectsInSolution
    /// </summary>
    public SourceOfProjects SourceOfProjects;
    List<string> _projects = new List<string>();
    /// <summary>
    /// Only name without path
    /// Is filled in ctor with CreateSolutionFolder()
    /// Only subfolders. csproj files must be find out manually
    /// Csproj are available to get with AP.GetCsprojs()
    /// </summary>
    public List<string> projectsInSolution { get => _projects; set => _projects = value; }

    List<string> _projectsGetCsprojs = null;
    /// <summary>
    /// SolutionFolder.GetCsprojs
    ///
    /// </summary>
    public List<string> projectsGetCsprojs
    {
        get
        {
            return _projectsGetCsprojs;
        }

        set
        {
            if (fullPathFolder.Contains("Mixed") && value.Count == 0)
            {
            }

            _projectsGetCsprojs = value;
        }
    }

    /// <summary>
    /// In format solution name\project name\module name
    /// </summary>
    public List<string> modulesSelling = new List<string>();
    /// <summary>
    /// In format solution name\project name\module name
    /// </summary>
    public List<string> modulesNotSelling = new List<string>();
    public string nameSolutionWithoutDiacritic = "";
    /// <summary>
    ///
    /// </summary>
    public int countOfImages = 0;
    public bool InVsFolder = false;
    public RepositoryLocal repository;
    public string slnNameWoExt = null;
    public override string ToString()
    {
        if (countOfImages != 0)
        {
            return displayedText + " (" + countOfImages.ToString() + " images)";
        }

        return displayedText;
    }

    public static bool operator>(SolutionFolder a, SolutionFolder b)
    {
        if (a.countOfImages > b.countOfImages)
        {
            return true;
        }

        return false;
    }

    public static bool operator <(SolutionFolder a, SolutionFolder b)
    {
        if (a.countOfImages < b.countOfImages)
        {
            return true;
        }

        return false;
    }
}