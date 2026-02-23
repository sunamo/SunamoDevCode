namespace SunamoDevCode.Aps.Interfaces;

/// <summary>
/// Interface for searching across Visual Studio solutions.
/// </summary>
public interface ISearchInSolutions
{
    /// <summary>
    /// Adds the specified solution folder to the latest search results.
    /// </summary>
    /// <param name="solutionFolderSerialize">Solution folder to add.</param>
    void AddToLatest(SolutionFolderSerialize solutionFolderSerialize);
}