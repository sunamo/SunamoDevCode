namespace SunamoDevCode._sunamo.SunamoBts;

internal class BTS
{
    public static string Replace(ref string id, bool replaceCommaForDot)
    {
        if (replaceCommaForDot) id = id.Replace(",", ".");

        return id;
    }
    public static int lastInt = -1;
    public static long lastLong = -1;
    public static float lastFloat = -1;
    public static double lastDouble = -1;
    public static bool IsFloat(string id, bool replace = false)
    {
        if (id == null) return false;

        Replace(ref id, replace);
        return float.TryParse(id.Replace(",", "."), out lastFloat);
    }

    public static bool IsInt(string id, bool excIfIsFloat = false, bool replaceCommaForDot = false)
    {
        if (id == null) return false;

        id = id.Replace(" ", "");
        Replace(ref id, replaceCommaForDot);


        var vr = int.TryParse(id, out lastInt);
        if (!vr)
            if (IsFloat(id))
                if (excIfIsFloat)
                    throw new Exception(id + " is float but is calling IsInt");

        return vr;
    }
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

    internal static bool GetValueOfNullable(bool? b)
    {
        return b.GetValueOrDefault();
    }
}