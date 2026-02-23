namespace SunamoDevCode;

/// <summary>
/// Constants and keyword lists for Vue.js source code parsing and generation.
/// </summary>
public class VueConsts
{
    /// <summary>
    /// Class keyword with trailing space.
    /// </summary>
    public const string Cl = "class ";
    /// <summary>
    /// Constructor keyword.
    /// </summary>
    public const string Constructor = "constructor";
    /// <summary>
    /// Import keyword.
    /// </summary>
    public const string Import = "import";
    /// <summary>
    /// Export keyword.
    /// </summary>
    public const string Export = "export";
    /// <summary>
    /// Methods section opening with curly brace.
    /// </summary>
    public const string MethodsRcub = "methods:{";
    /// <summary>
    /// Props section opening with curly brace.
    /// </summary>
    public const string PropsRcub = "props:{";

    // name, inject, mixins, extends, watch
    // https://github.com/pablohpsilva/vuejs-component-style-guide
    /// <summary>
    /// Vue component options that should be matched by StartsWith.
    /// </summary>
    public static List<string> SStartWith = new List<string>(["name:", "inject:", "mixins:", "extends:", "watch:"]);

    /// <summary>
    /// Sections where right curly brace should not be added automatically.
    /// </summary>
    public static List<string> DontAddRcub = new List<string>([MethodsRcub, PropsRcub]);

    //List<string> sContains = new List<string>("data():", "methods:{");
    /// <summary>
    /// Keywords to match using Contains check.
    /// </summary>
    public static List<string> ContainsList = new List<string>([Cl]);

    /// <summary>
    /// Keywords to match using StartsWith check.
    /// </summary>
    public static List<string> StartWithList = new List<string>([Constructor, Import, "//", Export]);
    /// <summary>
    /// Characters treated as assignment operators.
    /// </summary>
    public static List<char> Equal = new List<char>(['=']);
}