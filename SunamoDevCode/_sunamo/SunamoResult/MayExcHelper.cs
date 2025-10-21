// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode._sunamo.SunamoResult;

internal class MayExcHelper
{
    internal static bool MayExc(string exc)
    {
        if (exc != null)
        {
            Console.WriteLine(exc);
            //ThisApp.Error( result.exc);
            return true;
        }

        return false;
    }
}