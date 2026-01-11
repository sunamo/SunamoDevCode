namespace SunamoDevCode._sunamo.SunamoConverters.Converts;

/// <summary>
/// Converts between type full names and C# shortcuts
/// </summary>
internal static class ConvertTypeShortcutFullName
{
    private const string SystemDot = "System.";

    /// <summary>
    /// Converts full type name to C# shortcut (throws exception if not basic type)
    /// </summary>
    /// <param name="fullName">Full type name (e.g. "System.String")</param>
    /// <returns>C# shortcut (e.g. "string")</returns>
    internal static string ToShortcut(string fullName)
    {
        return ToShortcut(fullName, true);
    }

    /// <summary>
    /// Converts full type name to C# shortcut
    /// </summary>
    /// <param name="fullName">Full type name (e.g. "System.String")</param>
    /// <param name="throwExceptionWhenNotBasicType">If true, throws exception for non-basic types</param>
    /// <returns>C# shortcut or original full name if not a basic type</returns>
    internal static string ToShortcut(string fullName, bool throwExceptionWhenNotBasicType)
    {
        if (!fullName.StartsWith(SystemDot))
        {
            fullName = SystemDot + fullName;
        }
        switch (fullName)
        {
            #region Basic Type Mappings
            case "System.String":
                return "string";
            case "System.Int32":
                return "int";
            case "System.Boolean":
                return "bool";
            case "System.Single":
                return "float";
            case "System.DateTime":
                return "DateTime";
            case "System.Double":
                return "double";
            case "System.Decimal":
                return "decimal";
            case "System.Char":
                return "char";
            case "System.Byte":
                return "byte";
            case "System.SByte":
                return "sbyte";
            case "System.Int16":
                return "short";
            case "System.Int64":
                return "long";
            case "System.UInt16":
                return "ushort";
            case "System.UInt32":
                return "uint";
            case "System.UInt64":
                return "ulong";
                #endregion
        }
        if (throwExceptionWhenNotBasicType)
        {
            throw new Exception("Unsupported type");
        }
        return fullName;
    }
}