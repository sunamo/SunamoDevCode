// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy

namespace SunamoDevCode.CodeGenerator;

public class SqlGenerator
{
    StringBuilder stringBuilder = new StringBuilder();

    public void Select(string table)
    {
        stringBuilder.AppendLine("select * from " + table);
    }

    public override string ToString()
    {
        return stringBuilder.ToString();
    }
}
