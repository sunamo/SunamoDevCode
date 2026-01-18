namespace SunamoDevCode.Data;

/// <summary>
/// Represents a single line from dotnet build output containing error or warning information.
/// </summary>
public class DotnetBuildOutputLine
{
    /// <summary>
    /// Gets or sets the file path where the error/warning occurred.
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Gets or sets the line number where the error/warning occurred.
    /// </summary>
    public int Line { get; set; }

    /// <summary>
    /// Gets or sets the column number where the error/warning occurred.
    /// </summary>
    public int Column { get; set; }

    /// <summary>
    /// Gets or sets the type of message (error, warning, etc.).
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the error code (e.g., CS0103, CS0246).
    /// </summary>
    public string ErrorCode { get; set; }

    /// <summary>
    /// Gets or sets the error/warning message text.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Deconstructs the build output line into individual components.
    /// </summary>
    /// <param name="path">File path where the error/warning occurred.</param>
    /// <param name="line">Line number where the error/warning occurred.</param>
    /// <param name="column">Column number where the error/warning occurred.</param>
    /// <param name="type">Type of message (error, warning, etc.).</param>
    /// <param name="errorCode">Error code.</param>
    /// <param name="message">Error/warning message text.</param>
    public void Deconstruct(out string path, out int line, out int column, out string type, out string errorCode, out string message)
    {
        path = Path;
        line = Line;
        column = Column;
        type = Type;
        errorCode = ErrorCode;
        message = Message;
    }
}