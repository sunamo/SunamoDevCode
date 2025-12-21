namespace SunamoDevCode.Data;

public class DotnetBuildOutputLine
{
    public string Path { get; set; }
    public int Line { get; set; }
    public int Column { get; set; }
    public string Type { get; set; }
    public string ErrorCode { get; set; }
    public string Message { get; set; }

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