namespace SunamoDevCode._public;

public class AsyncLoadingBaseDC<T, ProgressBar>
{
    public ProgressBar ProgressBarInstance { get; set; } = default!;

    public long ProcessedCount { get; set; } = 0;

    public Action<T> StatusAfterLoad { get; set; } = null!;
}
