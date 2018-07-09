// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.Smbd.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smbd;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace Microsoft.Protocol.TestSuites.Smbd.TestSuite
{
    [TestClass]
    public class SmbdCreditManagement : SmbdTestBase
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
        [TestCategory("SmbdCreditMgmt")]
        [Description("Verify SMBD operation works fine with minimum credits (only 1 credit)")]
        public void BVT_SmbdCreditMgmt_OperationWithMinimumCredits()
        {
            const ushort MIN_CREDITS = 1;
            OperationWithSpecifiedCredits(MIN_CREDITS);
        }

        [TestMethod()]
        [TestCategory("SmbdCreditMgmt")]
        [Description("Verify SMBD operation works fine with limited credits.")]
        public void SmbdCreditMgmt_OperationWithLimitedCredits()
        {
            const ushort LIMITED_CREDITS = 10;
            OperationWithSpecifiedCredits(LIMITED_CREDITS);
        }

        [TestMethod()]
        [TestCategory("SmbdCreditMgmt")]
        [Description("Consume all the send credits on client. Verify server will send empty SMBD data transfer message to grant credits.")]
        public void SmbdCreditMgmt_ConsumeAllSendCredits()
        {
            Initialize_ConsumeAllSendCredits();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server will send empty SMBD data transfer message to grant credits.");
            SmbdDataTransferMessage dataTransfer;
            // receive the credit grant
            smbdAdapter.SmbdReceivDataTransferMessage(
                TimeSpan.FromSeconds(SmbdConnection.KEEP_ALIVE_INTERVAL),
                out dataTransfer);

            BaseTestSite.Assert.IsTrue(
                dataTransfer.CreditsGranted > 0,
                "Server should grant credits in an empty SMBD data transfer message.");
            BaseTestSite.Assert.IsTrue(
                dataTransfer.CreditsRequested > 0,
                "CreditsRequested is {0}", dataTransfer.CreditsRequested);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                dataTransfer.DataLength,
                "DataLength is {0}", dataTransfer.DataLength);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                dataTransfer.DataOffset,
                "DataOffset is {0}", dataTransfer.DataOffset);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                dataTransfer.RemainingDataLength,
                "RemainingDataLength is {0}", dataTransfer.RemainingDataLength);
        }


        [TestMethod()]
        [TestCategory("SmbdCreditMgmt")]
        [Description("Make server send several segments and consume all sent credits on server. "
                    + "Verify server will not continue to send SMBD data transfer message until client grant new credits.")]
        public void SmbdCreditMgmt_ConsumeAllReceiveCredits()
        {
            // define data for test case
            const ushort RECEIVE_CREDIT_MAX = 10;
            const ushort SEND_CREDIT_TARGET = 10;

            Initialize_ReceiveSmbdDataTransferTestCase(
                RECEIVE_CREDIT_MAX,
                SEND_CREDIT_TARGET,
                smbdAdapter.TestConfig.ModerateFileSizeInByte);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Validate SMB2 Read response");
            ValidateReadResponse(
                RECEIVE_CREDIT_MAX,
                (int)smbdAdapter.TestConfig.ModerateFileSizeInByte);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Close file and disconnect from server.");
            smbdAdapter.Smb2CloseFile();
            smbdAdapter.DisconnectRdma();
        }

        [TestMethod()]
        [TestCategory("SmbdCreditMgmt")]
        [Description("Verify connection will be terminated because of Send Credit Grant Timer expiration if server with no send credits")]
        public void SmbdCreditMgmt_ConsumeAllCreditServer_Timeout()
        {
            // define data for test case
            const ushort RECEIVE_CREDIT_MAX = 10;
            const ushort SEND_CREDIT_TARGET = 10;

            Initialize_ReceiveSmbdDataTransferTestCase(
                RECEIVE_CREDIT_MAX,
                SEND_CREDIT_TARGET,
                smbdAdapter.TestConfig.ModerateFileSizeInByte);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Receive 10 messages to comsume all credits.");
            #region Receive 10 messages to comsume all credits
            SmbdDataTransferMessage transferPackage;
            int offset = 0;

            NtStatus status;
            while (smbdAdapter.ClientConnection.ReceiveCredits > 0)
            {
                status = (NtStatus)smbdAdapter.SmbdReceivDataTransferMessage(
                    smbdAdapter.TestConfig.Smb2ConnectionTimeout,
                    out transferPackage
                    );
                if (transferPackage.DataLength == 0)
                {
                    if (offset > 0)
                    {
                        // 0 length SMBDirect Data Transfer message in the segments
                        BaseTestSite.Assert.Fail("Empty SMBDirect Data Transfer Message in the Fragmented messages");
                    }
                    else
                    {
                        continue;
                    }
                }

                #region Assert
                BaseTestSite.Assert.AreEqual<uint>(
                    (uint)(smbdAdapter.TestConfig.ModerateFileSizeInByte - offset),
                    transferPackage.DataLength + transferPackage.RemainingDataLength,
                    "Received SMBDirect Data Transfer Message: DataLength {0}, RemainingDataLength {1}", transferPackage.DataLength, transferPackage.RemainingDataLength);
                BaseTestSite.Assert.AreEqual<uint>(
                    0,
                    transferPackage.DataOffset % 8,
                    "Received SMBDirect Data Transfer Message: DataOffset {0}, which should be 8-byte aligned", transferPackage.DataOffset);
                #endregion

                offset += (int)transferPackage.DataLength;

                if (transferPackage.RemainingDataLength == 0)
                {
                    break;
                }
            }
            #endregion

            BaseTestSite.Log.Add(LogEntryKind.TestStep, string.Format("Wait for a KEEP_ALIVE_INTERVAL time in {0} seconds.", SmbdConnection.KEEP_ALIVE_INTERVAL));
            Thread.Sleep(TimeSpan.FromSeconds(SmbdConnection.KEEP_ALIVE_INTERVAL));

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Check no more message receive.");
            BaseTestSite.Assert.AreEqual<int>(
                0,
                smbdAdapter.ReceiveEntryInQueue,
                "No message should be sent from server");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Check server connection will be terminated.");
            smbdAdapter.WaitRdmaDisconnect();
            BaseTestSite.Assert.IsFalse(smbdAdapter.ClientConnection.Endpoint.IsConnected, "Connection should be terminated");
        }

        [TestMethod()]
        [TestCategory("SmbdCreditMgmt")]
        [Description("Verify the connection will be terminated if CreditsRequested is 0 in SMBD Negotiate Request.")]
        public void SmbdCreditMgmt_NegativeParameter_CreditRequested()
        {
            // define data for test case
            const ushort RECEIVE_CREDIT_MAX = 10;
            const ushort SEND_CREDIT_TARGET = 10;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Connect to server over RDMA");
            NtStatus status = smbdAdapter.ConnectToServerOverRDMA();
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMBD connection is {0}", status);
            
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "SMBD Negotiate");
            SmbdNegotiateResponse response;
            status = smbdAdapter.SmbdNegotiate(
                SEND_CREDIT_TARGET,
                RECEIVE_CREDIT_MAX,
                out response);
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMBD negotiate is {0}", status);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send SMBD Data Transfer Message request with 0 credit.");
            status = smbdAdapter.SmbdSendDataTransferMessage(
                0,
                0,
                SmbdDataTransfer_Flags.NONE,
                0,
                0,
                0,
                new byte[0]);
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of send SMBD Data Transfer message is {0}", status);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server connection will be terminated.");
            smbdAdapter.WaitRdmaDisconnect();
            BaseTestSite.Assert.IsFalse(smbdAdapter.ClientConnection.Endpoint.IsConnected, "Connection should be terminated");
        }
        #endregion

        #region Common Methods
        public void OperationWithSpecifiedCredits(ushort credits)
        {
            // define test data for this test case
            ushort receiveCreditMax = credits;
            ushort sendCreditTarget = credits;

            uint size = smbdAdapter.TestConfig.ModerateFileSizeInByte;

            string fileName = CreateRandomFileName();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Connect to server over RDMA.");            
            NtStatus status = smbdAdapter.ConnectToServerOverRDMA();
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMBD connection is {0}", status);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "SMBD Negotiate with server.");             
            SmbdNegotiateResponse response;
            status = smbdAdapter.SmbdNegotiate(sendCreditTarget, receiveCreditMax, out response);
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMBD negotiate is {0}", status);
            BaseTestSite.Assert.IsTrue(response.MaxFragmentedSize >= SmbdConnection.FLOOR_MAX_FRAGMENTED_SIZE
                , "MaxFragmentedSize in negotiate response is {0}", response.MaxFragmentedSize); // check the MaxFragementSize

            // SMB2 Negotiate, Session Setup, Tree Connect and Open File
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Establish SMB2 connection and open file " + fileName);  
            status = smbdAdapter.Smb2EstablishSessionAndOpenFile(fileName);
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMB2 establish session and open file is {0}", status);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Write a moderate size of data to the file."); 
            byte[] data = new byte[size];
            WRITE_Response writeResponse;
            status = (NtStatus)smbdAdapter.Smb2Write(0, data, out writeResponse);
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMB2 write File is {0}", status);
            BaseTestSite.Assert.AreEqual<uint>(size, writeResponse.Count, "Size of written file is {0}", writeResponse.Count);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Close file and disconnect from server."); 
            smbdAdapter.Smb2CloseFile();
            // disconnect
            smbdAdapter.DisconnectRdma();
        }

        /// <summary>
        /// Initialize receive SMBDirect data transfer test case
        /// And send the SMB2 READ request to peer
        /// </summary>
        /// <param name="maxReceiveCredit"></param>
        /// <param name="maxSendCredit"></param>
        /// <param name="smb2ReadResponseSize"></param>
        /// <param name="isUseMaxSendSize"></param>
        public void Initialize_ReceiveSmbdDataTransferTestCase(
            ushort maxReceiveCredit,
            ushort maxSendCredit,
            uint smb2ReadResponseSize,
            bool isUseMaxSendSize = false)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Connect to server over RDMA");
            NtStatus status = smbdAdapter.ConnectToServerOverRDMA();
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMBD connection is {0}", status);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "SMBD Negotiate");
            SmbdNegotiateResponse response;
            status = smbdAdapter.SmbdNegotiate(maxSendCredit, maxReceiveCredit, out response);
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMBD negotiate is {0}", status);

            // SMB2 Negotiate, Session Setup, Tree Connect and Create
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Establish SMB2 connection and open file " + smbdAdapter.TestConfig.TestFileName_LargeFile);
            status = smbdAdapter.Smb2EstablishSessionAndOpenFile(smbdAdapter.TestConfig.TestFileName_LargeFile);
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMB2 establish session and open file is {0}", status);

            // calculate file size and generate file content
            int receivedFileSize = (int)smb2ReadResponseSize -
                Smb2OverSmbdTestClient.READ_RESPONSE_HEADER_SIZE;// calculate the file content size
            if (isUseMaxSendSize)
            {
                receivedFileSize = (int)(smbdAdapter.ClientConnection.MaxReceiveSize -
                    Smb2OverSmbdTestClient.READ_RESPONSE_HEADER_SIZE);
            }

            byte[] readRequestPacket = smbdAdapter.Smb2GetReadRequest((uint)receivedFileSize);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send SMB2 Read request over RDMA");
            status = (NtStatus)smbdAdapter.SmbdSendDataTransferMessage(
                maxReceiveCredit,
                0,
                SmbdDataTransfer_Flags.NONE,
                0,
                (uint)SmbdDataTransferMessage.DEFAULT_DATA_OFFSET,
                (uint)readRequestPacket.Length,
                readRequestPacket
                );
            BaseTestSite.Assert.AreEqual<NtStatus>(status, NtStatus.STATUS_SUCCESS, "Status of send SMB2 Read request is {0}", status);

        }

        /// <summary>
        /// Initialize for "Comsume Send Credits" test cases.
        /// It will establish RDMA connection and then consume all sent credits
        /// </summary>
        public void Initialize_ConsumeAllSendCredits(bool sendOneMoreMessage = false)
        {
            // define data for test case
            const ushort RECEIVE_CREDIT_MAX = 10;
            const ushort SEND_CREDIT_TARGET = 10;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Connect to server over RDMA");
            NtStatus status = smbdAdapter.ConnectToServerOverRDMA();
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMBD connection is {0}", status);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "SMBD Negotiate");
            SmbdNegotiateResponse negotiateResponse;
            status = smbdAdapter.SmbdNegotiate(SEND_CREDIT_TARGET, RECEIVE_CREDIT_MAX, out negotiateResponse);
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMBD negotiate is {0}", status);

            status = smbdAdapter.SmbdPostReceive();
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of post receive is {0}", status);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Consume all send credits on the client.");
            while (smbdAdapter.ClientConnection.SendCredits > 0)
            {
                status = smbdAdapter.SmbdSendDataTransferMessage(
                    SEND_CREDIT_TARGET,
                    1,
                    SmbdDataTransfer_Flags.NONE,
                    0,
                    0,
                    0,
                    new byte[0]);
                smbdAdapter.ClientConnection.SendCredits--;
            }
            if (sendOneMoreMessage)
            {
                status = smbdAdapter.SmbdSendDataTransferMessage(
                    SEND_CREDIT_TARGET,
                    1,
                    SmbdDataTransfer_Flags.NONE,
                    0,
                    0,
                    0,
                    new byte[0]);
            }
        }

        /// <summary>
        /// Receive and validate SMB2 Read Response
        /// </summary>
        /// <param name="readResponseSize">Size of SMB2 read response</param>
        public void ValidateReadResponse(int credits, int readResponseSize)
        {
            // receive read response, maybe in several SMBDirect data transfer messages
            SmbdDataTransferMessage transferPackage = new SmbdDataTransferMessage();
            byte[] readResponsePacket = new byte[readResponseSize];
            int offset = 0;

            NtStatus status;
            while (offset < readResponseSize)
            {
                while (smbdAdapter.ClientConnection.ReceiveCredits > 0)
                {
                    status = (NtStatus)smbdAdapter.SmbdReceivDataTransferMessage(
                        smbdAdapter.TestConfig.Smb2ConnectionTimeout,
                        out transferPackage
                        );
                    BaseTestSite.Log.Add(
                        LogEntryKind.Debug,
                        "Received a SMBDirect Data Transfer message, current Connection.ReceiveCredits is {0}", smbdAdapter.ClientConnection.ReceiveCredits);

                    #region Assert
                    BaseTestSite.Assert.AreEqual<NtStatus>(
                        NtStatus.STATUS_SUCCESS,
                        status,
                        "Received SMBDirect Data Transfer Message with status {0}", status);
                    BaseTestSite.Assert.AreEqual<uint>(
                        (uint)(readResponseSize - offset),
                        transferPackage.DataLength + transferPackage.RemainingDataLength,
                        "Received SMBDirect Data Transfer Message: DataLength {0}, RemainingDataLength {1}", transferPackage.DataLength, transferPackage.RemainingDataLength);
                    BaseTestSite.Assert.AreEqual<uint>(
                        0,
                        transferPackage.DataOffset % 8,
                        "Received SMBDirect Data Transfer Message: DataOffset {0}, which should be 8-byte aligned", transferPackage.DataOffset);
                    #endregion

                    Array.Copy(transferPackage.Buffer, 0, readResponsePacket, offset, transferPackage.DataLength);
                    offset += (int)transferPackage.DataLength;

                    if (transferPackage.RemainingDataLength == 0)
                    {
                        break;
                    }
                }

                BaseTestSite.Assert.IsTrue(transferPackage.CreditsGranted > 0,
                    "SMBDirect Data Tranfer message with last credit should with CreditsGranted greater than 0");
                #region send empty message to grant credit
                status = NtStatus.STATUS_SUCCESS;
                do
                {
                    status = smbdAdapter.SmbdPostReceive();
                }
                while (status == NtStatus.STATUS_SUCCESS
                    && smbdAdapter.ClientConnection.ReceiveCredits < credits);

                // check receive queue before sending
                BaseTestSite.Assert.AreEqual<int>(
                        0,
                        smbdAdapter.ReceiveEntryInQueue,
                        "No message should be sent from server");

                smbdAdapter.SmbdSendDataTransferMessage(
                    smbdAdapter.ClientConnection.SendCreditTarget,
                    (ushort)smbdAdapter.ClientConnection.ReceiveCredits,
                    SmbdDataTransfer_Flags.NONE,
                    0,
                    0,
                    0,
                    new byte[0]);

                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Grant {0} credits to peer, current Connection.ReceiveCredits is {0}", smbdAdapter.ClientConnection.ReceiveCredits);

                #endregion
            }

            #region Decode SMB2 READ response and validate
            object endpoint = new object();
            int consumedLength;
            int expectedLength;
            StackPacket[] stackPackets = smbdAdapter.Decoder.Smb2DecodePacketCallback(
                endpoint,
                readResponsePacket,
                out consumedLength,
                out expectedLength);
            Smb2ReadResponsePacket package = (Smb2ReadResponsePacket)stackPackets[0];

            READ_Response readResponse = package.PayLoad;
            BaseTestSite.Assert.AreEqual<NtStatus>(
                NtStatus.STATUS_SUCCESS,
                (NtStatus)package.Header.Status,
                "Status of SMB2 Read File is {0}", (NtStatus)package.Header.Status);
            #endregion
        }

        #endregion
    }
}
