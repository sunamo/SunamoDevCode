// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.Aps.Enums;

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
    CL,
    FS,
    MailSender,
    PathSelector,

    RoslynHelper,

    SqlServerHelper,

    SunamoStrings,
    TextBoxBackend,

    TextBoxHelper,
    XamlDisplay,
  
    HtmlGenerator2,
 
    PowershellBuilder,
    JunctionPoint,
    ColorPicker,
    UH,
    SHShared,
    HtmlHelperShared,
    MSStoredProceduresIBaseShared,
    FSShared,
    SH,
    ChartsCs,
    AppsHtmlGenerator,

    /// <summary>
    /// multiline string without lateral 
    /// </summary>
    _CodeFragment,
    _Consts,
    /// <summary>
    /// 
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
    /// 
    /// </summary>
    _LeadingTrailing,

    // Last item to allow all have comma on end(
    End
}