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
using System.Linq;
using System.Net;
using System.Text;

namespace Microsoft.Protocols.TestSuites.FileSharing.ServerFailover.TestSuite
{
    [TestClass]
    public class FileServerFailover : ServerFailoverTestBase
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

        /// <summary>
        /// The type indicates witness is used or not.
        /// </summary>
        private WitnessType witnessType;

        #endregion

        #region Class Initialized and Cleanup
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
            SWNTestUtility.BaseTestSite = BaseTestSite;
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }
        #endregion

        #region Test Initialize and Cleanup
        protected override void TestInitialize()
        {
            base.TestInitialize();

            swnClientForInterface = new SwnClient();
            swnClientForWitness = new SwnClient();
            pContext = IntPtr.Zero;
            witnessType = WitnessType.None;
        }

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
                BaseTestSite.Log.Add(
                    LogEntryKind.Warning,
                    "TestCleanup: Unexpected Exception:", ex);
            }

            try
            {
                swnClientForWitness.SwnUnbind(TestConfig.Timeout);
            }
            catch (Exception ex)
            {
                BaseTestSite.Log.Add(
                    LogEntryKind.Warning,
                    "TestCleanup: Unexpected Exception:", ex);
            }

            base.TestCleanup();
        }
        #endregion

        #region BVT_FileServerFailover_FileServer

        [TestMethod]
        [TestCategory(TestCategories.Failover)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Swn)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server can handle failover to another node of continuous available file servers.")]
        public void FileServerFailover_FileServer()
        {
            FileServerFailoverTest(TestConfig.ClusteredFileServerName,
                FileServerType.GeneralFileServer);
        }

        #endregion

        #region BVT_FileServerFailover_ScaleOutFileServer

        [TestMethod]
        [TestCategory(TestCategories.Failover)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Swn)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server can handle failover to another node of continuous available scale out file servers.")]
        public void FileServerFailover_ScaleOutFileServer()
        {
            FileServerFailoverTest(TestConfig.ClusteredScaleOutFileServerName,
                FileServerType.ScaleOutFileServer);
        }

        #endregion

        #region FileServerFailover_ScaleOutFileServer_ReconnectWithoutFailover

        [TestMethod]
        [TestCategory(TestCategories.Failover)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Swn)]
        [TestCategory(TestCategories.Positive)]
        [Description("Ensure persistent handle could be re-connected via connection with another node without failover.")]
        public void FileServerFailover_ScaleOutFileServer_ReconnectWithoutFailover()
        {
            FileServerFailoverTest(TestConfig.ClusteredScaleOutFileServerName,
                FileServerType.ScaleOutFileServer,
                true);
        }

        #endregion

        #region BVT_SWNFileServerFailover_FileServer

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Swn)]
        [TestCategory(TestCategories.Positive)]
        [Description("Get WitnessrAsyncNotify notification on cluster server.")]
        public void SWNFileServerFailover_FileServer()
        {
            witnessType = WitnessType.SwnWitness;
            FileServerFailoverTest(TestConfig.ClusteredFileServerName,
                FileServerType.GeneralFileServer);
        }

        #endregion

        #region BVT_SWNFileServerFailover_ScaleOutFileServer

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Swn)]
        [TestCategory(TestCategories.Positive)]
        [Description("Get WitnessrAsyncNotify notification on scaleout cluster server.")]
        public void SWNFileServerFailover_ScaleOutFileServer()
        {
            witnessType = WitnessType.SwnWitness;
            FileServerFailoverTest(TestConfig.ClusteredScaleOutFileServerName,
                FileServerType.ScaleOutFileServer);
        }
        #endregion

        #region FileServerFailover_SMB311_Redirect_To_Owner_SOFS
        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Failover)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test server can handle a TreeConnect request with flag SMB2_SHAREFLAG_REDIRECT_TO_OWNER when SMB dialect is 3.1.1 and share type includes STYPE_CLUSTER_SOFS.")]
        public void FileServerFailover_SMB311_Redirect_To_Owner_SOFS()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb311);
            #endregion

            if (testConfig.IsWindowsPlatform)
            {
                // Use GetClusterResourceOwner to get active host node
                string sofsHostedNode = sutController.GetClusterResourceOwner(TestConfig.ClusteredScaleOutFileServerName).ToLower();
                string hostedNode = TestConfig.ClusterNode01.ToLower().Contains(sofsHostedNode) ?
                    TestConfig.ClusterNode01 : TestConfig.ClusterNode02;
                string nonHostedNode = !TestConfig.ClusterNode01.ToLower().Contains(hostedNode) ?
                    TestConfig.ClusterNode01 : TestConfig.ClusterNode02;

                TestRedirectToOwner(hostedNode, nonHostedNode);
            }
            else
            {
                // For non-Windows platform,
                // since we cannot find which host node is active, node01/node02 is treated as host node for testing respectively.
                bool isRedirectToOwnerTested = TestRedirectToOwner(TestConfig.ClusterNode01, TestConfig.ClusterNode02);
                if (isRedirectToOwnerTested == false)
                {
                    TestRedirectToOwner(TestConfig.ClusterNode02, TestConfig.ClusterNode01);
                }
            }
        }
        #endregion       

        #region Test Utility

        private void FileServerFailoverTest(string server, FileServerType fsType, bool reconnectWithoutFailover = false)
        {
            int ret = 0;
            uint callId = 0;
            IPAddress currentAccessIpAddr = null;
            WITNESS_INTERFACE_INFO registerInterface = new WITNESS_INTERFACE_INFO();
            WITNESS_INTERFACE_LIST interfaceList = new WITNESS_INTERFACE_LIST();

            currentAccessIpAddr = SWNTestUtility.GetCurrentAccessIP(server);
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Get current file server IP: {0}.", currentAccessIpAddr);

            #region Register SWN witness
            if (witnessType == WitnessType.SwnWitness)
            {
                if (TestConfig.IsWindowsPlatform && fsType == FileServerType.ScaleOutFileServer)
                {
                    // Windows Server: when stopping a non-owner node of ScaleOutFS, no notification will be sent by SMB witness.
                    // So get one IP of the owner node of ScaleOutFS to access.                    
                    string resourceOwnerNode = sutController.GetClusterResourceOwner(server);
                    IPAddress[] ownerIpList = Dns.GetHostEntry(resourceOwnerNode).AddressList;
                    foreach (var ip in ownerIpList)
                    {
                        BaseTestSite.Log.Add(LogEntryKind.Debug, "Owner IP: {0}", ip);
                    }
                    if (!ownerIpList.Contains(currentAccessIpAddr))
                    {
                        currentAccessIpAddr = null;
                        IPAddress[] accessIpList = Dns.GetHostEntry(server).AddressList;
                        foreach (var ip in accessIpList)
                        {
                            if (ownerIpList.Contains(ip))
                            {
                                currentAccessIpAddr = ip;
                                break;
                            }
                        }
                        BaseTestSite.Assert.IsNotNull(currentAccessIpAddr, "IP should not be null.");
                        BaseTestSite.Log.Add(LogEntryKind.Debug, "Get the owner IP {0} as file server IP.", currentAccessIpAddr);
                    }

                    DoUntilSucceed(() => SWNTestUtility.BindServer(swnClientForInterface, currentAccessIpAddr,
                        TestConfig.DomainName, TestConfig.UserName, TestConfig.UserPassword, TestConfig.DefaultSecurityPackage,
                        TestConfig.DefaultRpceAuthenticationLevel, TestConfig.Timeout, resourceOwnerNode), TestConfig.FailoverTimeout,
                        "Retry BindServer until succeed within timeout span");
                }
                else
                {
                    DoUntilSucceed(() => SWNTestUtility.BindServer(swnClientForInterface, currentAccessIpAddr,
                        TestConfig.DomainName, TestConfig.UserName, TestConfig.UserPassword, TestConfig.DefaultSecurityPackage,
                        TestConfig.DefaultRpceAuthenticationLevel, TestConfig.Timeout, server), TestConfig.FailoverTimeout,
                        "Retry BindServer until succeed within timeout span");
                }

                DoUntilSucceed(() =>
                {
                    ret = swnClientForInterface.WitnessrGetInterfaceList(out interfaceList);
                    BaseTestSite.Assert.AreEqual<SwnErrorCode>(
                        SwnErrorCode.ERROR_SUCCESS,
                        (SwnErrorCode)ret,
                        "WitnessrGetInterfaceList returns with result code = 0x{0:x8}", ret);
                    return SWNTestUtility.VerifyInterfaceList(interfaceList, TestConfig.Platform);
                }, TestConfig.FailoverTimeout, "Retry to call WitnessrGetInterfaceList until succeed within timeout span.");

                SWNTestUtility.GetRegisterInterface(interfaceList, out registerInterface);

                DoUntilSucceed(() => SWNTestUtility.BindServer(swnClientForWitness,
                    (registerInterface.Flags & (uint)SwnNodeFlagsValue.IPv4) != 0 ? new IPAddress(registerInterface.IPV4) : SWNTestUtility.ConvertIPV6(registerInterface.IPV6),
                    TestConfig.DomainName, TestConfig.UserName, TestConfig.UserPassword, TestConfig.DefaultSecurityPackage,
                    TestConfig.DefaultRpceAuthenticationLevel, TestConfig.Timeout, registerInterface.InterfaceGroupName), TestConfig.FailoverTimeout,
                    "Retry BindServer until succeed within timeout span");

                string clientName = TestConfig.WitnessClientName;

                BaseTestSite.Log.Add(LogEntryKind.Debug, "Register witness:");
                BaseTestSite.Log.Add(LogEntryKind.Debug, "\tNetName: {0}", SWNTestUtility.GetPrincipleName(TestConfig.DomainName, server));
                BaseTestSite.Log.Add(LogEntryKind.Debug, "\tIPAddress: {0}", currentAccessIpAddr.ToString());
                BaseTestSite.Log.Add(LogEntryKind.Debug, "\tClient Name: {0}", clientName);

                ret = swnClientForWitness.WitnessrRegister(SwnVersion.SWN_VERSION_1, SWNTestUtility.GetPrincipleName(TestConfig.DomainName, server),
                    currentAccessIpAddr.ToString(), clientName, out pContext);
                BaseTestSite.Assert.AreEqual<SwnErrorCode>(
                    SwnErrorCode.ERROR_SUCCESS,
                    (SwnErrorCode)ret,
                    "WitnessrRegister returns with result code = 0x{0:x8}", ret);
                BaseTestSite.Assert.IsNotNull(
                    pContext,
                    "Expect pContext is not null.");

                callId = swnClientForWitness.WitnessrAsyncNotify(pContext);
                BaseTestSite.Assert.AreNotEqual<uint>(
                    0,
                    callId,
                    "WitnessrAsyncNotify returns callId = {0}", callId);
            }
            #endregion

            #region Create a file and write content
            string uncSharePath = Smb2Utility.GetUncPath(server, TestConfig.ClusteredFileShare);
            string content = Smb2Utility.CreateRandomString(TestConfig.WriteBufferLengthInKb);
            string testDirectory = CreateTestDirectory(uncSharePath);
            string file = Path.Combine(testDirectory, Guid.NewGuid().ToString());
            Guid clientGuid = Guid.NewGuid();
            Guid createGuid = Guid.NewGuid();

            FILEID fileId = FILEID.Zero;
            DoUntilSucceed(() => WriteContentBeforeFailover(fsType, server, currentAccessIpAddr, uncSharePath, file, content, clientGuid, createGuid, out fileId),
                    TestConfig.FailoverTimeout,
                    "Before failover, retry Write content until succeed within timeout span.");
            #endregion

            #region Disable accessed node

            if (TestConfig.IsWindowsPlatform)
            {
                AssignCurrentAccessNode(server, fsType, currentAccessIpAddr);
            }


            if (!reconnectWithoutFailover)
            {
                BaseTestSite.Log.Add(
                    LogEntryKind.TestStep,
                    "Disable owner node for general file server or the node currently provides the access for scale-out file server.");
                FailoverServer(currentAccessIpAddr, server, fsType);
            }

            #endregion

            #region Wait for available server
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Wait for available server.");
            if (witnessType == WitnessType.None)
            {
                if (fsType == FileServerType.GeneralFileServer)
                {
                    currentAccessIpAddr = null;

                    DoUntilSucceed(() =>
                    {
                        this.sutController.FlushDNS();
                        IPAddress[] accessIpList = Dns.GetHostEntry(server).AddressList;
                        foreach (IPAddress ipAddress in accessIpList)
                        {
                            Smb2FunctionalClient pingClient = new Smb2FunctionalClient(TestConfig.FailoverTimeout, TestConfig, BaseTestSite);

                            try
                            {
                                pingClient.ConnectToServerOverTCP(ipAddress);
                                pingClient.Disconnect();
                                pingClient = null;

                                currentAccessIpAddr = ipAddress;
                                return true;
                            }
                            catch
                            {
                            }
                        }
                        return false;
                    }, TestConfig.FailoverTimeout, "Retry to ping to server until succeed within timeout span");
                }
                else
                {
                    currentAccessIpAddr = null;
                    DoUntilSucceed(() =>
                    {
                        this.sutController.FlushDNS();
                        IPAddress[] accessIpList = Dns.GetHostEntry(server).AddressList;
                        foreach (IPAddress ipAddress in accessIpList)
                        {
                            if (TestConfig.IsWindowsPlatform)
                            {
                                // When setting failover mode to StopNodeService for Windows, SMB2 servers on two nodes can still be accessed by the client.
                                // So the client needs to get the new node to access it after failover by comparing host name.
                                if (!reconnectWithoutFailover && string.Compare(currentAccessNode, Dns.GetHostEntry(ipAddress).HostName, true) == 0)
                                {
                                    continue;
                                }
                            }
                            Smb2FunctionalClient pingClient = new Smb2FunctionalClient(TestConfig.FailoverTimeout, TestConfig, BaseTestSite);

                            try
                            {
                                pingClient.ConnectToServerOverTCP(ipAddress);
                                pingClient.Disconnect();
                                pingClient = null;

                                currentAccessIpAddr = ipAddress;
                                return true;
                            }
                            catch
                            {
                            }
                        }
                        return false;
                    }, TestConfig.FailoverTimeout, "Retry to ping to server until succeed within timeout span");
                }
            }
            else if (witnessType == WitnessType.SwnWitness)
            {
                // Verifying for notification 
                RESP_ASYNC_NOTIFY respNotify;
                do
                {
                    // Wait the notification
                    ret = swnClientForWitness.ExpectWitnessrAsyncNotify(callId, out respNotify);
                    BaseTestSite.Assert.AreEqual<SwnErrorCode>(
                        SwnErrorCode.ERROR_SUCCESS,
                        (SwnErrorCode)ret,
                        "ExpectWitnessrAsyncNotify returns with result code = 0x{0:x8}", ret);
                    SWNTestUtility.PrintNotification(respNotify);

                    RESOURCE_CHANGE[] resourceChangeList;
                    SwnUtility.ParseResourceChange(respNotify, out resourceChangeList);
                    BaseTestSite.Assert.AreEqual<uint>(0x00000001, respNotify.NumberOfMessages, "Expect NumberOfMessages is set to 1.");

                    if (resourceChangeList[0].ChangeType == (uint)SwnResourceChangeType.RESOURCE_STATE_AVAILABLE)
                    {
                        // Verify RESP_ASYNC_NOTIFY, the resource is available
                        SWNTestUtility.VerifyResourceChange(respNotify, SwnResourceChangeType.RESOURCE_STATE_AVAILABLE);
                        break;
                    }

                    // Verify RESP_ASYNC_NOTIFY, the resource is unavailable
                    SWNTestUtility.VerifyResourceChange(respNotify, SwnResourceChangeType.RESOURCE_STATE_UNAVAILABLE);

                    callId = swnClientForWitness.WitnessrAsyncNotify(pContext);
                    BaseTestSite.Assert.AreNotEqual<uint>(0, callId, "WitnessrAsyncNotify returns callId = {0}", callId);
                } while (true);

                ret = swnClientForWitness.WitnessrUnRegister(pContext);
                BaseTestSite.Assert.AreEqual<SwnErrorCode>(
                    SwnErrorCode.ERROR_SUCCESS,
                    (SwnErrorCode)ret,
                    "WitnessrUnRegister returns with result code = 0x{0:x8}", ret);
                pContext = IntPtr.Zero;
                swnClientForWitness.SwnUnbind(TestConfig.Timeout);

                if (fsType == FileServerType.ScaleOutFileServer)
                {
                    // For scale-out file server case, retrieve and use another access IP for connection
                    currentAccessIpAddr =
                        (registerInterface.Flags & (uint)SwnNodeFlagsValue.IPv4) != 0 ? new IPAddress(registerInterface.IPV4) : SWNTestUtility.ConvertIPV6(registerInterface.IPV6);
                }
            }
            #endregion

            #region Read content and close the file
            BaseTestSite.Assert.AreNotEqual(
                null,
                currentAccessIpAddr,
                "Access IP to the file server should not be empty when reconnecting.");

            DoUntilSucceed(() => ReadContentAfterFailover(server, currentAccessIpAddr, uncSharePath, file, content, clientGuid, createGuid, fileId),
                    TestConfig.FailoverTimeout,
                    "After failover, retry Read content until succeed within timeout span.");
            #endregion
        }

        /// <summary>
        /// Verify members in Error_Context
        /// </summary>
        /// <param name="ctx">Error_Context in Error Response</param>
        /// <param name="uncSharePath">uncSharePath</param>
        /// <param name="sofsHostedNode">ScaleOutFS hosted node</param>
        private void verifyErrorContext(Error_Context ctx, string uncSharePath, string sofsHostedNode)
        {
            #region Verify Error_Context
            BaseTestSite.Assert.AreEqual(
                Error_Id.ERROR_ID_SHARE_REDIRECT,
                ctx.ErrorId,
                "The Error ID should be SMB2_ERROR_ID_SHARE_REDIRECT, actually server returns {0}", ctx.ErrorId);
            BaseTestSite.Assert.IsTrue(
                ctx.ErrorDataLength > 0,
                "The length, in bytes, of the ErrorContextData field should be greater than 0. Actually server returns {0}.", ctx.ErrorDataLength);
            #endregion

            #region Verify Share_Reditect_Error_Context_Response
            Share_Redirect_Error_Context_Response errCtx = ctx.ErrorData.ShareRedirectErrorContextResponse;
            BaseTestSite.Assert.IsTrue(
                errCtx.StructureSize > 0,
                "This field (StructureSize) MUST be set to the size of the structure. Actually server returns {0}.", errCtx.StructureSize);
            BaseTestSite.Assert.AreEqual(
                (uint)3,
                errCtx.NotificationType,
                "This field (NotificationType) MUST be set to 3. Actually server returns {0}.", errCtx.NotificationType);
            byte[] uncSharePathToByte = Encoding.Unicode.GetBytes(uncSharePath);
            BaseTestSite.Assert.AreEqual(
                (uint)uncSharePathToByte.Length,
                errCtx.ResourceNameLength,
                "The length of the share name provided in the ResourceName field, in bytes, should be the length of {0}. Actually server returns {1}.", uncSharePathToByte.Length, errCtx.ResourceNameLength);
            BaseTestSite.Assert.AreEqual(
                0,
                errCtx.Flags,
                "This field (Flags) MUST be set to zero. Actually server returns {0}.", errCtx.Flags);
            BaseTestSite.Assert.AreEqual(
                0,
                errCtx.TargetType,
                "This field (TargetType) MUST be set to zero. Actually server returns {0}.", errCtx.TargetType);
            BaseTestSite.Assert.IsTrue(
                errCtx.IPAddrCount > 0,
                "The number of MOVE_DST_IPADDR structures in the IPAddrMoveList field should be greater than 0. Actually server returns {0}.", errCtx.IPAddrCount);
            IPAddress[] ipv4AddressList = Dns.GetHostEntry(sofsHostedNode).AddressList;
            System.Net.Sockets.AddressFamily addressFamily = ipv4AddressList[0].AddressFamily;

            for (int i = 0; i < errCtx.IPAddrCount; i++)
            {
                Move_Dst_IpAddr dstMoveIpAddr = errCtx.IPAddrMoveList[i];
                BaseTestSite.Assert.AreEqual(
                    (uint)0,
                    dstMoveIpAddr.Reserved,
                    "The server SHOULD set this field to zero, and the client MUST ignore it on receipt. Actually server returns {0}", dstMoveIpAddr.Reserved);
                if (addressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    BaseTestSite.Assert.AreEqual(
                        Move_Dst_IpAddr_Type.MOVE_DST_IPADDR_V4,
                        dstMoveIpAddr.Type,
                        "Type of destination IP address should be MOVE_DST_IPADDR_V4, actually server returns {0}", dstMoveIpAddr.Type);
                    // Check Reserved2 field (offset from 4 to 15) is 0
                    for (int k = 4; k < 16; k++)
                    {
                        BaseTestSite.Assert.IsTrue(
                            dstMoveIpAddr.IPv6Address[k] == 0,
                            "The client MUST set this (Reserved2) to 0.");
                    }
                }
                else if (addressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    BaseTestSite.Assert.AreEqual(
                        Move_Dst_IpAddr_Type.MOVE_DST_IPADDR_V6,
                        dstMoveIpAddr.Type,
                        "Type of destination IP address should be MOVE_DST_IPADDR_V6, actually server returns {0}", dstMoveIpAddr.Type);
                }
            }

            // Verify all IP addresses for this node are in IPAddrMoveList
            if (addressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                for (int i = 0; i < ipv4AddressList.Length; i++)
                {
                    IPAddress curAddr = ipv4AddressList[i];
                    bool isIpAddrInList = false;
                    for (int j = 0; j < errCtx.IPAddrMoveList.Length; j++)
                    {
                        Move_Dst_IpAddr dstMoveIpAddr = errCtx.IPAddrMoveList[j];
                        uint ipAddr = BitConverter.ToUInt32(dstMoveIpAddr.IPv6Address, 0);
                        if (ipAddr == (uint)curAddr.Address)
                        {
                            isIpAddrInList = true;
                            break;
                        }
                    }
                    BaseTestSite.Assert.IsTrue(isIpAddrInList, "IP address of sofsHostedNode {0} is not in IPAddrMoveList.", curAddr.ToString());
                }
            }

            string resourceName = Encoding.Unicode.GetString(errCtx.ResourceName, 0, (int)errCtx.ResourceNameLength);
            BaseTestSite.Assert.IsTrue(
                uncSharePath.ToLower().Contains(resourceName.ToLower()),
                "ResourceName should be the same as uncSharePath. Actually server returns {0}.", resourceName);
            #endregion
        }

        /// <summary>
        /// Verify behavior of redirect to owner specified in MS-SMB2 section 3.3.5.7
        /// </summary>
        /// <param name="sofsHostedNode">ScaleOutFS hosted node</param>
        /// <param name="nonSofsHostedNode">Non ScaleOutFS hosted node</param>
        /// <returns>Redirect to owner is tested or not</returns>
        private bool TestRedirectToOwner(string sofsHostedNode, string nonSofsHostedNode)
        {
            bool isRedirectToOwnerTested = false;
            #region Get IP address list from ScaleOutFS
            string server = TestConfig.ClusteredScaleOutFileServerName;
            IPAddress[] accessIpList = Dns.GetHostEntry(server).AddressList;
            #endregion

            #region Get IP address from nonSofsHostedNode
            IPAddress currentAccessIpAddr = null;
            IPAddress[] accessIpListNonSoftHosted = Dns.GetHostEntry(nonSofsHostedNode).AddressList;
            for (int i = 0; i < accessIpList.Length; i++)
            {
                for (int j = 0; j < accessIpListNonSoftHosted.Length; j++)
                {
                    // Make sure to get IP address from nonSofsHostedNode
                    if (accessIpList[i].Address.Equals(accessIpListNonSoftHosted[j].Address))
                    {
                        currentAccessIpAddr = accessIpList[i];
                        break;
                    }
                }
            }
            BaseTestSite.Assert.IsNotNull(
                currentAccessIpAddr,
                "currentAccessIpAddr should be set as IP of nonSofsHostedNode {0}", nonSofsHostedNode);
            #endregion

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start a client by sending the following requests: NEGOTIATE; SESSION_SETUP");
            Smb2FunctionalClient client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client.ConnectToServer(TestConfig.UnderlyingTransport, server, currentAccessIpAddr);
            client.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);
            client.SessionSetup(TestConfig.DefaultSecurityPackage, server, TestConfig.AccountCredential, false);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends TREE_CONNECT request with flag SMB2_SHAREFLAG_REDIRECT_TO_OWNER.");
            string uncSharePath = Smb2Utility.GetUncPath(server, testConfig.CAShareName);
            uint treeId;
            Share_Capabilities_Values shareCap = Share_Capabilities_Values.NONE;
            uint status = client.TreeConnect(
                uncSharePath,
                out treeId,
                (header, response) =>
                {
                    if (header.Status == Smb2Status.STATUS_SUCCESS)
                    {
                        shareCap = response.Capabilities;
                    }
                },
                TreeConnect_Flags.SMB2_SHAREFLAG_REDIRECT_TO_OWNER);

            if (status != Smb2Status.STATUS_SUCCESS &&
                client.Smb2Client.Error != null)
            {
                ERROR_Response_packet error = client.Smb2Client.Error.PayLoad;
                if (error.ErrorContextCount > 0)
                {
                    for (int i = 0; i < error.ErrorContextCount; i++)
                    {
                        Error_Context ctx = error.ErrorContextErrorData[i];
                        if (ctx.ErrorId == Error_Id.ERROR_ID_SHARE_REDIRECT)
                        {
                            Share_Capabilities_Values shareCaps = GetShareCapabilities(sofsHostedNode, uncSharePath, TreeConnect_Flags.SMB2_SHAREFLAG_REDIRECT_TO_OWNER);
                            if (!shareCaps.HasFlag(Share_Capabilities_Values.SHARE_CAP_REDIRECT_TO_OWNER))
                            {
                                BaseTestSite.Assert.Inconclusive(
                                    "The share {0} does not have the capability SHARE_CAP_REDIRECT_TO_OWNER",
                                    Smb2Utility.GetUncPath(TestConfig.ClusteredScaleOutFileServerName, testConfig.CAShareName)
                                );
                            }

                            BaseTestSite.Assert.AreEqual(
                                Smb2Status.STATUS_BAD_NETWORK_NAME,
                                status,
                                "If TreeConnect.Share.Type includes STYPE_CLUSTER_SOFS," +
                                "Connection.Dialect is \"3.1.1\" and" +
                                "the SMB2_TREE_CONNECT_FLAG_REDIRECT_TO_OWNER bit is set" +
                                "in the Flags field of the SMB2 TREE_CONNECT request," +
                                "the server MUST query the underlying object store in an implementation-specific manner " +
                                "to determine whether the share is hosted on this node." +
                                "If not, the server MUST return error data as specified in section 2.2.2" +
                                "with ErrorData set to SMB2 ERROR Context response formatted as ErrorId" +
                                "set to SMB2_ERROR_ID_SHARE_REDIRECT, and ErrorContextData set to the Share Redirect error context data" +
                                "as specified in section 2.2.2.2.2 with IPAddrMoveList set to" +
                                "the list of IP addresses obtained in an implementation-specific manner." +
                                "Actually server returns {0}.", Smb2Status.GetStatusCode(status)
                            );
                            verifyErrorContext(ctx, uncSharePath, sofsHostedNode);
                            isRedirectToOwnerTested = true;
                        }
                    }
                }
            }

            if (status == Smb2Status.STATUS_SUCCESS)
            {
                if (!shareCap.HasFlag(Share_Capabilities_Values.SHARE_CAP_REDIRECT_TO_OWNER))
                {
                    BaseTestSite.Assert.Inconclusive(
                        "The share {0} does not have the capability SHARE_CAP_REDIRECT_TO_OWNER",
                        Smb2Utility.GetUncPath(TestConfig.ClusteredScaleOutFileServerName, testConfig.CAShareName)
                    );
                }
                client.TreeDisconnect(treeId);
            }
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the client by sending the following requests: LOG_OFF; DISCONNECT.");
            client.LogOff();
            client.Disconnect();
            return isRedirectToOwnerTested;
        }

        /// <summary>
        /// Get share capabilities
        /// </summary>
        /// <param name="server">The host node</param>
        /// <param name="sharePath">The path to the share</param>
        /// <param name="flags">Tree Connect flag</param>
        /// <returns>Share capabilities</returns>
        private Share_Capabilities_Values GetShareCapabilities(string server, string sharePath, TreeConnect_Flags flags = TreeConnect_Flags.SMB2_SHAREFLAG_NONE)
        {
            IPAddress shareIpAddr = Dns.GetHostEntry(server).AddressList[0];

            Smb2FunctionalClient client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client.ConnectToServer(TestConfig.UnderlyingTransport, server, shareIpAddr);
            client.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);
            client.SessionSetup(TestConfig.DefaultSecurityPackage, server, TestConfig.AccountCredential, false);
            Share_Capabilities_Values shareCaps = Share_Capabilities_Values.NONE;
            uint treeId;
            uint status = client.TreeConnect(
                sharePath,
                out treeId,
                (header, response) =>
                {
                    shareCaps = response.Capabilities;
                },
                flags);
            if (status == Smb2Status.STATUS_SUCCESS)
            {
                client.TreeDisconnect(treeId);
            }
            client.LogOff();
            client.Disconnect();
            return shareCaps;
        }
        #endregion
    }
}
