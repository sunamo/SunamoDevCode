// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
﻿namespace SunamoDevCode._public;


public class ResultWithExceptionDC<T>
{
    public T Data { get; set; }




    public string Exc { get; set; }
    public ResultWithExceptionDC(T data)
    {
        Data = data;
    }
    public ResultWithExceptionDC(string exc)
    {
        this.Exc = exc;
    }
    public ResultWithExceptionDC(Exception exc)
    {
        this.Exc = Exceptions.TextOfExceptions(exc);
    }



    public ResultWithExceptionDC()
    {
    }
}