namespace SunamoDevCode;


/// <summary>
///     Přes tyto třídy je jediná možnost jak se vypořádat s out/ref v async metodách
///     Ukládat do to statické property je nesmysl protože k tomu můžou v jeden čas přistupovat úplně všichni
///     Třída s 1 parametrem je nesmysl protože vždy musím vrátit i původní result
/// </summary>
/// <typeparam name="T"></typeparam>
public class OutRef<T, U>
{
    public OutRef(T t, U u)
    {
        Item1 = t;
        Item2 = u;
    }
    public T Item1 { get; set; }
    public U Item2 { get; set; }
}