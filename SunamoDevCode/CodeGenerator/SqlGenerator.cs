namespace SunamoDevCode.CodeGenerator;

public class SqlGenerator
{
    private StringBuilder stringBuilder = new StringBuilder();

    public void Select(string table)
    {
        stringBuilder.AppendLine("select * from " + table);
    }

    public override string ToString() => stringBuilder.ToString();
}
