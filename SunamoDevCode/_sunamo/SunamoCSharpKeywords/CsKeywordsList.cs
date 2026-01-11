namespace SunamoDevCode._sunamo.SunamoCSharpKeywords;

/// <summary>
/// C# keywords categorized by type
/// </summary>
internal static class CsKeywordsList
{
    internal static readonly HashSet<string> Modifier = new HashSet<string>
    {
        "abstract", "async", "const", "event", "extern", "new", "override", "partial", "readonly", "sealed", "static", "unsafe", "virtual", "volatile"
    };

    internal static readonly HashSet<string> AccessModifier = new HashSet<string>
    {
        "public", "private", "protected", "internal", "protected internal", "private protected"
    };

    internal static readonly HashSet<string> Statement = new HashSet<string>
    {
        "if", "else", "switch", "case", "default", "do", "for", "foreach", "in", "while", "break", "continue", "goto", "return", "throw", "try", "catch", "finally", "checked", "unchecked", "fixed", "lock"
    };

    internal static readonly HashSet<string> MethodParameter = new HashSet<string>
    {
        "params", "ref", "out", "in"
    };

    internal static readonly HashSet<string> NamespaceKeywords = new HashSet<string>
    {
        "namespace", "using"
    };

    internal static readonly HashSet<string> OperatorKeywords = new HashSet<string>
    {
        "operator", "explicit", "implicit"
    };

    internal static readonly HashSet<string> Access = new HashSet<string>
    {
        "base", "this"
    };

    internal static readonly HashSet<string> Literal = new HashSet<string>
    {
        "null", "true", "false"
    };

    internal static readonly HashSet<string> TypeKeywords = new HashSet<string>
    {
        "bool", "byte", "char", "class", "decimal", "double", "enum", "float", "int", "interface", "long", "object", "sbyte", "short", "string", "struct", "uint", "ulong", "ushort", "void", "record"
    };

    internal static readonly HashSet<string> Contextual = new HashSet<string>
    {
        "add", "alias", "ascending", "async", "await", "by", "descending", "dynamic", "equals", "file", "from", "get", "global", "group", "init", "into", "join", "let", "managed", "nameof", "not", "notnull", "on", "orderby", "partial", "record", "remove", "required", "select", "set", "unmanaged", "value", "var", "when", "where", "with", "yield"
    };

    internal static readonly HashSet<string> Query = new HashSet<string>
    {
        "from", "where", "select", "group", "into", "orderby", "join", "let", "in", "on", "equals", "by", "ascending", "descending"
    };
}
