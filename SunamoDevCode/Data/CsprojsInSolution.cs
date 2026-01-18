namespace SunamoDevCode.Data;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public class CsprojsInSolution
{
    public List<string> CsprojFolderPaths { get; set; }
    public List<string> CsprojPaths { get; set; }

    /// <summary>
    /// Converts the csproj folder paths and csproj paths to a dictionary.
    /// </summary>
    /// <returns>Dictionary mapping folder paths to csproj file paths.</returns>
    public Dictionary<string, string> ToDictionary()
    {
        Dictionary<string, string> dictionary = [];

        TestBothHaveSameLength();

        for (int i = 0; i < CsprojFolderPaths.Count; i++)
        {
            dictionary.Add(CsprojFolderPaths[i], CsprojPaths[i]);
        }

        return dictionary;
    }

    /// <summary>
    /// Tests that both lists have the same length.
    /// </summary>
    private void TestBothHaveSameLength()
    {
        if (CsprojPaths.Count != CsprojFolderPaths.Count)
        {
            ThrowEx.Custom("Different count in collection");
        }
    }

    /// <summary>
    /// Intersects the current instance with another CsprojsInSolution instance.
    /// </summary>
    /// <param name="other">Other CsprojsInSolution instance to merge with.</param>
    /// <returns>The current instance with merged data.</returns>
    public CsprojsInSolution Intersect(CsprojsInSolution other)
    {
        CsprojFolderPaths.AddRange(other.CsprojFolderPaths);
        CsprojPaths.AddRange(other.CsprojPaths);

        var distinctFolderPaths = CsprojFolderPaths.Distinct();
        if (distinctFolderPaths.Count() != CsprojFolderPaths.Count)
        {
            ThrowEx.Custom("Not all CsprojNames is unique");
        }

        return this;
    }
}