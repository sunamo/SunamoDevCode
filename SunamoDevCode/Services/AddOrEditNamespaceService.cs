namespace SunamoDevCode.Services;

public class AddOrEditNamespaceService
{
    public async Task<string?> AddOrEditNamespaceForSingleFileAndSave(string pathToCsprojFolder, string projectName
        , string csPath, List<string> linesFile = null, string pathToSave = null)
    {
        if (linesFile == null)
        {
            linesFile = (await File.ReadAllLinesAsync(csPath)).ToList();
        }
        if (CSharpHelper.IsEmptyCommentedOrOnlyWithNamespace(Path.GetFileNameWithoutExtension(csPath), linesFile, null, [null]))
        {
            return null;
        }

        var fnwoe = Path.GetFileNameWithoutExtension(csPath);
        if (csPath.EndsWith(".xaml.cs")) return null;
        if (csPath.Contains(@"\obj\")) return null;
        var fn = Path.GetFileNameWithoutExtension(csPath);
        if (fn == "GlobalSuppressions") return null;
        if (fn == "GlobalUsings") return null;
        if (pathToSave != null)
        {
            csPath = pathToSave;
        }
        //if (CSharpHelper.IsEmptyCommentedOrOnlyWithNamespace("", linesFile, null, new List<string>()))
        //{
        //    return null;
        //}
        var relPath = csPath.Replace(pathToCsprojFolder, string.Empty);
        var parts = relPath.Split('\\', StringSplitOptions.RemoveEmptyEntries).ToList();
        parts.RemoveAt(parts.Count - 1);
        // todo zde je dobré zkontrolovat zda nemám cíe FS NS. když slučuji soubory, občas se to podaří.
        // ve výsledku třída má např FP SunamoFileSystem.SunamoFileSystem.FS
        // případně hledat na CS8954
        // Prvně je nutné odstranit FS NS. druhá metoda musí být 100% správně.
        var linesFileOri = linesFile.ToList();
        linesFile = await RemoveFileScopedNamespaceWhenIsInSharpIf(linesFile);
        var newNs2 = projectName + (parts.Count == 0 ? "" : ".") + string.Join(".", parts);
        linesFile = AddNamespaceIfIsMissingInCs(linesFile, newNs2);
        linesFile = RemoveIfContainsMoreNamespace(linesFile);
        if (!linesFileOri.SequenceEqual(linesFile))
        {
            await TFCsFormat
                .WriteAllLines(pathToSave == null ? csPath : pathToSave, linesFile);
        }
        return newNs2;
    }
    private List<string> RemoveIfContainsMoreNamespace(List<string> list)
    {
        // EN: Find all namespace lines and remove duplicates (keep only the first one)
        // CZ: Najdi všechny namespace řádky a odstraň duplikáty (zachovej pouze první)
        var nsLines = new List<int>();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].StartsWith("namespace ") && list[i].EndsWith(";"))
            {
                nsLines.Add(i);
            }
        }

        // EN: Remove from the end to avoid index shifting
        // CZ: Odstraňuj od konce aby se neposunuly indexy
        for (int i = nsLines.Count - 1; i > 0; i--)
        {
            list.RemoveAt(nsLines[i]);
        }
        return list;
    }
    /// <summary>
    /// Pracovní metoda která se už volá na konkrétní soubor
    /// Volána z AddNamespaceByInputFolderName
    /// </summary>
    /// <param name="item"></param>
    /// <param name="newNs"></param>
    /// <returns></returns>
    private List<string> AddNamespaceIfIsMissingInCs(List<string> lines, string newNs)
    {
        // Tohle jsem tu dal, když jsem byl dement a pracoval jsem v konzoli na neex cestě. Divil jsem se jaktože to v programu jde. Nebylo to tedy debug vs release jak jsem si původně myslel! Opět jsem hledal problém jinde než byl!
        //if (item.EndsWith("XmlGenerator.cs"))
        //{
        //    Console.WriteLine("XmlGenerator: " + item);
        //}
        //throw new Exception("Nepouštět, dojebává mi to soubory tím že sice vloží na první řádek NS; ale nesmaže { }");
        // todo napsat testy na toto
        //var lastIndexOfUsing = -1;
        var isNsOuter = false;
        var dxNamespaceLine = -1;
        RemoveEmptyLinesService removeEmptyLinesService = new RemoveEmptyLinesService();
        for (var i = 0; i < lines.Count; i++)
        {
            // .Trim() tu nemůže být protože pak mi to ořezává celý soubor a musím to znovu formátovat
            lines[i] = lines[i];
            var list = lines[i];
            isNsOuter = list.StartsWith("namespace");
            if (isNsOuter)
            {
                //isNsOuter = isNs;
                dxNamespaceLine = i;
                break;
            }
            if (classCodeElements.Any(d => list.Contains(d)))
            {
                break;
            }
            // vůbec nevím k čemu jsem tu dal tuto konstrukci
            //if (list != "" && list.StartsWith("using") && list.StartsWith("global using") && isNsOuter)
            //{
            //    lastIndexOfUsing = i - 1;
            //    break;
            //}
        }
        // todo zde je problém. přidává mi to NS i když už je v #elif. stačí potom zakomentovat RemoveFileScopedNamespaceWhenIsInSharpIf
        if (dxNamespaceLine != -1 && lines[dxNamespaceLine].Trim() == "namespace")
        {
            // kontrola zda je pod #else správný NS
            var dx = lines.IndexOf("#else");
            if (lines[dx + 1] != newNs)
            {
                lines[dx + 1] = newNs;
            }
        }
        else
        {
            // pokud nějaký řádek začíná namespace
            if (!isNsOuter)
            {
                AddNamespaceOnBegin(newNs, lines);
            }
            else
            {
                // EN: Check if namespace is already correct at line 0 - skip processing to avoid corruption
                // CZ: Zkontroluj zda je namespace už správně na řádku 0 - skipni processing aby se nepokazil soubor
                var expectedNs = "namespace " + newNs + ";";
                if (dxNamespaceLine == 0 && lines[0].Trim() == expectedNs.Trim())
                {
                    // EN: Namespace is already correct, do nothing
                    // CZ: Namespace je již správně, nedělej nic
                    return lines;
                }

                if (dxNamespaceLine != 0)
                {
                    //lines.RemoveAt(dxNamespaceLine);
                    if (!lines[dxNamespaceLine].Contains(";"))
                    {
                        removeEmptyLinesService.RemoveEmptyLinesFromStartAndEnd(lines);
                        lines[0] = lines[0].TrimStart('{');
                        lines[lines.Count - 1] = lines[lines.Count - 1].TrimEnd('}');
                    }
                    //AddNamespaceOnBegin(newNs, lines);
                    #region Tohle jsem nemusel dělat, od toho tu mám už TFCsFormat
                    lines.RemoveAt(dxNamespaceLine);
                    lines.Insert(0, "");
                    lines.Insert(0, "namespace " + newNs + ";");
                    #endregion
                }
                else
                {
                    lines[0] = "namespace " + newNs + ";";
                }
            }
        }
        //// u jiných přidává prázdný řádek protože smazal }
        //var temp = SHJoin.JoinNL(lines, false, lines);
        //// Tohle jsem tu dal, když jsem byl dement a pracoval jsem v konzoli na neex cestě. Divil jsem se jaktože to v programu jde. Nebylo to tedy debug vs release jak jsem si původně myslel! Opět jsem hledal problém jinde než byl!
        ////if (item.EndsWith("XmlGenerator.cs"))
        ////{
        ////    if (temp != text)
        ////    {
        ////        Console.WriteLine("Content changed, writing...");
        ////    }
        ////    else
        ////    {
        ////        Console.WriteLine("Content NOT changed");
        ////    }
        ////}
        //if (temp != text)
        //{
        //    await TFCsFormat.WriteAllText(item, temp);
        //}
        //await TFCsFormat.WriteAllLines(item, lines);
        return lines;
    }
    public readonly List<string> classCodeElements = new List<string>() { "class ", "interface ", "delegate", "enum ", "struct " };
    /// <summary>
    /// FUnguje to OK, prošel jsem si všechny soubory před commitem
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private async Task<List<string>> RemoveFileScopedNamespaceWhenIsInSharpIf(List<string> list)
    {
        //var list = (await TF.ReadAllLines(item)).ToList();
        List<int> dxNs = new List<int>();
        int dxElse = -1;
        for (int i = 0; i < list.Count; i++)
        {
            var line = list[i];
            // chyba byla tady že namespace bylo s mezerou. pak mi to nevrátilo tu v #if
            if (line.StartsWith("namespace"))
            {
                dxNs.Add(i);
            }
            if (line == "#else")
            {
                dxElse = i + 1;
            }
            if (dxElse != -1 && dxNs.Any())
            {
                break;
            }
            if (classCodeElements.Any(d => line.Contains(d)))
            {
                break;
            }
        }
        if (dxElse != -1 && dxNs.Count == 1)
        {
            // tady nevím jestli to je správně. mám jen jeden řádek začínající namespace ale chci ho odstranit
            // proto to zakomentuji
            //list.RemoveAt(dxNs.First());
            //await TFCsFormat.WriteAllLines(item, list);
        }
        else if (dxNs.Count > 1)
        {
            List<string> lines = new List<string>(dxNs.Count);
            foreach (var item2 in dxNs)
            {
                lines.Add(list[item2]);
            }
            // seřadím od nejmenší k největší
            var ordered = lines.OrderBy(d => d.Length).Skip(1);
            foreach (var item3 in ordered)
            {
                list.Remove(item3);
            }
            // Řádky jen odstraňuje, není nutné formátovat
            // Zde se zapíšou jen změny - je to v else if
            //await TFCsFormat.WriteAllLines(item, list);
        }
        return list;
    }
    /// <summary>
    /// Přidá nový file scoped namespace na začátek souboru
    /// </summary>
    /// <param name="newNs"></param>
    /// <param name="lines"></param>
    private void AddNamespaceOnBegin(string newNs, List<string> lines)
    {
        if (lines.Count > 0)
        {
            lines.Insert(0, "namespace " + newNs + ";");
            if (lines[1].Trim() != string.Empty) lines.Insert(0, "");
        }
    }
}