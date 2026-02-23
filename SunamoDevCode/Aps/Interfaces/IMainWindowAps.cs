// variables names: ok
namespace SunamoDevCode.Aps.Interfaces;

/// <summary>
/// Interface for the main APS application window.
/// </summary>
public interface IMainWindowAps
{
    /// <summary>
    /// Gets or sets whether the application is running in command-line mode.
    /// </summary>
    bool Cmd { get; set; }
}
