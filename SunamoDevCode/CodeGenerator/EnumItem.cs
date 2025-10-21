// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.CodeGenerator;

public class EnumItem
{
    //protected string css = "";
    /// <summary>
    /// Zadává se bez počátečního 0x
    /// </summary>
    public string Hex = "";
    public Dictionary<string, string> Attributes = null;
    public string Name = "";
    public string Comment = string.Empty;

    ///// <summary>
    ///// Zadává se bez počátečního 0x
    ///// </summary>
    //public string Hex
    //{
    //    get
    //    {
    //        return hex;
    //    }
    //}
    //public Dictionary<string, string> Attributes
    //{
    //    get
    //    {
    //        return attributes;
    //    }
    //}
    //public string Name
    //{
    //    get
    //    {
    //        return name;
    //    }
    //}
}
