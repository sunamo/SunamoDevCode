namespace SunamoDevCode._public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OutRefDC<T, U>
{
    public OutRefDC(T t, U u)
    {
        Item1 = t;
        Item2 = u;
    }
    public T Item1 { get; set; }
    public U Item2 { get; set; }
}