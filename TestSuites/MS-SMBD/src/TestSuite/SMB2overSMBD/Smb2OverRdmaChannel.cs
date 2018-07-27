// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smbd;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;


namespace Microsoft.Protocol.TestSuites.Smbd.TestSuite
{
    [TestClass]
    public class Smb2OverRdmaChannel : Smb2OverSmbdTestBase
    {
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
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can read file with large size over RDMA channel")]
        public void BVT_Smb2OverRdmaChannel_ReadLargeFile()
        {
            ReadOverRdma();
        }

        [TestMethod()]
        [TestCategory("BVT")]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can write file with large size over RDMA channel")]
        public void BVT_Smb2OverRdmaChannel_WriteLargeFile()
        {
            WriteOverRdma();
        }

        [TestMethod()]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can read file with large size over RDMA channel with multiple SMBD READ requests and responses.")]
        public void Smb2OverRdmaChannel_ReadMultipleOperation()
        {
            const uint OPERATION_COUNT = 64;
            ReadOverRdma(OPERATION_COUNT);
        }

        [TestMethod()]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify SMB2 can write file with large size over RDMA channel with multiple SMBD WRITE requests and responses.")]
        public void Smb2OverRdmaChannel_WriteMultipleOperation()
        {
            const uint OPERATION_COUNT = 64;
            WriteOverRdma(OPERATION_COUNT);
        }

        [TestMethod()]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify server can work with SMB2 READ and WRITE over RDMA channel with multiple buffer descriptors in channel information.")]
        public void Smb2OverRdmaChannel_ReadWriteMultipleBufferDescriptorList()
        {
            InitSmbdConnectionForTestCases(smbdAdapter.TestConfig.TestFileName_LargeFile);
            uint size = smbdAdapter.Smb2MaxReadSize;
            if (size > smbdAdapter.Smb2MaxWriteSize)
            {
                size = smbdAdapter.Smb2MaxWriteSize;
            }
            int bufferCount = ushort.MaxValue / SmbdBufferDescriptorV1.SIZE; // SMB2 max support channel info size
            uint bufferLength = (uint)(size / bufferCount + 1); // buffer length of each buffer descriptor
            byte[] channelInfo = new byte[SmbdBufferDescriptorV1.SIZE * bufferCount];
            byte[] writeContent = Encoding.ASCII.GetBytes(Smb2Utility.CreateRandomStringInByte((int)size));
            byte[] readContent = new byte[size];
            NtStatus status;

            #region SMB2 Write file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Register memory and get buffer descriptor for SMB2 WRITE"); 
            SmbdBufferDescriptorV1[] writeDescp = new SmbdBufferDescriptorV1[bufferCount];
            for (int i = 0; i < bufferCount; ++i)
            {
                smbdAdapter.SmbdRegisterBuffer(
                    bufferLength,
                    SmbdBufferReadWrite.RDMA_READ_PERMISSION_FOR_WRITE_FILE,
                    out writeDescp[i]);
                byte[] channelInfoBlock = TypeMarshal.ToBytes<SmbdBufferDescriptorV1>(writeDescp[i]);
                Array.Copy(
                    channelInfoBlock,
                    0,
                    channelInfo,
                    SmbdBufferDescriptorV1.SIZE * i,
                    SmbdBufferDescriptorV1.SIZE);
            }
            // copy write content
            for (int index = 0, writeOffset = 0;
                index < bufferCount && writeOffset < writeContent.Length;
                ++index, writeOffset += (int)bufferLength)
            {
                int length = (int)bufferLength;
                if (length + writeOffset > writeContent.Length)
                {
                    length = writeContent.Length - writeOffset;
                }
                byte[] buffer = new byte[length];
                Array.Copy(writeContent, writeOffset, buffer, 0, length);
                smbdAdapter.SmbdWriteRegisteredBuffer(buffer, writeDescp[index]);
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Write content to file over RDMA.");
            WRITE_Response writeResponse;
            status = (NtStatus)smbdAdapter.Smb2WriteOverRdmaChannel(
                0,
                channelInfo,
                size,
                out writeResponse
                );
            BaseTestSite.Assert.AreEqual<NtStatus>(
                NtStatus.STATUS_SUCCESS,
                status,
                "Status of SMB2 Write File is {0}", status);
            BaseTestSite.Assert.AreEqual<uint>(
                (uint)size,
                writeResponse.Count,
                "DataLength in WRITE response is {0}", writeResponse.Count);

            #endregion


            #region Read file from server
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Register memory and get buffer descriptor for SMB2 WRITE"); 
            SmbdBufferDescriptorV1[] readDescp = new SmbdBufferDescriptorV1[bufferCount];
            for (int i = 0; i < bufferCount; ++i)
            {
                smbdAdapter.SmbdRegisterBuffer(
                    bufferLength,
                    SmbdBufferReadWrite.RDMA_WRITE_PERMISSION_FOR_READ_FILE,
                    out readDescp[i]);
                byte[] channelInfoBlock = TypeMarshal.ToBytes<SmbdBufferDescriptorV1>(readDescp[i]);
                Array.Copy(
                    channelInfoBlock,
                    0,
                    channelInfo,
                    SmbdBufferDescriptorV1.SIZE * i,
                    SmbdBufferDescriptorV1.SIZE);
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Read content from file over RDMA.");
            READ_Response readResponse;
            byte[] readData;
            status = (NtStatus)smbdAdapter.Smb2ReadOverRdmaChannel(
                0,
                size,
                channelInfo,
                out readResponse,
                out readData
                );
            BaseTestSite.Assert.AreEqual<NtStatus>(
                NtStatus.STATUS_SUCCESS, 
                status, 
                "Status of SMB2 Read File is {0}", status);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                readResponse.DataLength,
                "DataLength in READ response is {0}", readResponse.DataLength);
            BaseTestSite.Assert.AreEqual<int>(
                0,
                readData.Length, 
                "Data length of content in response is {0}", readData.Length);
            BaseTestSite.Assert.AreEqual<byte>(
                80, 
                readResponse.DataOffset, 
                "DataOffset in response is {0}", readResponse.DataOffset);
            BaseTestSite.Assert.AreEqual<uint>(
                size,
                readResponse.DataRemaining,
                "DataRemaining in response is {0}", readResponse.DataRemaining);

            // read content 
            for (int index = 0, readOffset = 0;
                index < bufferCount && readOffset < readContent.Length; 
                ++index, readOffset += (int)bufferLength)
            {
                int length = (int)bufferLength;
                if (length + readOffset > readContent.Length)
                {
                    length = writeContent.Length - readOffset;
                }
                byte[] buffer = new byte[length];
                smbdAdapter.SmbdReadRegisteredBuffer(buffer, readDescp[index]);
                Array.Copy(buffer, 0, readContent, readOffset, length);
            }
            #endregion

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Check the read content is same as the one written into the file.");
            BaseTestSite.Assert.IsTrue(
                    SmbdUtilities.CompareByteArray(writeContent, readContent),
                    "Check content of file");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Close file.");
            smbdAdapter.Smb2CloseFile();
        }

        [TestMethod()]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify server will not crash if Offset in Buffer Descriptor is not correct.")]
        public void Smb2OverRdmaChannel_InvalidBufferDescriptor_Offset()
        {
            InitSmbdConnectionForTestCases(smbdAdapter.TestConfig.TestFileName_LargeFile);

            // SMB2 Read file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Register memory and get buffer descriptor for SMB2 READ."); 
            SmbdBufferDescriptorV1 descp;
            uint bufferSize = smbdAdapter.Smb2MaxReadSize;
            smbdAdapter.SmbdRegisterBuffer(
                bufferSize,
                SmbdBufferReadWrite.RDMA_WRITE_PERMISSION_FOR_READ_FILE,
                out descp);

            // modify offset to overrun buffer size which is invalid
            descp.Offset = bufferSize + 1;

            byte[] channelInfo = TypeMarshal.ToBytes<SmbdBufferDescriptorV1>(descp);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Read content from file over RDMA.");
            READ_Response readResponse;
            byte[] readData;
            try
            {
                NtStatus status = (NtStatus)smbdAdapter.Smb2ReadOverRdmaChannel(
                    0,
                    smbdAdapter.Smb2MaxReadSize,
                    channelInfo,
                    out readResponse,
                    out readData
                    );
                BaseTestSite.Assert.AreNotEqual<NtStatus>(
                        NtStatus.STATUS_SUCCESS,
                        status,
                        "Status of SMB2 Read File is {0}", status);
            }
            catch (TimeoutException)
            {
                BaseTestSite.Assert.Pass("No SMB2 READ response received");
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Disconnect and reconnect to verify server is available.");
            smbdAdapter.DisconnectRdma();
            smbdAdapter.ConnectToServerOverRDMA();
        }

        [TestMethod()]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify server will not crash if Length in Buffer Descriptor is not correct.")]
        public void Smb2OverRdmaChannel_InvalidBufferDescriptor_Length()
        {
            InitSmbdConnectionForTestCases(smbdAdapter.TestConfig.TestFileName_LargeFile);

            // SMB2 Read file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Register memory and get buffer descriptor for SMB2 WRITE"); 
            SmbdBufferDescriptorV1 descp;
            smbdAdapter.SmbdRegisterBuffer(
                smbdAdapter.Smb2MaxReadSize - 1,
                SmbdBufferReadWrite.RDMA_WRITE_PERMISSION_FOR_READ_FILE,
                out descp);
            // modify offset to make it invalid
            descp.Length++;

            byte[] channelInfo = TypeMarshal.ToBytes<SmbdBufferDescriptorV1>(descp);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Read content from file over RDMA.");
            READ_Response readResponse;
            byte[] readData;
            try
            {
                NtStatus status = (NtStatus)smbdAdapter.Smb2ReadOverRdmaChannel(
                    0,
                    smbdAdapter.Smb2MaxReadSize,
                    channelInfo,
                    out readResponse,
                    out readData
                    );
                BaseTestSite.Assert.AreNotEqual<NtStatus>(
                        NtStatus.STATUS_SUCCESS,
                        status,
                        "Status of SMB2 Read File is {0}", status);
            }
            catch (TimeoutException)
            {
                BaseTestSite.Assert.Pass("No SMB2 READ response received");
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Disconnect and reconnect to verify server is available.");
            smbdAdapter.DisconnectRdma();
            smbdAdapter.ConnectToServerOverRDMA();
        }

        [TestMethod()]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify server will not crash if Token in Buffer Descriptor is not correct.")]
        public void Smb2OverRdmaChannel_InvalidBufferDescriptor_Token()
        {
            InitSmbdConnectionForTestCases(smbdAdapter.TestConfig.TestFileName_LargeFile);

            // SMB2 Read file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Register memory and get buffer descriptor for SMB2 WRITE"); 
            SmbdBufferDescriptorV1 descp;
            smbdAdapter.SmbdRegisterBuffer(
                smbdAdapter.Smb2MaxReadSize,
                SmbdBufferReadWrite.RDMA_WRITE_PERMISSION_FOR_READ_FILE,
                out descp);
            // modify offset to make it invalid
            descp.Token++;

            byte[] channelInfo = TypeMarshal.ToBytes<SmbdBufferDescriptorV1>(descp);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Read content from file over RDMA.");
            READ_Response readResponse;
            byte[] readData;
            try
            {
                NtStatus status = (NtStatus)smbdAdapter.Smb2ReadOverRdmaChannel(
                    0,
                    smbdAdapter.Smb2MaxReadSize,
                    channelInfo,
                    out readResponse,
                    out readData
                    );
                BaseTestSite.Assert.AreNotEqual<NtStatus>(
                        NtStatus.STATUS_SUCCESS,
                        status,
                        "Status of SMB2 Read File is {0}", status);
            }
            catch (TimeoutException)
            {
                BaseTestSite.Assert.Pass("No SMB2 READ response received");
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Disconnect and reconnect to verify server is available.");            
            smbdAdapter.DisconnectRdma();
            smbdAdapter.ConnectToServerOverRDMA();
        }

        [TestMethod()]
        [TestCategory("Smb2OverRdmaChannel")]
        [Description("Verify server will not crash if buffer descriptor in channel info is deregistered.")]
        public void Smb2OverRdmaChannel_InvalidBufferDescriptor_DeregisteredBuffer()
        {
            InitSmbdConnectionForTestCases(smbdAdapter.TestConfig.TestFileName_LargeFile);

            // SMB2 Read file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Register memory and get buffer descriptor for SMB2 WRITE"); 
            SmbdBufferDescriptorV1 descp;
            smbdAdapter.SmbdRegisterBuffer(
                smbdAdapter.Smb2MaxReadSize,
                SmbdBufferReadWrite.RDMA_WRITE_PERMISSION_FOR_READ_FILE,
                out descp);
            smbdAdapter.SmbdDeregisterBuffer(descp);
            byte[] channelInfo = TypeMarshal.ToBytes<SmbdBufferDescriptorV1>(descp);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Read content from file over RDMA.");
            READ_Response readResponse;
            byte[] readData;
            try
            {
                NtStatus status = (NtStatus)smbdAdapter.Smb2ReadOverRdmaChannel(
                    0,
                    smbdAdapter.Smb2MaxReadSize,
                    channelInfo,
                    out readResponse,
                    out readData
                    );
                BaseTestSite.Assert.AreNotEqual<NtStatus>(
                        NtStatus.STATUS_SUCCESS,
                        status,
                        "Status of SMB2 Read File is {0}", status);
            }
            catch (TimeoutException)
            {
                BaseTestSite.Assert.Pass("No SMB2 READ response received");
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Disconnect and reconnect to verify server is available.");
            smbdAdapter.DisconnectRdma();
            smbdAdapter.ConnectToServerOverRDMA();
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
        public void ReadOverRdma(uint operationCount = 1)
        {
            InitSmbdConnectionForTestCases(smbdAdapter.TestConfig.TestFileName_LargeFile);

            uint readSize = smbdAdapter.Smb2MaxReadSize / operationCount;
            uint totalSize = readSize * operationCount;
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
                BaseTestSite.Assert.AreEqual<int>(
                    0,
                    readData.Length,
                    "Data length of content in response is {0}", readData.Length);
                BaseTestSite.Assert.AreEqual<byte>(
                    80,
                    readResponse.DataOffset,
                    "DataOffset in response is {0}", readResponse.DataOffset);
                BaseTestSite.Assert.AreEqual<uint>(
                    0,
                    readResponse.DataLength,
                    "DataLength in response is {0}", readResponse.DataLength);

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
        public void WriteOverRdma(uint operationCount = 1)
        {
            string fileName = CreateRandomFileName();

            InitSmbdConnectionForTestCases(fileName);

            uint writeSize = smbdAdapter.Smb2MaxWriteSize / operationCount;
            uint totalSize = writeSize * operationCount;

            // SMB2 Write file
            byte[] fileContent = Encoding.ASCII.GetBytes(Smb2Utility.CreateRandomStringInByte((int)writeSize));
            
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send each write request according SMB2 write file limit.");            
            for (int i = 0; i < operationCount; ++i)
            {
                // register memory and get buffer descriptor
                SmbdBufferDescriptorV1 descp;
                smbdAdapter.SmbdRegisterBuffer(
                    writeSize,
                    SmbdBufferReadWrite.RDMA_READ_PERMISSION_FOR_WRITE_FILE,
                    out descp
                    );
                smbdAdapter.SmbdWriteRegisteredBuffer(fileContent, descp);
                byte[] channelInfo = TypeMarshal.ToBytes<SmbdBufferDescriptorV1>(descp);

                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Write content to file over RDMA.");
                WRITE_Response writeResponse;
                NtStatus status = (NtStatus)smbdAdapter.Smb2WriteOverRdmaChannel(
                    (UInt64)i * writeSize,
                    channelInfo,
                    writeSize,
                    out writeResponse
                    );
                BaseTestSite.Assert.AreEqual<NtStatus>(
                    NtStatus.STATUS_SUCCESS,
                    status,
                    "Status of SMB2 Write File offset {0} is {1}", i * writeSize, status);
                BaseTestSite.Assert.AreEqual<uint>(
                    (uint)writeSize,
                    writeResponse.Count,
                    "DataLength in WRITE response is {0}", writeResponse.Count);
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Validate file content.");
            ValidateFileContent(fileContent, totalSize);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Close file.");
            smbdAdapter.Smb2CloseFile();
        }


        public void ValidateFileContent(byte[] content, uint fileSize)
        {
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
                BaseTestSite.Assert.AreEqual<int>(
                    0,
                    readData.Length,
                    "Data length of content in response is {0}", readData.Length);
                BaseTestSite.Assert.AreEqual<byte>(
                    80,
                    readResponse.DataOffset,
                    "DataOffset in response is {0}", readResponse.DataOffset);
                BaseTestSite.Assert.AreEqual<uint>(
                    0,
                    readResponse.DataLength,
                    "DataLength in response is {0}", readResponse.DataLength);

                byte[] readFileContent = new byte[readSize];
                smbdAdapter.SmbdReadRegisteredBuffer(readFileContent, descp);
                BaseTestSite.Assert.IsTrue(
                    SmbdUtilities.CompareByteArray(content, readFileContent),
                    "Check content of file");


                offset += readSize;
            }
        }

        public void InitSmbdConnectionForTestCases(string fileName)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Initial SMBD connection and open file " + fileName);

            uint size = smbdAdapter.TestConfig.LargeFileSizeInByte;
            // Connect to server over RDMA
            NtStatus status = smbdAdapter.ConnectToServerOverRDMA();
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMBD connection is {0}", status);

            // SMBD Negotiate
            status = smbdAdapter.SmbdNegotiate();
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMBD negotiate is {0}", status);


            status = smbdAdapter.Smb2EstablishSessionAndOpenFile(fileName);
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMB2 establish session and open file is {0}", status);
        }
        #endregion
    }
}