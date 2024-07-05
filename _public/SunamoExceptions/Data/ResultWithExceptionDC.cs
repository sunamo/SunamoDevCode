namespace SunamoDevCode._public.SunamoExceptions.Data;


public class ResultWithExceptionDC<T>
{
    public T Data { get; set; }
    
    
    
    
    public string exc { get; set; }
    public ResultWithExceptionDC(T data)
    {
        Data = data;
    }
    public ResultWithExceptionDC(string exc)
    {
        this.exc = exc;
    }
    public ResultWithExceptionDC(Exception exc)
    {
        this.exc = Exceptions.TextOfExceptions(exc);
    }
    
    
    
    public ResultWithExceptionDC()
    {
    }
}