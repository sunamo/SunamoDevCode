// variables names: ok
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SunamoDevCode.SunamoSolutionsIndexer;

namespace SunamoDevCode.Tests.SunamoSolutionsIndexer.Data.SolutionFolderNs;

public class SolutionFolderTests //: TestsBase
{
    ILogger logger = NullLogger.Instance;

    [Fact]
    public void ExeToReleaseTest()
    {
        var appWithoutProjectDistinction = "ConsoleApp1";
        var projectDistinction = "";

        FoldersWithSolutions.InsertIntoFwss(logger, @"E:\vs", null);

        var app = SolutionsIndexerHelper.SolutionWithName(appWithoutProjectDistinction);
        app.ExeToRelease(app, projectDistinction, true);


    }
}
