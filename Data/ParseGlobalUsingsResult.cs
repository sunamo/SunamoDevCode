using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ParseGlobalUsingsResult
{
    public List<string> GlobalUsings { get; set; } = new List<string>();
    public Dictionary<string, string> GlobalSymbols { get; set; } = new();
}

