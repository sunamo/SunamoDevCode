// variables names: ok
namespace SunamoDevCode.Tests;

using SunamoStringGetLines;
using SunamoString;

/// <summary>
/// Tests for CSharpHelper Sunamo string indentation functionality.
/// </summary>
public class CSharpHelperSunamoTests
{
    /// <summary>
    /// Tests that IndentAsPreviousLine correctly aligns code lines to match the indentation of the previous line.
    /// </summary>
    [Fact]
    public void IndentAsPreviousLineTest()
    {
        var input = SHGetLines.GetLines( @"for (int i = 0; i < args.Length; i++)
{
    string nazev = args[i];
HttpCookie cook = new HttpCookie(nazev, args[++i]);
cook.Expires = DateTime.Now.AddYears(1);
    Response.Cookies.Set(cook);
 
}");

        var expected = SHGetLines.GetLines( @"for (int i = 0; i < args.Length; i++)
{
    string nazev = args[i];
    HttpCookie cook = new HttpCookie(nazev, args[++i]);
    cook.Expires = DateTime.Now.AddYears(1);
    Response.Cookies.Set(cook);

}");


        SH.IndentAsPreviousLine(input);

        Assert.Equal(expected, input);
    }
}