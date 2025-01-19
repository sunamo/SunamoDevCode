using SunamoDevCode.Aps.Enums;
using SunamoDevCode.Aps.Projs.Data.ItemGroup;

namespace SunamoDevCode.Aps.Projs;
public partial class VsProjectsFileHelper
{
    /// <summary>
    /// Used in:
    /// MoveClassElementIntoSharedFileUC
    /// AddFilesToCsproj
    ///
    /// Work with purely xml class
    /// Using for Compile tag
    /// If A4 = null, no control whether already exists
    /// </summary>
    /// <param name="pathCsproj"></param>
    /// <param name="ig"></param>
    /// <param name="ige"></param>
    public static
#if ASYNC
            async Task
#else
        void
#endif
                AddItemGroupSdkStyle(string pathCsproj, ItemGroups ig, ItemGroupElement ige, bool writeToStorage)
    {
        ResultWithException<XmlDocument> xd2 = null;
#if ASYNC
        await
#endif
XmlDocumentsCache.Get(pathCsproj);
        if (MayExcHelper.MayExc(xd2.Exc))
        {
            return;
        }
        if (xd2.Data == null)
        {
            return;
        }
        XmlDocument xd = new XmlDocument();
        string content = xd2.Data.OuterXml; //TF.ReadAllText(pathCsproj);
        string uri = "https://schemas.microsoft.com/developer/msbuild/2003";
        string from = "xmlns=\"";
        string to = "xmlns2=\"";
        //content = content.Replace(uri, string.Empty);
        content = content.Replace(from, to);
        XmlNamespaceManager nsmgr;
        XmlNode itemGroup, parent, project;
        LoadXml(ig, xd, content, out nsmgr, out itemGroup, out parent, out project);
        if (itemGroup == null)
        {
            #region No ItemGroup, add new
            itemGroup = AddNewItemGroup(pathCsproj, xd, nsmgr, itemGroup, project);
            #endregion
        }
        else
        {
            var igs = ig.ToString();
            #region Item group isnt null
            if (xd.SelectSingleNode(@"//Project/ItemGroup/" + igs + "[@Include='" + ige.Include + "']", nsmgr) != null)
            {
                // Already Exists
                return;
            }
            parent = xd.SelectSingleNode(@"//Project/ItemGroup/" + igs, nsmgr);
            if (parent == null)
            {
                if (ig == ItemGroups.Compile)
                {
                    // Is .net standard project whcih dont have any compile
                    return;
                }
            }
            if (parent != null)
            {
                itemGroup = parent.ParentNode;
            }
            else
            {
                itemGroup = AddNewItemGroup(pathCsproj, xd, nsmgr, itemGroup, project);
            }
            #region MyRegion
            //if (parent == null)
            //{
            //    var nodes = xd.SelectNodes("//Project/ItemGroup/Compile", nsmgr);
            //    parent = nodes[0].ParentNode;
            //}
            //else
            //{
            //    parent = parent.ParentNode;
            //}
            #endregion
            #endregion
        }
        Type tIge = ige.GetType();
        if (tIge == typeof(CompileItemGroup))
        {
            #region Add ItemGroup
            CompileItemGroup compileItemGroup = (CompileItemGroup)ige;
            #region MyRegion
            //string include = string.Empty;
            //// originally was here this. But dont know purpose of it. If I add reference to sunamo from sunamo.web need it without Substring(6)
            ////include = compileItemGroup.Include.Substring(6);
            //include = compileItemGroup.Include;
            //Dictionary<string, string> dict = new Dictionary<string, string>();
            //if (!string.IsNullOrWhiteSpace(compileItemGroup.Link))
            //{
            //    dict.Add("Link", compileItemGroup.Link);
            //}
            #endregion
            var xn = compileItemGroup.ToXml(xd);
            xn = xd.ImportNode(xn, true);
            //string s2 = xn.OuterXml;
            itemGroup.PrependChild(xn);
            #endregion
        }
        else if (tIge == ProjectReferenceItemGroup.type)
        {
            ProjectReferenceItemGroup compileItemGroup = (ProjectReferenceItemGroup)ige;
            #region MyRegion
            //string include = string.Empty;
            //// originally was here this. But dont know purpose of it. If I add reference to sunamo from sunamo.web need it without Substring(6)
            ////include = compileItemGroup.Include.Substring(6);
            //include = compileItemGroup.Include;
            //Dictionary<string, string> dict = new Dictionary<string, string>();
            //if (!string.IsNullOrWhiteSpace(compileItemGroup.Link))
            //{
            //    dict.Add("Link", compileItemGroup.Link);
            //}
            #endregion
            var xn = compileItemGroup.ToXml(xd);
            xn = xd.ImportNode(xn, true);
            itemGroup.PrependChild(xn);
        }
        else if (tIge == ReferenceItemGroup.type)
        {
            ReferenceItemGroup compileItemGroup = (ReferenceItemGroup)ige;
            var xn = compileItemGroup.ToXml(xd);
            xn = xd.ImportNode(xn, true);
            itemGroup.PrependChild(xn);
        }
        else
        {
            ThrowEx.NotImplementedCase(tIge);
        }
        if (content.Contains(uri))
        {
            ////nsmgr.AddNamespace("ns", uri);
            //var attr = xd.CreateAttribute("xmlns2");
            //attr.Value = uri;
            //project.Attributes.Append(attr);
        }
        await XmlDocumentsCache.Set(pathCsproj, xd, writeToStorage);
    }
}