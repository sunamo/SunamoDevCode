namespace SunamoDevCode;
public class SHNotTranslateAble
{
    public static string DecodeSlashEncodedString(string value)
    {
        // was added ; after 1,2 line and  after 2,3
        // keep as was writte
        value = SHReplace.ReplaceAll(value, "\\", "\\\\");
        value = SHReplace.ReplaceAll(value, "\"", "\\\"");
        value = SHReplace.ReplaceAll(value, "\'", "\\\'");
        return value;
    }
}
