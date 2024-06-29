

namespace SunamoDevCode;


public class UnindexableFiles
{
    public static UnindexableFiles uf = new UnindexableFiles();

    private UnindexableFiles()
    {
    }

    public CollectionWithoutDuplicatesDevCode<string> unindexablePathPartsFiles = new CollectionWithoutDuplicatesDevCode<string>();
    public CollectionWithoutDuplicatesDevCode<string> unindexableFileNamesFiles = new CollectionWithoutDuplicatesDevCode<string>();
    public CollectionWithoutDuplicatesDevCode<string> unindexableFileNamesExactlyFiles = new CollectionWithoutDuplicatesDevCode<string>();
    public CollectionWithoutDuplicatesDevCode<string> unindexablePathEndsFiles = new CollectionWithoutDuplicatesDevCode<string>();
    public CollectionWithoutDuplicatesDevCode<string> unindexablePathStartsFiles = new CollectionWithoutDuplicatesDevCode<string>();
}
