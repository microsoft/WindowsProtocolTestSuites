// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP.Adapter;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP;

namespace Microsoft.Protocols.TestSuites.WspTS
{
    [TestClass]
    public partial class CPMCreateQueryTestCases : WspCommonTestBase
    {
        private WspAdapter wspAdapter;

        public enum ArgumentType
        {
            AllValid,
            ColumnSetAbsent,
            RestrictionArrayAbsent,
            PidMapperAbsent,
        }
        private ArgumentType argumentType;

        #region Test Initialize and Cleanup
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }
        #endregion

        #region Test Case Initialize and Cleanup
        protected override void TestInitialize()
        {
            base.TestInitialize();
            wspAdapter = new WspAdapter();
            wspAdapter.Initialize(this.Site);

            wspAdapter.CPMConnectOutResponse += EnsureSuccessfulCPMConnectOut;
            wspAdapter.CPMSetBindingsInResponse += EnsureSuccessfulCPMSetBindingsOut;

            wspAdapter.CPMCreateQueryOutResponse += CPMCreateQueryOut;
            wspAdapter.CPMGetRowsOut += CPMGetRowsOut;
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }
        #endregion

        #region Test Cases

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test the basic functionality of CPMCreateQuery.")]
        public void BVT_CPMCreateQuery()
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectInRequest();

            var columnSet = wspAdapter.builder.GetColumnSet(2);
            var restrictionArray = wspAdapter.builder.GetRestrictionArray(Site.Properties.Get("QueryText"), Site.Properties.Get("QueryPath"), WspConsts.System_Search_Contents);
            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_ItemFolderNameDisplay,
                WspConsts.System_Search_Scope,
                WspConsts.System_FileName,
            };
            pidMapper.count = (UInt32)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, null, null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.builder.parameter.LCID_VALUE);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(true, true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            wspAdapter.CPMGetRowsIn(true);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to verify the server response if no ColumnSet is sent in CPMCreateQueryIn.")]
        public void CPMCreateQuery_ColumnSetAbsent()
        {

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectInRequest();

            var restrictionArray = wspAdapter.builder.GetRestrictionArray(Site.Properties.Get("QueryText"), Site.Properties.Get("QueryPath"), WspConsts.System_Search_Contents);

            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_ItemFolderNameDisplay,
                WspConsts.System_Search_Scope,
                WspConsts.System_Search_Contents,
            };
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn without ColumnSet.");
            argumentType = ArgumentType.ColumnSetAbsent;
            wspAdapter.CPMCreateQueryIn(null, restrictionArray, null, new CCategorizationSet(), new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.builder.parameter.LCID_VALUE);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to verify the server response if no RestrictionArray is sent in CPMCreateQueryIn.")]
        public void CPMCreateQuery_RestrictionArrayAbsent()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectInRequest();

            var columnSet = wspAdapter.builder.GetColumnSet(2);
            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_ItemFolderNameDisplay,
                WspConsts.System_Search_Scope,
                WspConsts.System_Search_Contents,
            };

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn without RestrictionArray.");
            argumentType = ArgumentType.RestrictionArrayAbsent;
            wspAdapter.CPMCreateQueryIn(columnSet, null, null, new CCategorizationSet(), new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.builder.parameter.LCID_VALUE);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to verify the server response if no PidMapper is sent in CPMCreateQueryIn.")]
        public void CPMCreateQuery_PidMapperAbsent()
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectInRequest();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn without PidMapper.");

            var columnSet = wspAdapter.builder.GetColumnSet(2);

            var restrictionArray = wspAdapter.builder.GetRestrictionArray(Site.Properties.Get("QueryText"), Site.Properties.Get("QueryPath"), WspConsts.System_FileName);

            argumentType = ArgumentType.PidMapperAbsent;
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, null, new CCategorizationSet(), new CRowsetProperties(), new CPidMapper(), new CColumnGroupArray(), wspAdapter.builder.parameter.LCID_VALUE);
        }

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if image info can be retrieved by CPMCreateQuery.")]
        public void BVT_CPMCreateQuery_QueryImage()
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectInRequest();

            var columnSet = wspAdapter.builder.GetColumnSet(2);
            var restrictionArray = wspAdapter.builder.GetRestrictionArray("*.png", Site.Properties.Get("QueryPath"), WspConsts.System_FileName);
            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_Image_HorizontalSize,
                WspConsts.System_Search_Scope,
                WspConsts.System_FileName,
            };
            pidMapper.count = (UInt32)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, null, null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.builder.parameter.LCID_VALUE);

            var columns = new CTableColumn[]
            {
                wspAdapter.builder.GetTableColumn(WspConsts.System_ItemName, vType_Values.VT_VARIANT),
                wspAdapter.builder.GetTableColumn(WspConsts.System_Image_HorizontalSize, vType_Values.VT_VARIANT)
            };

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(columns);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            argumentType = ArgumentType.AllValid;
            wspAdapter.CPMGetRowsIn(out CPMGetRowsOut getRowsOut);

            Site.Assert.AreEqual((uint)1, getRowsOut._cRowsReturned, "The number of image file should be 1.");
            Site.Assert.AreEqual("test.png", getRowsOut.Rows[0].Columns[0].Data, "The name of the image file should be test.png.");
            Site.Assert.AreEqual(1279, Convert.ToInt32(getRowsOut.Rows[0].Columns[1].Data), "The HorizontalSize of the image file should be 1279.");
        }

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if the create date of the file can be retrieved by CPMCreateQuery.")]
        public void BVT_CPMCreateQuery_QueryDateCreated()
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectInRequest();

            var columnSet = wspAdapter.builder.GetColumnSet(2);
            var restrictionArray = wspAdapter.builder.GetRestrictionArray("test106.txt", Site.Properties.Get("QueryPath"), WspConsts.System_FileName);
            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_DateCreated,
                WspConsts.System_Search_Scope,
                WspConsts.System_FileName,
            };
            pidMapper.count = (UInt32)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, null, null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.builder.parameter.LCID_VALUE);

            var columns = new CTableColumn[]
            {
                wspAdapter.builder.GetTableColumn(WspConsts.System_ItemName, vType_Values.VT_VARIANT),
                wspAdapter.builder.GetTableColumn(WspConsts.System_DateCreated, vType_Values.VT_VARIANT)
            };

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(columns);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            argumentType = ArgumentType.AllValid;
            wspAdapter.CPMGetRowsIn(out CPMGetRowsOut getRowsOut);

            Site.Assert.AreEqual((uint)1, getRowsOut._cRowsReturned, "The number of row returned should be 1.");
            Site.Assert.AreEqual("test106.txt", getRowsOut.Rows[0].Columns[0].Data.ToString().ToLower(), "The file name should be test106.txt.");
            Site.Assert.IsTrue(getRowsOut.Rows[0].Columns[1].Data is DateTime, "The second column of the row should be in DateTime format.");
            Site.Log.Add(LogEntryKind.Debug, "The create date is {0}", getRowsOut.Rows[0].Columns[1].Data);
        }


        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if the size of the files can be retrieved in ascend order by CPMCreateQuery.")]
        public void BVT_CPMCreateQuery_SortAscend()
        {
            CPMCreateQuery_Sort(ascend: true);
        }

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if the size of the files can be retrieved in descend order by CPMCreateQuery.")]
        public void BVT_CPMCreateQuery_SortDescend()
        {
            CPMCreateQuery_Sort(ascend: false);
        }

        #endregion


        private void CPMCreateQuery_Sort(bool ascend)
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectInRequest();

            var columnSet = wspAdapter.builder.GetColumnSet(2);
            var restrictionArray = wspAdapter.builder.GetRestrictionArray("*.txt", Site.Properties.Get("QueryPath") + "Data/CreateQuery_Size", WspConsts.System_FileName);
            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_Size,
                WspConsts.System_Search_Scope,
                WspConsts.System_FileName,
            };
            pidMapper.count = (UInt32)pidMapper.aPropSpec.Length;

            CInGroupSortAggregSets inGroupSortAggregSets = new CInGroupSortAggregSets();
            inGroupSortAggregSets.cCount = 1;
            inGroupSortAggregSets.SortSets = new CSortSet[1];
            inGroupSortAggregSets.SortSets[0].count = 1;
            inGroupSortAggregSets.SortSets[0].sortArray = new CSort[1];
            inGroupSortAggregSets.SortSets[0].sortArray[0].dwOrder = ascend ? dwOrder_Values.QUERY_SORTASCEND : dwOrder_Values.QUERY_DESCEND;
            inGroupSortAggregSets.SortSets[0].sortArray[0].pidColumn = 1; // Sort by Size.

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, inGroupSortAggregSets, null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.builder.parameter.LCID_VALUE);

            var columns = new CTableColumn[]
            {
                wspAdapter.builder.GetTableColumn(WspConsts.System_ItemName, vType_Values.VT_VARIANT),
                wspAdapter.builder.GetTableColumn(WspConsts.System_Size, vType_Values.VT_VARIANT)
            };

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(columns);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            argumentType = ArgumentType.AllValid;
            wspAdapter.CPMGetRowsIn(out CPMGetRowsOut getRowsOut);

            Site.Assert.AreEqual((uint)3, getRowsOut._cRowsReturned, "Number of rows returned should be 3.");

            string[] fileNameList = null;
            int[] sizeList = null;
            if (ascend)
            {
                fileNameList = new string[] { "test27.txt", "test1.txt", "test132.txt" };
                sizeList = new int[] { 30, 1124, 3868 };
            }
            else
            {
                fileNameList = new string[] { "test132.txt", "test1.txt", "test27.txt" };
                sizeList = new int[] { 3868, 1124, 30 };
            }

            for (int i = 0; i < 3; i++)
            {
                Site.Assert.AreEqual(fileNameList[i], getRowsOut.Rows[i].Columns[0].Data, "The index {0} file in Ascend order should be {1}.", i, fileNameList[i]);
                Site.Assert.AreEqual(sizeList[i], Convert.ToInt32(getRowsOut.Rows[i].Columns[1].Data), "The size of {0} should be {1} bytes.", fileNameList[i], sizeList[i]);
            }
        }

        private void CPMCreateQueryOut(uint errorCode)
        {
            switch (argumentType)
            {
                case ArgumentType.AllValid:
                    Site.Assert.AreEqual((uint)WspErrorCode.SUCCESS, errorCode, "CPMCreateQueryOut should return SUCCESS if everything is valid.");
                    break;
                case ArgumentType.ColumnSetAbsent:
                    Site.Assert.AreEqual((uint)WspErrorCode.ERROR_INVALID_PARAMETER, errorCode, "CPMCreateQueryOut should return ERROR_INVALID_PARAMETER if no ColumnSet is sent in CPMCreateQueryIn.");
                    break;
                case ArgumentType.RestrictionArrayAbsent:
                    Site.Assert.AreEqual((uint)WspErrorCode.ERROR_INVALID_PARAMETER, errorCode, "CPMCreateQueryOut should return ERROR_INVALID_PARAMETER if no RestrictionArray is sent in CPMCreateQueryIn.");
                    break;
                case ArgumentType.PidMapperAbsent:
                    Site.Assert.AreEqual((uint)WspErrorCode.ERROR_INVALID_PARAMETER, errorCode, "CPMCreateQueryOut should return ERROR_INVALID_PARAMETER if no PidMapper is sent in CPMCreateQueryIn.");
                    break;
            }
        }

        private void CPMGetRowsOut(uint errorCode)
        {
            switch (argumentType)
            {
                case ArgumentType.AllValid:
                    bool succeed = errorCode == (uint)WspErrorCode.SUCCESS || errorCode == (uint)WspErrorCode.DB_S_ENDOFROWSET ? true : false;
                    Site.Assert.IsTrue(succeed, "Server should return succeed or DB_S_ENDOFROWSET for CPMGetRowsIn.");
                    break;
            }
        }
    }
}
