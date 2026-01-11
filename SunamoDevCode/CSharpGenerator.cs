// variables names: ok
namespace SunamoDevCode;

public partial class CSharpGenerator : GeneratorCodeAbstract //, ICSharpGenerator
{
    /// <summary>
    /// EN: Generates the start of a class declaration with optional inheritance
    /// CZ: Generuje začátek deklarace třídy s volitelným děděním
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="accessModifier">Access modifier for the class</param>
    /// <param name="isStatic">Whether the class should be static</param>
    /// <param name="className">Name of the class</param>
    /// <param name="derivedTypes">Base types or interfaces to derive from</param>
    public void StartClass(int tabCount, AccessModifiers accessModifier, bool isStatic, string className, params string[] derivedTypes)
    {
        AddTab(tabCount);
        PublicStatic(accessModifier, isStatic);
        sb.AddItem(" class " + className);
        if (derivedTypes.Length != 0)
        {
            sb.AddItem(":");
            for (var i = 0; i < derivedTypes.Length - 1; i++)
                sb.AddItem(derivedTypes[i] + ",");
            sb.AddItem(derivedTypes[derivedTypes.Length - 1]);
        }

        StartBrace(tabCount);
    }

    /// <summary>
    /// EN: Writes public/protected/internal and static modifiers to the output
    /// CZ: Zapíše public/protected/internal a static modifikátory do výstupu
    /// </summary>
    /// <param name="accessModifier">Access modifier to write</param>
    /// <param name="isStatic">Whether to add static modifier</param>
    private void PublicStatic(AccessModifiers accessModifier, bool isStatic)
    {
        WriteAccessModifiers(accessModifier);
        if (isStatic)
            sb.AddItem("static");
    }

    /// <summary>
    /// EN: Writes the access modifier keyword to the output
    /// CZ: Zapíše klíčové slovo modifikátoru přístupu do výstupu
    /// </summary>
    /// <param name="accessModifier">Access modifier to write</param>
    private void WriteAccessModifiers(AccessModifiers accessModifier)
    {
        if (accessModifier == AccessModifiers.Public)
        {
            sb.AddItem("public");
        }
        else if (accessModifier == AccessModifiers.Protected)
        {
            sb.AddItem("protected");
        }
        else if (accessModifier == AccessModifiers.Private)
        {
            // Private is default - no keyword needed
        }
        else if (accessModifier == AccessModifiers.Internal)
        {
            sb.AddItem("public");
        }
        else
        {
            ThrowEx.NotImplementedCase(accessModifier);
        }
    }

    /// <summary>
    /// EN: Writes the end of a region directive
    /// CZ: Zapíše konec region direktivy
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    public void EndRegion(int tabCount)
    {
        AppendLine(tabCount, "#endregion");
    }

    /// <summary>
    /// EN: Writes the start of a region directive with a name
    /// CZ: Zapíše začátek region direktivy se jménem
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="regionName">Name of the region</param>
    public void Region(int tabCount, string regionName)
    {
        AppendLine(tabCount, "#region " + regionName);
    }

    /// <summary>
    /// EN: Generates an attribute with parameters
    /// CZ: Generuje atribut s parametry
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="attributeName">Name of the attribute</param>
    /// <param name="attributeParameters">Parameters for the attribute</param>
    public void Attribute(int tabCount, string attributeName, string attributeParameters)
    {
        AddTab(tabCount);
        sb.AppendLine("[" + attributeName + "(" + attributeParameters + ")]");
    }

    /// <summary>
    /// EN: Creates a list of EnumItem objects from a list of strings
    /// CZ: Vytvoří seznam EnumItem objektů ze seznamu stringů
    /// </summary>
    /// <param name="list">List of enum value names</param>
    /// <returns>List of EnumItem objects</returns>
    public static List<EnumItem> CreateEnumItemsFromList(List<string> list)
    {
        var enumItems = new List<EnumItem>(list.Count);
        foreach (var item in list)
            enumItems.Add(new EnumItem { Name = item });
        return enumItems;
    }

    /// <summary>
    /// EN: Generates a field declaration with optional quotation of the value
    /// CZ: Generuje deklaraci pole s volitelným přidáním uvozovek k hodnotě
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="accessModifier">Access modifier for the field</param>
    /// <param name="isStatic">Whether the field should be static</param>
    /// <param name="variableModifiers">Variable modifiers (const, readonly, etc.)</param>
    /// <param name="type">Type of the field</param>
    /// <param name="name">Name of the field</param>
    /// <param name="isAddingHyphensToValue">Whether to add quotes around the value</param>
    /// <param name="value">Initial value of the field</param>
    public void Field(int tabCount, AccessModifiers accessModifier, bool isStatic, VariableModifiers variableModifiers, string type, string name, bool isAddingHyphensToValue, string value)
    {
        var initializationOption = ObjectInitializationOptions.Original;
        if (isAddingHyphensToValue)
            initializationOption = ObjectInitializationOptions.Hyphens;
        Field(tabCount, accessModifier, isStatic, variableModifiers, type, name, initializationOption, value);
    }

    /// <summary>
    /// EN: Generates a field declaration with initialization
    /// CZ: Generuje deklaraci pole s inicializací
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="accessModifier">Access modifier for the field. Private modifier is omitted (default).</param>
    /// <param name="isStatic">Whether the field should be static. Ignored if initializationOption is NewAssign.</param>
    /// <param name="variableModifiers">Variable modifiers (const, readonly, etc.)</param>
    /// <param name="type">Type of the field</param>
    /// <param name="name">Name of the field</param>
    /// <param name="initializationOption">How to initialize the field: Original (use value as-is), Hyphens (add quotes), or NewAssign (use new constructor)</param>
    /// <param name="value">Initial value. If initializationOption is NewAssign, use string.Empty for empty constructor. If Original, put the part after =. Cannot be null.</param>
    public void Field(int tabCount, AccessModifiers accessModifier, bool isStatic, VariableModifiers variableModifiers, string type, string name, ObjectInitializationOptions initializationOption, string value)
    {
        AddTab(tabCount);
        ModifiersField(accessModifier, isStatic, variableModifiers);
        ReturnTypeName(type, name);
        sb.AddItem("=");
        if (initializationOption == ObjectInitializationOptions.Hyphens)
            value = "\"" + value + "\"";
        else if (initializationOption == ObjectInitializationOptions.NewAssign)
            value = "new " + type + "()";
        var statement = value + ";";
        sb.AddItem(statement);
        sb.AppendLine();
    }

    /// <summary>
    /// EN: Generates a field declaration with optional default value
    /// CZ: Generuje deklaraci pole s volitelnou výchozí hodnotou
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="accessModifier">Access modifier for the field</param>
    /// <param name="isStatic">Whether the field should be static</param>
    /// <param name="variableModifiers">Variable modifiers (const, readonly, etc.)</param>
    /// <param name="type">Type of the field</param>
    /// <param name="name">Name of the field</param>
    /// <param name="isUsingDefaultValue">Whether to add a default value for the type</param>
    public void Field(int tabCount, AccessModifiers accessModifier, bool isStatic, VariableModifiers variableModifiers, string type, string name, bool isUsingDefaultValue)
    {
        AddTab(tabCount);
        ModifiersField(accessModifier, isStatic, variableModifiers);
        ReturnTypeName(type, name);
        DefaultValue(type, isUsingDefaultValue);
        sb.RemoveEndDelimiter();
        sb.AddItem(";");
        sb.AppendLine();
    }

    /// <summary>
    /// EN: Adds a default value for the specified type (uses CSharpHelper.DefaultValueForType)
    /// CZ: Přidá výchozí hodnotu pro zadaný typ (používá CSharpHelper.DefaultValueForType)
    /// </summary>
    /// <param name="type">Type to get default value for</param>
    /// <param name="isUsingDefaultValue">Whether to add the default value</param>
    private void DefaultValue(string type, bool isUsingDefaultValue)
    {
        if (isUsingDefaultValue)
        {
            sb.AddItem("=");
            sb.AddItem(CSharpHelperSunamo.DefaultValueForType(type, ConvertTypeShortcutFullName.ToShortcut));
        }
    }

    /// <summary>
    /// EN: Inserts lines into a class before the closing brace
    /// CZ: Vloží řádky do třídy před uzavírací složenou závorku
    /// </summary>
    /// <param name="lines">File content as list of lines</param>
    /// <param name="insertedLines">Lines to insert into the class</param>
    /// <param name="classIndex">Output: index of the class declaration line</param>
    /// <param name="namespaceName">Namespace name to process</param>
    /// <returns>Modified file content</returns>
    public static List<string> AddIntoClass(List<string> lines, List<string> insertedLines, out int classIndex, string namespaceName)
    {
        classIndex = -1;
        var foundClass = false;
        var foundOpeningBrace = false;

        for (var i = 0; i < lines.Count; i++)
        {
            if (!foundClass)
            {
                if (lines[i].Contains(" class "))
                {
                    lines[i] = lines[i].Replace(namespaceName + "Page", "Page");
                    classIndex = i;
                    foundClass = true;
                }
            }
            else if (foundClass && !foundOpeningBrace)
            {
                if (lines[i].Contains("{"))
                    foundOpeningBrace = true;
            }
            else if (foundClass && foundOpeningBrace)
            {
                if (lines[i].Contains("}"))
                {
                    lines.InsertRange(i, insertedLines);
                    break;
                }
            }
        }

        return lines;
    }

    /// <summary>
    /// EN: Generates a namespace declaration
    /// CZ: Generuje deklaraci namespace
    /// </summary>
    /// <param name="namespaceName">Name of the namespace</param>
    public void Namespace(string namespaceName)
    {
        sb.AddItem("namespace" + " " + namespaceName);
        sb.AppendLine();
        sb.AddItem("{");
        sb.AppendLine();
    }

    /// <summary>
    /// EN: Writes field modifiers (access, static, const/readonly) to the output
    /// CZ: Zapíše modifikátory pole (přístup, static, const/readonly) do výstupu
    /// </summary>
    /// <param name="accessModifier">Access modifier for the field</param>
    /// <param name="isStatic">Whether the field is static</param>
    /// <param name="variableModifiers">Variable modifiers (const, readonly, etc.)</param>
    private void ModifiersField(AccessModifiers accessModifier, bool isStatic, VariableModifiers variableModifiers)
    {
        WriteAccessModifiers(accessModifier);
        if (variableModifiers == VariableModifiers.Mapped)
        {
            sb.AddItem("const");
        }
        else
        {
            if (isStatic && variableModifiers == VariableModifiers.ReadOnly)
            {
                sb.AddItem("const");
            }
            else
            {
                if (isStatic)
                    sb.AddItem("static");
                if (variableModifiers == VariableModifiers.ReadOnly)
                    sb.AddItem("readonly");
            }
        }
    }

    /// <summary>
    /// EN: Generates a constructor declaration with parameters
    /// CZ: Generuje deklaraci konstruktoru s parametry
    /// </summary>
    /// <param name="tabCount">Number of tabs for indentation</param>
    /// <param name="modifierType">Constructor modifier (public, private, etc.)</param>
    /// <param name="constructorName">Name of the constructor (should match class name)</param>
    /// <param name="bodyContent">Content of the constructor body</param>
    /// <param name="parameters">Alternating type and name pairs for parameters (type1, name1, type2, name2, ...)</param>
    public void Ctor(int tabCount, ModifiersConstructor modifierType, string constructorName, string bodyContent, params string[] parameters)
    {
        AddTab(tabCount);
        var modifierStringBuilder = new StringBuilder(modifierType.ToString());
        modifierStringBuilder[0] = char.ToLower(modifierStringBuilder[0]);
        sb.AddItem(modifierStringBuilder.ToString());
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
        StartBrace(tabCount);
        Append(tabCount + 1, bodyContent);
        EndBrace(tabCount - 2);
        sb.AppendLine();
    }
}