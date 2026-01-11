// variables names: ok
namespace SunamoDevCode.Aps.Projs;

public partial class VsProjectsFileHelper
{
    /// <summary>
    /// Adds an ItemGroup element to an SDK-style .csproj file.
    /// Used in: MoveClassElementIntoSharedFileUC, AddFilesToCsproj.
    /// Works with pure XML classes, primarily for Compile tags.
    /// </summary>
    /// <param name="csprojPath">Path to the .csproj file.</param>
    /// <param name="itemGroups">The type of ItemGroup to add (Compile, Reference, etc.).</param>
    /// <param name="itemGroupElement">The ItemGroup element to add to the project.</param>
    /// <param name="isWritingToStorage">Whether to write changes to storage immediately.</param>
    public static
#if ASYNC
            async Task
#else
        void
#endif
                AddItemGroupSdkStyle(string csprojPath, ItemGroups itemGroups, ItemGroupElement itemGroupElement, bool isWritingToStorage)
    {
        ResultWithException<XmlDocument> xmlDocumentResult = null;
#if ASYNC
        await
#endif
XmlDocumentsCache.Get(csprojPath);
        if (MayExcHelper.MayExc(xmlDocumentResult.Exc))
        {
            return;
        }
        if (xmlDocumentResult.Data == null)
        {
            return;
        }
        XmlDocument xmlDocument = new XmlDocument();
        string content = xmlDocumentResult.Data.OuterXml;
        string uri = "https://schemas.microsoft.com/developer/msbuild/2003";
        string from = "xmlns=\"";
        string to = "xmlns2=\"";
        content = content.Replace(from, to);
        XmlNamespaceManager namespaceManager;
        XmlNode itemGroup, parent, project;
        LoadXml(itemGroups, xmlDocument, content, out namespaceManager, out itemGroup, out parent, out project);
        if (itemGroup == null)
        {
            #region No ItemGroup, add new
            itemGroup = AddNewItemGroup(csprojPath, xmlDocument, namespaceManager, itemGroup, project);
            #endregion
        }
        else
        {
            var itemGroupsString = itemGroups.ToString();
            #region Item group isnt null
            if (xmlDocument.SelectSingleNode(@"//Project/ItemGroup/" + itemGroupsString + "[@Include='" + itemGroupElement.Include + "']", namespaceManager) != null)
            {
                // Already Exists
                return;
            }
            parent = xmlDocument.SelectSingleNode(@"//Project/ItemGroup/" + itemGroupsString, namespaceManager);
            if (parent == null)
            {
                if (itemGroups == ItemGroups.Compile)
                {
                    // Is .net standard project which doesn't have any compile
                    return;
                }
            }
            if (parent != null)
            {
                itemGroup = parent.ParentNode;
            }
            else
            {
                itemGroup = AddNewItemGroup(csprojPath, xmlDocument, namespaceManager, itemGroup, project);
            }
            #endregion
        }
        Type itemGroupElementType = itemGroupElement.GetType();
        if (itemGroupElementType == typeof(CompileItemGroup))
        {
            #region Add ItemGroup
            CompileItemGroup compileItem = (CompileItemGroup)itemGroupElement;
            var xmlNode = compileItem.ToXml(xmlDocument);
            xmlNode = xmlDocument.ImportNode(xmlNode, true);
            itemGroup.PrependChild(xmlNode);
            #endregion
        }
        else if (itemGroupElementType == ProjectReferenceItemGroup.Type)
        {
            ProjectReferenceItemGroup projectReferenceItem = (ProjectReferenceItemGroup)itemGroupElement;
            var xmlNode = projectReferenceItem.ToXml(xmlDocument);
            xmlNode = xmlDocument.ImportNode(xmlNode, true);
            itemGroup.PrependChild(xmlNode);
        }
        else if (itemGroupElementType == ReferenceItemGroup.Type)
        {
            ReferenceItemGroup referenceItem = (ReferenceItemGroup)itemGroupElement;
            var xmlNode = referenceItem.ToXml(xmlDocument);
            xmlNode = xmlDocument.ImportNode(xmlNode, true);
            itemGroup.PrependChild(xmlNode);
        }
        else
        {
            ThrowEx.NotImplementedCase(itemGroupElementType);
        }
        await XmlDocumentsCache.Set(csprojPath, xmlDocument, isWritingToStorage);
    }
}