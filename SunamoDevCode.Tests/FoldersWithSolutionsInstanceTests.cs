// variables names: ok
namespace SunamoDevCode.Tests.SunamoSolutionsIndexer;

using Microsoft.Extensions.Logging;
using SunamoDevCode.SunamoSolutionsIndexer;
using SunamoPaths;
using SunamoTest;

public class FoldersWithSolutionsTests
{
    ILogger logger = TestLogger.Instance;

    [Fact]
    public void InsertIntoFwssTest()
    {
        FoldersWithSolutions.InsertIntoFwss(logger, DefaultPaths.eVs, null);
    }

    [Fact]
    public void Reload_AllProjectFoldersIsLoaded()
    {
        FoldersWithSolutions foldersWithSolutionsInstance = new(TestLogger.Instance, @"E:\vs\", null);


    }

    [Fact]
    public void ReloadTest_WithNoAddingSlns()
    {
        var fws = new FoldersWithSolutions(logger, DefaultPaths.eVs, null, false);
    }

    [Fact]
    public void ReloadTest_WithAddSlns()
    {
        var fws = new FoldersWithSolutions(logger, DefaultPaths.eVs, null, true);
    }

    [Fact]
    public void ReloadTest()
    {
        var basePath = @"E:\vs\";

        //DefaultPaths.eVs = basePath;
        FoldersWithSolutions.PairProjectFolderWithEnum(TestLogger.Instance, basePath);
        FoldersWithSolutions instance = new FoldersWithSolutions(TestLogger.Instance, basePath, null, false);
        instance.Reload(TestLogger.Instance, basePath, null);
        var solutions = instance.Solutions(Enums.RepositoryLocal.Vs17);
        //instance.Reload()
    }
}