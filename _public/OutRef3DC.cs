namespace SunamoDevCode._public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OutRef3DC<T, U, V> : OutRefDC<T, U>
{
    public OutRef3DC(T t, U u, V v) : base(t, u)
    {
        Item3 = v;
    }
    public V Item3 { get; set; }
}