namespace SunamoDevCode;


/// <summary>
/// Když nemám očíslované, počítá od 0. tedy warning = 0, error = 1, fatal = 2, ve VS debuggeru při error | fatal vidím 3 
/// </summary>
internal enum GitTypesOfMessages
{
    warning,
    error,
    fatal
}