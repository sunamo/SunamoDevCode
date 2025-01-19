
namespace Aps.Projs._;
public partial class VsProjectsFileHelper
{
    const string sdkAttrName = "Sdk";
    /// <summary>
    /// Vytvoří mi nový vsproj z templaty VS2019
    /// </summary>
    /// <param name="safeProjectName"></param>
    /// <returns></returns>
    public static
#if ASYNC
        async Task<string>
#else
            string
#endif
        XmlClassLibraryFromTemplate(string safeProjectName)
    {
        var p = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\ProjectTemplates\CSharp\Windows\1033\ClassLibrary\classlibrary.csproj";
        var c =
#if ASYNC
            await
#endif
            TF.ReadAllText(p);
        ReplaceProjectTemplateParameter(ref c, VsProjectTemplateParameters.guid1, Guid.NewGuid());
        ReplaceProjectTemplateParameter(ref c, VsProjectTemplateParameters.safeprojectname, safeProjectName);
        ReplaceProjectTemplateParameter(ref c, VsProjectTemplateParameters.targetframeworkversion, "4.7.2");
        var l = SHGetLines.GetLines(c);
        string trimmedLine = null;
        for (int i = l.Count - 1; i >= 0; i--)
        {
            trimmedLine = l[i].Trim();
            if (trimmedLine.StartsWith("$if$") || trimmedLine.StartsWith("$endif"))
            {
                l.RemoveAt(i);
                continue;
            }
            if (trimmedLine == "<Compile Include=\"Class1.cs\" />" || trimmedLine == "<Compile Include=\"Properties\\AssemblyInfo.cs\" />")
            {
                l.RemoveAt(i);
                continue;
            }
        }
        return SHJoin.JoinNL(l);
    }
}