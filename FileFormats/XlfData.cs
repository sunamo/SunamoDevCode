namespace SunamoDevCode.FileFormats;

/// <summary>
/// Trans-units in *.xlf file and others
/// </summary>
public class XlfData
{
    public string path = null;
    public XElement group = null;
    public XDocument xd = null;
    public List<XElement> trans_units = null;
    public List<string> allids = null;
        
    public void FillIds()
    {
        allids = new List<string>(trans_units.Count);

        foreach (var item in trans_units)
        {
            allids.Add(XmlLocalisationInterchangeFileFormat.Id(item));
        }
    }
}
