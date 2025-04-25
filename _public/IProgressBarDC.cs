namespace SunamoDevCode._public;

public interface IProgressBarDC
{
    bool isRegistered { get; set; }
    int writeOnlyDividableBy { get; set; }
    void Init(IPercentCalculatorDC pc);
    void Init(IPercentCalculatorDC pc, bool isNotUt);
    /// <summary>
    ///     A1 is to increment done items after really finished async operation. Can be any.
    /// </summary>
    /// <param name="asyncResult"></param>
    void DoneOne(object asyncResult);
    void DoneOne();
    void DoneOne(int i);
    void Start(int obj);
    void Done();
}