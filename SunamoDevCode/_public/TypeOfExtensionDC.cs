namespace SunamoDevCode._public;

/// <summary>
/// Defines categories of file extensions based on their content type and purpose.
/// </summary>
public enum TypeOfExtensionDC
{
    /// <summary>
    /// Archive file type (zip, rar, etc.).
    /// </summary>
    archive,
    /// <summary>
    /// Image file type (png, jpg, etc.).
    /// </summary>
    image,
    /// <summary>
    /// Source code file type (cs, js, etc.).
    /// </summary>
    source_code,
    /// <summary>
    /// Text document file type (doc, txt, etc.).
    /// </summary>
    documentText,
    /// <summary>
    /// Binary document file type (pdf, docx, etc.).
    /// </summary>
    documentBinary,
    /// <summary>
    /// Database file type.
    /// </summary>
    database,

    /// <summary>
    ///     prošel jsem zda v AllExtension jsou všechny textové
    /// </summary>
    configText,

    /// <summary>
    ///     XML, JSON, mdf, ldf, sdf, atd.
    ///     Can't name data because is difficult search (exists also database)
    /// </summary>
    contentText,
    /// <summary>
    /// Binary content file type.
    /// </summary>
    contentBinary,

    /// <summary>
    ///     prošel jsem zda v AllExtension jsou všechny textové
    ///     ini, atd.
    /// </summary>
    settingsText,

    /// <summary>
    ///     prošel jsem zda v AllExtension jsou všechny textové
    /// </summary>
    visual_studioText,
    /// <summary>
    /// Executable file type (exe, dll, etc.).
    /// </summary>
    executable,
    /// <summary>
    /// Binary file type.
    /// </summary>
    binary,

    /// <summary>
    ///     u resourců by to asi tak nevadilo kdyby byly zakódovany třeba ve b64 ale pro jistotu je všechny řadím do binárních
    ///     ať je nepoškodím
    /// </summary>
    resource,

    /// <summary>
    ///     prošel jsem zda v AllExtension jsou všechny textové
    ///     sql, cmd, ps1,
    /// </summary>
    script,
    /// <summary>
    /// Font file type (ttf, otf, etc.).
    /// </summary>
    font,
    /// <summary>
    /// Multimedia file type (mp3, mp4, etc.).
    /// </summary>
    multimedia,
    /// <summary>
    /// Temporary file type.
    /// </summary>
    temporary,

    /// <summary>
    ///     Is used when extension isn't know
    ///     U ostatních souborů vypsat jejich popis z windows
    /// </summary>
    other
}