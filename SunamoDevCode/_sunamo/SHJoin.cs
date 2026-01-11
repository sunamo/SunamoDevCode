// variables names: ok
namespace SunamoDevCode._sunamo;

/// <summary>
/// String helper for joining collections
/// </summary>
internal class SHJoin
{
    /// <summary>
    /// Joins list of items with newline separator
    /// </summary>
    /// <typeparam name="T">Type of list elements</typeparam>
    /// <param name="list">List to join</param>
    /// <returns>String with items joined by newlines</returns>
    internal static string JoinNL<T>(List<T> list)
    {
        var strings = list.ConvertAll(item => item.ToString());
        return string.Join("\n", strings);
    }

    /// <summary>
    /// Joins list of strings with newline separator
    /// </summary>
    /// <param name="list">List of strings to join</param>
    /// <returns>String with items joined by newlines</returns>
    internal static string JoinNL(List<string> list)
    {
        return string.Join('\n', list);
    }
}