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
    /// This test class is for SWN AsyncNotification.
    /// </summary>
    [TestClass]
    public class SWNAsyncNotification : ServerFailoverTestBase
    {
        #region Variables

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

            base.TestCleanup();
        }
        #endregion

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Swn)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Register with WitnessrRegister and Get CLIENT_MOVE_NOTIFICATION on scaleout cluster server.")]
        public void BVT_WitnessrRegister_SWNAsyncNotification_ClientMove()
        {
            SWNAsyncNotification_ClientMove(SwnVersion.SWN_VERSION_1);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Swn)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Register with WitnessrRegisterEx and Get CLIENT_MOVE_NOTIFICATION on scaleout cluster server.")]
        public void BVT_WitnessrRegisterEx_SWNAsyncNotification_ClientMove()
        {
            SWNAsyncNotification_ClientMove(SwnVersion.SWN_VERSION_2);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Swn)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Get IP_CHANGE_NOTIFICATION on scaleout cluster server.")]
        public void BVT_WitnessrRegisterEx_SWNAsyncNotification_IPChange()
        {
            SWNAsyncNotification_IPChange(SwnMessageType.IP_CHANGE_NOTIFICATION);
        }

        /// <summary>
        /// Test CLIENT_MOVE_NOTIFICATION.
        /// </summary>
        /// <param name="expectedVersion">SwnVersion.SWN_VERSION_2 indicates that register with WitnessrRegisterEx, SwnVersion.SWN_VERSION_1 indicates that register with WitnessrRegister.</param>
        private void SWNAsyncNotification_ClientMove(SwnVersion expectedVersion)
        {
            int ret = 0;
            uint callId = 0;
            WITNESS_INTERFACE_INFO registerInterface;
            string server = TestConfig.ClusteredScaleOutFileServerName;

            #region Get the file server to access it through SMB2

            IPAddress currentAccessIp = SWNTestUtility.GetCurrentAccessIP(server);
            BaseTestSite.Assert.AreNotEqual(null, currentAccessIp, "IP address of the file server should NOT be empty");
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Got the IP {0} to access the file server", currentAccessIp.ToString());

            #endregion

            #region Get register interface

            DoUntilSucceed(() => SWNTestUtility.BindServer(swnClientForInterface, currentAccessIp,
                TestConfig.DomainName, TestConfig.UserName, TestConfig.UserPassword, TestConfig.DefaultSecurityPackage,
                TestConfig.DefaultRpceAuthenticationLevel, TestConfig.Timeout, server), TestConfig.FailoverTimeout,
                "Retry BindServer until succeed within timeout span");

            WITNESS_INTERFACE_LIST interfaceList = new WITNESS_INTERFACE_LIST();

            DoUntilSucceed(() =>
            {
                ret = swnClientForInterface.WitnessrGetInterfaceList(out interfaceList);
                BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_SUCCESS, (SwnErrorCode)ret, "WitnessrGetInterfaceList returns with result code = 0x{0:x8}", ret);
                return SWNTestUtility.VerifyInterfaceList(interfaceList, TestConfig.Platform);
            }, TestConfig.FailoverTimeout, "Retry to call WitnessrGetInterfaceList until succeed within timeout span");

            swnClientForInterface.SwnUnbind(TestConfig.Timeout);

            SWNTestUtility.GetRegisterInterface(interfaceList, out registerInterface);

            SWNTestUtility.CheckVersion(expectedVersion, (SwnVersion)registerInterface.Version);
            #endregion

            #region Register SWN witness

            DoUntilSucceed(() => SWNTestUtility.BindServer(swnClientForWitness,
                (registerInterface.Flags & (uint)SwnNodeFlagsValue.IPv4) != 0 ? new IPAddress(registerInterface.IPV4) : SWNTestUtility.ConvertIPV6(registerInterface.IPV6),
                TestConfig.DomainName, TestConfig.UserName, TestConfig.UserPassword, TestConfig.DefaultSecurityPackage,
                TestConfig.DefaultRpceAuthenticationLevel, TestConfig.Timeout, server), TestConfig.FailoverTimeout,
                "Retry BindServer until succeed within timeout span");

            string clientName = TestConfig.WitnessClientName;
            string netName = SWNTestUtility.GetPrincipleName(TestConfig.DomainName, server);

            BaseTestSite.Log.Add(LogEntryKind.Debug, "Register witness:");
            BaseTestSite.Log.Add(LogEntryKind.Debug, "\tNetName: {0}", netName);
            BaseTestSite.Log.Add(LogEntryKind.Debug, "\tIPAddress: {0}", currentAccessIp.ToString());
            BaseTestSite.Log.Add(LogEntryKind.Debug, "\tClient Name: {0}", clientName);

            if (SwnVersion.SWN_VERSION_2 == expectedVersion)
            {
                ret = swnClientForWitness.WitnessrRegisterEx(SwnVersion.SWN_VERSION_2,
                    netName,
                    null,
                    currentAccessIp.ToString(),
                    clientName,
                    WitnessrRegisterExFlagsValue.WITNESS_REGISTER_IP_NOTIFICATION,
                    120,
                    out pContext);
                BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_SUCCESS, (SwnErrorCode)ret, "WitnessrRegisterEx returns with result code = 0x{0:x8}", ret);
            }
            else
            {
                ret = swnClientForWitness.WitnessrRegister(SwnVersion.SWN_VERSION_1,
                netName,
                currentAccessIp.ToString(),
                clientName,
                out pContext);
                BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_SUCCESS, (SwnErrorCode)ret, "WitnessrRegister returns with result code = 0x{0:x8}", ret);
            }
            BaseTestSite.Assert.IsNotNull(pContext, "Expect pContext is not null.");

            callId = swnClientForWitness.WitnessrAsyncNotify(pContext);
            BaseTestSite.Assert.AreNotEqual<uint>(0, callId, "WitnessrAsyncNotify returns callId = {0}", callId);

            #endregion

            #region Create a file and write content

            string uncSharePath = Smb2Utility.GetUncPath(server, TestConfig.ClusteredFileShare);
            string content = Smb2Utility.CreateRandomString(TestConfig.WriteBufferLengthInKb);
            string testDirectory = CreateTestDirectory(uncSharePath);
            string file = Path.Combine(testDirectory, Guid.NewGuid().ToString());
            Guid clientGuid = Guid.NewGuid();
            Guid createGuid = Guid.NewGuid();
            FileServerType fsType = FileServerType.ScaleOutFileServer;

            FILEID fileId = FILEID.Zero;
            DoUntilSucceed(() => WriteContentBeforeFailover(fsType, server, currentAccessIp, uncSharePath, file, content, clientGuid, createGuid, out fileId),
                    TestConfig.FailoverTimeout,
                    "Before failover, retry Write content until succeed within timeout span.");

            #endregion

            #region Move resource node

            // Move resource node to trigger CLIENT_MOVE_NOTIFICATION
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Move resource to interface {0} to trigger CLIENT_MOVE_NOTIFICATION", registerInterface.InterfaceGroupName);
            sutController.MoveSmbWitnessClient(clientName,
                SWNTestUtility.GetPrincipleName(TestConfig.DomainName, registerInterface.InterfaceGroupName));

            #endregion

            #region Wait CLIENT_MOVE_NOTIFICATION

            RESP_ASYNC_NOTIFY respNotify;
            // Wait the CLIENT_MOVE_NOTIFICATION
            ret = swnClientForWitness.ExpectWitnessrAsyncNotify(callId, out respNotify);
            BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_SUCCESS, (SwnErrorCode)ret, "WitnessrAsyncNotify returns with result code = 0x{0:x8}", ret);
            SWNTestUtility.PrintNotification(respNotify);

            // Verify RESP_ASYNC_NOTIFY
            SWNTestUtility.VerifyClientMoveShareMoveAndIpChange(respNotify, SwnMessageType.CLIENT_MOVE_NOTIFICATION, (uint)SwnIPAddrInfoFlags.IPADDR_V4, TestConfig.Platform);
            #endregion

            #region Get the new IpAddr
            IPADDR_INFO_LIST ipAddrInfoList;
            SwnUtility.ParseIPAddrInfoList(respNotify, out ipAddrInfoList);
            currentAccessIp = (ipAddrInfoList.IPAddrList[0].Flags & (uint)SwnNodeFlagsValue.IPv4) != 0 ? new IPAddress(ipAddrInfoList.IPAddrList[0].IPV4) : SWNTestUtility.ConvertIPV6(ipAddrInfoList.IPAddrList[0].IPV6);
            #endregion

            #region Unregister SWN Witness

            ret = swnClientForWitness.WitnessrUnRegister(pContext);
            BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_SUCCESS, (SwnErrorCode)ret, "WitnessrUnRegister returns with result code = 0x{0:x8}", ret);
            pContext = IntPtr.Zero;
            swnClientForWitness.SwnUnbind(TestConfig.Timeout);

            #endregion

            #region Make sure Cluster Share is available
            DoUntilSucceed(() => sutProtocolController.CheckIfShareIsAvailable(uncSharePath),
                testConfig.Timeout,
                "Make sure cluster share is available.");
            #endregion

            #region Read content and close the file
            DoUntilSucceed(() => ReadContentAfterFailover(server, currentAccessIp, uncSharePath, file, content, clientGuid, createGuid, fileId),
                    TestConfig.FailoverTimeout,
                    "Retry Read content until succeed within timeout span.");
            #endregion
        }

        private void SWNAsyncNotification_IPChange(SwnMessageType messageType)
        {
            int ret = 0;
            uint callId = 0;
            WITNESS_INTERFACE_INFO registerInterface;
            string server = TestConfig.ClusteredScaleOutFileServerName;

            #region Get the file server IP and access it through SMB2
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Get the file server Ip and access it through SMB2.");
            IPAddress currentAccessIp = SWNTestUtility.GetCurrentAccessIP(server);
            BaseTestSite.Assert.AreNotEqual(null, currentAccessIp, "IP address of the file server should NOT be empty");
            BaseTestSite.Log.Add(LogEntryKind.Debug, "IP address of the file server is {0}.", currentAccessIp.ToString());

            #endregion

            #region Get interface list to register.
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Get interface list to register.");
            DoUntilSucceed(() => SWNTestUtility.BindServer(swnClientForInterface, currentAccessIp,
                TestConfig.DomainName, TestConfig.UserName, TestConfig.UserPassword, TestConfig.DefaultSecurityPackage,
                TestConfig.DefaultRpceAuthenticationLevel, TestConfig.Timeout, server), TestConfig.FailoverTimeout,
                "Retry BindServer until succeed within timeout span");

            WITNESS_INTERFACE_LIST interfaceList = new WITNESS_INTERFACE_LIST();
            DoUntilSucceed(() =>
            {
                ret = swnClientForInterface.WitnessrGetInterfaceList(out interfaceList);
                BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_SUCCESS, (SwnErrorCode)ret, "WitnessrGetInterfaceList returns with result code = 0x{0:x8}", ret);
                return SWNTestUtility.VerifyInterfaceList(interfaceList, TestConfig.Platform);
            }, TestConfig.FailoverTimeout, "Retry to call WitnessrGetInterfaceList until succeed within timeout span");

            swnClientForInterface.SwnUnbind(TestConfig.Timeout);

            SWNTestUtility.GetRegisterInterface(interfaceList, out registerInterface);

            SWNTestUtility.CheckVersion(SwnVersion.SWN_VERSION_2, (SwnVersion)registerInterface.Version);
            #endregion

            #region Register SWN witness
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Register SWN witness.");
            DoUntilSucceed(() => SWNTestUtility.BindServer(swnClientForWitness,
                (registerInterface.Flags & (uint)SwnNodeFlagsValue.IPv4) != 0 ? new IPAddress(registerInterface.IPV4) : SWNTestUtility.ConvertIPV6(registerInterface.IPV6),
                TestConfig.DomainName, TestConfig.UserName, TestConfig.UserPassword, TestConfig.DefaultSecurityPackage,
                TestConfig.DefaultRpceAuthenticationLevel, TestConfig.Timeout, server), TestConfig.FailoverTimeout,
                "Retry BindServer until succeed within timeout span");

            string clientName = TestConfig.WitnessClientName;
            string netName = SWNTestUtility.GetPrincipleName(TestConfig.DomainName, server);
            string shareName = TestConfig.ClusteredFileShare;
            WitnessrRegisterExFlagsValue flag = WitnessrRegisterExFlagsValue.WITNESS_REGISTER_NONE;
            if (messageType == SwnMessageType.IP_CHANGE_NOTIFICATION)
                flag = WitnessrRegisterExFlagsValue.WITNESS_REGISTER_IP_NOTIFICATION;

            BaseTestSite.Log.Add(LogEntryKind.Debug, "Register witness:");
            BaseTestSite.Log.Add(LogEntryKind.Debug, "\tNetName: {0}", netName);
            BaseTestSite.Log.Add(LogEntryKind.Debug, "\tIPAddress: {0}", currentAccessIp.ToString());
            BaseTestSite.Log.Add(LogEntryKind.Debug, "\tClient Name: {0}", clientName);

            ret = swnClientForWitness.WitnessrRegisterEx(SwnVersion.SWN_VERSION_2,
                netName,
                shareName,
                currentAccessIp.ToString(),
                clientName,
                flag,
                120,
                out pContext);
            BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_SUCCESS, (SwnErrorCode)ret, "WitnessrRegisterEx returns with result code = 0x{0:x8}", ret);

            BaseTestSite.Assert.IsNotNull(pContext, "Expect pContext is not null.");

            // Reboot witness node (Restart witness service), cause SHARE_MOVE_NOTIFICATION            
            callId = swnClientForWitness.WitnessrAsyncNotify(pContext);
            BaseTestSite.Assert.AreNotEqual<uint>(0, callId, "WitnessrAsyncNotify returns callId = {0}", callId);
            RESP_ASYNC_NOTIFY respNotify;
            
            ret = swnClientForWitness.ExpectWitnessrAsyncNotify(callId, out respNotify);
            BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_SUCCESS, (SwnErrorCode)ret, "WitnessrAsyncNotify returns with result code = 0x{0:x8}", ret);
            SWNTestUtility.PrintNotification(respNotify);
            SWNTestUtility.VerifyClientMoveShareMoveAndIpChange(respNotify, SwnMessageType.SHARE_MOVE_NOTIFICATION, (uint)SwnIPAddrInfoFlags.IPADDR_V4, TestConfig.Platform);
            #endregion

            #region Trigger Notification
            if (messageType == SwnMessageType.IP_CHANGE_NOTIFICATION)
            {
                #region Refresh network adapter on node to trigger IP_CHANGE_NOTIFICATION               
                callId = swnClientForWitness.WitnessrAsyncNotify(pContext);
                BaseTestSite.Assert.AreNotEqual<uint>(0, callId, "WitnessrAsyncNotify returns callId = {0}", callId);
                // Refresh an adapter on the node the client is connected, to trigger IP_CHANGE_NOTIFICATION
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Refresh an adapter to trigger IP_CHANGE_NOTIFICATION");
                IPAddress[] addressList = Dns.GetHostEntry(currentAccessIp).AddressList;
                IPAddress refreshIpAddress = null;
                foreach (IPAddress ipAddress in addressList)
                {
                    if (ipAddress.ToString() != currentAccessIp.ToString())
                    {
                        refreshIpAddress = ipAddress;
                        break;
                    }
                }

                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Refresh network adapter on node {0} to trigger IP_CHANGE_NOTIFICATION.", refreshIpAddress.ToString());
                sutController.RefreshNetAdapter(refreshIpAddress.ToString(), Dns.GetHostEntry(currentAccessIp).HostName);
                #endregion

                #region Wait for IP_CHANGE_NOTIFICATION
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Wait for IP_CHANGE_NOTIFICATION.");

                ret = swnClientForWitness.ExpectWitnessrAsyncNotify(callId, out respNotify);
                BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_SUCCESS, (SwnErrorCode)ret, "WitnessrAsyncNotify returns with result code = 0x{0:x8}", ret);
                SWNTestUtility.PrintNotification(respNotify);
                SWNTestUtility.VerifyClientMoveShareMoveAndIpChange(respNotify, SwnMessageType.IP_CHANGE_NOTIFICATION, (uint)(SwnIPAddrInfoFlags.IPADDR_V4 | SwnIPAddrInfoFlags.IPADDR_OFFLINE), TestConfig.Platform);
                #endregion
            }
            #endregion

            #region Unregister SWN Witness
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Unregister SWN Witness.");

            ret = swnClientForWitness.WitnessrUnRegister(pContext);
            BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_SUCCESS, (SwnErrorCode)ret, "WitnessrUnRegister returns with result code = 0x{0:x8}", ret);
            pContext = IntPtr.Zero;
            swnClientForWitness.SwnUnbind(TestConfig.Timeout);
            #endregion
        }
    }
}
