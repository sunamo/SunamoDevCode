namespace SunamoDevCode._public;

/// <summary>
/// Base class for asynchronous loading operations with progress tracking.
/// </summary>
/// <typeparam name="T">Type of the status object.</typeparam>
/// <typeparam name="ProgressBar">Type of the progress bar control.</typeparam>
public class AsyncLoadingBaseDC<T, ProgressBar>
{
    /// <summary>
    /// Gets or sets the progress bar instance for displaying load progress.
    /// </summary>
    public ProgressBar ProgressBarInstance { get; set; }

    /// <summary>
    /// Gets or sets the count of processed items.
    /// </summary>
    public long ProcessedCount { get; set; } = 0;

    /// <summary>
    /// Gets or sets the action to execute after load operation completes.
    /// </summary>
    public Action<T> StatusAfterLoad { get; set; }
}