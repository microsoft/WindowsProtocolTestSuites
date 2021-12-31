// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.WspTS
{
    [TestClass]
    public partial class CPMGetQueryStatusTestCases : WspCommonTestBase
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

            wspAdapter.CPMGetQueryStatusOutResponse -= EnsureSuccessfulCPMGetQueryStatusOut;
            wspAdapter.CPMGetQueryStatusOutResponse += CPMGetQueryStatusOut;
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }
        #endregion

        #region Test Cases

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("CPMGetQueryStatus")]
        [Description("This test case is designed to verify the server response if CPMGetQueryStatusIn is sent after CPMCreateQueryIn.")]
        public void BVT_CPMGetQueryStatus_AfterCPMCreateQueryIn()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(false);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetQueryStatusIn and expects success.");
            wspAdapter.CPMGetQueryStatusIn(true);
        }

        [TestMethod]
        [TestCategory("CPMGetQueryStatus")]
        [Description("This test case is designed to verify the server response if CPMGetQueryStatusIn is sent after CPMSetBindingsIn.")]
        public void CPMGetQueryStatus_AfterCPMSetBindingsIn()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(false);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(true, true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetQueryStatusIn and expects success.");
            wspAdapter.CPMGetQueryStatusIn(true);
        }

        [TestMethod]
        [TestCategory("CPMGetQueryStatus")]
        [Description("This test case is designed to verify the server response if CPMGetQueryStatusIn is sent after CPMGetRowsIn.")]
        public void CPMGetQueryStatus_AfterCPMGetRowsIn()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(false);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(true, true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            wspAdapter.CPMGetRowsIn(true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetQueryStatusIn and expects success.");
            wspAdapter.CPMGetQueryStatusIn(true);
        }

        [TestMethod]
        [TestCategory("CPMGetQueryStatus")]
        [Description("This test case is designed to verify the server response if invalid cursor is sent in CPMGetQueryStatusIn.")]
        public void CPMGetQueryStatus_InvalidCursor()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(false);

            isCursorValid = false;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetQueryStatusIn and expects ERROR_INVALID_PARAMETER.");
            wspAdapter.CPMGetQueryStatusIn(false);
        }

        #endregion

        private void CPMGetQueryStatusOut(uint errorCode)
        {
            if (isCursorValid)
            {
                Site.Assert.AreEqual((uint)WspErrorCode.SUCCESS, errorCode, "CPMGetQueryStatusIn should succeed.");
            }
            else
            {
                Site.Assert.AreEqual((uint)WspErrorCode.ERROR_INVALID_PARAMETER, errorCode, "Server should return ERROR_INVALID_PARAMETER if invalid curosr is sent in CPMGetQueryStatusIn.");
            }
        }
    }
}
