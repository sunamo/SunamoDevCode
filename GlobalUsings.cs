global using System.Collections.Generic;
global using System;
global using System.Linq;
global using System.Text;
global using System.Collections;
global using System.IO;
global using System.Threading.Tasks;
global using System.Diagnostics;
global using System.Data;
global using System.Text.RegularExpressions;
global using System.Xml.Linq;
global using Case.NET.Extensions;
global using System.Reflection;
global using TextCopy;
global using HtmlAgilityPack;
global using SunamoDevCode.Args;
global using SunamoDevCode.CodeGenerator;
global using SunamoDevCode.Data;
global using SunamoDevCode.Enums;
global using SunamoDevCode.Helpers;
global using SunamoDevCode.Interfaces;
global using SunamoDevCode.SunamoCSharp;
global using SunamoDevCode.Values;
global using SunamoDevCode._public;
global using SunamoDevCode._sunamo;
global using SunamoDevCode.SunamoCSharp.Args;
global using SunamoDevCode.SunamoCSharp.Helpers;
global using SunamoDevCode.SunamoCSharp.Values;
global using SunamoDevCode.SunamoSolutionsIndexer.Args;
global using SunamoDevCode.SunamoSolutionsIndexer.Enums;
global using SunamoDevCode.SunamoSolutionsIndexer.Interfaces;
global using SunamoDevCode._public.SunamoCollectionsNonGeneric;
global using SunamoDevCode._public.SunamoCollectionWithoutDuplicates;
global using SunamoDevCode._public.SunamoGitBashBuilder;
global using SunamoDevCode._public.SunamoTextOutputGenerator;
global using SunamoDevCode._sunamo.SunamoArgs;
global using SunamoDevCode._sunamo.SunamoBts;
global using SunamoDevCode._sunamo.SunamoCollectionsChangeContent;
global using SunamoDevCode._sunamo.SunamoCollectionsGeneric;
global using SunamoDevCode._sunamo.SunamoCollectionsIndexesWithNull;
global using SunamoDevCode._sunamo.SunamoEmbeddedResources;
global using SunamoDevCode._sunamo.SunamoEnumsHelper;
global using SunamoDevCode._sunamo.SunamoExceptions;
global using SunamoDevCode._sunamo.SunamoFileExtensions;
global using SunamoDevCode._sunamo.SunamoFileIO;
global using SunamoDevCode._sunamo.SunamoGetFiles;
global using SunamoDevCode._sunamo.SunamoRegex;
global using SunamoDevCode._sunamo.SunamoString;
global using SunamoDevCode._sunamo.SunamoStringFormat;
global using SunamoDevCode._sunamo.SunamoStringGetLines;
global using SunamoDevCode._sunamo.SunamoStringParts;
global using SunamoDevCode._sunamo.SunamoStringReplace;
global using SunamoDevCode._sunamo.SunamoStringSplit;
global using SunamoDevCode._sunamo.SunamoStringTrim;
global using SunamoDevCode._sunamo.SunamoTextOutputGenerator;
global using SunamoDevCode._sunamo.SunamoTwoWayDictionary;
global using SunamoDevCode._sunamo.SunamoValues;

global using SunamoDevCode._sunamo.SunamoXml;
global using SunamoDevCode.SunamoSolutionsIndexer.Data.SolutionFolderNs;
global using SunamoDevCode.SunamoSolutionsIndexer.Data.SolutionFoldersNs;
global using SunamoDevCode._public.SunamoData.Data;
global using SunamoDevCode._public.SunamoEnums.Enums;
global using SunamoDevCode._public.SunamoExceptions.Data;
global using SunamoDevCode._public.SunamoExceptions.OnlyInSE;
global using SunamoDevCode._public.SunamoInterfaces.Interfaces;
global using SunamoDevCode._sunamo.SunamoConverters.Converts;
global using SunamoDevCode._sunamo.SunamoEnums.Enums;
global using SunamoDevCode._sunamo.SunamoExceptions.OnlyInSE;
global using SunamoDevCode._sunamo.SunamoHtml.Html;
global using SunamoDevCode._sunamo.SunamoInterfaces.Interfaces;
global using SunamoDevCode._sunamo.SunamoLang.SunamoI18N;
global using SunamoDevCode._sunamo.SunamoLang.SunamoXlf;
global using SunamoDevCode._sunamo.SunamoValues.All;
global using SunamoDevCode._sunamo.SunamoValues.Constants;
global using SunamoDevCode._sunamo.SunamoValues.Values;
global using Diacritics.Extensions;
global using System.Net;
global using System.Runtime.CompilerServices;
global using System.Web;
global using System.Xml;
global using SunamoDevCode._sunamo.SunamoCollections;
global using SunamoDevCode._sunamo.SunamoFileSystem;
global using SunamoDevCode;
global using SunamoDevCode.Constants;
global using SunamoDevCode.FileFormats;
global using SunamoDevCode.SunamoSolutionsIndexer;
global using SunamoDevCode.Templates;
global using SunamoDevCode.Trask;
global using SunamoDevCode._sunamo.SunamoDelegates;
global using SunamoDevCode.SunamoSolutionsIndexer.Data.Project;
global using SunamoDevCode._sunamo.SunamoData.Data;