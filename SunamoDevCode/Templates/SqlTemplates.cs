namespace SunamoDevCode.Templates;

/// <summary>
/// SQL query templates.
/// </summary>
public class SqlTemplates
{
    /// <summary>
    /// SELECT query template with table name placeholder.
    /// </summary>
    public const string Select = "select * from {0}";

    /// <summary>
    /// USE database command for sunamo.cz database.
    /// </summary>
    public const string UseSunamoCz = "use [sunamo.cz]";
}