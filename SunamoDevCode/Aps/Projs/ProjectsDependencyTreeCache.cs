// variables names: ok
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//public class ProjectsDependencyTreeCache
//{
//    /// <summary>
//    /// Must be List, not CollectionWithoutDuplicates to use with AddOrCreate
//    /// </summary>
//    public Dictionary<string, List<string>> c = new Dictionary<string, List<string>>();

//    public static ProjectsDependencyTreeCache Instance = new ProjectsDependencyTreeCache();

//    public ProjectsDependencyTreeCache()
//    {

//    }

//    static object _gbbLs = new object();

//    public List<string> AddOrCreate(string csprojPath)
//    {
//        List<string> result = null;
//        //lock (_gbbLs)
//        //{
//            result = DictionaryHelper.AddOrCreate<string, string>(c, csprojPath, SunamoCsprojHelper.BuildProjectsDependencyTreeList);
//        //}
//        return result;
//    }
//}
