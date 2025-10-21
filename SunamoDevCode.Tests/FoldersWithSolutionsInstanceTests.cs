// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy

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
        var parameter = @"E:\vs\";

        //DefaultPaths.eVs = parameter;
        FoldersWithSolutions.PairProjectFolderWithEnum(TestLogger.Instance, parameter);
        FoldersWithSolutions d = new FoldersWithSolutions(TestLogger.Instance, parameter, null, false);
        d.Reload(TestLogger.Instance, parameter, null);
        var slns = d.Solutions(Enums.RepositoryLocal.Vs17);
        //d.Reload()
    }
}