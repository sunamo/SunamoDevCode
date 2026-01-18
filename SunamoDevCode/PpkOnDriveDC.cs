namespace SunamoDevCode;

/// <summary>
///     Checking whether string is already contained.
/// </summary>
public class PpkOnDriveDC : PpkOnDriveDevCodeBase<string>
{
    private static PpkOnDriveDC wroteOnDrive = null;
    public bool RemoveDuplicates { get; set; } = false;

    public PpkOnDriveDC(PpkOnDriveDevCodeArgs args) : base(args)
    {
    }

    public PpkOnDriveDC(string filePath, bool isLoading = true) : base(new PpkOnDriveDevCodeArgs { File = filePath, Load = isLoading })
    {
    }

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
    public async Task Load(string filePath)
    {
        args.File = filePath;
        await Load();
    }

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