// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode._sunamo.SunamoBts;

internal class BTS
{
    internal static string Replace(ref string id, bool replaceCommaForDot)
    {
        if (replaceCommaForDot) id = id.Replace(",", ".");

        return id;
    }
    internal static int lastInt = -1;
    internal static long lastLong = -1;
    internal static float lastFloat = -1;
    internal static double lastDouble = -1;
    internal static bool IsFloat(string id, bool replace = false)
    {
        if (id == null) return false;

        Replace(ref id, replace);
        return float.TryParse(id.Replace(",", "."), out lastFloat);
    }

    internal static bool IsInt(string id, bool excIfIsFloat = false, bool replaceCommaForDot = false)
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

        if (lower)
        {
            return vr.ToLower();
        }
        return vr;
    }

    internal static bool GetValueOfNullable(bool? b)
    {
        return b.GetValueOrDefault();
    }
}