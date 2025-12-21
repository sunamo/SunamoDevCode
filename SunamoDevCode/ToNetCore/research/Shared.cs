namespace SunamoDevCode.ToNetCore.research;

public class Shared
{
    public static Type type = typeof(Shared);

    public static Action<string, bool> ExtractArchive;

    static
#if ASYNC
    async Task<string>
#else
    string
#endif
 ReplaceTargetPlatform(string replaceFor, string PropertyGroup, string start, string end, List<string> tt, bool throwEx = false)
    {
        StringBuilder onlyStart = null;
        StringBuilder onlyEnd = null;
        StringBuilder dontHavePropertyGroup = null;

        if (!throwEx)
        {
            onlyStart = new StringBuilder();
            onlyEnd = new StringBuilder();
            dontHavePropertyGroup = new StringBuilder();
        }

        foreach (var item in tt)
        {
            var f =
#if ASYNC
    await
#endif
 TF.ReadAllText(item);
            var b1 = f.Contains(start);
            var b2 = f.Contains(end);

            if (b1 && b2)
            {
                var builder = SH.GetTextBetween(f, start, end, false);

                if (builder != replaceFor)
                {
                    f = SHReplace.ReplaceOnce(f, start + builder + end, start + replaceFor + end);
                    await TF.WriteAllText(item, f);
                }
            }
            else if (b1 && !b2)
            {
                if (throwEx)
                {
                    ThrowEx.Custom("Have only starting tag: " + item);
                }
                else
                {
                    onlyStart.AppendLine(item);
                }
            }
            else if (b2 && !b1)
            {
                if (throwEx)
                {
                    ThrowEx.Custom("Have only ending tag: " + item);
                }
                else
                {
                    onlyEnd.AppendLine(item);
                }
            }
            else
            {
                var dx = f.IndexOf(PropertyGroup);
                if (dx == -1)
                {
                    if (throwEx)
                    {
                        ThrowEx.Custom("Don't have PropertyGroup: " + item);
                    }
                    else
                    {
                        dontHavePropertyGroup.AppendLine(item);
                    }
                }
                else
                {
                    f = f.Insert(dx + PropertyGroup.Length, start + replaceFor + end);
                    await TF.WriteAllText(item, f);
                }
            }
        }

        if (!throwEx)
        {
            TextOutputGenerator tog = new TextOutputGenerator();
            tog.ListSB(onlyStart, "onlyStart");
            tog.ListSB(onlyEnd, "onlyEnd");
            tog.ListSB(dontHavePropertyGroup, "dontHavePropertyGroup");

            return tog.ToString();
        }
        return null;

    }

    public static
#if ASYNC
    async Task<string>
#else
    string
#endif
 PlaformTargetTo(ILogger logger, string replaceFor, string folderNonRec, bool throwEx = false)
    {
        if (ExtractArchive != null)
        {
            var zf = FS.Combine(folderNonRec, FS.GetFileName(folderNonRec) + AllExtensions.zip);
            ExtractArchive(zf, true);
        }

        var gf = FSGetFiles.GetFiles(logger, folderNonRec, "*.csproj", false);
        return
#if ASYNC
    await
#endif
 Shared.PlatformTargetTo(replaceFor, gf, throwEx);
    }

    /// <summary>
    /// Vyu��v� se v ChangeConvertNonWebPlatformTargetTo(), PlatformTargetTo a PlatformTargetToWeb()
    ///
    /// </summary>
    /// <param name="replaceFor"></param>
    /// <param name="tt"></param>
    /// <param name="throwEx"></param>
    /// <returns></returns>
    public static
#if ASYNC
    async Task<string>
#else
    string
#endif
 PlatformTargetTo(string replaceFor, List<string> tt, bool throwEx = false)
    {
        const string PropertyGroup = "<PropertyGroup>";
        const string start = "<PlatformTarget>";
        const string end = "</PlatformTarget>";

        StringBuilder f2 = new StringBuilder();

        if (replaceFor.StartsWith("!"))
        {
            replaceFor = replaceFor.Substring(1);
            string start2 = start + replaceFor + end;
            foreach (var item in tt)
            {
                var f =
#if ASYNC
    await
#endif
 TF.ReadAllText(item);
                f2.Clear();
                f2.Append(f.Replace(start, string.Empty));
                var f2s = f2.ToString();
                if (f != f2s)
                {
                    await TF.WriteAllText(item, f2s);
                }
            }
        }
        else
        {
            return
#if ASYNC
    await
#endif
 Shared.ReplaceTargetPlatform(replaceFor, PropertyGroup, start, end, tt, throwEx);
        }

        return null;
    }
}