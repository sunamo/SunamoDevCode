namespace SunamoDevCode._public.SunamoCollectionWithoutDuplicates;

/// <summary>
/// Collection without duplicates implementation
/// </summary>
/// <typeparam name="T">Type of items in collection</typeparam>
public class CollectionWithoutDuplicatesDC<T> : CollectionWithoutDuplicatesBaseDC<T>
{
    /// <summary>
    /// Initializes a new collection without duplicates
    /// </summary>
    public CollectionWithoutDuplicatesDC() : base()
    {
    }

    /// <summary>
    /// Initializes a new collection with specified capacity
    /// </summary>
    /// <param name="count">Initial capacity</param>
    public CollectionWithoutDuplicatesDC(int count) : base(count)
    {
    }

    /// <summary>
    /// Initializes a new collection from existing list
    /// </summary>
    /// <param name="list">Initial list of items</param>
    public CollectionWithoutDuplicatesDC(IList<T> list) : base(list)
    {
    }

    /// <summary>
    /// Adds value with index, returns index of the value (existing or newly added)
    /// </summary>
    /// <param name="value">Value to add</param>
    /// <returns>Index of the value in collection</returns>
    public override int AddWithIndex(T value)
    {
        if (IsComparingByString())
        {
            if (Contains(value).GetValueOrDefault())
            {

            }
            else
            {
                Add(value);
                return Collection.Count - 1;
            }
        }
        int index = Collection.IndexOf(value);
        if (index == -1)
        {
            Add(value);
            return Collection.Count - 1;
        }
        return index;
    }

    /// <summary>
    /// Checks if collection contains the specified value
    /// </summary>
    /// <param name="value">Value to check</param>
    /// <returns>True if contains, false if not, null if value is null and nulls are not allowed</returns>
    public override bool? Contains(T value)
    {
        if (IsComparingByString())
        {
            ItemString = value.ToString();
            return StringRepresentations.Contains(ItemString);
        }
        else
        {
            if (!Collection.Contains(value))
            {
                if (EqualityComparer<T>.Default.Equals(value, default(T)))
                {
                    return null;
                }
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Gets index of the specified value, adds it if not present
    /// </summary>
    /// <param name="value">Value to find or add</param>
    /// <returns>Index of the value in collection</returns>
    public override int IndexOf(T value)
    {
        if (IsComparingByString())
        {
            return StringRepresentations.IndexOf(value.ToString());
        }
        int index = Collection.IndexOf(value);
        if (index == -1)
        {
            Collection.Add(value);
            return Collection.Count - 1;
        }
        return index;
    }

    /// <summary>
    /// Determines if comparing by string representation
    /// </summary>
    /// <returns>True if comparing by string</returns>
    protected override bool IsComparingByString()
    {
        return AllowNull.HasValue && AllowNull.Value;
    }
}