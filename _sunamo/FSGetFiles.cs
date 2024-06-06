using SunamoDevCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal class FSGetFiles
{
    public static List<string> GetFiles(string folder2, string mask, SearchOption searchOption, GetFilesArgs getFilesArgs = null)
    {
        if (getFilesArgs != null)
        {
            ThrowEx.Custom("getFilesArgs is not null");
        }

        return Directory.GetFiles(folder2, mask, searchOption).ToList();
    }
}

