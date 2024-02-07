namespace SunamoDevCode._sunamo;
internal class BTS
{
    internal static bool Is(bool binFp, bool n)
    {
        if (n)
        {
            return !binFp;
        }
        return binFp;
    }

    private const string Yes = "Yes";
    private const string No = "No";

    internal static string BoolToString(bool p, bool lower = false)
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
