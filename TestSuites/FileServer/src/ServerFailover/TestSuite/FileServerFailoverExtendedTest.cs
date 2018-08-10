// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;

namespace Microsoft.Protocols.TestSuites.FileSharing.ServerFailover.TestSuite
{
    [TestClass]
    public partial class FileServerFailoverExtendedTest : ServerFailoverTestBase
    {
        #region Variables
        private uint status;
        private Guid clientGuid;
        private string fileName;
        private string testDirectory;
        private string uncSharePath;
        private string contentWrite;
        private string contentRead;
        private Smb2FunctionalClient clientBeforeFailover;
        private Smb2FunctionalClient clientAfterFailover;

        /// <summary>
        /// Ip address for access to the file server
        /// It's also used to failover by Non-Windows implementation
        /// </summary>
        private IPAddress currentAccessIp;

        /// <summary>
        /// All possible Ip address for access to the file server
        /// </summary>
        private IPAddress[] accessIpList;

        /// <summary>
        /// Guid when create an open on a file, 
        /// </summary>
        private Guid createGuid;

        /// <summary>
        /// Guid when request leasing on an open
        /// </summary>
        private Guid leaseKey;
        #endregion

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

            clientGuid = Guid.NewGuid();
            clientBeforeFailover = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            clientAfterFailover = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            accessIpList = Dns.GetHostEntry(TestConfig.ClusteredFileServerName).AddressList;
            uncSharePath = Smb2Utility.GetUncPath(TestConfig.ClusteredFileServerName, TestConfig.ClusteredFileShare);
            contentWrite = Smb2Utility.CreateRandomString(TestConfig.WriteBufferLengthInKb);
            fileName = GetTestFileName(uncSharePath);
        }

        protected override void TestCleanup()
        {
            if (clientBeforeFailover != null)
            {
                try
                {
                    clientBeforeFailover.Disconnect();
                }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(
                        LogEntryKind.Debug,
                        "Unexpected exception when disconnect clientBeforeFailover: {0}", ex.ToString());
                }
            }

            if (clientAfterFailover != null)
            {
                try
                {
                    clientAfterFailover.Disconnect();
                }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(
                        LogEntryKind.Debug,
                        "Unexpected exception when disconnect clientAfterFailover: {0}", ex.ToString());
                }
            }

            base.TestCleanup();
        }
        #endregion

        #region Common Method


        /// <summary>
        /// Get the owner node of general clustered file server
        /// </summary>
        /// <param name="server">Name of clustered file server</param>
        /// <param name="fsType">Type of clustered file server</param>
        private void GetGeneralFileServerClusterOwner(string server)
        {
            currentAccessIp = null;
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

                        currentAccessIp = ipAddress;
                        return true;
                    }
                    catch
                    {
                    }
                }
                return false;
            }, TestConfig.FailoverTimeout, "Retry to ping to server until succeed within timeout span");

            BaseTestSite.Assert.AreNotEqual(
                null,
                currentAccessIp,
                "Access IP to the file server should NOT be empty");
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Got IP {0} to access the file server", currentAccessIp.ToString());

            if (TestConfig.IsWindowsPlatform)
            {
                AssignCurrentAccessNode(server, FileServerType.GeneralFileServer, currentAccessIp);
            }
        }

        /// <summary>
        /// Connect to share on general clustered file server before failover happens, include NEGOTIATE, SESSION_SETUP, TREE_CONNECT
        /// </summary>
        /// <param name="server">Name of clustered file server</param>
        /// <param name="fsType">Type of clustered file server</param>
        /// <param name="treeId">Out param for tree Id that connected</param>
        /// <param name="enableEncryptionPerShare">Set true if enable encryption on share, otherwise set false</param>
        private void ConnectGeneralFileServerBeforeFailover(string server, out uint treeId, bool enableEncryptionPerShare = false)
        {
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "BeforeFailover: Connect share {0} on server {1} with following steps.", uncSharePath, server);
            GetGeneralFileServerClusterOwner(server);

            clientBeforeFailover.ConnectToServer(TestConfig.UnderlyingTransport, server, currentAccessIp);

            #region Negotiate
            Capabilities_Values clientCapabilitiesBeforeFAilover = Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES | Capabilities_Values.GLOBAL_CAP_ENCRYPTION;
            status = clientBeforeFailover.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                capabilityValue: clientCapabilitiesBeforeFAilover,
                clientGuid: clientGuid,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    TestConfig.CheckNegotiateDialect(DialectRevision.Smb30, response);
                }
                );
            #endregion

            #region SESSION_SETUP
            status = clientBeforeFailover.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                server,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Global encryption disabled");
            #endregion

            #region TREE_CONNECT to share
            status = clientBeforeFailover.TreeConnect(
                uncSharePath,
                out treeId,
                (Packet_Header header, TREE_CONNECT_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "TreeConnect should succeed.");

                    if (enableEncryptionPerShare)
                    {
                        BaseTestSite.Assert.AreEqual(
                            ShareFlags_Values.SHAREFLAG_ENCRYPT_DATA,
                            ShareFlags_Values.SHAREFLAG_ENCRYPT_DATA & response.ShareFlags,
                            "Server should set SMB2_SHAREFLAG_ENCRYPT_DATA for ShareFlags field in TREE_CONNECT response");
                    }

                    BaseTestSite.Assert.IsTrue(response.Capabilities.HasFlag(Share_Capabilities_Values.SHARE_CAP_CONTINUOUS_AVAILABILITY),
                        "CA Share should have SHARE_CAP_CONTINUOUS_AVAILABILITY bit set for Capabilities in TreeConnect response.");
                });

            clientBeforeFailover.SetTreeEncryption(treeId, enableEncryptionPerShare);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Per share encryption for TreeId=0x{0:x} : {1}", treeId, enableEncryptionPerShare);
            #endregion
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "BeforeFailover: Finish connecting to share {0} on server {1}", uncSharePath, server);
        }

        /// <summary>
        /// Reconnect to share on clustered file server after failover happens, include NEGOTIATE, SESSION_SETUP, TREE_CONNECT
        /// </summary>
        /// <param name="server">Name of clustered file server</param>
        /// <param name="fsType">Type of clustered file server</param>
        /// <param name="treeId">Out param for tree Id that connected</param>
        /// <param name="enableEncryptionPerShare">Set true if enable encryption on share, otherwise set false</param>
        private void ReconnectServerAfterFailover(string server, FileServerType fsType, out uint treeId, bool enableEncryptionPerShare = false)
        {
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "AfterFailover: Connect share {0} on server {1} with following steps.", uncSharePath, server);
            if (fsType == FileServerType.ScaleOutFileServer)
            {
                // For scale-out file server case, retrieve and use another access IP for connection
                IPAddress oldAccessIp = currentAccessIp;
                currentAccessIp = null;

                foreach (var ip in accessIpList)
                {
                    if (!ip.Equals(oldAccessIp))
                    {
                        currentAccessIp = ip;
                        break;
                    }
                }
            }
            else if (fsType == FileServerType.GeneralFileServer)
            {
                currentAccessIp = null;
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

                            currentAccessIp = ipAddress;
                            return true;
                        }
                        catch
                        {
                        }
                    }
                    return false;
                }, TestConfig.FailoverTimeout, "Retry to ping to server until succeed within timeout span");
            }
            BaseTestSite.Assert.AreNotEqual(
                null,
                currentAccessIp,
                "Access IP to the file server should not be empty");
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Got IP {0} to access the file server",
                    currentAccessIp.ToString());

            DoUntilSucceed(() => clientAfterFailover.ConnectToServer(TestConfig.UnderlyingTransport, server, currentAccessIp), TestConfig.FailoverTimeout,
                "Retry to connect to server until succeed within timeout span");

            #region Negotiate
            Capabilities_Values clientCapabilitiesAfterFailover = Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES | Capabilities_Values.GLOBAL_CAP_ENCRYPTION;
            status = clientAfterFailover.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                capabilityValue: clientCapabilitiesAfterFailover,
                clientGuid: clientGuid,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    TestConfig.CheckNegotiateDialect(DialectRevision.Smb30, response);
                });
            #endregion

            #region SESSION_SETUP
            status = clientAfterFailover.ReconnectSessionSetup(
                        clientBeforeFailover,
                        TestConfig.DefaultSecurityPackage,
                        server,
                        TestConfig.AccountCredential,
                        TestConfig.UseServerGssToken);
            
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Global encryption disabled");
            #endregion

            #region TREE_CONNECT to share
            uint innerTreeId = 0;
            //Retry TreeConnect until succeed within timeout span
            status = DoUntilSucceed(
                () => clientAfterFailover.TreeConnect(uncSharePath, out innerTreeId, (header, response) => { }),
                TestConfig.FailoverTimeout,
                "Retry TreeConnect until succeed within timeout span");
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "TreeConnect to {0} should succeed, actual status is {1}", uncSharePath, Smb2Status.GetStatusCode(status));
            treeId = innerTreeId;

            clientAfterFailover.SetTreeEncryption(innerTreeId, enableEncryptionPerShare);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Per share encryption for TreeId=0x{0:x} : {1}", treeId, enableEncryptionPerShare);
            #endregion

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "AfterFailover: Finish connecting to share {0} on server {1}", uncSharePath, server);
        }

        #endregion
    }
}
