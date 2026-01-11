// variables names: ok
/// <summary>
/// Interface for move to shared folder operations.
/// </summary>
public interface IMoveToShared
{
    /// <summary>
    /// Gets the base folder path.
    /// </summary>
    string Folder { get; }

    /// <summary>
    /// Gets the Sunamo folder path.
    /// </summary>
    string FolderSunamo { get; }

    /// <summary>
    /// Gets the Sunamo web folder path.
    /// </summary>
    string FolderSunamoWeb { get; }

    /// <summary>
    /// Gets the postfix for folder name.
    /// </summary>
    string Postfix { get; }

    /// <summary>
    /// Gets the source solution name.
    /// </summary>
    string SlnFrom { get; }
}