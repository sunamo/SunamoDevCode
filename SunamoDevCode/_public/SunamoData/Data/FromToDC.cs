// variables names: ok
namespace SunamoDevCode._public.SunamoData.Data;

/// <summary>
///     Must have always entered both from and to
///     None of event could have unlimited time!
/// </summary>
public class FromToDC : FromToTSHDC<long>
{
    /// <summary>
    /// Represents an empty FromTo range.
    /// </summary>
    public static FromToDC Empty = new(true);

    /// <summary>
    /// Initializes a new instance of FromToDC class with default values.
    /// </summary>
    public FromToDC()
    {
    }

    /// <summary>
    /// Private constructor for creating empty instance. Use Empty constant outside of class.
    /// </summary>
    /// <param name="empty">Marker parameter to create empty instance.</param>
    private FromToDC(bool empty)
    {
        this.empty = empty;
    }

    /// <summary>
    /// Initializes a new instance with from and to values.
    /// </summary>
    /// <param name="from">Start value (timestamp).</param>
    /// <param name="to">End value (timestamp).</param>
    /// <param name="ftUse">Usage type - true for DateTime, false for None.</param>
    public FromToDC(long from, long to, FromToUseDC ftUse = FromToUseDC.DateTime)
    {
        this.from = from;
        this.to = to;
        this.ftUse = ftUse;
    }
}