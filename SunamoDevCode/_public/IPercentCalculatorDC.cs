namespace SunamoDevCode._public;

/// <summary>
/// EN: Interface for calculating percentages for progress tracking
/// CZ: Rozhraní pro výpočet procent pro sledování postupu
/// </summary>
public interface IPercentCalculatorDC
{
    /// <summary>
    /// Gets or sets the total sum representing 100%.
    /// </summary>
    double OverallSum { get; set; }
    /// <summary>
    /// Gets or sets the last calculated percentage value.
    /// </summary>
    double Last { get; set; }
    /// <summary>
    /// Creates a new percent calculator instance with the specified overall sum.
    /// </summary>
    /// <param name="overallSum">Total sum representing 100%.</param>
    /// <returns>A new percent calculator instance.</returns>
    IPercentCalculatorDC Create(double overallSum);
    /// <summary>
    /// Adds one percent to the computed sum.
    /// </summary>
    void AddOnePercent();
    /// <summary>
    /// Calculates the percentage for the given value relative to the overall sum.
    /// </summary>
    /// <param name="value">Value to calculate percentage for.</param>
    /// <param name="isLast">Whether this is the last value in the sequence.</param>
    /// <returns>Calculated percentage as an integer.</returns>
    int PercentFor(double value, bool isLast);
    /// <summary>
    /// Resets the computed sum back to zero.
    /// </summary>
    void ResetComputedSum();
}