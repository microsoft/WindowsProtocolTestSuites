// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using StackCifs = Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;
using StackSmb = Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb;

namespace Microsoft.Protocol.TestSuites.Smb
{
    /// <summary>
    /// Summary description for TraditionalTestCases
    /// </summary>
    public partial class TestSuite : TestClassBase
    {
        /// <summary>
        /// Test case of verifying large read & write operations.
        /// </summary>
        [TestMethod]
        [TestCategory("Traditional")]
        public void TraditionalTestCase_LARGE_Read_Write_10()
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

            // Check whether server returns a response.
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
                negotiateResponse.SmbHeader.Status,
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

            #region Set up the request parameters

            SMB_COM_SESSION_SETUP_ANDX_Request_SMB_Parameters sessionSetupParam = sessionSetupAndxRequest.SmbParameters;
            sessionSetupParam.Capabilities |= 0xC000;
            sessionSetupAndxRequest.SmbParameters = sessionSetupParam;

            #endregion

            smbClientStack.SendPacket(sessionSetupAndxRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server returns a response.
            Site.Assert.IsNotNull(
                response,
                "SMB_COM_SESSION_SETUP_ANDX response should not be null.");

            // Check whether server returns a SMB_COM_SESSION_SETUP_ANDX response.
            Site.Assert.IsInstanceOfType(
                response,
                typeof(SmbSessionSetupAndxResponsePacket),
                "SMB_COM_SESSION_SETUP_ANDX response should be received.");

            SmbSessionSetupAndxResponsePacket sessionSetupResponse = (SmbSessionSetupAndxResponsePacket)response;

            #endregion

            #region Send the second SMB_COM_SESSION_SETUP_ANDX  request

            ushort sessionUid = sessionSetupResponse.SmbHeader.Uid;

            if ((int)sessionSetupResponse.SmbHeader.Status == (int)SmbStatus.STATUS_MORE_PROCESSING_REQUIRED)
            {
                SmbSessionSetupAndxRequestPacket secondSessionSetupRequest =
                    smbClientStack.CreateSecondSessionSetupRequest(sessionUid, smbSecurityPackage);

                sessionSetupParam = sessionSetupAndxRequest.SmbParameters;
                sessionSetupParam.Capabilities |= 0xC000;
                sessionSetupAndxRequest.SmbParameters = sessionSetupParam;

                smbClientStack.SendPacket(secondSessionSetupRequest);
                response = smbClientStack.ExpectPacket(timeout);

                // Check whether server returns a response.
                Site.Assert.IsNotNull(
                    response,
                    "SMB_COM_SESSION_SETUP_ANDX response should not be null.");

                // Check whether server returns a SMB_COM_SESSION_SETUP_ANDX response.
                Site.Assert.IsInstanceOfType(
                    response,
                    typeof(SmbSessionSetupAndxResponsePacket),
                    "SMB_COM_SESSION_SETUP_ANDX response should be received.");
            }

            // Check the response validity by verifying the Status field in the SMB Header packet.
            sessionSetupResponse = (SmbSessionSetupAndxResponsePacket)response;
            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                sessionSetupResponse.SmbHeader.Status,
                "SMB_COM_SESSION_SETUP_ANDX response status should be SUCCESS.");

            #endregion

            #region Send the SMB_COM_TREE_CONNECT_ANDX request

            string path = Site.Properties["SutNtfsShare1FullName"];

            SmbTreeConnectAndxRequestPacket treeconnectRequest =
                smbClientStack.CreateTreeConnectRequest(sessionUid, path);

            smbClientStack.SendPacket(treeconnectRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server returns a response.
            Site.Assert.IsNotNull(
                response,
                "SMB_COM_TREE_CONNECT_ANDX response should not be null.");

            // Check whether server returns a SMB_COM_TREE_CONNECT_ANDX response.
            Site.Assert.IsInstanceOfType(
                response,
                typeof(SmbTreeConnectAndxResponsePacket),
                "SMB_COM_TREE_CONNECT_ANDX response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            SmbTreeConnectAndxResponsePacket treeConnectResponse = (SmbTreeConnectAndxResponsePacket)response;
            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                treeConnectResponse.SmbHeader.Status,
                "SMB_COM_TREE_CONNECT_ANDX response status should be SUCCESS.");

            #endregion

            #region Send the SMB_COM_NT_CREATE_ANDX request

            ushort treeId = treeConnectResponse.SmbHeader.Tid;
            string fileName = Site.Properties["SutShareTest1"];
            smbClientStack.Capability.Flag |= SmbHeader_Flags_Values.OPLOCK;

            SmbNtCreateAndxRequestPacket createRequest =
                smbClientStack.CreateCreateRequest(
                treeId,
                fileName,
                StackCifs.NtTransactDesiredAccess.FILE_READ_DATA
                | StackCifs.NtTransactDesiredAccess.FILE_WRITE_DATA
                | StackCifs.NtTransactDesiredAccess.FILE_READ_EA
                | StackCifs.NtTransactDesiredAccess.FILE_READ_ATTRIBUTES,
                StackCifs.SMB_EXT_FILE_ATTR.NONE,
                StackCifs.NtTransactShareAccess.FILE_SHARE_READ
                | StackCifs.NtTransactShareAccess.FILE_SHARE_WRITE
                | StackCifs.NtTransactShareAccess.FILE_SHARE_DELETE,
                StackCifs.NtTransactCreateDisposition.FILE_OPEN_IF,
                NtTransactCreateOptions.FILE_OPEN_REPARSE_POINT
                | NtTransactCreateOptions.FILE_SEQUENTIAL_ONLY
                | NtTransactCreateOptions.FILE_NON_DIRECTORY_FILE,
                StackCifs.NtTransactImpersonationLevel.SEC_ANONYMOUS,
                CreateFlags.NT_CREATE_REQUEST_OPLOCK);

            smbClientStack.SendPacket(createRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server returns a response.
            Site.Assert.IsNotNull(
                response,
                "SMB_COM_NT_CREATE_ANDX response should not be null.");

            // Check whether server returns a SMB_COM_NT_CREATE_ANDX response.
            Site.Assert.IsInstanceOfType(
                response,
                typeof(SmbNtCreateAndxResponsePacket),
                "SMB_COM_NT_CREATE_ANDX response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            SmbNtCreateAndxResponsePacket createResponse = (SmbNtCreateAndxResponsePacket)response;
            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                createResponse.SmbHeader.Status,
                "SMB_COM_NT_CREATE_ANDX response status should be SUCCESS.");

            #endregion

            #region Send the SMB_WRITE_ADNX request

            ushort fileId = createResponse.SmbParameters.FID;
            //0xFFFE is the max vaule can be written by protocol SDK once call.
            byte[] writeData = new byte[0xFFFE];

            for (int i = 0; i < writeData.Length; i++)
            {
                writeData[i] = (byte)'a';
            }

            SmbWriteAndxRequestPacket writeRequest = smbClientStack.CreateWriteRequest(fileId, 0, writeData, 0x10000);

            smbClientStack.SendPacket(writeRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server returns a response.
            Site.Assert.IsNotNull(
                response,
                "SMB_WRITE_ADNX response should not be null.");

            // Check whether server returns a SMB_WRITE_ADNX response.
            Site.Assert.IsInstanceOfType(
                response,
                typeof(SmbWriteAndxResponsePacket),
                "SMB_WRITE_ADNX response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            SmbWriteAndxResponsePacket writeResponse = (SmbWriteAndxResponsePacket)response;
            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                writeResponse.SmbHeader.Status,
                "SMB_WRITE_ADNX response status should be SUCCESS.");

            #endregion

            #region Verify R6002, R6003

            //             
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R6003");

            //
            // Verify MS-SMB requirement: MS-SMB_R6003
            //
            Site.CaptureRequirementIfAreEqual<ushort>(
                 1,
                 writeResponse.SmbParameters.CountHigh,
                 6003,
                 @"[In Server Response Extensions]CountHigh (2 bytes): If the number of bytes written is equal to 0x00010000( 64 kilobytes),
                 then the server MUST set the two most significant bytes of the length in the CountHigh field.");

            //             
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R6002");

            //
            // Verify MS-SMB requirement: MS-SMB_R6002
            //
            Site.CaptureRequirementIfAreEqual<ushort>(
                 0,
                 writeResponse.SmbParameters.Count,
                 6002,
                 @"[In Server Response Extensions]CountHigh (2 bytes): If the number of bytes written is equal to 0x00010000( 64 kilobytes),
                 then the server MUST set the two least significant bytes of the length in the Count field of the request.");

            #endregion

            #region Send the SMB_READ_ANDX request

            SmbReadAndxRequestPacket readRequest = smbClientStack.CreateReadRequest(fileId, 17000, 0);

            smbClientStack.SendPacket(readRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server returns a response.
            Site.Assert.IsNotNull(
                response,
                "SMB_READ_ANDX response should not be null.");

            // Check whether server returns a SMB_READ_ANDX response.
            Site.Assert.IsInstanceOfType(
                response,
                typeof(SmbReadAndxResponsePacket),
                "SMB_READ_ANDX response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            SmbReadAndxResponsePacket readResponse = (SmbReadAndxResponsePacket)response;

            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                readResponse.SmbHeader.Status,
                "SMB_READ_ANDX response status should be SUCCESS.");

            #endregion

            #region Capture requirements R109957, R9957, R106932

            bool isRequestExt = ((treeconnectRequest.SmbParameters.Flags & 0x0008) == 0x0008);

            if (isRequestExt)
            {
                if (((uint)sessionSetupAndxRequest.SmbParameters.Capabilities & (uint)Capabilities.CapLargeReadx)
                    == (uint)Capabilities.CapLargeReadx)
                {
                    //             
                    // Add the debug information
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R9957,ByteCount is {0},
                        MaxBufferSize is {1}",
                        readResponse.SmbData.ByteCount,
                        negotiateResponse.SmbParameters.MaxBufferSize);

                    //
                    // Verify MS-SMB requirement: MS-SMB_R9957
                    //
                    Site.CaptureRequirementIfIsTrue(
                         readResponse.SmbData.ByteCount > negotiateResponse.SmbParameters.MaxBufferSize,
                         9957,
                         @"[In Extended Security Response]MaxBufferSize (4 bytes): 
                         The only exceptions in which this maximum buffer size MUST be exceeded are: 
                         When the SMB_COM_WRITE_ANDX command is used and the client and server both support 
                         the CAP_LARGE_WRITEX capability (see the Capabilities field for more information).");

                    //             
                    // Add the debug information
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R109957,ByteCount is {0},
                        MaxBufferSize is {1}",
                        readResponse.SmbData.ByteCount,
                        negotiateResponse.SmbParameters.MaxBufferSize);

                    //
                    // Verify MS-SMB requirement: MS-SMB_R109957
                    //
                    Site.CaptureRequirementIfIsTrue(
                         readResponse.SmbData.ByteCount > negotiateResponse.SmbParameters.MaxBufferSize,
                         109957,
                         @"[In Extended Security Response]MaxBufferSize (4 bytes): 
                         The exceptions in which this maximum buffer size MUST be exceeded are:
                         When the SMB_COM_WRITE_ANDX command is used and the client and server both support 
                         the CAP_LARGE_WRITEX capability (see the Capabilities field for more information).");

                    // this RS has been covered by the R9957 logic
                    Site.CaptureRequirement(
                        5402,
                        @"When this capability is set by the server (and set by the client in the
                        SMB_COM_SESSION_SETUP_ANDX request), then the maximum server buffer size for 
                        sending data can exceed the MaxBufferSize field.");

                    // this RS has been covered by the R9957 logic
                    Site.CaptureRequirement(
                        206932,
                        @"<32> Section 2.2.4.5.2.1:Windows-based clients and servers support CAP_LARGE_READX, 
                        which permits file transfers larger than the negotiated MaxBufferSize.");

                    //
                    // Add the debug information
                    //
                    Site.Log.Add(LogEntryKind.Comment, "Verify MS-SMB_R106932");

                    //
                    // Verify MS-SMB requirement: MS-SMB_R106932
                    //
                    Site.CaptureRequirementIfIsTrue(
                         Convert.ToInt32(readResponse.SmbData.ByteCount) < 65536,
                         106932,
                         @"<32> Section 2.2.4.5.2.1: With CAP_LARGE_READX enabled, 
                         Windows-based servers set this limit to 64 kilobytes. ");
                }
            }

            #endregion

            #region Disconnect the tree, session and connection.

            // Close1
            SmbCloseRequestPacket CloseRequest = smbClientStack.CreateCloseRequest(fileId);

            smbClientStack.SendPacket(CloseRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server return a response.
            Site.Assert.IsNotNull(
                 response,
                "SMB_CLOSE response should not be null.");

            Site.Assert.IsInstanceOfType(
                response,
                typeof(SmbCloseResponsePacket),
                "SMB_CLOSE response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            SmbCloseResponsePacket closeResponse = (SmbCloseResponsePacket)response;
            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                closeResponse.SmbHeader.Status,
                "SMB_CLOSE response status should be SUCCESS.");

            // TreeDisconnect
            SmbTreeDisconnectRequestPacket treeDisconnectRequest = smbClientStack.CreateTreeDisconnectRequest(treeId);

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

            #region Connect

            smbClientStack.Connect(serverName, serverPort, ipVersion, bufferSize);

            #endregion

            #region Send the Negotiate request

            smbClientStack.Capability.IsSupportsExtendedSecurity = false;

            negotiateRequest =
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

            smbClientStack.SendPacket(negotiateRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server returns a response.
            Site.Assert.IsNotNull(
                response,
                "SMB_COM_NEGOTIATE response should not be null.");

            // Check whether server returns a SMB_COM_NEGOTIATE response.
            Site.Assert.IsInstanceOfType(
                response,
                typeof(SmbNegotiateImplicitNtlmResponsePacket),
                "SMB_COM_NEGOTIATE response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            SmbNegotiateImplicitNtlmResponsePacket negotiateImpNtlmResponse
                = (SmbNegotiateImplicitNtlmResponsePacket)response;

            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                negotiateImpNtlmResponse.SmbHeader.Status,
                "SMB_COM_NEGOTIATE response status should be SUCCESS.");

            #endregion

            #region Send the SMB_COM_SESSION_SETUP_ANDX request

            SmbSessionSetupImplicitNtlmAndxRequestPacket sessionSetupImpNtlmRequest =
                smbClientStack.CreateSessionSetupImplicitNtlmRequest(
                ImplicitNtlmVersion.NtlmVersion1,
                domainName,
                userName,
                password);

            smbClientStack.SendPacket(sessionSetupImpNtlmRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server returns a response.
            Site.Assert.IsNotNull(
                response,
                "SMB_COM_SESSION_SETUP_ANDX response should not be null.");

            // Check whether server returns a SMB_COM_SESSION_SETUP_ANDX response.
            Site.Assert.IsInstanceOfType(
                response,
                typeof(SmbSessionSetupImplicitNtlmAndxResponsePacket),
                "SMB_COM_SESSION_SETUP_ANDX response should be received.");

            SmbSessionSetupImplicitNtlmAndxResponsePacket sessionSetupImpNtlmResponse =
                (SmbSessionSetupImplicitNtlmAndxResponsePacket)response;

            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                sessionSetupImpNtlmResponse.SmbHeader.Status,
                "SMB_COM_SESSION_SETUP_ANDX response status should be SUCCESS.");

            #endregion

            #region Send the SMB_COM_TREE_CONNECT_ANDX request

            sessionUid = sessionSetupImpNtlmResponse.SmbHeader.Uid;
            path = Site.Properties["SutNtfsShare1FullName"];
            treeconnectRequest = smbClientStack.CreateTreeConnectRequest(sessionUid, path);

            smbClientStack.SendPacket(treeconnectRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server returns a response.
            Site.Assert.IsNotNull(
                response,
                "SMB_COM_TREE_CONNECT_ANDX response should not be null.");

            // Check whether server returns a SMB_COM_TREE_CONNECT_ANDX response.
            Site.Assert.IsInstanceOfType(
                response,
                typeof(SmbTreeConnectAndxResponsePacket),
                "SMB_COM_TREE_CONNECT_ANDX response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            treeConnectResponse = (SmbTreeConnectAndxResponsePacket)response;
            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                treeConnectResponse.SmbHeader.Status,
                "SMB_COM_TREE_CONNECT_ANDX response status should be SUCCESS.");

            treeId = treeConnectResponse.SmbHeader.Tid;
            fileName = Site.Properties["SutShareTest1"];
            smbClientStack.Capability.Flag |= SmbHeader_Flags_Values.OPLOCK;

            #endregion

            #region Send the SMB_NT_CREATE_ANDX request

            createRequest =
                smbClientStack.CreateCreateRequest(
                treeId,
                fileName,
                StackCifs.NtTransactDesiredAccess.FILE_READ_DATA
                | StackCifs.NtTransactDesiredAccess.FILE_WRITE_DATA
                | StackCifs.NtTransactDesiredAccess.FILE_READ_EA
                | StackCifs.NtTransactDesiredAccess.FILE_READ_ATTRIBUTES,
                StackCifs.SMB_EXT_FILE_ATTR.NONE,
                StackCifs.NtTransactShareAccess.FILE_SHARE_READ
                | StackCifs.NtTransactShareAccess.FILE_SHARE_WRITE
                | StackCifs.NtTransactShareAccess.FILE_SHARE_DELETE,
                StackCifs.NtTransactCreateDisposition.FILE_OPEN_IF,
                NtTransactCreateOptions.FILE_OPEN_REPARSE_POINT
                | NtTransactCreateOptions.FILE_SEQUENTIAL_ONLY
                | NtTransactCreateOptions.FILE_NON_DIRECTORY_FILE,
                StackCifs.NtTransactImpersonationLevel.SEC_ANONYMOUS,
                CreateFlags.NT_CREATE_REQUEST_OPLOCK);

            smbClientStack.SendPacket(createRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server returns a response.
            Site.Assert.IsNotNull(
                response,
                "SMB_COM_NT_CREATE_ANDX response should not be null.");

            // Check whether server returns a SMB_COM_NT_CREATE_ANDX response.
            Site.Assert.IsInstanceOfType(
                response,
                typeof(SmbNtCreateAndxResponsePacket),
                "SMB_COM_NT_CREATE_ANDX response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            createResponse = (SmbNtCreateAndxResponsePacket)response;
            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                createResponse.SmbHeader.Status,
                "SMB_COM_NT_CREATE_ANDX response status should be SUCCESS.");

            fileId = createResponse.SmbParameters.FID;

            #endregion

            #region Send the SMB_WRITE_ANDX request

            writeRequest = smbClientStack.CreateWriteRequest(fileId, 0, writeData);

            smbClientStack.SendPacket(writeRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server returns a response.
            Site.Assert.IsNotNull(
                response,
                "SMB_WRITE_ANDX response should not be null.");

            // Check whether server returns a SMB_WRITE_ANDX response.
            Site.Assert.IsInstanceOfType(
                response,
                typeof(SmbWriteAndxResponsePacket),
                "SMB_WRITE_ANDX response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            writeResponse = (SmbWriteAndxResponsePacket)response;

            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                writeResponse.SmbHeader.Status,
                "SMB_WRITE_ANDX response status should be SUCCESS.");

            #endregion

            #region Capture requirement r9207

            // according to TD , the following condition means non-Extension
            if (((uint)negotiateRequest.SmbHeader.Flags2 & 0x0800) == 0)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(
                    LogEntryKind.Debug,
                    @"Verify MS-SMB_R9207,
                    ByteCount is {0},
                    MaxBufferSize is {1}",
                    writeResponse.SmbParameters.Count,
                    negotiateResponse.SmbParameters.MaxBufferSize);

                //
                // Verify MS-SMB requirement: MS-SMB_R9957
                //
                Site.CaptureRequirementIfIsTrue(
                     writeResponse.SmbParameters.Count > negotiateResponse.SmbParameters.MaxBufferSize,
                     9207,
                     @"[In Non-Extended Security Response]MaxBufferSize (4 bytes): 
                     The only exceptions in which this maximum buffer size MUST be exceeded are:
                     When the SMB_COM_WRITE_ANDX command is used and both the client and server support the 
                     CAP_LARGE_WRITEX capability (see the Capabilities field for more information).");
            }

            #endregion

            #region Disconnect the tree, session and connection.

            // Close1
            CloseRequest = smbClientStack.CreateCloseRequest(fileId);

            smbClientStack.SendPacket(CloseRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server return a response.
            Site.Assert.IsNotNull(
                 response,
                "SMB_CLOSE response should not be null.");

            // Check whether server returns a SMB_CLOSE response.
            Site.Assert.IsInstanceOfType(
                response,
                typeof(SmbCloseResponsePacket),
                "SMB_CLOSE response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            closeResponse = (SmbCloseResponsePacket)response;
            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                closeResponse.SmbHeader.Status,
                "SMB_CLOSE response status should be SUCCESS.");

            // TreeDisconnect
            treeDisconnectRequest = smbClientStack.CreateTreeDisconnectRequest(treeId);

            smbClientStack.SendPacket(treeDisconnectRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server return a response.
            Site.Assert.IsNotNull(
                response,
                "SMB_COM_TREE_DISCONNECT response should not be null.");

            // Check whether server returns a SMB_COM_TREE_DISCONNECT response.
            Site.Assert.IsInstanceOfType(
                response,
                typeof(SmbTreeDisconnectResponsePacket),
                "SMB_COM_TREE_DISCONNECT response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            treeDisconnectResponse = (SmbTreeDisconnectResponsePacket)response;
            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                treeDisconnectResponse.SmbHeader.Status,
                @"SMB_COM_TREE_DISCONNECT response status should be SUCCESS.");

            // LogOff
            logoffRequest = smbClientStack.CreateLogoffRequest(sessionUid);
            smbClientStack.SendPacket(logoffRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server return a response.
            Site.Assert.IsNotNull(
                response,
                "SMB_COM_LOGOFF_ANDX response should not be null.");

            // Check whether server returns a SMB_COM_LOGOFF_ANDX response.
            Site.Assert.IsInstanceOfType(
                response,
                typeof(SmbLogoffAndxResponsePacket),
                "SMB_COM_LOGOFF_ANDX response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            logoffResponse = (SmbLogoffAndxResponsePacket)response;
            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                logoffResponse.SmbHeader.Status,
                @"SMB_COM_LOGOFF_ANDX response status should be SUCCESS.");

            // Disconnect
            smbClientStack.Disconnect();

            Site.Assert.IsFalse(
               smbClientStack.IsDataAvailable,
               @"SmbClient should not receive any packet after Disconnect method is called.");

            #endregion
        }
    }
}
