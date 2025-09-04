// Instance variables refactored according to C# conventions
using SunamoDevCode.Tests.SunamoSolutionsIndexer;

namespace RunnerDevCode;

internal class Program
{
    const string pinp = @"E:\vs\Projects\PlatformIndependentNuGetPackages\";

    static void Main()
    {
        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
        await Task.Delay(1);

        //DotnetOutputServiceTests t = new();
        //t.GetPartsFromDotnetBuildLineTest();


        //GlobalUsingsInstanceTests t = new GlobalUsingsInstanceTests();
        //await t.GlobalUsingsInstance_Test();

        //TFCsFormatTests tFCsFormatTests = new TFCsFormatTests();
        //await tFCsFormatTests.WriteAllLinesTest2();

        FoldersWithSolutionsTests foldersWithSolutionsInstanceTests = new();
        //foldersWithSolutionsInstanceTests.ReloadTest();
        foldersWithSolutionsInstanceTests.ReloadTest_WithAddSlns();

        //AddOrEditNamespaceServiceTests t = new AddOrEditNamespaceServiceTests();
        //await t.AddOrEditNamespaceForSingleFileAndSaveTest();
        //FoldersWithSolutionsTests foldersWithSolutionsInstanceTests = new();
        //foldersWithSolutionsInstanceTests.ReloadTest();

        AddOrEditNamespaceServiceTests t = new AddOrEditNamespaceServiceTests();
        //await t.AddOrEditNamespaceForSingleFileAndSaveTest();

        //FoldersWithSolutionsTests t = new FoldersWithSolutionsTests();
        //t.ReloadTest2();
        //t.ReloadTest();
        //t.InsertIntoFwssTest();

        //SolutionFolderTests t = new();
        //t.ExeToReleaseTest();

        //var t = new CSharpHelperTests();
        //t.RemoveCommentsKeepLinesTest();
        //await t.IsEmptyCommentedOrOnlyWithNamespaceTest();

        //var directoryFiles = FSGetFilesDC.GetFilesDC(pinp, "XlfKeys.cs", SearchOption.AllDirectories, new GetFilesDCArgs { OnlyIn_Sunamo = true });

        //foreach (var filePath in directoryFiles)
        //{
        //    var fileLines = (await File.ReadAllLinesAsync(filePath)).ToList();
        //    CSharpHelper.SetValuesAsNamesToConsts(fileLines);
        //    await File.WriteAllLinesAsync(filePath, fileLines);
        //}
    }
}