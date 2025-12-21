namespace SunamoDevCode._sunamo.SunamoConverters.Converts;

internal class ConvertPascalConvention //: IConvertConvention
{
        /// <summary>
    /// Will include numbers
    /// hello world = HelloWorld
    /// Hello world = HelloWorld
    /// helloWorld = HelloWorld
    /// </summary>
    /// <param name="p"></param>
    internal static string ToConvention(string p)
    {
        StringBuilder stringBuilder = new StringBuilder();
        bool dalsiVelke = false;
        foreach (char item in p)
        {
            if (dalsiVelke)
            {
                if (char.IsUpper(item))
                {
                    dalsiVelke = false;
                    stringBuilder.Append(item);
                    continue;
                }
                else if (char.IsLower(item))
                {
                    dalsiVelke = false;
                    stringBuilder.Append(char.ToUpper(item));
                    continue;
                }
                else if (char.IsDigit(item))
                {
                    dalsiVelke = true;
                    stringBuilder.Append(item);
                    continue;
                }
                else
                {
                    continue;
                }
            }
            if (char.IsUpper(item))
            {
                stringBuilder.Append(item);
            }
            else if (char.IsLower(item))
            {
                stringBuilder.Append(item);
            }
            else if (char.IsDigit(item))
            {
                stringBuilder.Append(item);
            }
            else
            {
                dalsiVelke = true;
            }
        }
        var result = stringBuilder.ToString().Trim();
        StringBuilder sb2 = new StringBuilder(result);
        sb2[0] = char.ToUpper(sb2[0]);
        //result = SH.FirstCharUpper(result);
        return result;
    }
}