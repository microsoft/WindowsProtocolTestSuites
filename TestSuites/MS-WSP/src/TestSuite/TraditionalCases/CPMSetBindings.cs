// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Microsoft.Protocols.TestSuites.WspTS
{
    [TestClass]
    public partial class CPMSetBindingsTestCases : WspCommonTestBase
    {
        public enum ArgumentType
        {
            AllValid,
            InvalidRowSize,
            InvalidCursor,
            InvalidCColumns,
            InvalidAColumns
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

            wspAdapter.CPMSetBindingsInResponse -= EnsureSuccessfulCPMSetBindingsOut;
            wspAdapter.CPMSetBindingsInResponse += CPMSetBindingsOut;
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }
        #endregion

        #region Test Cases

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("CPMSetBindings")]
        [Description("This test case is designed to test the basic functionality of CPMSetBindings.")]
        public void BVT_CPMSetBindings()
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(true, true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            wspAdapter.CPMGetRowsIn(true);
        }

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("CPMSetBindings")]
        [Description("This test case is designed to test the basic functionality of CPMSetBindings with multiple columns.")]
        public void BVT_CPMSetBindingsWithMultipleColumns()
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            var columnSet = wspAdapter.Builder.GetColumnSet(3);
            var restrictionArray = wspAdapter.Builder.GetRestrictionArray("test106.txt", Site.Properties.Get("QueryPath"), WspConsts.System_FileName);
            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_ItemFolderNameDisplay,
                WspConsts.System_FileExtension,
                WspConsts.System_Search_Scope,
                WspConsts.System_Search_Contents,
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, null, null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn with multiple columns and expects success.");
            var columns = GetTableColumns();
            wspAdapter.CPMSetBindingsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                (uint)wspAdapter.Builder.Parameter.EachRowSize,
                (uint)columns.Length,
                columns
                );

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            wspAdapter.CPMGetRowsIn(out CPMGetRowsOut getRowsOut);

            Site.Assert.AreEqual("test106.txt", getRowsOut.Rows[0].Columns[0].Data.ToString().ToLower(), "The file name should be test106.txt.");
            Site.Assert.AreEqual("test", getRowsOut.Rows[0].Columns[1].Data.ToString().ToLower(), "The name of the folder should be test.");
            Site.Assert.AreEqual(".txt", getRowsOut.Rows[0].Columns[2].Data.ToString().ToLower(), "The file extension of the file should be .txt.");
        }

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("CPMSetBindings")]
        [Description("This test case is designed to verify the server response if the columns of SetBindings contained in CreateQuery")]
        public void BVT_CPMSetBindingsWithColumnsContainedInCreateQuery()
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            var columnSet = wspAdapter.Builder.GetColumnSet(3);
            var restrictionArray = wspAdapter.Builder.GetRestrictionArray(Site.Properties.Get("QueryText"), Site.Properties.Get("QueryPath"), WspConsts.System_Search_Contents);
            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_ItemFolderNameDisplay,
                WspConsts.System_FileExtension,
                WspConsts.System_Search_Scope,
                WspConsts.System_Search_Contents,
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, null, null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn with columns contained in CreateQuery and expects success.");
            wspAdapter.CPMSetBindingsIn(true, true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            wspAdapter.CPMGetRowsIn(true);
        }

        [TestMethod]
        [TestCategory("CPMSetBindings")]
        [Description("This test case is designed to verify the server response if invalid row size is sent in CPMSetBindingsIn.")]
        public void CPMSetBindings_InvalidRowSize()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn with invalid row size and expects NOT SUCCEED.");
            argumentType = ArgumentType.InvalidRowSize;
            wspAdapter.CPMSetBindingsIn(false, true);
            //E_ABORT
        }

        [TestMethod]
        [TestCategory("CPMSetBindings")]
        [Description("This test case is designed to verify the server response if invalid cursor is sent in CPMSetBindingsIn.")]
        public void CPMSetBindings_InvalidCursor()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn with invalid cursor and expects NOT SUCCEED.");
            argumentType = ArgumentType.InvalidCursor;
            wspAdapter.CPMSetBindingsIn(true, false);
            //ERROR_INVALID_STATUS
        }

        [TestMethod]
        [TestCategory("CPMSetBindings")]
        [Description("This test case is designed to verify the server response if invalid cColumns is sent in CPMSetBindingsIn.")]
        public void CPMSetBindings_InvalidCColumns()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn with invalid cColumns and expects NOT SUCCEED.");
            argumentType = ArgumentType.InvalidCColumns;
            var columns = wspAdapter.Builder.GetDefaultTableColumns();
            CTableColumn[] aColumns = columns.Select(column => wspAdapter.Builder.GetTableColumn(column)).ToArray();
            Helper.UpdateTableColumns(aColumns);
            uint cColumns = (uint)(aColumns.Length + 1);
            wspAdapter.CPMSetBindingsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                (uint)wspAdapter.Builder.Parameter.EachRowSize,
                cColumns,
                aColumns
                );
        }

        [TestMethod]
        [TestCategory("CPMSetBindings")]
        [Description("This test case is designed to verify the server response if the columns of SetBindings is not contained in CreateQuery")]
        public void CPMSetBindingsWithColumnsNotContainedInCreateQuery()
        {
            argumentType = ArgumentType.InvalidAColumns;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            var columnSet = wspAdapter.Builder.GetColumnSet(2);
            var restrictionArray = wspAdapter.Builder.GetRestrictionArray(Site.Properties.Get("QueryText"), Site.Properties.Get("QueryPath"), WspConsts.System_Search_Contents);
            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_ItemFolderNameDisplay,
                WspConsts.System_Search_Scope,
                WspConsts.System_Search_Contents,
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, null, null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn with column not contained in CreateQuery and expects NOT SUCCEED.");
            var columns = GetTableColumns();
            wspAdapter.CPMSetBindingsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                (uint)wspAdapter.Builder.Parameter.EachRowSize,
                (uint)columns.Length,
                columns
                );
        }

        #endregion

        private void CPMSetBindingsOut(uint errorCode)
        {
            switch (argumentType)
            {
                case ArgumentType.AllValid:
                    Site.Assert.AreEqual((uint)WspErrorCode.SUCCESS, errorCode, "Server should return succeed for CPMSetBindingsIn.");
                    break;
                case ArgumentType.InvalidRowSize:
                    Site.Assert.AreNotEqual((uint)WspErrorCode.SUCCESS, errorCode, "Server should not return succeed if row size of CPMSetBindingsIn is invalid.");
                    break;
                case ArgumentType.InvalidCursor:
                    Site.Assert.AreEqual((uint)WspErrorCode.ERROR_INVALID_PARAMETER, errorCode, "Server should return ERROR_INVALID_PARAMETER if the cursor of CPMSetBindingsIn is invalid.");
                    break;
                case ArgumentType.InvalidCColumns:
                    Site.Assert.AreNotEqual((uint)WspErrorCode.SUCCESS, errorCode, "Server should not return succeed if the cColumns of CPMSetBindingsIn is invalid.");
                    break;
                case ArgumentType.InvalidAColumns:
                    Site.Assert.AreNotEqual((uint)WspErrorCode.SUCCESS, errorCode, "Server should not return succeed if the columns of SetBindings is not contained in CreateQuery.");
                    break;
            }
        }

        private CTableColumn[] GetTableColumns()
        {
            CTableColumn[] columns = new CTableColumn[] { };
            switch (argumentType)
            {
                case ArgumentType.InvalidAColumns:
                    columns = new CTableColumn[]
                    {
                        wspAdapter.Builder.GetTableColumn(WspConsts.System_FileExtension, CBaseStorageVariant_vType_Values.VT_VARIANT)
                    };
                    break;
                case ArgumentType.AllValid:
                    columns = new CTableColumn[]
                    {
                        wspAdapter.Builder.GetTableColumn(WspConsts.System_ItemName, CBaseStorageVariant_vType_Values.VT_VARIANT),
                        wspAdapter.Builder.GetTableColumn(WspConsts.System_ItemFolderNameDisplay, CBaseStorageVariant_vType_Values.VT_VARIANT),
                        wspAdapter.Builder.GetTableColumn(WspConsts.System_FileExtension, CBaseStorageVariant_vType_Values.VT_VARIANT)
                    };
                    break;
            }

            return columns;
        }
    }
}
