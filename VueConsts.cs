namespace SunamoDevCode;

public class VueConsts
{
    public const string cl = "class ";
    public const string constructor = "constructor";
    public const string import = "import";
    public const string export = "export";
    public const string methodsRcub = "methods:{";
    public const string propsRcub = "props:{";

    // name, inject, mixins, extends, watch
    // https://github.com/pablohpsilva/vuejs-component-style-guide
    public static List<string> sStartWith = CAG.ToList<string>("name:", "inject:", "mixins:", "extends:", "watch:");
    public static List<string> dontAddRcub = CAG.ToList<string>(methodsRcub, propsRcub);
    //List<string> sContains = CAG.ToList<string>("data():", "methods:{");
    public static List<string> containsList = CAG.ToList<string>(cl);

    public static List<string> startWithList = CAG.ToList<string>(constructor, import, "//", export);
    public static List<char> equal = CAG.ToList<char>(AllChars.equals);
}
