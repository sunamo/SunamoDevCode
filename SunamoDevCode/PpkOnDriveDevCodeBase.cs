namespace SunamoDevCode;

/// <summary>
/// Abstract base class for persistent collections stored on drive, with file watching and auto-save capabilities.
/// </summary>
/// <typeparam name="T">Type of elements in the collection.</typeparam>
public abstract class PpkOnDriveDevCodeBase<T> : List<T>
{
    #region DPP

    /// <summary>
    /// Configuration arguments for persistence behavior.
    /// </summary>
    protected PpkOnDriveDevCodeArgs args;

    #endregion

    private bool isSaving;

    /// <summary>
    ///     Must use FileSystemWatcher, not FileSystemWatcher because its in sunamo, not desktop
    /// </summary>
    private readonly FileSystemWatcher w = null!;

    /// <summary>
    /// Removes all items from the collection and clears the backing file.
    /// </summary>
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

    /// <summary>
    /// Removes the specified item and saves the collection to disk.
    /// </summary>
    /// <param name="value">Item to remove.</param>
    public new async Task Remove(T value)
    {
        base.Remove(value);
        await Save();
    }

    /// <summary>
    /// Clears all items and saves the empty collection to disk.
    /// </summary>
    public new async Task Clear()
    {
        base.Clear();
        await Save();
    }

    /// <summary>
    /// Loads the collection contents from the backing file on disk.
    /// </summary>
    public abstract
#if ASYNC
        Task
#else
void
#endif
        Load();

    /// <summary>
    /// Adds an item to the collection without triggering a save to disk.
    /// </summary>
    /// <param name="value">Item to add.</param>
    public void AddWithoutSave(T value)
    {
        if (!Contains(value)) base.Add(value);
    }

    /// <summary>
    /// Adds multiple items to the collection, saving after each addition.
    /// </summary>
    /// <param name="items">Items to add.</param>
    public async Task Add(IList<T> items)
    {
        foreach (var item in items) await Add(item);
    }

    /// <summary>
    /// Adds an item if not already present and the value is non-empty, then saves to disk.
    /// </summary>
    /// <param name="value">Item to add.</param>
    /// <returns>True if the item was actually added.</returns>
    public new async Task<bool> Add(T value)
    {
        var wasAdded = false;
        if (!Contains(value))
        {
            if (value!.ToString()!.Trim() != string.Empty)
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

    /// <summary>
    /// Persists the current collection contents to the backing file on disk.
    /// </summary>
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
        foreach (var item in this) stringBuilder.AppendLine(item!.ToString());
        content = stringBuilder.ToString();
        return content;
    }

    /// <summary>
    /// Returns all items as a newline-separated string.
    /// </summary>
    /// <returns>String representation of all items.</returns>
    public override string ToString()
    {
        return ReturnContent();
    }

    #region base

    /// <summary>
    /// Initializes the persistent collection with the specified arguments, creating the file if needed and optionally watching for changes.
    /// </summary>
    /// <param name="args">Configuration arguments for persistence behavior.</param>
    public PpkOnDriveDevCodeBase(PpkOnDriveDevCodeArgs args)
    {
        this.args = args;
        File.AppendAllText(args.File, "");
        //FS.CreateFileIfDoesntExists(args.File);
        Load(args.Load);
        if (args.LoadChangesFromDrive)
        {
            w = new FileSystemWatcher(Path.GetDirectoryName(args.File)!);
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