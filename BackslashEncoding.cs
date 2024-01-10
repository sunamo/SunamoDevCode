namespace SunamoDevCode;

public class BackslashEncoding
{
    public static void RemoveWhichIsInString(string s, List<int> pl)
    {
        var ft = CSharpHelperSunamo.DetectFromToString(s);
        for (int i = 0; i < pl.Count; i++)
        {
            if (ft.IsInRange(pl[i]))
            {
                pl.RemoveAt(i);
            }
        }
    }
}
