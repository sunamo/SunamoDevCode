namespace SunamoDevCode;


public class GitConfigFileHelper
{
    public const string coreStart = "[core]";
    public const string remoteStart = "[remote ";
    public const string branchStart = "[branch ";
    public const string mergeStart = "[merge]";
    public const string mergetoolStart = "[mergetool]";

    public static string Format(string actual)
    {
        var l = SHGetLines.GetLines(actual);
        for (int i = 0; i < l.Count; i++)
        {
            var line = l[i];
            if (line.StartsWith("["))
            {
                continue;
            }

            if (line.StartsWith("\t"))
            {
                line = "\t" + line;
            }
        }

        return string.Join(Environment.NewLine, l).Trim();
    }

    public static ExistsNonExistsList<GitConfigSection> ExistsBlocks(string s)
    {
        ExistsNonExistsList<GitConfigSection> r = new ExistsNonExistsList<GitConfigSection>();
        var result = r.Exists;
        var l = ParseBlocks(s);
        foreach (var item in l)
        {
            /*
To co upravím v těchto větvích
            musím upravit i v foreach (var item2 in blocks.NonExists)
             */

            if (item.StartsWith(coreStart))
            {
                result.Add(GitConfigSection.core);
            }
            else if (item.StartsWith(remoteStart))
            {
                result.Add(GitConfigSection.remote);
            }
            else if (item.StartsWith(branchStart))
            {
                result.Add(GitConfigSection.branch);
            }
            else if (item == mergeStart || item == mergetoolStart)
            {

            }
            else
            {
                ThrowEx.NotImplementedCase(item);
            }
        }

        var values = Enum.GetValues<GitConfigSection>();
        foreach (var item in values)
        {
            if (!result.Contains(item))
            {
                r.NonExists.Add(item);
            }
        }

        return r;
    }

    public static List<string> ParseBlocks(string s)
    {
        List<string> result = new List<string>();

        var l = SHGetLines.GetLines(s);
        for (int i = 0; i < l.Count; i++)
        {
            var line = l[i];
            if (line.StartsWith("["))
            {
                result.Add(line);
            }
        }

        return result;
    }
}
