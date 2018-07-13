// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite.ResilientHandle
{
    [TestClass]
    public class ResilientOpenScavengerTimer : SMB2TestBase
    {
        public const uint DEFAULT_RESILIENT_TIMEOUT_IN_SECONDS = 120;

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

        #region Test Case Initialize and Clean up
        protected override void TestInitialize()
        {
            base.TestInitialize();
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }
        #endregion

        #region Test Case

        [TestMethod]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.FsctlLmrRequestResiliency)]
        [TestCategory(TestCategories.Positive)]
        [Description("Verify that the Open will be closed after Resilient Open Scavenger Timer expired.")]
        public void ResilientOpenScavengerTimer_ReconnectAfterTimeout()
        {
            /// 1. Wait for MaxResiliencyTimeoutInSecond seconds to expire the Resilient Timer and make sure that the Timer is not started.
            /// 2. Open Resilient Handle with specific timeout.
            /// 3. Disconnect to start the Resilient Timer.
            /// 4. Wait for specific timeout + 1 seconds.
            /// 5. Reconnect the resilient handle and expect the handle is terminated.

            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb21);
            TestConfig.CheckIOCTL(CtlCode_Values.FSCTL_LMR_REQUEST_RESILIENCY);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT);
            #endregion

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Wait {0} seconds to make sure Resilient Timer is not started.", testConfig.MaxResiliencyTimeoutInSecond);
            Thread.Sleep(TimeSpan.FromSeconds((int)testConfig.MaxResiliencyTimeoutInSecond));

            Smb2FunctionalClient prepareClient = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            uint timeoutInSecond = testConfig.MaxResiliencyTimeoutInSecond / 2;
            Guid clientGuid = Guid.NewGuid();
            string fileName = GetTestFileName(Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare));
            FILEID fileId;

            // Open file & Resiliency request
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Create resilient handle to file '{0}' and timeout is {1} seconds", fileName, timeoutInSecond);
            OpenFileAndResilientRequest(
                prepareClient,
                clientGuid,
                fileName,
                timeoutInSecond * 1000, // convert second to millisecond
                out fileId);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Disconnect the client to start the Resilient Open Scavenger Timer on server");
            prepareClient.Disconnect();

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Wait {0} seconds before Resilient Open Scavenger Timer expired.", timeoutInSecond + 10);
            Thread.Sleep(TimeSpan.FromSeconds(timeoutInSecond + 10));

            Smb2FunctionalClient reconnectClient = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Re-establish Resilient Open, verify the returned status is STATUS_OBJECT_NAME_NOT_FOUND.");
            ReconnectResilientHandle(
                reconnectClient,
                clientGuid,
                fileName,
                fileId,
                NtStatus.STATUS_OBJECT_NAME_NOT_FOUND,
                "Resilient handle should be terminated.");

            // clean
            reconnectClient.Disconnect();
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.FsctlLmrRequestResiliency)]
        [TestCategory(TestCategories.Positive)]
        [Description("Verify that the Open will be preserved before Resilient Open Scavenger Timer expired.")]
        public void ResilientOpenScavengerTimer_ReconnectBeforeTimeout()
        {
            /// 1. Open Resilient Handle with specific timeout.
            /// 2. Disconnect to start the Resilient Timer.
            /// 3. Wait for specific timeout - 1 seconds.
            /// 4. Reconnect the resilient handle and expect the result is success.
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb21);
            TestConfig.CheckIOCTL(CtlCode_Values.FSCTL_LMR_REQUEST_RESILIENCY);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT);
            #endregion

            Smb2FunctionalClient prepareClient = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            uint timeoutInSecond = testConfig.MaxResiliencyTimeoutInSecond / 2;
            Guid clientGuid = Guid.NewGuid();
            string fileName = GetTestFileName(Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare));
            FILEID fileId;

            // Open file & Resiliency request
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Create resilient handle to file '{0}' and timeout is {1} seconds", fileName, timeoutInSecond);
            OpenFileAndResilientRequest(
                prepareClient,
                clientGuid,
                fileName,
                timeoutInSecond * 1000, // convert second to millisecond
                out fileId);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Disconnect the client to start the Resilient Open Scavenger Timer on server");
            prepareClient.Disconnect();

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Wait {0} seconds before Resilient Open Scavenger Timer expired.", timeoutInSecond - 1);
            Thread.Sleep(TimeSpan.FromSeconds(timeoutInSecond - 1));

            Smb2FunctionalClient reconnectClient = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Re-establish Resilient Open, verify the returned status is STATUS_SUCCESS.");
            ReconnectResilientHandle(
                reconnectClient,
                clientGuid,
                fileName,
                fileId,
                NtStatus.STATUS_SUCCESS,
                "Reconnect resilient handle should be successful.");

            // clean
            reconnectClient.Disconnect();
        }

        #endregion

        #region Private Methods

        private void ConnectToShare(
            Smb2FunctionalClient client,
            Guid clientGuid,
            out uint treeId)
        {
            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);

            // Negotiate
            client.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                clientGuid: clientGuid,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                    {
                        BaseTestSite.Assert.AreEqual(
                            Smb2Status.STATUS_SUCCESS,
                            header.Status,
                            "CREATE should succeed.");

                        TestConfig.CheckNegotiateDialect(DialectRevision.Smb21, response);
                    });

            // SMB2 SESSION SETUP
            client.SessionSetup(
                testConfig.DefaultSecurityPackage,
                testConfig.SutComputerName,
                testConfig.AccountCredential,
                testConfig.UseServerGssToken);

            // SMB2 Tree Connect
            client.TreeConnect(
                Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare),
                out treeId);
        }

        private void OpenFileAndResilientRequest(
            Smb2FunctionalClient client,
            Guid clientGuid,
            string fileName,
            uint timeoutInMilliSeconds,
            out FILEID fileId)
        {
            uint treeId;

            // connect to share
            ConnectToShare(
                client,
                clientGuid,
                out treeId);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Connect to share '{0}' on server '{1}'", testConfig.BasicFileShare, testConfig.SutComputerName);

            // open file
            Smb2CreateContextResponse[] createContextResponse;
            client.Create(
                treeId,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out createContextResponse
            );
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Create Open with file name '{0}'", fileName);


            // resiliency request
            Packet_Header ioCtlHeader;
            IOCTL_Response ioCtlResponse;
            byte[] inputInResponse;
            byte[] outputInResponse;
            client.ResiliencyRequest(
                treeId,
                fileId,
                timeoutInMilliSeconds,
                (uint)Marshal.SizeOf(typeof(NETWORK_RESILIENCY_Request)),
                out ioCtlHeader,
                out ioCtlResponse,
                out inputInResponse,
                out outputInResponse
                );
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Resiliency request with timeout {0} milliseconds", timeoutInMilliSeconds);
        }

        private void ReconnectResilientHandle(
            Smb2FunctionalClient client,
            Guid clientGuid,
            string fileName,
            FILEID fileId,
            NtStatus expectedReconnectStatus,
            string message)
        {
            uint treeId;
            ConnectToShare(
                client,
                clientGuid,
                out treeId);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Reconnect to resilient handle");

            Smb2CreateContextResponse[] createContextResponse;
            uint status = client.Create(
                treeId,
                fileName,
                CreateOptions_Values.NONE,
                out fileId,
                out createContextResponse,
                createContexts: new Smb2CreateContextRequest[]
                    { 
                        new Smb2CreateDurableHandleReconnect()
                        {
                            Data = fileId
                        }
                    },
                checker: (header, response) =>
                {
                    // do nothing, skip the exception 
                }
                );
            BaseTestSite.Assert.AreEqual<NtStatus>(
                expectedReconnectStatus,
                (NtStatus)status,
                message);
        }
        #endregion
    }
}
