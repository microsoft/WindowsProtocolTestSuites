// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.Smbd.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smbd;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Microsoft.Protocol.TestSuites.Smbd.TestSuite
{
    [TestClass]
    public class Smb2OverRdmaChannelTransformEncryption : Smb2OverSmbdTestBase
    {
        #region Variables
        // Length of Smb2 Packet Header + Smb2 READ Response before Buffer
        private const int EXPECTED_READ_DATA_OFFSET = 80;
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
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("Encryption")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can read file with large size over RDMA channel encrypted with AES128_CCM")]
        public void BVT_Smb2OverRdmaChannelTransform_Encryption_ReadLargeFile_AES128CCM()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_ENCRYPTION };
            ReadOverRdma_Encryption(smb2RdmaTransformIds: smb2RdmaTransformIds, encryptionAlgorithm: EncryptionAlgorithm.ENCRYPTION_AES128_CCM);
        }

        [TestMethod()]
        [TestCategory("Positive")]
        [TestCategory("Encryption")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can read file with large size over RDMA channel encrypted with AES128_GCM")]
        public void Smb2OverRdmaChannelTransform_Encryption_ReadLargeFile_AES128GCM()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_ENCRYPTION };
            ReadOverRdma_Encryption(smb2RdmaTransformIds: smb2RdmaTransformIds, encryptionAlgorithm: EncryptionAlgorithm.ENCRYPTION_AES128_GCM);
        }

        [TestMethod()]
        [TestCategory("Positive")]
        [TestCategory("Encryption")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can read file with large size over RDMA channel encrypted with AES256_CCM")]
        public void Smb2OverRdmaChannelTransform_Encryption_ReadLargeFile_AES256CCM()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_ENCRYPTION };
            ReadOverRdma_Encryption(smb2RdmaTransformIds: smb2RdmaTransformIds, encryptionAlgorithm: EncryptionAlgorithm.ENCRYPTION_AES256_CCM);
        }

        [TestMethod()]
        [TestCategory("Positive")]
        [TestCategory("Encryption")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can read file with large size over RDMA channel encrypted with AES256_GCM")]
        public void Smb2OverRdmaChannelTransform_Encryption_ReadLargeFile_AES256GCM()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_ENCRYPTION };
            ReadOverRdma_Encryption(smb2RdmaTransformIds: smb2RdmaTransformIds, encryptionAlgorithm: EncryptionAlgorithm.ENCRYPTION_AES256_GCM);
        }

        [TestMethod()]
        [TestCategory("Positive")]
        [TestCategory("Encryption")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can read file with large size over R" +
            "DMA channel with multiple SMBD READ requests and responses encrypted with AES128_CCM.")]
        public void Smb2OverRdmaChannelTransform_Encryption_ReadMultipleOperation()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_ENCRYPTION };
            const uint OPERATION_COUNT = 64;
            ReadOverRdma_Encryption(OPERATION_COUNT, smb2RdmaTransformIds: smb2RdmaTransformIds);
        }

        [TestMethod()]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("Encryption")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can write file with large size over RDMA channel encrypted with AES128_CCM")]
        public void BVT_Smb2OverRdmaChannelTransform_Encryption_WriteLargeFile_AES128CCM()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_ENCRYPTION };
            WriteOverRdma_Encryption(
                smb2RdmaTransformIds: smb2RdmaTransformIds,
                channel: Channel_Values.CHANNEL_RDMA_TRANSFORM,
                encryptionAlgorithm: EncryptionAlgorithm.ENCRYPTION_AES128_CCM);
        }

        [TestMethod()]
        [TestCategory("Positive")]
        [TestCategory("Encryption")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can write file with large size over RDMA channel encrypted with AES128_GCM")]
        public void Smb2OverRdmaChannelTransform_Encryption_WriteLargeFile_AES128GCM()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_ENCRYPTION };
            WriteOverRdma_Encryption(
                smb2RdmaTransformIds: smb2RdmaTransformIds,
                channel: Channel_Values.CHANNEL_RDMA_TRANSFORM,
                encryptionAlgorithm: EncryptionAlgorithm.ENCRYPTION_AES128_GCM);
        }

        [TestMethod()]
        [TestCategory("Positive")]
        [TestCategory("Encryption")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can write file with large size over RDMA channel encrypted with AES256_CCM")]
        public void Smb2OverRdmaChannelTransform_Encryption_WriteLargeFile_AES256CCM()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_ENCRYPTION };
            WriteOverRdma_Encryption(
                smb2RdmaTransformIds: smb2RdmaTransformIds,
                channel: Channel_Values.CHANNEL_RDMA_TRANSFORM,
                encryptionAlgorithm: EncryptionAlgorithm.ENCRYPTION_AES256_CCM);
        }

        [TestMethod()]
        [TestCategory("Positive")]
        [TestCategory("Encryption")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can write file with large size over RDMA channel encrypted with AES256_GCM")]
        public void Smb2OverRdmaChannelTransform_Encryption_WriteLargeFile_AES256GCM()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_ENCRYPTION };
            WriteOverRdma_Encryption(
                smb2RdmaTransformIds: smb2RdmaTransformIds,
                channel: Channel_Values.CHANNEL_RDMA_TRANSFORM,
                encryptionAlgorithm: EncryptionAlgorithm.ENCRYPTION_AES256_GCM);
        }

        [TestMethod()]
        [TestCategory("Positive")]
        [TestCategory("Encryption")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can write file with large size over RDMA channel encrypted with AES128_CCM " +
            "with multiple SMBD WRITE requests and responses.")]
        public void Smb2OverRdmaChannelTransform_Encryption_WriteMultipleOperation()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_ENCRYPTION };
            const uint OPERATION_COUNT = 64;
            WriteOverRdma_Encryption(OPERATION_COUNT, smb2RdmaTransformIds: smb2RdmaTransformIds, channel: Channel_Values.CHANNEL_RDMA_TRANSFORM);
        }

        [TestMethod()]
        [TestCategory("Encryption")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can write file with large size over RDMA channel encrypted with AES128_CCM fails " +
            "if RDMATransformId is not specified on Connection Open")]
        public void Smb2OverRdmaChannelTransform_Encryption_WriteLargeFile_NoTransformIds_Fails()
        {
            WriteOverRdma_Encryption(channel: Channel_Values.CHANNEL_RDMA_TRANSFORM);
        }

        [TestMethod()]
        [TestCategory("Encryption")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can write file with large size over RDMA channel encrypted with AES128_CCM fails " +
            "if RDMA_TRANSFORM TransformCount is set to zero")]
        public void Smb2OverRdmaChannelTransform_Encryption_WriteLargeFile_ZeroTransformCount_Fails()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_ENCRYPTION };
            WriteOverRdma_Encryption(smb2RdmaTransformIds: smb2RdmaTransformIds, setZeroTransformCount: true);
        }

        [TestMethod()]
        [TestCategory("Encryption")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can write file with large size over RDMA channel encrypted with AES128_CCM fails " +
            "if multiple RDMA_TRANSFORM_TYPE_ENCRYPTION RDMA_CRYPTO_TRANSFORM structure are used for the request")]
        public void Smb2OverRdmaChannelTransform_Encryption_WriteLargeFile_MultipleEncryptionStructure_Fails()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_ENCRYPTION };
            WriteOverRdma_Encryption(smb2RdmaTransformIds: smb2RdmaTransformIds, addEncryptionTransform: true);
        }

        [TestMethod()]
        [TestCategory("Encryption")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can write file with large size over RDMA channel encrypted with AES128_CCM fails " +
            "if an RDMA_TRANSFORM_TYPE_ENCRYPTION and RDMA_TRANSFORM_TYPE_SIGNING structure are used together for the request")]
        public void Smb2OverRdmaChannelTransform_Encryption_WriteLargeFile_EncryptionAndSigningStructure_Fails()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] {
                Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_SIGNING,
                Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_ENCRYPTION
            };
            SigningAlgorithm[] signingAlgorithms = new SigningAlgorithm[]
            {
                SigningAlgorithm.AES_GMAC
            };
            WriteOverRdma_Encryption(smb2RdmaTransformIds: smb2RdmaTransformIds, addEncryptionTransform: true, addSigningTransform: true, signingAlgorithms: signingAlgorithms);
        }

        [TestMethod()]
        [TestCategory("Encryption")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can write file with large size over RDMA channel encrypted with AES128_CCM fails " +
            "if the RDMA_CRYPTO_TRANSFORM structure is not 8-byte aligned")]
        public void Smb2OverRdmaChannelTransform_Encryption_WriteLargeFile_BufferNotAligned_Fails()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_ENCRYPTION };
            WriteOverRdma_Encryption(
                smb2RdmaTransformIds: smb2RdmaTransformIds,
                channel: Channel_Values.CHANNEL_RDMA_TRANSFORM,
                encryptionAlgorithm: EncryptionAlgorithm.ENCRYPTION_AES128_GCM,
                shouldAlignBuffer: false);
        }

        [TestMethod()]
        [TestCategory("Encryption")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can write file with large size over RDMA channel encrypted fails " +
            "if the RDMA_CRYPTO_TRANSFORM SignatureLength is greater than 16 bytes")]
        public void Smb2OverRdmaChannelTransform_Encryption_WriteLargeFile_IncreaseSignatureLength_Fails()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_ENCRYPTION };
            WriteOverRdma_Encryption(
                smb2RdmaTransformIds: smb2RdmaTransformIds,
                channel: Channel_Values.CHANNEL_RDMA_TRANSFORM,
                encryptionAlgorithm: EncryptionAlgorithm.ENCRYPTION_AES128_GCM,
                shouldIncreaseSignature: true);
        }

        [TestMethod()]
        [TestCategory("Encryption")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can write file with large size over RDMA channel encrypted with AES128_CCM fails " +
            "if the RDMA_CRYPTO_TRANSFORM NonceLength is greater than 11 bytes")]
        public void Smb2OverRdmaChannelTransform_Encryption_WriteLargeFile_IncreaseNonceLength_AES128CCM_Fails()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_ENCRYPTION };
            WriteOverRdma_Encryption(
                smb2RdmaTransformIds: smb2RdmaTransformIds,
                channel: Channel_Values.CHANNEL_RDMA_TRANSFORM,
                encryptionAlgorithm: EncryptionAlgorithm.ENCRYPTION_AES128_CCM,
                shouldIncreaseNonce: true);
        }

        [TestMethod()]
        [TestCategory("Encryption")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can write file with large size over RDMA channel encrypted with AES128_GCM fails " +
            "if the RDMA_CRYPTO_TRANSFORM NonceLength is greater than 12 bytes")]
        public void Smb2OverRdmaChannelTransform_Encryption_WriteLargeFile_IncreaseNonceLength_AES128GCM_Fails()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_ENCRYPTION };
            WriteOverRdma_Encryption(
                smb2RdmaTransformIds: smb2RdmaTransformIds,
                encryptionAlgorithm: EncryptionAlgorithm.ENCRYPTION_AES128_GCM,
                shouldIncreaseNonce: true);
        }

        [TestMethod()]
        [TestCategory("Encryption")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can write file with large size over RDMA channel encrypted with AES256_CCM fails " +
            "if the RDMA_CRYPTO_TRANSFORM NonceLength is greater than 11 bytes")]
        public void Smb2OverRdmaChannelTransform_Encryption_WriteLargeFile_IncreaseNonceLength_AES256CCM_Fails()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_ENCRYPTION };
            WriteOverRdma_Encryption(
                smb2RdmaTransformIds: smb2RdmaTransformIds,
                channel: Channel_Values.CHANNEL_RDMA_TRANSFORM,
                encryptionAlgorithm: EncryptionAlgorithm.ENCRYPTION_AES256_CCM,
                shouldIncreaseNonce: true);
        }

        [TestMethod()]
        [TestCategory("Encryption")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can write file with large size over RDMA channel encrypted with AES256_GCM fails " +
            "if the RDMA_CRYPTO_TRANSFORM NonceLength is greater than 12 bytes")]
        public void Smb2OverRdmaChannelTransform_Encryption_WriteLargeFile_IncreaseNonceLength_AES256GCM_Fails()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_ENCRYPTION };
            WriteOverRdma_Encryption(
                smb2RdmaTransformIds: smb2RdmaTransformIds,
                encryptionAlgorithm: EncryptionAlgorithm.ENCRYPTION_AES256_GCM,
                shouldIncreaseNonce: true);
        }

        [TestMethod()]
        [TestCategory("Encryption")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can write file with large size over RDMA channel encrypted with AES128_CCM fails " +
            "if the RDMA_CRYPTO_TRANSFORM NonceLength is less than 11 bytes")]
        public void Smb2OverRdmaChannelTransform_Encryption_WriteLargeFile_DecreaseNonceLength_AES128CCM_Fails()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_ENCRYPTION };
            WriteOverRdma_Encryption(
                smb2RdmaTransformIds: smb2RdmaTransformIds,
                channel: Channel_Values.CHANNEL_RDMA_TRANSFORM,
                encryptionAlgorithm: EncryptionAlgorithm.ENCRYPTION_AES128_CCM,
                shouldDecreaseNonce: true);
        }

        [TestMethod()]
        [TestCategory("Encryption")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can write file with large size over RDMA channel encrypted with AES128_GCM fails " +
            "if the RDMA_CRYPTO_TRANSFORM NonceLength is less than 12 bytes")]
        public void Smb2OverRdmaChannelTransform_Encryption_WriteLargeFile_DecreaseNonceLength_AES128GCM_Fails()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_ENCRYPTION };
            WriteOverRdma_Encryption(
                smb2RdmaTransformIds: smb2RdmaTransformIds,
                encryptionAlgorithm: EncryptionAlgorithm.ENCRYPTION_AES128_GCM,
                shouldDecreaseNonce: true);
        }

        [TestMethod()]
        [TestCategory("Encryption")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can write file with large size over RDMA channel encrypted with AES256_CCM fails " +
            "if the RDMA_CRYPTO_TRANSFORM NonceLength is less than 11 bytes")]
        public void Smb2OverRdmaChannelTransform_Encryption_WriteLargeFile_DecreaseNonceLength_AES256CCM_Fails()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_ENCRYPTION };
            WriteOverRdma_Encryption(
                smb2RdmaTransformIds: smb2RdmaTransformIds,
                channel: Channel_Values.CHANNEL_RDMA_TRANSFORM,
                encryptionAlgorithm: EncryptionAlgorithm.ENCRYPTION_AES256_CCM,
                shouldDecreaseNonce: true);
        }

        [TestMethod()]
        [TestCategory("Encryption")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can write file with large size over RDMA channel encrypted with AES256_GCM fails " +
            "if the RDMA_CRYPTO_TRANSFORM NonceLength is less than 12 bytes")]
        public void Smb2OverRdmaChannelTransform_Encryption_WriteLargeFile_DecreaseNonceLength_AES256GCM_Fails()
        {
            Smb2RDMATransformId[] smb2RdmaTransformIds = new Smb2RDMATransformId[] { Smb2RDMATransformId.SMB2_RDMA_TRANSFORM_ENCRYPTION };
            WriteOverRdma_Encryption(
                smb2RdmaTransformIds: smb2RdmaTransformIds,
                encryptionAlgorithm: EncryptionAlgorithm.ENCRYPTION_AES256_GCM,
                shouldDecreaseNonce: true);
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
        /// <param name="encryptionAlgorithm">EncryptionAlgorithm to test</param>
        /// <param name="smb2RdmaTransformIds">The RDMA_TRANSFORM_IDs to use for NegotiateContext</param>
        public void ReadOverRdma_Encryption(
            uint operationCount = 1,
            Smb2RDMATransformId[] smb2RdmaTransformIds = null,
            EncryptionAlgorithm encryptionAlgorithm = EncryptionAlgorithm.ENCRYPTION_AES128_CCM)
        {
            EncryptionAlgorithm[] encryptionAlgorithms = new EncryptionAlgorithm[] { encryptionAlgorithm };
            InitSmbdConnectionForTestCases(
                smbdAdapter.TestConfig.TestFileName_LargeFile,
                smb2RdmaTransformIds,
                encryptionAlgorithms: encryptionAlgorithms);

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
                byte[] channelInfo = TypeMarshal.ToBytes<SmbdBufferDescriptorV1>(descp);

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
                BaseTestSite.Assert.AreNotEqual<int>(
                    0,
                    readData.Length,
                    "Data length of content in response is {0} and NOT 0", readData.Length);
                BaseTestSite.Assert.AreEqual<byte>(
                    EXPECTED_READ_DATA_OFFSET,
                    readResponse.DataOffset,
                    "DataOffset in response is {0}", readResponse.DataOffset);
                BaseTestSite.Assert.AreNotEqual<uint>(
                    0,
                    readResponse.DataLength,
                    "DataLength in response is {0} and NOT 0", readResponse.DataLength);

            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Close file.");
            smbdAdapter.Smb2CloseFile();
        }

        /// <summary>
        /// Write file content over RDMA. The file content will be writen with specific number of operations. 
        /// The total size of the write content is SMB2 negotiated MaxWriteize, so content size 
        /// in each SMB2 WRITE request is ( MaxWriteSize / operationCount )
        /// </summary>
        /// <param name="operationCount">count of SMB2 WRITE operation</param>
        /// <param name="encryptionAlgorithm">EncryptionAlgorithm to test</param>
        /// <param name="smb2RdmaTransformIds">The RDMA_TRANSFORM_IDs to use for NegotiateContext</param>
        /// <param name="channel">Channel used for Write Request</param>
        /// <param name="addEncryptionTransform">Add an extra SMB2_RDMA_TRANSFORM_TYPE_ENCRYPTION structure</param>
        /// <param name="addSigningTransform">Add an SMB2_RDMA_TRANSFORM_TYPE_SIGNING structure</param>
        /// <param name="setZeroTransformCount">Set the TransformCount to 0</param>
        /// <param name="shouldAlignBuffer">Indicates if the SMB2_RDMA_TRANSFORM_TYPE_ENCRYPTION should be 8 byte aligned</param>
        /// <param name="signingAlgorithms">The SigningAlgorithms to use (if including signingTransform)</param>
        public void WriteOverRdma_Encryption(
            uint operationCount = 1,
            Smb2RDMATransformId[] smb2RdmaTransformIds = null,
            EncryptionAlgorithm encryptionAlgorithm = EncryptionAlgorithm.ENCRYPTION_AES128_CCM,
            Channel_Values channel = Channel_Values.CHANNEL_RDMA_TRANSFORM,
            bool setZeroTransformCount = false,
            bool addEncryptionTransform = false,
            bool addSigningTransform = false,
            bool shouldAlignBuffer = true,
            SigningAlgorithm[] signingAlgorithms = null,
            bool shouldIncreaseSignature = false,
            bool shouldIncreaseNonce = false,
            bool shouldDecreaseNonce = false)
        {
            var isFailedTest = false;
            string fileName = CreateRandomFileName();
            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"The filename ==> {fileName}");

            var descriptorOffset = 0;
            var transformCount = setZeroTransformCount ? 0 : 1;

            EncryptionAlgorithm[] encryptionAlgorithms = encryptionAlgorithm == EncryptionAlgorithm.ENCRYPTION_NONE ? null : new EncryptionAlgorithm[] { encryptionAlgorithm };
            InitSmbdConnectionForTestCases(
                fileName,
                smb2RdmaTransformIds: smb2RdmaTransformIds,
                encryptionAlgorithms: encryptionAlgorithms,
                signingAlgorithms: signingAlgorithms,
                addDefaultEncryption: false,
                enableEncryption: encryptionAlgorithms != null);

            uint writeSize = smbdAdapter.Smb2MaxWriteSize / operationCount;
            uint totalSize = writeSize * operationCount;
            List<byte> totalBytes = new List<byte>();

            // SMB2 Write file
            byte[] fileContent = Encoding.ASCII.GetBytes(Smb2Utility.CreateRandomStringInByte((int)writeSize));
            smbdAdapter.EncryptByteArray(fileContent, out byte[] encrypted, out byte[] nonce, out byte[] signature);

            if (shouldDecreaseNonce)
            {
                byte[] newNonce = new byte[nonce.Length - 1];
                Array.Copy(nonce, 0, newNonce, 0, newNonce.Length);
                nonce = newNonce;
            }

            if (shouldIncreaseNonce)
            {
                byte[] newNonce = new byte[nonce.Length + 1];
                Array.Copy(nonce, 0, newNonce, 1, nonce.Length);
                newNonce[0] = nonce[0];
                nonce = newNonce;
            }

            if (shouldIncreaseSignature)
            {
                byte[] newSignature = new byte[signature.Length + 1];
                Array.Copy(signature, 0, newSignature, 1, signature.Length);
                newSignature[0] = signature[0];
                signature = newSignature;
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send each write request according SMB2 write file limit. with operationCount == " + operationCount);
            for (int i = 0; i < operationCount; ++i)
            {
                // register memory and get buffer descriptor
                smbdAdapter.SmbdRegisterBuffer(
                    writeSize,
                    SmbdBufferReadWrite.RDMA_READ_PERMISSION_FOR_WRITE_FILE,
                    out SmbdBufferDescriptorV1 descp
                    );

                // Build RDMA_CRYPTO_TRANSFORM
                SMB2_RDMA_CRYPTO_TRANSFORM cryptoTransform = new()
                {
                    TransformType = Smb2RDMATransformType.SMB2_RDMA_TRANSFORM_TYPE_ENCRYPTION,
                    SignatureLength = (ushort)signature.Length,
                    NonceLength = (ushort)nonce.Length,
                    Signature = signature,
                    Nonce = nonce,
                };
                var cryptoTransformBytes = TypeMarshal.ToBytes(cryptoTransform);
                if (shouldAlignBuffer)
                    Smb2Utility.Align8(ref cryptoTransformBytes);
                descriptorOffset += cryptoTransformBytes.Length;

                if (addEncryptionTransform)
                {
                    transformCount++;
                    descriptorOffset += cryptoTransformBytes.Length;
                }

                if (addSigningTransform)
                {
                    SMB2_RDMA_CRYPTO_TRANSFORM signingTransform = new()
                    {
                        TransformType = Smb2RDMATransformType.SMB2_RDMA_TRANSFORM_TYPE_ENCRYPTION,
                        SignatureLength = (ushort)signature.Length,
                        NonceLength = (ushort)nonce.Length,
                        Signature = signature,
                        Nonce = nonce,
                    };
                    var signingTransformBytes = TypeMarshal.ToBytes(signingTransform);
                    if (shouldAlignBuffer)
                        Smb2Utility.Align8(ref signingTransformBytes);
                    transformCount++;
                    descriptorOffset += signingTransformBytes.Length;
                }

                // Build RDMA_TRANSFORM
                SMB2_RDMA_TRANSFORM rdmaTransform = new()
                {
                    RdmaDescriptorOffset = 0,
                    RdmaDescriptorLength = 1,
                    Channel = Smb2RdmaTransformChannel.SMB2_CHANNEL_RDMA_V1,
                    TransformCount = (ushort)transformCount,
                    Reserved1 = 0,
                    Reserved2 = 0
                };
                var rdmaTransformBytes = TypeMarshal.ToBytes(rdmaTransform);
                descriptorOffset += rdmaTransformBytes.Length;

                rdmaTransform.RdmaDescriptorOffset = (ushort)descriptorOffset;

                smbdAdapter.SmbdWriteRegisteredBuffer(encrypted, descp);
                byte[] channelInfo = TypeMarshal.ToBytes<SmbdBufferDescriptorV1>(descp);
                rdmaTransform.RdmaDescriptorLength = (ushort)channelInfo.Length;

                rdmaTransformBytes = TypeMarshal.ToBytes(rdmaTransform);

                totalBytes.AddRange(rdmaTransformBytes);

                totalBytes.AddRange(cryptoTransformBytes);

                if (addEncryptionTransform)
                {
                    totalBytes.AddRange(cryptoTransformBytes);
                }

                totalBytes.AddRange(channelInfo);

                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Write content to file over RDMA.");
                NtStatus status = (NtStatus)smbdAdapter.Smb2WriteOverRdmaChannel(
                    (UInt64)i * writeSize,
                    totalBytes.ToArray(),
                    writeSize,
                    out WRITE_Response writeResponse,
                    channel
                    );

                if ((smb2RdmaTransformIds == null || smb2RdmaTransformIds.Length == 0) && channel == Channel_Values.CHANNEL_RDMA_TRANSFORM)
                {
                    BaseTestSite.Assert.AreEqual(
                        NtStatus.STATUS_INVALID_PARAMETER,
                        status,
                        "Server MUST return STATUS_INVALID_PARAMETER if RDMATransformIds is empty and channel is SMB2_CHANNEL_RDMA_TRANSFORM");
                    isFailedTest = true;
                }
                else if (setZeroTransformCount)
                {
                    BaseTestSite.Assert.AreEqual(
                        NtStatus.STATUS_INVALID_PARAMETER,
                        status,
                        "Server MUST return STATUS_INVALID_PARAMETER if TransformCount is 0 and channel " +
                        "is SMB2_CHANNEL_RDMA_TRANSFORM");
                    isFailedTest = true;
                }
                else if (addEncryptionTransform)
                {
                    BaseTestSite.Assert.AreEqual(
                        NtStatus.STATUS_INVALID_PARAMETER,
                        status,
                        "Server MUST return STATUS_INVALID_PARAMETER if More than one SMB2_RDMA_CRYPTO_TRANSFORM " +
                        "structures with TransformType equal to SMB2_RDMA_TRANSFORM_TYPE_ENCRYPTION are present");
                    isFailedTest = true;
                }
                else if (addSigningTransform)
                {
                    BaseTestSite.Assert.AreEqual(
                        NtStatus.STATUS_INVALID_PARAMETER,
                        status,
                        "Server MUST return STATUS_INVALID_PARAMETER if Two SMB2_RDMA_CRYPTO_TRANSFORM " +
                        "structures with TransformType equal to SMB2_RDMA_TRANSFORM_TYPE_ENCRYPTION and " +
                        "SMB2_RDMA_TRANSFORM_TYPE_SIGNING are present");
                    isFailedTest = true;
                }
                else if (!shouldAlignBuffer)
                {
                    BaseTestSite.Assert.AreEqual(
                        NtStatus.STATUS_INVALID_PARAMETER,
                        status,
                        "Server MUST return STATUS_INVALID_PARAMETER if SMB_DIRECT_BUFFER_DESCRIPTOR_V1 structures " +
                        "don't begin at the first 8-byte aligned offset");
                    isFailedTest = true;
                }
                else if (shouldIncreaseSignature || shouldIncreaseNonce || shouldDecreaseNonce)
                {
                    BaseTestSite.Assert.AreEqual(
                        NtStatus.STATUS_AUTH_TAG_MISMATCH,
                        status,
                        "The Server MUST fail the request with STATUS_AUTH_TAG_MISMATCH if SignatureLength field is greater than 16, " +
                        "Connection.CipherId is AES-128-CCM or AES-256-CCM and NonceLength field is not equal to 11 or " +
                        "Connection.CipherId is AES-128-GCM or AES-256-GCM and NonceLength field is not equal to 12.");
                    isFailedTest = true;
                }
                else
                {
                    BaseTestSite.Assert.AreEqual(
                        NtStatus.STATUS_SUCCESS,
                        status,
                        "Status of SMB2 Write File offset {0} is {1}", i * writeSize, status);
                    BaseTestSite.Assert.AreEqual(
                        (uint)writeSize,
                        writeResponse.Count,
                        "DataLength in WRITE response is {0}", writeResponse.Count);
                }
            }

            if (!isFailedTest)
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Validate file content.");
                ValidateFileContent(fileContent, totalSize, fileName);
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Close file.");
            smbdAdapter.Smb2CloseFile();
        }

        private void ValidateFileContent(byte[] content, uint fileSize, string fileName)
        {
            smbdAdapter.Smb2CloseFile();

            smbdAdapter.Smb2LogOff();
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
                byte[] channelInfo = TypeMarshal.ToBytes<SmbdBufferDescriptorV1>(descp);

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
                    80,
                    readResponse.DataOffset,
                    "DataOffset in response is {0}", readResponse.DataOffset);

                byte[] readFileContent = new byte[readSize];
                smbdAdapter.SmbdReadRegisteredBuffer(readFileContent, descp);

                BaseTestSite.Assert.IsTrue(
                    SmbdUtilities.CompareByteArray(content, readFileContent),
                    "Check content of file");

                offset += readSize;
            }
        }

        private void InitSmbdConnectionForTestCases(
            string fileName,
            Smb2RDMATransformId[] smb2RdmaTransformIds = null,
            bool addDefaultEncryption = true,
            EncryptionAlgorithm[] encryptionAlgorithms = null,
            SigningAlgorithm[] signingAlgorithms = null,
            bool enableSigning = false,
            bool enableEncryption = true)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Initial SMBD connection and open file " + fileName);

            uint size = smbdAdapter.TestConfig.LargeFileSizeInByte;
            // Connect to server over RDMA
            NtStatus status = smbdAdapter.ConnectToServerOverRDMA();
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMBD connection is {0}", status);

            // SMBD Negotiate
            status = smbdAdapter.SmbdNegotiate();
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMBD negotiate is {0}", status);


            DialectRevision[] dialects = new DialectRevision[]
            {
                DialectRevision.Smb311
            };

            status = smbdAdapter.Smb2EstablishSessionAndOpenFile(
                fileName,
                negotiatedDialects: dialects,
                smb2RDMATransformIds: smb2RdmaTransformIds,
                addDefaultEncryption: addDefaultEncryption,
                encryptionAlgs: encryptionAlgorithms,
                signingAlgorithms: signingAlgorithms,
                enableSigning: enableSigning,
                enableEncryption: enableEncryption);
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMB2 establish session and open file is {0}", status);
        }

        #endregion
    }
}
