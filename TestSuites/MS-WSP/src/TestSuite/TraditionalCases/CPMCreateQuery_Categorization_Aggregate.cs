// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp.Adapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.WspTS
{
    public partial class CPMCreateQueryTestCases : WspCommonTestBase
    {
        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to verify the server behavior if aggregate type MAX is sent in CMPCreateQueryIn.")]
        public void CPMCreateQuery_Categorization_Aggregate_MAX()
        {
            // Test132Size is the maximum size of the files in query.
            CPMCreateQuery_Categorization_Aggregate(CAggregSpec_type_Values.DBAGGTTYPE_MAX, WspConsts.System_Size, test132Size);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to verify the server behavior if aggregate type MIN is sent in CMPCreateQueryIn.")]
        public void CPMCreateQuery_Categorization_Aggregate_MIN()
        {
            // Test27Size is the minimum size of the files in query.
            CPMCreateQuery_Categorization_Aggregate(CAggregSpec_type_Values.DBAGGTTYPE_MIN, WspConsts.System_Size, test27Size);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to verify the server behavior if aggregate type AVG is sent in CMPCreateQueryIn.")]
        public void CPMCreateQuery_Categorization_Aggregate_AVG()
        {
            // It is converted from double to int.
            var averageSize = Convert.ToInt32((test1Size + test27Size + test132Size) / 3.0);
            CPMCreateQuery_Categorization_Aggregate(CAggregSpec_type_Values.DBAGGTTYPE_AVG, WspConsts.System_Size, averageSize);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to verify the server behavior if aggregate type SUM is sent in CMPCreateQueryIn.")]
        public void CPMCreateQuery_Categorization_Aggregate_SUM()
        {
            var totalSize = test1Size + test27Size + test132Size;
            CPMCreateQuery_Categorization_Aggregate(CAggregSpec_type_Values.DBAGGTTYPE_SUM, WspConsts.System_Size, totalSize);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to verify the server behavior if aggregate type DATERANGE is sent in CMPCreateQueryIn.")]
        public void CPMCreateQuery_Categorization_Aggregate_DATERANGE()
        {
            // Returns the lower and upper bounds of the queried files' created date.
            CPMCreateQuery_Categorization_Aggregate(CAggregSpec_type_Values.DBAGGTTYPE_DATERANGE, WspConsts.System_DateCreated);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to verify the server behavior if aggregate type COUNT is sent in CMPCreateQueryIn.")]
        public void CPMCreateQuery_Categorization_Aggregate_COUNT()
        {
            // 3 is the total count of the files in query.
            CPMCreateQuery_Categorization_Aggregate(CAggregSpec_type_Values.DBAGGTTYPE_COUNT, WspConsts.System_Size, 3);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to verify the server behavior if aggregate type CHILDCOUNT is sent in CMPCreateQueryIn.")]
        public void CPMCreateQuery_Categorization_Aggregate_CHILDCOUNT()
        {
            // CATEGORIZE_UNIQUE is used to group.
            // So 1 is the count of the immediate child group.
            CPMCreateQuery_Categorization_Aggregate_ChildCount(WspConsts.System_Size, 1);
        }

        private void CPMCreateQuery_Categorization_Aggregate(CAggregSpec_type_Values aggregateType, CFullPropSpec propSpec, int? comparedValue = null)
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            var categorizationSet = new CCategorizationSet();
            var categorizationSpecs = new List<CCategorizationSpec>();
            categorizationSet.count = 1;
            categorizationSet.categories = new CCategorizationSpec[]{ConstructCategorizationSpec(aggregateType, 1)};

            Site.Log.Add(LogEntryKind.TestStep, $"Client sends CPMCreateQueryIn with aggregate type {aggregateType} and expects success.");
            wspAdapter.CPMCreateQueryIn(
                GetColumnSet(),
                wspAdapter.Builder.GetRestrictionArray("*.bin", Site.Properties.Get("QueryPath") + "Data/CreateQuery_Size", WspConsts.System_FileName),
                CreateSortSets(),
                categorizationSet,
                GetRowsetProperties(),
                GetPidMapper(propSpec),
                new CColumnGroupArray(),
                wspAdapter.Builder.Parameter.LcidValue,
                out CPMCreateQueryOut createQueryResponse);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(createQueryResponse.aCursors[0], MessageBuilder.RowWidth, 1, new CTableColumn[] {  GetTableColumn(propSpec, aggregateType)  });

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            wspAdapter.CPMGetRowsIn(createQueryResponse.aCursors[0], 20, MessageBuilder.RowWidth, wspAdapter.Builder.Parameter.BufferSize, 0, wspAdapter.Builder.Parameter.EType, 0, null, out CPMGetRowsOut response);

            Site.Assert.AreEqual(1U, response._cRowsReturned, "The count of rows returned from server should be 1.");

            if (aggregateType == CAggregSpec_type_Values.DBAGGTTYPE_DATERANGE)
            {
                Site.Assert.AreEqual(CBaseStorageVariant_vType_Values.VT_VECTOR | CBaseStorageVariant_vType_Values.VT_FILETIME,
                    response.Rows[0].Columns[0].RowVariant.vType,
                    "The type of the column should be VT_VECTOR | VT_FILETIME.");
                object[] dateRange = (object[])response.Rows[0].Columns[0].Data;
                Site.Assert.AreEqual(2, dateRange.Length, "The count of date range returned from server should be 2.");
                Site.Log.Add(LogEntryKind.Debug, $"The first date is {dateRange[0]}");
                Site.Log.Add(LogEntryKind.Debug, $"The second date is {dateRange[1]}");
            }
            else
            {
                int data = Convert.ToInt32(response.Rows[0].Columns[0].Data); // Only one row with a single column is returned.
                Site.Assert.AreEqual(comparedValue, data, $"The aggregated value retrieved from the row should be {comparedValue}");
            }
        }

        private void CPMCreateQuery_Categorization_Aggregate_ChildCount(CFullPropSpec propSpec, int comparedValue)
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            // Nested groups with 2 level.
            // Firstly group by Author, then by Size.
            var categorizationSet = new CCategorizationSet();
            var categorizationSpecs = new List<CCategorizationSpec>();
            categorizationSet.count = 2;
            // The first categorization spec does not have a aggregation set.
            categorizationSpecs.Add(ConstructCategorizationSpec(CAggregSpec_type_Values.DBAGGTTYPE_BYNONE, 0));
            categorizationSpecs.Add(ConstructCategorizationSpec(CAggregSpec_type_Values.DBAGGTTYPE_CHILDCOUNT, 1));
            categorizationSet.categories = categorizationSpecs.ToArray();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn with aggregate type DBAGGTTYPE_CHILDCOUNT and expects success.");
            wspAdapter.CPMCreateQueryIn(
                GetColumnSet(),
                wspAdapter.Builder.GetRestrictionArray("*.bin", Site.Properties.Get("QueryPath") + "Data/CreateQuery_Size", WspConsts.System_FileName), 
                CreateSortSets(0, 1), 
                categorizationSet,
                GetRowsetProperties(),
                GetPidMapper(propSpec), 
                new CColumnGroupArray(), 
                wspAdapter.Builder.Parameter.LcidValue,
                out CPMCreateQueryOut createQueryResponse);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn to the first two cursors and expects success.");
            wspAdapter.CPMSetBindingsIn(createQueryResponse.aCursors[0], MessageBuilder.RowWidth, 1, new CTableColumn[] { GetTableColumn(WspConsts.System_Author, CAggregSpec_type_Values.DBAGGTTYPE_BYNONE) });
            wspAdapter.CPMSetBindingsIn(createQueryResponse.aCursors[1], MessageBuilder.RowWidth, 1, new CTableColumn[] { GetTableColumn(propSpec, CAggregSpec_type_Values.DBAGGTTYPE_CHILDCOUNT) });

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn to the first two cursors and expects success.");
            wspAdapter.CPMGetRowsIn(createQueryResponse.aCursors[0], 20, MessageBuilder.RowWidth, wspAdapter.Builder.Parameter.BufferSize, 0, wspAdapter.Builder.Parameter.EType, 0, null, out CPMGetRowsOut response);
            wspAdapter.CPMGetRowsIn(createQueryResponse.aCursors[1], 1, MessageBuilder.RowWidth, wspAdapter.Builder.Parameter.BufferSize, 0, wspAdapter.Builder.Parameter.EType, 1, null, out response);

            Site.Assert.AreEqual(1U, response._cRowsReturned, "The count of rows returned for cursor 2 should be 1.");

            int data = Convert.ToInt32(response.Rows[0].Columns[0].Data); // One row with a single column is returned.
            Site.Assert.AreEqual(comparedValue, data, $"The aggregated value retrieved from the row should be {comparedValue}");
        }

        private CColumnSet GetColumnSet()
        {
            var columnSet = new CColumnSet();
            columnSet.count = 1;
            columnSet.indexes = new uint[] { 0 };

            return columnSet;
        }

        private CPidMapper GetPidMapper(CFullPropSpec propSpec)
        {
            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_Author,
                propSpec,
                WspConsts.System_Search_Scope,
                WspConsts.System_FileName
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            return pidMapper;
        }

        private CRowsetProperties GetRowsetProperties()
        {
            var rowsetProperties = new CRowsetProperties();
            rowsetProperties._uBooleanOptions = CRowsetProperties_uBooleanOptions_Values.eChaptered | CRowsetProperties_uBooleanOptions_Values.eSequential;
            return rowsetProperties;
        }

        private CTableColumn GetTableColumn(CFullPropSpec propSpec, CAggregSpec_type_Values aggregateType)
        {
            var tableColumn = new CTableColumn();
            tableColumn.AggregateType = aggregateType;
            tableColumn.PropSpec = propSpec;
            tableColumn.vType = CBaseStorageVariant_vType_Values.VT_VARIANT;
            tableColumn.ValueSize = Helper.GetSize(tableColumn.vType, wspAdapter.Builder.Is64bit);

            return tableColumn;
        }

        private CCategorizationSpec ConstructCategorizationSpec(CAggregSpec_type_Values aggregateType, uint idColumn)
        {
            // Construct sortKey.
            var sortKey = new CSort();
            sortKey.locale = wspAdapter.Builder.Parameter.LcidValue;
            sortKey.dwOrder = CSort_dwOrder_Values.QUERY_SORTASCEND;
            sortKey.pidColumn = aggregateType == CAggregSpec_type_Values.DBAGGTTYPE_CHILDCOUNT ? idColumn : 0;
            sortKey.dwIndividual = CSort_dwIndividual_Values.QUERY_SORTALL;

            // Construct aggregation set.
            var aggregateSet = new CAggregSet();
            if (aggregateType == CAggregSpec_type_Values.DBAGGTTYPE_BYNONE)
            {
                aggregateSet.cCount = 0;
            }
            else
            {
                aggregateSet.cCount = 1;
                aggregateSet.AggregSpecs = new CAggregSpec[]
                {
                    new CAggregSpec()
                    {
                        type = aggregateType,
                        idColumn = idColumn,
                    }
                };
            }

            // Construct categorization spec
            var categorizationSpec = new CCategorizationSpec()
            {
                _csColumns = new CColumnSet()
                {
                    count = 0
                },
                _Spec = new CCategSpec()
                {
                    _ulCategType = CCategSpec_ulCategType_Values.CATEGORIZE_UNIQUE,
                    _sortKey = sortKey,
                },
                _AggregSet = aggregateSet,
                _SortAggregSet = new CSortAggregSet()
                {
                    cCount = 0
                },
                _InGroupSortAggregSets = new CInGroupSortAggregSets()
                {
                    cCount = 0
                },
                _cMaxResults = 0
            };

            return categorizationSpec;
        }
    }
}