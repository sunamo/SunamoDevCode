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
    public static List<string> sStartWith = new List<string>(["name:", "inject:", "mixins:", "extends:", "watch:"]);

    public static List<string> dontAddRcub = new List<string>([methodsRcub, propsRcub]);

    //List<string> sContains = new List<string>("data():", "methods:{");
    public static List<string> containsList = new List<string>([cl]);

    public static List<string> startWithList = new List<string>([constructor, import, "//", export]);
    public static List<char> equal = new List<char>([AllChars.equals]);
}