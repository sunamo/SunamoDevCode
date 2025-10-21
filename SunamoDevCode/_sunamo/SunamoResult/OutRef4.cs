// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode._sunamo.SunamoResult;

internal class OutRef4<T, U, V, W> : OutRef3<T, U, V>
{
    internal OutRef4(T t, U u, V v, W w) : base(t, u, v)
    {
        Item4 = w;
    }

    internal W Item4 { get; set; }
}