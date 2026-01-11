namespace SunamoDevCode._sunamo.SunamoEmbeddedResources;

/// <summary>
/// Helper for embedded resources
/// Requires assembly and default namespace.
/// Content is referred like with ResourcesH - with fs path
/// </summary>
internal class EmbeddedResourcesH
{
    /// <summary>
    /// Singleton instance for entry assembly
    /// </summary>
    internal static EmbeddedResourcesH Instance = null;

    /// <summary>
    /// Protected constructor for inheritance
    /// </summary>
    protected EmbeddedResourcesH()
    {

    }

    /// <summary>
    /// Constructor for use in assembly like SunamoNTextCat
    /// Parameter is name of project, therefore don't insert typeResourcesSunamo.Namespace
    /// </summary>
    /// <param name="entryAssembly">Entry assembly</param>
    /// <param name="defaultNamespace">Default namespace for resources</param>
    internal EmbeddedResourcesH(Assembly entryAssembly, string defaultNamespace)
    {
        this._entryAssembly = entryAssembly;
        _defaultNamespace = defaultNamespace;
    }

    protected Assembly _entryAssembly = null;
    protected string _defaultNamespace;

    /// <summary>
    /// Gets the entry assembly
    /// </summary>
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
}
