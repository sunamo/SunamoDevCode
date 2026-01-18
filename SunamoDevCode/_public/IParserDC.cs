namespace SunamoDevCode._public;

/// <summary>
/// EN: Parser interface for processing text input
/// CZ: Rozhraní parseru pro zpracování textového vstupu
/// </summary>
public interface IParserDC
{
    /// <summary>
    /// EN: Parses the input text
    /// CZ: Parsuje vstupní text
    /// </summary>
    /// <param name="input">Input text to parse</param>
    void Parse(string input);
}