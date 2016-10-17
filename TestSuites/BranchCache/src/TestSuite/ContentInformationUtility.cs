// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Protocols.TestSuites.BranchCache.TestSuite;
using Microsoft.Protocols.TestSuites.BranchCache.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp;
using Microsoft.Protocols.TestTools.StackSdk.CommonStack;
using Microsoft.Protocols.TestTools.StackSdk.CommonStack.Enum;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.BranchCache
{
    public class ContentInformationUtility
    {
        private ITestSite testSite;

        private TestConfig testConfig;

        private ISUTControlAdapter sutControlAdapter;

        public const int DefaultBlockSize = 64 * 1024;

        public ContentInformationUtility(ITestSite testSite, TestConfig testConfig, ISUTControlAdapter sutControlAdapter)
        {
            this.testSite = testSite;
            this.testConfig = testConfig;
            this.sutControlAdapter = sutControlAdapter;
        }

        public void ClearContentServerHash()
        {
            switch (testConfig.ContentTransport)
            {
                case ContentInformationTransport.PCCRTP:
                    sutControlAdapter.ClearHTTPHash(testConfig.ContentServerComputerName, testConfig.WebsiteLocalPath);
                    break;

                case ContentInformationTransport.SMB2:
                    sutControlAdapter.ClearSMB2Hash(testConfig.ContentServerComputerName, testConfig.FileShareLocalPath);
                    break;
            }
        }

        public byte[] RetrieveContentData()
        {
            return RetrieveContentData(testConfig.NameOfFileWithMultipleBlocks);
        }

        public byte[] RetrieveContentData(string filename)
        {
            switch (testConfig.ContentTransport)
            {
                case ContentInformationTransport.PCCRTP:

                    PccrtpClient pccrtpClient = new PccrtpClient();
                    PccrtpRequest pccrtpRequest = pccrtpClient.CreatePccrtpRequest(
                        testConfig.ContentServerComputerName,
                        testConfig.ContentServerHTTPListenPort,
                        filename);

                    PccrtpResponse pccrtpResponse = pccrtpClient.SendHttpRequest(
                        Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp.HttpVersionType.HttpVersion11,
                        pccrtpRequest,
                        (int)testConfig.Timeout.TotalMilliseconds);

                    testSite.Assert.AreNotEqual(
                       "peerdist",
                       pccrtpResponse.HttpResponse.ContentEncoding,
                       "The content server should not have content information ready yet");

                    return pccrtpResponse.PayloadData;

                case ContentInformationTransport.SMB2:

                    using (BranchCacheSmb2ClientTransport smb2Client = new BranchCacheSmb2ClientTransport(testConfig.Timeout, testSite, testConfig.SupportBranchCacheV1, testConfig.SupportBranchCacheV2))
                    {
                        smb2Client.OpenFile(
                            testConfig.ContentServerComputerName,
                            testConfig.SharedFolderName,
                            filename,
                            testConfig.SecurityPackageType,
                            testConfig.DomainName,
                            testConfig.UserName,
                            testConfig.UserPassword,
                            AccessMask.GENERIC_READ);

                        HASH_HEADER hashHeader;
                        byte[] hashData = null;

                        uint status = 0;

                        if (testConfig.SupportBranchCacheV1)
                        {
                            status = smb2Client.ReadHash(
                                SRV_READ_HASH_Request_HashType_Values.SRV_HASH_TYPE_PEER_DIST,
                                SRV_READ_HASH_Request_HashVersion_Values.SRV_HASH_VER_1,
                                SRV_READ_HASH_Request_HashRetrievalType_Values.SRV_HASH_RETRIEVE_HASH_BASED,
                                0,
                                uint.MaxValue,
                                out hashHeader,
                                out hashData);

                            testSite.Assert.AreNotEqual(
                                Smb2Status.STATUS_SUCCESS,
                                status,
                                "The content server should not have content information ready yet");

                            testSite.CaptureRequirementIfAreEqual(
                                Smb2Status.STATUS_HASH_NOT_PRESENT,
                                status,
                                RequirementCategory.HashNotPresent,
                                RequirementCategory.HashNotPresentMessage);
                        }

                        if (testConfig.SupportBranchCacheV2)
                        {
                            status = smb2Client.ReadHash(
                                SRV_READ_HASH_Request_HashType_Values.SRV_HASH_TYPE_PEER_DIST,
                                SRV_READ_HASH_Request_HashVersion_Values.SRV_HASH_VER_2,
                                SRV_READ_HASH_Request_HashRetrievalType_Values.SRV_HASH_RETRIEVE_FILE_BASED,
                                0,
                                uint.MaxValue,
                                out hashHeader,
                                out hashData);

                            testSite.Assert.AreNotEqual(
                                Smb2Status.STATUS_SUCCESS,
                                status,
                                "The content server should not have content information ready yet");

                            testSite.CaptureRequirementIfAreEqual(
                                Smb2Status.STATUS_HASH_NOT_PRESENT,
                                status,
                                RequirementCategory.HashNotPresent,
                                RequirementCategory.HashNotPresentMessage);
                        }

                        return smb2Client.ReadAllBytes();
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        public void ForceTriggerContentServerHashGeneration()
        {
            ForceTriggerContentServerHashGeneration(testConfig.NameOfFileWithMultipleBlocks);
        }

        public void ForceTriggerContentServerHashGeneration(string filename)
        {
            switch (testConfig.ContentTransport)
            {
                case ContentInformationTransport.PCCRTP:

                    // Make sure forced hash generation is supported
                    if (!testConfig.SupportWebsiteForcedHashGeneration)
                        testSite.Assert.Inconclusive("Implementation does not support HTTP forced hash generation");

                    // Trigger forced hash generation
                    sutControlAdapter.GenerateHTTPHash(testConfig.ContentServerComputerName, testConfig.WebsiteLocalPath);

                    break;

                case ContentInformationTransport.SMB2:

                    // Make sure forced hash generation is supported
                    if (!testConfig.SupportFileShareForcedHashGeneration)
                        testSite.Assert.Inconclusive("Implementation does not support SMB2 forced hash generation");

                    // Trigger forced hash generation
                    sutControlAdapter.GenerateSMB2Hash(testConfig.ContentServerComputerName, testConfig.FileShareLocalPath);

                    break;
            }
        }

        public byte[] RetrieveContentInformation(BranchCacheVersion version)
        {
            return RetrieveContentInformation(version, testConfig.NameOfFileWithMultipleBlocks);
        }

        public byte[] RetrieveContentInformation(BranchCacheVersion version, string filename)
        {
            switch (testConfig.ContentTransport)
            {
                case ContentInformationTransport.PCCRTP:

                    TestUtility.DoUntilSucceed(() => sutControlAdapter.IsHTTPHashExisted(testConfig.ContentServerComputerName), testConfig.Timeout, testConfig.RetryInterval);

                    PccrtpClient pccrtpClient = new PccrtpClient();
                    PccrtpRequest pccrtpRequest = pccrtpClient.CreatePccrtpRequest(
                        testConfig.ContentServerComputerName,
                        testConfig.ContentServerHTTPListenPort,
                        filename,
                        version);

                    var pccrtpResponse = pccrtpClient.SendHttpRequest(
                        Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp.HttpVersionType.HttpVersion11,
                        pccrtpRequest,
                        (int)testConfig.Timeout.TotalMilliseconds);

                    testSite.Assert.AreEqual(
                       "peerdist",
                       pccrtpResponse.HttpResponse.ContentEncoding,
                       "The content server should return peerdist encoded content information");

                    return pccrtpResponse.PayloadData;

                case ContentInformationTransport.SMB2:

                    using (BranchCacheSmb2ClientTransport smb2Client = new BranchCacheSmb2ClientTransport(testConfig.Timeout, testSite, testConfig.SupportBranchCacheV1, testConfig.SupportBranchCacheV2))
                    {
                        smb2Client.OpenFile(
                            testConfig.ContentServerComputerName,
                            testConfig.SharedFolderName,
                            filename,
                            testConfig.SecurityPackageType,
                            testConfig.DomainName,
                            testConfig.UserName,
                            testConfig.UserPassword,
                            AccessMask.GENERIC_READ);

                        HASH_HEADER hashHeader;
                        byte[] hashData = null;

                        TestUtility.DoUntilSucceed(
                            () => smb2Client.ReadHash(
                                SRV_READ_HASH_Request_HashType_Values.SRV_HASH_TYPE_PEER_DIST,
                                version == BranchCacheVersion.V1 ? SRV_READ_HASH_Request_HashVersion_Values.SRV_HASH_VER_1 : SRV_READ_HASH_Request_HashVersion_Values.SRV_HASH_VER_2,
                                version == BranchCacheVersion.V1 ? SRV_READ_HASH_Request_HashRetrievalType_Values.SRV_HASH_RETRIEVE_HASH_BASED : SRV_READ_HASH_Request_HashRetrievalType_Values.SRV_HASH_RETRIEVE_FILE_BASED,
                                0,
                                uint.MaxValue,
                                out hashHeader,
                                out hashData) == Smb2Status.STATUS_SUCCESS,
                            testConfig.Timeout,
                            testConfig.RetryInterval);

                        testSite.Assert.AreNotEqual(
                           0,
                           hashData.Length,
                           "The content server should return content information in READ_HASH_RESPONSE");

                        return hashData;
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        public Content_Information_Data_Structure CreateContentInformationV1()
        {
            var contentInformation = new Content_Information_Data_Structure
            {
                Version = 0x0100,
                dwHashAlgo = dwHashAlgo_Values.SHA256,
                cSegments = 1,
                segments = new SegmentDescription[]
                    {
                        new SegmentDescription
                        {
                             cbBlockSize = DefaultBlockSize,
                             cbSegment = DefaultBlockSize,
                             SegmentHashOfData = TestUtility.GenerateRandomArray(32),
                             SegmentSecret = TestUtility.GenerateRandomArray(32),
                             ullOffsetInContent = 0
                        }
                    },
                dwOffsetInFirstSegment = 0,
                dwReadBytesInLastSegment = DefaultBlockSize,
                blocks = new SegmentContentBlocks[]
                {
                    new SegmentContentBlocks
                    {
                        cBlocks = 1,
                        BlockHashes = TestUtility.GenerateRandomArray(32),
                    }
                }
            };

            return contentInformation;
        }

        public Content_Information_Data_Structure_V2 CreateContentInformationV2()
        {
            var contentInformation = new Content_Information_Data_Structure_V2
            {
                bMajorVersion = 2,
                bMinorVersion = 0,
                ullIndexOfFirstSegment = 0,
                ullLengthOfRange = DefaultBlockSize,
                dwOffsetInFirstSegment = 0,
                ullStartInContent = 0,
                dwHashAlgo = dwHashAlgoV2_Values.TRUNCATED_SHA512,
                chunks = new ChunkDescription[]
                {
                    new ChunkDescription
                    {
                        dwChunkDataLength = DefaultBlockSize,
                        chunkData = new SegmentDescriptionV2[]
                        {
                            new SegmentDescriptionV2
                            {
                                cbSegment = DefaultBlockSize,
                                SegmentHashOfData = TestUtility.GenerateRandomArray(32),
                                SegmentSecret = TestUtility.GenerateRandomArray(32)
                            }
                        }
                    }
                }
            };

            return contentInformation;
        }

        public void VerifyContentInformation(byte[] content, byte[] contentInformation, BranchCacheVersion version)
        {
            switch (version)
            {
                case BranchCacheVersion.V1:
                    VerifyHashGenerationV1(content, PccrcUtility.ParseContentInformation(contentInformation));
                    break;

                case BranchCacheVersion.V2:
                    VerifyHashGenerationV2(content, PccrcUtility.ParseContentInformationV2(contentInformation));
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public void VerifyHashGenerationV1(byte[] content, Content_Information_Data_Structure contentInfo)
        {
            const int BLOCKBYTECOUNT = 0x10000;
            const int SEGMENTBLOCKCOUNT = 512;

            dwHashAlgo_Values hashAlgo = contentInfo.dwHashAlgo;

            int blockTotalCount = content.Length / BLOCKBYTECOUNT;
            if (content.Length > BLOCKBYTECOUNT * blockTotalCount)
            {
                blockTotalCount = blockTotalCount + 1;
            }

            int segmentCount = blockTotalCount / SEGMENTBLOCKCOUNT;
            if (blockTotalCount > SEGMENTBLOCKCOUNT * segmentCount)
            {
                segmentCount = segmentCount + 1;
            }

            HashAlgorithm hashAlgorithm;
            HMAC hmacAlgorithm;
            int blockHashSize;
            PccrcUtility.GetHashAlgorithm(hashAlgo, out hashAlgorithm, out hmacAlgorithm, out blockHashSize);
            hmacAlgorithm.Key = hashAlgorithm.ComputeHash(testConfig.ServerSecret);

            for (int segmentIndex = 0; segmentIndex < segmentCount; segmentIndex++)
            {
                List<byte> blockHashList = new List<byte>();

                int blockCount = (segmentIndex == segmentCount - 1) ? (blockTotalCount % SEGMENTBLOCKCOUNT) : (SEGMENTBLOCKCOUNT);

                for (int blockIndex = 0; blockIndex < blockCount; blockIndex++)
                {
                    var block = content.Skip(BLOCKBYTECOUNT * SEGMENTBLOCKCOUNT * segmentIndex + BLOCKBYTECOUNT * blockIndex).Take(BLOCKBYTECOUNT).ToArray();

                    byte[] blockHash = hashAlgorithm.ComputeHash(block);

                    testSite.Assert.IsTrue(
                        blockHash.SequenceEqual((contentInfo.blocks[segmentIndex].BlockHashes).Skip(blockIndex * blockHashSize).Take(blockHashSize).ToArray()),
                        "The local calculated block hash in Segment: {0} Block: {1} should cosistent with the received value.", segmentIndex, blockIndex);

                    blockHashList.AddRange(blockHash);
                }

                byte[] hod = hashAlgorithm.ComputeHash(blockHashList.ToArray());

                testSite.Assert.IsTrue(
                    hod.SequenceEqual(contentInfo.segments[segmentIndex].SegmentHashOfData),
                    "The local calculated Hod should cosistent with the received value.");

                byte[] kp = hmacAlgorithm.ComputeHash(hod);

                testSite.Assert.IsTrue(
                    kp.SequenceEqual(contentInfo.segments[segmentIndex].SegmentSecret),
                    "The local calculated Kp should cosistent with the received value.");
            }
        }

        public void VerifyHashGenerationV2(byte[] content, Content_Information_Data_Structure_V2 contentInfoV2)
        {
            dwHashAlgoV2_Values hashAlgo = contentInfoV2.dwHashAlgo;

            HashAlgorithm hashAlgorithm;
            HMAC hmacAlgorithm;
            PccrcUtility.GetHashAlgorithm(hashAlgo, out hashAlgorithm, out hmacAlgorithm);
            hmacAlgorithm.Key = hashAlgorithm.ComputeHash(testConfig.ServerSecret).Take(32).ToArray();

            // Local calculate SegmentHashOfData and SegmentSecret
            ChunkDescription[] chunkDescription = contentInfoV2.chunks;
            int chunkCount = contentInfoV2.chunks.Length;
            int segmentOffset = 0;

            for (int chunkIndex = 0; chunkIndex < chunkCount; chunkIndex++)
            {
                SegmentDescriptionV2[] segmentDescription = chunkDescription[chunkIndex].chunkData;
                int segmentCount = segmentDescription.Length;

                for (int segmentIndex = 0; segmentIndex < segmentCount; ++segmentIndex)
                {
                    var segment = content.Skip(segmentOffset).Take((int)(segmentDescription[segmentIndex].cbSegment)).ToArray();

                    segmentOffset += (int)(segmentDescription[segmentIndex].cbSegment);

                    //TRANCATED_SHA_512
                    byte[] hod = hashAlgorithm.ComputeHash(segment).Take(32).ToArray();

                    testSite.Assert.IsTrue(
                        hod.SequenceEqual((chunkDescription[chunkIndex].chunkData)[segmentIndex].SegmentHashOfData),
                        "The local calculated Hod should cosistent with the received value.");

                    byte[] kp = hmacAlgorithm.ComputeHash(hod).Take(32).ToArray();

                    testSite.Assert.IsTrue(
                        kp.SequenceEqual((chunkDescription[chunkIndex].chunkData)[segmentIndex].SegmentSecret),
                        "The local calculated Kp should be consistent with the received value.");
                }
            }
        }

        class BranchCacheSmb2ClientTransport : Smb2ClientTransport
        {
            private ITestSite testSite;
            private bool supportV1;
            private bool supportV2;

            public BranchCacheSmb2ClientTransport(TimeSpan timeout, ITestSite testSite, bool supportV1, bool supportV2)
                :base(timeout)
            {
                this.testSite = testSite;
                this.supportV1 = supportV1;
                this.supportV2 = supportV2;
            }

            protected override uint Negotiate(ushort creditCharge, ushort creditRequest, ulong messageId, Guid clientGuid, 
                out DialectRevision selectedDialect, out byte[] gssToken, out Packet_Header responseHeader, out NEGOTIATE_Response responsePayload)
            {
                if (supportV2)
                {
                    return client.Negotiate(
                    creditCharge,
                    creditRequest,
                    Packet_Header_Flags_Values.NONE,
                    messageId,
                    new DialectRevision[] { DialectRevision.Smb30 },
                    SecurityMode_Values.NONE,
                    Capabilities_Values.NONE,
                    clientGuid,
                    out selectedDialect,
                    out gssToken,
                    out responseHeader,
                    out responsePayload);
                }
                else
                {
                    return client.Negotiate(
                    creditCharge,
                    creditRequest,
                    Packet_Header_Flags_Values.NONE,
                    messageId,
                    new DialectRevision[] { DialectRevision.Smb21 },
                    SecurityMode_Values.NONE,
                    Capabilities_Values.NONE,
                    clientGuid,
                    out selectedDialect,
                    out gssToken,
                    out responseHeader,
                    out responsePayload);
                }
            }

            protected override uint TreeConnect(ushort creditCharge, ushort creditRequest, Packet_Header_Flags_Values flags, ulong messageId, ulong sessionId, 
                string server, string share, out Packet_Header header, out TREE_CONNECT_Response response)
            {
                uint treeConnectResponseCode = base.TreeConnect(creditCharge, creditRequest, flags, messageId, sessionId, server, share, out header, out response);

                if (supportV1)
                {
                    testSite.Assert.IsTrue(
                    response.ShareFlags.HasFlag(ShareFlags_Values.SHAREFLAG_ENABLE_HASH_V1),
                    "The share content should enable hash v1");
                }
                if (supportV2)
                {
                    testSite.Assert.IsTrue(
                    response.ShareFlags.HasFlag(ShareFlags_Values.SHAREFLAG_ENABLE_HASH_V2),
                    "The share content should enable hash v2");
                }

                return treeConnectResponseCode;
            }
        }
    }
}

