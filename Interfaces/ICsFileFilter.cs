namespace SunamoDevCode;

public interface ICsFileFilter
{
    List<string> GetFilesFiltered(string p, string masc, SearchOption so);
}
