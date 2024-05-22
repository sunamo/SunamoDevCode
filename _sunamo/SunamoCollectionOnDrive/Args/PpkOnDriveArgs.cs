namespace SunamoDevCode;


internal class PpkOnDriveArgs
{
    internal string file;
    /// <summary>
    /// Originally was false but think that true is better
    /// Its not important because still I'm using old ctor interface and it will set to false if needed
    /// </summary>
    internal bool load = true;
    internal bool save = true;
    internal bool loadChangesFromDrive = true;
}