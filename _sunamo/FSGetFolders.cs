namespace SunamoDevCode._sunamo;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class FSGetFolders
{
    internal static IEnumerable<string> GetFolders(string v)
    {
        return Directory.GetFiles(v);
    }
}