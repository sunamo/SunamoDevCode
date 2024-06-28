namespace SunamoDevCode;

using System.Text;

internal abstract class PpkOnDriveBase<T> : List<T>
{
    #region DPP
    protected PpkOnDriveArgs a = null;
    #endregion
    public
#if ASYNC
    async Task
#else
void
#endif
    RemoveAll()
    {
        Clear();
#if ASYNC
        await
#endif
        File.WriteAllTextAsync(a.file, string.Empty);
    }
    internal new void Remove(T t)
    {
        base.Remove(t);
        Save();
    }
    internal new void Clear()
    {
        base.Clear();
        Save();
    }
    internal abstract
#if ASYNC
    Task
#else
void
#endif
    Load();
    internal void AddWithoutSave(T t)
    {
        if (!Contains(t))
        {
            base.Add(t);
        }
    }
    internal void Add(IList<T> prvek)
    {
        foreach (var item in prvek)
        {
            Add(item);
        }
    }
    internal new bool Add(T prvek)
    {
        bool b = false;
        if (!Contains(prvek))
        {
            if (prvek.ToString().Trim() != string.Empty)
            {
                base.Add(prvek);
                b = true;
            }
            else
            {
                // keep on false
            }
        }
        else
        {
            // keep on false
        }
        Save();
        return b;
    }
    bool isSaving = false;
    /// <summary>
    /// Must use FileSystemWatcher, not FileSystemWatcher because its in sunamo, not desktop
    /// </summary>
    FileSystemWatcher w = null;
    #region base
    internal PpkOnDriveBase(PpkOnDriveArgs a)
    {
        this.a = a;
        File.AppendAllText(a.file, "");
        //FSSH.CreateFileIfDoesntExists(a.file);
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
        if (!isSaving)
        {
            Load();
        }
    }
    #endregion
    private void Load(bool loadImmediately)
    {
        if (loadImmediately)
        {
            Load();
        }
    }
    internal async Task Save()
    {
        if (a.save)
        {
            isSaving = true;
            bool removedOrNotExists = false;
            //if (FSSH.ExistsFile(a.file))
            //{
            //    removedOrNotExists = FSSH.TryDeleteFile(a.file);
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
        StringBuilder sb = new StringBuilder();
        foreach (T var in this)
        {
            sb.AppendLine(var.ToString());
        }
        obsah = sb.ToString();
        return obsah;
    }
    internal override string ToString()
    {
        return ReturnContent();
    }
}