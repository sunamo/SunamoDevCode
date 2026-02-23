// variables names: ok
/// <summary>
/// Interface for the main window providing access to a C# file filter.
/// </summary>
public interface IMainWindowCsFileFilter
{
    /// <summary>
    /// Gets or sets the C# file filter used for filtering source files.
    /// </summary>
    CsFileFilter CsFileFilter { get; set; }

}
