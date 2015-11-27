// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{

    public partial class AppInstanceIdExtendedTest : SMB2TestBase
    {
        #region Test Cases
        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.AppInstanceVersion)]
        [Description("Check when client fails over to a new client, the previous opened file can be reopened with a greater AppInstanceVersion.")]
        public void BVT_AppInstanceVersion_SMB311_GreaterVersion()
        {
            //Client1 opens a file with AppInstanceID and AppInstanceVersion.            
            //Client2 tries to open that file with with the same AppInstanceID and a greater AppInstanceVersion and succeeds
            //Client1 tries to write to that file and fails
            //Client2 closes the open

            CheckApplicability();
            AppInstanceVersionTest(firstClientAppInstanceVersionHigh: 1,
                firstClientAppInstanceVersionLow: 0,
                firstClientDialect: DialectRevision.Smb311,
                secondClientAppInstanceVersionHigh: 2,
                secondClientAppInstanceVersionLow: 1,
                secondClientDialect: DialectRevision.Smb311,
                expectedReopenStatus: Smb2Status.STATUS_SUCCESS,
                expectedInitialOpenStatusAfterReopen: Smb2Status.STATUS_FILE_CLOSED);

        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.AppInstanceVersion)]
        [Description("Check when client fails over to a new client, the previous opened file can NOT be reopened with the same AppInstanceVersion.")]
        public void BVT_AppInstanceVersion_SMB311_SameVersion()
        {
            //Client1 opens a file with AppInstanceID and AppInstanceVersion with dialect 311.                
            //Client2 tries to open that file with with a valid AppInstanceID and higher AppInstanceVersion and succeds            
            //Client1 comes back to open that file with with a valid AppInstanceID and same AppInstanceVersion as Client2 but fails
            //Client2 closes the open and logs off

            CheckApplicability();
            string content = Smb2Utility.CreateRandomString(TestConfig.WriteBufferLengthInKb);
            uint treeIdForInitialOpen;
            FILEID fileIdForInitialOpen;
            Guid appInstanceId = new Guid();
            #region Client1 connects to Server
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client1 connects to the file server by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE");
            SendCreateRequestWithSpecificAppInstanceversion(
                clientForInitialOpen,
                appInstanceId,
                1,
                0,
                DialectRevision.Smb311,
                Smb2Status.STATUS_SUCCESS,
                out treeIdForInitialOpen,
                out fileIdForInitialOpen
                );

            #endregion

            #region Client2 connects to Server
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client2 connects to file server by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE.");
            uint treeIdForReOpen;
            FILEID fileIdForReopen;
            SendCreateRequestWithSpecificAppInstanceversion(
               clientForReOpen,
               appInstanceId,
               2,
               0,
               DialectRevision.Smb311,
               Smb2Status.STATUS_SUCCESS,
               out treeIdForReOpen,
               out fileIdForReopen
               );
            #endregion

            #region Client1 reconnects to Server and fails
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client1 sends another WRITE request.");
            clientForInitialOpen.Write(treeIdForInitialOpen, fileIdForInitialOpen, content,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_FILE_CLOSED,
                        header.Status,
                       "The initial open is closed. Write should not succeed. Actually server returns with {0}.",
                         Smb2Status.GetStatusCode(header.Status));
                });

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client1 sends another CREATE request with same AppInstanceVersion as Client2.");
            Smb2CreateContextResponse[] serverCreateContexts;            
            clientForInitialOpen.Create(
                treeIdForInitialOpen,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdForInitialOpen,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                new Smb2CreateContextRequest[] {                        
                    new Smb2CreateAppInstanceId
                    {
                            AppInstanceId = appInstanceId
                    },
                    new Smb2CreateAppInstanceVersion{
                        AppInstanceVersionHigh = 2,
                        AppInstanceVersionLow = 0
                    }
                },
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_FILE_FORCED_CLOSED,
                        header.Status,
                        "The open cannot be closed. Create should not succeed. Actually server returns with {0}.",
                        Smb2Status.GetStatusCode(header.Status));
                });

            Logoff(clientForInitialOpen);
            Logoff(clientForReOpen);

            #endregion
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.AppInstanceVersion)]
        [Description("Check when client fails over to a new client, the previous opened file can NOT be reopened with lower AppInstanceVersion.AppInstanceVersionHigh.")]
        public void BVT_AppInstanceVersion_SMB311_LowerAppInstanceVersionHigh()
        {
            //Client1 opens a file with AppInstanceID and AppInstanceVersion with dialect 311.                    
            //Client2 tries to open that file with with a valid AppInstanceID and lower AppInstanceVersion.AppInstanceVersionHigh and fails
            //Client1 writes to that file and succeeds            
            //Client1 closes the open and logs off

            CheckApplicability();
            AppInstanceVersionTest(
                firstClientAppInstanceVersionHigh: 2,
                firstClientAppInstanceVersionLow: 0,
                firstClientDialect: DialectRevision.Smb311,
                secondClientAppInstanceVersionHigh: 1,
                secondClientAppInstanceVersionLow: 0,
                secondClientDialect: DialectRevision.Smb311,
                expectedReopenStatus: Smb2Status.STATUS_FILE_FORCED_CLOSED,
                expectedInitialOpenStatusAfterReopen: Smb2Status.STATUS_SUCCESS
                );
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.AppInstanceVersion)]
        [Description("Check when client fails over to a new client, the previous opened file can NOT be reopened with lower AppInstanceVersion.AppInstanceVersionLow.")]
        public void BVT_AppInstanceVersion_SMB311_LowerAppInstanceVersionLow()
        {
            //Client1 opens a file with AppInstanceID and AppInstanceVersion with dialect 311.                  
            //Client2 tries to open that file with with a valid AppInstanceID and lower AppInstanceVersion.AppInstanceVersionLow and fails
            //Client1 writes to that file and succeeds            
            //Client1 closes the open and logs off

            CheckApplicability();
            AppInstanceVersionTest(
                firstClientAppInstanceVersionHigh: 1,
                firstClientAppInstanceVersionLow: 2,
                firstClientDialect: DialectRevision.Smb311,
                secondClientAppInstanceVersionHigh: 1,
                secondClientAppInstanceVersionLow: 1,
                secondClientDialect: DialectRevision.Smb311,
                expectedReopenStatus: Smb2Status.STATUS_FILE_FORCED_CLOSED,
                expectedInitialOpenStatusAfterReopen: Smb2Status.STATUS_SUCCESS
                );
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.AppInstanceVersion)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Check when client fails over to a new client, the previous opened file can NOT be reopened with no AppInstanceVersion.")]
        public void Negative_AppInstanceVersion_SMB311_NoVersion()
        {
            //Client1 opens a file with AppInstanceID and AppInstanceVersion with dialect 311.                     
            //Client2 tries to open that file with with a valid AppInstanceID and no AppInstanceVersion but failed            
            //Client1 writes to that file and succeeds
            //Client1 closes the open and logs off

            CheckApplicability();
            AppInstanceVersionTest(firstClientAppInstanceVersionHigh: 1,
                firstClientAppInstanceVersionLow: 0,
                firstClientDialect: DialectRevision.Smb311,
                secondClientAppInstanceVersionHigh: null,
                secondClientAppInstanceVersionLow: null,
                secondClientDialect: DialectRevision.Smb311,
                expectedReopenStatus: Smb2Status.STATUS_FILE_FORCED_CLOSED,
                expectedInitialOpenStatusAfterReopen: Smb2Status.STATUS_SUCCESS);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.AppInstanceVersion)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Check if server can fail the create request if the second client send a create request with dialect302 and AppInstanceID create context. ")]
        public void AppInstanceVersion_SMB311_SMB302()
        {
            //Client1 opens a file with AppInstanceId and AppInstanceVersion with dialect 311.                    
            //Client2 tries to open that file with with a valid AppInstanceID and dialect 302 but failed            
            //Client1 writes to that file and succeeds
            //Client1 closes the open and logs off

            CheckApplicability();
            AppInstanceVersionTest(firstClientAppInstanceVersionHigh: 1,
                firstClientAppInstanceVersionLow: 0,
                firstClientDialect: DialectRevision.Smb311,
                secondClientAppInstanceVersionHigh: null,
                secondClientAppInstanceVersionLow: null,
                secondClientDialect: DialectRevision.Smb302,
                expectedReopenStatus: Smb2Status.STATUS_FILE_FORCED_CLOSED,
                expectedInitialOpenStatusAfterReopen: Smb2Status.STATUS_SUCCESS);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.AppInstanceVersion)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Check when the first client negotiates dialect 3.02, the second client negotiates dialect 3.11," + 
            " the create request with SMB2_CREATE_APP_INSTANCE_VERSION create context sent by the second client should succeed.")]
        public void AppInstanceVersion_SMB302_SMB311()
        {
            //Client1 opens a file with AppInstanceId and dialect 302.
            //Client2 tries to opens that file with with dialect 311, a valid AppInstanceID and a valid AppInstanceVersion and succeeds
            //Client1 writes to that file and fails
            //Client2 closes the open and logs off

            CheckApplicability();
            AppInstanceVersionTest(firstClientAppInstanceVersionHigh: null,
                firstClientAppInstanceVersionLow: null,
                firstClientDialect: DialectRevision.Smb302,
                secondClientAppInstanceVersionHigh: 1,
                secondClientAppInstanceVersionLow: 0,
                secondClientDialect: DialectRevision.Smb311,
                expectedReopenStatus: Smb2Status.STATUS_SUCCESS,
                expectedInitialOpenStatusAfterReopen: Smb2Status.STATUS_FILE_CLOSED);
        }

        private void SendCreateRequestWithSpecificAppInstanceversion(
             Smb2FunctionalClient client,
             Guid appInstanceId,
             ulong? appInstanceVersionHigh,
             ulong? appInstanceVersionLow,
             DialectRevision dialect,
             uint expectedCreateResponseStatus,
             out uint treeId,
             out FILEID fileId
             )
        {
            #region Client connects to Server
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client connects to the file server by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT");
            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress, TestConfig.ClientNic1IPAddress);
            client.Negotiate(Smb2Utility.GetDialects(dialect), TestConfig.IsSMB1NegotiateEnabled);
            client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);

            client.TreeConnect(uncSharePath, out treeId);

            Smb2CreateContextResponse[] serverCreateContexts;
            Smb2CreateAppInstanceVersion appInstanceVersion = new Smb2CreateAppInstanceVersion();
            Smb2CreateContextRequest[] clientCreateContexts;

            if (appInstanceVersionHigh.HasValue && appInstanceVersionLow.HasValue)
            {
                appInstanceVersion.AppInstanceVersionHigh = appInstanceVersionHigh.Value;
                appInstanceVersion.AppInstanceVersionLow = appInstanceVersionLow.Value;
                clientCreateContexts =
                new Smb2CreateContextRequest[] {                        
                    new Smb2CreateAppInstanceId
                    {
                        AppInstanceId = appInstanceId
                    },
                    appInstanceVersion
                };
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends CREATE request with AppInstanceVersionHigh = {0}, AppInstanceVersionLow = {1}.", appInstanceVersion.AppInstanceVersionHigh, appInstanceVersion.AppInstanceVersionLow);
            }
            else
            {
                clientCreateContexts =
                new Smb2CreateContextRequest[] {                        
                    new Smb2CreateAppInstanceId
                    {
                        AppInstanceId = appInstanceId
                    }
                };
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends CREATE request without AppInstanceVersion.");
            }

            client.Create(
                treeId,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                clientCreateContexts,
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        expectedCreateResponseStatus,
                        header.Status,
                        (expectedCreateResponseStatus == Smb2Status.STATUS_SUCCESS ?
                        "The open will be closed. Create should succeed. Actually server returns with {0}."
                        : "The open cannot be closed. Create should not succeed. Actually server returns with {0}."),
                        Smb2Status.GetStatusCode(header.Status));
                });

            #endregion
        }

        private void AppInstanceVersionTest(
             ulong? firstClientAppInstanceVersionHigh,
             ulong? firstClientAppInstanceVersionLow,
             DialectRevision firstClientDialect,
             ulong? secondClientAppInstanceVersionHigh,
             ulong? secondClientAppInstanceVersionLow,
             DialectRevision secondClientDialect,
             uint expectedReopenStatus,
             uint expectedInitialOpenStatusAfterReopen,
             bool logoff = true
            )
        {
            string content = Smb2Utility.CreateRandomString(TestConfig.WriteBufferLengthInKb);

            #region Client1 connects to Server
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client1 connects to the file server by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT");
            uint treeIdForInitialOpen;
            FILEID fileIdForInitialOpen;
            Guid appInstanceId = Guid.NewGuid();
            SendCreateRequestWithSpecificAppInstanceversion(
                clientForInitialOpen,
                appInstanceId,
                firstClientAppInstanceVersionHigh,
                firstClientAppInstanceVersionLow,
                firstClientDialect,
                Smb2Status.STATUS_SUCCESS,
                out treeIdForInitialOpen,
                out fileIdForInitialOpen
                );

            #endregion

            #region Client2 connects to Server
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client2 connects to file server by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT with dialect {0}.", secondClientDialect);
            uint treeIdForReOpen;
            FILEID fileIdForReOpen;
            SendCreateRequestWithSpecificAppInstanceversion(
                clientForReOpen,
                appInstanceId,
                secondClientAppInstanceVersionHigh,
                secondClientAppInstanceVersionLow,
                secondClientDialect,
                expectedReopenStatus,
                out treeIdForReOpen,
                out fileIdForReOpen
                );
            #endregion

            #region Client1 reconnects to Server
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client1 sends another WRITE request.");
            clientForInitialOpen.Write(
                treeIdForInitialOpen,
                fileIdForInitialOpen,
                content,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        expectedInitialOpenStatusAfterReopen,
                        header.Status,
                        (expectedInitialOpenStatusAfterReopen == Smb2Status.STATUS_SUCCESS ?
                        "The initial open is not closed. Write should succeed. Actually server returns with {0}."
                        : "The initial open is closed. Write should not succeed. Actually server returns with {0}."),
                         Smb2Status.GetStatusCode(header.Status));
                });
            #endregion

            #region Disconnect Client1 or Client2 to Server according to the expectedReopenStatus and expectedInitialOpenStatusAfterReopen
            if (expectedReopenStatus == Smb2Status.STATUS_SUCCESS)
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Tear down the client2 by sending CLOSE requests.");
                clientForReOpen.Close(treeIdForReOpen, fileIdForReOpen);
            }

            BaseTestSite.Log.Add(LogEntryKind.Debug, "Tear down the client2 by sending TREE_DISCONNECT request.");
            clientForReOpen.TreeDisconnect(treeIdForReOpen);

            if (logoff)
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Tear down the client1 by sending the following requests: LOG_OFF; DISCONNECT");
                Logoff(clientForReOpen);
            }

            if (expectedInitialOpenStatusAfterReopen == Smb2Status.STATUS_SUCCESS)
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Tear down the client2 by sending CLOSE requests.");
                clientForInitialOpen.Close(treeIdForInitialOpen, fileIdForInitialOpen);
            }

            BaseTestSite.Log.Add(LogEntryKind.Debug, "Tear down the client2 by sending TREE_DISCONNECT request.");
            clientForInitialOpen.TreeDisconnect(treeIdForInitialOpen);

            if (logoff)
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Tear down the client2 by sending the following requests: LOG_OFF; DISCONNECT");
                Logoff(clientForInitialOpen);
            }
            #endregion
        }

        private void Logoff(Smb2FunctionalClient client)
        {
            client.LogOff();
            client.Disconnect();
        }

        private void CheckApplicability()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb311);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_APP_INSTANCE_ID, CreateContextTypeValue.SMB2_CREATE_APP_INSTANCE_VERSION);
            #endregion
        }
        #endregion
    }
}
