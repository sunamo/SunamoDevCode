// variables names: ok
namespace SunamoDevCode;

public partial class CSharpGenerator : GeneratorCodeAbstract //, ICSharpGenerator
{
    /// <summary>
    /// EN: Generates a constructor with automatic field assignment. Parameters are stored in pairs: type, name, type, name. Static constructors cannot be created here.
    /// CZ: Generuje konstruktor s automatickým přiřazením polí. Parametry jsou uloženy v párech: typ, název, typ, název. Statický konstruktor zde nelze vytvořit.
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="modifierType">Constructor modifier (public, private, etc.)</param>
    /// <param name="constructorName">Name of the constructor (should match class name)</param>
    /// <param name="autoAssign">Whether to automatically assign parameters to fields</param>
    /// <param name="isBase">Whether this is a base class constructor</param>
    /// <param name="parameters">Alternating type and name pairs for parameters</param>
    public void Ctor(int tabCount, ModifiersConstructor modifierType, string constructorName, bool autoAssign, bool isBase, params string[] parameters)
    {
        AddTab(tabCount);
        var modifierBuilder = new StringBuilder(modifierType.ToString());
        modifierBuilder[0] = char.ToLower(modifierBuilder[0]);
        sb.AddItem(modifierBuilder.ToString());
        sb.AddItem(constructorName);
        StartParenthesis();
        var parameterNames = new List<string>(parameters.Length / 2);
        for (var i = 0; i < parameters.Length; i++)
        {
            sb.AddItem(parameters[i]);
            var parameterName = parameters[++i];
            parameterNames.Add(parameterName);
            if (i != parameters.Length - 1)
                sb.AddItem(parameterName + ",");
            else
                sb.AddItem(parameterName);
        }

        EndParenthesis();
        if (!isBase)
            if (parameterNames.Count > 0)
                sb.AddItem(": base(" + string.Join(',', parameterNames.ToArray()) + ")");
        StartBrace(tabCount);
        if (autoAssign && isBase)
            foreach (var item in parameterNames)
            {
                This(tabCount, item);
                sb.AddItem("=");
                sb.AddItem(item + ";");
                sb.AppendLine();
            }

        EndBrace(tabCount);
        sb.AppendLine();
    }

    /// <summary>
    /// EN: Generates a property with getter and setter. The get and set parameters can be string (custom implementation) or bool (auto-implementation).
    /// CZ: Generuje vlastnost s getterem a setterem. Parametry get a set mohou být string (vlastní implementace) nebo bool (auto-implementace).
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="accessModifier">Access modifier for the property</param>
    /// <param name="isStatic">Whether the property is static</param>
    /// <param name="returnType">Type of the property</param>
    /// <param name="name">Name of the property</param>
    /// <param name="getImplementation">Getter implementation: true for auto-implementation, string for custom code, null/false for none</param>
    /// <param name="setImplementation">Setter implementation: true for auto-implementation, string for custom code, null/false for none</param>
    /// <param name="field">Backing field name</param>
    /// <param name="shortGet">Whether to use short getter syntax (get;)</param>
    /// <param name="shortSet">Whether to use short setter syntax (set;)</param>
    public void Property(int tabCount, AccessModifiers accessModifier, bool isStatic, string returnType, string name, object getImplementation, object setImplementation, string field, bool shortGet, bool shortSet)
    {
#region MyRegion
        AddTab(tabCount);
        PublicStatic(accessModifier, isStatic);
#endregion
        ReturnTypeName(returnType, name);
        AddTab(tabCount);
        if (shortGet && shortSet)
            sb.AddItem("{");
        else
            StartBrace(tabCount);
        var hasGetterImplementation = !(getImplementation == null || getImplementation.ToString() == false.ToString());
        if (hasGetterImplementation)
        {
            if (shortGet)
                throw new Exception("Can't be set shortGet and getImplementation in one time");
            var getterCode = getImplementation.ToString();
            AddTab(tabCount + 1);
            sb.AddItem("get");
            StartBrace(tabCount + 1);
            AddTab(tabCount + 2);
            if (getterCode == true.ToString())
                sb.AddItem("return " + field + ";");
            else
                sb.AddItem(getterCode);
            sb.AppendLine();
            EndBrace(tabCount + 1);
        }

        var hasSetterImplementation = !(setImplementation == null || setImplementation.ToString() == false.ToString());
        if (hasSetterImplementation)
        {
            if (shortSet)
                throw new Exception("Can't be set shortSet and setImplementation in one time");
            AddTab(tabCount + 1);
            sb.AddItem("set");
            StartBrace(tabCount + 1);
            AddTab(tabCount + 2);
            var setterCode = setImplementation.ToString();
            if (setterCode == true.ToString())
                sb.AddItem(field + " = value;");
            else
                sb.AddItem(setterCode);
            sb.AppendLine();
            EndBrace(tabCount + 1);
        }

        if (shortGet)
        {
            if (hasSetterImplementation)
                throw new Exception("Can't be set shortGet and getImplementation in one time");
            sb.AddItem("get;");
        }

        if (shortSet)
        {
            if (hasGetterImplementation)
                throw new Exception("Can't be set shortGet and getImplementation in one time");
            sb.AddItem("set;");
        }

        EndBrace(tabCount);
        sb.AppendLine();
    }

    /// <summary>
    /// EN: Generates a method declaration with body. The inner content must already be indented for this method.
    /// CZ: Generuje deklaraci metody s tělem. Vnitřní obsah již musí být odsazený pro tuto metodu.
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="accessModifier">Access modifier for the method</param>
    /// <param name="isStatic">Whether the method is static</param>
    /// <param name="returnType">Return type of the method</param>
    /// <param name="name">Name of the method</param>
    /// <param name="bodyContent">Body content of the method (already indented)</param>
    /// <param name="parametersDeclaration">Method parameters declaration</param>
    public void Method(int tabCount, AccessModifiers accessModifier, bool isStatic, string returnType, string name, string bodyContent, string parametersDeclaration)
    {
        AddTab(tabCount);
        PublicStatic(accessModifier, isStatic);
        ReturnTypeName(returnType, name);
        StartParenthesis();
        sb.AddItem(parametersDeclaration);
        EndParenthesis();
        AppendLine();
        StartBrace(tabCount);
        AddTab(tabCount + 1);
        sb.AddItem(bodyContent);
        sb.AppendLine();
        EndBrace(tabCount);
        sb.AppendLine();
    }

    private void ReturnTypeName(string returnType, string name)
    {
        sb.AddItem(returnType);
        sb.AddItem(name);
    }

    /// <summary>
    /// EN: Generates a method with pre-formatted header and body content
    /// CZ: Generuje metodu s předformátovanou hlavičkou a obsahem těla
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="header">Pre-formatted method header</param>
    /// <param name="bodyContent">Body content of the method</param>
    public void Method(int tabCount, string header, string bodyContent)
    {
        AddTab(tabCount);
        sb.AddItem(header);
        StartBrace(tabCount);
        //AddTab(tabCount + 1);
        sb.AddItem(bodyContent);
        sb.AppendLine("");
        EndBrace(tabCount);
        sb.AppendLine();
    }

    /// <summary>
    /// EN: Generates a using directive, automatically adding 'using' keyword and semicolon if needed
    /// CZ: Generuje using direktivu, automaticky přidává klíčové slovo 'using' a středník pokud chybí
    /// </summary>
    /// <param name="usingStatement">Using statement (with or without 'using' keyword and semicolon)</param>
    public void Using(string usingStatement)
    {
        if (!usingStatement.StartsWith("using "))
            usingStatement = "using " + usingStatement + ";";
        else if (!usingStatement.Trim().EndsWith(";"))
            usingStatement += ";";
        sb.AddItem(usingStatement);
        sb.AppendLine();
    }

    /// <summary>
    /// EN: Generates an if statement with opening brace. Automatically adds the opening brace.
    /// CZ: Generuje if příkaz s otevírací závorkou. Automaticky přidává počáteční závorku.
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="condition">Condition for the if statement</param>
    public void If(int tabCount, string condition)
    {
        AddTab(tabCount);
        sb.AppendLine("if(" + condition + ")");
        StartBrace(tabCount);
    }

    /// <summary>
    /// EN: Generates an else statement with opening brace. Automatically adds the opening brace.
    /// CZ: Generuje else příkaz s otevírací závorkou. Automaticky přidává počáteční závorku.
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    public void Else(int tabCount)
    {
        AddTab(tabCount);
        sb.AppendLine("else");
        StartBrace(tabCount);
    }

    /// <summary>
    /// EN: Generates an enum with XML summary comments for each member
    /// CZ: Generuje enum s XML summary komentáři pro každý člen
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="accessModifier">Access modifier for the enum</param>
    /// <param name="enumName">Name of the enum</param>
    /// <param name="memberComments">Dictionary mapping enum member names to their comments</param>
    public void EnumWithComments(int tabCount, AccessModifiers accessModifier, string enumName, Dictionary<string, string> memberComments)
    {
        WriteAccessModifiers(accessModifier);
        AddTab(tabCount);
        sb.AddItem("enum " + enumName);
        StartBrace(tabCount);
        foreach (var item in memberComments)
        {
            XmlSummary(tabCount + 1, item.Value);
            AppendLine(tabCount + 1, item.Key + ",");
        }

        EndBrace(tabCount);
    }

    /// <summary>
    /// EN: Appends an attribute with optional parameters
    /// CZ: Přidává atribut s volitelnými parametry
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="attributeName">Name of the attribute</param>
    /// <param name="attributeParameters">Content inside parentheses (can be null)</param>
    private void AppendAttribute(int tabCount, string attributeName, string attributeParameters)
    {
        var parentheses = "";
        if (attributeParameters != null)
            parentheses = "(" + attributeParameters + ")";
        AppendLine(tabCount, "[" + attributeName + parentheses + "]");
    }
}