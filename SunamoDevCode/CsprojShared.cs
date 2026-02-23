namespace SunamoDevCode;

using Microsoft.Extensions.Logging;

/// <summary>
/// EN: Although this is from CommandsToAllCsprojs.Shared, it is useful for all CommandsToAll* projects
/// EN: Not adding it there as a single file but as an entire csproj
/// CZ: Toto je sice z CommandsToAllCsprojs.Shared ale hodí se na to všechny CommandsToAll*
/// CZ: Nepřidávám to tam jako soubor ale jako celý csproj
/// </summary>
public class CsprojShared
{
    /// <summary>
    /// Detects whether the folder contains a solution or csproj file. Returns true for sln, false for csproj, null for neither or both.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="path">Folder path to check.</param>
    /// <param name="whatIsExcepted">Preference when both sln and csproj exist.</param>
    /// <returns>True for sln, false for csproj, null if neither or ambiguous.</returns>
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
                    File.Delete(slnPath!);
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
                    File.Delete(csprojPath!);
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

    /// <summary>
    /// Gets the single .csproj file in the given folder.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="path">Folder path to search.</param>
    /// <param name="isThrowingExceptionOrReturningNull">If true, throws on not found or multiple; if false, returns null.</param>
    /// <returns>Path to the csproj file, or null if not found.</returns>
    public static string? GetCsprojForFolder(ILogger logger, string path, bool isThrowingExceptionOrReturningNull)
    {
        return GetExtForFolder(logger, path, isThrowingExceptionOrReturningNull, "csproj");
    }

    /// <summary>
    /// Gets the single solution file (.sln, .slnx, .slnj) in the given folder.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="path">Folder path to search.</param>
    /// <param name="isThrowingExceptionOrReturningNull">If true, throws on not found; if false, returns null.</param>
    /// <returns>Path to the solution file, or null if not found.</returns>
    public static string? GetSlnForFolder(ILogger logger, string path, bool isThrowingExceptionOrReturningNull)
    {
        string? result = null;
        foreach (var ext in new[] { "sln", "slnx", "slnj" })
        {
            result = GetExtForFolder(logger, path, false, ext);
            if (result != null) break;
        }

        if (result == null && isThrowingExceptionOrReturningNull)
        {
            throw new Exception("No sln/slnx/slnj");
        }

        // VS sometimes generate sln in folder of project
        if (result != null && Path.GetFileName(result).Contains(".generated."))
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

    /// <summary>
    /// Gets the single file matching the extension in the given folder.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="path">Folder path to search.</param>
    /// <param name="isThrowingExceptionOrReturningNull">If true, throws on not found or multiple; if false, returns null.</param>
    /// <param name="ext">File extension to search for (with or without leading dot).</param>
    /// <returns>Path to the matching file, or null if not found.</returns>
    public static string? GetExtForFolder(ILogger logger, string path, bool isThrowingExceptionOrReturningNull, string ext)
    {
        ext = ext.TrimStart('.');

        var matchingFiles = FSGetFiles.GetFilesEveryFolder(logger, path, "*." + ext, SearchOption.TopDirectoryOnly);

        if (matchingFiles.Count > 1)
        {
            if (isThrowingExceptionOrReturningNull)
            {
                throw new Exception("More than 1 ." + ext);
            }
            else
            {
                return null;
            }
        }
        else if (matchingFiles.Count == 0)
        {
            if (isThrowingExceptionOrReturningNull)
            {
                throw new Exception("No " + ext);
            }
            else
            {
                return null;
            }
        }

        return matchingFiles[0];
    }


    /// <summary>
    /// Checks whether a solution file exists in the parent folder of the given csproj path.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="csprojPath">Path to the csproj file.</param>
    /// <param name="slnFolder">Output: the parent folder path where solution files were searched.</param>
    /// <returns>True if at least one solution file exists in the parent folder.</returns>
    public static bool HasSlnFileInUpFolder(ILogger logger, string csprojPath, out string slnFolder)
    {
        var projectFolder = Path.GetDirectoryName(csprojPath);
        slnFolder = Path.GetDirectoryName(projectFolder)!;

        var slnFiles = new List<string>();
        foreach (var ext in new[] { "*.sln", "*.slnx", "*.slnj" })
        {
            slnFiles.AddRange(FSGetFiles.GetFilesEveryFolder(logger, slnFolder!, ext, SearchOption.AllDirectories));
        }

        return slnFiles.Count > 0;
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