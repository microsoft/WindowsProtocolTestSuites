// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.Smbd.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smbd;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;


namespace Microsoft.Protocol.TestSuites.Smbd.TestSuite
{
    [TestClass]
    public class SmbdNegotiate : SmbdTestBase
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
        [TestCategory("SmbdNegotiate")]
        [Description("Verify SMBD negotiate request is working as expected using accepted values.")]
        public void BVT_SmbdNegotiate_Basic()
        {
            const ushort CREDIT_REQUESTED = 10;
            const uint MAX_SEND_SIZE = 1024;
            const uint MAX_RECEIVE_SIZE = 1024;
            const uint MAX_FRAGMENT_SIZE = 131072;

            // test purpose: Verify SMBD negotiate request is working as expected using accepted values.
            SmbdNegotiateResponse response;
            BasicNegotiate(
                CREDIT_REQUESTED,
                MAX_SEND_SIZE,
                MAX_RECEIVE_SIZE,
                MAX_FRAGMENT_SIZE,
                out response);
        }

        [TestMethod()]
        [TestCategory("SmbdNegotiate")]
        [Description("Verify SMBD negotiate will be successful when PreferredSendSize field is zero.")]
        public void SmbdNegotiate_PreferredSendSizeWithZero()
        {
            const ushort CREDIT_REQUESTED = 10;
            const uint PREFERRED_SEND_SIZE = 0;
            const uint MAX_RECEIVE_SIZE = 1024;
            const uint MAX_FRAGMENT_SIZE = 131072;
            SmbdNegotiateResponse response;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "SMBD negotiate with PreferredSendSize field set to zero.");
            BasicNegotiate(
                CREDIT_REQUESTED,
                PREFERRED_SEND_SIZE,
                MAX_RECEIVE_SIZE,
                MAX_FRAGMENT_SIZE,
                out response);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify the response.MaxReceiveSize is " + SmbdConnection.FLOOR_MAX_RECEIVE_SIZE);
            BaseTestSite.Assert.AreEqual<uint>(
                SmbdConnection.FLOOR_MAX_RECEIVE_SIZE,
                response.MaxReceiveSize,
                "MaxReceiveSize is {0}", response.MaxReceiveSize);
        }

        [TestMethod()]
        [TestCategory("SmbdNegotiate")]
        [Description("Verify SMBD negotiate will be successful when PreferredSendSize field is less than 128 bytes.")]
        public void SmbdNegotiate_PreferredSendSizeLessThan128()
        {
            const ushort CREDIT_REQUESTED = 10;
            const uint ExpectedSendSize = 128;
            const uint MAX_RECEIVE_SIZE = 1024;
            const uint MAX_FRAGMENT_SIZE = 131072;
            uint preferredSendSize;
            SmbdNegotiateResponse response;
            
            preferredSendSize = ExpectedSendSize - 1;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Scenario 1: SMBD negotiate with preferredSendSize set to " + preferredSendSize);
            BasicNegotiate(
                CREDIT_REQUESTED,
                preferredSendSize,
                MAX_RECEIVE_SIZE,
                MAX_FRAGMENT_SIZE,
                out response);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify the response.MaxReceiveSize is " + SmbdConnection.FLOOR_MAX_RECEIVE_SIZE);
            BaseTestSite.Assert.AreEqual<uint>(
                SmbdConnection.FLOOR_MAX_RECEIVE_SIZE,
                response.MaxReceiveSize,
                "MaxReceiveSize is {0}", response.MaxReceiveSize);

            preferredSendSize = ExpectedSendSize;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Scenario 1: SMBD negotiate with preferredSendSize set to " + preferredSendSize);
            BasicNegotiate(
               CREDIT_REQUESTED,
               preferredSendSize,
               MAX_RECEIVE_SIZE,
               MAX_FRAGMENT_SIZE,
               out response);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify the response.MaxReceiveSize is " + SmbdConnection.FLOOR_MAX_RECEIVE_SIZE);
            BaseTestSite.Assert.AreEqual<uint>(
                SmbdConnection.FLOOR_MAX_RECEIVE_SIZE,
                response.MaxReceiveSize,
                "MaxReceiveSize is {0}", response.MaxReceiveSize);
        }

        [TestMethod()]
        [TestCategory("SmbdNegotiate")]
        [Description("Verify the server will terminate the connection when PreferredSendSize field is maximum value of unsigned integer.")]
        public void SmbdNegotiate_PreferredSendSizeMaxValue()
        {
            const ushort CREDIT_REQUESTED = 10;
            const uint MAX_RECEIVE_SIZE = 1024;
            const uint MAX_FRAGMENT_SIZE = 131072;
            uint preferredSendSize;
            SmbdNegotiateResponse response;

            if (smbdAdapter.TestConfig.Platform == Platform.WindowsServer2012R2 || smbdAdapter.TestConfig.Platform == Platform.WindowsServer2016)
            {
                // Windows Server 2012 R2, Windows Server 2016, and Windows Server operating system fail the request 
                // with STATUS_INSUFFICIENT_RESOURCES if the PreferredSendSize field is greater than 8136.
                preferredSendSize = 8136;
            }
            else
            {
                // uint max value
                preferredSendSize = uint.MaxValue;
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "SMBD negotiate with PreferredSendSize field set to the max value " + preferredSendSize);
            BasicNegotiate(
               CREDIT_REQUESTED,
               preferredSendSize,
               MAX_RECEIVE_SIZE,
               MAX_FRAGMENT_SIZE,
               out response);
        }


        [TestMethod()]
        [TestCategory("SmbdNegotiate")]
        [Description("Verify server can receive SMBD negotiate request with 512 bytes, 20 bytes of SMBD Negotiate request and 492 redundancy bytes.")]
        public void SmbdNegotiate_Redundancy()
        {
            const ushort CREDIT_REQUESTED = 10;
            const uint MAX_SEND_SIZE = 1024;
            const uint MAX_RECEIVE_SIZE = 1024;
            const uint MAX_FRAGMENT_SIZE = 131072;
            const int NEGOTIATE_SIZE = 512; // it is the size of receive size after server accept the connection

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Connect to server over RDMA.");
            NtStatus status = smbdAdapter.ConnectToServerOverRDMA();
            BaseTestSite.Assert.AreEqual<NtStatus>(status, NtStatus.STATUS_SUCCESS, "Status of SMBD connection is {0}.", status);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Create SMBD Negotiate request.");
            SmbdNegotiateRequest request = new SmbdNegotiateRequest();
            request.MinVersion = SmbdVersion.V1;
            request.MaxVersion = SmbdVersion.V1;
            request.Reserved = 0;
            request.CreditsRequested = CREDIT_REQUESTED;
            request.PreferredSendSize = MAX_SEND_SIZE;
            request.MaxReceiveSize = MAX_RECEIVE_SIZE;
            request.MaxFragmentedSize = MAX_FRAGMENT_SIZE;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Add 492 redundancy bytes.");
            byte[] requestBytes = TypeMarshal.ToBytes<SmbdNegotiateRequest>(request);
            byte[] allbytes = new byte[NEGOTIATE_SIZE];
            Array.Copy(requestBytes, allbytes, requestBytes.Length);

            // post receive 
            status = smbdAdapter.ClientConnection.Endpoint.PostReceive(NEGOTIATE_SIZE);
            BaseTestSite.Assert.AreEqual<NtStatus>(status, NtStatus.STATUS_SUCCESS, "Status of Post Receive is {0}.", status);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send SMBD negotiate request.");
            status = smbdAdapter.SendDataOverRdma(allbytes);
            BaseTestSite.Assert.AreEqual<NtStatus>(status, NtStatus.STATUS_SUCCESS, "Status of Send SMBD negotiate is {0}.", status);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Receive and verify NEGOTIATE response.");
            byte[] responseData;
            smbdAdapter.ReceiveDataOverRdma(
                TimeSpan.FromSeconds(SmbdConnection.ACTIVE_NEGOTIATION_TIMEOUT),
                out responseData);
            SmbdNegotiateResponse response = TypeMarshal.ToStruct<SmbdNegotiateResponse>(responseData);
            smbdAdapter.DisconnectRdma();

            NegotiateBasicChecker(
                response,
                CREDIT_REQUESTED,
                MAX_RECEIVE_SIZE,
                MAX_SEND_SIZE,
                MAX_FRAGMENT_SIZE);
        }

        [TestMethod()]
        [TestCategory("SmbdNegotiate")]
        [Description("Verify server is still available if client disconnect RDMA after SMBD negotiate")]
        public void SmbdNegotiate_DisconnectAfterNegotiate()
        {
            const ushort CREDIT_REQUESTED = 10;
            const uint MAX_SEND_SIZE = 1024;
            const uint MAX_RECEIVE_SIZE = 1024;
            const uint MAX_FRAGMENT_SIZE = 131072;

            SmbdNegotiateResponse response;
            BasicNegotiate(
                CREDIT_REQUESTED,
                MAX_SEND_SIZE,
                MAX_RECEIVE_SIZE,
                MAX_FRAGMENT_SIZE,
                out response);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Disconnect from server.");
            smbdAdapter.DisconnectRdma();

            // check server does not crash
            // Connect to server over RDMA, and negotiate
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server is still available after disconnect RDMA after SMBD negotiate.");
            NtStatus ret = smbdAdapter.ConnectToServerOverRDMA();
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, ret, "Status of SMBD connection is {0}", ret);
        }

        [TestMethod()]
        [TestCategory("SmbdNegotiate")]
        [Description("Verify server will terminate the connection if received negotiate message is less than 20 bytes.")]
        public void SmbdNegotiate_UncompletedMessage()
        {
            const ushort CREDIT_REQUESTED = 10;
            const uint MAX_SEND_SIZE = 1024;
            const uint MAX_RECEIVE_SIZE = 1024;
            const uint MAX_FRAGMENT_SIZE = 131072;

            #region Create a negotiate message, Sends only 19 bytes of negotiate message to peer
            
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Connect to server over RDMA");
            NtStatus status = smbdAdapter.ConnectToServerOverRDMA();
            BaseTestSite.Assert.AreEqual<NtStatus>(status, NtStatus.STATUS_SUCCESS, "Status of SMBD connection is {0}", status);

            // Create negotiate message
            SmbdNegotiateRequest smbdRequest = new SmbdNegotiateRequest();
            smbdRequest.MinVersion = SmbdVersion.V1;
            smbdRequest.MaxVersion = SmbdVersion.V1;
            smbdRequest.Reserved = 0;
            smbdRequest.CreditsRequested = CREDIT_REQUESTED;
            smbdRequest.PreferredSendSize = MAX_SEND_SIZE;
            smbdRequest.MaxReceiveSize = MAX_RECEIVE_SIZE;
            smbdRequest.MaxFragmentedSize = MAX_FRAGMENT_SIZE;

            byte[] requestBytes = TypeMarshal.ToBytes<SmbdNegotiateRequest>(smbdRequest);
            byte[] uncompletedRequest = new byte[requestBytes.Length - 1];
            Array.Copy(requestBytes, uncompletedRequest, uncompletedRequest.Length);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send SMBD negotiate message with only 19 bytes.");
            status = smbdAdapter.SendDataOverRdma(uncompletedRequest);
            BaseTestSite.Assert.AreEqual<NtStatus>(status, NtStatus.STATUS_SUCCESS, "Status of Send SMBD negotiate {0}", status);

            // wait for connection to be terminated 
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server connection will be terminated.");
            smbdAdapter.WaitRdmaDisconnect();            
            BaseTestSite.Assert.IsFalse(smbdAdapter.ClientConnection.Endpoint.IsConnected, "Connection should be terminated.");
            #endregion

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Basic negotiate, sending the whole 20 bytes of SMBD negotiate message to peer, verify server returns STATUS_SUCCESS.");
            SmbdNegotiateResponse response;
            BasicNegotiate(
                CREDIT_REQUESTED,
                MAX_SEND_SIZE,
                MAX_RECEIVE_SIZE,
                MAX_FRAGMENT_SIZE,
                out response);
        }

        [TestMethod]
        [TestCategory("SmbdNegotiate")]
        [Description("Verify server responses with STATUS_NOT_SUPPORTED when MinVersion is grater than 0x0100 which should be included in the range.")]
        public void SmbdNegotiate_VersionRangeNotCover0x0100_LessThanMinVersion()
        {
            Negotiate_Version(
                (SmbdVersion)(SmbdVersion.V1 + 1),
                (SmbdVersion)ushort.MaxValue);
        }

        [TestMethod]
        [TestCategory("SmbdNegotiate")]
        [Description("Verify server responses with STATUS_NOT_SUPPORTED when MaxVersion is less than 0x0100 which should be included in the range.")]
        public void SmbdNegotiate_VersionRangeNotCover0x0100_LargerThanMaxVersion()
        {
            Negotiate_Version(
                (SmbdVersion)ushort.MinValue,
                (SmbdVersion)(SmbdVersion.V1 - 1));
        }

        [TestMethod]
        [TestCategory("SmbdNegotiate")]
        [Description("Verify server responses with STATUS_SUCCESS when MinVersion equals to 0x0100 which should be included in the range.")]
        public void SmbdNegotiate_VersionRangeCover0x0100_EqualMinVersion()
        {
            Negotiate_Version(
                SmbdVersion.V1,
                (SmbdVersion)ushort.MaxValue);
        }

        [TestMethod]
        [TestCategory("SmbdNegotiate")]
        [Description("Verify server responses with STATUS_SUCCESS when MaxVersion equals to 0x0100 which should be included in the range.")]
        public void SmbdNegotiate_VersionRangeCover0x0100_EqualMaxVersion()
        {
            Negotiate_Version(
                (SmbdVersion)ushort.MinValue,
                SmbdVersion.V1);
        }

        [TestMethod]
        [TestCategory("SmbdNegotiate")]
        [Description("Verify server responses with STATUS_SUCCESS when MinVersion is less than 0x0100 while MaxVersion is grater than 0x0100.")]
        public void SmbdNegotiate_VersionRangeCover0x0100_InMiddle()
        {
            Negotiate_Version(
                (SmbdVersion)(SmbdVersion.V1 - 1),
                (SmbdVersion)(SmbdVersion.V1 + 1));
        }

        [TestMethod]
        [TestCategory("SmbdNegotiate")]
        [Description("Verify server responses with STATUS_SUCCESS when MinVersion and MaxVersion equal to 0x0100 which should be included in the range.")]
        public void SmbdNegotiate_VersionRangeCover0x0100_EqualMinVersionAndMaxVersion()
        {
            Negotiate_Version(
                SmbdVersion.V1,
                SmbdVersion.V1);
        }

        [TestMethod()]
        [TestCategory("SmbdNegotiate")]
        [Description("Verify the server will terminate the connection when CreditsRequested field is less than or equal to 0.")]
        public void SmbdNegotiate_NegativeParameter_CreditsRequested()
        {
            const ushort ExpectedCredit = 0;
            const uint MAX_SEND_SIZE = 1024;
            const uint MAX_RECEIVE_SIZE = 1024;
            const uint MAX_FRAGMENT_SIZE = 131072;
            ushort creditsRequested = ExpectedCredit;

            // CreditRequested = 0
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "SMBD negotiate with CreditRequested field set to 0.");
            NegativeNegotiate(
                creditsRequested,
                MAX_SEND_SIZE,
                MAX_RECEIVE_SIZE,
                MAX_FRAGMENT_SIZE);

            // CreditRequested = 1
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "SMBD negotiate with CreditRequested field set to 1.");
            creditsRequested = ExpectedCredit + 1;
            SmbdNegotiateResponse response;
            BasicNegotiate(
               creditsRequested,
               MAX_SEND_SIZE,
               MAX_RECEIVE_SIZE,
               MAX_FRAGMENT_SIZE,
               out response);

            // CreditRequested = ushort.MaxValue
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "SMBD negotiate with CreditRequested field set to ushort.MaxValue:" + ushort.MaxValue);
            creditsRequested = ushort.MaxValue;
            BasicNegotiate(
               creditsRequested,
               MAX_SEND_SIZE,
               MAX_RECEIVE_SIZE,
               MAX_FRAGMENT_SIZE,
               out response);
        }

        [TestMethod()]
        [TestCategory("SmbdNegotiate")]
        [Description("Verify the server will terminate the connection when MaxReceiveSize field is less than 128 bytes.")]
        public void SmbdNegotiate_NegativeParameter_MaxReceiveSize()
        {
            const ushort CREDIT_REQUESTED = 10;
            const uint MAX_SEND_SIZE = 1024;
            const uint ExpectedReceiveSize = 128;
            const uint MAX_FRAGMENT_SIZE = 131072;
            uint maxReceiveSize;
            SmbdNegotiateResponse response;

            // MaxReceiveSize = 127, the connection MUST be terminated
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "SMBD negotiate with MaxReceiveSize field set to 127, the connection MUST be terminated.");
            maxReceiveSize = ExpectedReceiveSize - 1;
            NegativeNegotiate(
                CREDIT_REQUESTED,
                MAX_SEND_SIZE,
                maxReceiveSize,
                MAX_FRAGMENT_SIZE);

            // MaxReceiveSize = 128(minimum value that can be accepted), the connection MUST be terminated
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "SMBD negotiate with MaxReceiveSize field set to 128(minimum value that can be accepted), the connection MUST be terminated.");
            maxReceiveSize = ExpectedReceiveSize;
            BasicNegotiate(
               CREDIT_REQUESTED,
               MAX_SEND_SIZE,
               maxReceiveSize,
               MAX_FRAGMENT_SIZE,
               out response);

            // MaxReceiveSize = uint.MaxValue, the connection MUST be terminated
            BaseTestSite.Log.Add(LogEntryKind.TestStep, string.Format("SMBD negotiate with MaxReceiveSize field set to uint.MaxValue {0}, the connection MUST be terminated.", uint.MaxValue));
            maxReceiveSize = uint.MaxValue;
            BasicNegotiate(
               CREDIT_REQUESTED,
               MAX_SEND_SIZE,
               maxReceiveSize,
               MAX_FRAGMENT_SIZE,
               out response);
        }

        [TestMethod()]
        [TestCategory("SmbdNegotiate")]
        [Description("Verify the server will terminate the connection when MaxFragmentedSize field is less than 131,072 bytes.")]
        public void SmbdNegotiate_NegativeParameter_MaxFragmentedSize()
        {
            const ushort CREDIT_REQUESTED = 10;
            const uint MAX_SEND_SIZE = 1024;
            const uint MAX_RECEIVE_SIZE = 1024;
            const uint ExpectedFragmentedSize = 131072;
            uint fragmentedSize;
            SmbdNegotiateResponse response;

            // MaxFragmentedSize = 131071, the connection MUST be terminated
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "SMBD negotiate with MaxFragmentedSize field set to 131071, the connection MUST be terminated.");
            fragmentedSize = ExpectedFragmentedSize - 1;
            NegativeNegotiate(
                CREDIT_REQUESTED,
                MAX_SEND_SIZE,
                MAX_RECEIVE_SIZE,
                fragmentedSize);

            // MaxFragmentedSize = 131072, Negotiate with STATUS_SUCCESS
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "SMBD negotiate with MaxFragmentedSize field set to 131072, Negotiate with STATUS_SUCCESS.");
            fragmentedSize = ExpectedFragmentedSize;
            BasicNegotiate(
               CREDIT_REQUESTED,
               MAX_SEND_SIZE,
               MAX_RECEIVE_SIZE,
               fragmentedSize,
               out response);

            // MaxFragmentedSize = uint.MaxValue, Negotiate with STATUS_SUCCESS
            BaseTestSite.Log.Add(LogEntryKind.TestStep, string.Format("SMBD negotiate with MaxFragmentedSize field set to uint.MaxValue {0}, Negotiate with STATUS_SUCCESS.", uint.MaxValue));
            fragmentedSize = uint.MaxValue;
            BasicNegotiate(
               CREDIT_REQUESTED,
               MAX_SEND_SIZE,
               MAX_RECEIVE_SIZE,
               fragmentedSize,
               out response);
        }

        [TestMethod()]
        [TestCategory("SmbdNegotiate")]
        [Description("Verify server will not terminate connection within 5 seconds after RDMA connection is established")]
        public void SmbdNegotiate_NegotiationTimer()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Connect to server over RDMA");
            NtStatus ret = smbdAdapter.ConnectToServerOverRDMA();
            BaseTestSite.Assert.AreEqual<NtStatus>(ret, NtStatus.STATUS_SUCCESS, "Status of SMBD connection is {0}", ret);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Wait 4 seconds (less than the connection timeout of 5 seconds).");
            Thread.Sleep(TimeSpan.FromSeconds(SmbdConnection.PASSIVE_NEGOTIATION_TIMEOUT - 1));

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send Negotiate request and receive the response.");
            NtStatus status = smbdAdapter.SmbdNegotiate();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server does not terminate the connection.");
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMBD negotiate", status);
        }

        [TestMethod()]
        [TestCategory("SmbdNegotiate")]
        [Description("Verify server will terminate connection after 5 seconds when RDMA connection is established.")]
        public void SmbdNegotiate_NegotiationTimer_Timeout()
        {
            const ushort CREDIT_REQUESTED = 10;
            const uint MAX_SEND_SIZE = 1024;
            const uint MAX_RECEIVE_SIZE = 1024;
            const uint MAX_FRAGMENT_SIZE = 131072;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Connect to server over RDMA");
            NtStatus ret = smbdAdapter.ConnectToServerOverRDMA();
            BaseTestSite.Assert.AreEqual<NtStatus>(ret, NtStatus.STATUS_SUCCESS, "Status of SMBD connection is {0}", ret);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Wait 5 seconds for timeout.");
            Thread.Sleep(TimeSpan.FromSeconds(SmbdConnection.PASSIVE_NEGOTIATION_TIMEOUT));
            // wait for connection to be terminated 
            smbdAdapter.WaitRdmaDisconnect();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server connection will be terminated.");
            BaseTestSite.Assert.IsFalse(smbdAdapter.ClientConnection.Endpoint.IsConnected, "Connection should be terminated.");

            // verify that server is still available
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server is still available, not crashed.");
            SmbdNegotiateResponse response;
            BasicNegotiate(
                CREDIT_REQUESTED,
                MAX_SEND_SIZE,
                MAX_RECEIVE_SIZE,
                MAX_FRAGMENT_SIZE,
                out response);
        }
        #endregion

        #region Test Common Method

        public void BasicNegotiate(
            ushort creditsRequested,
            uint preferredSendSize,
            uint maxReceiveSize,
            uint maxFragmentedSize,
            out SmbdNegotiateResponse response)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Connect to server over RDMA");
            NtStatus status = smbdAdapter.ConnectToServerOverRDMA();
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMBD connection is {0}", status);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "SMBD Negotiate");
            status = smbdAdapter.SmbdNegotiate(
                creditsRequested,
                (ushort)smbdAdapter.TestConfig.ReceiveCreditMax,
                preferredSendSize,
                maxReceiveSize,
                maxFragmentedSize,
                out response,
                SmbdVersion.V1,
                SmbdVersion.V1
                );
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMBD negotiate is {0}", status);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify SMBD negotiate response");
            NegotiateBasicChecker(
                response,
                creditsRequested,
                maxReceiveSize,
                preferredSendSize,
                maxFragmentedSize);
        }


        public void NegativeNegotiate(
            ushort creditsRequested,
            uint preferredSendSize,
            uint maxReceiveSize,
            uint maxFragmentSize)
        {
            // Connect to server over RDMA, and negotiate
            NtStatus status = smbdAdapter.ConnectToServerOverRDMA();
            BaseTestSite.Assert.AreEqual<NtStatus>(status, NtStatus.STATUS_SUCCESS, "Status of SMBD connection is {0}", status);

            // send negotiate message
            SmbdNegotiateRequest smbdRequest = new SmbdNegotiateRequest();
            smbdRequest.MinVersion = SmbdVersion.V1;
            smbdRequest.MaxVersion = SmbdVersion.V1;
            smbdRequest.Reserved = 0;
            smbdRequest.CreditsRequested = creditsRequested;
            smbdRequest.PreferredSendSize = preferredSendSize;
            smbdRequest.MaxReceiveSize = maxReceiveSize;
            smbdRequest.MaxFragmentedSize = maxFragmentSize;

            byte[] requestBytes = TypeMarshal.ToBytes<SmbdNegotiateRequest>(smbdRequest);

            BaseTestSite.Log.Add(LogEntryKind.Debug,
                        @"Send Negotiate request with parameters:
                        MinVersion: {0},
                        MaxVersion: {1},
                        CreditsRequested: {2},
                        PreferredSendSize: {3},
                        MaxReceiveSize: {4},
                        MaxFragmentedSize: {5}
                        ",
                        smbdRequest.MinVersion,
                        smbdRequest.MaxVersion,
                        smbdRequest.CreditsRequested,
                        smbdRequest.PreferredSendSize,
                        smbdRequest.MaxReceiveSize,
                        smbdRequest.MaxFragmentedSize);
            // send message
            status = smbdAdapter.SendDataOverRdma(requestBytes);
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of Send SMBD negotiate {0}", status);

            // wait for connection to be terminated 
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server connection will be terminated.");
            smbdAdapter.WaitRdmaDisconnect();
            BaseTestSite.Assert.IsFalse(smbdAdapter.ClientConnection.Endpoint.IsConnected, "Connection MUST be terminated, but it is not terminated.");
        }

        public void Negotiate_Version(
            SmbdVersion minVer,
            SmbdVersion maxVer)
        {
            const ushort CREDIT_REQUESTED = 10;
            const ushort RECEIVE_CREDIT_MAX = 10;
            const uint MAX_SEND_SIZE = 1024;
            const uint MAX_RECEIVE_SIZE = 1024;
            const uint MAX_FRAGMENT_SIZE = 131072;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Connect to server over RDMA");
            NtStatus status = smbdAdapter.ConnectToServerOverRDMA();
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMBD connection is {0}", status);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, string.Format("SMBD Negotiate, MinVersion: {0}; MaxVersion: {1}", minVer.ToString(), maxVer.ToString()));
            SmbdNegotiateResponse response;
            status = smbdAdapter.SmbdNegotiate(
                CREDIT_REQUESTED,
                RECEIVE_CREDIT_MAX,
                MAX_SEND_SIZE,
                MAX_RECEIVE_SIZE,
                MAX_FRAGMENT_SIZE,
                out response,
                minVer,
                maxVer
                );

            // try to negotiate on SMB2
            string fileName = CreateRandomFileName();

            // Windows Server 2012 fails the Negotiate Request Message with STATUS_NOT_SUPPORTED if MinVersion or MaxVersion is not 0x0100.
            if (smbdAdapter.TestConfig.Platform == Platform.WindowsServer2012)
            {
                if (minVer == SmbdVersion.V1
                    && maxVer == SmbdVersion.V1)
                {
                    BaseTestSite.Assert.AreEqual<NtStatus>(
                        NtStatus.STATUS_SUCCESS,
                        status,
                        "SMBD Negotiate should succeed");

                    BaseTestSite.Log.Add(LogEntryKind.TestStep, "Establish SMB2 connection and open file " + fileName);
                    status = smbdAdapter.Smb2EstablishSessionAndOpenFile(fileName);
                    BaseTestSite.Assert.AreEqual<NtStatus>(
                            NtStatus.STATUS_SUCCESS,
                            status,
                            "Smb2EstablishSessionAndOpenFile should success");
                }
                else
                {
                    BaseTestSite.Assert.AreEqual<NtStatus>(
                        NtStatus.STATUS_NOT_SUPPORTED,
                        status,
                        "Status of SMBD negotiate {0}", status);

                    try
                    {
                        BaseTestSite.Log.Add(LogEntryKind.TestStep, "Try to establish SMB2 connection and open file " + fileName);
                        status = smbdAdapter.Smb2EstablishSessionAndOpenFile(fileName);
                        BaseTestSite.Assert.AreNotEqual<NtStatus>(
                            NtStatus.STATUS_SUCCESS,
                            status,
                            "Status of Smb2EstablishSessionAndOpenFile is {0}", status);
                    }
                    catch (TimeoutException e)
                    {
                        BaseTestSite.Assert.Pass("Cannot send or receive packets from peer. \nException: {0}\n{1}", e.Message, e.StackTrace);
                    }
                }
            }
            else
            {
                if (minVer <= SmbdVersion.V1
                    && maxVer >= SmbdVersion.V1)
                {
                    BaseTestSite.Assert.AreEqual<NtStatus>(
                        NtStatus.STATUS_SUCCESS,
                        status,
                        "SMBD Negotiate should succeed");

                    BaseTestSite.Log.Add(LogEntryKind.TestStep, "Establish SMB2 connection and open file " + fileName);
                    status = smbdAdapter.Smb2EstablishSessionAndOpenFile(fileName);
                    BaseTestSite.Assert.AreEqual<NtStatus>(
                            NtStatus.STATUS_SUCCESS,
                            status,
                            "Smb2EstablishSessionAndOpenFile should success");
                }
                else
                {
                    BaseTestSite.Assert.AreEqual<NtStatus>(
                        NtStatus.STATUS_NOT_SUPPORTED,
                        status,
                        "Status of SMBD negotiate {0}", status);

                    try
                    {
                        BaseTestSite.Log.Add(LogEntryKind.TestStep, "Try to establish SMB2 connection and open file " + fileName);
                        status = smbdAdapter.Smb2EstablishSessionAndOpenFile(fileName);
                        BaseTestSite.Assert.AreNotEqual<NtStatus>(
                            NtStatus.STATUS_SUCCESS,
                            status,
                            "Status of Smb2EstablishSessionAndOpenFile is {0}", status);
                    }
                    catch (TimeoutException e)
                    {
                        BaseTestSite.Assert.Pass("Cannot send or receive packets from peer. \nException: {0}\n{1}", e.Message, e.StackTrace);
                    }
                }
            }
        }

        public void NegotiateBasicChecker(
            SmbdNegotiateResponse response,
            ushort creditsRequested,
            uint maxReceiveSize,
            uint preferredSendSize,
            uint maxFragmentedSize)
        {
            // verify the response
            BaseTestSite.Assert.AreEqual<SmbdVersion>(
                SmbdVersion.V1,
                response.MinVersion,
                "MinVersion in negotiate response is {0}", response.MinVersion);
            BaseTestSite.Assert.AreEqual<SmbdVersion>(
                SmbdVersion.V1,
                response.MaxVersion,
                "MaxVersion in negotiate response is {0}", response.MaxVersion);
            BaseTestSite.Assert.AreEqual<SmbdVersion>(
                SmbdVersion.V1,
                response.NegotiatedVersion,
                "NegotiatedVersion in negotiate response is {0}", response.NegotiatedVersion);
            BaseTestSite.Assert.AreEqual<ushort>(
                0,
                response.Reserved,
                "Reserved in negotiate response is {0}", response.Reserved);
            BaseTestSite.Assert.IsTrue(
                response.CreditsGranted > 0 && response.CreditsGranted <= creditsRequested,
                "CreditsGranted in negotiate response is {0}", response.CreditsGranted);
            BaseTestSite.Assert.IsTrue(
                response.CreditsRequested > 0,
                "CreditsRequested in negotiate response is {0}", response.CreditsRequested);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                response.Status,
                "Status in negotiate response is {0}", response.Status);
            BaseTestSite.Assert.IsTrue(
                response.MaxReadWriteSize >= SmbdConnection.FLOOR_MAX_READ_WRITE_SIZE,
                "MaxReadWriteSize in negotiate response is {0}", response.MaxReadWriteSize);
            if (preferredSendSize >= SmbdConnection.FLOOR_MAX_RECEIVE_SIZE)
            {
                // sever MUST set Connection.MaxReceiveSize to the smaller(Connection.MaxReceiveSize, PreferredSendSize)
                // If the result is smaller than 128, then Connection.MaxReceiveSize MUST be set to 128.
                BaseTestSite.Assert.IsTrue(
                    response.MaxReceiveSize >= SmbdConnection.FLOOR_MAX_RECEIVE_SIZE && response.MaxReceiveSize <= preferredSendSize,
                    "MaxReceiveSize in negotiate response is {0}", response.MaxReceiveSize);
            }
            // server MUST set Connection.MaxSendSize to the smaller(Connection.MaxSendSize, MaxReceiveSize).
            BaseTestSite.Assert.IsTrue(
                response.PreferredSendSize >= SmbdConnection.FLOOR_MAX_RECEIVE_SIZE && response.PreferredSendSize <= maxReceiveSize,
                "PreferredSendSize in negotiate response is {0}", response.PreferredSendSize);
            BaseTestSite.Assert.IsTrue(
                response.MaxFragmentedSize >= SmbdConnection.FLOOR_MAX_FRAGMENTED_SIZE,
                "MaxFragmentedSize in negotiate response is {0}", response.MaxFragmentedSize);
        }

        #endregion
    }
}
