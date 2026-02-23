namespace SunamoDevCode;

/// <summary>
/// Arguments for configuring PpkOnDrive collection persistence behavior.
/// </summary>
public class PpkOnDriveDevCodeArgs
{
    /// <summary>
    /// Gets or sets the file path for the collection data file.
    /// </summary>
    public string File { get; set; } = null!;

    /// <summary>
    ///     Originally was false but think that true is better
    ///     Its not important because still I'm using old ctor interface and it will set to false if needed
    /// </summary>
    public bool Load { get; set; } = true;

    /// <summary>
    /// Gets or sets whether to load changes from the drive on initialization.
    /// </summary>
    public bool LoadChangesFromDrive { get; set; } = true;
    /// <summary>
    /// Gets or sets whether to save changes to the drive.
    /// </summary>
    public bool Save { get; set; } = true;
}