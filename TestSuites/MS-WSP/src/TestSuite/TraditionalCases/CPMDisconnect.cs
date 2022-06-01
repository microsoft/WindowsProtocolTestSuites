// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.WspTS
{
    [TestClass]
    public partial class CPMDisconnectTestCases : WspCommonTestBase
    {
        private const uint STATUS_PIPE_DISCONNECTED = 0xC00000B0;

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
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }
        #endregion

        #region Test Cases
        [TestMethod]
        [TestCategory("CPMDisconnect")]
        [Description("This test case is designed to test that CPMDisconnect can be sent when the client is not connected.")]
        public void CPMDisconnect_NotConnected()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMDisconnect.");
            wspAdapter.CPMDisconnect();
        }

        [TestMethod]
        [TestCategory("CPMDisconnect")]
        [Description("This test case is designed to test that CPMDisconnect can be sent after CPMConnectIn.")]
        public void CPMDisconnect_AfterConnect()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMDisconnect.");
            wspAdapter.CPMDisconnect();
        }

        [TestMethod]
        [TestCategory("CPMDisconnect")]
        [Description("This test case is designed to test that CPMDisconnect can be sent after CPMCreateQueryIn.")]
        public void CPMDisconnect_AfterQuery()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(false);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMDisconnect.");
            wspAdapter.CPMDisconnect();
        }

        [TestMethod]
        [TestCategory("CPMDisconnect")]
        [Description("This test case is designed to test that CPMDisconnect can be sent after CPMSetBindingsIn.")]
        public void CPMDisconnect_AfterBinding()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(false);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(true, true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMDisconnect.");
            wspAdapter.CPMDisconnect();
        }

        [TestMethod]
        [TestCategory("CPMDisconnect")]
        [Description("This test case is designed to test that CPMDisconnect can be sent after CPMGetRowsIn.")]
        public void CPMDisconnect_AfterGetRows()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(false);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(true, true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            wspAdapter.CPMGetRowsIn(true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMDisconnect.");
            wspAdapter.CPMDisconnect();
        }

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("CPMDisconnect")]
        [Description("This test case is designed to test that CPMDisconnect can be sent after CPMFreeCursorIn.")]
        public void BVT_CPMDisconnect_AfterFreeCursor()
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

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMDisconnect.");
            wspAdapter.CPMDisconnect();
        }

        [TestMethod]
        [TestCategory("CPMDisconnect")]
        [Description("This test case is designed to test the server behavior if the same client trigger a new connection after CPMDisconnect.")]
        public void CPMDisconnect_Reconnect()
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

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMDisconnect.");
            wspAdapter.CPMDisconnect(false);

            var isExpectedExeceptionThrown = false;
            try
            {
                Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects that exceptions should be thrown.");
                wspAdapter.CPMConnectIn();
            }
            catch (RequestSender.RequestSenderException e)
            {
                if (e.Smb2Status == STATUS_PIPE_DISCONNECTED)
                {
                    isExpectedExeceptionThrown = true;
                }
                else
                {
                    throw;
                }
            }

            Site.Assert.IsTrue(isExpectedExeceptionThrown, "There should be an expected exception thrown which indicates the underlying named pipe of the client is disconnected.");
        }
        #endregion
    }
}

