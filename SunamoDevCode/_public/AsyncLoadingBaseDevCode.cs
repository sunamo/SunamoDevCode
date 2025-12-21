namespace SunamoDevCode._public;

public class AsyncLoadingBaseDC<T, ProgressBar>
{
    public ProgressBar pb;
    public long processedCount = 0;
    public Action<T> statusAfterLoad;
}