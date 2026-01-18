namespace SunamoDevCode._public;

public interface IProgressBarDC
{
    bool IsRegistered { get; set; }
    int WriteOnlyDividableBy { get; set; }
    void Init(IPercentCalculatorDC pc);
    void Init(IPercentCalculatorDC pc, bool isNotUt);
    /// <summary>
    ///     A1 is to increment done items after really finished async operation. Can be any.
    /// </summary>
    /// <param name="asyncResult"></param>
    void DoneOne(object asyncResult);
    void DoneOne();
    void DoneOne(int count);
    void Start(int totalCount);
    void Done();
}