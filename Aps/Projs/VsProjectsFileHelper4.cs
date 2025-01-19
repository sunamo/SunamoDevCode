using SunamoDevCode.Aps.Enums;

namespace Aps.Projs._;

public partial class VsProjectsFileHelper
{


    private static void LoadXml(ItemGroups ig, XmlDocument xd, string content, out XmlNamespaceManager nsmgr, out XmlNode itemGroup, out XmlNode parent, out XmlNode project)
    {
        xd.PreserveWhitespace = true;
        xd.LoadXml(content);


        nsmgr = new XmlNamespaceManager(xd.NameTable);
        string wanted = ig.ToString();
        itemGroup = xd.SelectSingleNode("//Project/ItemGroup", nsmgr);
        parent = null;
        project = xd.SelectSingleNode("//Project", nsmgr);
    }
}
