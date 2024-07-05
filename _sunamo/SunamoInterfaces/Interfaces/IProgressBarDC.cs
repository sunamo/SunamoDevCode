namespace SunamoDevCode._sunamo.SunamoInterfaces.Interfaces;


internal interface IProgressBarDC
{
    bool isRegistered { get; set; }
    int writeOnlyDividableBy { get; set; }
    void Init(IPercentCalculatorDC pc);
    void Init(IPercentCalculatorDC pc, bool isNotUt);
    /// <summary>
    ///     A1 is to increment done items after really finished async operation. Can be any.
    /// </summary>
    /// <param name="asyncResult"></param>
    void LyricsHelper_AnotherSong(object asyncResult);
    void LyricsHelper_AnotherSong();
    void LyricsHelper_AnotherSong(int i);
    void LyricsHelper_OverallSongs(int obj);
    void LyricsHelper_WriteProgressBarEnd();
}