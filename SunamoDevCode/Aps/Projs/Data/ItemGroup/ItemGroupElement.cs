namespace SunamoDevCode.Aps.Projs.Data.ItemGroup;

public abstract class ItemGroupElement
{
    public ItemGroupElement(string _FullPath)
    {
        this._FullPath = _FullPath;
    }



    /// <summary>
    /// To csproj. Must exists. From it and relative structure is filled other properties
    /// </summary>
    public string _FullPath;

    public string Include;

    public abstract XmlNode ToXml(XmlDocument xd);
}