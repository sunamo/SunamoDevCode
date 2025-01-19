namespace SunamoDevCode;

// m�m u� EmbeddedResourcesH v SunamoEmbeddedResources
//class EmbeddedResourcesH
//{
//    private Assembly assembly;
//    private string v;

//    public EmbeddedResourcesH(Assembly assembly, string v)
//    {
//        this.assembly = assembly;
//        this.v = v;
//    }

//    public static string GetString(string s)
//    {
//        return "";
//    }
//}

public static class SystemWindowsControls
{
    private static readonly Type type = typeof(SystemWindowsControls);
    private static bool s_initialized;
    private static readonly Dictionary<string, List<string>> s_controls = new();
    private static EmbeddedResourcesH s_embeddedResourcesH;

    private static Dictionary<string, string> controlsShortLong;

    public static void InitControlsShortLong()
    {
        Init();

        if (controlsShortLong == null)
        {
            controlsShortLong = new Dictionary<string, string>();

            foreach (var item in s_controls)
            foreach (var item2 in item.Value)
                controlsShortLong.Add(item2, item.Key);
        }
    }

    public static void Init()
    {
        if (!s_initialized)
        {
            s_initialized = true;

            s_embeddedResourcesH = new EmbeddedResourcesH(type.Assembly, "SunamoDevCode");

            //var d = SHGetLines.GetLines(s_embeddedResourcesH.GetString("/Resources/SystemWindowsControls.txt"));
            //foreach (var item in d)
            //{
            //    var p = SHSplit.SplitMore(item, " ");
            //    s_controls.Add(p[0], SHSplit.SplitMore(p[1], ","));
            //}
        }
    }

    public static bool StartingWithShortcutOfControl(string r)
    {
        foreach (var item in s_controls)
        foreach (var item2 in item.Value)
            if (item2.Length > 2)
                if (r.StartsWith(item2))
                    return true;

        return false;
    }

    public static bool IsShortcutOfControl(string r)
    {
        foreach (var item in s_controls)
        foreach (var item2 in item.Value)
            if (item2 == r)
                return true;

        return false;
    }

    public static bool IsNameOfControl(string r)
    {
        return s_controls.ContainsKey(r);
    }
}