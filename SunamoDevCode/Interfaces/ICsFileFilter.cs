namespace SunamoDevCode.Interfaces;

public interface ICsFileFilter
{
    List<string> GetFilesFiltered(string p, string masc, SearchOption so);
}
