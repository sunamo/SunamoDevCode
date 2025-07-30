namespace SunamoDevCode;

/// <summary>
///     Checking whether string is already contained.
/// </summary>
public class PpkOnDriveDC : PpkOnDriveDevCodeBase<string>
{
    private static PpkOnDriveDC wroteOnDrive = null;
    public bool removeDuplicates = false;

    public PpkOnDriveDC(PpkOnDriveDevCodeArgs a) : base(a)
    {
    }

    public PpkOnDriveDC(string file2, bool load = true) : base(new PpkOnDriveDevCodeArgs { file = file2, load = load })
    {
    }

    public PpkOnDriveDC(string file, bool load, bool save) : base(new PpkOnDriveDevCodeArgs
    { file = file, load = load, save = save })
    {
    }

    //public static PpkOnDrive WroteOnDrive
    //{
    //    get
    //    {
    //        if (wroteOnDrive == null)
    //        {
    //            wroteOnDrive = new PpkOnDrive(AppData.ci.GetFile(AppFolders.Logs, "WrittenFiles.txt"));
    //        }
    //        return wroteOnDrive;
    //    }
    //}
    public async Task Load(string file)
    {
        a.file = file;
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
        if (File.Exists(a.file))
        {
            AddRange(SHGetLines.GetLines(
#if ASYNC
                await
#endif
                    File.ReadAllTextAsync(a.file)));
            //CA.RemoveStringsEmpty2(this);
            if (removeDuplicates)
            {
                //CAG.RemoveDuplicitiesList<string>(this);
                var d = this.ToList();
                await Clear();
                d = d.Distinct().ToList();
                AddRange(d);
            }
        }
    }
}