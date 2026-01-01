namespace SunamoDevCode.Aps.Args;

/// <summary>
/// EN: Arguments for asking user to select projects or solutions
/// CZ: Argumenty pro dotázání uživatele na výběr projektů nebo solutions
/// </summary>
public class AskForProjectsOrSlnsArgs
{
    /// <summary>
    /// EN: Hint text to display to user
    /// CZ: Text nápovědy k zobrazení uživateli
    /// </summary>
    public string Hint = string.Empty;

    /// <summary>
    /// EN: Name of solution to start from
    /// CZ: Název solution ze kterého začít
    /// </summary>
    public string? NameSlnFrom = null;

    /// <summary>
    /// EN: Tag for checkbox list
    /// CZ: Tag pro checkbox list
    /// </summary>
    public object? ChblTag = null;

    /// <summary>
    /// EN: Whether to ask for solutions instead of projects
    /// CZ: Zda se ptát na solutions místo projektů
    /// </summary>
    public bool Slns = false;

    /// <summary>
    /// EN: Whether to show dialog to make sure about replaced with session i18n
    /// CZ: Zda zobrazit dialog pro ujištění o nahrazení session i18n
    /// </summary>
    public bool DialogForMakeSureAboutReplacedWithSessI18n = false;
}