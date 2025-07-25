namespace SunamoDevCode._sunamo.SunamoExceptions;

internal partial class ThrowEx
{
    internal static bool FolderCantBeRemoved(string folder)
    { return ThrowIsNotNull(Exceptions.FolderCantBeRemoved(FullNameOfExecutedCode(), folder)); }
    internal static bool IsEmpty(IEnumerable folders, string colName, string additionalMessage = "")
    { return ThrowIsNotNull(Exceptions.IsEmpty(FullNameOfExecutedCode(), folders, colName, additionalMessage)); }

    internal static void UseNonDummyCollection()
    {
        throw new NotImplementedException();
    }
    internal static bool HasOddNumberOfElements(string listName, ICollection list)
    {
        var f = Exceptions.HasOddNumberOfElements;
        return ThrowIsNotNull(f, listName, list);
    }

    internal static bool Custom(string message, bool reallyThrow = true, string secondMessage = "")
    {
        string joined = string.Join(" ", message, secondMessage);
        string? str = Exceptions.Custom(FullNameOfExecutedCode(), joined);
        return ThrowIsNotNull(str, reallyThrow);
    }
    internal static bool DifferentCountInLists<T>(string namefc, IList<T> countfc, string namesc, IList<T> countsc)
    {
        return ThrowIsNotNull(
            Exceptions.DifferentCountInLists(FullNameOfExecutedCode(), namefc, countfc.Count, namesc, countsc.Count));
    }
    internal static bool DifferentCountInLists(string namefc, int countfc, string namesc, int countsc)
    {
        return ThrowIsNotNull(
            Exceptions.DifferentCountInLists(FullNameOfExecutedCode(), namefc, countfc, namesc, countsc));
    }

    internal static bool DifferentCountInListsTU<T, U>(string namefc, IList<T> countfc, string namesc, IList<U> countsc)
    {
        return ThrowIsNotNull(
            Exceptions.DifferentCountInLists(FullNameOfExecutedCode(), namefc, countfc.Count, namesc, countsc.Count));
    }


    internal static bool DirectoryExists(string path)
    { return ThrowIsNotNull(Exceptions.DirectoryExists(FullNameOfExecutedCode(), path)); }
    internal static bool DuplicatedElements(string nameOfVariable, List<string> duplicatedElements, string message = "")
    {
        return ThrowIsNotNull(
            Exceptions.DuplicatedElements(FullNameOfExecutedCode(), nameOfVariable, duplicatedElements, message));
    }

    internal static bool ExcAsArg(Exception ex, string message = "")
    { return ThrowIsNotNull(Exceptions.ExcAsArg, ex, message); }

    internal static bool IsNotAllowed(string what)
    { return ThrowIsNotNull(Exceptions.IsNotAllowed(FullNameOfExecutedCode(), what)); }
    internal static bool IsNull(string variableName, object? variable = null)
    { return ThrowIsNotNull(Exceptions.IsNull(FullNameOfExecutedCode(), variableName, variable)); }

    internal static bool IsNullOrEmpty(string argName, string argValue)
    { return ThrowIsNotNull(Exceptions.IsNullOrWhitespace(FullNameOfExecutedCode(), argName, argValue, true)); }
    internal static bool IsNullOrWhitespace(string argName, string argValue)
    { return ThrowIsNotNull(Exceptions.IsNullOrWhitespace(FullNameOfExecutedCode(), argName, argValue, false)); }


    internal static bool KeyNotFound<T, U>(IDictionary<T, U> en, string dictName, T key)
    { return ThrowIsNotNull(Exceptions.KeyNotFound(FullNameOfExecutedCode(), en, dictName, key)); }
    internal static bool NotContains(string text, params string[] shouldContains)
    { return ThrowIsNotNull(Exceptions.NotContains(FullNameOfExecutedCode(), text, shouldContains)); }

    internal static bool NotImplementedCase(object notImplementedName)
    { return ThrowIsNotNull(Exceptions.NotImplementedCase, notImplementedName); }
    internal static bool NotImplementedMethod() { return ThrowIsNotNull(Exceptions.NotImplementedMethod); }

    #region Other
    internal static string FullNameOfExecutedCode()
    {
        Tuple<string, string, string> placeOfExc = Exceptions.PlaceOfException();
        string f = FullNameOfExecutedCode(placeOfExc.Item1, placeOfExc.Item2, true);
        return f;
    }

    static string FullNameOfExecutedCode(object type, string methodName, bool fromThrowEx = false)
    {
        if (methodName == null)
        {
            int depth = 2;
            if (fromThrowEx)
            {
                depth++;
            }

            methodName = Exceptions.CallingMethod(depth);
        }
        string typeFullName;
        if (type is Type type2)
        {
            typeFullName = type2.FullName ?? "Type cannot be get via type is Type type2";
        }
        else if (type is MethodBase method)
        {
            typeFullName = method.ReflectedType?.FullName ?? "Type cannot be get via type is MethodBase method";
            methodName = method.Name;
        }
        else if (type is string)
        {
            typeFullName = type.ToString() ?? "Type cannot be get via type is string";
        }
        else
        {
            Type t = type.GetType();
            typeFullName = t.FullName ?? "Type cannot be get via type.GetType()";
        }
        return string.Concat(typeFullName, ".", methodName);
    }

    internal static bool ThrowIsNotNull(string? exception, bool reallyThrow = true)
    {
        if (exception != null)
        {
            Debugger.Break();
            if (reallyThrow)
            {
                throw new Exception(exception);
            }
            return true;
        }
        return false;
    }

    #region For avoid FullNameOfExecutedCode

    internal static bool ThrowIsNotNull<A, B>(Func<string, A, B, string?> f, A ex, B message)
    {
        string? exc = f(FullNameOfExecutedCode(), ex, message);
        return ThrowIsNotNull(exc);
    }

    internal static bool ThrowIsNotNull<A>(Func<string, A, string?> f, A ex)
    {
        string? exc = f(FullNameOfExecutedCode(), ex);
        return ThrowIsNotNull(exc);
    }

    internal static bool ThrowIsNotNull(Func<string, string?> f)
    {
        string? exc = f(FullNameOfExecutedCode());
        return ThrowIsNotNull(exc);
    }
    #endregion
    #endregion
}