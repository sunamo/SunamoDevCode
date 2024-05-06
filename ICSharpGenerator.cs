//namespace SunamoDevCode;



//public interface ICSharpGenerator
//{
//    void AddValuesViaAddRange(int tabCount, string timeObjectName, string v, string type, List<string> whereIsUsed2, CSharpGeneratorArgs a);
//    void AddValuesViaAddRange(int tabCount, string timeObjectName, string v, Type type, List<string> whereIsUsed2, CSharpGeneratorArgs a);
//    void AssignValue(int tabCount, string objectName, string variable, string value, CSharpGeneratorArgs a);
//    void Attribute(int tabCount, string name, string attrs);
//    void Ctor(int tabCount, ModifiersConstructor mc, string ctorName, bool autoAssing, bool isBase, params string[] args);
//    void Ctor(int tabCount, ModifiersConstructor mc, string ctorName, string inner, params string[] args);
//    void DictionaryFromDictionary<Key, Value>(int tabCount, string nameDictionary, Dictionary<Key, Value> dict, CSharpGeneratorArgs arg = null);
//    void DictionaryFromDictionaryInnerList<Key, Value>(int tabCount, string nameDictionary, Dictionary<Key, Value> dict, CSharpGeneratorArgs a);
//    void DictionaryFromRandomValue<Key, Value>(int tabCount, string nameDictionary, List<Key> keys, Func<Value> randomValue, CSharpGeneratorArgs a = null);
//    void DictionaryFromTwoList<Key, Value>(int tabCount, string nameDictionary, List<Key> keys, List<Value> values, CSharpGeneratorArgs a = null);
//    void DictionaryNumberNumber<T, U>(int tabCount, string nameDictionary, Dictionary<T, U> nameCommentEnums, CSharpGeneratorArgs a = null);
//    void DictionaryStringListString(int tabCount, string nameDictionary, Dictionary<string, List<string>> result, CSharpGeneratorArgs a = null);
//    void DictionaryStringObject<Value>(int tabCount, string nameDictionary, Dictionary<string, Value> dict, CSharpGeneratorArgs a);
//    void DictionaryStringString(int tabCount, string nameDictionary, Dictionary<string, string> nameCommentEnums, CSharpGeneratorArgs a = null);
//    void Else(int tabCount);
//    void EndRegion(int tabCount);
//    void Enum(int tabCount, AccessModifiers _public, string nameEnum, List<EnumItem> enumItems);
//    void EnumWithComments(int tabCount, AccessModifiers _public, string nameEnum, Dictionary<string, string> nameCommentEnums);
//    void Field(int tabCount, AccessModifiers _public, bool _static, VariableModifiers variableModifiers, string type, string name, bool defaultValue);
//    void Field(int tabCount, AccessModifiers _public, bool _static, VariableModifiers variableModifiers, string type, string name, bool addHyphensToValue, string value);
//    void Field(int tabCount, AccessModifiers _public, bool _static, VariableModifiers variableModifiers, string type, string name, ObjectInitializationOptions oio, string value);
//    void GetDictionaryValuesFromDictionary<Key, Value>(int tabCount, string nameDictionary, Dictionary<Key, Value> dict);
//    void GetDictionaryValuesFromDictionaryInnerList<Key, Value>(int tabCount, string nameDictionary, Dictionary<Key, List<Value>> dict, CSharpGeneratorArgs a);
//    void GetDictionaryValuesFromRandomValue<Key, Value>(int tabCount, string nameDictionary, List<Key> keys, Func<Value> randomValue);
//    void GetDictionaryValuesFromTwoList<Key, Value>(int tabCount, string nameDictionary, List<Key> keys, List<Value> values, CSharpGeneratorArgs a);
//    void If(int tabCount, string podminka);
//    void List(int tabCount, string genericType, string listName, List<string> list, CSharpGeneratorArgs a = null);
//    void Method(int tabCount, AccessModifiers _public, bool _static, string returnType, string name, string inner, string args);
//    void Method(int tabCount, string header, string inner);
//    void Namespace(int tabCount, string ns);
//    void Property(int tabCount, AccessModifiers _public, bool _static, string returnType, string name, object _get, object _set, string field);
//    void Region(int tabCount, string v);
//    void StartClass(int tabCount, AccessModifiers _public, bool _static, string className, params string[] derive);
//    void This(int tabCount, string item);
//    void Using(string usings);
//}
