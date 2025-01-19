namespace SunamoDevCode._public.SunamoData.Data;

/// <summary>
///     Must have always entered both from and to
///     None of event could have unlimited time!
/// </summary>
public class FromToDC : FromToTSHDC<long>
{
    public static FromToDC Empty = new(true);
    public FromToDC()
    {
    }
    /// <summary>
    ///     Use Empty contstant outside of class
    /// </summary>
    /// <param name="empty"></param>
    private FromToDC(bool empty)
    {
        this.empty = empty;
    }
    /// <summary>
    ///     A3 true = DateTime
    ///     A3 False = None
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="ftUse"></param>
    public FromToDC(long from, long to, FromToUseDC ftUse = FromToUseDC.DateTime)
    {
        this.from = from;
        this.to = to;
        this.ftUse = ftUse;
    }
}
