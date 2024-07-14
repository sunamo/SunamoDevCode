namespace SunamoDevCode._public.SunamoData.Data;

/// <summary>
///     Must have always entered both from and to
///     None of event could have unlimited time!
/// </summary>
public class FromToDevCode : FromToTSHDevCode<long>
{
    public static FromToDevCode Empty = new(true);
    public FromToDevCode()
    {
    }
    /// <summary>
    ///     Use Empty contstant outside of class
    /// </summary>
    /// <param name="empty"></param>
    private FromToDevCode(bool empty)
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
    public FromToDevCode(long from, long to, FromToUseDevCode ftUse = FromToUseDevCode.DateTime)
    {
        this.from = from;
        this.to = to;
        this.ftUse = ftUse;
    }
}
