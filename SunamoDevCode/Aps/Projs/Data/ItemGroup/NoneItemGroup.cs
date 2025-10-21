// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.Aps.Projs.Data.ItemGroup;

public class NoneItemGroup : ItemGroupElement
{
    public NoneItemGroup() : base(string.Empty)
    {
    }
    public override XmlNode ToXml(XmlDocument xd)
    {
        XmlGenerator x = new XmlGenerator();
        x.WriteNonPairTagWithAttr(VsProjectItemTypes.None, "Include", Include);
        XmlNode xn = XH.ReturnXmlNode(x.ToString());
        return xn;
    }
}