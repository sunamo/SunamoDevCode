// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode._sunamo.SunamoConverters.Converts;

internal static class ConvertTypeShortcutFullName //: IConvertShortcutFullName
{
    const string systemDot = "System.";
    internal static string ToShortcut(string fullName)
    {
        return ToShortcut(fullName, true);
    }
    /// <param name="fullName"></param>
    internal static string ToShortcut(string fullName, bool throwExceptionWhenNotBasicType)
    {
        if (!fullName.StartsWith(systemDot))
        {
            fullName = systemDot + fullName;
        }
        switch (fullName)
        {
            #region MyRegion
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
            throw new Exception("Nepodporovan\u00FD typ");
            return null;
        }
        return fullName;
    }
    static Type type = typeof(ConvertTypeShortcutFullName);
}