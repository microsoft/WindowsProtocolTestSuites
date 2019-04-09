// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestSuites.FileSharing.ServerFailover.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using System;
using System.IO;
using System.Net;

namespace Microsoft.Protocols.TestSuites.FileSharing.ServerFailover.TestSuite
{
    public class ServerFailoverTestBase : SMB2TestBase
    {
        public new ServerFailoverTestConfig TestConfig
        {
            get
            {
                return testConfig as ServerFailoverTestConfig;
            }
        }
        protected ISutControlAdapter sutController;

        #region For FileServerFailover

        /// <summary>
        /// Test endpoint before failover happens
        /// </summary>
        protected Smb2FunctionalClient beforeFailover = null;

        /// <summary>
        /// The current access node's name, used only by Windows, assigned by function AssignCurrentAccessNode
        /// It's used in failover/restore node.
        /// </summary>
        protected string currentAccessNode = null;

        protected bool isAccessNodeFailovered = false;

        #endregion

        protected override void TestInitialize()
        {
            base.TestInitialize();

            testConfig = new ServerFailoverTestConfig(BaseTestSite);
            sutController = BaseTestSite.GetAdapter<ISutControlAdapter>();
        }

        protected override void TestCleanup()
        {
            if (beforeFailover != null)
            {
                beforeFailover.Disconnect();
            }

            RestoreServer();

            base.TestCleanup();
        }

        protected void AssignCurrentAccessNode(string server, FileServerType fsType, IPAddress currentAccessIpAddr)
        {
            if (fsType == FileServerType.GeneralFileServer)
            {
                currentAccessNode = sutController.GetClusterResourceOwner(server);
            }
            else
            {
                currentAccessNode = Dns.GetHostEntry(currentAccessIpAddr).HostName;
            }

            BaseTestSite.Assert.AreNotEqual(
                 true,
                 string.IsNullOrEmpty(currentAccessNode),
                 "Current owner node is NOT expected to be Null or Empty");

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Current owner node is {0}", currentAccessNode);
        }

        #region Failover/Restore ClusterNode

        protected void FailoverClusterNode(string clusterNode)
        {
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Disable cluster service on {0}", clusterNode);

            DoUntilSucceed(() => sutController.DisableClusterNode(clusterNode), TestConfig.FailoverTimeout,
                "Retry to stop cluster service on {0} until succeed within timeout span", clusterNode);
        }

        protected void RestoreClusterNodes(params string[] servers)
        {
            // Restore cluster node
            foreach (string node in servers)
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Restore cluster node {0}", node);
                DoUntilSucceed(() => sutController.EnableClusterNode(node), TestConfig.FailoverTimeout,
                    "Retry to restore cluster node {0} until succeed within timeout span", node);
            }

            if (!TestConfig.IsWindowsPlatform)
                return;

            // Only works for windows.
            // Check node availability
            foreach (string node in servers)
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Make sure cluster node {0}", node);

                DoUntilSucceed(() => sutController.GetClusterNodeStatus(node) == "Running", TestConfig.FailoverTimeout,
                    "Retry to make sure cluster node {0} is in Running state", node);
            }

            // Check cluster resource availability
            foreach (string resource in new string[] { TestConfig.ClusteredFileServerName, TestConfig.ClusteredScaleOutFileServerName })
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Make sure cluster resource {0} is ready.", resource);

                DoUntilSucceed(() => !string.IsNullOrEmpty(sutController.GetClusterResourceOwner(resource)), TestConfig.FailoverTimeout,
                        "Retry to make sure cluster resource {0} is ready.", resource);

                BaseTestSite.Log.Add(LogEntryKind.Debug, "Make sure cluster share {0} is ready.", TestConfig.ClusteredFileShare);
                string uncSharePath = Smb2Utility.GetUncPath(resource, TestConfig.ClusteredFileShare);
                string testDirectory = "RestoreCluster_" + Guid.NewGuid().ToString();

                DoUntilSucceed(() => sutProtocolController.CreateDirectory(uncSharePath, testDirectory),
                    testConfig.Timeout,
                    "Retry to make sure cluster share is ready by creating test directory.");

                DoUntilSucceed(() => sutProtocolController.DeleteDirectory(uncSharePath, testDirectory),
                    testConfig.Timeout,
                    "Retry to make sure cluster share is ready by deleting test directory.");
            }
        }
        #endregion

        #region Failover/Restore Server
        /// <summary>
        /// File Server failover 
        /// For windows, simulated by disabling current access node of clustered file server
        /// </summary>
        /// <param name="server">Name of clustered file server</param>
        /// <param name="fsType">Type of clustered file server</param>
        protected void FailoverServer(IPAddress currentAccessIp, string server, FileServerType fsType)
        {
            isAccessNodeFailovered = true;
            // Non Windows
            if (!TestConfig.IsWindowsPlatform)
            {
                sutController.TriggerFailover(currentAccessIp.ToString());
                return;
            }

            // Windows
            #region Failover accessed node
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Failover node {0}", currentAccessNode);

            FailoverClusterNode(currentAccessNode);

            #endregion

            #region Wait for available server
            if (fsType == FileServerType.GeneralFileServer)
            {
                string newOwnerNode = null;
                //Wait the file server back online
                DoUntilSucceed(() =>
                {
                    newOwnerNode = sutController.GetClusterResourceOwner(server);
                    return (!string.IsNullOrEmpty(newOwnerNode) && newOwnerNode.ToUpper() != currentAccessNode.ToUpper());
                }, TestConfig.FailoverTimeout, "Retry to get cluster owner node until succeed within timeout span.");

                BaseTestSite.Assert.AreNotEqual(
                    null,
                    newOwnerNode,
                    "New owner node should not be null.");
                BaseTestSite.Assert.AreNotEqual(
                    currentAccessNode.ToUpper(),
                    newOwnerNode.ToUpper(),
                    "New owner node should not be the same as the old one.");
                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Current owner node changed to {0}", newOwnerNode);
            }
            #endregion
        }

        /// <summary>
        /// Restore File Server 
        /// For windows, simulated by enabling current access node of clustered file server
        /// </summary>
        protected void RestoreServer()
        {
            if (isAccessNodeFailovered)
            {
                if (TestConfig.IsWindowsPlatform)
                {
                    RestoreClusterNodes(currentAccessNode);
                }
                else
                {
                    sutController.RestoreToInitialState();
                }
            }
        }
        #endregion

        #region Write content before failover

        /// <summary>
        /// Write content before failover
        /// </summary>
        /// <param name="fsType">FileServerType</param>
        /// <param name="server">File Server name.</param>
        /// <param name="serverAccessIp">File Server Access IP.</param>
        /// <param name="uncSharePath">The share path to write the file.</param>
        /// <param name="file">The file name for writing content.</param>
        /// <param name="content">The content to write.</param>
        /// <param name="clientGuid">Smb2 client Guid.</param>
        /// <param name="createGuid">The Guid for smb2 create request.</param>
        /// <param name="fileId">File id returned by server</param>
        /// <returns></returns>
        protected bool WriteContentBeforeFailover(
            FileServerType fsType,
            string server,
            IPAddress serverAccessIp,
            string uncSharePath,
            string file,
            string content,            
            Guid clientGuid,
            Guid createGuid,
            out FILEID fileId)
        {
            uint status = 0;
            fileId = FILEID.Zero;
            beforeFailover = new Smb2FunctionalClient(TestConfig.FailoverTimeout, TestConfig, BaseTestSite);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start a client by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT to {0}", uncSharePath);            
            
            beforeFailover.ConnectToServer(TestConfig.UnderlyingTransport, server, serverAccessIp);

            Capabilities_Values requestCapabilities = Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES;            
            status = beforeFailover.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                capabilityValue: requestCapabilities,
                clientGuid: clientGuid,
                checker: (header, response) =>
                {
                    TestConfig.CheckNegotiateDialect(DialectRevision.Smb30, response);
                });
            if (status != Smb2Status.STATUS_SUCCESS)
            {
                BaseTestSite.Log.Add(LogEntryKind.Warning, "Negotiate failed with {0}.", Smb2Status.GetStatusCode(status));
                return false;
            }

            status = beforeFailover.SessionSetup(
                    TestConfig.DefaultSecurityPackage,
                    server,
                    TestConfig.AccountCredential,
                    TestConfig.UseServerGssToken);
            if (status != Smb2Status.STATUS_SUCCESS)
            {
                BaseTestSite.Log.Add(LogEntryKind.Warning, "SessionSetup failed with {0}.", Smb2Status.GetStatusCode(status));
                return false;
            }

            uint treeId = 0;
            Share_Capabilities_Values shareCapabilities = Share_Capabilities_Values.NONE;
            status = DoUntilSucceed(
                () => beforeFailover.TreeConnect(uncSharePath, out treeId, (header, response) =>
                {
                    shareCapabilities = response.Capabilities;
                }),
                TestConfig.FailoverTimeout,
                "Retry TreeConnect until succeed within timeout span");

            if (status != Smb2Status.STATUS_SUCCESS)
            {
                BaseTestSite.Log.Add(LogEntryKind.Warning, "TreeConnect failed with {0}.", Smb2Status.GetStatusCode(status));
                return false;
            }

            BaseTestSite.Assert.IsTrue(shareCapabilities.HasFlag(Share_Capabilities_Values.SHARE_CAP_CONTINUOUS_AVAILABILITY),
                "CA Share should have SHARE_CAP_CONTINUOUS_AVAILABILITY bit set for Capabilities in TreeConnect response.");

            if (fsType == FileServerType.ScaleOutFileServer)
            {
                BaseTestSite.Assert.IsTrue(shareCapabilities.HasFlag(Share_Capabilities_Values.SHARE_CAP_SCALEOUT),
                    "ScaleOut FS should have SHARE_CAP_SCALEOUT bit set for Capabilities in TreeConnect response.");
            }

            Smb2CreateContextResponse[] serverCreateContexts;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends CREATE request with SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 with PERSISTENT flag set.");
            status = beforeFailover.Create(
                treeId,
                file,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                new Smb2CreateContextRequest[] { 
                    new Smb2CreateDurableHandleRequestV2
                    {
                         CreateGuid = createGuid,
                         Flags = CREATE_DURABLE_HANDLE_REQUEST_V2_Flags.DHANDLE_FLAG_PERSISTENT,
                         Timeout = 3600000,
                    },
                });
            if (status != Smb2Status.STATUS_SUCCESS)
            {
                BaseTestSite.Log.Add(LogEntryKind.Warning, "Create failed with {0}.", Smb2Status.GetStatusCode(status));
                return false;
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends WRITE request to write content to the file.");
            status = beforeFailover.Write(treeId, fileId, content);
            if (status != Smb2Status.STATUS_SUCCESS)
            {
                BaseTestSite.Log.Add(LogEntryKind.Warning, "Write content failed with {0}.", Smb2Status.GetStatusCode(status));
                return false;
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends FLUSH request.");
            status = beforeFailover.Flush(treeId, fileId);
            if (status != Smb2Status.STATUS_SUCCESS)
            {
                BaseTestSite.Log.Add(LogEntryKind.Warning, "Flush failed with {0}.", Smb2Status.GetStatusCode(status));
                return false;
            }

            return true;
        }

        #endregion

        #region Read content after failover
        /// <summary>
        /// Read content after failover
        /// </summary>
        /// <param name="server">File server name.</param>
        /// <param name="serverAccessIp">File server access IP.</param>
        /// <param name="uncSharePath">The share path to read the file.</param>
        /// <param name="file">The file name for reading content.</param>
        /// <param name="content">The content to read.</param>
        /// <param name="clientGuid">Smb2 client Guid.</param>
        /// <param name="createGuid">The Guid for smb2 create request.</param>
        /// <param name="fileId">FileId used in DH2C create context</param>
        /// <returns></returns>
        protected bool ReadContentAfterFailover(string server,
            IPAddress serverAccessIp,
            string uncSharePath,
            string file,
            string content,
            Guid clientGuid,
            Guid createGuid,
            FILEID fileId)
        {
            uint status;

            BaseTestSite.Assert.AreNotEqual(
                null,
                serverAccessIp,
                "Access IP to the file server should not be empty");
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Got IP {0} to access the file server", serverAccessIp.ToString());

            Smb2FunctionalClient afterFailover = new Smb2FunctionalClient(TestConfig.FailoverTimeout, TestConfig, BaseTestSite);
            DoUntilSucceed(() => afterFailover.ConnectToServer(TestConfig.UnderlyingTransport, server, serverAccessIp), TestConfig.FailoverTimeout,
                "Retry to connect to server until succeed within timeout span");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends NEGOTIATE request with the same clientguid of previous client.");
            status = afterFailover.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                capabilityValue: Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES,
                clientGuid: clientGuid);
            if (status != Smb2Status.STATUS_SUCCESS)
            {
                BaseTestSite.Log.Add(LogEntryKind.Warning, "Negotiate failed with {0}.", Smb2Status.GetStatusCode(status));
                return false;
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends SESSION_SETUP request with the same SESSION_ID of previous client.");
            status = afterFailover.ReconnectSessionSetup(
                        beforeFailover,
                        TestConfig.DefaultSecurityPackage,
                        server,
                        TestConfig.AccountCredential,
                        TestConfig.UseServerGssToken);
            if (status != Smb2Status.STATUS_SUCCESS)
            {
                BaseTestSite.Log.Add(LogEntryKind.Warning, "ReconnectSessionSetup failed with {0}.", Smb2Status.GetStatusCode(status));
                return false;
            }

            // Retry TreeConnect because network path may not be available immediately
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client retries TREE_CONNECT to {0} until succeed or timeout in {1} because network path may not be available immediately.", uncSharePath, TestConfig.FailoverTimeout);
            uint treeId = 0;
            status = afterFailover.TreeConnect(uncSharePath, out treeId, (header, response) => { });
            if (status != Smb2Status.STATUS_SUCCESS)
            {
                BaseTestSite.Log.Add(LogEntryKind.Warning, "TreeConnect failed with {0}.", Smb2Status.GetStatusCode(status));
                return false;
            }

            // Retry Create because file may not be available immediately
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client retries to send CREATE request with SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 context with PERSISTENT flag set until succeed or timeout in {0}.", TestConfig.FailoverTimeout);
            Smb2CreateContextResponse[] serverCreateContexts;
            status = DoUntilSucceed(
                () => afterFailover.Create(
                        treeId,
                        file,
                        CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                        out fileId,
                        out serverCreateContexts,
                        RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                        new Smb2CreateContextRequest[] { 
                            new Smb2CreateDurableHandleReconnectV2
                            {
                                FileId = new FILEID { Persistent = fileId.Persistent },
                                CreateGuid = createGuid,
                                Flags = CREATE_DURABLE_HANDLE_RECONNECT_V2_Flags.DHANDLE_FLAG_PERSISTENT
                            },
                        },
                        checker: (header, response) => { }),
                TestConfig.FailoverTimeout,
                "Retry Create until succeed within timeout span");
            if (status != Smb2Status.STATUS_SUCCESS)
            {
                BaseTestSite.Log.Add(LogEntryKind.Warning, "Create failed with {0}.", Smb2Status.GetStatusCode(status));
                return false;
            }

            string readContent;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends READ request to read content.");
            status = afterFailover.Read(treeId, fileId, 0, (uint)content.Length, out readContent);
            if (status != Smb2Status.STATUS_SUCCESS)
            {
                BaseTestSite.Log.Add(LogEntryKind.Warning, "Read failed with {0}.", Smb2Status.GetStatusCode(status));
                return false;
            }

            BaseTestSite.Assert.IsTrue(
                content.Equals(readContent),
                "Content read after failover should be identical to that written before failover");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            status = afterFailover.Close(treeId, fileId);
            status = afterFailover.TreeDisconnect(treeId);
            status = afterFailover.LogOff();
            afterFailover.Disconnect();
            return true;
        }

        #endregion
    }
}
