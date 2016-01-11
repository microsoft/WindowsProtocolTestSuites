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
        [TestCategory(TestCategories.Cluster)]
        [TestCategory(TestCategories.Smb30)]
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
        [TestCategory(TestCategories.Cluster)]
        [TestCategory(TestCategories.Smb30)]
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
        [TestCategory(TestCategories.Cluster)]
        [TestCategory(TestCategories.Smb30)]
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
                    // Windows Server: when stopping a non-owner node of ScaleOutFS, no notication will be sent by SMB witness.
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

            DoUntilSucceed(() => WriteContentBeforeFailover(fsType, server, currentAccessIpAddr, uncSharePath, file, content, clientGuid, createGuid),
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
                    IPAddress[] accessIpList = Dns.GetHostEntry(server).AddressList;
                    DoUntilSucceed(() =>
                    {
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

                    IPAddress[] accessIpList = Dns.GetHostEntry(server).AddressList;
                    foreach (IPAddress ipAddress in accessIpList)
                    {
                        if (TestConfig.IsWindowsPlatform)
                        {
                            // When setting failover mode to StopNodeService for Windows, SMB2 servers on two nodes can still be accessed by the client.
                            // So the client needs to get the new node to access it after failover by comparing host name.
                            if (string.Compare(currentAccessNode, Dns.GetHostEntry(ipAddress).HostName, true) == 0)
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
                            break;
                        }
                        catch
                        {
                        }
                    }
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
            DoUntilSucceed(() => ReadContentAfterFailover(server, currentAccessIpAddr, uncSharePath, file, content, clientGuid, createGuid),
                    TestConfig.FailoverTimeout,
                    "After failover, retry Read content until succeed within timeout span.");
            #endregion
        }        

        #endregion
    }
}
