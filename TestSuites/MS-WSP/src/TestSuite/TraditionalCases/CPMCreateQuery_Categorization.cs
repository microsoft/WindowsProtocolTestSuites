// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.WspTS
{
    public partial class CPMCreateQueryTestCases : WspCommonTestBase
    {
        public class RangePivot
        {
            public object RangeValue { get; }

            public string RangeLabel { get; }

            public RangePivot(object rangeValue, string rangeLabel)
            {
                this.RangeValue = rangeValue;
                this.RangeLabel = rangeLabel;
            }
        }

        private uint[] hierarchicalCursors;

        #region Test Cases

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to verify the server behavior if a range-based grouping operation over a numeric property is requested in CPMCreateQueryIn.")]
        public void CPMCreateQuery_Categorization_NumericRanges()
        {
            var searchScope = Site.Properties.Get("QueryPath") + "Data/CreateQuery_Categorization";
            var mappedProps = new CFullPropSpec[] { WspConsts.System_Size, WspConsts.System_ItemName };
            var columnIdsForGrouping = new uint[] { 0 };
            var rangePivots = new RangePivot[][]
            {
                new RangePivot[]
                {
                    new RangePivot(5UL, "Small"),
                    new RangePivot(10UL, "Medium"),
                    new RangePivot(50UL, "Large")
                },
            };
            var leafResultColumnIds = new uint[] { 0, 1 };
            var createQueryOut = CreateQueryWithCategorization(searchScope, mappedProps, columnIdsForGrouping, rangePivots, leafResultColumnIds);
            hierarchicalCursors = createQueryOut.aCursors;

            var propsForBindings = new Dictionary<uint, CFullPropSpec[]>
            {
                [0] = new CFullPropSpec[] { mappedProps[columnIdsForGrouping[0]] },
                [1] = new CFullPropSpec[] { mappedProps[leafResultColumnIds[0]], mappedProps[leafResultColumnIds[1]] }
            };
            SetBindingsForHierarchicalCursors(propsForBindings);

            var rowStrings = GetChapteredResultsFromRows(1);
            var expectedResults = new string[]
            {
                "0;",
                "    0;abc0.bin;",
                "    4;abc1.bin;",
                "Small;",
                "    8;abc2.bin;",
                "Medium;",
                "    11;abc3.bin;",
                "    12;rst0.bin;",
                "    12;rst2.bin;",
                "    12;rst3.bin;",
                "    30;cda0.bin;",
                "    30;cda1.bin;",
                "    30;cda2.bin;",
                "Large;",
                "    117;ggg4.bin;",
                "    23552;ttt0.doc;",
                "    23552;ttt3.doc;",
                "    27648;mmm0.xls;",
                "    27648;mmm2.xls;",
                "    43520;kkk0.ppt;",
                "    43520;kkk1.ppt;",
                "NULL;",
                "    NULL;AAA;",
                "    NULL;BBB;",
                "    NULL;EEE;",
                "    NULL;SSS;",
            };

            var succeed = expectedResults.SequenceEqual(rowStrings);
            Site.Assert.IsTrue(succeed, "The hierarchical chaptered results should be the same as the expected results.");
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to verify the server behavior if a range-based grouping operation over a string property is requested in CPMCreateQueryIn.")]
        public void CPMCreateQuery_Categorization_UnicodeRanges()
        {
            var searchScope = Site.Properties.Get("QueryPath") + "Data/CreateQuery_Categorization";
            var mappedProps = new CFullPropSpec[] { WspConsts.System_Author, WspConsts.System_ItemName };
            var columnIdsForGrouping = new uint[] { 0 };
            var rangePivots = new RangePivot[][]
            {
                new RangePivot[]
                {
                    new RangePivot("d", "Range 1"),
                    new RangePivot("m", "Range 2"),
                    new RangePivot("r", "Range 3")
                },
            };
            var leafResultColumnIds = new uint[] { 0, 1 };
            var createQueryOut = CreateQueryWithCategorization(searchScope, mappedProps, columnIdsForGrouping, rangePivots, leafResultColumnIds);
            hierarchicalCursors = createQueryOut.aCursors;

            var propsForBindings = new Dictionary<uint, CFullPropSpec[]>
            {
                [0] = new CFullPropSpec[] { mappedProps[columnIdsForGrouping[0]] },
                [1] = new CFullPropSpec[] { mappedProps[leafResultColumnIds[0]], mappedProps[leafResultColumnIds[1]] }
            };
            SetBindingsForHierarchicalCursors(propsForBindings);

            var rowStrings = GetChapteredResultsFromRows(1);
            var expectedResults = new string[]
            {
                ";",
                "    [AAA,BBB];ttt0.doc;",
                "    [AAA,CCC];ttt3.doc;",
                "    [ABC,AAA];mmm0.xls;",
                "    [NNN,BBB];kkk0.ppt;",
                "Range 1;",
                "    [GGG];kkk1.ppt;",
                "Range 2;",
                "    [NNN,BBB];kkk0.ppt;",
                "Range 3;",
                "    [SSS];mmm2.xls;",
                "NULL;",
                "    NULL;AAA;",
                "    NULL;abc0.bin;",
                "    NULL;abc1.bin;",
                "    NULL;abc2.bin;",
                "    NULL;abc3.bin;",
                "    NULL;BBB;",
                "    NULL;cda0.bin;",
                "    NULL;cda1.bin;",
                "    NULL;cda2.bin;",
                "    NULL;EEE;",
                "    NULL;ggg4.bin;",
                "    NULL;rst0.bin;",
                "    NULL;rst2.bin;",
                "    NULL;rst3.bin;",
                "    NULL;SSS;",
            };

            var succeed = expectedResults.SequenceEqual(rowStrings);
            Site.Assert.IsTrue(succeed, "The hierarchical chaptered results should be the same as the expected results.");
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to verify the server behavior if a unique grouping operation over a numeric property is requested in CPMCreateQueryIn.")]
        public void CPMCreateQuery_Categorization_UniqueNumericValues()
        {
            var searchScope = Site.Properties.Get("QueryPath") + "Data/CreateQuery_Categorization";
            var mappedProps = new CFullPropSpec[] { WspConsts.System_Size, WspConsts.System_ItemName };
            var columnIdsForGrouping = new uint[] { 0 };
            var rangePivots = new RangePivot[][] { null };
            var leafResultColumnIds = new uint[] { 0, 1 };
            var createQueryOut = CreateQueryWithCategorization(searchScope, mappedProps, columnIdsForGrouping, rangePivots, leafResultColumnIds);
            hierarchicalCursors = createQueryOut.aCursors;

            var propsForBindings = new Dictionary<uint, CFullPropSpec[]>
            {
                [0] = new CFullPropSpec[] { mappedProps[columnIdsForGrouping[0]] },
                [1] = new CFullPropSpec[] { mappedProps[leafResultColumnIds[0]], mappedProps[leafResultColumnIds[1]] }
            };
            SetBindingsForHierarchicalCursors(propsForBindings);

            var rowStrings = GetChapteredResultsFromRows(1);
            var expectedResults = new string[]
            {
                "0;",
                "    0;abc0.bin;",
                "4;",
                "    4;abc1.bin;",
                "8;",
                "    8;abc2.bin;",
                "11;",
                "    11;abc3.bin;",
                "12;",
                "    12;rst0.bin;",
                "    12;rst2.bin;",
                "    12;rst3.bin;",
                "30;",
                "    30;cda0.bin;",
                "    30;cda1.bin;",
                "    30;cda2.bin;",
                "117;",
                "    117;ggg4.bin;",
                "23552;",
                "    23552;ttt0.doc;",
                "    23552;ttt3.doc;",
                "27648;",
                "    27648;mmm0.xls;",
                "    27648;mmm2.xls;",
                "43520;",
                "    43520;kkk0.ppt;",
                "    43520;kkk1.ppt;",
                "NULL;",
                "    NULL;AAA;",
                "    NULL;BBB;",
                "    NULL;EEE;",
                "    NULL;SSS;",
            };

            var succeed = expectedResults.SequenceEqual(rowStrings);
            Site.Assert.IsTrue(succeed, "The hierarchical chaptered results should be the same as the expected results.");
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to verify the server behavior if a unique grouping operation over a string property is requested in CPMCreateQueryIn.")]
        public void CPMCreateQuery_Categorization_UniqueUnicodeValues()
        {
            var searchScope = Site.Properties.Get("QueryPath") + "Data/CreateQuery_Categorization";
            var mappedProps = new CFullPropSpec[] { WspConsts.System_Author, WspConsts.System_ItemName };
            var columnIdsForGrouping = new uint[] { 0 };
            var rangePivots = new RangePivot[][] { null };
            var leafResultColumnIds = new uint[] { 0, 1 };
            var createQueryOut = CreateQueryWithCategorization(searchScope, mappedProps, columnIdsForGrouping, rangePivots, leafResultColumnIds);
            hierarchicalCursors = createQueryOut.aCursors;

            var propsForBindings = new Dictionary<uint, CFullPropSpec[]>
            {
                [0] = new CFullPropSpec[] { mappedProps[columnIdsForGrouping[0]] },
                [1] = new CFullPropSpec[] { mappedProps[leafResultColumnIds[0]], mappedProps[leafResultColumnIds[1]] }
            };
            SetBindingsForHierarchicalCursors(propsForBindings);

            var rowStrings = GetChapteredResultsFromRows(1);
            var expectedResults = new string[]
            {
                "AAA;",
                "    [AAA,BBB];ttt0.doc;",
                "    [AAA,CCC];ttt3.doc;",
                "    [ABC,AAA];mmm0.xls;",
                "ABC;",
                "    [ABC,AAA];mmm0.xls;",
                "BBB;",
                "    [AAA,BBB];ttt0.doc;",
                "    [NNN,BBB];kkk0.ppt;",
                "CCC;",
                "    [AAA,CCC];ttt3.doc;",
                "GGG;",
                "    [GGG];kkk1.ppt;",
                "NNN;",
                "    [NNN,BBB];kkk0.ppt;",
                "SSS;",
                "    [SSS];mmm2.xls;",
                "NULL;",
                "    NULL;AAA;",
                "    NULL;abc0.bin;",
                "    NULL;abc1.bin;",
                "    NULL;abc2.bin;",
                "    NULL;abc3.bin;",
                "    NULL;BBB;",
                "    NULL;cda0.bin;",
                "    NULL;cda1.bin;",
                "    NULL;cda2.bin;",
                "    NULL;EEE;",
                "    NULL;ggg4.bin;",
                "    NULL;rst0.bin;",
                "    NULL;rst2.bin;",
                "    NULL;rst3.bin;",
                "    NULL;SSS;",
            };

            var succeed = expectedResults.SequenceEqual(rowStrings);
            Site.Assert.IsTrue(succeed, "The hierarchical chaptered results should be the same as the expected results.");
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to verify the server behavior if a netsted grouping operation over 2 properties is requested in CPMCreateQueryIn.")]
        public void CPMCreateQuery_Categorization_NestedGrouping_2Levels()
        {
            var searchScope = Site.Properties.Get("QueryPath") + "Data/CreateQuery_Categorization";
            var mappedProps = new CFullPropSpec[] { WspConsts.System_ItemFolderNameDisplay, WspConsts.System_Author, WspConsts.System_ItemName };
            var columnIdsForGrouping = new uint[] { 0, 1 };
            var rangePivots = new RangePivot[][]
            {
                null,
                new RangePivot[]
                {
                    new RangePivot("d", null),
                    new RangePivot("m", null),
                    new RangePivot("r", null)
                }
            };
            var leafResultColumnIds = new uint[] { 0, 1, 2 };
            var createQueryOut = CreateQueryWithCategorization(searchScope, mappedProps, columnIdsForGrouping, rangePivots, leafResultColumnIds);
            hierarchicalCursors = createQueryOut.aCursors;

            var propsForBindings = new Dictionary<uint, CFullPropSpec[]>
            {
                [0] = new CFullPropSpec[] { mappedProps[columnIdsForGrouping[0]] },
                [1] = new CFullPropSpec[] { mappedProps[columnIdsForGrouping[1]] },
                [2] = new CFullPropSpec[] { mappedProps[leafResultColumnIds[0]], mappedProps[leafResultColumnIds[1]], mappedProps[leafResultColumnIds[2]] }
            };
            SetBindingsForHierarchicalCursors(propsForBindings);

            var rowStrings = GetChapteredResultsFromRows(2);
            var expectedResults = new string[]
            {
                "AAA;",
                "    d;",
                "        AAA;[GGG];kkk1.ppt;",
                "    NULL;",
                "        AAA;NULL;abc1.bin;",
                "        AAA;NULL;cda1.bin;",
                "BBB;",
                "    r;",
                "        BBB;[SSS];mmm2.xls;",
                "    NULL;",
                "        BBB;NULL;abc2.bin;",
                "        BBB;NULL;cda2.bin;",
                "        BBB;NULL;rst2.bin;",
                "CreateQuery_Categorization;",
                "    ;",
                "        CreateQuery_Categorization;[AAA,BBB];ttt0.doc;",
                "        CreateQuery_Categorization;[ABC,AAA];mmm0.xls;",
                "    m;",
                "        CreateQuery_Categorization;[NNN,BBB];kkk0.ppt;",
                "    NULL;",
                "        CreateQuery_Categorization;NULL;AAA;",
                "        CreateQuery_Categorization;NULL;abc0.bin;",
                "        CreateQuery_Categorization;NULL;BBB;",
                "        CreateQuery_Categorization;NULL;cda0.bin;",
                "        CreateQuery_Categorization;NULL;EEE;",
                "        CreateQuery_Categorization;NULL;rst0.bin;",
                "        CreateQuery_Categorization;NULL;SSS;",
                "EEE;",
                "    ;",
                "        EEE;[AAA,CCC];ttt3.doc;",
                "    NULL;",
                "        EEE;NULL;abc3.bin;",
                "        EEE;NULL;rst3.bin;",
                "SSS;",
                "    NULL;",
                "        SSS;NULL;ggg4.bin;",
            };

            var succeed = expectedResults.SequenceEqual(rowStrings);
            Site.Assert.IsTrue(succeed, "The hierarchical chaptered results should be the same as the expected results.");
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to verify the server behavior if a netsted grouping operation over 3 properties is requested in CPMCreateQueryIn.")]
        public void CPMCreateQuery_Categorization_NestedGrouping_3Levels()
        {
            var searchScope = Site.Properties.Get("QueryPath") + "Data/CreateQuery_Categorization";
            var mappedProps = new CFullPropSpec[] { WspConsts.System_ItemFolderNameDisplay, WspConsts.System_Author, WspConsts.System_Size, WspConsts.System_ItemName };
            var columnIdsForGrouping = new uint[] { 0, 1, 2 };
            var rangePivots = new RangePivot[][]
            {
                null,
                new RangePivot[]
                {
                    new RangePivot("d", null),
                    new RangePivot("m", null),
                    new RangePivot("r", null)
                },
                null
            };
            var leafResultColumnIds = new uint[] { 0, 1, 3 };
            var createQueryOut = CreateQueryWithCategorization(searchScope, mappedProps, columnIdsForGrouping, rangePivots, leafResultColumnIds);
            hierarchicalCursors = createQueryOut.aCursors;

            var propsForBindings = new Dictionary<uint, CFullPropSpec[]>
            {
                [0] = new CFullPropSpec[] { mappedProps[columnIdsForGrouping[0]] },
                [1] = new CFullPropSpec[] { mappedProps[columnIdsForGrouping[1]] },
                [2] = new CFullPropSpec[] { mappedProps[columnIdsForGrouping[2]] },
                [3] = new CFullPropSpec[] { mappedProps[leafResultColumnIds[0]], mappedProps[leafResultColumnIds[1]], mappedProps[leafResultColumnIds[2]] }
            };
            SetBindingsForHierarchicalCursors(propsForBindings);

            var rowStrings = GetChapteredResultsFromRows(3);
            var expectedResults = new string[]
            {
                "AAA;",
                "    d;",
                "        43520;",
                "            AAA;[GGG];kkk1.ppt;",
                "    NULL;",
                "        4;",
                "            AAA;NULL;abc1.bin;",
                "        30;",
                "            AAA;NULL;cda1.bin;",
                "BBB;",
                "    r;",
                "        27648;",
                "            BBB;[SSS];mmm2.xls;",
                "    NULL;",
                "        8;",
                "            BBB;NULL;abc2.bin;",
                "        12;",
                "            BBB;NULL;rst2.bin;",
                "        30;",
                "            BBB;NULL;cda2.bin;",
                "CreateQuery_Categorization;",
                "    ;",
                "        23552;",
                "            CreateQuery_Categorization;[AAA,BBB];ttt0.doc;",
                "        27648;",
                "            CreateQuery_Categorization;[ABC,AAA];mmm0.xls;",
                "    m;",
                "        43520;",
                "            CreateQuery_Categorization;[NNN,BBB];kkk0.ppt;",
                "    NULL;",
                "        0;",
                "            CreateQuery_Categorization;NULL;abc0.bin;",
                "        12;",
                "            CreateQuery_Categorization;NULL;rst0.bin;",
                "        30;",
                "            CreateQuery_Categorization;NULL;cda0.bin;",
                "        NULL;",
                "            CreateQuery_Categorization;NULL;AAA;",
                "            CreateQuery_Categorization;NULL;BBB;",
                "            CreateQuery_Categorization;NULL;EEE;",
                "            CreateQuery_Categorization;NULL;SSS;",
                "EEE;",
                "    ;",
                "        23552;",
                "            EEE;[AAA,CCC];ttt3.doc;",
                "    NULL;",
                "        11;",
                "            EEE;NULL;abc3.bin;",
                "        12;",
                "            EEE;NULL;rst3.bin;",
                "SSS;",
                "    NULL;",
                "        117;",
                "            SSS;NULL;ggg4.bin;",
            };

            var succeed = expectedResults.SequenceEqual(rowStrings);
            Site.Assert.IsTrue(succeed, "The hierarchical chaptered results should be the same as the expected results.");
        }

        #endregion

        private List<string> GetChapteredResultsFromRows(uint chapterDepth)
        {
            // This variable is to record the current chapter value of each cursor for getting chaptered rows.
            var chapterMap = new Dictionary<uint, uint>();
            for (var currentDepth = 0U; currentDepth <= chapterDepth; currentDepth++)
            {
                chapterMap.Add(currentDepth, 1);
            }

            var rowStrings = new List<string>();
            GetChapteredRows(chapterDepth, 0, chapterMap, rowStrings);

            Site.Log.Add(LogEntryKind.Debug, "Start to print all rows obtained by the query.");
            foreach (var rowString in rowStrings)
            {
                Site.Log.Add(LogEntryKind.Debug, rowString);
            }
            Site.Log.Add(LogEntryKind.Debug, "All rows have been printed.");

            return rowStrings;
        }

        private void GetChapteredRows(uint chapterDepth, uint currentDepth, Dictionary<uint, uint> chapterMap, List<string> rowStrings)
        {
            if (currentDepth < chapterDepth)
            {
                if (currentDepth == 0)
                {
                    chapterMap[currentDepth] = 0;
                }

                wspAdapter.CPMGetRowsIn(
                    hierarchicalCursors[currentDepth],
                    1,
                    wspAdapter.Builder.Parameter.EachRowSize,
                    wspAdapter.Builder.Parameter.BufferSize,
                    0,
                    (uint)CPMGetRowsIn_eType_Values.eRowSeekNext,
                    chapterMap[currentDepth],
                    new CRowSeekNext { _cskip = 0 },
                    out var getRowsOut);

                if (getRowsOut._cRowsReturned > 0)
                {
                    rowStrings.AddRange(GetPrintableRows(currentDepth, getRowsOut.Rows));

                    if (currentDepth + 1 > chapterDepth)
                    {
                        return;
                    }

                    GetChapteredRows(chapterDepth, currentDepth + 1, chapterMap, rowStrings);
                }
                else
                {
                    if (currentDepth == 0)
                    {
                        return;
                    }

                    chapterMap[currentDepth] = chapterMap[currentDepth] + 1;
                    GetChapteredRows(chapterDepth, currentDepth - 1, chapterMap, rowStrings);
                }
            }
            else
            {
                while (true)
                {
                    wspAdapter.CPMGetRowsIn(
                        hierarchicalCursors[currentDepth],
                        1,
                        wspAdapter.Builder.Parameter.EachRowSize,
                        wspAdapter.Builder.Parameter.BufferSize,
                        0,
                        (uint)CPMGetRowsIn_eType_Values.eRowSeekNext,
                        chapterMap[currentDepth],
                        new CRowSeekNext { _cskip = 0 },
                        out var getRowsOut);

                    if (getRowsOut._cRowsReturned > 0)
                    {
                        rowStrings.AddRange(GetPrintableRows(currentDepth, getRowsOut.Rows));
                    }
                    else
                    {
                        if (currentDepth == 0)
                        {
                            return;
                        }

                        chapterMap[currentDepth] = chapterMap[currentDepth] + 1;
                        GetChapteredRows(chapterDepth, currentDepth - 1, chapterMap, rowStrings);
                        return;
                    }
                }
            }
        }

        private List<string> GetPrintableRows(uint chapterDepth, Row[] rows)
        {
            var ret = new List<string>();

            foreach (var row in rows)
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append(' ', (int)(chapterDepth * 4));
                foreach (var column in row.Columns)
                {
                    if (column.Data != null)
                    {
                        if (column.Data is string[])
                        {
                            var stringVector = column.Data as string[];
                            stringBuilder.Append('[');
                            foreach (var element in stringVector)
                            {
                                stringBuilder.Append($"{element},");
                            }
                            stringBuilder.Remove(stringBuilder.Length - 1, 1);
                            stringBuilder.Append("];");
                        }
                        else
                        {
                            stringBuilder.Append($"{column.Data};");
                        }
                    }
                    else
                    {
                        stringBuilder.Append("NULL;");
                    }
                }

                ret.Add(stringBuilder.ToString());
            }

            return ret;
        }

        private CPMCreateQueryOut CreateQueryWithCategorization(
            string searchScope,
            CFullPropSpec[] mappedProps,
            uint[] columnIdsForGrouping,
            RangePivot[][] rangePivots,
            uint[] leafResultColumnIds)
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            var searchScopeRetriction = wspAdapter.Builder.GetPropertyRestriction(
                    CPropertyRestriction_relop_Values.PREQ,
                    WspConsts.System_Search_Scope,
                    wspAdapter.Builder.GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_LPWSTR, new VT_LPWSTR(searchScope)));
            var restrictionArray = new CRestrictionArray { count = 1, isPresent = 1, Restriction = searchScopeRetriction };

            var columnSet = wspAdapter.Builder.GetColumnSet(mappedProps.Length);

            var pidMapper = GetCPidMapper(mappedProps);

            var sortSets = GetCInGroupSortAggregSets(columnIdsForGrouping, leafResultColumnIds);

            var specs = new CCategorizationSpec[columnIdsForGrouping.Length];
            for (var idx = 0U; idx < rangePivots.Length; idx++)
            {
                if (rangePivots[idx] != null)
                {
                    specs[idx] = GetRangeCCategorizationSpec(rangePivots[idx], columnIdsForGrouping[idx]);
                }
                else
                {
                    specs[idx] = GetUniqueCCategorizationSpec(columnIdsForGrouping[idx]);
                }
            }
            var categorizationSet = GetCCategorizationSet(specs);
            Site.Log.Add(LogEntryKind.TestStep, $"Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(
                columnSet,
                restrictionArray,
                sortSets,
                categorizationSet,
                new CRowsetProperties(),
                pidMapper,
                new CColumnGroupArray(),
                wspAdapter.Builder.Parameter.LcidValue,
                out var createQueryOut);

            return createQueryOut;
        }

        private void SetBindingsForHierarchicalCursors(Dictionary<uint, CFullPropSpec[]> propsForBindings)
        {
            for (var idx = 0U; idx < hierarchicalCursors.Length; idx++)
            {
                var columns = new List<CTableColumn>();
                foreach (var prop in propsForBindings[idx])
                {
                    columns.Add(wspAdapter.Builder.GetTableColumn(prop, CBaseStorageVariant_vType_Values.VT_VARIANT));
                }

                Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
                wspAdapter.CPMSetBindingsIn(
                    hierarchicalCursors[idx],
                    wspAdapter.Builder.Parameter.EachRowSize,
                    (uint)columns.Count,
                    columns.ToArray());
            }
        }

        private CPidMapper GetCPidMapper(params CFullPropSpec[] mappedProps)
        {
            var ret = new CPidMapper();
            ret.aPropSpec = mappedProps;
            ret.count = (uint)ret.aPropSpec.Length;

            return ret;
        }

        private CInGroupSortAggregSets GetCInGroupSortAggregSets(uint[] columnIdsForGrouping, uint[] leafResultColumnIds)
        {
            var ret = new CInGroupSortAggregSets();
            ret.cCount = 1;
            ret.SortSets = new CSortSet[1];
            ret.SortSets[0].count = (uint)(columnIdsForGrouping.Length + leafResultColumnIds.Length);

            var sortArray = new CSort[ret.SortSets[0].count];
            var idx = 0;
            foreach (var columnId in columnIdsForGrouping.Concat(leafResultColumnIds))
            {
                sortArray[idx] = new CSort
                {
                    dwOrder = CSort_dwOrder_Values.QUERY_SORTASCEND,
                    dwIndividual = CSort_dwIndividual_Values.QUERY_SORTALL,
                    pidColumn = columnId,
                    locale = wspAdapter.Builder.Parameter.LcidValue
                };
                idx++;
            }

            ret.SortSets[0].sortArray = sortArray;
            return ret;
        }

        private CCategorizationSet GetCCategorizationSet(params CCategorizationSpec[] categSpecs)
        {
            var ret = new CCategorizationSet();
            ret.count = (uint)categSpecs.Length;
            ret.categories = categSpecs;

            return ret;
        }

        private CCategorizationSpec GetUniqueCCategorizationSpec(uint columnIdForGrouping)
        {
            var lcid = wspAdapter.Builder.Parameter.LcidValue;

            var ret = new CCategorizationSpec();

            var csColumn = new CColumnSet
            {
                count = 0,
                indexes = new uint[0]
            };
            ret._csColumns = csColumn;

            var spec = new CCategSpec();
            spec._ulCategType = CCategSpec_ulCategType_Values.CATEGORIZE_UNIQUE;
            var sortKey = new CSort
            {
                pidColumn = columnIdForGrouping,
                dwOrder = CSort_dwOrder_Values.QUERY_SORTASCEND,
                dwIndividual = CSort_dwIndividual_Values.QUERY_SORTALL,
                locale = lcid
            };
            spec._sortKey = sortKey;
            ret._Spec = spec;

            ret._AggregSet = new CAggregSet { cCount = 0, AggregSpecs = new CAggregSpec[0] };
            ret._SortAggregSet = new CSortAggregSet { cCount = 0, SortKeys = new CAggregSortKey[0] };
            ret._InGroupSortAggregSets = new CInGroupSortAggregSets { cCount = 0, Reserved = 0, SortSets = new CSortSet[0] };

            return ret;
        }

        private CCategorizationSpec GetRangeCCategorizationSpec(RangePivot[] rangePivots, uint columnIdForGrouping)
        {
            var lcid = wspAdapter.Builder.Parameter.LcidValue;

            CBaseStorageVariant_vType_Values prValVType;
            var keyType = rangePivots[0].RangeValue.GetType();
            if (keyType == typeof(ulong))
            {
                prValVType = CBaseStorageVariant_vType_Values.VT_UI8;
            }
            else if (keyType == typeof(string))
            {
                prValVType = CBaseStorageVariant_vType_Values.VT_LPWSTR;
            }
            else
            {
                throw new NotImplementedException($"The process for {keyType} is not implemented.");
            }

            var ret = new CCategorizationSpec();

            var csColumn = new CColumnSet
            {
                count = 0,
                indexes = new uint[0]
            };
            ret._csColumns = csColumn;

            var spec = new CCategSpec();
            spec._ulCategType = CCategSpec_ulCategType_Values.CATEGORIZE_RANGE;
            var sortKey = new CSort
            {
                pidColumn = columnIdForGrouping,
                dwOrder = CSort_dwOrder_Values.QUERY_SORTASCEND,
                dwIndividual = CSort_dwIndividual_Values.QUERY_SORTALL,
                locale = lcid
            };
            spec._sortKey = sortKey;
            var ranges = new CRangeCategSpec();
            ranges._lcid = lcid;
            ranges.cRange = (uint)rangePivots.Length;
            var boundaries = new RANGEBOUNDARY[ranges.cRange];
            var idx = 0;
            foreach (var rangePivot in rangePivots)
            {
                var (pivot, label) = (rangePivot.RangeValue, rangePivot.RangeLabel);
                boundaries[idx] = new RANGEBOUNDARY
                {
                    ulType = RANGEBOUNDARY_ulType_Values.DBRANGEBOUNDTTYPE_EXACT,
                    prVal = wspAdapter.Builder.GetBaseStorageVariant(prValVType, GetValueByVType(prValVType, pivot)),
                };

                if (string.IsNullOrEmpty(label))
                {
                    boundaries[idx].labelPresent = 0x0;
                }
                else
                {
                    boundaries[idx].labelPresent = 0x1;
                    boundaries[idx].ccLabel = (uint)label.Length;
                    boundaries[idx].Label = label;
                }

                idx++;
            }
            ranges.aRangeBegin = boundaries;
            spec.CRangeCategSpec = ranges;
            ret._Spec = spec;

            ret._AggregSet = new CAggregSet { cCount = 0, AggregSpecs = new CAggregSpec[0] };
            ret._SortAggregSet = new CSortAggregSet { cCount = 0, SortKeys = new CAggregSortKey[0] };
            ret._InGroupSortAggregSets = new CInGroupSortAggregSets { cCount = 0, Reserved = 0, SortSets = new CSortSet[0] };

            return ret;
        }

        private object GetValueByVType(CBaseStorageVariant_vType_Values prValVType, object pivot)
        {
            switch (prValVType)
            {
                case CBaseStorageVariant_vType_Values.VT_UI8:
                    return pivot;

                case CBaseStorageVariant_vType_Values.VT_LPWSTR:
                    return new VT_LPWSTR(pivot as string);

                default:
                    throw new NotImplementedException($"The process for {prValVType} is not implemented.");
            }
        }
    }
}