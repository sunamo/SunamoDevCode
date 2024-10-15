namespace SunamoDevCode._public.SunamoData.Data;

public class FromToTSHDevCode<T>
{
    public bool empty;
    protected long fromL;
    public FromToUseDevCode ftUse = FromToUseDevCode.DateTime;
    protected long toL;
    public FromToTSHDevCode()
    {
        var t = typeof(T);
        if (t == typeof(int)) ftUse = FromToUseDevCode.None;
    }
    /// <summary>
    ///     Use Empty contstant outside of class
    /// </summary>
    /// <param name="empty"></param>
    private FromToTSHDevCode(bool empty) : this()
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
    public FromToTSHDevCode(T from, T to, FromToUseDevCode ftUse = FromToUseDevCode.DateTime) : this()
    {
        this.from = from;
        this.to = to;
        this.ftUse = ftUse;
    }
    public T from
    {
        get => (T)(dynamic)fromL;
        set => fromL = (long)(dynamic)value;
    }
    public T to
    {
        get => (T)(dynamic)toL;
        set => toL = (long)(dynamic)value;
    }
    public long FromL => fromL;
    public long ToL => toL;
}
