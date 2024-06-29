namespace SunamoDevCode;


public class ResultWithExceptionDC<T>
{
    internal T Data { get; set; }
    /// <summary>
    ///     only string, because Message property isn't editable after instatiate
    ///     Usage: FubuCsprojFile
    /// </summary>
    internal string exc { get; set; }
    internal ResultWithExceptionDC(T data)
    {
        Data = data;
    }
    internal ResultWithExceptionDC(string exc)
    {
        this.exc = exc;
    }
    internal ResultWithExceptionDC(Exception exc)
    {
        this.exc = Exceptions.TextOfExceptions(exc);
    }
    /// <summary>
    /// Pro případ že data josu string což je typ i exception
    /// </summary>
    internal ResultWithExceptionDC()
    {
    }
}