namespace SunamoDevCode._public;

/// <summary>
/// EN: Interface for calculating percentages for progress tracking
/// CZ: Rozhraní pro výpočet procent pro sledování postupu
/// </summary>
public interface IPercentCalculatorDC
{
    double OverallSum { get; set; }
    double Last { get; set; }
    IPercentCalculatorDC Create(double overallSum);
    void AddOnePercent();
    int PercentFor(double value, bool isLast);
    void ResetComputedSum();
}