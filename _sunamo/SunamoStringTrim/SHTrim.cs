namespace SunamoDevCode._sunamo.SunamoStringTrim;

internal class SHTrim
{
    public static string Trim(string s, string args)
    {
        s = TrimStart(s, args);
        s = TrimEnd(s, args);

        return s;
    }
    internal static string TrimLeadingNumbersAtStart(string nameSolution)
    {
        for (int i = 0; i < nameSolution.Length; i++)
        {
            bool replace = false;
            for (int n = 0; n < 10; n++)
            {
                if (nameSolution[i] == n.ToString()[0])
                {
                    replace = true;
                    nameSolution = nameSolution.Substring(1);
                    break;
                }
            }
            if (!replace)
            {
                return nameSolution;
            }
        }
        return nameSolution;
    }

    internal static string TrimEnd(string name, string ext)
    {
        while (name.EndsWith(ext)) return name.Substring(0, name.Length - ext.Length);

        return name;
    }

    internal static string TrimStartAndEnd(string target, Func<char, bool> startAllowed, Func<char, bool> endAllowed)
    {
        for (int i = 0; i < target.Length; i++)
        {
            if (!startAllowed.Invoke(target[i]))
            {
                target = target.Substring(1);
                i--;
            }
            else
            {
                break;
            }
        }

        for (int i = target.Length - 1; i >= 0; i--)
        {
            if (!endAllowed.Invoke(target[i]))
            {
                target = target.Remove(target.Length - 1, 1);

            }
            else
            {
                break;
            }
        }
        return target;
    }

    internal static string TrimStart(string v, string s)
    {
        while (v.StartsWith(s))
        {
            v = v.Substring(s.Length);
        }

        return v;
    }

}
