namespace SunamoDevCode._public.SunamoData.Data;

public class FromToTSHDC<T>
{
    public bool empty;
    protected long fromL;
    public FromToUseDC ftUse = FromToUseDC.DateTime;
    protected long toL;

    public FromToTSHDC()
    {
        var type = typeof(T);
        if (type == typeof(int)) ftUse = FromToUseDC.None;
    }

    // Use Empty constant outside of class
    private FromToTSHDC(bool empty) : this()
    {
        this.empty = empty;
    }

    // A3 true = DateTime, A3 false = None
    public FromToTSHDC(T from, T to, FromToUseDC ftUse = FromToUseDC.DateTime) : this()
    {
        this.from = from;
        this.to = to;
        this.ftUse = ftUse;
    }

    public T from
    {
        get => (T)(dynamic)fromL;
        set => fromL = (long)(dynamic)value!;
    }

    public T to
    {
        get => (T)(dynamic)toL;
        set => toL = (long)(dynamic)value!;
    }

    public long FromL => fromL;

    public long ToL => toL;
}
