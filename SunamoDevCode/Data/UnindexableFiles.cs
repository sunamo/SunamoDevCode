namespace SunamoDevCode.Data;

public class UnindexableFiles
{
    public static UnindexableFiles uf = new UnindexableFiles();

    private UnindexableFiles()
    {
    }

    public CollectionWithoutDuplicatesDC<string> unindexablePathPartsFiles = new CollectionWithoutDuplicatesDC<string>();
    public CollectionWithoutDuplicatesDC<string> unindexableFileNamesFiles = new CollectionWithoutDuplicatesDC<string>();
    public CollectionWithoutDuplicatesDC<string> unindexableFileNamesExactlyFiles = new CollectionWithoutDuplicatesDC<string>();
    public CollectionWithoutDuplicatesDC<string> unindexablePathEndsFiles = new CollectionWithoutDuplicatesDC<string>();
    public CollectionWithoutDuplicatesDC<string> unindexablePathStartsFiles = new CollectionWithoutDuplicatesDC<string>();
}