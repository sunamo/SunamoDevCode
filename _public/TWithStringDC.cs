using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunamoDevCode._public;

public class TWithStringDC<T>
{
    public string path;

    public T t;

    public TWithStringDC()
    {
    }

    public TWithStringDC(T t, string path)
    {
        this.t = t;
        this.path = path;
    }

    public override string ToString()
    {
        return path;
    }
}