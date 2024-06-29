namespace SunamoDevCode;


internal class FromToList
{
    internal List<FromToDevCode> c = new();
    internal bool IsInRange(int i)
    {
        foreach (var item in c)
            if (i < item.to && i > item.from)
                return true;
        return false;
    }
}