namespace SunamoDevCode;


/// <summary>
/// Checking whether string is already contained.
/// </summary>
internal class PpkOnDrive : PpkOnDriveBase<string>
{
    internal bool removeDuplicates = false;
    static PpkOnDrive wroteOnDrive = null;
    //internal static PpkOnDrive WroteOnDrive
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
    internal async Task Load(string file)
    {
        a.file = file;
        await Load();
    }
    internal override
#if ASYNC
    async Task
#else
void
#endif
    Load()
    {
        if (File.Exists(a.file))
        {
            this.AddRange(SHGetLines.GetLines(
#if ASYNC
            await
#endif
            File.ReadAllTextAsync(a.file)));
            //CASH.RemoveStringsEmpty2(this);
            if (removeDuplicates)
            {
                //CAG.RemoveDuplicitiesList<string>(this);
                var d = this.ToList();
                this.Clear();
                d = d.Distinct().ToList();
                this.AddRange(d);
            }
        }
    }
    internal PpkOnDrive(PpkOnDriveArgs a) : base(a)
    {
    }
    internal PpkOnDrive(string file2, bool load = true) : base(new PpkOnDriveArgs { file = file2, load = load })
    {
    }
    internal PpkOnDrive(string file, bool load, bool save) : base(new PpkOnDriveArgs { file = file, load = load, save = save })
    {
    }
}