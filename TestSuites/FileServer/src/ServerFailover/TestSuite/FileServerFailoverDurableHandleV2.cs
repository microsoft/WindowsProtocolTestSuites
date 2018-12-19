// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Protocols.TestSuites.FileSharing.ServerFailover.TestSuite
{
    public partial class FileServerFailoverExtendedTest : ServerFailoverTestBase
    {
        #region Test Case

        [TestMethod]
        [TestCategory(TestCategories.Failover)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Test the CREATE request from different client could succeed after failover when persistent handle is requested")]
        public void FileServerFailover_BlockCreateFromDifferentClient()
        {
            uncSharePath = Smb2Utility.GetUncPath(TestConfig.ClusteredFileServerName, TestConfig.ClusteredFileShare);

            #region From one client open a file and request persistent handle before failover
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "From one client open a file and request persistent handle before failover");

            FILEID fileIdBeforeFailover;
            uint treeIdBeforeFailover;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "BeforeFailover: Connect to general file server {0}.", TestConfig.ClusteredFileServerName);
            ConnectGeneralFileServerBeforeFailover(TestConfig.ClusteredFileServerName, out treeIdBeforeFailover);

            #region CREATE a durable open with flag DHANDLE_FLAG_PERSISTENT
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "BeforeFailover: CREATE a durable open with flag DHANDLE_FLAG_PERSISTENT.");
            Smb2CreateContextResponse[] serverCreateContexts;
            createGuid = Guid.NewGuid();
            status = clientBeforeFailover.Create(
                treeIdBeforeFailover,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdBeforeFailover,
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
            #endregion
            #endregion

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Disable owner node to simulate failover");
            FailoverServer(currentAccessIp, TestConfig.ClusteredFileServerName, FileServerType.GeneralFileServer);

            #region From a different client try to open the same file after failover
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "AfterFailover: From a different client try to open the same file");
            DoUntilSucceed(() => clientAfterFailover.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.ClusteredFileServerName, currentAccessIp), TestConfig.FailoverTimeout,
                "Retry to connect to server until succeed within timeout span");

            #region Connect to the same share of the same general file server
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "AfterFailover: Connect to the same share of the same general file server.");

            #region Negotiate
            Capabilities_Values clientCapabilitiesAfterFailover = Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES;
            status = clientAfterFailover.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                capabilityValue: clientCapabilitiesAfterFailover,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    TestConfig.CheckNegotiateDialect(DialectRevision.Smb21, response);
                });
            #endregion

            #region SESSION_SETUP
            status = clientAfterFailover.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.ClusteredFileServerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);
            #endregion

            #region TREE_CONNECT to share
            uint treeIdAfterFailover = 0;
            //Retry TreeConnect until succeed within timeout span
            status = DoUntilSucceed(
                () => clientAfterFailover.TreeConnect(uncSharePath, out treeIdAfterFailover, (header, response) => { }),
                TestConfig.FailoverTimeout,
                "Retry TreeConnect until succeed within timeout span");
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "TreeConnect to {0} should succeed, actual status is {1}", uncSharePath, Smb2Status.GetStatusCode(status));
            #endregion
            #endregion

            #region CREATE to open the same file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "AfterFailover: Send CREATE request using incompatible share access with previous CREATE and this should result violation");
            FILEID fileIdAfterFailover;
            status = clientAfterFailover.Create(
                treeIdAfterFailover,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdAfterFailover,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                null,
                // Incompatible share access with previous CREATE request and will result violation
                shareAccess: ShareAccess_Values.FILE_SHARE_WRITE | ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_DELETE,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreNotEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "From a different client try to open the same file after failover should not success");
                });
            #endregion
            #endregion
        }
        #endregion
    }
}
