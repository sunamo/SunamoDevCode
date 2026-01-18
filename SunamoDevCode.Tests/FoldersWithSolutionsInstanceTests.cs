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
        var solutions = instance.GetSolutions(Enums.RepositoryLocal.Vs17);
        //instance.Reload()
    }

    [Fact]
    public void Reload_IndexesProjectsInWhenFolder_WhenRootIsEVs()
    {
        // Arrange
        var rootPath = @"E:\vs\";
        var fws = new FoldersWithSolutions(logger, rootPath, null, false);

        // Act
        var solutions = fws.Reload(logger, rootPath, null);

        // Assert
        // Verify that if E:\vs\Projects\_When exists and contains projects, they are indexed
        var whenFolderPath = Path.Combine(rootPath, "Projects", "_When");
        if (Directory.Exists(whenFolderPath))
        {
            var subFolders = Directory.GetDirectories(whenFolderPath);
            if (subFolders.Length > 0)
            {
                // Should have at least one solution with Projects\_When in path
                var whenProjects = solutions.Where(s => s.FullPathFolder.Contains(@"Projects\_When")).ToList();
                Assert.NotEmpty(whenProjects);

                // Verify each subfolder in _When is represented in solutions
                foreach (var subFolder in subFolders)
                {
                    var folderName = Path.GetFileName(subFolder);
                    var matchingSolution = solutions.FirstOrDefault(s =>
                        s.FullPathFolder.Contains(@"Projects\_When") &&
                        s.FullPathFolder.EndsWith(folderName + @"\"));

                    Assert.NotNull(matchingSolution);
                }
            }
        }
    }
}