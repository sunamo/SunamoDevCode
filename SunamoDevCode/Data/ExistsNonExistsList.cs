// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.Data;

public class ExistsNonExistsList<T>
{
    public List<T> Exists = new List<T>();
    public List<T> NonExists = new List<T>();
}
