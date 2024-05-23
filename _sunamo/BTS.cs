namespace SunamoDevCode;
public class BTS
{
    public static bool Is(bool binFp, bool n)
    {
        if (n)
        {
            return !binFp;
        }
        return binFp;
    }

    private const string Yes = "Yes";
    private const string No = "No";

    public static string BoolToString(bool p, bool lower = false)
    {
        string vr = null;
        if (p)
            vr = Yes;
        else
        {
            vr = No;
        }

        return vr.ToLower();
    }
}
