namespace SunamoDevCode.Aps.Projs.Data.ItemGroup;

using SunamoDevCode.Aps.Projs.Data.ItemGroup;

public class NoneItemGroup : ItemGroupElement
{
    public NoneItemGroup() : base(string.Empty)
    {
    }
    public override XmlNode ToXml(XmlDocument xd)
    {
        XmlGenerator x = new XmlGenerator();
        x.WriteNonPairTagWithAttr(VsProjectItemTypes.None, "Include", Include);
        XmlNode xn = XH.ReturnXmlNode(x.ToString(), xd);
        return xn;
    }
}