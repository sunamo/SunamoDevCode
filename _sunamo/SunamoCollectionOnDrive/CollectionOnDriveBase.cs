namespace SunamoDevCode._sunamo.SunamoCollectionOnDrive;

internal abstract class CollectionOnDriveBase<T>(ILogger logger) : List<T>
{
    /// <summary>
    /// whether duplicates should be removed on load and whether duplicate items should not even be saved
    /// </summary>
    protected bool removeDuplicates = false;
    protected CollectionOnDriveArgs a = new();
    private bool isSaving;
    private FileSystemWatcher? w;
    internal async Task RemoveAll()
    {
        await ClearWithSave();
        await File.WriteAllTextAsync(a.path, string.Empty);
    }
    internal async Task RemoveWithSave(T t)
    {
        Remove(t);
        await Save();
    }
    internal async Task ClearWithSave()
    {
        Clear();
        await Save();
    }
    internal abstract Task Load(bool removeDuplicates);
    /// <summary>
    /// Check whether T is already contained.
    /// </summary>
    /// <param name="t"></param>
    internal virtual void AddWithoutSave(T t)
    {
        if (logger == NullLogger.Instance)
        {
            ThrowEx.UseNonDummyCollection();
        }
        if (removeDuplicates)
        {
            if (!Contains(t))
            {
                Add(t);
            }
        }
        else
        {
            Add(t);
        }
    }
    /// <summary>
    /// Check whether T is already contained.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    internal virtual async Task<bool> AddWithSave(T? element)
    {
        if (logger == NullLogger.Instance)
        {
            ThrowEx.UseNonDummyCollection();
        }
        if (element is null)
        {
            throw new Exception($"{nameof(element)} is null");
        }
        var wasChanged = false;
        if (removeDuplicates)
        {
            if (!Contains(element))
            {
                var ts = element.ToString() ?? throw new Exception($"ToString of type ${element} cannot return null");
                if (ts.Trim() != string.Empty)
                {
                    Add(element);
                    wasChanged = true;
                }
            }
        }
        else
        {
            Add(element);
            wasChanged = true;
        }
        if (wasChanged)
        {
            await Save();
        }
        return wasChanged;
    }
    internal async Task Save()
    {
        isSaving = true;
        await File.WriteAllTextAsync(a.path, SHJoin.JoinNL(this));
        isSaving = false;
    }
    public override string ToString()
    {
        return SHJoin.JoinNL(this);
    }
    #region ctor
    /// <summary>
    /// optional call only if you want to set by CollectionOnDriveArgs. Calling Load() for already existing records is important.
    /// </summary>
    /// <param name="a"></param>
    internal void Init(CollectionOnDriveArgs a)
    {
        this.a = a;
        if (a.loadChangesFromDrive)
        {
            var up = Path.GetDirectoryName(a.path);
            if (up is null)
            {
                logger.LogWarning("FileSystemWatcher cannot be registered because null value");
                return;
            }
            else
            {
                w = new FileSystemWatcher
                {
                    Path = a.path
                };
                w.Changed += W_Changed;
            }
        }
    }
    private void W_Changed(object sender, FileSystemEventArgs e)
    {
        if (!isSaving)
            Load(removeDuplicates);
    }
    #endregion
}