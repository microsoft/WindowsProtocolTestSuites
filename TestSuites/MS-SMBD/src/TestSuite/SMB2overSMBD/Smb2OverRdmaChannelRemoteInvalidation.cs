// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smbd;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace Microsoft.Protocol.TestSuites.Smbd.TestSuite
{
    [TestClass]
    public class Smb2OverRdmaChannelRemoteInvalidation : Smb2OverSmbdTestBase
    {
        #region Fields
        private DialectRevision[] Smb302AboveDialects = new DialectRevision[] { DialectRevision.Smb302, DialectRevision.Smb311 };
        private DialectRevision[] Smb300OnlyDialects = new DialectRevision[] { DialectRevision.Smb30 };
        private string fileName;
        #endregion

        #region Class Initialization and Cleanup
        [ClassInitializeAttribute()]
        public static void ClassInitialize(TestContext context)
        {
            TestClassBase.Initialize(context, "MS-SMBD_ServerTestSuite");
        }

        [ClassCleanupAttribute()]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }
        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            base.TestInitialize();
            fileName = CreateRandomFileName();
        }
        #endregion

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("Smb2OverRdmaChannelInvalidate")]
        public void BVT_Smb2OverRdma_Smb302_Write_SMB2_CHANNEL_RDMA_V1_INVALIDATE()
        {
            EstablishConnectionAndOpenFile(fileName, Smb302AboveDialects);

            uint writeSize = smbdAdapter.Smb2MaxWriteSize;
            byte[] fileContent = Smb2Utility.CreateRandomByteArray((int)writeSize);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Register memory and get buffer descriptor for SMB2 WRITE");
            SmbdBufferDescriptorV1 descp;
            NtStatus status = smbdAdapter.SmbdRegisterBuffer(
                writeSize,
                SmbdBufferReadWrite.RDMA_READ_PERMISSION_FOR_WRITE_FILE,
                out descp);
            BaseTestSite.Assert.AreEqual(
                NtStatus.STATUS_SUCCESS,
                status,
                "Register buffer should succeed.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Write content to file over RDMA.");
            status = Smb2WriteOverRdma(fileName, fileContent, Channel_Values.CHANNEL_RDMA_V1_INVALIDATE, descp);
            BaseTestSite.Assert.AreEqual<NtStatus>(
                NtStatus.STATUS_SUCCESS,
                status,
                "SMB2 WRITE over RDMA should succeed.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify file content.");
            ValidateFileContent(fileContent);

            try
            {
                fileContent = Encoding.ASCII.GetBytes(Smb2Utility.CreateRandomStringInByte((int)writeSize));

                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send Smb2 WRITE request using same descriptor which should be invalidated.");
                status = Smb2WriteOverRdma(fileName, fileContent, Channel_Values.CHANNEL_RDMA_V1_INVALIDATE, descp);
            }
            catch
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Verify connection is terminated .");
                smbdAdapter.WaitRdmaDisconnect();
            }

            BaseTestSite.Assert.IsFalse(smbdAdapter.ClientConnection.Endpoint.IsConnected, "Connection should be terminated when accessing a memory window which is already invalidated.");
        }

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("Smb2OverRdmaChannelInvalidate")]
        public void BVT_Smb2OverRdma_Smb302_Read_SMB2_CHANNEL_RDMA_V1_INVALIDATE()
        {
            EstablishConnectionAndOpenFile(fileName, Smb302AboveDialects);

            uint fileSize = smbdAdapter.Smb2MaxReadSize;
            byte[] fileContent = Smb2Utility.CreateRandomByteArray((int)fileSize);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Register memory and get buffer descriptor for SMB2 WRITE");
            SmbdBufferDescriptorV1 descp;
            NtStatus status = smbdAdapter.SmbdRegisterBuffer(
                fileSize,
                SmbdBufferReadWrite.RDMA_READ_PERMISSION_FOR_WRITE_FILE,
                out descp);
            BaseTestSite.Assert.AreEqual(
                NtStatus.STATUS_SUCCESS,
                status,
                "Register buffer should succeed.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Write content to file over RDMA.");
            status = Smb2WriteOverRdma(fileName, fileContent, Channel_Values.CHANNEL_RDMA_V1_INVALIDATE, descp);
            BaseTestSite.Assert.AreEqual<NtStatus>(
                NtStatus.STATUS_SUCCESS,
                status,
                "SMB2 WRITE over RDMA should succeed.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Register memory and get buffer descriptor for SMB2 READ.");
            status = smbdAdapter.SmbdRegisterBuffer(
                fileSize,
                SmbdBufferReadWrite.RDMA_WRITE_PERMISSION_FOR_READ_FILE,
                out descp);
            BaseTestSite.Assert.AreEqual(
                NtStatus.STATUS_SUCCESS,
                status,
                "Register buffer should succeed.");
            byte[] channelInfo = TypeMarshal.ToBytes<SmbdBufferDescriptorV1>(descp);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Read content from file over RDMA.");
            READ_Response readResponse;
            byte[] readData;
            status = (NtStatus)smbdAdapter.Smb2ReadOverRdmaChannel(
                0,
                (uint)fileSize,
                channelInfo,
                out readResponse,
                out readData,
                Channel_Values.CHANNEL_RDMA_V1_INVALIDATE);

            BaseTestSite.Assert.AreEqual<NtStatus>(
                NtStatus.STATUS_SUCCESS,
                status,
                "SMB2 READ over RDMA should succeed.");

            byte[] readContent = new byte[fileSize];
            smbdAdapter.SmbdReadRegisteredBuffer(readContent, descp);

            BaseTestSite.Assert.IsTrue(
                SmbdUtilities.CompareByteArray(fileContent, readContent),
                "Content read should be identical with original on server.");

            try
            {
                status = (NtStatus)smbdAdapter.Smb2ReadOverRdmaChannel(
                    0,
                    (uint)fileSize,
                    channelInfo,
                    out readResponse,
                    out readData,
                    Channel_Values.CHANNEL_RDMA_V1_INVALIDATE);
            }
            catch
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Wait for connection to be terminated ");
                smbdAdapter.WaitRdmaDisconnect();
            }

            BaseTestSite.Assert.IsFalse(smbdAdapter.ClientConnection.Endpoint.IsConnected, "Connection should be terminated when accessing a memory window which is already invalidated.");
        }

        [TestMethod]
        [TestCategory("Smb2OverRdmaChannel")]
        public void Smb2OverRdma_Write_SMB2_CHANNEL_RDMA_V1()
        {
            EstablishConnectionAndOpenFile(fileName);

            uint writeSize = smbdAdapter.Smb2MaxWriteSize;
            byte[] fileContent = Smb2Utility.CreateRandomByteArray((int)writeSize);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Register memory and get buffer descriptor for SMB2 WRITE");
            SmbdBufferDescriptorV1 descp;
            NtStatus status = smbdAdapter.SmbdRegisterBuffer(
                writeSize,
                SmbdBufferReadWrite.RDMA_READ_PERMISSION_FOR_WRITE_FILE,
                out descp);
            BaseTestSite.Assert.AreEqual(
                NtStatus.STATUS_SUCCESS,
                status,
                "Register buffer should succeed.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Write content to file over RDMA.");
            status = Smb2WriteOverRdma(fileName, fileContent, Channel_Values.CHANNEL_RDMA_V1, descp);
            BaseTestSite.Assert.AreEqual<NtStatus>(
                NtStatus.STATUS_SUCCESS,
                status,
                "SMB2 WRITE over RDMA should succeed.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify file content.");
            ValidateFileContent(fileContent);

            fileContent = Encoding.ASCII.GetBytes(Smb2Utility.CreateRandomStringInByte((int)writeSize));

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send Smb2 WRITE request using same descriptor.");
            status = Smb2WriteOverRdma(fileName, fileContent, Channel_Values.CHANNEL_RDMA_V1, descp);

            BaseTestSite.Assert.AreEqual<NtStatus>(
                NtStatus.STATUS_SUCCESS,
                status,
                "SMB2 WRITE over RDMA should succeed.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify file content.");
            ValidateFileContent(fileContent);
        }

        [TestMethod]
        [TestCategory("Smb2OverRdmaChannel")]
        public void Smb2OverRdma_Read_SMB2_CHANNEL_RDMA_V1()
        {
            EstablishConnectionAndOpenFile(fileName);

            uint fileSize = smbdAdapter.Smb2MaxReadSize;
            byte[] fileContent = Smb2Utility.CreateRandomByteArray((int)fileSize);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Register memory and get buffer descriptor for SMB2 WRITE");
            SmbdBufferDescriptorV1 descp;
            NtStatus status = smbdAdapter.SmbdRegisterBuffer(
                fileSize,
                SmbdBufferReadWrite.RDMA_READ_PERMISSION_FOR_WRITE_FILE,
                out descp);
            BaseTestSite.Assert.AreEqual(
                NtStatus.STATUS_SUCCESS,
                status,
                "Register buffer should succeed.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Write content to file over RDMA.");
            status = Smb2WriteOverRdma(fileName, fileContent, Channel_Values.CHANNEL_RDMA_V1, descp);
            BaseTestSite.Assert.AreEqual<NtStatus>(
                NtStatus.STATUS_SUCCESS,
                status,
                "SMB2 WRITE over RDMA should succeed.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Register memory and get buffer descriptor for SMB2 READ.");
            status = smbdAdapter.SmbdRegisterBuffer(
                fileSize,
                SmbdBufferReadWrite.RDMA_WRITE_PERMISSION_FOR_READ_FILE,
                out descp);
            BaseTestSite.Assert.AreEqual(
                NtStatus.STATUS_SUCCESS,
                status,
                "Register buffer should succeed.");

            byte[] channelInfo = TypeMarshal.ToBytes<SmbdBufferDescriptorV1>(descp);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Read content from file over RDMA.");
            READ_Response readResponse;
            byte[] readData;
            status = (NtStatus)smbdAdapter.Smb2ReadOverRdmaChannel(
                0,
                (uint)fileSize,
                channelInfo,
                out readResponse,
                out readData,
                Channel_Values.CHANNEL_RDMA_V1);

            BaseTestSite.Assert.AreEqual<NtStatus>(
                NtStatus.STATUS_SUCCESS,
                status,
                "SMB2 READ over RDMA should succeed.");

            byte[] readContent = new byte[fileSize];
            smbdAdapter.SmbdReadRegisteredBuffer(readContent, descp);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify the read content same as the original one on server.");
            BaseTestSite.Assert.IsTrue(
                SmbdUtilities.CompareByteArray(fileContent, readContent),
                "Content read should be identical with original on server.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send SMB2 READ using same descriptor.");
            status = (NtStatus)smbdAdapter.Smb2ReadOverRdmaChannel(
                0,
                (uint)fileSize,
                channelInfo,
                out readResponse,
                out readData,
                Channel_Values.CHANNEL_RDMA_V1);

            BaseTestSite.Assert.AreEqual<NtStatus>(
                NtStatus.STATUS_SUCCESS,
                status,
                "SMB2 READ over RDMA should succeed.");

            smbdAdapter.SmbdReadRegisteredBuffer(readContent, descp);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify the read content same as the original one on server.");
            BaseTestSite.Assert.IsTrue(
                SmbdUtilities.CompareByteArray(fileContent, readContent),
                "Content read should be identical with original on server.");
        }

        [TestMethod]
        [TestCategory("Negative")]
        [TestCategory("Smb2OverRdmaChannelInvalidate")]
        public void Smb2OverRdma_Smb300_Write_SMB2_CHANNEL_RDMA_V1_INVALIDATE()
        {
            EstablishConnectionAndOpenFile(fileName, Smb300OnlyDialects);

            uint writeSize = smbdAdapter.Smb2MaxWriteSize;
            byte[] fileContent = Smb2Utility.CreateRandomByteArray((int)writeSize);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Register memory and get buffer descriptor for SMB2 WRITE");
            SmbdBufferDescriptorV1 descp;
            NtStatus status = smbdAdapter.SmbdRegisterBuffer(
                writeSize,
                SmbdBufferReadWrite.RDMA_READ_PERMISSION_FOR_WRITE_FILE,
                out descp);
            BaseTestSite.Assert.AreEqual(
                NtStatus.STATUS_SUCCESS,
                status,
                "Register buffer should succeed.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Write content to file over RDMA.");
            status = Smb2WriteOverRdma(fileName, fileContent, Channel_Values.CHANNEL_RDMA_V1_INVALIDATE, descp);

            BaseTestSite.Assert.AreEqual<NtStatus>(
                NtStatus.STATUS_INVALID_PARAMETER,
                status,
                "SMB2 WRITE over RDMA should fail with STATUS_INVALID_PARAMETER if set Channel CHANNEL_RDMA_V1_INVALIDATE on SMB 3.0 dialect.");
        }

        [TestMethod]
        [TestCategory("Negative")]
        [TestCategory("Smb2OverRdmaChannelInvalidate")]
        public void Smb2OverRdma_Smb300_Read_SMB2_CHANNEL_RDMA_V1_INVALIDATE()
        {
            EstablishConnectionAndOpenFile(fileName, Smb300OnlyDialects);

            uint fileSize = smbdAdapter.Smb2MaxReadSize;
            byte[] fileContent = Smb2Utility.CreateRandomByteArray((int)fileSize);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Register memory and get buffer descriptor for SMB2 WRITE");
            SmbdBufferDescriptorV1 descp;
            NtStatus status = smbdAdapter.SmbdRegisterBuffer(
                fileSize,
                SmbdBufferReadWrite.RDMA_READ_PERMISSION_FOR_WRITE_FILE,
                out descp);
            BaseTestSite.Assert.AreEqual(
                NtStatus.STATUS_SUCCESS,
                status,
                "Register buffer should succeed.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Write content to file over RDMA.");
            status = Smb2WriteOverRdma(fileName, fileContent, Channel_Values.CHANNEL_RDMA_V1, descp);
            BaseTestSite.Assert.AreEqual<NtStatus>(
                NtStatus.STATUS_SUCCESS,
                status,
                "SMB2 WRITE over RDMA should succeed.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Register memory and get buffer descriptor for SMB2 READ.");
            smbdAdapter.SmbdRegisterBuffer(
                fileSize,
                SmbdBufferReadWrite.RDMA_WRITE_PERMISSION_FOR_READ_FILE,
                out descp);
            byte[] channelInfo = TypeMarshal.ToBytes<SmbdBufferDescriptorV1>(descp);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Read content from file over RDMA.");
            READ_Response readResponse;
            byte[] readData;
            status = (NtStatus)smbdAdapter.Smb2ReadOverRdmaChannel(
                0,
                (uint)fileSize,
                channelInfo,
                out readResponse,
                out readData,
                Channel_Values.CHANNEL_RDMA_V1_INVALIDATE);

            BaseTestSite.Assert.AreEqual<NtStatus>(
                NtStatus.STATUS_INVALID_PARAMETER,
                status,
                "SMB2 READ over RDMA should fail with STATUS_INVALID_PARAMETER if set Channel CHANNEL_RDMA_V1_INVALIDATE on SMB 3.0 dialect.");
        }

        protected NtStatus Smb2WriteOverRdma(string fileName, byte[] content, Channel_Values channel, SmbdBufferDescriptorV1 descp)
        {
            smbdAdapter.SmbdWriteRegisteredBuffer(content, descp);

            byte[] channelInfo = TypeMarshal.ToBytes<SmbdBufferDescriptorV1>(descp);

            WRITE_Response writeResponse;
            return (NtStatus)smbdAdapter.Smb2WriteOverRdmaChannel(
                0,
                channelInfo,
                (uint)content.Length,
                out writeResponse,
                channel);
        }

    }
}
