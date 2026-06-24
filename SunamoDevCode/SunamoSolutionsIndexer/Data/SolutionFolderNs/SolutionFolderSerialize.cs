namespace SunamoDevCode.SunamoSolutionsIndexer.Data.SolutionFolderNs;

public class SolutionFolderSerialize : IListBoxHelperItem, ISolutionFolderSerialize
{
    public override string ToString()
    {
        return DisplayedText;
    }

    public static Type Type { get; } = typeof(SolutionFolderSerialize);

    private string _DisplayedText = "";

    public string DisplayedText
    {
        get => _DisplayedText;
        set => _DisplayedText = value;
    }

    public string _FullPathFolder = "";

    public string _NameSolution = "";

    public string projectFolder { get; set; } = null!;

    public string slnFullPath { get; set; } = null!;

    public string FullPathFolder
    {
        get => _FullPathFolder;
        set
        {
            _FullPathFolder = value;
            _NameSolution = Path.GetFileName(value.TrimEnd('\\'));
            if (SolutionsIndexerSettings.IgnorePartAfterUnderscore)
            {
                int lastUnderscoreIndex = _NameSolution.LastIndexOf('_');
                if (lastUnderscoreIndex != -1)
                {
                    _NameSolution = _NameSolution.Substring(0, lastUnderscoreIndex);
                }
            }
        }
    }

    public string NameSolution => _NameSolution;

    public string RunOne => FullPathFolder;

    public string ShortName => _NameSolution;

    public string LongName => _FullPathFolder;
}
