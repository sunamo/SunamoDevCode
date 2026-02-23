namespace SunamoDevCode._public;

/// <summary>
/// Interface for progress bar operations supporting percent-based tracking.
/// </summary>
public interface IProgressBarDC
{
    /// <summary>
    /// Gets or sets whether this progress bar is registered with the system.
    /// </summary>
    bool IsRegistered { get; set; }
    /// <summary>
    /// Gets or sets the divisor for write frequency (only writes progress when count is divisible by this).
    /// </summary>
    int WriteOnlyDividableBy { get; set; }
    /// <summary>
    /// Initializes the progress bar with a percent calculator.
    /// </summary>
    /// <param name="pc">Percent calculator instance.</param>
    void Init(IPercentCalculatorDC pc);
    /// <summary>
    /// Initializes the progress bar with a percent calculator and unit test flag.
    /// </summary>
    /// <param name="pc">Percent calculator instance.</param>
    /// <param name="isNotUt">Whether this is not a unit test context.</param>
    void Init(IPercentCalculatorDC pc, bool isNotUt);
    /// <summary>
    /// Marks one item as completed with an async result. Increments done items after a finished async operation.
    /// </summary>
    /// <param name="asyncResult">Result of the completed async operation.</param>
    void DoneOne(object asyncResult);
    /// <summary>
    /// Marks one item as completed.
    /// </summary>
    void DoneOne();
    /// <summary>
    /// Marks the specified number of items as completed.
    /// </summary>
    /// <param name="count">Number of items completed.</param>
    void DoneOne(int count);
    /// <summary>
    /// Starts the progress bar with the specified total item count.
    /// </summary>
    /// <param name="totalCount">Total number of items to process.</param>
    void Start(int totalCount);
    /// <summary>
    /// Marks the entire operation as complete.
    /// </summary>
    void Done();
}