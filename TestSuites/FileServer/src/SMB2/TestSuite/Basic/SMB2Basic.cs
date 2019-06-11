// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2.Adapter;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
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
        /// <summary>
        /// Specify the type of FileBasicInformation
        /// </summary>
        public enum FileBasicInfoType
        {
            /// <summary>
            /// File Attributes of file in FileBasicInformation
            /// </summary>
            FileBasicInfoAttribute,

            /// <summary>
            /// Last Access Time of file in FileBasicInformation
            /// </summary>
            FileBasicInfoLastAccessTime,

            /// <summary>
            /// Last Write Time of file in FileBasicInformation
            /// </summary>
            FileBasicInfoLastWriteTime,

            /// <summary>
            /// Creation Time of file in FileBasicInformation
            /// </summary>
            FileBasicInfoCreationTime
        }

        /// <summary>
        /// According to MS-SMB2 2.2.37.1
        /// Specify the type of element in SidBuffer
        /// </summary>
        private enum SidBufferFormat
        {
            /// <summary>
            /// Format 1 of SidBuffer
            /// SidBuffer contains a list of FILE_GET_QUOTA_INFORMATION
            /// </summary>
            FILE_GET_QUOTA_INFORMATION,

            /// <summary>
            /// Format 2 of SidBuffer
            /// SidBuffer contains a SID.
            /// Please be noted that Windows-based clients never send a request using the SidBuffer format 2.
            /// </summary>
            SID
        }

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
            string fileName = GetTestFileName(uncSharePath);
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
        [TestCategory(TestCategories.QueryInfo)]
        [Description("This test case is designed to test whether server can handle QUERY requests to a file for FileAllInformation correctly.")]
        public void BVT_SMB2Basic_Query_FileAllInformation()
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
            string fileName = GetTestFileName(uncSharePath);
            FILEID fileId1;
            client1.Create(
                treeId1,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId1,
                out serverCreateContexts,
                accessMask: AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends QUERY_INFO request to query file attributes.");
            byte[] outputBuffer;
            uint status = client1.QueryFileAttributes(
                treeId1,
                (byte)FileInformationClasses.FileAllInformation,
                QUERY_INFO_Request_Flags_Values.SL_RESTART_SCAN,
                fileId1,
                new byte[0] { },
                out outputBuffer);

            ushort filenameInfoOffset = 40 + // BasicInformation (40 bytes)
                                        24 + // StandardInformation (24 bytes)
                                        8 +  // InternalInformation (8 bytes)
                                        4 +  // EaInformation (4 bytes)
                                        4 +  // AccessInformation (4 bytes)
                                        8 +  // PositionInformation (8 bytes)
                                        4 +  // ModeInformation (4 bytes)
                                        4;   // AlignmentInformation (4 bytes)
            byte[] filenameInfoBuffer = new byte[outputBuffer.Length - filenameInfoOffset];
            Array.Copy(outputBuffer, filenameInfoOffset, filenameInfoBuffer, 0, outputBuffer.Length - filenameInfoOffset);
            FileNameInformation filenameInfo = TypeMarshal.ToStruct<FileNameInformation>(filenameInfoBuffer);

            /*
             * If the information class is FileAllInformation,
             * the server SHOULD return an empty FileNameInformation
             * by setting FileNameLength field to zero and FileName field
             * to an empty string.
             */
            if ((testConfig.Platform >= Platform.WindowsServer2016) ||
                (testConfig.Platform == Platform.NonWindows))
            {
                BaseTestSite.Assert.IsTrue(
                    0 == filenameInfo.FileNameLength,
                    "FileNameLength in FileNameInformation should be set to 0, actually server returns {0}",
                    filenameInfo.FileNameLength);
                BaseTestSite.Assert.IsTrue(
                    0 == filenameInfo.FileName.Length,
                    "FileName in FileNameInformation should be set to an empty string, actually server returns {0}.",
                    Encoding.Unicode.GetString(filenameInfo.FileName));
            }
            /*
             * <357>
             * If the information class is FileAllInformation, Windows Vista SP1,
             * Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8,
             * Windows Server 2012, Windows 8.1, and Windows Server 2012 R2
             * return an absolute path to the file name as part of FileNameInformation.
             */
            else
            {
                string absoluteFilePath = "\\" + TestConfig.BasicFileShare + "\\" + fileName;
                string filenameInFilenameInfo = Encoding.Unicode.GetString(filenameInfo.FileName);
                BaseTestSite.Assert.IsTrue(
                    absoluteFilePath.Length * 2 == filenameInfo.FileNameLength, // FileNameLength is the length of unicode characters
                    "FileNameLength should be set as the length of the absolute path to the file, actually server returns {0}.",
                    filenameInfo.FileNameLength);
                BaseTestSite.Assert.AreEqual(
                    absoluteFilePath, filenameInFilenameInfo,
                    "FileName should be set as the absolute path to the file, actually server returns {0}.",
                    filenameInFilenameInfo);
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            client1.Close(treeId1, fileId1);
            client1.TreeDisconnect(treeId1);
            client1.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.QueryInfo)]
        [Description("This test case is designed to verify the behavior of querying quota information with FILE_GET_QUOTA_INFO in SidBuffer.")]
        public void BVT_SMB2Basic_Query_Quota_Info()
        {
            QueryQuotaInfo(SidBufferFormat.FILE_GET_QUOTA_INFORMATION);
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
            string fileName = GetTestFileName(uncSharePath);
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
            string fileName = GetTestFileName(uncSharePath);

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

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.QueryDir)]
        [Description("This test case is designed to verify QUERY_DIRECTORY with flag SMB2_REOPEN to a directory is handled correctly.")]
        public void BVT_SMB2Basic_QueryDir_Reopen_OnDir()
        {
            QueryDir_Reopen(FileType.DirectoryFile);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.QueryDir)]
        [Description("This test case is designed to verify QUERY_DIRECTORY with flag SMB2_REOPEN to a file is handled correctly.")]
        public void BVT_SMB2Basic_QueryDir_Reopen_OnFile()
        {
            QueryDir_Reopen(FileType.DataFile);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.ChangeNotify)]
        [Description("This test case is designed to verify CHANGE_NOTIFY for CompletionFilter FILE_NOTIFY_CHANGE_FILE_NAME is handled correctly.")]
        public void BVT_SMB2Basic_ChangeNotify_ChangeFileName()
        {
            string testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Test directory \"{0}\" was created on share \"{1}\"", testDirectory, TestConfig.BasicFileShare);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client1 to create a directory \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", testDirectory);
            uint treeIdClient1;
            FILEID fileIdDir;
            SmbClientConnectAndOpenFile(out client1, testDirectory, out treeIdClient1, out fileIdDir);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client1 starts to register CHANGE_NOTIFY on directory \"{0}\" with CompletionFilter FILE_NOTIFY_CHANGE_FILE_NAME and flag WATCH_TREE", testDirectory);
            client1.ChangeNotify(
                treeIdClient1,
                fileIdDir,
                CompletionFilter_Values.FILE_NOTIFY_CHANGE_FILE_NAME,
                flags: CHANGE_NOTIFY_Request_Flags_Values.WATCH_TREE);

            string fileName = Guid.NewGuid().ToString();
            string filePath = testDirectory + "\\" + fileName;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client1 starts to create a file \"{0}\" under directory \"{1}\" by sending CREATE request", fileName, testDirectory);
            SmbClientCreateNewFile(client1, treeIdClient1, filePath);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client2 to open a file \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", filePath);
            uint treeIdClient2;
            FILEID fileIdFileToBeRenamed;
            SmbClientConnectAndOpenFile(
                out client2,
                filePath,
                out treeIdClient2,
                out fileIdFileToBeRenamed,
                createOption: CreateOptions_Values.FILE_NON_DIRECTORY_FILE |
                              CreateOptions_Values.FILE_DELETE_ON_CLOSE);

            // MS-FSCC 2.4.34 FileRenameInformation
            // Create a buffer for FileRenameInformation
            // This information class is used to rename a file.
            string newName = "Renamed_" + fileName;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client2 renames the file from \"{0}\" to \"{1}\" by sending SET_INFO request", fileName, newName);
            client2.SetFileAttributes(
                treeIdClient2,
                (byte)FileInformationClasses.FileRenameInformation,
                fileIdFileToBeRenamed,
                CreateFileRenameInfo(newName),
                (header, response) => { });

            BaseTestSite.Assert.IsTrue(
                changeNotificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                "CHANGE_NOTIFY should be received within {0} milliseconds", TestConfig.WaitTimeoutInMilliseconds);

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                receivedChangeNotifyHeader.Status,
                "CHANGE_NOTIFY is expected to success, actually server returns {0}.",
                Smb2Status.GetStatusCode(receivedChangeNotifyHeader.Status));

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down client2 by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client2, treeIdClient2, fileIdFileToBeRenamed);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down client1 by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client1, treeIdClient1, fileIdDir);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.ChangeNotify)]
        [Description("This test case is designed to verify CHANGE_NOTIFY for CompletionFilter FILE_NOTIFY_CHANGE_DIR_NAME is handled correctly.")]
        public void BVT_SMB2Basic_ChangeNotify_ChangeDirName()
        {
            string testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Test directory \"{0}\" was created on share \"{1}\"", testDirectory, TestConfig.BasicFileShare);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client1 to create a directory \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", testDirectory);
            uint treeIdClient1;
            FILEID fileIdDir;
            SmbClientConnectAndOpenFile(out client1, testDirectory, out treeIdClient1, out fileIdDir);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client1 starts to register CHANGE_NOTIFY on directory \"{0}\" with CompletionFilter FILE_NOTIFY_CHANGE_DIR_NAME and flag WATCH_TREE", testDirectory);
            client1.ChangeNotify(
                treeIdClient1,
                fileIdDir,
                CompletionFilter_Values.FILE_NOTIFY_CHANGE_DIR_NAME,
                flags: CHANGE_NOTIFY_Request_Flags_Values.WATCH_TREE);

            string dirName = Guid.NewGuid().ToString();
            string dirPath = testDirectory + "\\" + dirName;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client1 starts to create a directory \"{0}\" under directory \"{1}\" by sending CREATE request", dirName, testDirectory);
            SmbClientCreateNewFile(client1, treeIdClient1, dirPath, CreateOptions_Values.FILE_DIRECTORY_FILE);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client2 to open a directory \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", testDirectory);
            uint treeIdClient2;
            FILEID fileIdDirToBeRenamed;
            SmbClientConnectAndOpenFile(
                out client2,
                dirPath,
                out treeIdClient2,
                out fileIdDirToBeRenamed,
                createOption: CreateOptions_Values.FILE_DIRECTORY_FILE |
                              CreateOptions_Values.FILE_DELETE_ON_CLOSE);

            // MS-FSCC 2.4.34 FileRenameInformation
            // Create a buffer for FileRenameInformation
            // This information class is used to rename a file.
            string newName = "Renamed_" + dirName;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client2 renames the directory from \"{0}\" to \"{1}\" by sending SET_INFO request", dirName, newName);
            client2.SetFileAttributes(
                treeIdClient2,
                (byte)FileInformationClasses.FileRenameInformation,
                fileIdDirToBeRenamed,
                CreateFileRenameInfo(newName),
                (header, response) => { });

            BaseTestSite.Assert.IsTrue(
                changeNotificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                "CHANGE_NOTIFY should be received within {0} milliseconds", TestConfig.WaitTimeoutInMilliseconds);

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                receivedChangeNotifyHeader.Status,
                "CHANGE_NOTIFY is expected to success, actually server returns {0}.",
                Smb2Status.GetStatusCode(receivedChangeNotifyHeader.Status));

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down client2 by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client2, treeIdClient2, fileIdDirToBeRenamed);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down client1 by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client1, treeIdClient1, fileIdDir);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.ChangeNotify)]
        [Description("This test case is designed to verify CHANGE_NOTIFY for CompletionFilter FILE_NOTIFY_CHANGE_ATTRIBUTES is handled correctly.")]
        public void BVT_SMB2Basic_ChangeNotify_ChangeAttributes()
        {
            string testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Test directory \"{0}\" was created on share \"{1}\"", testDirectory, TestConfig.BasicFileShare);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client1 to create a directory \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", testDirectory);
            uint treeIdClient1;
            FILEID fileIdDir;
            SmbClientConnectAndOpenFile(out client1, testDirectory, out treeIdClient1, out fileIdDir);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client1 starts to register CHANGE_NOTIFY on directory \"{0}\" with CompletionFilter FILE_NOTIFY_CHANGE_ATTRIBUTES and flag WATCH_TREE", testDirectory);
            client1.ChangeNotify(
                treeIdClient1,
                fileIdDir,
                CompletionFilter_Values.FILE_NOTIFY_CHANGE_ATTRIBUTES,
                flags: CHANGE_NOTIFY_Request_Flags_Values.WATCH_TREE);

            string fileName = Guid.NewGuid().ToString();
            string filePath = testDirectory + "\\" + fileName;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client1 starts to create a file \"{0}\" under directory \"{1}\" by sending CREATE request", fileName, testDirectory);
            SmbClientCreateNewFile(client1, treeIdClient1, filePath);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client2 to open a file \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", filePath);
            uint treeIdClient2;
            FILEID fileIdFileToBeModified;
            SmbClientConnectAndOpenFile(
                out client2,
                filePath,
                out treeIdClient2,
                out fileIdFileToBeModified,
                createOption: CreateOptions_Values.FILE_NON_DIRECTORY_FILE |
                              CreateOptions_Values.FILE_DELETE_ON_CLOSE);

            // MS-FSCC 2.4.7 FileBasicInformation
            // Create a buffer for FileBasicInformation for setting file attributes.
            DateTime dateTimeToSet = DateTime.UtcNow;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client2 sets file attribute for the file \"{0}\" to FILE_ATTRIBUTE_HIDDEN by sending SET_INFO request", filePath);
            client2.SetFileAttributes(
                treeIdClient2,
                (byte)FileInformationClasses.FileBasicInformation,
                fileIdFileToBeModified,
                CreateFileBasicInfo(client2, treeIdClient2, fileIdFileToBeModified, FileBasicInfoType.FileBasicInfoAttribute, dateTimeToSet));

            BaseTestSite.Assert.IsTrue(
                changeNotificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                "CHANGE_NOTIFY should be received within {0} milliseconds", TestConfig.WaitTimeoutInMilliseconds);

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                receivedChangeNotifyHeader.Status,
                "CHANGE_NOTIFY is expected to success, actually server returns {0}.",
                Smb2Status.GetStatusCode(receivedChangeNotifyHeader.Status));

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down client2 by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client2, treeIdClient2, fileIdFileToBeModified);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down client1 by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client1, treeIdClient1, fileIdDir);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.ChangeNotify)]
        [Description("This test case is designed to verify CHANGE_NOTIFY for CompletionFilter FILE_NOTIFY_CHANGE_SIZE is handled correctly.")]
        public void BVT_SMB2Basic_ChangeNotify_ChangeSize()
        {
            string testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Test directory \"{0}\" was created on share \"{1}\"", testDirectory, TestConfig.BasicFileShare);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client1 to create a directory \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", testDirectory);
            uint treeIdClient1;
            FILEID fileIdDir;
            SmbClientConnectAndOpenFile(out client1, testDirectory, out treeIdClient1, out fileIdDir);

            string fileName = Guid.NewGuid().ToString();
            string filePath = testDirectory + "\\" + fileName;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client1 starts to create a file \"{0}\" by sending CREATE request and write 1-kbyte content to file by sending WRITE request", filePath);
            SmbClientCreateNewFileAndWrite(client1, treeIdClient1, filePath, 1);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client1 starts to register CHANGE_NOTIFY on directory \"{0}\" with CompletionFilter FILE_NOTIFY_CHANGE_SIZE and flag WATCH_TREE", testDirectory);
            client1.ChangeNotify(
                treeIdClient1,
                fileIdDir,
                CompletionFilter_Values.FILE_NOTIFY_CHANGE_SIZE,
                flags: CHANGE_NOTIFY_Request_Flags_Values.WATCH_TREE);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client2 to open a file \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", filePath);
            uint treeIdClient2;
            FILEID fileIdFileToBeModified;
            SmbClientConnectAndOpenFile(
                out client2,
                filePath,
                out treeIdClient2,
                out fileIdFileToBeModified,
                createOption: CreateOptions_Values.FILE_NON_DIRECTORY_FILE |
                              CreateOptions_Values.FILE_DELETE_ON_CLOSE);

            Random random = new Random();
            long newEofPos = random.Next(0, 1023);
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client2 sets new EOF Information (newEofPos={0}) for the file \"{1}\" by sending SET_INFO request", newEofPos, filePath);
            client2.SetFileAttributes(
                treeIdClient2,
                (byte)FileInformationClasses.FileEndOfFileInformation,
                fileIdFileToBeModified,
                CreateFileEndOfFileInfo(newEofPos));

            BaseTestSite.Assert.IsTrue(
                changeNotificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                "CHANGE_NOTIFY should be received within {0} milliseconds", TestConfig.WaitTimeoutInMilliseconds);

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                receivedChangeNotifyHeader.Status,
                "CHANGE_NOTIFY is expected to success, actually server returns {0}.",
                Smb2Status.GetStatusCode(receivedChangeNotifyHeader.Status));

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down client2 by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client2, treeIdClient2, fileIdFileToBeModified);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down client1 by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client1, treeIdClient1, fileIdDir);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.ChangeNotify)]
        [Description("This test case is designed to verify CHANGE_NOTIFY for CompletionFilter FILE_NOTIFY_CHANGE_LAST_ACCESS is handled correctly.")]
        public void BVT_SMB2Basic_ChangeNotify_ChangeLastAccess()
        {
            string testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Test directory \"{0}\" was created on share \"{1}\"", testDirectory, TestConfig.BasicFileShare);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client1 to create a directory \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", testDirectory);
            uint treeIdClient1;
            FILEID fileIdDir;
            SmbClientConnectAndOpenFile(out client1, testDirectory, out treeIdClient1, out fileIdDir);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client1 starts to register CHANGE_NOTIFY on directory \"{0}\" with CompletionFilter FILE_NOTIFY_CHANGE_LAST_ACCESS and flag WATCH_TREE", testDirectory);
            client1.ChangeNotify(
                treeIdClient1,
                fileIdDir,
                CompletionFilter_Values.FILE_NOTIFY_CHANGE_LAST_ACCESS,
                flags: CHANGE_NOTIFY_Request_Flags_Values.WATCH_TREE);

            string fileName = Guid.NewGuid().ToString();
            string filePath = testDirectory + "\\" + fileName;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client1 starts to create a file \"{0}\" under directory \"{1}\" by sending CREATE request", fileName, testDirectory);
            SmbClientCreateNewFile(client1, treeIdClient1, filePath);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client2 to open a file \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", filePath);
            uint treeIdClient2;
            FILEID fileIdFileToBeModified;
            SmbClientConnectAndOpenFile(
                out client2,
                filePath,
                out treeIdClient2,
                out fileIdFileToBeModified,
                createOption: CreateOptions_Values.FILE_NON_DIRECTORY_FILE |
                              CreateOptions_Values.FILE_DELETE_ON_CLOSE);

            // MS-FSCC 2.4.7 FileBasicInformation
            // Create a buffer for FileBasicInformation for setting last access time for the file.
            DateTime dateTimeToSet = DateTime.UtcNow;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client2 sets LastAccessTime for the file \"{0}\" to \"{1}\" by sending SET_INFO request", filePath, dateTimeToSet.ToString("MM/dd/yyy hh:mm:ss.ffffff"));
            client2.SetFileAttributes(
                treeIdClient2,
                (byte)FileInformationClasses.FileBasicInformation,
                fileIdFileToBeModified,
                CreateFileBasicInfo(client2, treeIdClient2, fileIdFileToBeModified, FileBasicInfoType.FileBasicInfoLastAccessTime, dateTimeToSet));

            BaseTestSite.Assert.IsTrue(
                changeNotificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                "CHANGE_NOTIFY should be received within {0} milliseconds", TestConfig.WaitTimeoutInMilliseconds);

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                receivedChangeNotifyHeader.Status,
                "CHANGE_NOTIFY is expected to success, actually server returns {0}.",
                Smb2Status.GetStatusCode(receivedChangeNotifyHeader.Status));

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down client2 by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client2, treeIdClient2, fileIdFileToBeModified);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down client1 by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client1, treeIdClient1, fileIdDir);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.ChangeNotify)]
        [Description("This test case is designed to verify CHANGE_NOTIFY with CompletionFilter FILE_NOTIFY_CHANGE_LAST_WRITE is handled correctly.")]
        public void BVT_SMB2Basic_ChangeNotify_ChangeLastWrite()
        {
            string testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Test directory \"{0}\" was created on share \"{1}\"", testDirectory, TestConfig.BasicFileShare);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client1 to create a directory \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", testDirectory);
            uint treeIdClient1;
            FILEID fileIdDir;
            SmbClientConnectAndOpenFile(out client1, testDirectory, out treeIdClient1, out fileIdDir);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client1 starts to register CHANGE_NOTIFY on directory \"{0}\" with CompletionFilter FILE_NOTIFY_CHANGE_LAST_WRITE and flag WATCH_TREE", testDirectory);
            client1.ChangeNotify(
                treeIdClient1,
                fileIdDir,
                CompletionFilter_Values.FILE_NOTIFY_CHANGE_LAST_WRITE,
                flags: CHANGE_NOTIFY_Request_Flags_Values.WATCH_TREE);

            string fileName = Guid.NewGuid().ToString();
            string filePath = testDirectory + "\\" + fileName;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client1 starts to create a file \"{0}\" under directory \"{1}\" by sending CREATE request", fileName, testDirectory);
            SmbClientCreateNewFile(client1, treeIdClient1, filePath);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client2 to open a file \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", filePath);
            uint treeIdClient2;
            FILEID fileIdFileToBeModified;
            SmbClientConnectAndOpenFile(
                out client2,
                filePath,
                out treeIdClient2,
                out fileIdFileToBeModified,
                createOption: CreateOptions_Values.FILE_NON_DIRECTORY_FILE |
                              CreateOptions_Values.FILE_DELETE_ON_CLOSE);

            // MS-FSCC 2.4.7 FileBasicInformation
            // Create a buffer for FileBasicInformation for setting last write time for the file.
            DateTime dateTimeToSet = DateTime.UtcNow;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client2 sets LastWriteTime for the file \"{0}\" to \"{1}\" by sending SET_INFO request", filePath, dateTimeToSet.ToString("MM/dd/yyy hh:mm:ss.ffffff"));
            client2.SetFileAttributes(
                treeIdClient2,
                (byte)FileInformationClasses.FileBasicInformation,
                fileIdFileToBeModified,
                CreateFileBasicInfo(client2, treeIdClient2, fileIdFileToBeModified, FileBasicInfoType.FileBasicInfoLastWriteTime, dateTimeToSet));

            BaseTestSite.Assert.IsTrue(
                changeNotificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                "CHANGE_NOTIFY should be received within {0} milliseconds", TestConfig.WaitTimeoutInMilliseconds);

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                receivedChangeNotifyHeader.Status, "CHANGE_NOTIFY is expected to success, actually server returns {0}.",
                Smb2Status.GetStatusCode(receivedChangeNotifyHeader.Status));

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down client2 by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client2, treeIdClient2, fileIdFileToBeModified);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down client1 by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client1, treeIdClient1, fileIdDir);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.ChangeNotify)]
        [Description("This test case is designed to verify CHANGE_NOTIFY with CompletionFilter FILE_NOTIFY_CHANGE_CREATION is handled correctly.")]
        public void BVT_SMB2Basic_ChangeNotify_ChangeCreation()
        {
            string testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Test directory \"{0}\" was created on share \"{1}\"", testDirectory, TestConfig.BasicFileShare);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client1 to create a directory \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", testDirectory);
            uint treeIdClient1;
            FILEID fileIdDir;
            SmbClientConnectAndOpenFile(out client1, testDirectory, out treeIdClient1, out fileIdDir);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client1 starts to register CHANGE_NOTIFY on directory \"{0}\" with CompletionFilter FILE_NOTIFY_CHANGE_CREATION and flag WATCH_TREE", testDirectory);
            client1.ChangeNotify(
                treeIdClient1,
                fileIdDir,
                CompletionFilter_Values.FILE_NOTIFY_CHANGE_CREATION,
                flags: CHANGE_NOTIFY_Request_Flags_Values.WATCH_TREE);

            string fileName = Guid.NewGuid().ToString();
            string filePath = testDirectory + "\\" + fileName;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client1 starts to create a file \"{0}\" under directory \"{1}\" by sending CREATE request", fileName, testDirectory);
            SmbClientCreateNewFile(client1, treeIdClient1, filePath);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client2 to open a file \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", filePath);
            uint treeIdClient2;
            FILEID fileIdFileToBeModified;
            SmbClientConnectAndOpenFile(
                out client2,
                filePath,
                out treeIdClient2,
                out fileIdFileToBeModified,
                createOption: CreateOptions_Values.FILE_NON_DIRECTORY_FILE |
                              CreateOptions_Values.FILE_DELETE_ON_CLOSE);

            // MS-FSCC 2.4.7 FileBasicInformation
            // Create a buffer for FileBasicInformation for setting creation time for the file.
            DateTime dateTimeToSet = DateTime.UtcNow;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client2 sets CreationTime for the file \"{0}\" to \"{1}\" by sending SET_INFO request", filePath, dateTimeToSet.ToString("MM/dd/yyy hh:mm:ss.ffffff"));
            client2.SetFileAttributes(
                treeIdClient2,
                (byte)FileInformationClasses.FileBasicInformation,
                fileIdFileToBeModified,
                CreateFileBasicInfo(client2, treeIdClient2, fileIdFileToBeModified, FileBasicInfoType.FileBasicInfoCreationTime, dateTimeToSet));

            BaseTestSite.Assert.IsTrue(
                changeNotificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                "CHANGE_NOTIFY should be received within {0} milliseconds", TestConfig.WaitTimeoutInMilliseconds);

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                receivedChangeNotifyHeader.Status, "CHANGE_NOTIFY is expected to success, actually server returns {0}.",
                Smb2Status.GetStatusCode(receivedChangeNotifyHeader.Status));

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down client2 by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client2, treeIdClient2, fileIdFileToBeModified);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down client1 by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client1, treeIdClient1, fileIdDir);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.ChangeNotify)]
        [Description("This test case is designed to verify CHANGE_NOTIFY with CompletionFilter FILE_NOTIFY_CHANGE_EA is handled correctly.")]
        public void BVT_SMB2Basic_ChangeNotify_ChangeEa()
        {
            string testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Test directory \"{0}\" was created on share \"{1}\"", testDirectory, TestConfig.BasicFileShare);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client to create a directory \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", testDirectory);
            uint status;
            uint treeIdClient1;
            FILEID fileIdDir;
            SmbClientConnectAndOpenFile(out client1, testDirectory, out treeIdClient1, out fileIdDir);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client starts to register CHANGE_NOTIFY on directory \"{0}\" with CompletionFilter FILE_NOTIFY_CHANGE_EA and flag WATCH_TREE", testDirectory);
            client1.ChangeNotify(
                treeIdClient1,
                fileIdDir,
                CompletionFilter_Values.FILE_NOTIFY_CHANGE_EA,
                flags: CHANGE_NOTIFY_Request_Flags_Values.WATCH_TREE);

            string fileName = Guid.NewGuid().ToString();
            string filePath = testDirectory + "\\" + fileName;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client starts to create a file \"{0}\" under directory \"{1}\" by sending CREATE request", fileName, testDirectory);
            FILEID fileIdFile;
            Smb2CreateContextResponse[] serverCreateContexts;
            status = client1.Create(
                treeIdClient1,
                filePath,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdFile,
                out serverCreateContexts);

            // MS-FSCC 2.4.15 FileFullEaInformation
            // Create a buffer for FileFullEaInformation
            // This information class is used to query or set extended attribute (EA) information for a file.
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client sets extended attribute information for the file \"{0}\" by sending SET_INFO request", filePath);
            client1.SetFileAttributes(
                treeIdClient1,
                (byte)FileInformationClasses.FileFullEaInformation,
                fileIdFile,
                CreateFileFullEaInfo());
            client1.Close(treeIdClient1, fileIdFile);

            BaseTestSite.Assert.IsTrue(
                changeNotificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                "CHANGE_NOTIFY should be received within {0} milliseconds", TestConfig.WaitTimeoutInMilliseconds);

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                receivedChangeNotifyHeader.Status,
                "CHANGE_NOTIFY is expected to success, actually server returns {0}.",
                Smb2Status.GetStatusCode(receivedChangeNotifyHeader.Status));

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down the client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client1, treeIdClient1, fileIdDir);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.ChangeNotify)]
        [Description("This test case is designed to verify CHANGE_NOTIFY with CompletionFilter FILE_NOTIFY_CHANGE_SECURITY is handled correctly.")]
        public void BVT_SMB2Basic_ChangeNotify_ChangeSecurity()
        {
            string testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Test directory \"{0}\" was created on share \"{1}\"", testDirectory, TestConfig.BasicFileShare);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client to create a directory \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", testDirectory);
            uint status;
            uint treeIdClient1;
            FILEID fileIdDir;
            SmbClientConnectAndOpenFile(out client1, testDirectory, out treeIdClient1, out fileIdDir);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client starts to register CHANGE_NOTIFY on directory \"{0}\" with CompletionFilter FILE_NOTIFY_CHANGE_SECURITY and flag WATCH_TREE", testDirectory);
            client1.ChangeNotify(
                treeIdClient1,
                fileIdDir,
                CompletionFilter_Values.FILE_NOTIFY_CHANGE_SECURITY,
                flags: CHANGE_NOTIFY_Request_Flags_Values.WATCH_TREE);

            string fileName = Guid.NewGuid().ToString();
            string filePath = testDirectory + "\\" + fileName;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client starts to create a file \"{0}\" under directory \"{1}\" by sending CREATE request", fileName, testDirectory);
            FILEID fileIdFile;
            Smb2CreateContextResponse[] serverCreateContexts;
            status = client1.Create(
                treeIdClient1,
                filePath,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdFile,
                out serverCreateContexts,
                accessMask: AccessMask.GENERIC_READ |
                            AccessMask.GENERIC_WRITE |
                            AccessMask.DELETE |
                            AccessMask.WRITE_DAC |
                            AccessMask.WRITE_OWNER |
                            AccessMask.ACCESS_SYSTEM_SECURITY);

            // MS-DTYP 2.4.5 ACL
            // Create a security descriptor with SACL information according to MS-DTYP 2.4.5.
            // A system access control list (SACL) is similar to the DACL, except that the SACL is used to audit rather than control access to an object.
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client sets SACL_SECURITY_INFORMATION for the file \"{0}\" by sending SET_INFO request", filePath);
            client1.SetSecurityDescriptor(
                treeIdClient1,
                fileIdFile,
                SET_INFO_Request_AdditionalInformation_Values.SACL_SECURITY_INFORMATION,
                CreateSecurityDescriptorSACL());
            client1.Close(treeIdClient1, fileIdFile);

            BaseTestSite.Assert.IsTrue(
                changeNotificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                "CHANGE_NOTIFY should be received within {0} milliseconds", TestConfig.WaitTimeoutInMilliseconds);

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                receivedChangeNotifyHeader.Status,
                "CHANGE_NOTIFY is expected to success, actually server returns {0}.",
                Smb2Status.GetStatusCode(receivedChangeNotifyHeader.Status));

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down the client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client1, treeIdClient1, fileIdDir);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.ChangeNotify)]
        [Description("This test case is designed to verify CHANGE_NOTIFY with CompletionFilter FILE_NOTIFY_CHANGE_STREAM_NAME is handled correctly.")]
        public void BVT_SMB2Basic_ChangeNotify_ChangeStreamName()
        {
            string testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Test directory \"{0}\" was created on share \"{1}\"", testDirectory, TestConfig.BasicFileShare);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client1 to create a directory \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", testDirectory);
            uint treeIdClient1;
            FILEID fileIdDir;
            SmbClientConnectAndOpenFile(out client1, testDirectory, out treeIdClient1, out fileIdDir);

            string fileName = Guid.NewGuid().ToString();
            string filePath = testDirectory + "\\" + fileName;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client1 starts to create a file \"{0}\" under directory \"{1}\" by sending CREATE request", fileName, testDirectory);
            SmbClientCreateNewFile(client1, treeIdClient1, filePath);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client1 starts to register CHANGE_NOTIFY on directory \"{0}\" with CompletionFilter FILE_NOTIFY_CHANGE_STREAM_NAME and flag WATCH_TREE", testDirectory);
            client1.ChangeNotify(
                treeIdClient1,
                fileIdDir,
                CompletionFilter_Values.FILE_NOTIFY_CHANGE_STREAM_NAME,
                flags: CHANGE_NOTIFY_Request_Flags_Values.WATCH_TREE);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client2 to open a file \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", filePath);
            uint treeIdClient2;
            FILEID fileIdFileToBeModified;
            SmbClientConnectAndOpenFile(
                out client2,
                filePath,
                out treeIdClient2,
                out fileIdFileToBeModified,
                createOption: CreateOptions_Values.FILE_NON_DIRECTORY_FILE |
                              CreateOptions_Values.FILE_DELETE_ON_CLOSE);

            string dataStreamPath = filePath + ":$DATA";
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client2 starts to open a data stream \"{0}\" by sending CREATE request and write data to it by sending WRITE request", dataStreamPath);
            SmbClientWriteDataStream(client2, treeIdClient2, dataStreamPath, 12);

            BaseTestSite.Assert.IsTrue(
                changeNotificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                "CHANGE_NOTIFY should be received within {0} milliseconds", TestConfig.WaitTimeoutInMilliseconds);

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                receivedChangeNotifyHeader.Status, "CHANGE_NOTIFY is expected to success, actually server returns {0}.",
                Smb2Status.GetStatusCode(receivedChangeNotifyHeader.Status));

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down client2 by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client2, treeIdClient2, fileIdFileToBeModified);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down client1 by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client1, treeIdClient1, fileIdDir);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.ChangeNotify)]
        [Description("This test case is designed to verify CHANGE_NOTIFY with CompletionFilter FILE_NOTIFY_CHANGE_STREAM_SIZE is handled correctly.")]
        public void BVT_SMB2Basic_ChangeNotify_ChangeStreamSize()
        {
            string testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Test directory \"{0}\" was created on share \"{1}\"", testDirectory, TestConfig.BasicFileShare);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client1 to create a directory \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", testDirectory);
            uint treeIdClient1;
            FILEID fileIdDir;
            SmbClientConnectAndOpenFile(out client1, testDirectory, out treeIdClient1, out fileIdDir);

            string fileName = Guid.NewGuid().ToString();
            string filePath = testDirectory + "\\" + fileName;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client1 starts to create a file \"{0}\" under directory \"{1}\" by sending CREATE request", fileName, testDirectory);
            SmbClientCreateNewFile(client1, treeIdClient1, filePath);

            string dataStreamPath = filePath + ":$DATA";
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client1 starts to create a data stream \"{0}\" by sending CREATE request and write data to it by sending WRITE request", dataStreamPath);
            SmbClientWriteDataStream(client1, treeIdClient1, dataStreamPath, 9);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client1 starts to register CHANGE_NOTIFY on directory \"{0}\" with CompletionFilter FILE_NOTIFY_CHANGE_STREAM_SIZE and flag WATCH_TREE", testDirectory);
            client1.ChangeNotify(
                treeIdClient1,
                fileIdDir,
                CompletionFilter_Values.FILE_NOTIFY_CHANGE_STREAM_SIZE,
                flags: CHANGE_NOTIFY_Request_Flags_Values.WATCH_TREE);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client2 to open a file \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", filePath);
            uint treeIdClient2;
            FILEID fileIdFileToBeModified;
            SmbClientConnectAndOpenFile(
                out client2,
                filePath,
                out treeIdClient2,
                out fileIdFileToBeModified,
                createOption: CreateOptions_Values.FILE_NON_DIRECTORY_FILE |
                              CreateOptions_Values.FILE_DELETE_ON_CLOSE);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client2 starts to open a data stream \"{0}\" by sending CREATE request and write data to it by sending WRITE request", dataStreamPath);
            SmbClientWriteDataStream(client2, treeIdClient2, dataStreamPath, 12);

            BaseTestSite.Assert.IsTrue(
                changeNotificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                "CHANGE_NOTIFY should be received within {0} milliseconds", TestConfig.WaitTimeoutInMilliseconds);

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                receivedChangeNotifyHeader.Status, "CHANGE_NOTIFY is expected to success, actually server returns {0}.",
                Smb2Status.GetStatusCode(receivedChangeNotifyHeader.Status));

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down client2 by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client2, treeIdClient2, fileIdFileToBeModified);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down client1 by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client1, treeIdClient1, fileIdDir);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.ChangeNotify)]
        [Description("This test case is designed to verify CHANGE_NOTIFY with CompletionFilter FILE_NOTIFY_CHANGE_STREAM_WRITE is handled correctly.")]
        public void BVT_SMB2Basic_ChangeNotify_ChangeStreamWrite()
        {
            string testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Test directory \"{0}\" was created on share \"{1}\"", testDirectory, TestConfig.BasicFileShare);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client1 to create a directory \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", testDirectory);
            uint treeIdClient1;
            FILEID fileIdDir;
            SmbClientConnectAndOpenFile(out client1, testDirectory, out treeIdClient1, out fileIdDir);

            string fileName = Guid.NewGuid().ToString();
            string filePath = testDirectory + "\\" + fileName;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client1 starts to create a file \"{0}\" under directory \"{1}\" by sending CREATE request", fileName, testDirectory);
            SmbClientCreateNewFile(client1, treeIdClient1, filePath);

            string dataStreamPath = filePath + ":$DATA";
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client2 starts to create a data stream \"{0}\" by sending CREATE request and write data to it by sending WRITE request", dataStreamPath);
            SmbClientWriteDataStream(client1, treeIdClient1, dataStreamPath, 9);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client1 starts to register CHANGE_NOTIFY on directory \"{0}\" with CompletionFilter FILE_NOTIFY_CHANGE_STREAM_WRITE and flag WATCH_TREE", testDirectory);
            client1.ChangeNotify(
                treeIdClient1,
                fileIdDir,
                CompletionFilter_Values.FILE_NOTIFY_CHANGE_STREAM_WRITE,
                flags: CHANGE_NOTIFY_Request_Flags_Values.WATCH_TREE);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client2 to open a file \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", filePath);
            uint treeIdClient2;
            FILEID fileIdFileToBeModified;
            SmbClientConnectAndOpenFile(
                out client2,
                filePath,
                out treeIdClient2,
                out fileIdFileToBeModified,
                createOption: CreateOptions_Values.FILE_NON_DIRECTORY_FILE |
                              CreateOptions_Values.FILE_DELETE_ON_CLOSE);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client2 starts to open a data stream \"{0}\" by sending CREATE request and write data to it by sending WRITE request", dataStreamPath);
            SmbClientWriteDataStream(client2, treeIdClient2, dataStreamPath, 13);

            BaseTestSite.Assert.IsTrue(
                changeNotificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                "CHANGE_NOTIFY should be received within {0} milliseconds", TestConfig.WaitTimeoutInMilliseconds);

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                receivedChangeNotifyHeader.Status, "CHANGE_NOTIFY is expected to success, actually server returns {0}.",
                Smb2Status.GetStatusCode(receivedChangeNotifyHeader.Status));

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down client2 by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client2, treeIdClient2, fileIdFileToBeModified);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down client1 by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client1, treeIdClient1, fileIdDir);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.ChangeNotify)]
        [Description("This test case is designed to verify that server must send an CHANGE_NOTIFY response with STATUS_NOTIFY_CLEANUP status code for all pending CHANGE_NOTIFY requests associated with the FileId that is closed.")]
        public void BVT_SMB2Basic_ChangeNotify_ServerReceiveSmb2Close()
        {
            string testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Test directory \"{0}\" was created on share \"{1}\"", testDirectory, TestConfig.BasicFileShare);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client to create a directory \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", testDirectory);
            uint treeIdClient1;
            FILEID fileIdDir;
            SmbClientConnectAndOpenFile(out client1, testDirectory, out treeIdClient1, out fileIdDir);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client starts to register CHANGE_NOTIFY on directory \"{0}\" with CompletionFilter FILE_NOTIFY_CHANGE_LAST_ACCESS", testDirectory);
            client1.ChangeNotify(
                treeIdClient1,
                fileIdDir,
                CompletionFilter_Values.FILE_NOTIFY_CHANGE_LAST_ACCESS);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client starts to close a directory \"{0}\" by sending a CLOSE Request", testDirectory);
            client1.Close(treeIdClient1, fileIdDir);

            BaseTestSite.Assert.IsTrue(
                changeNotificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                "CHANGE_NOTIFY should be received within {0} milliseconds", TestConfig.WaitTimeoutInMilliseconds);

            // MS-SMB2 3.3.5.10 Receiving an SMB2 CLOSE Request
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_NOTIFY_CLEANUP,
                receivedChangeNotifyHeader.Status,
                "The Server MUST send an SMB2 CHANGE_NOTIFY Response with STATUS_NOTIFY_CLEANUP status code for all pending CHANGE_NOTIFY requests associated with the FileId that is closed. Actually server returns {0}.",
                Smb2Status.GetStatusCode(receivedChangeNotifyHeader.Status));

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down the client by sending the following requests: TREE_DISCONNECT; LOG_OFF");
            SmbClientDisconnect(client1, treeIdClient1);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.ChangeNotify)]
        [Description("This test case is designed to verify that server must send an CHANGE_NOTIFY response with STATUS_INVALID status code if CHANGE_NOTIFY request is for a non-directory file.")]
        public void BVT_SMB2Basic_ChangeNotify_NonDirectoryFile()
        {
            string testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Test directory \"{0}\" was created on share \"{1}\"", testDirectory, TestConfig.BasicFileShare);

            string fileName = Guid.NewGuid().ToString();
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client to create a file \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", fileName);
            uint treeIdClient1;
            FILEID fileIdFile;
            SmbClientConnectAndOpenFile(out client1, fileName, out treeIdClient1, out fileIdFile, createOption: CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client starts to register CHANGE_NOTIFY on a non-directory file \"{0}\"", fileName);
            client1.ChangeNotify(
                treeIdClient1,
                fileIdFile,
                CompletionFilter_Values.FILE_NOTIFY_CHANGE_LAST_ACCESS);

            BaseTestSite.Assert.IsTrue(
                changeNotificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                "CHANGE_NOTIFY should be received within {0} milliseconds", TestConfig.WaitTimeoutInMilliseconds);

            // MS-SMB2 3.3.5.19 Receiving an SMB2 CHANGE_NOTIFY request
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_INVALID_PARAMETER,
                receivedChangeNotifyHeader.Status,
                "If the open is not an open to a directory, the request MUST be failed with STATUS_INVALID_PARAMETER. Actually server returns {0}.",
                Smb2Status.GetStatusCode(receivedChangeNotifyHeader.Status));

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down the client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client1, treeIdClient1, fileIdFile);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.ChangeNotify)]
        [Description("This test case is designed to verify the MaxTransactSize in CHANGE_NOTIFY request in SMB 2.0.2.")]
        public void BVT_SMB2Basic_ChangeNotify_MaxTransactSizeCheck_Smb2002()
        {
            TestConfig.CheckDialect(DialectRevision.Smb2002);

            string testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Test directory \"{0}\" was created on share \"{1}\"", testDirectory, TestConfig.BasicFileShare);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client to create a directory \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", testDirectory);
            uint treeIdClient1;
            FILEID fileIdDir;
            SmbClientConnectAndOpenFile(out client1, testDirectory, out treeIdClient1, out fileIdDir);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client starts to register CHANGE_NOTIFY on directory \"{0}\" with CompletionFilter FILE_NOTIFY_CHANGE_LAST_ACCESS and MaxOutputBufferLength {1}", testDirectory, client1.MaxTransactSize + 1);
            client1.ChangeNotify(
                treeIdClient1,
                fileIdDir,
                CompletionFilter_Values.FILE_NOTIFY_CHANGE_LAST_ACCESS,
                maxOutputBufferLength: client1.MaxTransactSize + 1);

            BaseTestSite.Assert.IsTrue(
                changeNotificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                "CHANGE_NOTIFY should be received within {0} milliseconds", TestConfig.WaitTimeoutInMilliseconds);

            // MS-SMB2 3.3.5.19 Receiving an SMB2 CHANGE_NOTIFY request
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_INVALID_PARAMETER,
                receivedChangeNotifyHeader.Status,
                "If OutputBufferLength is greater than Connection.MaxTransactSize, the server SHOULD fail the request with STATUS_INVALID_PARAMETER. Actually server returns {0}.",
                Smb2Status.GetStatusCode(receivedChangeNotifyHeader.Status));

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down the client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client1, treeIdClient1, fileIdDir);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.ChangeNotify)]
        [Description("This test case is designed to verify the MaxTransactSize in CHANGE_NOTIFY request in SMB 2.1.0.")]
        public void BVT_SMB2Basic_ChangeNotify_MaxTransactSizeCheck_Smb21()
        {
            TestConfig.CheckDialect(DialectRevision.Smb21);

            string testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Test directory \"{0}\" was created on share \"{1}\"", testDirectory, TestConfig.BasicFileShare);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client to create a directory \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", testDirectory);
            uint treeIdClient1;
            FILEID fileIdDir;
            SmbClientConnectAndOpenFile(out client1, testDirectory, out treeIdClient1, out fileIdDir);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client starts to register CHANGE_NOTIFY on directory \"{0}\" with CompletionFilter FILE_NOTIFY_CHANGE_LAST_ACCESS and MaxOutputBufferLength {1}", testDirectory, client1.MaxTransactSize + 1);
            client1.ChangeNotify(
                treeIdClient1,
                fileIdDir,
                CompletionFilter_Values.FILE_NOTIFY_CHANGE_LAST_ACCESS,
                maxOutputBufferLength: client1.MaxTransactSize + 1);

            BaseTestSite.Assert.IsTrue(
                changeNotificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                "CHANGE_NOTIFY should be received within {0} milliseconds", TestConfig.WaitTimeoutInMilliseconds);

            // MS-SMB2 3.3.5.19 Receiving an SMB2 CHANGE_NOTIFY request
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_INVALID_PARAMETER,
                receivedChangeNotifyHeader.Status,
                "If OutputBufferLength is greater than Connection.MaxTransactSize, the server SHOULD fail the request with STATUS_INVALID_PARAMETER. Actually server returns {0}.",
                Smb2Status.GetStatusCode(receivedChangeNotifyHeader.Status));

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down the client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client1, treeIdClient1, fileIdDir);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.ChangeNotify)]
        [Description("This test case is designed to verify the MaxTransactSize in CHANGE_NOTIFY request in SMB 3.0.")]
        public void BVT_SMB2Basic_ChangeNotify_MaxTransactSizeCheck_Smb30()
        {
            TestConfig.CheckDialect(DialectRevision.Smb30);

            string testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Test directory \"{0}\" was created on share \"{1}\"", testDirectory, TestConfig.BasicFileShare);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client to create a directory \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", testDirectory);
            uint treeIdClient1;
            FILEID fileIdDir;
            SmbClientConnectAndOpenFile(out client1, testDirectory, out treeIdClient1, out fileIdDir);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client starts to register CHANGE_NOTIFY on directory \"{0}\" with CompletionFilter FILE_NOTIFY_CHANGE_LAST_ACCESS and MaxOutputBufferLength {1}", testDirectory, client1.MaxTransactSize + 1);
            client1.ChangeNotify(
                treeIdClient1,
                fileIdDir,
                CompletionFilter_Values.FILE_NOTIFY_CHANGE_LAST_ACCESS,
                maxOutputBufferLength: client1.MaxTransactSize + 1);

            BaseTestSite.Assert.IsTrue(
                changeNotificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                "CHANGE_NOTIFY should be received within {0} milliseconds", TestConfig.WaitTimeoutInMilliseconds);

            // MS-SMB2 3.3.5.19 Receiving an SMB2 CHANGE_NOTIFY request
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_INVALID_PARAMETER,
                receivedChangeNotifyHeader.Status,
                "If OutputBufferLength is greater than Connection.MaxTransactSize, the server SHOULD fail the request with STATUS_INVALID_PARAMETER. Actually server returns {0}.",
                Smb2Status.GetStatusCode(receivedChangeNotifyHeader.Status));

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down the client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client1, treeIdClient1, fileIdDir);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb302)]
        [TestCategory(TestCategories.ChangeNotify)]
        [Description("This test case is designed to verify the MaxTransactSize in CHANGE_NOTIFY request in SMB 3.0.2.")]
        public void BVT_SMB2Basic_ChangeNotify_MaxTransactSizeCheck_Smb302()
        {
            TestConfig.CheckDialect(DialectRevision.Smb302);

            string testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Test directory \"{0}\" was created on share \"{1}\"", testDirectory, TestConfig.BasicFileShare);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client to create a directory \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", testDirectory);
            uint treeIdClient1;
            FILEID fileIdDir;
            SmbClientConnectAndOpenFile(out client1, testDirectory, out treeIdClient1, out fileIdDir);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client starts to register CHANGE_NOTIFY on directory \"{0}\" with CompletionFilter FILE_NOTIFY_CHANGE_LAST_ACCESS and MaxOutputBufferLength {1}", testDirectory, client1.MaxTransactSize + 1);
            client1.ChangeNotify(
                treeIdClient1,
                fileIdDir,
                CompletionFilter_Values.FILE_NOTIFY_CHANGE_LAST_ACCESS,
                maxOutputBufferLength: client1.MaxTransactSize + 1);

            BaseTestSite.Assert.IsTrue(
                changeNotificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                "CHANGE_NOTIFY should be received within {0} milliseconds", TestConfig.WaitTimeoutInMilliseconds);

            // MS-SMB2 3.3.5.19 Receiving an SMB2 CHANGE_NOTIFY request
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_INVALID_PARAMETER,
                receivedChangeNotifyHeader.Status,
                "If OutputBufferLength is greater than Connection.MaxTransactSize, the server SHOULD fail the request with STATUS_INVALID_PARAMETER. Actually server returns {0}.",
                Smb2Status.GetStatusCode(receivedChangeNotifyHeader.Status));

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down the client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client1, treeIdClient1, fileIdDir);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.ChangeNotify)]
        [Description("This test case is designed to verify the MaxTransactSize in CHANGE_NOTIFY request in SMB 3.1.1.")]
        public void BVT_SMB2Basic_ChangeNotify_MaxTransactSizeCheck_Smb311()
        {
            TestConfig.CheckDialect(DialectRevision.Smb311);

            string testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Test directory \"{0}\" was created on share \"{1}\"", testDirectory, TestConfig.BasicFileShare);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client to create a directory \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", testDirectory);
            uint treeIdClient1;
            FILEID fileIdDir;
            SmbClientConnectAndOpenFile(out client1, testDirectory, out treeIdClient1, out fileIdDir);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client starts to register CHANGE_NOTIFY on directory \"{0}\" with CompletionFilter FILE_NOTIFY_CHANGE_LAST_ACCESS and MaxOutputBufferLength {1}", testDirectory, client1.MaxTransactSize + 1);
            client1.ChangeNotify(
                treeIdClient1,
                fileIdDir,
                CompletionFilter_Values.FILE_NOTIFY_CHANGE_LAST_ACCESS,
                maxOutputBufferLength: client1.MaxTransactSize + 1);

            BaseTestSite.Assert.IsTrue(
                changeNotificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                "CHANGE_NOTIFY should be received within {0} milliseconds", TestConfig.WaitTimeoutInMilliseconds);

            // MS-SMB2 3.3.5.19 Receiving an SMB2 CHANGE_NOTIFY request
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_INVALID_PARAMETER,
                receivedChangeNotifyHeader.Status,
                "If OutputBufferLength is greater than Connection.MaxTransactSize, the server SHOULD fail the request with STATUS_INVALID_PARAMETER. Actually server returns {0}.",
                Smb2Status.GetStatusCode(receivedChangeNotifyHeader.Status));

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down the client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client1, treeIdClient1, fileIdDir);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.ChangeNotify)]
        [Description("This test case is designed to verify that server must send an CHANGE_NOTIFY response with STATUS_ACCESS_DENIED status code if CHANGE_NOTIFY request is for a directory which GrantedAccess does not include FILE_LIST_DIRECTORY.")]
        public void BVT_SMB2Basic_ChangeNotify_NoFileListDirectoryInGrantedAccess()
        {
            string testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Test directory \"{0}\" was created on share \"{1}\"", testDirectory, TestConfig.BasicFileShare);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start client to create a directory \"{0}\" by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", testDirectory);
            uint treeIdClient1;
            FILEID fileIdDir;
            SmbClientConnectAndOpenFile(
                out client1,
                testDirectory,
                out treeIdClient1,
                out fileIdDir,
                applyAccessMask: AccessMask.GENERIC_WRITE | AccessMask.DELETE);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client starts to register CHANGE_NOTIFY on a directory \"{0}\" with CompletionFilter FILE_NOTIFY_LAST_ACCESS", testDirectory);
            client1.ChangeNotify(
                treeIdClient1,
                fileIdDir,
                CompletionFilter_Values.FILE_NOTIFY_CHANGE_LAST_ACCESS);

            BaseTestSite.Assert.IsTrue(
                changeNotificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                "CHANGE_NOTIFY should be received within {0} milliseconds", TestConfig.WaitTimeoutInMilliseconds);

            // MS-SMB2 3.3.5.19 Receiving an SMB2 CHANGE_NOTIFY request
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_ACCESS_DENIED,
                receivedChangeNotifyHeader.Status,
                "If Open.GrantedAccess does not include FILE_LIST_DIRECTORY, the operation MUST be failed with STATUS_ACCESS_DENIED. Actually server returns {0}.",
                Smb2Status.GetStatusCode(receivedChangeNotifyHeader.Status));

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down the client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            SmbClientCloseFileAndDisconnect(client1, treeIdClient1, fileIdDir);
        }

        #endregion

        /// <summary>
        /// Connect by sending NEGOTIATE, SESSION_SETUP and TREE_CONNECT request and create file by sending CREATE request.
        /// </summary>
        private void SmbClientConnectAndOpenFile(
            out Smb2FunctionalClient client,
            string fileName,
            out uint treeId,
            out FILEID fileId,
            AccessMask applyAccessMask = AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE,
            CreateOptions_Values createOption = CreateOptions_Values.FILE_DIRECTORY_FILE)
        {
            uint status;
            Smb2FunctionalClient newClient;

            newClient = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            newClient.Smb2Client.ChangeNotifyResponseReceived += new Action<FILE_NOTIFY_INFORMATION[], Packet_Header, CHANGE_NOTIFY_Response>(OnChangeNotifyResponseReceived);

            uint _treeId;
            SmbClientConnect(newClient, out _treeId);
            FILEID _fileID;
            Smb2CreateContextResponse[] serverCreateContexts;
            status = newClient.Create(
                _treeId,
                fileName,
                createOption,
                out _fileID,
                out serverCreateContexts,
                accessMask: applyAccessMask);
            client = newClient;
            treeId = _treeId;
            fileId = _fileID;
        }

        /// <summary>
        /// Connect by sending NEGOTIATE, SESSION_SETUP and TREE_CONNECT request.
        /// </summary>
        private void SmbClientConnect(Smb2FunctionalClient client, out uint treeId)
        {
            uint status;
            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            status = client.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);
            status = client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);
            uint treeIdClient;
            status = client.TreeConnect(uncSharePath, out treeIdClient);
            treeId = treeIdClient;
        }

        /// <summary>
        /// Disconnect by sending TREE_DISCONNECT and LOGOFF request.
        /// </summary>
        private void SmbClientDisconnect(Smb2FunctionalClient client, uint treeId)
        {
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        /// <summary>
        /// Close file by sending CLOSE request and disconnect by sending TREE_DISCONNECT and LOGOFF request.
        /// </summary>
        private void SmbClientCloseFileAndDisconnect(Smb2FunctionalClient client, uint treeId, FILEID fileId)
        {
            client.Close(treeId, fileId);
            SmbClientDisconnect(client, treeId);
        }

        /// <summary>
        /// Create a new data stream by sending CREATE request and write writeLen bytes to it by sending WRITE request.
        /// </summary>
        private void SmbClientWriteDataStream(Smb2FunctionalClient client, uint treeId, string dataStreamPath, int writeLen)
        {
            uint status;
            FILEID fileIdDataStream;
            Smb2CreateContextResponse[] serverCreateContexts;
            status = client.Create(
                treeId,
                dataStreamPath,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdDataStream,
                out serverCreateContexts,
                fileAttributes: File_Attributes.FILE_ATTRIBUTE_NORMAL);
            client.Write(treeId, fileIdDataStream, Smb2Utility.CreateRandomString(writeLen));
            client.Close(treeId, fileIdDataStream);
        }

        /// <summary>
        /// Create a new file by sending CREATE request.
        /// </summary>
        private void SmbClientCreateNewFile(Smb2FunctionalClient client, uint treeId, string filePath, CreateOptions_Values createOption = CreateOptions_Values.FILE_NON_DIRECTORY_FILE)
        {
            FILEID fileId;
            Smb2CreateContextResponse[] serverCreateContexts;
            uint status = client.Create(
                treeId,
                filePath,
                createOption,
                out fileId,
                out serverCreateContexts);
            client1.Close(treeId, fileId);
        }

        /// <summary>
        /// Create a new file by sending CREATE request and write writeLen bytes to it by sending WRITE request.
        /// </summary>
        private void SmbClientCreateNewFileAndWrite(Smb2FunctionalClient client, uint treeId, string filePath, int writeLen)
        {
            FILEID fileId;
            Smb2CreateContextResponse[] serverCreateContexts;
            uint status = client.Create(
                treeId,
                filePath,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                out fileId,
                out serverCreateContexts);
            client.Write(treeId, fileId, Smb2Utility.CreateRandomString(writeLen));
            client1.Close(treeId, fileId);
        }

        /// <summary>
        /// Create a buffer for FileRenameInformation according to MS-FSCC 2.4.34.
        /// This information class is used to rename a file.
        /// </summary>
        private byte[] CreateFileRenameInfo(string fileName)
        {
            FileRenameInformation fileRenameInfo;
            fileRenameInfo.ReplaceIfExists = TypeMarshal.ToBytes(false)[0];
            fileRenameInfo.Reserved = new byte[7];
            fileRenameInfo.RootDirectory = FileRenameInformation_RootDirectory_Values.V1;
            fileRenameInfo.FileName = Encoding.Unicode.GetBytes(fileName);
            fileRenameInfo.FileNameLength = (uint)fileRenameInfo.FileName.Length;
            return TypeMarshal.ToBytes<FileRenameInformation>(fileRenameInfo);
        }

        /// <summary>
        /// Create a buffer for FileBasicInformation according to MS-FSCC 2.4.7.
        /// This information class is used to query or set file information.
        /// </summary>
        private byte[] CreateFileBasicInfo(Smb2FunctionalClient client, uint treeId, FILEID fileId, FileBasicInfoType fileBasicInfoType, DateTime dateTimeToSet)
        {
            byte[] outputBuffer;
            client.QueryFileAttributes(
                treeId,
                (byte)FileInformationClasses.FileBasicInformation,
                QUERY_INFO_Request_Flags_Values.SL_RESTART_SCAN,
                fileId,
                new byte[0] { },
                out outputBuffer);

            FileBasicInformation fileBasicInfoToSet = TypeMarshal.ToStruct<FileBasicInformation>(outputBuffer);
            switch (fileBasicInfoType) {
                case FileBasicInfoType.FileBasicInfoAttribute:
                    fileBasicInfoToSet.FileAttributes = File_Attributes.FILE_ATTRIBUTE_HIDDEN;
                    break;
                case FileBasicInfoType.FileBasicInfoCreationTime:
                    fileBasicInfoToSet.CreationTime = Smb2Utility.ConvertToFileTime(dateTimeToSet);
                    break;
                case FileBasicInfoType.FileBasicInfoLastAccessTime:
                    fileBasicInfoToSet.LastAccessTime = Smb2Utility.ConvertToFileTime(dateTimeToSet);
                    break;
                case FileBasicInfoType.FileBasicInfoLastWriteTime:
                    fileBasicInfoToSet.LastWriteTime = Smb2Utility.ConvertToFileTime(dateTimeToSet);
                    break;
                default:
                    break;
            }
            return TypeMarshal.ToBytes<FileBasicInformation>(fileBasicInfoToSet);
        }

        /// <summary>
        /// Create a buffer for FileFullEaInformation according to MS-FSCC 2.4.15.
        /// This information class is used to query or set extended attribute (EA) information for a file.
        /// </summary>
        private byte[] CreateFileFullEaInfo()
        {
            string eaName = Guid.NewGuid().ToString();
            string eaValue = Guid.NewGuid().ToString();
            FileFullEaInformation fileFullEaInfo;
            fileFullEaInfo.NextEntryOffset = 0;
            fileFullEaInfo.Flags = 0;
            fileFullEaInfo.EaNameLength = (byte)eaName.Length;
            fileFullEaInfo.EaName = Encoding.ASCII.GetBytes(eaName + "\0");
            fileFullEaInfo.EaValueLength = (ushort)eaValue.Length;
            fileFullEaInfo.EaValue = Encoding.ASCII.GetBytes(eaValue);
            return TypeMarshal.ToBytes<FileFullEaInformation>(fileFullEaInfo);
        }

        /// <summary>
        /// Create a buffer for FileEndOfFileInformation according to MS-FSCC 2.4.13.
        /// This information class is used to set end-of-file information for a file.
        /// </summary>
        /// <param name="newEofPos">New end of file position as a byte offset from the start of the file</param>
        private byte[] CreateFileEndOfFileInfo(long newEofPos)
        {
            FileEndOfFileInformation fileEofInfo;
            fileEofInfo.EndOfFile = newEofPos;
            return TypeMarshal.ToBytes<FileEndOfFileInformation>(fileEofInfo);
        }

        /// <summary>
        /// Create a security descriptor with SACL information according to MS-DTYP 2.4.5.
        /// A system access control list (SACL) is similar to the DACL, except that the SACL is used to audit rather than control access to an object.
        /// </summary>
        private _SECURITY_DESCRIPTOR CreateSecurityDescriptorSACL()
        {
            _ACL sacl = DtypUtility.CreateAcl(false);
            return DtypUtility.CreateSecurityDescriptor(
                SECURITY_DESCRIPTOR_Control.SACLAutoInherited | SECURITY_DESCRIPTOR_Control.SACLInheritanceRequired |
                SECURITY_DESCRIPTOR_Control.SACLPresent | SECURITY_DESCRIPTOR_Control.SelfRelative,
                null,
                null,
                sacl,
                null);
        }

        private void OnChangeNotifyResponseReceived(FILE_NOTIFY_INFORMATION[] fileNotifyInfo, Packet_Header respHeader, CHANGE_NOTIFY_Response changeNotify)
        {
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Client1 receives the response of CHANGE_NOTIFY");
            receivedChangeNotify = changeNotify;
            receivedFileNotifyInfo = fileNotifyInfo;
            receivedChangeNotifyHeader = respHeader;
            if (receivedChangeNotifyHeader.Status == Smb2Status.STATUS_SUCCESS)
            {
                // According to MS-SMB2 3.5.16
                BaseTestSite.Assert.IsTrue(
                    receivedChangeNotify.OutputBufferLength != 0,
                    "If the status field of the SMB2 header of the response indicated success, the client MUST copy the received information OutputBufferLength in the SMB2 CHANGE_NOTIFY Response to the application");

                // According to MS-FSCC 2.7.1
                BaseTestSite.Assert.IsTrue(
                    fileNotifyInfo[0].NextEntryOffset % 4 == 0,
                    "NextEntryOffset MUST always be an integral multiple of 4");
                BaseTestSite.Assert.IsTrue(
                    fileNotifyInfo[0].Action != 0,
                    "The changes that occurred on the file. This field MUST contain one of the following values. (For details of Action value, see MS-FSCC section 2.7.1)");
            }
            changeNotificationReceived.Set();
        }

        private void ReplacePacketByStructureSize(Smb2Packet packet)
        {
            Smb2CreateRequestPacket request = packet as Smb2CreateRequestPacket;
            if (request == null)
                return;
            request.PayLoad.StructureSize += 1;
        }

        private void QueryDir_Reopen(FileType fileType)
        {
            string target = (fileType == FileType.DataFile) ? "file" : "directory";
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start a client to create a {0} by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE", target);
            client1 = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client1.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);

            uint status = client1.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled);

            status = client1.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);

            uint treeId;
            status = client1.TreeConnect(uncSharePath, out treeId);

            FILEID fileId;
            Smb2CreateContextResponse[] serverCreateContexts;
            CreateOptions_Values createOptions = (fileType == FileType.DataFile) ? CreateOptions_Values.FILE_NON_DIRECTORY_FILE : CreateOptions_Values.FILE_DIRECTORY_FILE;

            string fileName = GetTestFileName(uncSharePath);
            status = client1.Create(
                treeId,
                fileName,
                createOptions | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                out fileId,
                out serverCreateContexts);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends QUERY_DIRECTORY request with flag SMB2_REOPEN to query directory information on a {0}.", target);
            byte[] outputBuffer;
            status = client1.QueryDirectory(
                treeId,
                FileInformationClass_Values.FileDirectoryInformation,
                QUERY_DIRECTORY_Request_Flags_Values.REOPEN,
                0,
                fileId,
                out outputBuffer,
                checker: (header, response) => { }
                );

            if (fileType == FileType.DataFile)
            {
                // MS-SMB2 section 3.3.5.18 Receiving an SMB2 QUERY_DIRECTORY Request
                BaseTestSite.Log.Add(LogEntryKind.TestStep,
                    "If the open is not an open to a directory, the server MUST process the request as follows:\n");

                if (testConfig.Platform == Platform.WindowsServer2008 ||
                    testConfig.Platform == Platform.WindowsServer2008R2 ||
                    testConfig.Platform == Platform.WindowsServer2012 ||
                    testConfig.Platform == Platform.WindowsServer2012R2)
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_NOT_SUPPORTED,
                        status,
                        "If SMB2_REOPEN is set in the Flags field of the SMB2 QUERY_DIRECTORY request, the request MUST be failed with an inplementation-specific error code:\n" +
                        "Windows Server 2008, Windows Server 2008R2, Windows Server 2012 and Windows Server 2012 R2 fail the request with STATUS_NOT_SUPPORTED. " +
                        "Actually server returns {0}.", Smb2Status.GetStatusCode(status));
                }
                else
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_INVALID_PARAMETER,
                        status,
                        "Otherwise, the request MUST be failed with STATUS_INVALID_PARAMETER." +
                        "Actually server returns {0}.", Smb2Status.GetStatusCode(status));
                }
            }
            else // FileType.DirectoryFile
            {
                BaseTestSite.Assert.AreEqual(
                    Smb2Status.STATUS_SUCCESS,
                    status,
                    "QUERY_DIRECTORY is expected to success, actually server returns {0}.", Smb2Status.GetStatusCode(status));
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            client1.Close(treeId, fileId);
            client1.TreeDisconnect(treeId);
            client1.LogOff();
        }

        private void QueryQuotaInfo(SidBufferFormat type)
        {
            // MS-SMB2 2.2.37.1
            BaseTestSite.Assert.IsFalse(
                type == SidBufferFormat.SID,
                "Windows-based clients never send a request using the SidBuffer format 2");

            client1 = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start a client to create a file by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE");
            client1.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            uint status = client1.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);
            status = client1.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);

            uint treeId;
            status = client1.TreeConnect(uncSharePath, out treeId);
            Smb2CreateContextResponse[] serverCreateContexts;
            FILEID fileId;
            string fileName = GetTestFileName(uncSharePath);
            status = client1.Create(
                treeId,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                out fileId,
                out serverCreateContexts);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client queries quota information by sending QUERY_INFO request.");
            byte[] inputBuffer = CreateSidBuffer(type);
            byte[] outputBuffer;
            status = client1.QueryFileQuotaInfo(
                treeId,
                QUERY_INFO_Request_Flags_Values.SL_RESTART_SCAN,
                fileId,
                inputBuffer,
                out outputBuffer
                );

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "QUERY_INFO is expected to success, actually server returns {0}.", Smb2Status.GetStatusCode(status));

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF.");
            client1.Close(treeId, fileId);
            client1.TreeDisconnect(treeId);
            client1.LogOff();
        }

        /// <summary>
        /// MS-SMB2 2.2.37.1
        /// Create SidBuffer
        /// </summary>
        /// <param name="type">Indicate the type of element in SidBuffer</param>
        /// <returns></returns>
        private byte[] CreateSidBuffer(SidBufferFormat type)
        {
            QUERY_QUOTA_INFO quotaInfo = new QUERY_QUOTA_INFO();
            // If the application provides a SidList,
            // via one or more FILE_GET_QUOTA_INFORMATION structures linked by NextEntryOffset,
            // they MUST be copied to the beginning of the SidBuffer,
            // SidListLength MUST be set to their length in bytes,
            // StartSidLength SHOULD be set to 0,
            // and StartSidOffset SHOULD be set to 0.

            FileGetQuotaInformation fileGetQuotaInfo = new FileGetQuotaInformation();
            _SID curSid = DtypUtility.GetSidFromAccount(TestConfig.DomainName, testConfig.UserName);
            fileGetQuotaInfo.Sid = curSid;
            fileGetQuotaInfo.SidLength = (uint)TypeMarshal.ToBytes<_SID>(curSid).Length;
            quotaInfo.Buffer = TypeMarshal.ToBytes<FileGetQuotaInformation>(fileGetQuotaInfo);
            quotaInfo.SidListLength = (uint)quotaInfo.Buffer.Length;
            return TypeMarshal.ToBytes<QUERY_QUOTA_INFO>(quotaInfo);
        }
    }
}
