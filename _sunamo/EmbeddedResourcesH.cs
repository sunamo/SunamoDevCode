namespace SunamoDevCode;
/// <summary>
///
/// Require assembly and default namespace.
/// Content is referred like with ResourcesH - with fs path
/// </summary>
internal class EmbeddedResourcesH //: IResourceHelper
{
    /*usage:
uri = new Uri("Wpf.Tests.Resources.EmbeddedResource.txt", UriKind.Relative);
GetString(uri.ToString()) - the same string as passed in ctor Uri
     */

    /// <summary>
    /// For entry assembly
    /// </summary>
    internal static EmbeddedResourcesH ci = null;

    protected EmbeddedResourcesH()
    {

    }

    /// <summary>
    /// internal to use in assembly like SunamoNTextCat
    /// A2 is name of project, therefore don't insert typeResourcesSunamo.Namespace
    /// </summary>
    /// <param name="_entryAssembly"></param>
    internal EmbeddedResourcesH(Assembly _entryAssembly, string defaultNamespace)
    {
        this._entryAssembly = _entryAssembly;
        _defaultNamespace = defaultNamespace;
    }

    protected Assembly _entryAssembly = null;
    protected string _defaultNamespace;

    protected Assembly entryAssembly
    {
        get
        {
            if (_entryAssembly == null)
            {
                _entryAssembly = Assembly.GetEntryAssembly();
            }
            return _entryAssembly;
        }
    }

    internal string GetResourceName(string name)
    {
        name = string.Join(".", _defaultNamespace, name.TrimStart(AllChars.slash).Replace(AllStrings.slash, AllStrings.dot));
        return name;
    }

    /// <summary>
    /// If it's file, return its content
    /// Its for getting string from file, never from resx or another in code variable
    /// </summary>
    /// <param name="name"></param>
    internal string GetString(string name)
    {
        var s = GetStream(name);

        return Encoding.UTF8.GetString(FS.StreamToArrayBytes(s));
    }

    /// <summary>
    /// Resources/tidy_config.txt (no assembly)
    /// </summary>
    /// <param name="name"></param>
    internal Stream GetStream(string name)
    {
        var s = GetResourceName(name);
        var vr = entryAssembly.GetManifestResourceStream(s);
        return vr;
    }
}
