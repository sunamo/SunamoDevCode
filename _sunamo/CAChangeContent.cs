using SunamoArgs;

namespace SunamoDevCode._sunamo;
internal class CAChangeContent
{
    internal static List<string> ChangeContent<Arg1>(ChangeContentArgs a, List<string> files_in, Func<string, Arg1, string> func, Arg1 arg, Func<Arg1, string, string> funcSwitch12 = null)
    {
        if (a == null)
        {
            a = new ChangeContentArgs();
        }

        if (a.switchFirstAndSecondArg)
        {
            files_in = ChangeContentSwitch12<Arg1>(files_in, funcSwitch12, arg);
        }
        else
        {
            for (int i = 0; i < files_in.Count; i++)
            {
                files_in[i] = func.Invoke(files_in[i], arg);
            }
        }


        RemoveNullOrEmpty(a, files_in);

        return files_in;
    }

    internal static List<string> ChangeContentSwitch12<Arg1>(List<string> files_in, Func<Arg1, string, string> func, Arg1 arg)
    {
        for (int i = 0; i < files_in.Count; i++)
        {
            files_in[i] = func.Invoke(arg, files_in[i]);
        }
        return files_in;
    }

    private static List<TResult> ChangeContent<T1, TResult>(List<T1> files_in, Func<T1, TResult> func)
    {
        List<TResult> result = new List<TResult>(files_in.Count);
        for (int i = 0; i < files_in.Count; i++)
        {
            result.Add(func.Invoke(files_in[i]));
        }
        return result;
    }

    private static List<TResult> ChangeContent<T1, T2, TResult>(ChangeContentArgs a, Func<T1, T2, TResult> func, List<T1> files_in, T2 t2)
    {
        List<TResult> result = new List<TResult>(files_in.Count);
        for (int i = 0; i < files_in.Count; i++)
        {
            // Fully generic - no strict string can't return the same collection
            result.Add(func.Invoke(files_in[i], t2));
        }

        //CA.RemoveDefaultT<TResult>(result);
        return result;
    }

    internal static List<string> ChangeContent<Arg1, Arg2>(ChangeContentArgs a, List<string> files_in, Func<string, Arg1, Arg2, string> func, Arg1 arg1, Arg2 arg2)
    {
        for (int i = 0; i < files_in.Count; i++)
        {
            files_in[i] = func.Invoke(files_in[i], arg1, arg2);
        }

        RemoveNullOrEmpty(a, files_in);

        return files_in;
    }

    private static void RemoveNullOrEmpty(ChangeContentArgs a, List<string> files_in)
    {
        if (a != null)
        {
            if (a.removeNull)
            {
                files_in.Remove(null);
            }

            if (a.removeEmpty)
            {
                for (int i = files_in.Count - 1; i >= 0; i--)
                {
                    if (files_in[i].Trim() == string.Empty)
                    {
                        files_in.RemoveAt(i);
                    }
                }
            }
        }
    }

    internal static List<string> ChangeContent0(ChangeContentArgs a, List<string> files_in, Func<string, string> func)
    {
        for (int i = 0; i < files_in.Count; i++)
        {
            files_in[i] = func.Invoke(files_in[i]);
        }

        RemoveNullOrEmpty(a, files_in);

        return files_in;
    }
}
