namespace SunamoDevCode;

public class SqlGenerator
{
    StringBuilder sb = new StringBuilder();

    public void Select(string table)
    {
        sb.AppendLine("select * from " + table);
    }

    public override string ToString()
    {
        return sb.ToString();
    }
}
