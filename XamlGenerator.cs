//using SunamoXml.Generators;

//namespace SunamoDevCode;

//public class XamlGenerator : XmlGenerator
//{
//    /// <summary>
//    /// in outer rows
//    /// in inner columns
//    /// </summary>
//    /// <param name="elements"></param>
//    public void Grid(List<List<string>> elements)
//    {
//        ThrowEx.HaveAllInnerSameCount(elements);
//        int rows = elements.Count;
//        int columns = elements[0].Count;

//        WriteTag("Grid");

//        //WriteColumnDefinitions(GridHelperSunamo.ForAllTheSame(columns));
//        //WriteRowDefinitions(GridHelperSunamo.ForAllTheSame(rows));

//        for (int row = 0; row < rows; row++)
//        {
//            for (int column = 0; column < columns; column++)
//            {
//                string cell = elements[row][column];
//                cell = cell.Replace("><", $" Grid.Column=\"{column}\" Grid.Row=\"{row}\"><");
//                WriteRaw(cell);
//            }
//        }
//        TerminateTag("Grid");
//    }

//    static Type type = typeof(XamlGenerator);
//    public void WriteDataTemplate(List<double> cd)
//    {
//        WriteRaw(@"<DataTemplate><Grid>");

//        WriteColumnDefinitions(cd);
//        WriteRaw(@"<TextBlock Text='{Binding Channel}'  Grid.Column='0' Width='{Binding Path=_0, Source={StaticResource SizeColumnsVideosListView}, Mode=TwoWay}' MaxWidth='{Binding Path=_0, Source={StaticResource SizeColumnsVideosListView}, Mode=TwoWay}'></TextBlock>
//                                        <TextBlock Text='{Binding Title}' Grid.Column='1' Width='{Binding Path=_1, Source={StaticResource SizeColumnsVideosListView}, Mode=OneWay}'></TextBlock>
//                                        <TextBlock Grid.Column='2' Text='{Binding Extension}' Width='{Binding Path=_2, Source={StaticResource SizeColumnsVideosListView}, Mode=OneWay}'></TextBlock>
//                                        <TextBlock Grid.Column='3' Text='{Binding YtCode}' Width='{Binding Path=_3, Source={StaticResource SizeColumnsVideosListView}, Mode=OneWay}'></TextBlock>
//                                    </Grid>
//                                </DataTemplate>");
//    }

//    public void WriteColumnDefinitions(List<double> cd)
//    {
//        var cds = cd.ConvertAll(d => d.ToString());
//        CAChangeContent.ChangeContent<string, string>(null, cds, SHReplaceOnce.ReplaceOnce, AllStrings.comma, AllStrings.dot);
//        WriteColumnDefinitions(cds);
//    }

//    /// <summary>
//    /// Xaml code write to XMlGenerator, return c# code 
//    /// </summary>
//    /// <param name="headers"></param>
//    /// <param name="methodHandlers"></param>
//    public void SuMenuItems(List<string> headers, bool methodHandlers)
//    {
//        CA.Trim(headers);
//        CA.RemoveStringsEmpty(headers);
//        CA.RemoveStartingWith(CSharpConsts.lc, headers);
//        CA.TrimEnd(headers, AllChars.comma);

//        List<string> headersInPascal = new List<string>(headers.Count);

//        foreach (var item2 in headers)
//        {
//            var item = item2;
//            string inPascal = item;

//            if (!ConvertPascalConvention.IsPascal(item))
//            {
//                inPascal = ConvertPascalConvention.ToConvention(item);
//            }
//            else
//            {
//                item = ConvertPascalConvention.FromConvention(item);
//            }

//            headersInPascal.Add(inPascal);
//            string SuMenuItemName = "mi" + inPascal;

//            string method = string.Empty;
//            if (methodHandlers)
//            {
//                method = "mi" + inPascal + "_" + "Click";
//            }

//            WriteTagWithAttrs("SuMenuItem", "x:" + "Name", SuMenuItemName, "Header", item, "Click", method);
//            TerminateTag("SuMenuItem");
//            AppendLine();
//        }

//        if (methodHandlers)
//        {
//            CSharpGenerator csg = new CSharpGenerator();

//            foreach (var item in headersInPascal)
//            {
//                csg.Method(2, AccessModifiers.Internal, false, "void", "mi" + item + "_" + "Click", "SetMode(Mode." + item + ");", "object o, RoutedEventArgs" + " " + "e");
//            }

//            WriteRaw(csg.ToString());


//        }


//    }

//    public void WriteColumnDefinitions(List<string> cd)
//    {
//        WriteRaw("<Grid.ColumnDefinitions>");
//        foreach (var item in cd)
//        {
//            WriteRaw("<ColumnDefinition Width='" + item + "'></ColumnDefinition>");
//        }
//        WriteRaw("</Grid.ColumnDefinitions>");
//    }

//    public void WriteRowDefinitions(List<double> cd)
//    {
//        var cds = cd.ConvertAll(d => d.ToString());
//        CAChangeContent.ChangeContent<string, string>(null, cds, SHReplaceOnce.ReplaceOnce, AllStrings.comma, AllStrings.dot);
//        WriteRowDefinitions(cds);
//    }

//    public void WriteRowDefinitions(List<string> cd)
//    {
//        WriteRaw("<Grid.RowDefinitions>");
//        foreach (var item in cd)
//        {
//            WriteRaw("<RowDefinition Height='" + item + "'></RowDefinition>");
//        }
//        WriteRaw("</Grid.RowDefinitions>");
//    }

//}
