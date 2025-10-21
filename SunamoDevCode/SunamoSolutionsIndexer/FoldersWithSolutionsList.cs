// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.SunamoSolutionsIndexer;

public class FoldersWithSolutionsList : List<FoldersWithSolutions>
{
    public new void Add(FoldersWithSolutions a)
    {
        base.Add(a);
    }
}
