namespace SunamoDevCode;


internal interface IPercentCalculatorDC
{
    double _overallSum { get; set; }
    double last { get; set; }
    IPercentCalculatorDC Create(double overallSum);
    void AddOnePercent();
    int PercentFor(double value, bool last);
    void ResetComputedSum();
}