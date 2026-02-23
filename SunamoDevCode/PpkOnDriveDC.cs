namespace SunamoDevCode;

/// <summary>
///     Checking whether string is already contained.
/// </summary>
public class PpkOnDriveDC : PpkOnDriveDevCodeBase<string>
{
    /// <summary>
    /// Whether to remove duplicate entries when loading.
    /// </summary>
    public bool RemoveDuplicates { get; set; } = false;

    /// <summary>
    /// Creates instance with the given arguments.
    /// </summary>
    /// <param name="args">Configuration arguments for the collection.</param>
    public PpkOnDriveDC(PpkOnDriveDevCodeArgs args) : base(args)
    {
    }

    /// <summary>
    /// Creates instance from a file path with optional loading.
    /// </summary>
    /// <param name="filePath">Path to the backing file.</param>
    /// <param name="isLoading">Whether to load content immediately.</param>
    public PpkOnDriveDC(string filePath, bool isLoading = true) : base(new PpkOnDriveDevCodeArgs { File = filePath, Load = isLoading })
    {
    }

    /// <summary>
    /// Creates instance from a file path with loading and saving options.
    /// </summary>
    /// <param name="filePath">Path to the backing file.</param>
    /// <param name="isLoading">Whether to load content immediately.</param>
    /// <param name="isSaving">Whether to save content on changes.</param>
    public PpkOnDriveDC(string filePath, bool isLoading, bool isSaving) : base(new PpkOnDriveDevCodeArgs
    { File = filePath, Load = isLoading, Save = isSaving })
    {
    }

    //public static PpkOnDrive WroteOnDrive
    //{
    //    get
    //    {
    //        if (wroteOnDrive == null)
    //        {
    //            wroteOnDrive = new PpkOnDrive(AppData.Instance.GetFile(AppFolders.Logs, "WrittenFiles.txt"));
    //        }
    //        return wroteOnDrive;
    //    }
    //}
    /// <summary>
    /// Loads content from the specified file path.
    /// </summary>
    /// <param name="filePath">Path to load from.</param>
    public async Task Load(string filePath)
    {
        args.File = filePath;
        await Load();
    }

    /// <summary>
    /// Loads the collection data from the file specified in PpkOnDriveDevCodeArgs.
    /// </summary>
    public override
#if ASYNC
        async Task
#else
void
#endif
        Load()
    {
        if (File.Exists(args.File))
        {
            AddRange(SHGetLines.GetLines(
#if ASYNC
                await
#endif
                    File.ReadAllTextAsync(args.File)));
            //CA.RemoveStringsEmpty2(this);
            if (RemoveDuplicates)
            {
                //CAG.RemoveDuplicitiesList<string>(this);
                var data = this.ToList();
                await Clear();
                data = data.Distinct().ToList();
                AddRange(data);
            }
        }
    }
}