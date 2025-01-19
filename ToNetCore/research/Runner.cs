

public partial class MoveToNet5
{
    public List<FoldersWithSolutions> fwss => FoldersWithSolutions.fwss;

    #region Helper methods

    #endregion

    private string ListOfProjectsWhichIsWebAndWhichIsNotWeb()
    {
        var t = WebAndNonWebProjects();

        TextOutputGenerator tog = new TextOutputGenerator();
        tog.List(t.Item1, "Web projects");
        tog.List(t.Item2, "Not web projects");
        var output = tog.ToString();

        //#if DEBUG
        //        ProgramShared.Output = output;
        //        ProgramShared.OutputOpen();
        //#elif !DEBUG
        //        ProgramShared.Output = output;
        //        ProgramShared.OutputOpen();
        //        //showTextResultWindow = new ShowTextResultWindow(output);
        //        //showTextResultWindow.ShowDialog();
        //#endif

        return output;
    }

    //Tuple<List<string>, List<string>> WebAndNonWebProjects()
    //{
    //    MoveToNet5 m = new MoveToNet5();
    //    return m.WebAndNonWebProjects();
    //}


}
