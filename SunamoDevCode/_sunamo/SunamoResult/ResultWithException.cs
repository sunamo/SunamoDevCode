namespace SunamoDevCode._sunamo.SunamoResult;

internal class ResultWithException<T>
{
    internal T Data { get; set; }
    /// <summary>
    ///     only string, because Message property isn't editable after instatiate
    ///     Usage: FubuCsprojFile
    /// </summary>
    internal string Exc { get; set; }
    internal ResultWithException(T data)
    {
        Data = data;
    }
    internal ResultWithException(string exc)
    {
        this.Exc = exc;
    }
    internal ResultWithException(Exception exc)
    {
        this.Exc = exc.Message;
    }
    /// <summary>
    /// Pro případ že data josu string což je typ i exception
    /// </summary>
    internal ResultWithException()
    {
    }
}