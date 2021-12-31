// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.WspTS
{
    [TestClass]
    public partial class WspMessageHeaderTestCases : WspCommonTestBase
    {
        private enum ArgumentType
        {
            InvalidMsg,
            InvalidStatus,
            InvalidUlChecksum
        }

        private ArgumentType argumentType;

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

            wspAdapter.CPMConnectOutResponse -= EnsureSuccessfulCPMConnectOut;
            wspAdapter.CPMConnectOutResponse += CPMConnectOut;
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }
        #endregion

        #region Test Cases
        [TestMethod]
        [TestCategory("WspMessageHeader")]
        [Description("This test case is designed to test the server response if invalid _msg is sent in WSP message header.")]
        public void WspMessageHeader_InvalidMsg()
        {
            argumentType = ArgumentType.InvalidMsg;
            wspAdapter.SendInvalidMsg = true;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends invalid _msg and expects STATUS_INVALID_PARAMETER.");
            wspAdapter.CPMConnectIn();
        }

        [TestMethod]
        [TestCategory("WspMessageHeader")]
        [Description("This test case is designed to test the server response if invalid _status is sent in WSP message header.")]
        public void WspMessageHeader_InvalidStatus()
        {
            argumentType = ArgumentType.InvalidStatus;
            wspAdapter.SendInvalidStatus = true;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends invalid _status and expects STATUS_INVALID_PARAMETER.");
            wspAdapter.CPMConnectIn();
        }

        [TestMethod]
        [TestCategory("WspMessageHeader")]
        [Description("This test case is designed to test the server response if invalid _ulChecksum is sent in WSP message header.")]
        public void WspMessageHeader_InvalidUlChecksum()
        {
            argumentType = ArgumentType.InvalidUlChecksum;
            wspAdapter.SendInvalidUlChecksum = true;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends invalid _ulChecksum and expects STATUS_INVALID_PARAMETER.");
            wspAdapter.CPMConnectIn();
        }
        #endregion

        private void CPMConnectOut(uint errorCode)
        {
            switch (argumentType)
            {
                case ArgumentType.InvalidMsg:
                    Site.Assert.AreEqual((uint)WspErrorCode.STATUS_INVALID_PARAMETER, errorCode, "Server should return STATUS_INVALID_PARAMETER if _msg of WSP message header is invalid.");
                    break;
                case ArgumentType.InvalidStatus:
                    Site.Assert.AreEqual((uint)WspErrorCode.STATUS_INVALID_PARAMETER, errorCode, "Server should return STATUS_INVALID_PARAMETER if _status of WSP message header is invalid.");
                    break;
                case ArgumentType.InvalidUlChecksum:
                    Site.Assert.AreEqual((uint)WspErrorCode.STATUS_INVALID_PARAMETER, errorCode, "Server should return STATUS_INVALID_PARAMETER if _ulChecksum of WSP message header is invalid.");
                    break;
            }
        }
    }
}
