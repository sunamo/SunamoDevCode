namespace SunamoDevCode;

public class MarvinPathsHelper
{
    public const string basePath = @"C:\repos\EOM-7\Marvin\";
    public const string ClientS = "Client\\";
    public const string SrcS = "src\\";

    public const string module = "Module.";



    static string ClientSrcWorker(string r, bool addS)
    {
        var p = Module(r) + ClientS + (addS ? "s" : "") + SrcS;
        p = SHReplaceOnce.ReplaceOnce(p, @"\s", @"s\");
        if (!Directory.Exists(p))
        {
            ThrowEx.DirectoryExists(p);
        }
        return p;
    }

    static string ClientWorker(string r, bool addS, bool throwExIfNotExists = true)
    {
        var p = Module(r) + ClientS + (addS ? "s" : "");
        p = SHReplaceOnce.ReplaceOnce(p, @"\s", @"s\");
        if (throwExIfNotExists && !Directory.Exists(p))
        {
            ThrowEx.DirectoryExists(p);
        }
        return p;
    }

    /// <summary>
    /// A1 = MarvinReposName.*
    /// </summary>
    /// <param name="r"></param>
    /// <returns></returns>
    public static string ClientSrc(string r)
    {
        return ClientSrcWorker(r, false);
    }

    /// <summary>
    /// A1 = MarvinReposName.*
    /// </summary>
    public static string Client(string r, bool throwExIfNotExists = true)
    {
        return ClientWorker(r, false, throwExIfNotExists);
    }

    /// <summary>
    /// A1 = MarvinReposName.*
    /// </summary>
    public static string ClientsSrc(string r)
    {
        return ClientSrcWorker(r, true);
    }

    /// <summary>
    /// A1 = MarvinReposName.*
    /// </summary>
    public static string Clients(string r, bool throwExIfNotExists = true)
    {
        return ClientWorker(r, true, false);
    }

    /// <summary>
    /// if A1 will be without Module., I will add it
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static string Module(string item)
    {
        item = SH.PrefixIfNotStartedWith(item, module);
        return basePath + item + "\\";
    }
}
