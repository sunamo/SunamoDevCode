namespace SunamoDevCode._sunamo.SunamoCollectionsChangeContent;

/// <summary>
/// Collection content change helper
/// </summary>
internal class CAChangeContent
{
    /// <summary>
    /// Removes null or empty values from list based on arguments
    /// </summary>
    /// <param name="args">Arguments specifying what to remove</param>
    /// <param name="list">List to process</param>
    private static void RemoveNullOrEmpty(ChangeContentArgsDC args, List<string> list)
    {
        if (args != null)
        {
            if (args.RemoveNull)
            {
                list.Remove(null);
            }
            if (args.RemoveEmpty)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    if (list[i].Trim() == string.Empty)
                    {
                        list.RemoveAt(i);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Direct edit - Changes content of list using provided function with 0 additional parameters
    /// If not every element fulfills pattern, it is good to remove null (or values returned if can't be changed) from result
    /// </summary>
    /// <param name="args">Arguments for change operation</param>
    /// <param name="list">List to process</param>
    /// <param name="func">Function to apply to each element</param>
    /// <returns>Modified list</returns>
    internal static List<string> ChangeContent0(ChangeContentArgsDC args, List<string> list, Func<string, string> func)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = func.Invoke(list[i]);
        }
        RemoveNullOrEmpty(args, list);
        return list;
    }

    /// <summary>
    /// Direct edit - Changes content of list using provided function with 1 additional parameter
    /// </summary>
    /// <param name="args">Arguments for change operation</param>
    /// <param name="list">List to process</param>
    /// <param name="func">Function to apply to each element</param>
    /// <param name="argument1">First additional argument</param>
    /// <returns>Modified list</returns>
    internal static List<string> ChangeContent1(ChangeContentArgsDC args, List<string> list, Func<string, string, string> func, string argument1)
    {
        var result = ChangeContent<string>(args, list, func, argument1);
        return result;
    }

    #region Switch first and second argument
    /// <summary>
    /// Changes content with switched argument order
    /// </summary>
    /// <typeparam name="Arg1">Type of first argument</typeparam>
    /// <param name="list">List to process</param>
    /// <param name="func">Function with switched arguments</param>
    /// <param name="argument">Argument to pass</param>
    /// <returns>Modified list</returns>
    internal static List<string> ChangeContentSwitch12<Arg1>(List<string> list, Func<Arg1, string, string> func, Arg1 argument)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = func.Invoke(argument, list[i]);
        }
        return list;
    }

    /// <summary>
    /// Direct edit input collection
    /// Changes content of list using provided function with 1 additional parameter
    /// </summary>
    /// <typeparam name="Arg1">Type of first argument</typeparam>
    /// <param name="args">Arguments for change operation</param>
    /// <param name="list">List to process</param>
    /// <param name="func">Function to apply to each element</param>
    /// <param name="argument">Argument to pass to function</param>
    /// <param name="funcSwitch12">Function with switched argument order (optional)</param>
    /// <returns>Modified list</returns>
    internal static List<string> ChangeContent<Arg1>(ChangeContentArgsDC args, List<string> list, Func<string, Arg1, string> func, Arg1 argument, Func<Arg1, string, string> funcSwitch12 = null)
    {
        if (args == null)
        {
            args = new();
        }
        if (args.SwitchFirstAndSecondArg)
        {
            list = ChangeContentSwitch12<Arg1>(list, funcSwitch12, argument);
        }
        else
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = func.Invoke(list[i], argument);
            }
        }
        RemoveNullOrEmpty(args, list);
        return list;
    }
    #endregion

    #region ChangeContent for easy copy
    /// <summary>
    /// Direct edit - Changes content of list using provided function with 2 additional parameters
    /// </summary>
    /// <typeparam name="Arg1">Type of first argument</typeparam>
    /// <typeparam name="Arg2">Type of second argument</typeparam>
    /// <param name="args">Arguments for change operation</param>
    /// <param name="list">List to process</param>
    /// <param name="func">Function to apply to each element</param>
    /// <param name="argument1">First additional argument</param>
    /// <param name="argument2">Second additional argument</param>
    /// <returns>Modified list</returns>
    internal static List<string> ChangeContent<Arg1, Arg2>(ChangeContentArgsDC args, List<string> list, Func<string, Arg1, Arg2, string> func, Arg1 argument1, Arg2 argument2)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = func.Invoke(list[i], argument1, argument2);
        }
        RemoveNullOrEmpty(args, list);
        return list;
    }
    #endregion
}
