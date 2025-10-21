// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
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
