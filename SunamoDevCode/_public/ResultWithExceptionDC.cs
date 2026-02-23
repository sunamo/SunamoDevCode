namespace SunamoDevCode._public;

/// <summary>
/// Wraps a result value of type T together with an optional exception message.
/// </summary>
/// <typeparam name="T">Type of the result data.</typeparam>
public class ResultWithExceptionDC<T>
{
    /// <summary>
    /// Gets or sets the result data.
    /// </summary>
    public T Data { get; set; } = default!;




    /// <summary>
    /// Gets or sets the exception message if the operation failed.
    /// </summary>
    public string Exc { get; set; } = null!;
    /// <summary>
    /// Initializes a new result with the specified data.
    /// </summary>
    /// <param name="data">Result data value.</param>
    public ResultWithExceptionDC(T data)
    {
        Data = data;
    }
    /// <summary>
    /// Initializes a new result with an exception message string.
    /// </summary>
    /// <param name="exc">Exception message text.</param>
    public ResultWithExceptionDC(string exc)
    {
        this.Exc = exc;
    }
    /// <summary>
    /// Initializes a new result from an exception, extracting its text.
    /// </summary>
    /// <param name="exc">Exception to extract text from.</param>
    public ResultWithExceptionDC(Exception exc)
    {
        this.Exc = Exceptions.TextOfExceptions(exc);
    }



    /// <summary>
    /// Initializes an empty result with default values.
    /// </summary>
    public ResultWithExceptionDC()
    {
    }
}