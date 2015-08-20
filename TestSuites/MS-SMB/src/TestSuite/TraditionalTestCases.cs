// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using StackCifs = Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;
using StackSmb = Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb;
//using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rap;
using System.Collections.Generic;

namespace Microsoft.Protocol.TestSuites.Smb
{
    /// <summary>
    /// Summary description for TraditionalTestCases
    /// </summary>
    public partial class TestSuite : TestClassBase
    {
        /// <summary>
        /// Test case of verifying NT_TRANSACT_COPY_CHUNK_List request.
        /// </summary>
        [TestMethod]
        [TestCategory("Traditional")]
        public void TraditionalTestCase_IgnoreFields_CopyChunk_05()
        {
            #region Connect to the specified server

            smbClientStack.Connect(serverName, serverPort, ipVersion, bufferSize);

            #endregion

            #region Send the Negotiate request

            // Create a SMB_COM_NEGOTIATE request.
            SmbNegotiateRequestPacket negotiateRequest =
                smbClientStack.CreateNegotiateRequest(
                StackSmb.SignState.NONE,
                new string[] {
                    DialectNameString.PCNET1,
                    DialectNameString.LANMAN10,
                    DialectNameString.WFW10,
                    DialectNameString.LANMAN12,
                    DialectNameString.LANMAN21,
                    DialectNameString.NTLANMAN
                });

            // Send the SMB_COM_NEGOTIATE request and expect the response in timeout milliseconds.
            smbClientStack.SendPacket(negotiateRequest);
            StackPacket response = smbClientStack.ExpectPacket(timeout);

            // Check whether server return a response.
            Site.Assert.IsNotNull(
                response,
                "SMB_COM_NEGOTIATE response should not be null.");

            // Check whether server returns a SMB_COM_NEGOTIATE response.
            Site.Assert.IsInstanceOfType(
                response,
                typeof(SmbNegotiateResponsePacket),
                "SMB_COM_NEGOTIATE response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            SmbNegotiateResponsePacket negotiateResponse = (SmbNegotiateResponsePacket)response;
            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                negotiateRequest.SmbHeader.Status,
                "SMB_COM_NEGOTIATE response status should be SUCCESS.");

            #endregion

            #region Send the first SMB_COM_SESSION_SETUP_ANDX  Request

            SmbSecurityPackage smbSecurityPackage = (SmbSecurityPackage)Enum.Parse(
                typeof(SmbSecurityPackage),
                Site.Properties["SmbSecurityPackageType"] as string,
                true);

            // Create the first SMB_COM_SESSION_SETUP_ANDX request.
            SmbSessionSetupAndxRequestPacket sessionSetupAndxRequest =
                smbClientStack.CreateFirstSessionSetupRequest(
                smbSecurityPackage,
                serverName,
                domainName,
                userName,
                password);

            // Send the first SMB_COM_SESSION_SETUP_ANDX request and expect the response in timeout milliseconds.
            smbClientStack.SendPacket(sessionSetupAndxRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server return a response.
            Site.Assert.IsNotNull(
                response,
                "SMB_COM_SESSION_SETUP_ANDX response should not be null.");

            // Check whether server returns a the SMB_COM_SESSION_SETUP_ANDX response.
            Site.Assert.IsInstanceOfType(
                response,
                typeof(SmbSessionSetupAndxResponsePacket),
                "SMB_COM_SESSION_SETUP_ANDX response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            // If SMB SecurityPackage type is NTLM, the expected SUCCESS response status is STATUS_MORE_PROCESSING_REQUIRED,
            // else if SMB SecurityPackage type is Kerberos, the expected SUCCESS response status is STATUS_SUCCESS.
            SmbSessionSetupAndxResponsePacket sessionSetupResponse = (SmbSessionSetupAndxResponsePacket)response;
            Site.Assert.IsTrue(
                (int)sessionSetupResponse.SmbHeader.Status == (int)SmbStatus.STATUS_MORE_PROCESSING_REQUIRED ||
                (int)sessionSetupResponse.SmbHeader.Status == (int)SmbStatus.STATUS_SUCCESS,
                "SMB_COM_SESSION_SETUP_ANDX response status should be SUCCESS.");

            #endregion

            #region Send the second SMB_COM_SESSION_SETUP_ANDX  request

            // Create the second SMB_COM_SESSION_SETUP_ANDX request.
            ushort sessionUid = sessionSetupResponse.SmbHeader.Uid;

            if ((int)sessionSetupResponse.SmbHeader.Status == (int)SmbStatus.STATUS_MORE_PROCESSING_REQUIRED)
            {
                SmbSessionSetupAndxRequestPacket secondSessionSetupRequest =
                    smbClientStack.CreateSecondSessionSetupRequest(sessionUid, smbSecurityPackage);

                // Send the second SMB_COM_SESSION_SETUP_ANDX request and expect the response in timeout milliseconds.
                smbClientStack.SendPacket(secondSessionSetupRequest);
                response = smbClientStack.ExpectPacket(timeout);

                // Check whether server return a response.
                Site.Assert.IsNotNull(
                    response,
                    "SMB_COM_SESSION_SETUP_ANDX response should not be null.");

                // Check whether server returns a the SMB_COM_SESSION_SETUP_ANDX response.
                Site.Assert.IsInstanceOfType(
                    response,
                    typeof(SmbSessionSetupAndxResponsePacket),
                    "SMB_COM_SESSION_SETUP_ANDX response should be received.");

                // Check the response validity by verifying the Status field in the SMB Header packet.
                sessionSetupResponse = (SmbSessionSetupAndxResponsePacket)response;
                Site.Assert.AreEqual<uint>(
                    (uint)SmbStatus.STATUS_SUCCESS,
                    sessionSetupResponse.SmbHeader.Status,
                    "SMB_COM_SESSION_SETUP_ANDX response status should be SUCCESS.");
            }

            #endregion

            #region Send the SMB_COM_TREE_CONNECT_ANDX request

            string path = Site.Properties["SutNtfsShare1FullName"];

            SmbTreeConnectAndxRequestPacket treeconnectRequest =
            smbClientStack.CreateTreeConnectRequest(sessionUid, path);

            smbClientStack.SendPacket(treeconnectRequest);
            response = smbClientStack.ExpectPacket(timeout);

            SmbTreeConnectAndxResponsePacket treeConnectResponse = (SmbTreeConnectAndxResponsePacket)response;
            #endregion

            #region Send the NT_CREATE_ANDX request

            ushort treeId = treeConnectResponse.SmbHeader.Tid;
            string fileName = Site.Properties["SutShareTest1"];
            smbClientStack.Capability.Flag |= SmbHeader_Flags_Values.OPLOCK;

            SmbNtCreateAndxRequestPacket createRequest =
                smbClientStack.CreateCreateRequest(
                treeId,
                fileName,
                StackCifs.NtTransactDesiredAccess.FILE_READ_DATA
                | StackCifs.NtTransactDesiredAccess.FILE_READ_EA
                | StackCifs.NtTransactDesiredAccess.FILE_READ_ATTRIBUTES
                | StackCifs.NtTransactDesiredAccess.FILE_WRITE_DATA,
                StackCifs.SMB_EXT_FILE_ATTR.NONE,
                StackCifs.NtTransactShareAccess.FILE_SHARE_READ
                | StackCifs.NtTransactShareAccess.FILE_SHARE_DELETE,
                StackCifs.NtTransactCreateDisposition.FILE_OPEN_IF,
                NtTransactCreateOptions.FILE_OPEN_REPARSE_POINT
                | NtTransactCreateOptions.FILE_SEQUENTIAL_ONLY
                | NtTransactCreateOptions.FILE_NON_DIRECTORY_FILE,
                StackCifs.NtTransactImpersonationLevel.SEC_ANONYMOUS,
                CreateFlags.NT_CREATE_REQUEST_OPLOCK);

            smbClientStack.SendPacket(createRequest);
            response = smbClientStack.ExpectPacket(timeout);

            #endregion

            #region Send the NT_CREATE_ANDX request

            SmbNtCreateAndxResponsePacket createResponse = (SmbNtCreateAndxResponsePacket)response;
            ushort fileId1 = createResponse.SmbParameters.FID;
            uint offset = uint.Parse(Site.Properties["SmbTransportWriteOffset"].ToString());
            byte[] writeData = new byte[17000];

            for (int i = 0; i < writeData.Length; i++)
            {
                writeData[i] = (byte)'a';
            }

            SmbWriteAndxRequestPacket writeRequest = smbClientStack.CreateWriteRequest(fileId1, offset, writeData);

            smbClientStack.SendPacket(writeRequest);
            response = smbClientStack.ExpectPacket(timeout);

            #endregion

            #region Send the WRITE_ANDX request

            SmbWriteAndxResponsePacket writeAndxResponsePacket = response as SmbWriteAndxResponsePacket;
            string fileName2 = Site.Properties["SutShareTest2"];

            SmbNtCreateAndxRequestPacket createSecondRequest =
                smbClientStack.CreateCreateRequest(
                treeId,
                fileName2,
                StackCifs.NtTransactDesiredAccess.FILE_READ_DATA
                | StackCifs.NtTransactDesiredAccess.FILE_WRITE_DATA
                | StackCifs.NtTransactDesiredAccess.FILE_APPEND_DATA,
                StackCifs.SMB_EXT_FILE_ATTR.NONE,
                StackCifs.NtTransactShareAccess.FILE_SHARE_WRITE
                | StackCifs.NtTransactShareAccess.FILE_SHARE_DELETE,
                StackCifs.NtTransactCreateDisposition.FILE_OPEN_IF,
                NtTransactCreateOptions.FILE_OPEN_REPARSE_POINT
                | NtTransactCreateOptions.FILE_SEQUENTIAL_ONLY
                | NtTransactCreateOptions.FILE_NON_DIRECTORY_FILE,
                StackCifs.NtTransactImpersonationLevel.SEC_ANONYMOUS,
                CreateFlags.NT_CREATE_REQUEST_OPBATCH);

            smbClientStack.SendPacket(createSecondRequest);
            response = smbClientStack.ExpectPacket(timeout);

            #endregion

            #region Send the NT_CREATE_ANDX

            SmbNtCreateAndxResponsePacket createSecondResponse = (SmbNtCreateAndxResponsePacket)response;
            ushort fileId2 = createSecondResponse.SmbParameters.FID;

            SmbNtTransFsctlSrvRequestResumeKeyRequestPacket nTTransIOCtlRequestResumeKeyRequest =
                smbClientStack.CreateNTTransIOCtlRequestResumeKeyRequest(fileId1, true, 0);

            StackCifs.SmbHeader smbHeader = nTTransIOCtlRequestResumeKeyRequest.SmbHeader;
            smbHeader.Uid = sessionUid;
            smbHeader.Tid = treeId;
            nTTransIOCtlRequestResumeKeyRequest.SmbHeader = smbHeader;

            smbClientStack.SendPacket(nTTransIOCtlRequestResumeKeyRequest);
            response = smbClientStack.ExpectPacket(timeout);

            SmbNtTransFsctlSrvRequestResumeKeyResponsePacket nTTransIOCtlRequestResumeKeyResponse =
                (SmbNtTransFsctlSrvRequestResumeKeyResponsePacket)response;

            #endregion

            #region Send the NT_TRANS_IO_CTL request

            bool isFsctl = true;
            byte isFlags = (byte)0;
            byte[] copychunkResumeKey = nTTransIOCtlRequestResumeKeyResponse.NtTransData.ResumeKey;

            nTTransIOCtlRequestResumeKeyRequest =
                smbClientStack.CreateNTTransIOCtlRequestResumeKeyRequest(fileId1, true, 0);

            smbHeader = nTTransIOCtlRequestResumeKeyRequest.SmbHeader;
            smbHeader.Uid = sessionUid;
            smbHeader.Tid = treeId;
            nTTransIOCtlRequestResumeKeyRequest.SmbHeader = smbHeader;

            smbClientStack.SendPacket(nTTransIOCtlRequestResumeKeyRequest);
            response = smbClientStack.ExpectPacket(timeout);

            #endregion

            #region Capture requirement r9074

            nTTransIOCtlRequestResumeKeyResponse =
                (SmbNtTransFsctlSrvRequestResumeKeyResponsePacket)response;
            byte[] copychunkResumeKey2 = nTTransIOCtlRequestResumeKeyResponse.NtTransData.ResumeKey;

            // Compare copychunkResumeKey and copychunkResumeKey2, if same, capture 9074
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-SMB_R9074");

            //
            // Verify MS-SMB requirement: MS-SMB_R9074
            //
            bool isVerifyR9074 = CompareArrayEquals(copychunkResumeKey, copychunkResumeKey2);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR9074,
                9074,
                @"[In Copychunk Resume Key Generation] The generation of Copychunk Resume Keys MUST satisfy the 
                following constraints:The Copychunk Resume Key MUST remain valid for the lifetime of the open file 
                on the server.");

            #endregion

            #region Send the NT_TRANSACT_COPY_CHUNK_List request

            NT_TRANSACT_COPY_CHUNK_List list = new NT_TRANSACT_COPY_CHUNK_List();
            list.Length = (uint)64;
            list.SourceOffset = (ulong)0;
            list.DestinationOffset = (ulong)0;

            SmbNtTransFsctlSrvCopyChunkRequestPacket nTTransIOCtlCopyChunkRequest =
                smbClientStack.CreateNTTransIOCtlCopyChunkRequest(fileId2, isFsctl, isFlags, copychunkResumeKey, list);

            smbClientStack.SendPacket(nTTransIOCtlCopyChunkRequest);
            response = smbClientStack.ExpectPacket(timeout);

            #endregion

            #region Send the NT_TRANSACT_COPY_CHUNK_Request_NT_Trans_Data request

            SmbNtTransFsctlSrvCopyChunkResponsePacket nTTransIOCtlCopyChunkResponse1 =
                (SmbNtTransFsctlSrvCopyChunkResponsePacket)response;

            list.Length = (uint)64;
            list.SourceOffset = (ulong)0;
            list.DestinationOffset = (ulong)0;
            list.Reserved = 0xFFFF;

            nTTransIOCtlCopyChunkRequest =
                smbClientStack.CreateNTTransIOCtlCopyChunkRequest(fileId2, isFsctl, isFlags, copychunkResumeKey, list);
            NT_TRANSACT_COPY_CHUNK_Request_NT_Trans_Data TransData = nTTransIOCtlCopyChunkRequest.NtTransData;

            TransData.Unused = 0xFFFF;
            nTTransIOCtlCopyChunkRequest.NtTransData = TransData;

            smbClientStack.SendPacket(nTTransIOCtlCopyChunkRequest);
            response = smbClientStack.ExpectPacket(timeout);

            #endregion

            #region Capture requirements R109390

            SmbNtTransFsctlSrvCopyChunkResponsePacket nTTransIOCtlCopyChunkResponse2 =
                (SmbNtTransFsctlSrvCopyChunkResponsePacket)response;

            // Compare 2 copy chunckResponse
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-SMB_R109390");

            //
            // Verify MS-SMB requirement: MS-SMB_R109390
            //
            bool isVerifyR109390 = VerifyResponse(nTTransIOCtlCopyChunkResponse1, nTTransIOCtlCopyChunkResponse2);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR109390,
                109390,
                @"[In  Client Request Extensions]Reserved (4 bytes):
                reply is the same whether zero or non-zero is used this field.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-SMB_R109396");

            //
            // Verify MS-SMB requirement: MS-SMB_R109396
            //
            bool isVerifyR109396 = VerifyResponse(nTTransIOCtlCopyChunkResponse1, nTTransIOCtlCopyChunkResponse2);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR109396,
                109396,
                @"[In SRV_COPYCHUNK]Reserved (4 bytes):
                reply is the same whether zero or non-zero is used in this field.");

            #endregion

            #region Send the SMB_NT_TRANS_FSCTL_SRV request request

            nTTransIOCtlRequestResumeKeyRequest =
                smbClientStack.CreateNTTransIOCtlRequestResumeKeyRequest(fileId2, true, 0);

            smbHeader = nTTransIOCtlRequestResumeKeyRequest.SmbHeader;
            smbHeader.Uid = sessionUid;
            smbHeader.Tid = treeId;
            nTTransIOCtlRequestResumeKeyRequest.SmbHeader = smbHeader;

            smbClientStack.SendPacket(nTTransIOCtlRequestResumeKeyRequest);
            response = smbClientStack.ExpectPacket(timeout);

            #endregion

            #region Capture reuqirements R9073

            nTTransIOCtlRequestResumeKeyResponse =
                (SmbNtTransFsctlSrvRequestResumeKeyResponsePacket)response;

            byte[] copychunkResumeKey3 = nTTransIOCtlRequestResumeKeyResponse.NtTransData.ResumeKey;

            //
            // Add the comment information for debugging
            //
            Site.Log.Add(
                LogEntryKind.Comment,
                @"Verify MS-SMB_R9073");

            //
            // Verify MS-SMB requirement: MS-SMB_R9073
            //
            bool isVerifyR9073 = CompareArrayEquals(copychunkResumeKey, copychunkResumeKey3);

            Site.CaptureRequirementIfIsFalse(
                isVerifyR9073,
                9073,
                @"[In Copychunk Resume Key Generation]
                The generation of Copychunk Resume Keys MUST satisfy the following constraints:
                The Copychunk Resume Key MUST be unique on the SMB server for a given open file on a server.");

            #endregion

            #region Disconnect the tree, session and connection.

            // TreeDisconnect
            SmbTreeDisconnectRequestPacket treeDisconnectRequest =
                smbClientStack.CreateTreeDisconnectRequest(treeId);

            smbClientStack.SendPacket(treeDisconnectRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server return a response.
            Site.Assert.IsNotNull(
                response,
                "SMB_COM_TREE_DISCONNECT response should not be null.");

            Site.Assert.IsInstanceOfType(
                response,
                typeof(SmbTreeDisconnectResponsePacket),
                "SMB_COM_TREE_DISCONNECT response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            SmbTreeDisconnectResponsePacket treeDisconnectResponse = (SmbTreeDisconnectResponsePacket)response;
            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                treeDisconnectResponse.SmbHeader.Status,
                "SMB_COM_TREE_DISCONNECT response status should be SUCCESS.");

            // LogOff
            SmbLogoffAndxRequestPacket logoffRequest = smbClientStack.CreateLogoffRequest(sessionUid);

            smbClientStack.SendPacket(logoffRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server return a response.
            Site.Assert.IsNotNull(
                response,
                "SMB_COM_LOGOFF_ANDX response should not be null.");

            Site.Assert.IsInstanceOfType(
                response,
                typeof(SmbLogoffAndxResponsePacket),
                "SMB_COM_LOGOFF_ANDX response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            SmbLogoffAndxResponsePacket logoffResponse = (SmbLogoffAndxResponsePacket)response;
            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                logoffResponse.SmbHeader.Status,
                "SMB_COM_LOGOFF_ANDX response status should be SUCCESS.");

            // Disconnect
            smbClientStack.Disconnect();

            Site.Assert.IsFalse(
               smbClientStack.IsDataAvailable,
               "SmbClient should not receive any packet after Disconnect method is called.");

            #endregion

        }

        /// <summary>
        /// Test case of verifying NetServerEnum  request.
        /// </summary>
        [TestMethod]
        [TestCategory("Traditional")]
        public void TraditionalTestCase_NetServerEnum2_06()
        {
            #region Connect to the specified server

            smbClientStack.Connect(serverName, serverPort, ipVersion, bufferSize);

            #endregion

            #region Send the Negotiate request

            // Create a SMB_COM_NEGOTIATE request.
            SmbNegotiateRequestPacket negotiateRequest =
                smbClientStack.CreateNegotiateRequest(
                StackSmb.SignState.NONE,
                new string[] {
                    DialectNameString.PCNET1,
                    DialectNameString.LANMAN10,
                    DialectNameString.WFW10,
                    DialectNameString.LANMAN12,
                    DialectNameString.LANMAN21,
                    DialectNameString.NTLANMAN
                });

            // Send the SMB_COM_NEGOTIATE request and expect the response in timeout milliseconds.
            smbClientStack.SendPacket(negotiateRequest);
            StackPacket response = smbClientStack.ExpectPacket(timeout);

            // Check whether server return a response.
            Site.Assert.IsNotNull(
                response,
                "SMB_COM_NEGOTIATE response should not be null.");

            // Check whether server returns a SMB_COM_NEGOTIATE response.
            Site.Assert.IsInstanceOfType(
                response,
                typeof(SmbNegotiateResponsePacket),
                "SMB_COM_NEGOTIATE response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            SmbNegotiateResponsePacket negotiateResponse = (SmbNegotiateResponsePacket)response;
            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                negotiateRequest.SmbHeader.Status,
                "SMB_COM_NEGOTIATE response status should be SUCCESS.");

            #endregion

            #region Send the first SMB_COM_SESSION_SETUP_ANDX  Request

            SmbSecurityPackage smbSecurityPackage = (SmbSecurityPackage)Enum.Parse(
                typeof(SmbSecurityPackage),
                Site.Properties["SmbSecurityPackageType"] as string,
                true);

            // Create the first SMB_COM_SESSION_SETUP_ANDX request.
            SmbSessionSetupAndxRequestPacket sessionSetupAndxRequest =
                smbClientStack.CreateFirstSessionSetupRequest(
                smbSecurityPackage,
                serverName,
                domainName,
                userName,
                password);

            // Send the first SMB_COM_SESSION_SETUP_ANDX request and expect the response in timeout milliseconds.
            smbClientStack.SendPacket(sessionSetupAndxRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server return a response.
            Site.Assert.IsNotNull(
                response,
                "SMB_COM_SESSION_SETUP_ANDX response should not be null.");

            // Check whether server returns a the SMB_COM_SESSION_SETUP_ANDX response.
            Site.Assert.IsInstanceOfType(
                response,
                typeof(SmbSessionSetupAndxResponsePacket),
                "SMB_COM_SESSION_SETUP_ANDX response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            // If SMB SecurityPackage type is NTLM, the expected SUCCESS response status is STATUS_MORE_PROCESSING_REQUIRED,
            // else if SMB SecurityPackage type is Kerberos, the expected SUCCESS response status is STATUS_SUCCESS.
            SmbSessionSetupAndxResponsePacket sessionSetupResponse = (SmbSessionSetupAndxResponsePacket)response;
            Site.Assert.IsTrue(
                (int)sessionSetupResponse.SmbHeader.Status == (int)SmbStatus.STATUS_MORE_PROCESSING_REQUIRED ||
                (int)sessionSetupResponse.SmbHeader.Status == (int)SmbStatus.STATUS_SUCCESS,
                "SMB_COM_SESSION_SETUP_ANDX response status should be SUCCESS.");

            #endregion

            #region Send the second SMB_COM_SESSION_SETUP_ANDX  request

            // Create the second SMB_COM_SESSION_SETUP_ANDX request.
            ushort sessionUid = sessionSetupResponse.SmbHeader.Uid;

            if ((int)sessionSetupResponse.SmbHeader.Status == (int)SmbStatus.STATUS_MORE_PROCESSING_REQUIRED)
            {
                SmbSessionSetupAndxRequestPacket secondSessionSetupRequest =
                    smbClientStack.CreateSecondSessionSetupRequest(sessionUid, smbSecurityPackage);

                // Send the second SMB_COM_SESSION_SETUP_ANDX request and expect the response in timeout milliseconds.
                smbClientStack.SendPacket(secondSessionSetupRequest);
                response = smbClientStack.ExpectPacket(timeout);

                // Check whether server return a response.
                Site.Assert.IsNotNull(
                    response,
                    "SMB_COM_SESSION_SETUP_ANDX response should not be null.");

                // Check whether server returns a the SMB_COM_SESSION_SETUP_ANDX response.
                Site.Assert.IsInstanceOfType(
                    response,
                    typeof(SmbSessionSetupAndxResponsePacket),
                    "SMB_COM_SESSION_SETUP_ANDX response should be received.");

                // Check the response validity by verifying the Status field in the SMB Header packet.
                sessionSetupResponse = (SmbSessionSetupAndxResponsePacket)response;
                Site.Assert.AreEqual<uint>(
                    (uint)SmbStatus.STATUS_SUCCESS,
                    sessionSetupResponse.SmbHeader.Status,
                    "SMB_COM_SESSION_SETUP_ANDX response status should be SUCCESS.");
            }

            #endregion

            #region Send the SMB_COM_TREE_CONNECT_ANDX request

            string path = Site.Properties["SutNamedPipeFullName"];

            SmbTreeConnectAndxRequestPacket treeconnectRequest =
            smbClientStack.CreateTreeConnectRequest(sessionUid, path);

            smbClientStack.SendPacket(treeconnectRequest);
            response = smbClientStack.ExpectPacket(timeout);

            SmbTreeConnectAndxResponsePacket treeConnectResponse = (SmbTreeConnectAndxResponsePacket)response;
            #endregion

            #region SMB transaction
             byte[] paramDesc =new byte[]{0x57,0x72,0x4c,0x65,0x68,0x44,0x4f,0x00};
             byte[] dataDesc =new byte[]{0x42,0x31,0x36,0x00};
             byte[] rapParamsAndAuxDesc =new byte[]{0x00,0x00,0x84,0x39};
             byte[] rapInData = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF};
            SmbTransRapRequestPacket netServerEnmum2Request =
                smbClientStack.CreateTransNamedRapRequest(treeConnectResponse.SmbHeader.Tid,
                StackCifs.TransSmbParametersFlags.NONE,
                (ushort)0x68,
                paramDesc,
                dataDesc,
                rapParamsAndAuxDesc, rapInData);
            smbClientStack.SendPacket(netServerEnmum2Request);
            response = smbClientStack.ExpectPacket(timeout);
            #endregion
        }

        /// <summary>
        /// Test case of verifying NetServerEnum  request.
        /// </summary>
        [TestMethod]
        [TestCategory("Traditional")]
        public void TraditionalTestCase_NetServerEnum3_06()
        {
            #region Connect to the specified server

            smbClientStack.Connect(serverName, serverPort, ipVersion, bufferSize);

            #endregion

            #region Send the Negotiate request

            // Create a SMB_COM_NEGOTIATE request.
            SmbNegotiateRequestPacket negotiateRequest =
                smbClientStack.CreateNegotiateRequest(
                StackSmb.SignState.NONE,
                new string[] {
                    DialectNameString.PCNET1,
                    DialectNameString.LANMAN10,
                    DialectNameString.WFW10,
                    DialectNameString.LANMAN12,
                    DialectNameString.LANMAN21,
                    DialectNameString.NTLANMAN
                });

            // Send the SMB_COM_NEGOTIATE request and expect the response in timeout milliseconds.
            smbClientStack.SendPacket(negotiateRequest);
            StackPacket response = smbClientStack.ExpectPacket(timeout);

            // Check whether server return a response.
            Site.Assert.IsNotNull(
                response,
                "SMB_COM_NEGOTIATE response should not be null.");

            // Check whether server returns a SMB_COM_NEGOTIATE response.
            Site.Assert.IsInstanceOfType(
                response,
                typeof(SmbNegotiateResponsePacket),
                "SMB_COM_NEGOTIATE response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            SmbNegotiateResponsePacket negotiateResponse = (SmbNegotiateResponsePacket)response;
            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                negotiateRequest.SmbHeader.Status,
                "SMB_COM_NEGOTIATE response status should be SUCCESS.");

            #endregion

            #region Send the first SMB_COM_SESSION_SETUP_ANDX  Request

            SmbSecurityPackage smbSecurityPackage = (SmbSecurityPackage)Enum.Parse(
                typeof(SmbSecurityPackage),
                Site.Properties["SmbSecurityPackageType"] as string,
                true);

            // Create the first SMB_COM_SESSION_SETUP_ANDX request.
            SmbSessionSetupAndxRequestPacket sessionSetupAndxRequest =
                smbClientStack.CreateFirstSessionSetupRequest(
                smbSecurityPackage,
                serverName,
                domainName,
                userName,
                password);

            // Send the first SMB_COM_SESSION_SETUP_ANDX request and expect the response in timeout milliseconds.
            smbClientStack.SendPacket(sessionSetupAndxRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server return a response.
            Site.Assert.IsNotNull(
                response,
                "SMB_COM_SESSION_SETUP_ANDX response should not be null.");

            // Check whether server returns a the SMB_COM_SESSION_SETUP_ANDX response.
            Site.Assert.IsInstanceOfType(
                response,
                typeof(SmbSessionSetupAndxResponsePacket),
                "SMB_COM_SESSION_SETUP_ANDX response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            // If SMB SecurityPackage type is NTLM, the expected SUCCESS response status is STATUS_MORE_PROCESSING_REQUIRED,
            // else if SMB SecurityPackage type is Kerberos, the expected SUCCESS response status is STATUS_SUCCESS.
            SmbSessionSetupAndxResponsePacket sessionSetupResponse = (SmbSessionSetupAndxResponsePacket)response;
            Site.Assert.IsTrue(
                (int)sessionSetupResponse.SmbHeader.Status == (int)SmbStatus.STATUS_MORE_PROCESSING_REQUIRED ||
                (int)sessionSetupResponse.SmbHeader.Status == (int)SmbStatus.STATUS_SUCCESS,
                "SMB_COM_SESSION_SETUP_ANDX response status should be SUCCESS.");

            #endregion

            #region Send the second SMB_COM_SESSION_SETUP_ANDX  request

            // Create the second SMB_COM_SESSION_SETUP_ANDX request.
            ushort sessionUid = sessionSetupResponse.SmbHeader.Uid;

            if ((int)sessionSetupResponse.SmbHeader.Status == (int)SmbStatus.STATUS_MORE_PROCESSING_REQUIRED)
            {
                SmbSessionSetupAndxRequestPacket secondSessionSetupRequest =
                    smbClientStack.CreateSecondSessionSetupRequest(sessionUid, smbSecurityPackage);

                // Send the second SMB_COM_SESSION_SETUP_ANDX request and expect the response in timeout milliseconds.
                smbClientStack.SendPacket(secondSessionSetupRequest);
                response = smbClientStack.ExpectPacket(timeout);

                // Check whether server return a response.
                Site.Assert.IsNotNull(
                    response,
                    "SMB_COM_SESSION_SETUP_ANDX response should not be null.");

                // Check whether server returns a the SMB_COM_SESSION_SETUP_ANDX response.
                Site.Assert.IsInstanceOfType(
                    response,
                    typeof(SmbSessionSetupAndxResponsePacket),
                    "SMB_COM_SESSION_SETUP_ANDX response should be received.");

                // Check the response validity by verifying the Status field in the SMB Header packet.
                sessionSetupResponse = (SmbSessionSetupAndxResponsePacket)response;
                Site.Assert.AreEqual<uint>(
                    (uint)SmbStatus.STATUS_SUCCESS,
                    sessionSetupResponse.SmbHeader.Status,
                    "SMB_COM_SESSION_SETUP_ANDX response status should be SUCCESS.");
            }

            #endregion

            #region Send the SMB_COM_TREE_CONNECT_ANDX request

            string path = Site.Properties["SutNamedPipeFullName"];

            SmbTreeConnectAndxRequestPacket treeconnectRequest =
            smbClientStack.CreateTreeConnectRequest(sessionUid, path);

            smbClientStack.SendPacket(treeconnectRequest);
            response = smbClientStack.ExpectPacket(timeout);

            SmbTreeConnectAndxResponsePacket treeConnectResponse = (SmbTreeConnectAndxResponsePacket)response;
            #endregion

            #region SMB transaction
            // System.Text.ASCIIEncoding.ASCII.GetBytes("WrLehDZz");
            byte[] paramDesc = new byte[] { 0x57, 0x72, 0x4c, 0x65, 0x68, 0x44, 0x7a, 0x7a, 0x00 };
               
            byte[] dataDesc = new byte[] { 0x42, 0x31, 0x36, 0x42,0x42,0x44,0x7a,0x00 };
            byte[] rapParamsAndAuxDesc = new byte[] { 0x01, 0x00, 0xFF, 0xFF };
           
            byte[] rapInData = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF,0x45,0x4e,0x44,0x50,
            0x4f,0x49,0x4e,0x54,0x30,0x31,0x00};
     
            SmbTransRapRequestPacket netServerEnmum2Request =
                smbClientStack.CreateTransNamedRapRequest(treeConnectResponse.SmbHeader.Tid,
                StackCifs.TransSmbParametersFlags.NONE,
                (ushort)0x00D7,
                paramDesc,
                dataDesc,
                rapParamsAndAuxDesc, rapInData);
            smbClientStack.SendPacket(netServerEnmum2Request);
            response = smbClientStack.ExpectPacket(timeout);
            #endregion
        }
    }
}
