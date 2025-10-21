// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode._public;

public class TWithStringDC<T>
{
    public string path;
    public T t;
    public TWithStringDC()
    {
    }
    public TWithStringDC(T t, string path)
    {
        this.t = t;
        this.path = path;
    }
    public override string ToString()
    {
        return path;
    }
}