namespace SunamoDevCode.Args;

public class RemoveFromXlfWhichHaveEmptyTargetOrSourceArgs
{
    public static RemoveFromXlfWhichHaveEmptyTargetOrSourceArgs Default = new RemoveFromXlfWhichHaveEmptyTargetOrSourceArgs();
    /// <summary>
    /// default var
    /// </summary>
    public bool removeWholeTransUnit = true;
    public bool save = true;
}