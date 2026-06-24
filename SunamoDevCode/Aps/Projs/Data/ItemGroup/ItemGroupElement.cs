namespace SunamoDevCode.Aps.Projs.Data.ItemGroup;

public abstract class ItemGroupElement
{
    public ItemGroupElement(string fullPath)
    {
        FullPath = fullPath;
    }

    // Must exist. From it and relative structure, other properties are filled.
    public string FullPath { get; set; }

    public string Include { get; set; } = null!;

    public abstract XmlNode ToXml(XmlDocument xmlDocument);
}
