// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.WspTS
{
    [TestClass]
    public partial class CPMCiStateTestCases : WspCommonTestBase
    {
        private bool isClientConnected = true;

        #region Test Class Initialize and Cleanup
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

            wspAdapter.CPMCiStateInOutResponse += CPMCiStateInOut;
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }
        #endregion

        #region Test Cases
        [TestMethod]
        [TestCategory("CPMCiState")]
        [Description("This test case is designed to test the server response if CPMCiStateInOut is sent when the client is not connected.")]
        public void CPMCiState_NotConnected()
        {
            isClientConnected = false;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCiStateInOut and expects STATUS_INVALID_PARAMETER.");
            wspAdapter.CPMCiStateInOut();
        }

        [TestMethod]
        [TestCategory("CPMCiState")]
        [Description("This test case is designed to test the server response if CPMCiStateInOut is sent after CPMConnectIn.")]
        public void CPMCiState_AfterConnect()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCiStateInOut and expects success.");
            wspAdapter.CPMCiStateInOut();
        }

        [TestMethod]
        [TestCategory("CPMCiState")]
        [Description("This test case is designed to test the server response if CPMCiStateInOut is sent after CPMCreateQueryIn.")]
        public void CPMCiState_AfterQuery()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(false);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCiStateInOut and expects success.");
            wspAdapter.CPMCiStateInOut();
        }

        [TestMethod]
        [TestCategory("CPMCiState")]
        [Description("This test case is designed to test the server response if CPMCiStateInOut is sent after CPMSetBindingsIn.")]
        public void CPMCiState_AfterBinding()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(false);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(true, true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCiStateInOut and expects success.");
            wspAdapter.CPMCiStateInOut();
        }

        [TestMethod]
        [TestCategory("CPMCiState")]
        [Description("This test case is designed to test the server response if CPMCiStateInOut is sent after CPMGetRowsIn.")]
        public void CPMCiState_AfterGetRows()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(false);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(true, true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            wspAdapter.CPMGetRowsIn(true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCiStateInOut and expects success.");
            wspAdapter.CPMCiStateInOut();
        }

        [TestMethod]
        [TestCategory("CPMCiState")]
        [Description("This test case is designed to test the server response if CPMCiStateIn is sent after CPMFreeCursorIn.")]
        public void CPMCiState_AfterFreeCursor()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(false);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(true, true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            wspAdapter.CPMGetRowsIn(true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMFreeCursorIn and expects success.");
            wspAdapter.CPMFreeCursorIn(true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCiStateInOut and expects success.");
            wspAdapter.CPMCiStateInOut();
        }
        #endregion

        private void CPMCiStateInOut(uint errorCode)
        {
            if (isClientConnected)
            {
                Site.Assert.AreEqual((uint)WspErrorCode.SUCCESS, errorCode, "Server should return SUCCESS for CPMCiStateInOut.");
            }
            else
            {
                Site.Assert.AreEqual((uint)WspErrorCode.STATUS_INVALID_PARAMETER, errorCode, "If the handle is not present, the server MUST report a STATUS_INVALID_PARAMETER error.");
            }
        }
    }
}
