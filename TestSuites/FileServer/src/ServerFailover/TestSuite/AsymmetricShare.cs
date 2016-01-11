// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Swn;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Net;

namespace Microsoft.Protocols.TestSuites.FileSharing.ServerFailover.TestSuite
{
    /// <summary>
    /// This test class is for asymmetric share.
    /// </summary>
    [TestClass]
    public class AsymmetricShare : ServerFailoverTestBase
    {
        #region Variables
        // Test endpoint before failover happens
        private Smb2FunctionalClient smb2Client;

        /// <summary>
        /// SWN client to get interface list.
        /// </summary>
        private SwnClient swnClientForInterface;

        /// <summary>
        /// SWN client to witness.
        /// </summary>
        private SwnClient swnClientForWitness;

        /// <summary>
        /// The registered handle of witness.
        /// </summary>
        private System.IntPtr pContext;

        /// <summary>
        /// The share which the test case will test.
        /// </summary>
        private string uncSharePath;

        /// <summary>
        /// The directory under which the test case will create test file.
        /// </summary>
        private string testDirectory;

        #endregion

        #region Test Suite Initialization

        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
            SWNTestUtility.BaseTestSite = BaseTestSite;
        }

        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }

        #endregion

        #region Test Initialization
        // Use TestInitialize to run code before running every test in the class
        protected override void TestInitialize()
        {
            base.TestInitialize();

            swnClientForInterface = new SwnClient();
            swnClientForWitness = new SwnClient();
            pContext = IntPtr.Zero;
        }

        // Use TestCleanup to run code after every test in a class have run
        protected override void TestCleanup()
        {
            if (smb2Client != null)
            {
                smb2Client.Disconnect();
            }

            if (pContext != IntPtr.Zero)
            {
                swnClientForWitness.WitnessrUnRegister(pContext);
                pContext = IntPtr.Zero;
            }

            try
            {
                swnClientForInterface.SwnUnbind(TestConfig.Timeout);
            }
            catch (Exception ex)
            {
                BaseTestSite.Log.Add(LogEntryKind.Warning, "TestCleanup: Unexpected Exception: {0}", ex);
            }

            try
            {
                swnClientForWitness.SwnUnbind(TestConfig.Timeout);
            }
            catch (Exception ex)
            {
                BaseTestSite.Log.Add(LogEntryKind.Warning, "TestCleanup: Unexpected Exception: {0}", ex);
            }

            try
            {
                sutProtocolController.DeleteDirectory(uncSharePath, testDirectory);
            }
            catch (Exception ex)
            {
                BaseTestSite.Log.Add(LogEntryKind.Warning, "TestCleanup: Unexpected Exception: {0}", ex);
            }

            base.TestCleanup();
        }
        #endregion

        #region Test Cases
        [TestMethod]
        [TestCategory(TestCategories.Smb302)]
        [TestCategory(TestCategories.Swn)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test asymmetric share when the client connects to the asymmetric share on the non-optimum node.")]
        public void AsymmetricShare_OnNonOptimumNode()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb302);
            #endregion

            TestAsymmetricShare(DialectRevision.Smb302, TestConfig.NonOptimumNodeOfAsymmetricShare, true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb302)]
        [TestCategory(TestCategories.Swn)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test asymmetric share when the client connects to the asymmetric share on the optimum node.")]
        public void AsymmetricShare_OnOptimumNode()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb302);
            #endregion
            TestAsymmetricShare(DialectRevision.Smb302, TestConfig.OptimumNodeOfAsymmetricShare, true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb302)]
        [TestCategory(TestCategories.Swn)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("This test case is designed to test asymmetric share when the client connects to the non scaleout share.")]
        public void AsymmetricShare_OnNonScaleOutShare()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb302);
            #endregion
            TestAsymmetricShare(DialectRevision.Smb302, TestConfig.SutComputerName, false);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb302)]
        [TestCategory(TestCategories.Swn)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test asymmetric share when the client connects to the non-optimum node with Smb30 dialect.")]
        public void AsymmetricShare_OnSmb30()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb302);
            #endregion
            TestAsymmetricShare(DialectRevision.Smb30, TestConfig.NonOptimumNodeOfAsymmetricShare, true);
        }
        #endregion

        private void TestAsymmetricShare(DialectRevision requestMaxDialect, string serverName, bool isAsymmetricShare)
        {
            int ret = 0;
            uint callId = 0;
            Guid clientGuid = Guid.NewGuid();
            WITNESS_INTERFACE_INFO registerInterface;
            string shareName = isAsymmetricShare ? TestConfig.AsymmetricShare : TestConfig.BasicFileShare;

            #region Get the file server to access it through SMB2
            IPAddress currentAccessIp = SWNTestUtility.GetCurrentAccessIP(serverName);
            BaseTestSite.Assert.IsNotNull(currentAccessIp, "IP address of the file server should NOT be empty");
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Got the IP {0} to access the file server", currentAccessIp.ToString());
            #endregion

            #region Connect to the asymmetric share
            uncSharePath = Smb2Utility.GetUncPath(serverName, shareName);
            string content = Smb2Utility.CreateRandomString(TestConfig.WriteBufferLengthInKb);
            testDirectory = CreateTestDirectory(uncSharePath);
            string file = Path.Combine(testDirectory, Guid.NewGuid().ToString());

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start the client by sending the following requests: NEGOTIATE; SESSION_SETUP");
            smb2Client = new Smb2FunctionalClient(TestConfig.FailoverTimeout, TestConfig, BaseTestSite);
            smb2Client.ConnectToServerOverTCP(currentAccessIp);
            smb2Client.Negotiate(
                Smb2Utility.GetDialects(requestMaxDialect),
                TestConfig.IsSMB1NegotiateEnabled,
                capabilityValue: Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES,
                clientGuid: clientGuid,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, header.Status, "Negotiate should succeed.");
                    BaseTestSite.Assert.AreEqual(
                        requestMaxDialect,
                        response.DialectRevision,
                        "The server is expected to use dialect {0}. Actual dialect is {1}",
                        requestMaxDialect,
                        response.DialectRevision);
                });
            smb2Client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                serverName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);

            uint treeId = 0;
            Share_Capabilities_Values shareCapabilities = Share_Capabilities_Values.NONE;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends TREE_CONNECT request to the {0} on the {1}", shareName, serverName);
            DoUntilSucceed(
                () => smb2Client.TreeConnect(uncSharePath, out treeId, (header, response) => { shareCapabilities = response.Capabilities; }),
                TestConfig.FailoverTimeout,
                "Retry TreeConnect until succeed within timeout span");

            if (requestMaxDialect == DialectRevision.Smb302 && isAsymmetricShare)
            {
                BaseTestSite.Assert.IsTrue(shareCapabilities.HasFlag(Share_Capabilities_Values.SHARE_CAP_ASYMMETRIC),
                    "The capabilities of the share should contain SHARE_CAP_ASYMMETRIC. The actual capabilities is {0}.", shareCapabilities);
            }
            else
            {
                BaseTestSite.Assert.IsFalse(shareCapabilities.HasFlag(Share_Capabilities_Values.SHARE_CAP_ASYMMETRIC),
                    "The capabilities of the share should not contain SHARE_CAP_ASYMMETRIC. The actual capabilities is {0}.", shareCapabilities);

                #region Disconnect current SMB2 connection
                smb2Client.TreeDisconnect(treeId);
                smb2Client.LogOff();
                smb2Client.Disconnect();
                #endregion
                return;
            }

            FILEID fileId;
            Smb2CreateContextResponse[] serverCreateContexts;
            Guid createGuid = Guid.NewGuid();
            Guid leaseKey = Guid.NewGuid();
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client writes to the file.");
            smb2Client.Create(
                treeId,
                file,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                out fileId,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE);
            smb2Client.Write(treeId, fileId, content);

            string readContent;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client reads from the file.");
            smb2Client.Read(treeId, fileId, 0, (uint)content.Length, out readContent);
            BaseTestSite.Assert.IsTrue(
                content.Equals(readContent),
                "Content read should be identical to that written.");
            #endregion

            #region Get register interface
            DoUntilSucceed(() => SWNTestUtility.BindServer(swnClientForInterface, currentAccessIp,
                TestConfig.DomainName, TestConfig.UserName, TestConfig.UserPassword, TestConfig.DefaultSecurityPackage,
                TestConfig.DefaultRpceAuthenticationLevel, TestConfig.Timeout, serverName), TestConfig.FailoverTimeout,
                "Retry BindServer until succeed within timeout span");

            WITNESS_INTERFACE_LIST interfaceList = new WITNESS_INTERFACE_LIST();
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client calls WitnessrGetInterfaceList.");
            DoUntilSucceed(() =>
            {
                ret = swnClientForInterface.WitnessrGetInterfaceList(out interfaceList);
                BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_SUCCESS, (SwnErrorCode)ret, "WitnessrGetInterfaceList returns with result code = 0x{0:x8}", ret);
                return SWNTestUtility.VerifyInterfaceList(interfaceList, TestConfig.Platform);
            }, TestConfig.FailoverTimeout, "Retry to call WitnessrGetInterfaceList until succeed within timeout span");

            swnClientForInterface.SwnUnbind(TestConfig.Timeout);

            SWNTestUtility.GetRegisterInterface(interfaceList, out registerInterface);

            #endregion

            #region Get SHARE_MOVE_NOTIFICATION
            DoUntilSucceed(() => SWNTestUtility.BindServer(swnClientForWitness,
                (registerInterface.Flags & (uint)SwnNodeFlagsValue.IPv4) != 0 ? new IPAddress(registerInterface.IPV4) : SWNTestUtility.ConvertIPV6(registerInterface.IPV6),
                TestConfig.DomainName, TestConfig.UserName, TestConfig.UserPassword, TestConfig.DefaultSecurityPackage,
                TestConfig.DefaultRpceAuthenticationLevel, TestConfig.Timeout, serverName), TestConfig.FailoverTimeout,
                "Retry BindServer until succeed within timeout span");

            string clientName = TestConfig.WitnessClientName;

            BaseTestSite.Log.Add(LogEntryKind.Debug, "Register witness:");
            BaseTestSite.Log.Add(LogEntryKind.Debug, "\tNetName: {0}", serverName);
            BaseTestSite.Log.Add(LogEntryKind.Debug, "\tIPAddress: {0}", currentAccessIp.ToString());
            BaseTestSite.Log.Add(LogEntryKind.Debug, "\tClient Name: {0}", clientName);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client calls WitnessrRegisterEx.");
            ret = swnClientForWitness.WitnessrRegisterEx(SwnVersion.SWN_VERSION_2,
                serverName,
                shareName,
                currentAccessIp.ToString(),
                clientName,
                WitnessrRegisterExFlagsValue.WITNESS_REGISTER_IP_NOTIFICATION,
                120,
                out pContext);
            BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_SUCCESS, (SwnErrorCode)ret, "WitnessrRegisterEx returns with result code = 0x{0:x8}", ret);

            BaseTestSite.Assert.IsNotNull(pContext, "Expect pContext is not null.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client calls WitnessrAsyncNotify.");
            callId = swnClientForWitness.WitnessrAsyncNotify(pContext);
            BaseTestSite.Assert.AreNotEqual<uint>(0, callId, "WitnessrAsyncNotify returns callId = {0}", callId);

            // NOTICE
            // This comment is for current Windows Cluster test environment.
            // Current test environment has only two nodes and both they are optimum nodes. 
            // So whatever the server name is, SHARE_MOVE_NOTIFICATION notification will be recieved.
            // The configuration items 'OptimumNodeOfAsymmetricShare' and 'NonOptimumNodeOfAsymmetricShare' are assigned the same default value.
            // The code in if block will be executed all the time.
            if (serverName == TestConfig.NonOptimumNodeOfAsymmetricShare)
            {
                #region Expect that SHARE_MOVE_NOTIFICATION notification will be received  when the client connects to the asymmetric share on the non-optimum share
                RESP_ASYNC_NOTIFY respNotify;
                ret = swnClientForWitness.ExpectWitnessrAsyncNotify(callId, out respNotify);
                BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_SUCCESS, (SwnErrorCode)ret, "WitnessrAsyncNotify returns with result code = 0x{0:x8}", ret);
                SWNTestUtility.PrintNotification(respNotify);
                SWNTestUtility.VerifyClientMoveShareMoveAndIpChange(respNotify, SwnMessageType.SHARE_MOVE_NOTIFICATION, (uint)SwnIPAddrInfoFlags.IPADDR_V4, TestConfig.Platform);

                #region Get the new IpAddr
                IPADDR_INFO_LIST ipAddrInfoList;
                SwnUtility.ParseIPAddrInfoList(respNotify, out ipAddrInfoList);
                currentAccessIp = (ipAddrInfoList.IPAddrList[0].Flags & (uint)SwnNodeFlagsValue.IPv4) != 0 ? new IPAddress(ipAddrInfoList.IPAddrList[0].IPV4) : SWNTestUtility.ConvertIPV6(ipAddrInfoList.IPAddrList[0].IPV6);
                #endregion

                #region Unregister SWN Witness
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client calls WitnessrUnRegister.");
                ret = swnClientForWitness.WitnessrUnRegister(pContext);
                BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_SUCCESS, (SwnErrorCode)ret, "WitnessrUnRegister returns with result code = 0x{0:x8}", ret);
                pContext = IntPtr.Zero;
                swnClientForWitness.SwnUnbind(TestConfig.Timeout);
                #endregion

                #region Disconnect current SMB2 connection
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the client by sending the following requests: TREE_DISCONNECT; LOG_OFF; DISCONNECT.");
                smb2Client.TreeDisconnect(treeId);
                smb2Client.LogOff();
                smb2Client.Disconnect();
                #endregion
                #endregion
            }
            else
            {
                #region Expect that no SHARE_MOVE_NOTIFICATION notification will be received when the client connects to the asymmetric share on the optimum share
                bool isNotificationReceived = false;
                try
                {
                    RESP_ASYNC_NOTIFY respNotify;
                    ret = swnClientForWitness.ExpectWitnessrAsyncNotify(callId, out respNotify);
                    isNotificationReceived = true;
                }
                catch (TimeoutException)
                {
                    isNotificationReceived = false;
                }

                #region Disconnect current SMB2 connection
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the second client by sending the following requests: TREE_DISCONNECT; LOG_OFF; DISCONNECT");
                smb2Client.TreeDisconnect(treeId);
                smb2Client.LogOff();
                smb2Client.Disconnect();
                #endregion

                #region Unregister SWN Witness
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client calls WitnessrUnRegister.");
                ret = swnClientForWitness.WitnessrUnRegister(pContext);
                BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_SUCCESS, (SwnErrorCode)ret, "WitnessrUnRegister returns with result code = 0x{0:x8}", ret);
                pContext = IntPtr.Zero;
                swnClientForWitness.SwnUnbind(TestConfig.Timeout);
                #endregion

                BaseTestSite.Assert.IsFalse(isNotificationReceived, "Expect that no notification will be received when the client has connected to asymmetric share on the optimum node.");
                #endregion

                return;
            }
            #endregion

            #region Connect to the share on the optimum node
            smb2Client = new Smb2FunctionalClient(TestConfig.FailoverTimeout, TestConfig, BaseTestSite);
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Got the IP {0} to access the file server", currentAccessIp.ToString());

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start a client by sending the following requests: NEGOTIATE; SESSION_SETUP");
            smb2Client.ConnectToServerOverTCP(currentAccessIp);
            smb2Client.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                capabilityValue: Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES,
                clientGuid: clientGuid,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, header.Status, "Negotiate should succeed.");
                });
            smb2Client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                serverName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client sends TREE_CONNECT and wait for a response until timeout.");
            DoUntilSucceed(
                () => smb2Client.TreeConnect(uncSharePath, out treeId, (header, response) => { }),
                TestConfig.FailoverTimeout,
                "Retry TreeConnect until succeed within timeout span");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client writes to the file.");
            smb2Client.Create(
                treeId,
                file,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                out fileId,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE);
            smb2Client.Write(treeId, fileId, content);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client reads from the file.");
            smb2Client.Read(treeId, fileId, 0, (uint)content.Length, out readContent);
            BaseTestSite.Assert.IsTrue(
                content.Equals(readContent),
                "Content read should be identical to that written.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the client by sending the following requests: TREE_DISCONNECT; LOG_OFF; DISCONNECT.");
            smb2Client.TreeDisconnect(treeId);
            smb2Client.LogOff();
            smb2Client.Disconnect();
            #endregion
        }
    }
}
