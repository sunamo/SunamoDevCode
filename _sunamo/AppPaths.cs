namespace SunamoDevCode._sunamo;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class AppPaths
{
    public static string GetStartupPath()
    {
        return Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
    }

    public static string GetFileInStartupPath(string file)
    {
        return Path.Combine(GetStartupPath(), file);
    }
}