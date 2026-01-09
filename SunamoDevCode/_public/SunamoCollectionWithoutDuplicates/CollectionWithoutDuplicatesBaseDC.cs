namespace SunamoDevCode._public.SunamoCollectionWithoutDuplicates;

/// <summary>
/// Base class for collections without duplicates
/// </summary>
public abstract class CollectionWithoutDuplicatesBaseDC<T>
{
    /// <summary>
    /// Gets or sets the internal collection
    /// </summary>
    public List<T> Collection { get; set; } = null;

    /// <summary>
    /// Gets or sets the string representations for comparison
    /// </summary>
    public List<string> StringRepresentations { get; set; } = null;

    private bool? _allowNull = false;

    /// <summary>
    /// Gets or sets whether null values are allowed
    /// </summary>
    public bool? AllowNull
    {
        get => _allowNull;
        set
        {
            _allowNull = value;
            if (value.HasValue && value.Value)
            {
                StringRepresentations = new List<string>(InitialCapacity);
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to break on construction for debugging
    /// </summary>
    public static bool BreakOnConstruction { get; set; } = false;

    private int InitialCapacity { get; set; } = 10000;

    /// <summary>
    /// Initializes a new collection without duplicates
    /// </summary>
    public CollectionWithoutDuplicatesBaseDC()
    {
        if (BreakOnConstruction)
        {
            System.Diagnostics.Debugger.Break();
        }
        Collection = new List<T>();
    }

    /// <summary>
    /// Initializes a new collection with specified capacity
    /// </summary>
    /// <param name="capacity">Initial capacity</param>
    public CollectionWithoutDuplicatesBaseDC(int capacity)
    {
        this.InitialCapacity = capacity;
        Collection = new List<T>(capacity);
    }

    /// <summary>
    /// Initializes a new collection from existing list
    /// </summary>
    /// <param name="initialList">Initial list of items</param>
    public CollectionWithoutDuplicatesBaseDC(IList<T> initialList)
    {
        Collection = new List<T>(initialList.ToList());
    }

    /// <summary>
    /// Adds a value to the collection if it doesn't exist
    /// </summary>
    /// <param name="value">Value to add</param>
    /// <returns>True if value was added, false if it already existed</returns>
    public bool Add(T value)
    {
        bool result = false;
        var containsResult = Contains(value);
        if (containsResult.HasValue)
        {
            if (!containsResult.Value)
            {
                Collection.Add(value);
                result = true;
            }
        }
        else
        {
            if (!AllowNull.HasValue)
            {
                Collection.Add(value);
                result = true;
            }
        }
        if (result)
        {
            if (IsComparingByString())
            {
                StringRepresentations.Add(ItemString);
            }
        }
        return result;
    }

    /// <summary>
    /// Determines if comparison is done by string representation
    /// </summary>
    /// <returns>True if comparing by string</returns>
    protected abstract bool IsComparingByString();

    /// <summary>
    /// Gets or sets the string representation of the current item
    /// </summary>
    protected string ItemString { get; set; } = null;

    /// <summary>
    /// Checks if the collection contains the specified value
    /// </summary>
    /// <param name="value">Value to check</param>
    /// <returns>True if contains, false if not, null if value is null and nulls are not allowed</returns>
    public abstract bool? Contains(T value);

    /// <summary>
    /// Adds value and returns its index
    /// </summary>
    /// <param name="value">Value to add</param>
    /// <returns>Index of the value</returns>
    public abstract int AddWithIndex(T value);

    /// <summary>
    /// Gets the index of the specified value
    /// </summary>
    /// <param name="value">Value to find</param>
    /// <returns>Index of the value</returns>
    public abstract int IndexOf(T value);

    private List<T> WasNotAdded { get; set; } = new List<T>();

    /// <summary>
    /// Adds range of items to the collection
    /// </summary>
    /// <param name="list">List of items to add</param>
    /// <returns>List of items that were not added (already existed)</returns>
    public List<T> AddRange(IList<T> list)
    {
        WasNotAdded.Clear();
        foreach (var item in list)
        {
            if (!Add(item))
            {
                WasNotAdded.Add(item);
            }
        }
        return WasNotAdded;
    }

    /// <summary>
    /// Dumps collection as string
    /// </summary>
    /// <param name="operation">Operation name</param>
    /// <param name="dumpAsStringHeaderArgs">Header arguments</param>
    /// <returns>String representation</returns>
    public string DumpAsString(string operation, object dumpAsStringHeaderArgs)
    {
        throw new Exception("Cannot be here because DumpListAsStringOneLine was moved to sunamo and will stay there");
    }
}
