// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.Smbd.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smbd;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Text;

namespace Microsoft.Protocol.TestSuites.Smbd.TestSuite
{
    [TestClass]
    public class Smb2OverRdmaChannelTransformSigning : Smb2OverSmbdTestBase
    {
        #region Variables
        // Length of Smb2 Packet Header + Smb2 READ Response before Buffer
        private const int EXPECTED_READ_DATA_OFFSET = 80;
        private const int AES_CMAC_NONCE_LENGTH = 0;
        private const int AES_GMAC_NONCE_LENGTH = 12;
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

        #region Test Cases

        [TestMethod()]
        [TestCategory("Positive")]
        [TestCategory("Signing")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can read file with large size over RDMA channel with AES_GMAC signing")]
        public void Smb2OverRdmaChannelTransform_Signing_ReadLargeFile_AES_GMAC()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_SIGNING };
            ReadOverRdma_Signing(
                smb2RdmaTransformIds: smb2RdmaTransformIds,
                signingAlgorithm: SigningAlgorithm.AES_GMAC);
        }

        [TestMethod()]
        [TestCategory("Positive")]
        [TestCategory("Signing")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can read file with large size over RDMA channel with AES_CMAC signing")]
        public void Smb2OverRdmaChannelTransform_Signing_ReadLargeFile_AES_CMAC()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_SIGNING };
            ReadOverRdma_Signing(
                smb2RdmaTransformIds: smb2RdmaTransformIds,
                signingAlgorithm: SigningAlgorithm.AES_CMAC);
        }

        [TestMethod()]
        [TestCategory("Positive")]
        [TestCategory("Signing")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can write file with large size over RDMA channel with AES_GMAC signing")]
        public void Smb2OverRdmaChannelTransform_Signing_WriteLargeFile_AES_GMAC()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_SIGNING };
            WriteOverRdma_Signing(
                smb2RdmaTransformIds: smb2RdmaTransformIds,
                signingAlgorithm: SigningAlgorithm.AES_GMAC);
        }

        [TestMethod()]
        [TestCategory("Positive")]
        [TestCategory("Signing")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can write file with large size over RDMA channel with AES_CMAC signing")]
        public void Smb2OverRdmaChannelTransform_Signing_WriteLargeFile_AES_CMAC()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_SIGNING };
            WriteOverRdma_Signing(
                smb2RdmaTransformIds: smb2RdmaTransformIds,
                signingAlgorithm: SigningAlgorithm.AES_CMAC);
        }

        [TestMethod()]
        [TestCategory("Smb2OverRdmaChannel")]
        [TestCategory("Signing")]
        [Description("Verify SMB2 write fails over RDMA channel when Session isn't setup with Signing and the Data is signed")]
        public void Smb2OverRdmaChannelTransform_Signing_WriteLargeFile_FlagsSignedNotSet_Fails()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_SIGNING };
            WriteOverRdma_Signing(
                smb2RdmaTransformIds: smb2RdmaTransformIds,
                enableSigning: false,
                enableEncryption: true);
        }

        [TestMethod()]
        [TestCategory("Smb2OverRdmaChannel")]
        [TestCategory("Signing")]
        [Description("Verify SMB2 write fails over RDMA channel when Session is setup with SMB2_RDMA_TRANSFORM_ENCRYPTION but SMB2_RDMA_TRANSFORM_TYPE_SIGNING is the selected TransformType")]
        public void Smb2OverRdmaChannelTransform_Signing_WriteLargeFile_SigningStructure_EncryptionId_Fails()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_ENCRYPTION };
            WriteOverRdma_Signing(
                smb2RdmaTransformIds: smb2RdmaTransformIds,
                enableEncryption: true);
        }

        [TestMethod()]
        [TestCategory("Smb2OverRdmaChannel")]
        [TestCategory("Signing")]
        [Description("Verify SMB2 write fails over RDMA channel when an invalid signature is sent")]
        public void Smb2OverRdmaChannelTransform_Signing_WriteLargeFile_InvalidSignature_Fails()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_SIGNING };
            WriteOverRdma_Signing(
                smb2RdmaTransformIds: smb2RdmaTransformIds,
                signingAlgorithm: SigningAlgorithm.AES_CMAC,
                setInvalidSignature: true);
        }

        [TestMethod()]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 write fails over RDMA channel when no RDMATransformId is created with the Connection")]
        public void Smb2OverRdmaChannelTransform_Signing_WriteLargeFile_NoTransformIds_Fails()
        {
            WriteOverRdma_Signing();
        }

        [TestMethod()]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 write fails over RDMA channel when TransformCount is set to 0")]
        public void Smb2OverRdmaChannelTransform_Signing_WriteLargeFile_ZeroTransformCount_Fails()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_SIGNING };
            WriteOverRdma_Signing(
                smb2RdmaTransformIds: smb2RdmaTransformIds,
                signingAlgorithm: SigningAlgorithm.AES_CMAC,
                setZeroTransformCount: true);
        }

        #endregion

        #region Common Methods

        /// <summary>
        /// Read file content over RDMA. The file content will be read with specific number of operations. 
        /// The total size of the read content is SMB2 negotiated MaxReadSize, so content size 
        /// in each SMB2 READ request is ( MaxReadSize / operationCount )
        /// 
        /// </summary>
        /// <param name="operationCount">Count of SMB2 READ operations.</param>
        /// <param name="enableEncryption">Enable encryption in the Smb2 Connection</param>
        /// <param name="enableSigning">Enable signing in the Smb2 connection</param>
        /// <param name="signingAlgorithm">The signing algorithm to use for the Smb2 connection</param>
        /// <param name="smb2RdmaTransformIds">The RDMA_TRANSFORM_IDs to use for NegotiateContext</param>
        public void ReadOverRdma_Signing(
            uint operationCount = 1,
            Smb2RDMATransformId[] smb2RdmaTransformIds = null,
            SigningAlgorithm signingAlgorithm = SigningAlgorithm.AES_GMAC,
            bool enableSigning = true,
            bool enableEncryption = false)
        {

            SigningAlgorithm[] signingAlgorithms = new SigningAlgorithm[]
            {
                signingAlgorithm
            };

            InitSmbdConnectionForTestCases(
                fileName: smbdAdapter.TestConfig.TestFileName_LargeFile,
                smb2RdmaTransformIds: smb2RdmaTransformIds,
                addDefaultEncryption: enableEncryption,
                signingAlgorithms: signingAlgorithms,
                enableSigning: enableSigning,
                enableEncryption: enableEncryption);

            uint readSize = smbdAdapter.Smb2MaxReadSize / operationCount;
            BaseTestSite.Log.Add(LogEntryKind.Debug, "ReadSize is {0}", readSize);
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Read operation count: {0}", operationCount);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send each read request according to SMB2 read file limit.");
            for (int i = 0; i < operationCount; ++i)
            {
                // SMB2 Read file
                SmbdBufferDescriptorV1 descp;
                smbdAdapter.SmbdRegisterBuffer(
                    readSize,
                    SmbdBufferReadWrite.RDMA_WRITE_PERMISSION_FOR_READ_FILE,
                    out descp);
                byte[] channelInfo = TypeMarshal.ToBytes(descp);

                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Read content from file over RDMA.");
                READ_Response readResponse;
                byte[] readData;
                NtStatus status = (NtStatus)smbdAdapter.Smb2ReadOverRdmaChannel(
                    (UInt64)i * readSize,
                    (uint)readSize,
                    channelInfo,
                    out readResponse,
                    out readData
                    );
                BaseTestSite.Assert.AreEqual<NtStatus>(
                    NtStatus.STATUS_SUCCESS,
                    status,
                    "Status of SMB2 Read File is {0}", status);
                BaseTestSite.Assert.AreEqual<uint>(
                    (uint)readSize,
                    readResponse.DataRemaining,
                    "DataRemaining in READ response is {0}", readResponse.DataRemaining);

                BaseTestSite.Assert.AreEqual<byte>(
                    EXPECTED_READ_DATA_OFFSET,
                    readResponse.DataOffset,
                    "DataOffset in response is {0}", readResponse.DataOffset);

                var rdmaTransformDataLength = 16;  // sizeof(SMB2_RDMA_TRANSFORM)
                var expectedDataLength = signingAlgorithm == SigningAlgorithm.AES_GMAC ? 56 : 40;  // sizeof(SMB2_RDMA_CRYPTO_TRANSFORM) with/without Nonce
                var cryptoTransformDataLength = expectedDataLength - rdmaTransformDataLength;

                BaseTestSite.Assert.AreEqual<int>(
                    expectedDataLength,
                    readData.Length,
                    "Data length of content in response is {0}", readData.Length);

                byte[] rdmaTransformBytes = new byte[rdmaTransformDataLength];
                Array.Copy(readData, 0, rdmaTransformBytes, 0, rdmaTransformDataLength);
                var rdmaTransform = TypeMarshal.ToStruct<SMB2_RDMA_TRANSFORM>(rdmaTransformBytes);
                BaseTestSite.Assert.AreEqual<int>(
                    1,
                    rdmaTransform.TransformCount,
                    "SMB2_RDMA_TRANSFORM TransformCount is {0}", rdmaTransform.TransformCount);

                byte[] cryptoTransformBytes = new byte[cryptoTransformDataLength];
                Array.Copy(readData, rdmaTransformBytes.Length, cryptoTransformBytes, 0, cryptoTransformDataLength);
                var cryptoTransform = TypeMarshal.ToStruct<SMB2_RDMA_CRYPTO_TRANSFORM>(cryptoTransformBytes);
                BaseTestSite.Assert.AreEqual<Smb2RDMATransformType>(
                    Smb2RDMATransformType.SMB2_RDMA_TRANSFORM_TYPE_SIGNING,
                    cryptoTransform.TransformType,
                    "SMB2_RDMA_TRANSFORM TransformCount is {0}", rdmaTransform.TransformCount);

                if (signingAlgorithm == SigningAlgorithm.AES_CMAC)
                {
                    BaseTestSite.Assert.AreEqual<uint>(
                        AES_CMAC_NONCE_LENGTH,
                        cryptoTransform.NonceLength,
                        "DataLength in response is {0}", cryptoTransform.NonceLength);
                }
                else if (signingAlgorithm == SigningAlgorithm.AES_GMAC)
                {
                    BaseTestSite.Assert.AreEqual<uint>(
                        AES_GMAC_NONCE_LENGTH,
                        cryptoTransform.NonceLength,
                        "DataLength in response is {0}", cryptoTransform.NonceLength);
                }
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Close file.");
            smbdAdapter.Smb2CloseFile();
        }

        /// <summary>
        /// Write file content over RDMA. The file content will be writen with specific number of operations. 
        /// The total size of the write content is SMB2 negotiated MaxWriteize, so content size 
        /// in each SMB2 WRITE request is ( MaxWriteSize / operationCount )
        /// 
        /// </summary>
        /// <param name="operationCount">Count of SMB2 READ operations.</param>
        /// <param name="enableEncryption">Enable encryption in the Smb2 Connection</param>
        /// <param name="enableSigning">Enable signing in the Smb2 connection</param>
        /// <param name="signingAlgorithm">The signing algorithm to use for the Smb2 connection</param>
        /// <param name="smb2RdmaTransformIds">The RDMA_TRANSFORM_IDs to use for NegotiateContext</param>
        /// <param name="channel">Channel used for Write Request</param>
        public void WriteOverRdma_Signing(
            uint operationCount = 1,
            Smb2RDMATransformId[] smb2RdmaTransformIds = null,
            SigningAlgorithm signingAlgorithm = SigningAlgorithm.AES_GMAC,
            Channel_Values channel = Channel_Values.CHANNEL_RDMA_TRANSFORM,
            bool enableSigning = true,
            bool enableEncryption = false,
            bool setInvalidSignature = false,
            bool setZeroTransformCount = false)
        {
            var isFailedTest = false;
            SigningAlgorithm[] signingAlgorithms = new SigningAlgorithm[]
            {
                signingAlgorithm
            };

            string fileName = CreateRandomFileName();
            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"The filename ==> {fileName}");

            InitSmbdConnectionForTestCases(
                fileName,
                smb2RdmaTransformIds,
                enableEncryption,
                signingAlgorithms,
                enableSigning,
                enableEncryption);

            uint writeSize = smbdAdapter.Smb2MaxWriteSize / operationCount;
            uint totalSize = writeSize * operationCount;

            // SMB2 Write file
            byte[] fileContent = Encoding.ASCII.GetBytes(Smb2Utility.CreateRandomStringInByte((int)writeSize));
            smbdAdapter.SignByteArray(fileContent, out byte[] nonce, out byte[] signature, Smb2Command.WRITE);

            if (setInvalidSignature)
            {
                // Invalidate the signature
                byte temp = signature[3];
                signature[3] = signature[2];
                signature[2] = temp;
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send each write request according SMB2 write file limit with operationCount == " + operationCount);
            for (int i = 0; i < operationCount; ++i)
            {
                // register memory and get buffer descriptor
                SmbdBufferDescriptorV1 descp;
                smbdAdapter.SmbdRegisterBuffer(
                    writeSize,
                    SmbdBufferReadWrite.RDMA_READ_PERMISSION_FOR_WRITE_FILE,
                    out descp
                    );

                // Build RDMA_CRYPTO_TRANSFORM
                SMB2_RDMA_CRYPTO_TRANSFORM cryptoTransform = new SMB2_RDMA_CRYPTO_TRANSFORM
                {
                    TransformType = Smb2RDMATransformType.SMB2_RDMA_TRANSFORM_TYPE_SIGNING,
                    SignatureLength = (ushort)signature.Length,
                    NonceLength = (ushort)nonce.Length,
                    Signature = signature,
                    Nonce = nonce,
                };
                var cryptoTransformBytes = TypeMarshal.ToBytes(cryptoTransform);
                Smb2Utility.Align8(ref cryptoTransformBytes);

                // Build RDMA_TRANSFORM
                SMB2_RDMA_TRANSFORM rdmaTransform = new SMB2_RDMA_TRANSFORM
                {
                    RdmaDescriptorOffset = 0,
                    RdmaDescriptorLength = 1,
                    Channel = Smb2RdmaTransformChannel.SMB2_CHANNEL_RDMA_V1_INVALIDATE,
                    TransformCount = (ushort)(setZeroTransformCount ? 0 : 1),
                    Reserved1 = 0,
                    Reserved2 = 0
                };
                var rdmaTransformBytes = TypeMarshal.ToBytes(rdmaTransform);

                rdmaTransform.RdmaDescriptorOffset = (ushort)(cryptoTransformBytes.Length + rdmaTransformBytes.Length);

                smbdAdapter.SmbdWriteRegisteredBuffer(fileContent, descp);
                byte[] channelInfo = TypeMarshal.ToBytes(descp);
                rdmaTransform.RdmaDescriptorLength = (ushort)channelInfo.Length;

                rdmaTransformBytes = TypeMarshal.ToBytes(rdmaTransform);

                ushort totalLength = (ushort)(channelInfo.Length + rdmaTransform.RdmaDescriptorOffset);
                byte[] totalBytes = new byte[totalLength];
                rdmaTransformBytes.CopyTo(totalBytes, 0);
                cryptoTransformBytes.CopyTo(totalBytes, rdmaTransformBytes.Length);
                channelInfo.CopyTo(totalBytes, cryptoTransformBytes.Length + rdmaTransformBytes.Length);

                BaseTestSite.Log.Add(LogEntryKind.TestStep, $"rdmaTransformBytes.Length ==> {rdmaTransformBytes.Length}");
                BaseTestSite.Log.Add(LogEntryKind.TestStep, $"cryptoTransformBytes.Length ==> {cryptoTransformBytes.Length}");
                BaseTestSite.Log.Add(LogEntryKind.TestStep, $"channelInfo ==> {channelInfo.Length}");

                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Write content to file over RDMA.");
                WRITE_Response writeResponse;
                NtStatus status = (NtStatus)smbdAdapter.Smb2WriteOverRdmaChannel(
                    (UInt64)i * writeSize,
                    totalBytes,
                    writeSize,
                    out writeResponse,
                    channel
                    );

                if (setInvalidSignature)
                {
                    BaseTestSite.Assert.AreEqual<NtStatus>(
                        NtStatus.STATUS_INVALID_SIGNATURE,
                        status,
                        "Status of SMB2 Write {0}", status);
                    isFailedTest = true;
                }
                else if (setZeroTransformCount)
                {
                    BaseTestSite.Assert.AreEqual<NtStatus>(
                        NtStatus.STATUS_INVALID_PARAMETER,
                        status,
                        "Server MUST return STATUS_INVALID_PARAMETER if TransformCount is 0 and channel is SMB2_CHANNEL_RDMA_TRANSFORM");
                    isFailedTest = true;
                }
                else if (!enableSigning ||
                    smb2RdmaTransformIds == null ||
                    channel != Channel_Values.CHANNEL_RDMA_TRANSFORM)
                {
                    BaseTestSite.Assert.AreEqual<NtStatus>(
                    NtStatus.STATUS_INVALID_PARAMETER,
                    status,
                    "Server MUST return STATUS_INVALID_PARAMETER when TransformType = SMB2_RDMA_TRANSFORM_SIGNING and " +
                    "SMB2_FLAGS_SIGNED bit is not set in the Flags field of SMB2 header" +
                    "", status);

                    isFailedTest = true;
                }
                else if (smb2RdmaTransformIds.Contains(Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_ENCRYPTION) ||
                    smb2RdmaTransformIds.Contains(Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_NONE))
                {
                    BaseTestSite.Assert.AreEqual<NtStatus>(
                    NtStatus.STATUS_INVALID_PARAMETER,
                    status,
                    "Server MUST return STATUS_INVALID_PARAMETER when Connection.RDMATransformIds does not contain " +
                    "SMB2_RDMA_TRANSFORM_SIGNING, and SMB2_RDMA_CRYPTO_TRANSFORM with TransformType equal to " +
                    "SMB2_RDMA_TRANSFORM_TYPE_SIGNING is present", status);

                    isFailedTest = true;
                }
                else
                {
                    BaseTestSite.Assert.AreEqual<NtStatus>(
                        NtStatus.STATUS_SUCCESS,
                        status,
                        "Status of SMB2 Write File offset {0} is {1}", i * writeSize, status);
                    BaseTestSite.Assert.AreEqual<uint>(
                        (uint)writeSize,
                        writeResponse.Count,
                        "DataLength in WRITE response is {0}", writeResponse.Count);
                }
            }

            smbdAdapter.Smb2CloseFile();
            smbdAdapter.Smb2LogOff();

            if (!isFailedTest)
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Validate file content.");
                ValidateFileContent(fileContent, totalSize, fileName);
            }
        }

        private void ValidateFileContent(byte[] content, uint fileSize, string fileName)
        {
            smbdAdapter = new SmbdAdapter(BaseTestSite, LogSmbdEndpointEvent);

            InitSmbdConnectionForTestCases(fileName);

            uint readSize = (uint)content.Length;
            // SMB2 Read file
            SmbdBufferDescriptorV1 descp;
            smbdAdapter.SmbdRegisterBuffer(
                readSize,
                SmbdBufferReadWrite.RDMA_WRITE_PERMISSION_FOR_READ_FILE,
                out descp);

            // send each read request according to SMB2 read file limit
            uint offset = 0;
            while (offset < fileSize)
            {
                byte[] channelInfo = TypeMarshal.ToBytes(descp);

                uint length = readSize;
                if (offset + length > fileSize)
                {
                    length = fileSize - offset;
                }
                READ_Response readResponse;
                byte[] readData;
                NtStatus status = (NtStatus)smbdAdapter.Smb2ReadOverRdmaChannel(
                    (UInt64)offset,
                    (uint)length,
                    channelInfo,
                    out readResponse,
                    out readData
                    );
                BaseTestSite.Assert.AreEqual<NtStatus>(
                    NtStatus.STATUS_SUCCESS,
                    status,
                    "Status of SMB2 Read File is {0}", status);
                BaseTestSite.Assert.AreEqual<uint>(
                    (uint)length,
                    readResponse.DataRemaining,
                    "DataRemaining in READ response is {0}", readResponse.DataRemaining);
                BaseTestSite.Assert.AreEqual<byte>(
                    EXPECTED_READ_DATA_OFFSET,
                    readResponse.DataOffset,
                    "DataOffset in response is {0}", readResponse.DataOffset);

                byte[] readFileContent = new byte[readSize];
                smbdAdapter.SmbdReadRegisteredBuffer(readFileContent, descp);

                BaseTestSite.Assert.IsTrue(
                    SmbdUtilities.CompareByteArray(content, readFileContent),
                    "Check content of file");

                offset += readSize;
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Close file.");
            smbdAdapter.Smb2CloseFile();
        }

        private void InitSmbdConnectionForTestCases(
            string fileName,
            Smb2RDMATransformId[] smb2RdmaTransformIds = null,
            bool addDefaultEncryption = true,
            SigningAlgorithm[] signingAlgorithms = null,
            bool enableSigning = false,
            bool enableEncryption = true)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Initial SMBD connection and open file " + fileName);

            // Connect to server over RDMA
            NtStatus status = smbdAdapter.ConnectToServerOverRDMA();
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMBD connection is {0}", status);

            // SMBD Negotiate
            status = smbdAdapter.SmbdNegotiate();
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMBD negotiate is {0}", status);

            DialectRevision[] dialects = new DialectRevision[]
            {
                DialectRevision.Smb311, DialectRevision.Smb302
            };

            status = smbdAdapter.Smb2EstablishSessionAndOpenFile(
                fileName,
                negotiatedDialects: dialects,
                smb2RDMATransformIds: smb2RdmaTransformIds,
                addDefaultEncryption: addDefaultEncryption,
                signingAlgorithms: signingAlgorithms,
                enableSigning: enableSigning,
                enableEncryption: enableEncryption);
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMB2 establish session and open file is {0}", status);
        }
        #endregion
    }
}
