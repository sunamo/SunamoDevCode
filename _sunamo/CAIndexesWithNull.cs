namespace SunamoDevCode._sunamo;
internal class CAIndexesWithNull
{
    internal static List<int> IndexesWithNull(IList times)
    {
        List<int> nulled = new List<int>();
        int i = 0;
        foreach (var item in times)
        {
            if (item == null)
            {
                nulled.Add(i);
            }
            i++;
        }

        return nulled;
    }
}
