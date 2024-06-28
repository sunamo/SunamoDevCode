namespace SunamoDevCode;


internal class ResultWithException<T>
{
    internal T Data { get; set; }
    /// <summary>
    ///     only string, because Message property isn't editable after instatiate
    ///     Usage: FubuCsprojFile
    /// </summary>
    internal string exc { get; set; }
    internal ResultWithException(T data)
    {
        Data = data;
    }
    internal ResultWithException(string exc)
    {
        this.exc = exc;
    }
    internal ResultWithException(Exception exc)
    {
        this.exc = Exceptions.TextOfExceptions(exc);
    }
    /// <summary>
    /// Pro případ že data josu string což je typ i exception
    /// </summary>
    internal ResultWithException()
    {
    }
}