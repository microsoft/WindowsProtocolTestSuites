// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Threading;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    [TestClass]
    public class SMB2Basic : SMB2TestBase
    {
        #region Variables
        private Smb2FunctionalClient client1;
        private Smb2FunctionalClient client2;
        private string uncSharePath;
        private CHANGE_NOTIFY_Response receivedChangeNotify;
        private Packet_Header receivedChangeNotifyHeader;
        private FILE_NOTIFY_INFORMATION[] receivedFileNotifyInfo;
        private ManualResetEvent changeNotificationReceived = new ManualResetEvent(false);
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
            uncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
        }

        protected override void TestCleanup()
        {
            if(client1 != null)
            {
                try
                {
                    client1.Disconnect();
                }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "Unexpected exception when disconnect client1: {0}", ex.ToString());
                }
            }

            if(client2 != null)
            {
                try
                {
                    client2.Disconnect();
                }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "Unexpected exception when disconnect client2: {0}", ex.ToString());
                }
            }
            
            base.TestCleanup();
        }
        #endregion

        #region Test cases

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.ChangeNotify)]
        [Description("This test case is designed to verify that CANCEL request cancels CHANGE_NOTIFY request when there's no CHANGE_NOTIFY response from server.")]
        public void BVT_SMB2Basic_CancelRegisteredChangeNotify()
        {            
            uint status;
            string testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Test directory \"{0}\" was created on share \"{1}\"", testDirectory, TestConfig.BasicFileShare);

            client1 = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client1.Smb2Client.ChangeNotifyResponseReceived += new Action<FILE_NOTIFY_INFORMATION[],Packet_Header,CHANGE_NOTIFY_Response>(OnChangeNotifyResponseReceived);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start a client to create a file by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE");
            client1.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            status = client1.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);
            status = client1.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);
            uint treeId1;
            status = client1.TreeConnect(uncSharePath, out treeId1);
            Smb2CreateContextResponse[] serverCreateContexts;
            FILEID fileId1;
            status = client1.Create(
                treeId1,
                testDirectory,
                CreateOptions_Values.FILE_DIRECTORY_FILE,
                out fileId1,
                out serverCreateContexts);

            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client starts to register CHANGE_NOTIFY on directory \"{0}\"", testDirectory);
            client1.ChangeNotify(treeId1, fileId1, CompletionFilter_Values.FILE_NOTIFY_CHANGE_LAST_ACCESS);

            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client starts to cancel the registered CHANGE_NOTIFY on directory \"{0}\"", testDirectory);
            client1.Cancel();

            BaseTestSite.Assert.IsTrue(
                changeNotificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                "Change notification should be received within {0} milliseconds", TestConfig.WaitTimeoutInMilliseconds);

            BaseTestSite.Assert.AreNotEqual(
                Smb2Status.STATUS_SUCCESS,
                receivedChangeNotifyHeader.Status, "CHANGE_NOTIFY is not expected to success after cancel, actually server returns {0}.", 
                Smb2Status.GetStatusCode(receivedChangeNotifyHeader.Status));
            BaseTestSite.CaptureRequirementIfAreEqual(
                Smb2Status.STATUS_CANCELLED,
                receivedChangeNotifyHeader.Status,
                RequirementCategory.STATUS_CANCELLED.Id,
                RequirementCategory.STATUS_CANCELLED.Description);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            client1.Close(treeId1, fileId1);
            client1.TreeDisconnect(treeId1);
            client1.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.QueryAndSetFileInfo)]
        [Description("This test case is designed to test whether server can handle QUERY and SET requests to a file correctly.")]
        public void BVT_SMB2Basic_QueryAndSet_FileInfo()
        {            
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Starts a client by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT.");
            client1 = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client1.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            client1.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);
            client1.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);
            uint treeId1;
            client1.TreeConnect(uncSharePath, out treeId1);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends CREATE request with desired access set to GENERIC_READ and GENERIC_WRITE to create a file.");
            Smb2CreateContextResponse[] serverCreateContexts;
            CREATE_Response? createResponse = null;
            string fileName = Guid.NewGuid().ToString() + ".txt";
            FILEID fileId1;
            client1.Create(
                treeId1,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId1,
                out serverCreateContexts,
                accessMask: AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE,
                checker: (Packet_Header header, CREATE_Response response) =>
                    {
                        BaseTestSite.Assert.AreEqual(
                            Smb2Status.STATUS_SUCCESS,
                            header.Status,
                            "CREATE should succeed, actually server returns {0}.", Smb2Status.GetStatusCode(header.Status));

                        BaseTestSite.Log.Add(LogEntryKind.TestStep,
                            "FileBasicInformation in CREATE response: \r\nCreationTime: {0}\r\nLastAccessTime:{1}\r\nLastWriteTime: {2}\r\nChangeTime: {3}\r\nFileAttributes: {4}",
                            Smb2Utility.ConvertToDateTimeUtc(response.CreationTime).ToString("MM/dd/yyy hh:mm:ss.ffffff"),
                            Smb2Utility.ConvertToDateTimeUtc(response.LastAccessTime).ToString("MM/dd/yyy hh:mm:ss.ffffff"),
                            Smb2Utility.ConvertToDateTimeUtc(response.LastWriteTime).ToString("MM/dd/yyy hh:mm:ss.ffffff"),
                            Smb2Utility.ConvertToDateTimeUtc(response.ChangeTime).ToString("MM/dd/yyy hh:mm:ss.ffffff"),
                            response.FileAttributes);
                        createResponse = response;
                    });

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends QUERY_INFO request to query file attributes.");
            byte[] outputBuffer;
            client1.QueryFileAttributes(
                treeId1,
                (byte)FileInformationClasses.FileBasicInformation,
                QUERY_INFO_Request_Flags_Values.SL_RESTART_SCAN,
                fileId1,
                new byte[0] { },
                out outputBuffer);

            FileBasicInformation fileBasicInfo = TypeMarshal.ToStruct<FileBasicInformation>(outputBuffer);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "FileBasicInformation in QUERY_INFO response: \r\nCreationTime: {0}\r\nLastAccessTime:{1}\r\nLastWriteTime: {2}\r\nChangeTime: {3}\r\nFileAttributes: {4}",
                Smb2Utility.ConvertToDateTimeUtc(fileBasicInfo.CreationTime).ToString("MM/dd/yyy hh:mm:ss.ffffff"),
                Smb2Utility.ConvertToDateTimeUtc(fileBasicInfo.LastAccessTime).ToString("MM/dd/yyy hh:mm:ss.ffffff"),
                Smb2Utility.ConvertToDateTimeUtc(fileBasicInfo.LastWriteTime).ToString("MM/dd/yyy hh:mm:ss.ffffff"),
                Smb2Utility.ConvertToDateTimeUtc(fileBasicInfo.ChangeTime).ToString("MM/dd/yyy hh:mm:ss.ffffff"),
                fileBasicInfo.FileAttributes);

            BaseTestSite.Assert.AreEqual(createResponse.Value.CreationTime, fileBasicInfo.CreationTime, "CreationTime received in QUERY_INFO response should be identical with that got in CREATE response");
            BaseTestSite.Assert.AreEqual(createResponse.Value.LastAccessTime, fileBasicInfo.LastAccessTime, "LastAccessTime received in QUERY_INFO response should be identical with that got in CREATE response");
            BaseTestSite.Assert.AreEqual(createResponse.Value.LastWriteTime, fileBasicInfo.LastWriteTime, "LastWriteTime received in QUERY_INFO response should be identical with that got in CREATE response");
            BaseTestSite.Assert.AreEqual(createResponse.Value.ChangeTime, fileBasicInfo.ChangeTime, "ChangeTime received in QUERY_INFO response should be identical with that got in CREATE response");
            BaseTestSite.Assert.AreEqual(createResponse.Value.FileAttributes, fileBasicInfo.FileAttributes, "FileAttributes received in QUERY_INFO response should be identical with that got in CREATE response");

            FileBasicInformation fileBasicInfoToSet = fileBasicInfo;
            DateTime dateTimeToSet = DateTime.UtcNow;
            fileBasicInfoToSet.LastAccessTime = Smb2Utility.ConvertToFileTime(dateTimeToSet);

            byte[] inputBuffer;
            inputBuffer = TypeMarshal.ToBytes<FileBasicInformation>(fileBasicInfoToSet);
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client sends SetFileAttributes request to set LastAccessTime for the file to {0}", dateTimeToSet.ToString("MM/dd/yyy hh:mm:ss.ffffff"));
            client1.SetFileAttributes(
                treeId1,
                (byte)FileInformationClasses.FileBasicInformation,
                fileId1,
                inputBuffer);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends QUERY request to query file attributes.");
            client1.QueryFileAttributes(
                treeId1,
                (byte)FileInformationClasses.FileBasicInformation,
                QUERY_INFO_Request_Flags_Values.SL_RESTART_SCAN,
                fileId1,
                new byte[0] { },
                out outputBuffer);
            fileBasicInfo = TypeMarshal.ToStruct<FileBasicInformation>(outputBuffer);
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "LastAccessTime in QUERY_INFO response after SET_INFO {0}",
                Smb2Utility.ConvertToDateTimeUtc(fileBasicInfo.LastAccessTime).ToString("MM/dd/yyy hh:mm:ss.ffffff"));

            BaseTestSite.Assert.AreNotEqual(
                createResponse.Value.LastAccessTime,
                fileBasicInfo.LastAccessTime,
                "LastAccessTime (dwHighDateTime:{0}, dwLowDateTime:{1}) after SET_INFO should not be equal to the value (dwHighDateTime:{2}, dwLowDateTime:{3}) before SET_INFO",
                fileBasicInfo.LastAccessTime.dwHighDateTime, fileBasicInfo.LastAccessTime.dwLowDateTime, createResponse.Value.LastAccessTime.dwHighDateTime, createResponse.Value.LastAccessTime.dwLowDateTime);

            BaseTestSite.Assert.AreEqual(
                fileBasicInfoToSet.LastAccessTime,
                fileBasicInfo.LastAccessTime,
                "LastAccessTime (dwHighDateTime:{0}, dwLowDateTime:{1}) queried after SET_INFO should be equal to the desired value (dwHighDateTime:{2}, dwLowDateTime:{3})",
                fileBasicInfo.LastAccessTime.dwHighDateTime, fileBasicInfo.LastAccessTime.dwLowDateTime, fileBasicInfoToSet.LastAccessTime.dwHighDateTime, fileBasicInfoToSet.LastAccessTime.dwLowDateTime);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            client1.Close(treeId1, fileId1);
            client1.TreeDisconnect(treeId1);
            client1.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.LockUnlock)]
        [Description("This test case is designed to test whether server can handle WRITE of locking content correctly.")]
        public void BVT_SMB2Basic_LockAndUnLock()
        {            
            uint status;
            string content = Smb2Utility.CreateRandomString(TestConfig.WriteBufferLengthInKb);

            #region From client1 lock a byte range and try to write content to the file within the range
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "From client1 locks a byte range and try to write content to the file within the range.");

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep, 
                "Start client1 to create a file with sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE.");
            client1 = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client1.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            status = client1.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);
            status = client1.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);
            uint treeId1;
            status = client1.TreeConnect(uncSharePath, out treeId1);
            Smb2CreateContextResponse[] serverCreateContexts;
            FILEID fileId1;
            string fileName = Guid.NewGuid().ToString() + ".txt";
            status = client1.Create(
                treeId1,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId1,
                out serverCreateContexts);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client1 writes content to the file created.");
            status = client1.Write(treeId1, fileId1, content);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down client1 by sending CLOSE request.");
            client1.Close(treeId1, fileId1);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client1 sends CREATE request.");
            status = client1.Create(
                treeId1,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId1,
                out serverCreateContexts);

            //Construct LOCK_ELEMENT
            LOCK_ELEMENT[] locks = new LOCK_ELEMENT[1];
            uint lockSequence = 0;
            locks[0].Offset = 0;
            locks[0].Length = (ulong)TestConfig.WriteBufferLengthInKb * 1024;
            locks[0].Flags = LOCK_ELEMENT_Flags_Values.LOCKFLAG_SHARED_LOCK;

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client1 starts to lock a byte range for file \"{0}\" with parameters offset:{1}, length:{2}, flags: {3})",
                fileName, locks[0].Offset, locks[0].Length, locks[0].Flags.ToString());
            status = client1.Lock(treeId1, lockSequence++, fileId1, locks);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client1 sends WRITE request to write content to the locking range");
            status = client1.Write(
                treeId1,
                fileId1,
                content,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreNotEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "All opens MUST NOT be allowed to write within the range when SMB2_LOCKFLAG_SHARED_LOCK set, actually server returns {0}.", Smb2Status.GetStatusCode(header.Status));
                    BaseTestSite.CaptureRequirementIfAreEqual(
                        Smb2Status.STATUS_FILE_LOCK_CONFLICT,
                        header.Status,
                        RequirementCategory.STATUS_FILE_LOCK_CONFLICT.Id,
                        RequirementCategory.STATUS_FILE_LOCK_CONFLICT.Description);
                });
            #endregion

            #region From client2 to read and write the locking range of the same file after lock
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "From client2 to read and take shared lock on the locking range of the same file after lock");

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client2 to create a file with sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE.");
            client2 = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client2.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            status = client2.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);
            status = client2.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);
            uint treeId2;
            status = client2.TreeConnect(uncSharePath, out treeId2);
            FILEID fileId2;
            status = client2.Create(
                treeId2,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId2,
                out serverCreateContexts);

            string data;
            Random random = new Random();
            uint offset = (uint)random.Next(0, TestConfig.WriteBufferLengthInKb * 1024 - 1);
            uint length = (uint)random.Next(0, (int)(TestConfig.WriteBufferLengthInKb * 1024 - offset));
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client2 sends READ request to read a random area in the locking range of file \"{0}\" with offset: {1}, length: {2}",
                fileName, offset, length);
            status = client2.Read(treeId2, fileId2, offset, length, out data);

            //Construct LOCK_ELEMENT
            LOCK_ELEMENT[] locksFromOtherOpen = new LOCK_ELEMENT[1];
            locksFromOtherOpen[0].Offset = offset;
            locksFromOtherOpen[0].Length = (ulong)length;
            locksFromOtherOpen[0].Flags = LOCK_ELEMENT_Flags_Values.LOCKFLAG_SHARED_LOCK;

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client2 attempts to take a shared lock on random range of file \"{0}\" with parameters offset:{1}, length:{2}, flags: {3})",
                fileName, locksFromOtherOpen[0].Offset, locksFromOtherOpen[0].Length, locksFromOtherOpen[0].Flags.ToString());
            status = client2.Lock(treeId2, lockSequence++, fileId2, locksFromOtherOpen);

            locksFromOtherOpen[0].Flags = LOCK_ELEMENT_Flags_Values.LOCKFLAG_UNLOCK;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client2 attempts to unlock the range");
            status = client2.Lock(treeId2, lockSequence++, fileId2, locksFromOtherOpen);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client2 sends WRITE request to write a random area in the locking range of file \"{0}\" after lock", fileName);
            status = client2.Write(
                treeId2,
                fileId2,
                content,
                offset,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreNotEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "All opens MUST NOT be allowed to write within the range when SMB2_LOCKFLAG_SHARED_LOCK set, actually server returns {0}.", Smb2Status.GetStatusCode(header.Status));
                    BaseTestSite.CaptureRequirementIfAreEqual(
                        Smb2Status.STATUS_FILE_LOCK_CONFLICT,
                        header.Status,
                        RequirementCategory.STATUS_FILE_LOCK_CONFLICT.Id,
                        RequirementCategory.STATUS_FILE_LOCK_CONFLICT.Description);
                });
            #endregion

            #region From client1 unlock the range
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client1 unlocks the range");
            locks[0].Flags = LOCK_ELEMENT_Flags_Values.LOCKFLAG_UNLOCK;
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Client1 attempts to unlock the range");
            status = client1.Lock(treeId1, lockSequence++, fileId1, locks);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down client1 by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            client1.Close(treeId1, fileId1);
            client1.TreeDisconnect(treeId1);
            client1.LogOff();
            #endregion

            #region From client2 write content to the previous locking range after unlock
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client2 sends WRITE request to write content to the previous locking range after unlock");
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Client2 attempts to write a random area in the locking range of file \"{0}\" after unlock", fileName);
            status = client2.Write(treeId2, fileId2, content, offset);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down client2 by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            client2.Close(treeId2, fileId2);
            client2.TreeDisconnect(treeId2);
            client2.LogOff();
            #endregion
        }

        // [MS-SMB2] Section 3.3.5.9   Receiving an SMB2 CREATE Request
        // The server MUST verify the request size. If the size of the SMB2 CREATE Request (excluding the SMB2 header) is less than specified in the StructureSize field, 
        // then the request MUST be failed with STATUS_INVALID_PARAMETER.
        [TestMethod]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.OutOfBoundary)]
        [TestCategory(TestCategories.CreateClose)]
        [Description("Test the server response when it receives an invalid structure size of CREATE request.")]
        public void InvalidCreateRequestStructureSize()
        {
            uint status;

            client1 = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, this.Site);
            client1.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);

            status = client1.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled);

            status = client1.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);

            uint treeId;
            status = client1.TreeConnect(uncSharePath, out treeId);
            string fileName = "BVT_SMB2Basic_InvalidCreateRequestStructureSize" + Guid.NewGuid();

            FILEID fileID;
            Smb2CreateContextResponse[] serverCreateContexts;

            // [MS-SMB2] Section 2.2.13 SMB2 CREATE Request
            // StructureSize (2 bytes):  The client MUST set this field to 57, indicating the size of the request structure, not including the header. 
            // The client MUST set it to this value regardless of how long Buffer[] actually is in the request being sent.

            // So set the StuctureSize to 58 here to make "the size of the SMB2 CREATE Request is less than specified in the StructureSize field".
            client1.BeforeSendingPacket(ReplacePacketByStructureSize);
            status = client1.Create(
                treeId,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileID,
                out serverCreateContexts,
                checker: (header, response) => { });

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_INVALID_PARAMETER,
                status,
                "The size of the SMB2 CREATE Request (excluding the SMB2 header) is less than specified in the StructureSize field, then the request MUST be failed with STATUS_ INVALID_PARAMETER");

            client1.TreeDisconnect(treeId);
            client1.LogOff();
        }

        #endregion

        private void OnChangeNotifyResponseReceived(FILE_NOTIFY_INFORMATION[] fileNotifyInfo, Packet_Header respHeader, CHANGE_NOTIFY_Response changeNotify)
        {
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Client1 receives the response of CHANGE_NOTIFY");
            receivedChangeNotify = changeNotify;
            receivedFileNotifyInfo = fileNotifyInfo;
            receivedChangeNotifyHeader = respHeader;
            changeNotificationReceived.Set();
        }

        private void ReplacePacketByStructureSize(Smb2Packet packet)
        {
            Smb2CreateRequestPacket request = packet as Smb2CreateRequestPacket;
            if (request == null)
                return;
            request.PayLoad.StructureSize += 1;
        }
    }
}
