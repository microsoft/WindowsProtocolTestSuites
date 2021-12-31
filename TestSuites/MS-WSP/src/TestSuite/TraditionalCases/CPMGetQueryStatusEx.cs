// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.WspTS
{
    [TestClass]
    public partial class CPMGetQueryStatusExTestCases : WspCommonTestBase
    {
        private bool isCursorValid = true;

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

            wspAdapter.CPMGetQueryStatusExOutResponse -= EnsureSuccessfulCPMGetQueryStatusExOut;
            wspAdapter.CPMGetQueryStatusExOutResponse += CPMGetQueryStatusExOut;
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }
        #endregion

        #region Test Cases

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("CPMGetQueryStatusEx")]
        [Description("This test case is designed to test the server response if CPMGetQueryStatusExIn is sent after CPMCreateQueryIn.")]
        public void BVT_CPMGetQueryStatusEx_AfterCPMCreateQueryIn()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            var columnSet = wspAdapter.Builder.GetColumnSet(2);
            var restrictionArray = wspAdapter.Builder.GetRestrictionArray("*.bin", Site.Properties.Get("QueryPath") + "Data/CreateQuery_Size", WspConsts.System_FileName);
            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_Size,
                WspConsts.System_Search_Scope,
                WspConsts.System_FileName,
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, null, null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetQueryStatusIn and expects success.");
            wspAdapter.CPMGetQueryStatusExIn(out var response);

            ValidateCPMGetQueryStatusExOut(response);
        }

        [TestMethod]
        [TestCategory("CPMGetQueryStatusEx")]
        [Description("This test case is designed to verify the server response if CPMGetQueryStatusIn is sent after CPMSetBindingsIn.")]
        public void CPMGetQueryStatusEx_AfterCPMSetBindingsIn()
        {

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            var columnSet = wspAdapter.Builder.GetColumnSet(2);
            var restrictionArray = wspAdapter.Builder.GetRestrictionArray("*.bin", Site.Properties.Get("QueryPath") + "Data/CreateQuery_Size", WspConsts.System_FileName);
            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_Size,
                WspConsts.System_Search_Scope,
                WspConsts.System_FileName,
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, null, null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);

            var columns = new CTableColumn[]
            {
                wspAdapter.Builder.GetTableColumn(WspConsts.System_ItemName, CBaseStorageVariant_vType_Values.VT_VARIANT),
                wspAdapter.Builder.GetTableColumn(WspConsts.System_Size, CBaseStorageVariant_vType_Values.VT_VARIANT)
            };

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(columns);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetQueryStatusIn and expects success.");
            wspAdapter.CPMGetQueryStatusExIn(out var response);

            ValidateCPMGetQueryStatusExOut(response);
        }

        [TestMethod]
        [TestCategory("CPMGetQueryStatusEx")]
        [Description("This test case is designed to verify the server response if CPMGetQueryStatusExIn is sent after CPMGetRowsIn.")]
        public void CPMGetQueryStatusEx_AfterCPMGetRowsIn()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            var columnSet = wspAdapter.Builder.GetColumnSet(2);
            var restrictionArray = wspAdapter.Builder.GetRestrictionArray("*.bin", Site.Properties.Get("QueryPath") + "Data/CreateQuery_Size", WspConsts.System_FileName);
            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_Size,
                WspConsts.System_Search_Scope,
                WspConsts.System_FileName,
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, null, null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);

            var columns = new CTableColumn[]
            {
                wspAdapter.Builder.GetTableColumn(WspConsts.System_ItemName, CBaseStorageVariant_vType_Values.VT_VARIANT),
                wspAdapter.Builder.GetTableColumn(WspConsts.System_Size, CBaseStorageVariant_vType_Values.VT_VARIANT)
            };

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(columns);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            wspAdapter.CPMGetRowsIn(true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetQueryStatusIn and expects success.");
            wspAdapter.CPMGetQueryStatusExIn(out var response);

            ValidateCPMGetQueryStatusExOut(response, isAfterGetRowsIn: true);
        }

        [TestMethod]
        [TestCategory("CPMGetQueryStatusEx")]
        [Description("This test case is designed to test the server response if CPMGetQueryStatusExIn is sent when the client does not request expensive properties to be computed.")]
        public void CPMGetQueryStatusEx_NoExpensiveProps()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            var columnSet = wspAdapter.Builder.GetColumnSet(2);
            var restrictionArray = wspAdapter.Builder.GetRestrictionArray("*.bin", Site.Properties.Get("QueryPath") + "Data/CreateQuery_Size", WspConsts.System_FileName);
            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_Size,
                WspConsts.System_Search_Scope,
                WspConsts.System_FileName,
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            var rowsetProperties = new CRowsetProperties();
            rowsetProperties._uBooleanOptions = CRowsetProperties_uBooleanOptions_Values.eDoNotComputeExpensiveProps;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, null, null, rowsetProperties, pidMapper, new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetQueryStatusIn and expects success.");
            wspAdapter.CPMGetQueryStatusExIn(out var response);

            ValidateCPMGetQueryStatusExOut(response, computeExpensiveProps: false);
        }

        [TestMethod]
        [TestCategory("CPMGetQueryStatusEx")]
        [Description("This test case is designed to verify the server response if invalid cursor is sent in CPMGetQueryStatusExIn.")]
        public void CPMGetQueryStatusEx_InvalidCursor()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(false);

            isCursorValid = false;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetQueryStatusIn and expects ERROR_INVALID_PARAMETER.");
            wspAdapter.CPMGetQueryStatusExIn(false);
        }

        #endregion

        private void CPMGetQueryStatusExOut(uint errorCode)
        {
            if (isCursorValid)
            {
                Site.Assert.AreEqual((uint)WspErrorCode.SUCCESS, errorCode, "CPMGetQueryStatusExIn should succeed.");
            }
            else
            {
                Site.Assert.AreEqual((uint)WspErrorCode.ERROR_INVALID_PARAMETER, errorCode, "Server should return ERROR_INVALID_PARAMETER if invalid curosr is sent in CPMGetQueryStatusExIn.");
            }
        }

        private void ValidateCPMGetQueryStatusExOut(CPMGetQueryStatusExOut response, bool isAfterGetRowsIn = false, bool computeExpensiveProps = true)
        {
            if (response._QStatus.HasFlag(QStatus_Values.STAT_DONE))
            {
                Site.Assert.AreEqual(3U, response._dwRatioFinishedDenominator, "The _dwRatioFinishedDenominator should be 3.");

                if (isAfterGetRowsIn)
                {
                    Site.Assert.AreEqual(3U, response._dwRatioFinishedNumerator, "The _dwRatioFinishedNumerator should be 3.");
                    Site.Assert.AreEqual(1U, response._iRowBmk, "The _iRowBmk should be 1.");
                }

                if (computeExpensiveProps)
                {
                    Site.Assert.AreEqual(3U, response._cRowsTotal, "The _cRowsTotal should be 3.");
                    Site.Assert.AreEqual(3U, response._cResultsFound, "The _cResultsFound should be 3.");
                }
                else
                {
                    Site.Assert.AreEqual(0U, response._cRowsTotal, "The _cRowsTotal should be 0 if the client does not request expensive properties to be computed.");
                    Site.Assert.AreEqual(0U, response._cResultsFound, "The _cResultsFound should be 0 if the client does not request expensive properties to be computed.");
                }
            }
        }
    }
}
