// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode._sunamo.SunamoResult;

internal class OutRef<T, U>(T t, U u)
{
    internal T Item1 { get; set; } = t;
    internal U Item2 { get; set; } = u;
}