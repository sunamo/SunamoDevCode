namespace SunamoDevCode._public.SunamoData.Data;

/// <summary>
/// Represents a list of from-to ranges and provides methods to check whether a value falls within any range.
/// </summary>
public class FromToList
{
    /// <summary>
    /// Gets or sets the list of from-to range definitions.
    /// </summary>
    public List<FromToDC> Ranges { get; set; } = new();
    /// <summary>
    /// Checks whether the given value falls within any of the defined ranges (exclusive boundaries).
    /// </summary>
    /// <param name="value">Value to check.</param>
    /// <returns>True if the value is within any range.</returns>
    public bool IsInRange(int value)
    {
        foreach (var item in Ranges)
            if (value < item.to && value > item.from)
                return true;
        return false;
    }
}