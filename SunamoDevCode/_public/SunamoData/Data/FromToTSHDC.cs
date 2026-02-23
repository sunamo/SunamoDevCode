namespace SunamoDevCode._public.SunamoData.Data;

/// <summary>
/// Represents a from-to range using long-backed values, supporting DateTime or integer range semantics.
/// </summary>
/// <typeparam name="T">Type of the range values (typically int or long).</typeparam>
public class FromToTSHDC<T>
{
    /// <summary>
    /// Whether this range is empty (no values set).
    /// </summary>
    public bool empty;
    /// <summary>
    /// Internal long representation of the "from" value.
    /// </summary>
    protected long fromL;
    /// <summary>
    /// Determines how this from-to range should be interpreted (DateTime or None).
    /// </summary>
    public FromToUseDC ftUse = FromToUseDC.DateTime;
    /// <summary>
    /// Internal long representation of the "to" value.
    /// </summary>
    protected long toL;
    /// <summary>
    /// Initializes a new FromToTSHDC, setting ftUse based on the generic type T.
    /// </summary>
    public FromToTSHDC()
    {
        var type = typeof(T);
        if (type == typeof(int)) ftUse = FromToUseDC.None;
    }
    /// <summary>
    ///     Use Empty contstant outside of class
    /// </summary>
    /// <param name="empty"></param>
    private FromToTSHDC(bool empty) : this()
    {
        this.empty = empty;
    }
    /// <summary>
    ///     A3 true = DateTime
    ///     A3 False = None
    /// </summary>
    /// <param name="from">Start of the range.</param>
    /// <param name="to">End of the range.</param>
    /// <param name="ftUse">How to interpret the range (DateTime or None).</param>
    public FromToTSHDC(T from, T to, FromToUseDC ftUse = FromToUseDC.DateTime) : this()
    {
        this.from = from;
        this.to = to;
        this.ftUse = ftUse;
    }
    /// <summary>
    /// Gets or sets the start of the range, stored internally as a long.
    /// </summary>
    public T from
    {
        get => (T)(dynamic)fromL;
        set => fromL = (long)(dynamic)value!;
    }
    /// <summary>
    /// Gets or sets the end of the range, stored internally as a long.
    /// </summary>
    public T to
    {
        get => (T)(dynamic)toL;
        set => toL = (long)(dynamic)value!;
    }
    /// <summary>
    /// Gets the raw long value of the "from" boundary.
    /// </summary>
    public long FromL => fromL;
    /// <summary>
    /// Gets the raw long value of the "to" boundary.
    /// </summary>
    public long ToL => toL;
}