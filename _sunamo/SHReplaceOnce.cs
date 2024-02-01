using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunamoDevCode._sunamo
{
    internal class SHReplaceOnce
    {
        internal static string ReplaceOnce(string a, string v, string empty)
        {
            return a.Replace(v, empty);
        }
    }
}
