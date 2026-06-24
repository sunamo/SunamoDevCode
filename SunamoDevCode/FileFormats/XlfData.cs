namespace SunamoDevCode.FileFormats;

public class XlfData
{
    public string Path { get; set; } = null!;

    public XElement Group { get; set; } = null!;

    public XDocument XmlDocument { get; set; } = null!;

    public List<XElement> TransUnits { get; set; } = null!;

    public List<string> AllIds { get; set; } = null!;

    public void FillIds()
    {
        AllIds = new List<string>(TransUnits.Count);

        foreach (var item in TransUnits)
        {
            AllIds.Add(XmlLocalisationInterchangeFileFormat.Id(item));
        }
    }
}
