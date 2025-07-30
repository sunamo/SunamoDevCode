namespace SunamoDevCode;

using Microsoft.Extensions.Logging;

/// <summary>
/// Toto je sice z CommandsToAllCsprojs.Shared ale hodí se na to všechny CommandsToAll*
/// Nepřidávám to tam jako soubor ale jako celý csproj
/// </summary>
public class CsprojShared
{
    /// <summary>
    /// True for sln
    /// False for csproj
    /// Null if neither or there is both sln/csproj
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static bool? DetectIsSlnOrCsprojFolder(ILogger logger, string path, WhatIsExcepted whatIsExcepted)
    {
        if (path == null) return null;

        if (string.IsNullOrEmpty(path)) return null;

        var slnPath = GetSlnForFolder(logger, path, false);
        var csprojPath = GetCsprojForFolder(logger, path, false);

        var isSln = slnPath != null;
        var isCsproj = csprojPath != null;

        if (isSln && isCsproj)
        {
            if (whatIsExcepted == WhatIsExcepted.Csproj)
            {
                try
                {
                    File.Delete(slnPath);
                    // is csproj
                    return false;
                }
                catch (Exception ex)
                {
                    logger.LogWarning($"{slnPath} cannot be deleted: " + Exceptions.TextOfExceptions(ex));
                }
            }
            else if (whatIsExcepted == WhatIsExcepted.Sln)
            {
                try
                {
                    File.Delete(csprojPath);
                    // is sln
                    return true;
                }
                catch (Exception ex)
                {
                    logger.LogWarning($"{csprojPath} cannot be deleted: " + Exceptions.TextOfExceptions(ex));
                }
            }
            else if (whatIsExcepted == WhatIsExcepted.Both)
            {
                return null;
            }
            else
            {
                ThrowEx.NotImplementedCase(whatIsExcepted);
            }
        }

        if (isSln) return true;

        if (isCsproj) return false;

        return null;
    }

    public static string GetCsprojForFolder(ILogger logger, string path, bool throwExOrReturnNull)
    {
        return GetExtForFolder(logger, path, throwExOrReturnNull, "csproj");
    }

    public static string GetSlnForFolder(ILogger logger, string path, bool throwExOrReturnNull)
    {
        var result = GetExtForFolder(logger, path, throwExOrReturnNull, "sln");
        // VS sometimes generate sln in folder of project
        if (result != null && result.EndsWith(".generated.sln"))
        {
            try
            {
                File.Delete(result);
            }
            catch (Exception)
            {
            }
            return null;
        }
        return result;
    }

    public static string GetExtForFolder(ILogger logger, string path, bool throwExOrReturnNull, string ext)
    {
        ext = ext.TrimStart('.');

        var sln = FSGetFiles.GetFilesEveryFolder(logger, path, "*." + ext, SearchOption.TopDirectoryOnly);

        if (sln.Count > 1)
        {
            if (throwExOrReturnNull)
            {
                throw new Exception("More than 1 ." + ext);
            }
            else
            {
                return null;
            }
        }
        else if (sln.Count == 0)
        {
            if (throwExOrReturnNull)
            {
                throw new Exception("No " + ext);
            }
            else
            {
                return null;
            }
        }

        return sln[0];
    }


    public static bool HasSlnFileInUpFolder(ILogger logger, string csprojPath, out string slnFolder)
    {
        var projectFolder = Path.GetDirectoryName(csprojPath);
        slnFolder = Path.GetDirectoryName(projectFolder);

        var slnFiles = FSGetFiles.GetFilesEveryFolder(logger, slnFolder, "*.sln", SearchOption.AllDirectories);

        return slnFiles.Count() > 0;
    }

    //public static List<string> GetCsprojs(bool onlyInSwld)
    //{
    //    var csprojs = 

    //    if (onlyInSwld)
    //    {
    //        return FSGetFiles.GetFilesEveryFolder(logger, @"E:\vs\Projects\PlatformIndependentNuGetPackages", "*.csproj", SearchOption.AllDirectories).ToList();
    //    }

    //    return csprojs;
    //}
}