using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunamoDevCode;
public class FSGetFilesDC
{
    public static List<string> GetFilesDC(string slnFolder, string masc, SearchOption so, GetFilesDCArgs a)
    {
        List<string> result = new List<string>();
        var projects = Directory.GetDirectories(slnFolder);
        foreach (var item in projects)
        {
            if (a.OnlyIn_Sunamo)
            {
                var _sunamo = Path.Combine(item, "_sunamo");
                if (Directory.Exists(_sunamo))
                {
                    result.AddRange(Directory.GetFiles(_sunamo, masc, SearchOption.AllDirectories));
                }
            }
            else
            {
                result.AddRange(Directory.GetFiles(item, masc, SearchOption.AllDirectories));
            }
        }

        return result;
    }
}
