// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite.ResilientHandle
{
    [TestClass]
    public class ResilientHandle : SMB2TestBase
    {
        #region Variables
        private string fileName;
        private string sharePath;
        private Smb2FunctionalClient clientBeforeDisconnection;
        private Smb2FunctionalClient clientAfterDisconnection;

        public const uint NETWORK_RESILIENCY_REQUEST_SIZE = 8;
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
            sharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare);
            fileName = GetTestFileName(sharePath);

            clientBeforeDisconnection = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            clientAfterDisconnection = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            clientBeforeDisconnection.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            clientAfterDisconnection.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
        }

        protected override void TestCleanup()
        {
            if (clientBeforeDisconnection != null)
            {
                clientBeforeDisconnection.Disconnect();
            }
            if (clientAfterDisconnection != null)
            {
                clientAfterDisconnection.Disconnect();
            }

            base.TestCleanup();
        }
        #endregion

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.FsctlLmrRequestResiliency)]
        [Description("Test whether server can request a durable open when receiving FSCTL_LMR_REQUEST_RESILLIENNCY successfully.")]
        public void BVT_ResilientHandle_Reconnect()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb21);
            TestConfig.CheckIOCTL(CtlCode_Values.FSCTL_LMR_REQUEST_RESILIENCY);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT);
            #endregion          

            Guid clientGuid = Guid.NewGuid();

            #region clientBeforeDisconnection Create a File
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start the first client to create a file by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT");
            clientBeforeDisconnection.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled, clientGuid: clientGuid);
            clientBeforeDisconnection.SessionSetup(TestConfig.DefaultSecurityPackage, TestConfig.SutComputerName, TestConfig.AccountCredential, false);
            uint treeId;
            clientBeforeDisconnection.TreeConnect(sharePath, out treeId);
            FILEID fileId;
            Smb2CreateContextResponse[] createContextResponse;
            clientBeforeDisconnection.Create(treeId, fileName, CreateOptions_Values.FILE_NON_DIRECTORY_FILE, out fileId, out createContextResponse);
            #endregion

            #region Request Resilient Handle
            IOCTL_Response IOCTLResponse;
            byte[] inputInResponse;
            byte[] outputInResponse;
            Packet_Header packetHeader;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The first client sends an IOCTL FSCTL_LMR_REQUEST_RESILLIENCY request.");
            clientBeforeDisconnection.ResiliencyRequest(treeId, fileId, TestConfig.MaxResiliencyTimeoutInSecond * 1000,
                NETWORK_RESILIENCY_REQUEST_SIZE, out packetHeader, out IOCTLResponse, out inputInResponse, out outputInResponse);
            #endregion

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the first client by sending DISCONNECT request.");
            clientBeforeDisconnection.Disconnect();

            #region ClientAfterDisconnection Opens the Previously Created File with DurableHandleReconnect
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start a second client by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT");
            clientAfterDisconnection.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled, clientGuid: clientGuid);
            clientAfterDisconnection.ReconnectSessionSetup(clientBeforeDisconnection,
                TestConfig.DefaultSecurityPackage, TestConfig.SutComputerName, TestConfig.AccountCredential, false);
            clientAfterDisconnection.TreeConnect(sharePath, out treeId);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The second client sends CREATE request with SMB2_CREATE_DURABLE_HANDLE_RECONNECT create context to open the same file created by the first client.");
            clientAfterDisconnection.Create(treeId, fileName, CreateOptions_Values.FILE_NON_DIRECTORY_FILE, out fileId, out createContextResponse, RequestedOplockLevel_Values.OPLOCK_LEVEL_BATCH,
                new Smb2CreateContextRequest[]
                {
                    new Smb2CreateDurableHandleReconnect
                    {
                        Data = fileId
                    }
                });
            #endregion

            #region Tear Down Client
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the second client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            clientAfterDisconnection.Close(treeId, fileId);
            clientAfterDisconnection.TreeDisconnect(treeId);
            clientAfterDisconnection.LogOff();
            #endregion
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.FsctlLmrRequestResiliency)]
        [Description("This test case is designed to test whether server can handle Lock request with specified LockSequence.")]
        public void BVT_ResilientHandle_LockSequence()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb21);
            TestConfig.CheckIOCTL(CtlCode_Values.FSCTL_LMR_REQUEST_RESILIENCY);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT);
            #endregion

            Guid clientGuid = Guid.NewGuid();

            #region clientBeforeDisconnection Create a File
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start the first client to create a file by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE.");
            clientBeforeDisconnection.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled, clientGuid: clientGuid);
            clientBeforeDisconnection.SessionSetup(TestConfig.DefaultSecurityPackage, TestConfig.SutComputerName, TestConfig.AccountCredential, false);
            uint treeId;
            clientBeforeDisconnection.TreeConnect(sharePath, out treeId);
            FILEID fileId;
            Smb2CreateContextResponse[] createContextResponse;
            clientBeforeDisconnection.Create(treeId, fileName, CreateOptions_Values.FILE_NON_DIRECTORY_FILE, out fileId, out createContextResponse);
            #endregion

            #region Request Resilient Handle
            IOCTL_Response IOCTLResponse;
            byte[] inputInResponse;
            byte[] outputInResponse;
            Packet_Header packetHeader;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "The first client sends an IOCTL FSCTL_LMR_REQUEST_RESILLIENCY request.");
            clientBeforeDisconnection.ResiliencyRequest(treeId, fileId, TestConfig.MaxResiliencyTimeoutInSecond * 1000,
                NETWORK_RESILIENCY_REQUEST_SIZE, out packetHeader, out IOCTLResponse, out inputInResponse, out outputInResponse);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The first client sends WRITE request.");
            clientBeforeDisconnection.Write(treeId, fileId, "12345678");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The first client sends Flush request.");
            clientBeforeDisconnection.Flush(treeId, fileId);
            #endregion

            //The LockSequence field of the SMB2 lock request MUST be set to (BucketNumber<< 4) + BucketSequence.
            int bucketNum = 1;
            int bucketSeq = 1;
            uint lockSequence = (uint)bucketNum << 4 + bucketSeq;

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "The first client sends LOCK request with LockSequence set to (BucketNumber<< 4) + BucketSequence");
            clientBeforeDisconnection.Lock(treeId,
                lockSequence,
                fileId,
                new LOCK_ELEMENT[]
                {
                    new LOCK_ELEMENT 
                    { 
                        Flags = LOCK_ELEMENT_Flags_Values.LOCKFLAG_EXCLUSIVE_LOCK | LOCK_ELEMENT_Flags_Values.LOCKFLAG_FAIL_IMMEDIATELY,
                        Offset = 0,
                        Length = 4
                    }
                });

            clientBeforeDisconnection.Disconnect();

            #region clientAfterDisconnection Opens the Previously Created File
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start the second client to reconnect to the file created by the first client by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE (with SMB2_CREATE_DURABLE_HANDLE_RECONNECT Create Context).");
            clientAfterDisconnection.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled, clientGuid: clientGuid);
            clientAfterDisconnection.ReconnectSessionSetup(clientBeforeDisconnection,
                TestConfig.DefaultSecurityPackage, TestConfig.SutComputerName, TestConfig.AccountCredential, false);
            clientAfterDisconnection.TreeConnect(sharePath, out treeId);
            clientAfterDisconnection.Create(treeId, fileName, CreateOptions_Values.FILE_NON_DIRECTORY_FILE, out fileId, out createContextResponse, RequestedOplockLevel_Values.OPLOCK_LEVEL_BATCH,
                new Smb2CreateContextRequest[]
                {
                    new Smb2CreateDurableHandleReconnect
                    {
                        Data = fileId
                    }
                });
            #endregion

            //If the sequence numbers are equal, the server MUST complete the lock/unlock request with success.
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The second client sends LOCK request with the same LockSequence with the first client.");
            clientAfterDisconnection.Lock(treeId,
                lockSequence, //Using same Lock Sequence
                fileId,
                new LOCK_ELEMENT[]
                {
                    new LOCK_ELEMENT 
                    { 
                        Flags = LOCK_ELEMENT_Flags_Values.LOCKFLAG_EXCLUSIVE_LOCK | LOCK_ELEMENT_Flags_Values.LOCKFLAG_FAIL_IMMEDIATELY,
                        Offset = 0,
                        Length = 4
                    }
                });

            clientAfterDisconnection.Lock(treeId,
            lockSequence + 1, //Using different Lock Sequence
            fileId,
            new LOCK_ELEMENT[]
            {
                new LOCK_ELEMENT 
                { 
                    Flags = LOCK_ELEMENT_Flags_Values.LOCKFLAG_EXCLUSIVE_LOCK | LOCK_ELEMENT_Flags_Values.LOCKFLAG_FAIL_IMMEDIATELY,
                    Offset = 0,
                    Length = 4
                }
            },
            (header, response) =>
            {
                BaseTestSite.Assert.AreEqual(
                    Smb2Status.STATUS_LOCK_NOT_GRANTED,
                    header.Status,
                    "If the range being locked is already locked by another open in a way that does not allow this open to take a lock on the range, " +
                    "and if SMB2_LOCKFLAG_FAIL_IMMEDIATELY is set, the server MUST fail the request with STATUS_LOCK_NOT_GRANTED. " +
                    "Actually server returns {0}.",
                    Smb2Status.GetStatusCode(header.Status));
            });

            #region Tear Down Client
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the second client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            clientAfterDisconnection.Close(treeId, fileId);
            clientAfterDisconnection.TreeDisconnect(treeId);
            clientAfterDisconnection.LogOff();
            #endregion
        }
    }
}
 
