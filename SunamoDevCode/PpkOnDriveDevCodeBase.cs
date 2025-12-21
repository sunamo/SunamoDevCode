namespace SunamoDevCode;

public abstract class PpkOnDriveDevCodeBase<T> : List<T>
{
    #region DPP

    protected PpkOnDriveDevCodeArgs a;

    #endregion

    private bool isSaving;

    /// <summary>
    ///     Must use FileSystemWatcher, not FileSystemWatcher because its in sunamo, not desktop
    /// </summary>
    private readonly FileSystemWatcher w;

    public
#if ASYNC
        async Task
#else
void
#endif
        RemoveAll()
    {
        await Clear();
#if ASYNC
        await
#endif
            File.WriteAllTextAsync(a.file, string.Empty);
    }

    public new async Task Remove(T t)
    {
        base.Remove(t);
        await Save();
    }

    public new async Task Clear()
    {
        base.Clear();
        await Save();
    }

    public abstract
#if ASYNC
        Task
#else
void
#endif
        Load();

    public void AddWithoutSave(T t)
    {
        if (!Contains(t)) base.Add(t);
    }

    public async Task Add(IList<T> prvek)
    {
        foreach (var item in prvek) await Add(item);
    }

    public new async Task<bool> Add(T prvek)
    {
        var builder = false;
        if (!Contains(prvek))
        {
            if (prvek.ToString().Trim() != string.Empty)
            {
                base.Add(prvek);
                builder = true;
            }
            // keep on false
        }

        // keep on false
        await Save();
        return builder;
    }

    private void Load(bool loadImmediately)
    {
        if (loadImmediately) Load();
    }

    public async Task Save()
    {
        if (a.save)
        {
            isSaving = true;
            var removedOrNotExists = false;
            //if (FS.ExistsFile(a.file))
            //{
            //    removedOrNotExists = FS.TryDeleteFile(a.file);
            //}
            if (removedOrNotExists)
            {
                string obsah;
                obsah = ReturnContent();
                await File.WriteAllTextAsync(a.file, obsah);
            }

            isSaving = false;
        }
    }

    private string ReturnContent()
    {
        string obsah;
        var stringBuilder = new StringBuilder();
        foreach (var var in this) stringBuilder.AppendLine(var.ToString());
        obsah = stringBuilder.ToString();
        return obsah;
    }

    public override string ToString()
    {
        return ReturnContent();
    }

    #region base

    public PpkOnDriveDevCodeBase(PpkOnDriveDevCodeArgs a)
    {
        this.a = a;
        File.AppendAllText(a.file, "");
        //FS.CreateFileIfDoesntExists(a.file);
        Load(a.load);
        if (a.loadChangesFromDrive)
        {
            w = new FileSystemWatcher(Path.GetDirectoryName(a.file));
            w.Filter = a.file;
            w.Changed += W_Changed;
        }
    }

    private void W_Changed(object sender, FileSystemEventArgs e)
    {
        if (!isSaving) Load();
    }

    #endregion
}