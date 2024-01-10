namespace SunamoDevCode.Data;

public class UnindexableFiles
{
    public static UnindexableFiles uf = new UnindexableFiles();

    private UnindexableFiles()
    {
    }

    public CollectionWithoutDuplicates<string> unindexablePathPartsFiles = new CollectionWithoutDuplicates<string>();
    public CollectionWithoutDuplicates<string> unindexableFileNamesFiles = new CollectionWithoutDuplicates<string>();
    public CollectionWithoutDuplicates<string> unindexableFileNamesExactlyFiles = new CollectionWithoutDuplicates<string>();
    public CollectionWithoutDuplicates<string> unindexablePathEndsFiles = new CollectionWithoutDuplicates<string>();
    public CollectionWithoutDuplicates<string> unindexablePathStartsFiles = new CollectionWithoutDuplicates<string>();
}
