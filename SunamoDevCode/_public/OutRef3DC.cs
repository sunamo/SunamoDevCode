// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode._public;

public class OutRef3DC<T, U, V> : OutRefDC<T, U>
{
    public OutRef3DC(T t, U u, V v) : base(t, u)
    {
        Item3 = v;
    }
    public V Item3 { get; set; }
}