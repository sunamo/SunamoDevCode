// variables names: ok
// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode;

// InstantSB(can specify own delimiter, check whether dont exists)
// TextBuilder(implements Undo, save to Sb or List)
// HtmlSB(Same as InstantSB, use br)
public class HtmlSB : InstantSB
{
    public HtmlSB() : base("<br /")
    {
    }
}
