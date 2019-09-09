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
    public partial class CPMSetBindingsTestCases : TestClassBase
    {
        private WspAdapter wspAdapter;

        public enum ArgumentType
        {
            AllValid,
            InvalidRowSize,
            InvalidCursor,
            InvalidcColumns,
            InvalidaColumns
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
            wspAdapter.CPMConnectOutResponse += CPMConnectOut;
            wspAdapter.CPMSetBindingsInResponse += CPMSetBindingsOut;
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
        [TestCategory("CPMSetBindings")]
        [Description("This test case is designed to test the basic functionality of CPMSetBindings.")]
        public void BVT_CPMSetBindings()
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectInRequest();

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
            wspAdapter.CPMConnectInRequest();

            var columnSet = wspAdapter.builder.GetColumnSet(3);
            var restrictionArray = wspAdapter.builder.GetRestrictionArray(Site.Properties.Get("QueryText"), Site.Properties.Get("QueryPath"));
            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_ItemFolderNameDisplay,
                WspConsts.System_FileExtension,
                WspConsts.System_Search_Scope,
                WspConsts.System_Search_Contents,
            };
            pidMapper.count = (UInt32)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, null, null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.builder.parameter.LCID_VALUE);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn with multiple columns and expects success.");
            var columns = GetTableColumns();
            CTableColumn[] aColumns = columns.Select(column => wspAdapter.builder.GetTableColumn(column)).ToArray();
            Helper.UpdateTableColumns(aColumns);
            uint cColumns = (uint)aColumns.Length;
            wspAdapter.CPMSetBindingsIn(
                wspAdapter.GetCursor(wspAdapter.clientMachineName),
                (uint)wspAdapter.builder.parameter.EachRowSize,
                cColumns,
                aColumns
                );

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            wspAdapter.CPMGetRowsIn(true);
        }

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("CPMSetBindings")]
        [Description("This test case is designed to verify the server response if the columns of SetBindings contained in CreateQuery")]
        public void BVT_CPMSetBindingsWithColumnsContainedInCreateQuery()
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectInRequest();

            var columnSet = wspAdapter.builder.GetColumnSet(3);
            var restrictionArray = wspAdapter.builder.GetRestrictionArray(Site.Properties.Get("QueryText"), Site.Properties.Get("QueryPath"));
            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_ItemFolderNameDisplay,
                WspConsts.System_FileExtension,
                WspConsts.System_Search_Scope,
                WspConsts.System_Search_Contents,
            };
            pidMapper.count = (UInt32)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, null, null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.builder.parameter.LCID_VALUE);

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
            wspAdapter.CPMConnectInRequest();

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
            wspAdapter.CPMConnectInRequest();

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
        public void CPMSetBindings_InvalidcColumns()
        {

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectInRequest();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn with invalid cColumns and expects NOT SUCCEED.");
            argumentType = ArgumentType.InvalidcColumns;
            var columns = wspAdapter.builder.GetDefaultTableColumns();
            CTableColumn[] aColumns = columns.Select(column => wspAdapter.builder.GetTableColumn(column)).ToArray();
            Helper.UpdateTableColumns(aColumns);
            uint cColumns = (uint)(aColumns.Length + 1);
            wspAdapter.CPMSetBindingsIn(
                wspAdapter.GetCursor(wspAdapter.clientMachineName),
                (uint)wspAdapter.builder.parameter.EachRowSize,
                cColumns,
                aColumns
                );
        }

        [TestMethod]
        [TestCategory("CPMSetBindings")]
        [Description("This test case is designed to verify the server response if the columns of SetBindings is not contained in CreateQuery")]
        public void CPMSetBindingsWithColumnsNotContainedInCreateQuery()
        {
            argumentType = ArgumentType.InvalidaColumns;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectInRequest();

            var columnSet = wspAdapter.builder.GetColumnSet(2);
            var restrictionArray = wspAdapter.builder.GetRestrictionArray(Site.Properties.Get("QueryText"), Site.Properties.Get("QueryPath"));
            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_ItemFolderNameDisplay,
                WspConsts.System_Search_Scope,
                WspConsts.System_Search_Contents,
            };
            pidMapper.count = (UInt32)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, null, null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.builder.parameter.LCID_VALUE);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn with column not contained in CreateQuery and expects NOT SUCCEED.");
            var columns = GetTableColumns();
            CTableColumn[] aColumns = columns.Select(column => wspAdapter.builder.GetTableColumn(column)).ToArray();
            Helper.UpdateTableColumns(aColumns);
            uint cColumns = (uint)aColumns.Length;
            wspAdapter.CPMSetBindingsIn(
                wspAdapter.GetCursor(wspAdapter.clientMachineName),
                (uint)wspAdapter.builder.parameter.EachRowSize,
                cColumns,
                aColumns
                );
        }

        #endregion

        private void CPMConnectOut(uint errorCode)
        {
            Site.Assert.AreEqual((uint)WspErrorCode.SUCCESS, errorCode, "CPMConnectIn should succeed.");
        }

        private void CPMCreateQueryOut(uint errorCode)
        {
            Site.Assert.AreEqual((uint)WspErrorCode.SUCCESS, errorCode, "CPMCreateQueryIn should succeed.");
        }

        private void CPMGetRowsOut(uint errorCode)
        {
            bool succeed = errorCode == (uint)WspErrorCode.SUCCESS || errorCode == (uint)WspErrorCode.DB_S_ENDOFROWSET ? true : false;
            Site.Assert.IsTrue(succeed, "Server should return succeed or DB_S_ENDOFROWSET for CPMGetRowsIn.");
        }

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
                case ArgumentType.InvalidcColumns:
                    Site.Assert.AreNotEqual((uint)WspErrorCode.SUCCESS, errorCode, "Server should not return succeed if the cColumns of CPMSetBindingsIn is invalid.");
                    break;
                case ArgumentType.InvalidaColumns:
                    Site.Assert.AreNotEqual((uint)WspErrorCode.SUCCESS, errorCode, "Server should not return succeed if the columns of SetBindings is not contained in CreateQuery.");
                    break;
            }
        }

        private TableColumn[] GetTableColumns()
        {
            TableColumn[] columns = null;
            switch (argumentType)
            {               
                case ArgumentType.InvalidaColumns:
                    columns = new TableColumn[]
                    {
                        new TableColumn()
                        {
                            Property = WspConsts.System_FileExtension,
                            Type = vType_Values.VT_VARIANT,
                        }
                    };
                    break;
                case ArgumentType.AllValid:
                    columns = new TableColumn[]
                    {
                        new TableColumn()
                        {
                            Property = WspConsts.System_ItemName,
                            Type = vType_Values.VT_VARIANT,
                        },
                        new TableColumn()
                        {
                            Property = WspConsts.System_ItemFolderNameDisplay,
                            Type = vType_Values.VT_VARIANT,
                        },
                        new TableColumn()
                        {
                            Property = WspConsts.System_FileExtension,
                            Type = vType_Values.VT_VARIANT,
                        },
                    };
                    break;
            }

            return columns;
        }
    }
}
