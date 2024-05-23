namespace SunamoDevCode;


public class ResultWithException<T>
{
    public T Data { get; set; }
    /// <summary>
    ///     only string, because Message property isn't editable after instatiate
    ///     Usage: FubuCsprojFile
    /// </summary>
    public string exc { get; set; }
    public ResultWithException(T data)
    {
        Data = data;
    }
    public ResultWithException(string exc)
    {
        this.exc = exc;
    }
    public ResultWithException(Exception exc)
    {
        this.exc = Exceptions.TextOfExceptions(exc);
    }
    /// <summary>
    /// Pro případ že data josu string což je typ i exception
    /// </summary>
    public ResultWithException()
    {
    }
}