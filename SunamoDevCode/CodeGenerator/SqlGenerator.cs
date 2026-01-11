// variables names: ok
namespace SunamoDevCode.CodeGenerator;

/// <summary>
/// Generator for SQL queries.
/// </summary>
public class SqlGenerator
{
    private StringBuilder stringBuilder = new StringBuilder();

    /// <summary>
    /// Generates a SELECT * query for the specified table.
    /// </summary>
    /// <param name="table">Name of the table to select from.</param>
    public void Select(string table)
    {
        stringBuilder.AppendLine("select * from " + table);
    }

    /// <summary>
    /// Returns the generated SQL as a string.
    /// </summary>
    /// <returns>The generated SQL query.</returns>
    public override string ToString()
    {
        return stringBuilder.ToString();
    }
}