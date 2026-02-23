namespace SunamoDevCode.Aps.Enums;

/// <summary>
/// Defines C# files that need translation to English. Each member represents a specific source file.
/// </summary>
public enum CsToEnFiles
{
    // Files commented

    //AllStrings,
    //AssemblyInfo,
    //RegexHelper,
    //XamlGenerator,
    //ConstsShared,
    //CSharpGenerator,
    //GeneratorCpp,
    //MSColumnsDB,
    //JavaScriptInjection,

        // Files
    /// <summary>
    /// Command line helper file.
    /// </summary>
    CL,
    /// <summary>
    /// File system operations file.
    /// </summary>
    FS,
    /// <summary>
    /// Mail sender utility file.
    /// </summary>
    MailSender,
    /// <summary>
    /// Path selector utility file.
    /// </summary>
    PathSelector,

    /// <summary>
    /// Roslyn code analysis helper file.
    /// </summary>
    RoslynHelper,

    /// <summary>
    /// SQL Server helper file.
    /// </summary>
    SqlServerHelper,

    /// <summary>
    /// Sunamo string constants file.
    /// </summary>
    SunamoStrings,
    /// <summary>
    /// Text box backend file.
    /// </summary>
    TextBoxBackend,

    /// <summary>
    /// Text box helper file.
    /// </summary>
    TextBoxHelper,
    /// <summary>
    /// XAML display helper file.
    /// </summary>
    XamlDisplay,

    /// <summary>
    /// HTML generator version 2 file.
    /// </summary>
    HtmlGenerator2,

    /// <summary>
    /// PowerShell command builder file.
    /// </summary>
    PowershellBuilder,
    /// <summary>
    /// Junction point filesystem operations file.
    /// </summary>
    JunctionPoint,
    /// <summary>
    /// Color picker utility file.
    /// </summary>
    ColorPicker,
    /// <summary>
    /// URI helper file.
    /// </summary>
    UH,
    /// <summary>
    /// String helper shared file.
    /// </summary>
    SHShared,
    /// <summary>
    /// HTML helper shared file.
    /// </summary>
    HtmlHelperShared,
    /// <summary>
    /// MS stored procedures base shared file.
    /// </summary>
    MSStoredProceduresIBaseShared,
    /// <summary>
    /// File system shared operations file.
    /// </summary>
    FSShared,
    /// <summary>
    /// String helper file.
    /// </summary>
    SH,
    /// <summary>
    /// Charts C# generation file.
    /// </summary>
    ChartsCs,
    /// <summary>
    /// Apps HTML generator file.
    /// </summary>
    AppsHtmlGenerator,

    /// <summary>
    /// multiline string without lateral
    /// </summary>
    _CodeFragment,
    /// <summary>
    /// Constants pattern file.
    /// </summary>
    _Consts,
    /// <summary>
    /// Number on sides pattern file.
    /// </summary>
    _NumberOnSides,
    /// <summary>
    /// var b = "{0}"; return $"c {b}";
    /// </summary>
    _StringInterpolation,
    /// <summary>
    /// dont know purpose
    /// </summary>
    _EncodedQuote,
    /// <summary>
    /// return sess.i18n(XlfKeys.HelloWorld);
    /// </summary>
    _AlreadyTranslated,
    /// <summary>
    /// Leading and trailing pattern file.
    /// </summary>
    _LeadingTrailing,

    // Last item to allow all have comma on end(
    /// <summary>
    /// Sentinel value marking the end of the enum.
    /// </summary>
    End
}