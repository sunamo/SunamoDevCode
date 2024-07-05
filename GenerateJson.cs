namespace SunamoDevCode;

public class GenerateJson
{
    public static Dictionary<string, string> WithSnakeConventionDict(List<string> l)
    {
        Dictionary<string, string> d = new Dictionary<string, string>();
        foreach (var item in l)
        {
            d.Add(ConvertSnakeConvention.ToConvention(item), item);
        }
        return d;
    }

    public static string WithSnakeConvention(List<string> l)
    {
        var d = WithSnakeConventionDict(l);
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("{");
        foreach (var item in d)
        {
            sb.AppendLine($"{SH.WrapWithQm(item.Key) + ": " + SH.WrapWithQm(item.Value)},");
        }
        sb.AppendLine("}");

        return sb.ToString();
    }
}
