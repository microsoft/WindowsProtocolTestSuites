// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    [TestClass]
    public class MS_FSA_Clarification : SMB2TestBase
    {
        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
        }

        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }

        protected override void TestInitialize()
        {
            base.TestInitialize();
            smb2Functionalclient = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }



        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.HvrsFsa)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("This test case is designed to test whether the server supports the FSCTL_OFFLOAD_READ and FSCTL_OFFLOAD_WRITE.")]
        public void BVT_OffloadReadWrite()
        {
            // Check the flag, if IsOffLoadImplemented == false, skip this case

            CheckHvrsCapability(TestConfig.IsOffLoadImplemented, 
            "If the server doesn't support the FSCTL_OFFLOAD_READ and FSCTL_OFFLOAD_WRITE commands, " +
            "as specified in [MS-FSA] sections 2.1.5.9.18 and 2.1.5.9.19, " +
            "then any small computer system interface (SCSI) ODX commands initiated by the virtual machine operating system fail.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Create a file wiht specified length {0} as for offload copy.", TestConfig.WriteBufferLengthInKb * 1024);
            string content = Smb2Utility.CreateRandomString(TestConfig.WriteBufferLengthInKb);
            uint treeId;
            FILEID fileIdSrc;
            NewTestFile(smb2Functionalclient, content, out treeId, out fileIdSrc);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends IOCTL request with FSCTL_OFFLOAD_READ");
            STORAGE_OFFLOAD_TOKEN token;
            ulong fileOffsetToRead = 0;
            ulong copyLengthToRead = (ulong)TestConfig.WriteBufferLengthInKb * 1024;
            ulong transferLength;

            smb2Functionalclient.OffloadRead(
                treeId, 
                fileIdSrc, 
                fileOffsetToRead, 
                copyLengthToRead, 
                out transferLength, 
                out token,
                checker: (Packet_Header header, IOCTL_Response response) =>
                {
                    BaseTestSite.Log.Add(LogEntryKind.TestStep, "Check IOCTL FSCTL_OFFLOAD_READ response.");
                    if (!TestConfig.IsOffLoadImplemented)
                    {
                        BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_INVALID_DEVICE_REQUEST,
                        header.Status,
                        "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST, The server returns {0}", Smb2Status.GetStatusCode(header.Status));
                    }
                    else if (header.Status == Smb2Status.STATUS_SUCCESS)
                    {
                        BaseTestSite.Log.Add(LogEntryKind.Debug, "the server doesn't support the FSCTL_OFFLOAD_READ.");
                    }
                    else if(header.Status == Smb2Status.STATUS_NOT_SUPPORTED)
                    {
                        BaseTestSite.Log.Add(LogEntryKind.Debug, "If Open.File.Volume.IsOffloadReadSupported is FALSE, the operation MUST be failed with STATUS_NOT_SUPPORTED.");
                    }
                    else if(header.Status == Smb2Status.STATUS_OFFLOAD_READ_FLT_NOT_SUPPORTED)
                    {
                        BaseTestSite.Log.Add(LogEntryKind.Debug, "A file system filter on the server has not opted in for Offload Read support.");
                    }
                    else if (header.Status == Smb2Status.STATUS_OFFLOAD_READ_FILE_NOT_SUPPORTED)
                    {
                        BaseTestSite.Log.Add(LogEntryKind.Debug, "offload read operations cannot be performed on: Compressed files, Sparse files, Encrypted files, File system metadata files.");
                    }
                    else
                    {
                        BaseTestSite.Log.Add(LogEntryKind.Warning, "Unexpected Response: {0}", Smb2Status.GetStatusCode(header.Status));
                    }                   
                });

            // Offload_Read

            BaseTestSite.Log.Add(LogEntryKind.Debug, "Transfer length during OFFLOAD_READ is {0}", transferLength);
            BaseTestSite.Assert.AreEqual(copyLengthToRead, transferLength,
                "Transfer length {0} should be equal to copy length {1}", transferLength, copyLengthToRead);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Create another file as the destination of offload copy.");
            FILEID fileIdDest;
            Smb2CreateContextResponse[] serverCreateContexts;
            smb2Functionalclient.Create(
                treeId,
                GetTestFileName(TestConfig.SharePath),
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdDest,
                out serverCreateContexts);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends IOCTL request with FSCTL_OFFLOAD_WRITE to ask server to copy the content from source to destination.");
            ulong fileOffsetToWrite = 0; //FileOffset should be aligned to logical sector boundary on the volume, e.g. 512 bytes
            ulong copyLengthToWrite = transferLength; //CopyLength should be aligned to logical sector boundary on the volume, e.g. 512 bytes
            ulong transferOffset = 0; //TransferOffset should be aligned to logical sector boundary on the volume, e.g. 512 bytes
            // Request hardware to write a range of file which is represented by the generated token
            // and length/offset to another place (a different file or different offset of the same file)
            smb2Functionalclient.OffloadWrite(
                treeId,
                fileIdDest,
                fileOffsetToWrite,
                copyLengthToWrite,
                transferOffset,
                token,
                checker: (Packet_Header header, IOCTL_Response response) =>
                {
                    BaseTestSite.Log.Add(LogEntryKind.TestStep, "Check IOCTL FSCTL_OFFLOAD_WRITE response.");
                    if(!TestConfig.IsOffLoadImplemented)
                    {
                        BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_INVALID_DEVICE_REQUEST,
                        header.Status,
                        "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST, The server returns {0}", Smb2Status.GetStatusCode(header.Status));
                    }
                    else if (header.Status == Smb2Status.STATUS_SUCCESS)
                    {
                        BaseTestSite.Log.Add(LogEntryKind.Debug, "The server supports the FSCTL_OFFLOAD_WRITE.");
                    }
                    else if (header.Status == Smb2Status.STATUS_NOT_SUPPORTED)
                    {
                        BaseTestSite.Log.Add(LogEntryKind.Debug, "If Open.File.Volume.IsOffloadWriteSupported is FALSE, the operation MUST be failed with STATUS_NOT_SUPPORTED.");
                    }
                    else if(header.Status == Smb2Status.STATUS_OFFLOAD_READ_FLT_NOT_SUPPORTED)
                    {
                        BaseTestSite.Log.Add(LogEntryKind.Debug, "A file system filter on the server has not opted in for Offload Write support.");
                    }
                    else if(header.Status == Smb2Status.STATUS_OFFLOAD_READ_FILE_NOT_SUPPORTED)
                    {
                        BaseTestSite.Log.Add(LogEntryKind.Debug, "Offload write operations cannot be performed on: Compressed files, Sparse files, Encrypted files, File system metadata files.");
                    }
                    else
                    {
                        BaseTestSite.Log.Add(LogEntryKind.Warning, "Unexpected Response: {0}", Smb2Status.GetStatusCode(header.Status));
                    }
                 });

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Compare the content of section 1 with the content of section 2.");
            string readContent;
            // Read the content that was just offload copied
            smb2Functionalclient.Read(
                treeId,
                fileIdDest,
                fileOffsetToWrite,
                (uint)copyLengthToWrite,
                out readContent);

            BaseTestSite.Assert.IsTrue(
                readContent.Equals(content),
                "File content read should equal to original");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF.");
            smb2Functionalclient.Close(treeId, fileIdSrc);
            smb2Functionalclient.Close(treeId, fileIdDest);
            smb2Functionalclient.TreeDisconnect(treeId);
            smb2Functionalclient.LogOff();

        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.HvrsFsa)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("This test case is designed to test whether the server supports the FSCTL_SET_ZERO_DATA.")]
        public void BVT_SetZeroData()
        {
            CheckHvrsCapability(TestConfig.IsSetZeroDataImplemented, 
            "If the server supports the FSCTL_SET_ZERO_DATA command, " +
            "as specified in [MS-FSA] section 2.1.5.9.36, " +
            "then Hyper-V can issue this command to optimize the performance of virtual-disk-creation operations.");
            
            FsCtl_Set_ZeroData_IsZeroDataSupported(
                smb2Functionalclient,
                checker: (Packet_Header header, IOCTL_Response response) =>
                {
                    BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify the server response.");
                    if(!TestConfig.IsSetZeroDataImplemented)
                    {
                        BaseTestSite.Assert.AreEqual(
                            Smb2Status.STATUS_INVALID_DEVICE_REQUEST,
                            header.Status,
                            @"If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST. 
                            The response status is {0}", Smb2Status.GetStatusCode(header.Status));
                    }
                    else if(header.Status == Smb2Status.STATUS_SUCCESS)
                    {
                        BaseTestSite.Log.Add(LogEntryKind.TestStep, "This server supports the FSCTL_SET_ZERO_DATA command, and returns STATUS_SUCCESS.");
                    }
                    else
                    {
                        BaseTestSite.Log.Add(LogEntryKind.Warning, "Unexpected Response: {0}", Smb2Status.GetStatusCode(header.Status));
                    }                 
                });
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.HvrsFsa)]
        [Description("This test case is designed to test whether the server supports the FSCTL_FILE_LEVEL_TRIM")]
        public void BVT_FileLevelTrim()
        {
            uint treeId;
            FILEID fileId;

            PrepareFileForTrimming(out treeId, out fileId);

            FSCTL_FILE_LEVEL_TRIM_RANGE fileLevelTrimRange;
            Random random = new Random();
            uint offset = (uint)random.Next(0, TestConfig.WriteBufferLengthInKb * 1024);
            uint length = (uint)random.Next(0, (int)(TestConfig.WriteBufferLengthInKb * 1024 - offset));
            fileLevelTrimRange.Offset = offset;
            fileLevelTrimRange.Length = length;

            FSCTL_FILE_LEVEL_TRIM_INPUT fileLevelTrimInput;
            fileLevelTrimInput.Key = 0;
            fileLevelTrimInput.NumRanges = 1;
            fileLevelTrimInput.Ranges = new FSCTL_FILE_LEVEL_TRIM_RANGE[] { fileLevelTrimRange };

            byte[] buffer = TypeMarshal.ToBytes<FSCTL_FILE_LEVEL_TRIM_INPUT>(fileLevelTrimInput);
            byte[] respOutput;
            uint status = smb2Functionalclient.FileLevelTrim(
                treeId,
                fileId,
                buffer,
                out respOutput,
                (header, response) => BaseTestSite.Assert.AreEqual(
                    true,
                    header.Status == Smb2Status.STATUS_SUCCESS || header.Status == Smb2Status.STATUS_NO_RANGES_PROCESSED,
                    // The operation was successful, but no range was processed.
                    "{0} should complete with STATUS_SUCCESS or STATUS_NO_RANGES_PROCESSED, actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status)));

            if (status != Smb2Status.STATUS_NO_RANGES_PROCESSED // Skip parsing the response when server returns STATUS_NO_RANGES_PROCESSED
                && respOutput != null) // Skip parsing the response if no output buffer is returned
            {
                FSCTL_FILE_LEVEL_TRIM_OUTPUT fileLevelTrimOutput = TypeMarshal.ToStruct<FSCTL_FILE_LEVEL_TRIM_OUTPUT>(respOutput);
                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Number of ranges that were processed: {0}", fileLevelTrimOutput.NumRangesProcessed);
            }
            else
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "No range was processed during this operation.");
            }

            status = smb2Functionalclient.Close(treeId, fileId);

            status = smb2Functionalclient.TreeDisconnect(treeId);

            status = smb2Functionalclient.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.HvrsFsa)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Test the server response when non-zero value is set to the Key field of FSCTL_FILE_LEVEL_TRIM request.")]
        public void FileLevelTrim_Negative_NonZeroKeyInRequest()
        {
            uint treeId;
            FILEID fileId;

            PrepareFileForTrimming(out treeId, out fileId);

            FSCTL_FILE_LEVEL_TRIM_RANGE fileLevelTrimRange;
            Random random = new Random();
            uint offset = (uint)random.Next(0, TestConfig.WriteBufferLengthInKb * 1024);
            uint length = (uint)random.Next(0, (int)(TestConfig.WriteBufferLengthInKb * 1024 - offset));
            fileLevelTrimRange.Offset = offset;
            fileLevelTrimRange.Length = length;

            FSCTL_FILE_LEVEL_TRIM_INPUT fileLevelTrimInput;
            fileLevelTrimInput.Key = (uint)random.Next(1, int.MaxValue); // Set the Key field a non-zero value
            fileLevelTrimInput.NumRanges = 1;
            fileLevelTrimInput.Ranges = new FSCTL_FILE_LEVEL_TRIM_RANGE[] { fileLevelTrimRange };

            byte[] buffer = TypeMarshal.ToBytes<FSCTL_FILE_LEVEL_TRIM_INPUT>(fileLevelTrimInput);
            byte[] respOutput;
            uint status = smb2Functionalclient.FileLevelTrim(
                treeId,
                fileId,
                buffer,
                out respOutput,
                (header, response) => Assert.AreEqual(
                    Smb2Status.STATUS_INVALID_PARAMETER,
                    header.Status,
                    "If the Key field in FSCTL_FILE_LEVEL_TRIM, as specified in [MS-FSCC] section 2.3.73, is not zero," +
                    " the server MUST fail the request with an error code of STATUS_INVALID_PARAMETER. " +
                    "Actually server returns {0}.", Smb2Status.GetStatusCode(header.Status)));
        }

        private void PrepareFileForTrimming(out uint treeId, out FILEID fileId)
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckIOCTL(CtlCode_Values.FSCTL_FILE_LEVEL_TRIM);
            #endregion

            string uncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            string fileName = GetTestFileName(uncSharePath);
            string contentWrite = Smb2Utility.CreateRandomString(TestConfig.WriteBufferLengthInKb);

            smb2Functionalclient.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);

            uint status = smb2Functionalclient.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                            Smb2Status.STATUS_SUCCESS,
                            header.Status,
                            "CREATE should succeed, actually server returns {0}.", Smb2Status.GetStatusCode(header.Status));

                    TestConfig.CheckNegotiateDialect(DialectRevision.Smb30, response);
                });

            status = smb2Functionalclient.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);

            status = smb2Functionalclient.TreeConnect(uncSharePath, out treeId);

            Smb2CreateContextResponse[] serverCreateContexts;
            status = smb2Functionalclient.Create(
                treeId,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out serverCreateContexts);

            status = smb2Functionalclient.Write(treeId, fileId, contentWrite);

            status = smb2Functionalclient.Close(treeId, fileId);

            status = smb2Functionalclient.Create(
                treeId,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out serverCreateContexts);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.HvrsFsa)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("This test case is designed to test whether the server supports FSCTL_DUPLICATE_EXTENTS_TO_FILE")]
        public void BVT_DuplicateExtentsToFile()
        {
            // Check the platform and ReFS file system.
            // Prepare
            // Create a file
            uint treeId;
            FILEID srcFileId;
            int clusterSize = TestConfig.VolumnClusterSize;
            string content = Smb2Utility.CreateRandomString(clusterSize * 2);
            NewTestFile(smb2Functionalclient, content, out treeId, out srcFileId);
            long sourceFileOffset = 0;
            long targetFileOffset = clusterSize * 1024;
            long byteCount = clusterSize * 1024;

            smb2Functionalclient.DuplicateExtentsToFile(
                treeId,
                srcFileId,
                sourceFileOffset,
                targetFileOffset,
                byteCount,
                checker: (Packet_Header header, IOCTL_Response response) =>
                {

                    if (header.Status == Smb2Status.STATUS_INVALID_DEVICE_REQUEST)
                    {
                        // check if the server advertises the FILE_SUPPROTS_BLOCK_REFCOUNTING flag for a given open.
                        bool res = FSInfo_Query_FileFsAttributeInformation_IsSupported(smb2Functionalclient, treeId, srcFileId, FileSystemAttributes_Values.FILE_SUPPORTS_BLOCK_REFCOUNTING);
                        BaseTestSite.Assert.AreEqual(false, res, 
                        "If the server adverties the FILE_SUPPORTS_BLOCK_REFCOUNTING flag for a given Open, " +
                        "the server must support the FSCTL_DUPLICATE_EXTENTS_TO_FILE command, " +
                        "as specified in [MS-FSA] section 2.1.5.9.4.");

                    }
                    else if (header.Status == Smb2Status.STATUS_SUCCESS)
                    {
                        BaseTestSite.Log.Add(LogEntryKind.Debug, "The server supports FSCTL_DUPLICATE_EXTENTS_TO_FILE, and response with STATUS_SUCCESS.");
                    }
                    else if (header.Status == Smb2Status.STATUS_MEDIA_WRITE_PROTECTED)
                    {
                        BaseTestSite.Log.Add(LogEntryKind.Debug, "If Open.File.Volume.IsReadOnly is TRUE, the operation MUST be failed with STATUS_MEDIA_WRITE_PROTECTED.");
                    }
                    else
                    {
                        BaseTestSite.Log.Add(LogEntryKind.Warning, "Unexpected Response: {0}", Smb2Status.GetStatusCode(header.Status));
                    }
                });

            // Check the file content.

            // Clean.
        }


        private void NewTestFile(Smb2FunctionalClient client, string content, out uint treeId, out FILEID fileId)
        {
            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.ShareServerName, TestConfig.ShareServerIP);
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
                TestConfig.ShareServerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);

            client.TreeConnect(TestConfig.SharePath, out treeId);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client writes to the file.");
            Smb2CreateContextResponse[] serverCreateContexts;
            client.Create(
                treeId,
                GetTestFileName(TestConfig.SharePath),
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out serverCreateContexts);

            client.Write(treeId, fileId, content);

            // Flush written content to backend storage to avoid cache.
            client.Flush(treeId, fileId);
        }


        private void FsCtl_Set_ZeroData_IsZeroDataSupported(Smb2FunctionalClient client, FileType fileType = FileType.DataFile, ResponseChecker<IOCTL_Response> checker = null)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Create {0}", fileType.ToString());
            string content = string.Empty;
            uint treeId;
            FILEID fileId;
            NewTestFile(smb2Functionalclient, content, out treeId, out fileId);

            // FSCTL request with FSCTL_SET_ZERO_DATA
            ulong fileOffset = 0;
            ulong beyondFinalZero = 0;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send FSCTL request with FSCTL_SET_ZERO_DATA");
            client.SetZeroData(
                treeId,
                fileId,
                fileOffset,
                beyondFinalZero,
                checker);
        }

        private bool FSInfo_Query_FileFsAttributeInformation_IsSupported(Smb2FunctionalClient client, uint treeId, FILEID fileId, FileSystemAttributes_Values attribute)
        {
            FileFsAttributeInformation fsAttributeInfo = new FileFsAttributeInformation();
            byte[] buffer;

            client.QueryFSAttributes(
                treeId,
                (byte)FileSystemInformationClasses.FileFsAttributeInformation,
                fileId,
                out buffer,
                checker: (Packet_Header header, QUERY_INFO_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, header.Status,
                        "Query File system should return STATUS_SUCCESS, the server returns with {0}", Smb2Status.GetStatusCode(header.Status));
                }
                );

            fsAttributeInfo = TypeMarshal.ToStruct<FileFsAttributeInformation>(buffer);
            return fsAttributeInfo.FileSystemAttributes.HasFlag(attribute);
        }

        private void CheckHvrsCapability(bool flag, string statement)
        {
            if (!flag)
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, statement);
                Site.Assert.Inconclusive("Test case is not applicable in this server.");
            }
        }
    }

    /// <summary>
    /// File type defined in [MS-FSA] 3.1.1.3
    /// file type
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// data
        /// </summary>
        DataFile,

        /// <summary>
        /// directory
        /// </summary>
        DirectoryFile
    };
}
