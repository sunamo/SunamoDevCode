namespace SunamoDevCode.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AddOrEditNamespaceService
{
    // Automatically save to file
    public async Task<string> AddOrEditNamespaceForSingleFileAndSave(string pathToCsprojFolder, string projectName
        , string csPath, List<string> linesFile = null, string pathToSave = null)
    {
        var fnwoe = Path.GetFileNameWithoutExtension(csPath);

        if (csPath.EndsWith(".xaml.cs")) return null;

        if (csPath.Contains(@"\obj\")) return null;

        var fn = Path.GetFileNameWithoutExtension(csPath);
        if (fn == "GlobalSuppressions") return null;

        if (fn == "GlobalUsings") return null;

        if (linesFile == null)
        {
            linesFile = (await File.ReadAllLinesAsync(csPath)).ToList();
        }

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



        linesFile = await RemoveFileScopedNamespaceWhenIsInSharpIf(linesFile, fnwoe);



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

    private List<string> RemoveIfContainsMoreNamespace(List<string> l)
    {
        // nemůžu to tu trimovat protože bych to dole nenašel. navíc je to asi zbytečné, jen by to snižovalo výkon
        var nsLines = l.Where(d => d.StartsWith("namespace ") && d.EndsWith(";")).ToList();

        foreach (var item in nsLines.Skip(1))
        {
            var dx = l.IndexOf(item);

            l.RemoveAt(dx);
        }

        return l;
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
            var l = lines[i];

            isNsOuter = l.StartsWith("namespace");

            if (isNsOuter)
            {
                //isNsOuter = isNs;
                dxNamespaceLine = i;

                break;
            }

            if (classCodeElements.Any(d => l.Contains(d)))
            {
                break;
            }

            // vůbec nevím k čemu jsem tu dal tuto konstrukci
            //if (l != "" && l.StartsWith("using") && l.StartsWith("global using") && isNsOuter)
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
        //var t = SHJoin.JoinNL(lines, false, lines);

        //// Tohle jsem tu dal, když jsem byl dement a pracoval jsem v konzoli na neex cestě. Divil jsem se jaktože to v programu jde. Nebylo to tedy debug vs release jak jsem si původně myslel! Opět jsem hledal problém jinde než byl!
        ////if (item.EndsWith("XmlGenerator.cs"))
        ////{
        ////    if (t != text)
        ////    {
        ////        Console.WriteLine("Content changed, writing...");
        ////    }
        ////    else
        ////    {
        ////        Console.WriteLine("Content NOT changed");
        ////    }
        ////}

        //if (t != text)
        //{
        //    await TFCsFormat.WriteAllText(item, t);
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
    private async Task<List<string>> RemoveFileScopedNamespaceWhenIsInSharpIf(List<string> l, string fnwoe)
    {
        //var l = (await TF.ReadAllLines(item)).ToList();


        List<int> dxNs = new List<int>();
        int dxElse = -1;






        for (int i = 0; i < l.Count; i++)
        {
            var line = l[i];
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
            //l.RemoveAt(dxNs.First());
            //await TFCsFormat.WriteAllLines(item, l);
        }
        else if (dxNs.Count > 1)
        {
            List<string> lines = new List<string>(dxNs.Count);
            foreach (var item2 in dxNs)
            {
                lines.Add(l[item2]);
            }

            // seřadím od nejmenší k největší
            var ordered = lines.OrderBy(d => d.Length).Skip(1);

            foreach (var item3 in ordered)
            {
                l.Remove(item3);
            }

            // Řádky jen odstraňuje, není nutné formátovat
            // Zde se zapíšou jen změny - je to v else if
            //await TFCsFormat.WriteAllLines(item, l);
        }

        return l;
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