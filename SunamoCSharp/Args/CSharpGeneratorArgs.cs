namespace SunamoDevCode.SunamoCSharp.Args;

public class CSharpGeneratorArgs
{
    /// <summary>
    /// Is null because in method wh
    /// </summary>
    public bool addHyphens = false;
    public bool createInstance = false;
    public bool alsoField = false;
    public bool addingValue = false;
    public bool useCA = true;
    /// <summary>
    /// must be char or string!
    /// nahradil jsem ho za object ale string bude asi lepší
    /// </summary>
    public string splitKeyWith = null;
    public bool checkForNull = false;
}

