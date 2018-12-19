// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    [TestClass]
    public class CopyOffload : SMB2TestBase
    {
        #region Variables

        private Smb2FunctionalClient client;

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

            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckIOCTL(CtlCode_Values.FSCTL_OFFLOAD_READ, CtlCode_Values.FSCTL_OFFLOAD_WRITE);
            #endregion

            client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
        }

        protected override void TestCleanup()
        {
            client.Disconnect();
            base.TestCleanup();
        }

        #endregion

        #region Test Case

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.FsctlOffloadReadWrite)]
        [Description("This test case is designed to test whether server can handle offload copy correctly when copy content between two files.")]
        public void BVT_CopyOffload()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "1. Create a file with specified length {0} as the source of offload copy.",
                TestConfig.WriteBufferLengthInKb*1024);
            string content = Smb2Utility.CreateRandomString(TestConfig.WriteBufferLengthInKb);
            uint treeId;
            FILEID fileIdSrc;
            PrepareTestFile(content, out treeId, out fileIdSrc);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "2. Client sends IOCTL request with FSCTL_OFFLOAD_READ to ask server to generate the token of the content for offload copy.");
            STORAGE_OFFLOAD_TOKEN token;
            ulong fileOffsetToRead = 0; //FileOffset should be aligned to logical sector boundary on the volume, e.g. 512 bytes
            ulong copyLengthToRead = (ulong)TestConfig.WriteBufferLengthInKb * 1024; //CopyLength should be aligned to logical sector boundary on the volume, e.g. 512 bytes
            ulong transferLength;
            // Request hardware to generate a token that represents a range of file to be copied
            client.OffloadRead(
                treeId,
                fileIdSrc,
                fileOffsetToRead,
                copyLengthToRead,
                out transferLength,
                out token);

            BaseTestSite.Log.Add(LogEntryKind.Debug, "Transfer length during OFFLOAD_READ is {0}", transferLength);
            BaseTestSite.Assert.AreEqual(copyLengthToRead, transferLength,
                "Transfer length {0} should be equal to copy length {1}", transferLength, copyLengthToRead);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Create another file as the destination of offload copy.");
            FILEID fileIdDest;
            Smb2CreateContextResponse[] serverCreateContexts;
            client.Create(
                treeId,
                GetTestFileName(Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare)),
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdDest,
                out serverCreateContexts);

            // The destination file of CopyOffload Write should be equal to or larger than the size of original file
            client.Write(treeId, fileIdDest, Smb2Utility.CreateRandomString(TestConfig.WriteBufferLengthInKb));
            client.Flush(treeId, fileIdDest);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Client sends IOCTL request with FSCTL_OFFLOAD_WRITE to ask server to copy the content from source to destination.");
            ulong fileOffsetToWrite = 0; //FileOffset should be aligned to logical sector boundary on the volume, e.g. 512 bytes
            ulong copyLengthToWrite = transferLength; //CopyLength should be aligned to logical sector boundary on the volume, e.g. 512 bytes
            ulong transferOffset = 0; //TransferOffset should be aligned to logical sector boundary on the volume, e.g. 512 bytes
            // Request hardware to write a range of file which is represented by the generated token
            // and length/offset to another place (a different file or different offset of the same file)
            client.OffloadWrite(
                treeId,
                fileIdDest,
                fileOffsetToWrite,
                copyLengthToWrite,
                transferOffset,
                token);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "5. Compare the content of section 1 with the content of section 2.");
            string readContent;
            // Read the content that was just offload copied
            client.Read(
                treeId,
                fileIdDest,
                fileOffsetToWrite,
                (uint)copyLengthToWrite,
                out readContent);

            BaseTestSite.Assert.IsTrue(
                readContent.Equals(content),
                "File content read should equal to original");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "6. Tear down the client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF.");
            client.Close(treeId, fileIdSrc);
            client.Close(treeId, fileIdDest);
            client.TreeDisconnect(treeId);
            client.LogOff();

        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.FsctlOffloadReadWrite)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server can handle offload copy correctly when copy content in the same file.")]
        public void CopyOffload_CopyContentWithinSameFile()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "1. Create a file with specified length {0} as for offload copy.",
                TestConfig.WriteBufferLengthInKb * 1024);
            string content = Smb2Utility.CreateRandomString(TestConfig.WriteBufferLengthInKb);
            uint treeId;
            FILEID fileId;
            PrepareTestFile(content, out treeId, out fileId);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "2. Client sends IOCTL request with FSCTL_OFFLOAD_READ to ask server to generate the token of the 1st half of file content in the file for offload copy.");
            STORAGE_OFFLOAD_TOKEN token;
            ulong fileOffsetToRead = 0; //FileOffset should be aligned to logical sector boundary on the volume, e.g. 512 bytes
            ulong copyLengthToRead = (ulong)TestConfig.WriteBufferLengthInKb / 2 * 1024; //CopyLength should be aligned to logical sector boundary on the volume, e.g. 512 bytes
            ulong transferLength;
            // Request hardware to generate a token that represents a range of file to be copied
            client.OffloadRead(
                treeId,
                fileId,
                fileOffsetToRead,
                copyLengthToRead,
                out transferLength,
                out token);

            BaseTestSite.Log.Add(LogEntryKind.Debug, "Transfer length during OFFLOAD_READ is {0}", transferLength);
            BaseTestSite.Assert.AreEqual(copyLengthToRead, transferLength,
                "Transfer length {0} should be equal to copy length {1}", transferLength, copyLengthToRead);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Client sends IOCTL request with FSCTL_OFFLOAD_WRITE to ask server to copy the content to 2nd half in the file.");
            ulong fileOffsetToWrite = (ulong)TestConfig.WriteBufferLengthInKb / 2 * 1024; //FileOffset should be aligned to logical sector boundary on the volume, e.g. 512 bytes
            ulong copyLengthToWrite = transferLength; //CopyLength should be aligned to logical sector boundary on the volume, e.g. 512 bytes
            ulong transferOffset = 0; //TransferOffset should be aligned to logical sector boundary on the volume, e.g. 512 bytes
            // Request hardware to write a range of file which is represented by the generated token and length/offset to another place (a different file or different offset of the same file)
            client.OffloadWrite(
                treeId,
                fileId,
                fileOffsetToWrite,
                copyLengthToWrite,
                transferOffset,
                token);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Compare the content of section 1 with the content of section 2.");
            string readContent;
            // Read the content that was just offload copied
            client.Read(
                treeId,
                fileId,
                fileOffsetToWrite,
                (uint)copyLengthToWrite,
                out readContent);

            BaseTestSite.Assert.IsTrue(
                readContent.Equals(content.Substring(0, TestConfig.WriteBufferLengthInKb / 2 * 1024)),
                "File content read should equal to original");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "5. Tear down the client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF.");
            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        #endregion

        private void PrepareTestFile(string content, out uint treeId, out FILEID fileId)
        {
            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start the client by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT");
            client.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "NEGOTIATE should succeed.");

                    TestConfig.CheckNegotiateDialect(DialectRevision.Smb30, response);
                });

            client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);

            string uncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            client.TreeConnect(uncSharePath, out treeId);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client writes to the file.");
            Smb2CreateContextResponse[] serverCreateContexts;
            client.Create(
                treeId,
                GetTestFileName(uncSharePath),
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out serverCreateContexts);

            client.Write(treeId, fileId, content);

            // Flush written content to backend storage to avoid cache.
            client.Flush(treeId, fileId);
        }

    }
}
