namespace SunamoDevCode._sunamo.SunamoResult;
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
        this.exc = exc.Message;
    }
    /// <summary>
    /// Pro případ že data josu string což je typ i exception
    /// </summary>
    internal ResultWithException()
    {
    }
}