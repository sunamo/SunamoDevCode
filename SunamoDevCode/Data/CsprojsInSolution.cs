// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.Data;
public class CsprojsInSolution
{
    public List<string> CsprojFolderPaths { get; set; }
    public List<string> CsprojPaths { get; set; }

    public Dictionary<string, string> ToDictionary()
    {
        Dictionary<string, string> d = [];

        TestBothHaveSameLength();

        for (int i = 0; i < CsprojFolderPaths.Count; i++)
        {
            d.Add(CsprojFolderPaths[i], CsprojPaths[i]);
        }

        return d;
    }

    private void TestBothHaveSameLength()
    {
        if (CsprojPaths.Count != CsprojFolderPaths.Count)
        {
            ThrowEx.Custom("Different count in collection");
        }
    }

    public CsprojsInSolution Intersect(CsprojsInSolution sunamo)
    {
        CsprojFolderPaths.AddRange(sunamo.CsprojFolderPaths);
        CsprojPaths.AddRange(sunamo.CsprojPaths);

        var dis = CsprojFolderPaths.Distinct();
        if (dis.Count() != CsprojFolderPaths.Count)
        {
            ThrowEx.Custom("Not all CsprojNames is unique");
        }

        return this;
    }
}
