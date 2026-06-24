namespace SunamoDevCode.Aps.Projs.Data.ItemGroup;

public class NoneItemGroup : ItemGroupElement
{
    public NoneItemGroup() : base(string.Empty)
    {
    }

    public override XmlNode ToXml(XmlDocument xmlDocument)
    {
        XmlGenerator generator = new XmlGenerator();
        generator.WriteNonPairTagWithAttr(VsProjectItemTypes.None, "Include", Include);
        XmlNode node = XH.ReturnXmlNode(generator.ToString()!)!;
        return node;
    }
}
