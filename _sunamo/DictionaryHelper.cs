namespace SunamoDevCode._sunamo;

internal class DictionaryHelper
{
    internal static void AppendLineOrCreate<T>(Dictionary<T, StringBuilder> sb, T n, string item)
    {
        if (sb.ContainsKey(n))
        {
            sb[n].AppendLine(item);
        }
        else
        {
            var sb2 = new StringBuilder();
            sb2.AppendLine(item);
            sb.Add(n, sb2);
        }
    }
    internal static IList<string> GetIfExists(Dictionary<string, List<string>> filesInSolutionReal, string prefix,
       string v, bool postfixWithA2)
    {
        if (filesInSolutionReal.ContainsKey(v))
        {
            var r = filesInSolutionReal[v];
            if (postfixWithA2)
            {
                if (!string.IsNullOrEmpty(v)) r = CA.PostfixIfNotEnding(v, r);
                CA.Prepend(prefix, r);
            }

            return r;
        }

        return new List<string>();
    }
    internal static void AddOrCreateIfDontExists<Key, Value>(Dictionary<Key, List<Value>> sl, Key key, Value value)
    {
        if (sl.ContainsKey(key))
        {
            if (!sl[key].Contains(value))
            {
                sl[key].Add(value);
            }
        }
        else
        {
            List<Value> ad = new List<Value>();
            ad.Add(value);
            sl.Add(key, ad);
        }
    }

    internal static void AddOrSet<T1, T2>(IDictionary<T1, T2> qs, T1 k, T2 v)
    {
        if (qs.ContainsKey(k))
        {
            qs[k] = v;
        }
        else
        {
            qs.Add(k, v);
        }
    }

    #region AddOrCreate když key i value není list
    /// <summary>
    ///     A3 is inner type of collection entries
    ///     dictS => is comparing with string
    ///     As inner must be List, not IList etc.
    ///     From outside is not possible as inner use other class based on IList
    /// </summary>
    /// <typeparam name="Key"></typeparam>
    /// <typeparam name="Value"></typeparam>
    /// <typeparam name="ColType"></typeparam>
    /// <param name="sl"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    internal static void AddOrCreate<Key, Value, ColType>(IDictionary<Key, List<Value>> dict, Key key, Value value,
    bool withoutDuplicitiesInValue = false, Dictionary<Key, List<string>> dictS = null)
    {
        var compWithString = false;
        if (dictS != null) compWithString = true;

        if (key is IList && typeof(ColType) != typeof(Object))
        {
            var keyE = key as IList<ColType>;
            var contains = false;
            foreach (var item in dict)
            {
                var keyD = item.Key as IList<ColType>;
                if (keyD.SequenceEqual(keyE)) contains = true;
            }

            if (contains)
            {
                foreach (var item in dict)
                {
                    var keyD = item.Key as IList<ColType>;
                    if (keyD.SequenceEqual(keyE))
                    {
                        if (withoutDuplicitiesInValue)
                            if (item.Value.Contains(value))
                                return;
                        item.Value.Add(value);
                    }
                }
            }
            else
            {
                List<Value> ad = new();
                ad.Add(value);
                dict.Add(key, ad);

                if (compWithString)
                {
                    List<string> ad2 = new();
                    ad2.Add(value.ToString());
                    dictS.Add(key, ad2);
                }
            }
        }
        else
        {
            var add = true;
            lock (dict)
            {
                if (dict.ContainsKey(key))
                {
                    if (withoutDuplicitiesInValue)
                    {
                        if (dict[key].Contains(value))
                            add = false;
                        else if (compWithString)
                            if (dictS[key].Contains(value.ToString()))
                                add = false;
                    }

                    if (add)
                    {
                        var val = dict[key];

                        if (val != null) val.Add(value);

                        if (compWithString)
                        {
                            var val2 = dictS[key];

                            if (val != null) val2.Add(value.ToString());
                        }
                    }
                }
                else
                {
                    if (!dict.ContainsKey(key))
                    {
                        List<Value> ad = new();
                        ad.Add(value);
                        dict.Add(key, ad);
                    }
                    else
                    {
                        dict[key].Add(value);
                    }

                    if (compWithString)
                    {
                        if (!dictS.ContainsKey(key))
                        {
                            List<string> ad2 = new();
                            ad2.Add(value.ToString());
                            dictS.Add(key, ad2);
                        }
                        else
                        {
                            dictS[key].Add(value.ToString());
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    ///     Pokud A1 bude obsahovat skupinu pod názvem A2, vložím do této skupiny prvek A3
    ///     Jinak do A1 vytvořím novou skupinu s klíčem A2 s hodnotou A3
    /// </summary>
    /// <typeparam name="Key"></typeparam>
    /// <typeparam name="Value"></typeparam>
    /// <param name="sl"></param>
    /// <param name="key"></param>
    /// <param name="p"></param>
    internal static void AddOrCreate<Key, Value>(IDictionary<Key, List<Value>> sl, Key key, Value value,
    bool withoutDuplicitiesInValue = false, Dictionary<Key, List<string>> dictS = null)
    {
        AddOrCreate<Key, Value, object>(sl, key, value, withoutDuplicitiesInValue, dictS);
    }
    #endregion

    internal static Dictionary<Key, Value> GetDictionary<Key, Value>(List<Key> keys, List<Value> values)
    {
        ThrowEx.DifferentCountInLists("keys", keys.Count, "values", values.Count);
        Dictionary<Key, Value> result = new Dictionary<Key, Value>();
        for (int i = 0; i < keys.Count; i++)
        {
            result.Add(keys[i], values[i]);
        }
        return result;
    }

    internal static Value GetFirstItemValue<Key, Value>(Dictionary<Key, Value> dict)
    {
        foreach (var item in dict)
        {
            return item.Value;
        }

        return default(Value);
    }
}