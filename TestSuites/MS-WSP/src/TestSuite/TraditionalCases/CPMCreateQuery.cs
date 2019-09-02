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
    public partial class CPMCreateQueryTestCases : TestClassBase
    {
        private WspAdapter wspAdapter;

        public enum ArgumentType
        {
            AllValid,
            ColumnSetAbsent,
            RestrictionArrayAbsent,
            PidMapperAbsent
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
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test the basic functionality of CPMCreateQuery.")]
        public void BVT_CPMCreateQuery()
        {
            argumentType = ArgumentType.AllValid;
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

            var restrictionArray = wspAdapter.builder.GetRestrictionArray(Site.Properties.Get("QueryText"), Site.Properties.Get("QueryPath"));

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

            var restrictionArray = wspAdapter.builder.GetRestrictionArray(Site.Properties.Get("QueryText"), Site.Properties.Get("QueryPath"));

            argumentType = ArgumentType.PidMapperAbsent;
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, null, new CCategorizationSet(), new CRowsetProperties(), new CPidMapper(), new CColumnGroupArray(), wspAdapter.builder.parameter.LCID_VALUE);
        }

        #endregion


        private void CPMSetBindingsOut(uint errorCode)
        {
            Site.Assert.AreEqual((uint)WspErrorCode.SUCCESS, errorCode, "CPMSetBindingsIn should succeed.");
        }

        private void CPMConnectOut(uint errorCode)
        {
            Site.Assert.AreEqual((uint)WspErrorCode.SUCCESS, errorCode, "CPMConnectIn should succeed.");
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
