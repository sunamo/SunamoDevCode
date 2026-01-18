namespace SunamoDevCode;

public abstract class PpkOnDriveDevCodeBase<T> : List<T>
{
    #region DPP

    protected PpkOnDriveDevCodeArgs args;

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
            File.WriteAllTextAsync(args.File, string.Empty);
    }

    public new async Task Remove(T value)
    {
        base.Remove(value);
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

    public void AddWithoutSave(T value)
    {
        if (!Contains(value)) base.Add(value);
    }

    public async Task Add(IList<T> items)
    {
        foreach (var item in items) await Add(item);
    }

    public new async Task<bool> Add(T value)
    {
        var wasAdded = false;
        if (!Contains(value))
        {
            if (value.ToString().Trim() != string.Empty)
            {
                base.Add(value);
                wasAdded = true;
            }
            // keep on false
        }

        // keep on false
        await Save();
        return wasAdded;
    }

    private void Load(bool loadImmediately)
    {
        if (loadImmediately) Load();
    }

    public async Task Save()
    {
        if (args.Save)
        {
            isSaving = true;
            var removedOrNotExists = false;
            //if (FS.ExistsFile(args.File))
            //{
            //    removedOrNotExists = FS.TryDeleteFile(args.File);
            //}
            if (removedOrNotExists)
            {
                string content;
                content = ReturnContent();
                await File.WriteAllTextAsync(args.File, content);
            }

            isSaving = false;
        }
    }

    private string ReturnContent()
    {
        string content;
        var stringBuilder = new StringBuilder();
        foreach (var item in this) stringBuilder.AppendLine(item.ToString());
        content = stringBuilder.ToString();
        return content;
    }

    public override string ToString()
    {
        return ReturnContent();
    }

    #region base

    public PpkOnDriveDevCodeBase(PpkOnDriveDevCodeArgs args)
    {
        this.args = args;
        File.AppendAllText(args.File, "");
        //FS.CreateFileIfDoesntExists(args.File);
        Load(args.Load);
        if (args.LoadChangesFromDrive)
        {
            w = new FileSystemWatcher(Path.GetDirectoryName(args.File));
            w.Filter = args.File;
            w.Changed += W_Changed;
        }
    }

    private void W_Changed(object sender, FileSystemEventArgs e)
    {
        if (!isSaving) Load();
    }

    #endregion
}