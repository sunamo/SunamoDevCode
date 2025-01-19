namespace SunamoDevCode._public.SunamoCollectionWithoutDuplicates;


public abstract class CollectionWithoutDuplicatesBaseDC<T> 
{
    public List<T> c = null;
    public List<string> sr = null;
    bool? _allowNull = false;





    public bool? allowNull
    {
        get => _allowNull;
        set
        {
            _allowNull = value;
            if (value.HasValue && value.Value)
            {
                sr = new List<string>(count);
            }
        }
    }
    public static bool br = false;
    int count = 10000;
    public CollectionWithoutDuplicatesBaseDC()
    {
        if (br)
        {
            System.Diagnostics.Debugger.Break();
        }
        c = new List<T>();
    }
    public CollectionWithoutDuplicatesBaseDC(int count)
    {
        this.count = count;
        c = new List<T>(count);
    }
    public CollectionWithoutDuplicatesBaseDC(IList<T> l)
    {
        c = new List<T>(l.ToList());
    }
    public bool Add(T t2)
    {
        bool result = false;
        var con = Contains(t2);
        if (con.HasValue)
        {
            if (!con.Value)
            {
                c.Add(t2);
                result = true;
            }
        }
        else
        {
            if (!allowNull.HasValue)
            {
                c.Add(t2);
                result = true;
            }
        }
        if (result)
        {
            if (IsComparingByString())
            {
                sr.Add(ts);
            }
        }
        return result;
    }
    protected abstract bool IsComparingByString();
    protected string ts = null;
    public abstract bool? Contains(T t2);
    public abstract int AddWithIndex(T t2);
    public abstract int IndexOf(T path);
    List<T> wasNotAdded = new List<T>();





    public List<T> AddRange(IList<T> list)
    {
        wasNotAdded.Clear();
        foreach (var item in list)
        {
            if (!Add(item))
            {
                wasNotAdded.Add(item);
            }
        }
        return wasNotAdded;
    }
    public string DumpAsString(string operation,  object dumpAsStringHeaderArgs)
    {
        throw new Exception("Nemůže tu být protože DumpListAsStringOneLine jsem přesouval do sunamo a tam už zůstane");
    
    }
}