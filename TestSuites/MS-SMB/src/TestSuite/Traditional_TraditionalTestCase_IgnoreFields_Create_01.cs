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
    /// MS-SMB test suite definition.
    /// </summary>
    public partial class TestSuite : TestClassBase
    {
        /// <summary>
        /// Test case of verifying the Create request with Fields ignored.
        /// This case failed, bug 6137 for track 
        /// </summary>
        [TestCategory("Traditional")]
        public void TraditionalTestCase_IgnoreFields_Create_01_Case()
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

            #region Send the first SMB_COM_SESSION_SETUP_ANDX request

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

            #region Send the SMB_COM_TREE_CONNECT_ANDX request

            // Create the SMB_COM_TREE_CONNECT_ANDX request.
            string path = Site.Properties["SutNtfsShare1FullName"];
             SmbTreeConnectAndxRequestPacket treeconnectRequest =
                    smbClientStack.CreateTreeConnectRequest(sessionSetupResponse.SmbHeader.Uid, path);

            if ((int)sessionSetupResponse.SmbHeader.Status == (int)SmbStatus.STATUS_MORE_PROCESSING_REQUIRED)
            {
                // Send the SMB_COM_TREE_CONNECT_ANDX request and expect the response in timeout milliseconds.
                smbClientStack.SendPacket(treeconnectRequest);
                response = smbClientStack.ExpectPacket(timeout);

                // Check whether server returns a response.
                Site.Assert.IsNotNull(
                    response,
                    "SMB_COM_TREE_CONNECT_ANDX response should not be null.");

                #region Capture requirements r11008

                // Check the response validity by verifying the Status field in the SMB Header packet.
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket; ;

                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-SMB_R11008");

                //
                // Verify MS-SMB requirement: MS-SMB_R11008
                //

                // When the session setup is in-progress, the client should have not been granted access, then expects a STATUS_ACCESS_DENIED from server
                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)MessageStatus.AccessDenied,
                    smbErrorResponsePacket.SmbHeader.Status,
                    11008,
                    @"[In Receiving an SMB_COM_TREE_CONNECT_ANDX Request] If no access is granted for the client on this share, 
                the server MUST fail the request with STATUS_ACCESS_DENIED.");

                #endregion
            }

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

            // Create the SMB_COM_TREE_CONNECT_ANDX request.
            path = Site.Properties["SutNtfsShare1FullName"];
            treeconnectRequest =
                smbClientStack.CreateTreeConnectRequest(sessionUid, path);

            // Send the SMB_COM_TREE_CONNECT_ANDX request and expect the response in timeout milliseconds.
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

            // Create the Create request.
            ushort treeId = treeConnectResponse.SmbHeader.Tid;
            string fileName = Site.Properties["SutShareTest2"];
            SmbNtCreateAndxRequestPacket createRequest =
                smbClientStack.CreateCreateRequest(
                treeId,
                fileName,
                StackCifs.NtTransactDesiredAccess.GENERIC_READ
                | StackCifs.NtTransactDesiredAccess.GENERIC_WRITE,
                StackCifs.SMB_EXT_FILE_ATTR.NONE,
                StackCifs.NtTransactShareAccess.NONE,
                StackCifs.NtTransactCreateDisposition.FILE_OPEN_IF,
                NtTransactCreateOptions.FILE_NON_DIRECTORY_FILE,
                StackCifs.NtTransactImpersonationLevel.SEC_ANONYMOUS,
                CreateFlags.NT_CREATE_REQUEST_OPLOCK);

            #region Set the request parameters

            StackCifs.SMB_COM_NT_CREATE_ANDX_Request_SMB_Parameters createRequestSmbParameters =
                createRequest.SmbParameters;

            // Reserved bits in ExtFileAttributes field set to 0.
            createRequestSmbParameters.ExtFileAttributes = 0;

            // Set the Flags field with
            // NT_CREATE_REQUEST_OPLOCK, NT_CREATE_OPEN_TARGET_DIR and NT_CREATE_REQUEST_EXTENDED_RESPONSE.
            createRequestSmbParameters.Flags = createRequestSmbParameters.Flags & (uint)(
                CreateFlags.NT_CREATE_REQUEST_OPLOCK
                | CreateFlags.NT_CREATE_OPEN_TARGET_DIR
                | CreateFlags.NT_CREATE_REQUEST_EXTENDED_RESPONSE);

            createRequest.SmbParameters = createRequestSmbParameters;

            #endregion

            // Send the SMB_COM_NT_CREATE_ANDX request with the File_Open_If flag and expect the response
            // in timeout milliseconds.
            smbClientStack.SendPacket(createRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server returns a response.
            Site.Assert.IsNotNull(
                 response,
                "SMB_COM_NT_CREATE_ANDX response should not be null.");

            // Check if server returns a SMB_COM_NT_CREATE_ANDX response.
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

            #region Send the SMB_CLOSE request

            // Create the Close request.
            ushort fileId = createResponse.SmbParameters.FID;
            SmbCloseRequestPacket CloseRequest = smbClientStack.CreateCloseRequest(fileId);

            // Send the Close request and expect the response in timeout milliseconds.
            smbClientStack.SendPacket(CloseRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server returns a response.
            Site.Assert.IsNotNull(
                 response,
                "SMB_CLOSE response should not be null.");

            // Check whether server returns a SMB_CLOSE response.
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

            #endregion

            #region Send the SMB_COM_NT_CREATE_ANDX request

            // Create the Create request.
            createRequest =
                smbClientStack.CreateCreateRequest(
                treeId,
                fileName,
                StackCifs.NtTransactDesiredAccess.GENERIC_READ
                | StackCifs.NtTransactDesiredAccess.GENERIC_WRITE,
                StackCifs.SMB_EXT_FILE_ATTR.NONE,
                StackCifs.NtTransactShareAccess.NONE,
                StackCifs.NtTransactCreateDisposition.FILE_OPEN_IF,
                NtTransactCreateOptions.FILE_NON_DIRECTORY_FILE,
                StackCifs.NtTransactImpersonationLevel.SEC_ANONYMOUS,
                CreateFlags.NT_CREATE_REQUEST_OPLOCK);

            #region Set the request parameters

            createRequestSmbParameters = createRequest.SmbParameters;

            // The ExtFileAttributes field is set to ATTR_READONLY(0x001).
            createRequestSmbParameters.ExtFileAttributes = 1;

            // Set the Flags field with
            // NT_CREATE_REQUEST_OPLOCK, NT_CREATE_OPEN_TARGET_DIR and NT_CREATE_REQUEST_EXTENDED_RESPONSE.
            createRequestSmbParameters.Flags = createRequestSmbParameters.Flags & (uint)(
                CreateFlags.NT_CREATE_REQUEST_OPLOCK
                | CreateFlags.NT_CREATE_OPEN_TARGET_DIR
                | CreateFlags.NT_CREATE_REQUEST_EXTENDED_RESPONSE);

            createRequest.SmbParameters = createRequestSmbParameters;

            #endregion

            // Send the Create request with the File_Open_If flag and expect the response in timeout milliseconds.
            smbClientStack.SendPacket(createRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server return a response.
            Site.Assert.IsNotNull(
                 response,
                "SMB_COM_NT_CREATE_ANDX response should not be null.");

            Site.Assert.IsInstanceOfType(
                response,
                typeof(SmbNtCreateAndxResponsePacket),
                "SMB_COM_NT_CREATE_ANDX response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            SmbNtCreateAndxResponsePacket createResponse2 = (SmbNtCreateAndxResponsePacket)response;
            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                createResponse2.SmbHeader.Status,
                "SMB_COM_NT_CREATE_ANDX response status should be SUCCESS.");

            #region Capture requirements r109027, r109243

            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-SMB_R109027");

            //
            // Verify MS-SMB requirement: MS-SMB_R109027
            //
            bool isVerifyR109027 = VerifyCreateResponse(createResponse, createResponse2);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR109027,
                109027,
                @"[In Extended File Attribute (SMB_EXT_FILE_ATTR) Extensions]Reserved 0xFFFF8048:
                Reply is the same no matter what value is used.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-SMB_R109243");

            //
            // Verify MS-SMB requirement: MS-SMB_R109243
            //
            bool isVerifyR109243 = VerifyCreateResponse(createResponse, createResponse2);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR109243,
                109243,
                @"[In Client Request Extensions]Flags (4 bytes):
                For Unused bits, reply is the same no matter what value is used.");

            #endregion

            #endregion

            #region Send the SMB_CLOSE request

            // Create the SMB_CLOSE request.
            fileId = createResponse2.SmbParameters.FID;
            CloseRequest = smbClientStack.CreateCloseRequest(fileId);

            // Send the Close request and expect the response in timeout milliseconds.
            smbClientStack.SendPacket(CloseRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server returns a response.
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

            #endregion

            #region Send the SMB_COM_NT_CREATE_ANDX request

            // Create the SMB_COM_NT_CREATE_ANDX request.
            createRequest = smbClientStack.CreateCreateRequest(
                treeId,
                fileName,
                StackCifs.NtTransactDesiredAccess.GENERIC_READ
                | StackCifs.NtTransactDesiredAccess.GENERIC_WRITE,
                StackCifs.SMB_EXT_FILE_ATTR.NONE,
                StackCifs.NtTransactShareAccess.NONE,
                StackCifs.NtTransactCreateDisposition.FILE_OPEN_IF,
                NtTransactCreateOptions.FILE_OPEN_REQUIRING_OPLOCK,
                StackCifs.NtTransactImpersonationLevel.SEC_ANONYMOUS,
                CreateFlags.NT_CREATE_REQUEST_OPLOCK);

            // Send the Create request and expect the response in timeout milliseconds.
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
            SmbNtCreateAndxResponsePacket createResponse3 = (SmbNtCreateAndxResponsePacket)response;

            if (sutOsVersion == Platform.Win7 || sutOsVersion == Platform.Win2K8R2)
            {
                Site.Assert.AreEqual<uint>(
                    (uint)SmbStatus.STATUS_SUCCESS,
                    createResponse3.SmbHeader.Status,
                    "SMB_COM_NT_CREATE_ANDX response status should be SUCCESS.");

                #region Cpature requirements r9459

                //
                // Add the comment information for debugging
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-SMB_R9459");

                //
                // Verify MS-SMB requirement: MS-SMB_R9459
                //
                bool isVerifyR9549 = createResponse3.SmbHeader.Status == (uint)SmbStatus.STATUS_SUCCESS;

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR9549,
                    9549,
                    @"<51> Section 2.2.4.9.1: Windows 7 and Windows Server 2008 R2 also support 
                    two new CreateOptions flags:FILE_OPEN_REQUIRING_OPLOCK (0x00010000),FILE_DISALLOW_EXCLUSIVE (0x00020000).");

                #endregion
            }

            #endregion

            #region Send the SMB_CLOSE request

            // Create the SMB_CLOSE request.
            fileId = createResponse3.SmbParameters.FID;
            CloseRequest = smbClientStack.CreateCloseRequest(fileId);

            // Send the Close request and expect the response in timeout milliseconds.
            smbClientStack.SendPacket(CloseRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server returns a response.
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

            #endregion

            #region Send the SMB_COM_NT_CREATE_ANDX request

            // Create the SMB_COM_NT_CREATE_ANDX request.
            fileName = Site.Properties["SutShareTest2"];
            createRequest =
               smbClientStack.CreateCreateRequest(
               treeId,
               fileName,
               StackCifs.NtTransactDesiredAccess.GENERIC_READ
               | StackCifs.NtTransactDesiredAccess.GENERIC_WRITE,
               StackCifs.SMB_EXT_FILE_ATTR.NONE,
               StackCifs.NtTransactShareAccess.NONE,
               StackCifs.NtTransactCreateDisposition.FILE_OPEN_IF,
               NtTransactCreateOptions.FILE_NON_DIRECTORY_FILE,
               StackCifs.NtTransactImpersonationLevel.SEC_ANONYMOUS,
               CreateFlags.NT_CREATE_REQUEST_OPLOCK);

            #region Set the request parameters

            createRequestSmbParameters = createRequest.SmbParameters;
            createRequestSmbParameters.ExtFileAttributes = 0;

            // Set the Flags field with
            // NT_CREATE_REQUEST_OPLOCK, NT_CREATE_OPEN_TARGET_DIR and NT_CREATE_REQUEST_EXTENDED_RESPONSE.
            createRequestSmbParameters.Flags = createRequestSmbParameters.Flags & (uint)(
                CreateFlags.NT_CREATE_REQUEST_OPLOCK
                | CreateFlags.NT_CREATE_OPEN_TARGET_DIR
                | CreateFlags.NT_CREATE_REQUEST_EXTENDED_RESPONSE);

            createRequest.SmbParameters = createRequestSmbParameters;

            #endregion

            // Send the Create request and expect the response in timeout milliseconds.
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
            SmbNtCreateAndxResponsePacket createResponse4 = (SmbNtCreateAndxResponsePacket)response;
            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                createResponse4.SmbHeader.Status,
                "SMB_COM_NT_CREATE_ANDX response status should be SUCCESS.");

            #region Cpature requirements r109550, r5227, r405227

            if (sutOsVersion == Platform.Win7 || sutOsVersion == Platform.Win2K8R2)
            {
                //
                // Add the comment information for debugging
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-SMB_R109550");

                //
                // Verify MS-SMB requirement: MS-SMB_R109550
                //
                bool isVerifyR109550 = VerifyCreateResponse(createResponse3, createResponse4);

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR109550,
                    109550,
                    @"<51> Section 2.2.4.9.1:FILE_OPEN_REQUIRING_OPLOCK (0x00010000) 
                reply from Windows 7 and Windows Server 2008 R2  is the same 
                whether zero or non-zero is used in this field.");
            }

            // As all the marked Unused fields that have been ignored are verified, this RS is covered by it.
            Site.CaptureRequirement(
                 5227,
                 @"[In Message Syntax]Unless otherwise noted, 
                 whatever the values of fields marked as Unused are, reply is the same upon receipt.");

            // As all the unused or reserved bits in bit fields that are ignored, this RS can be verified directly.
            Site.CaptureRequirement(
                 405227,
                 @"[In Message Syntax]Unless otherwise noted, 
                 whatever the values of unused or reserved bits in bit fields  are, reply is the same upon receipt.");

            #endregion

            #endregion

            #region Send the SMB_CLOSE request

            // Create the SMB_CLOSE request.
            fileId = createResponse4.SmbParameters.FID;
            CloseRequest = smbClientStack.CreateCloseRequest(fileId);

            // Send the Close request and expect the response in timeout milliseconds.
            smbClientStack.SendPacket(CloseRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server returns a response.
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

            #endregion

            #region Send the SMB_COM_NT_CREATE_ANDX request

            // Create the SMB_COM_NT_CREATE_ANDX request.
            createRequest = smbClientStack.CreateCreateRequest(
                treeId,
                fileName,
                StackCifs.NtTransactDesiredAccess.GENERIC_READ
                | StackCifs.NtTransactDesiredAccess.GENERIC_WRITE,
                StackCifs.SMB_EXT_FILE_ATTR.NONE,
                StackCifs.NtTransactShareAccess.NONE,
                StackCifs.NtTransactCreateDisposition.FILE_OPEN_IF,
                NtTransactCreateOptions.FILE_DISALLOW_EXCLUSIVE,
                StackCifs.NtTransactImpersonationLevel.SEC_ANONYMOUS,
                CreateFlags.NT_CREATE_REQUEST_OPLOCK);

            // Send the Create request and expect the response in timeout milliseconds.
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
            SmbNtCreateAndxResponsePacket createResponse5 = (SmbNtCreateAndxResponsePacket)response;

            if (sutOsVersion == Platform.Win7 || sutOsVersion == Platform.Win2K8R2)
            {
                Site.Assert.AreEqual<uint>(
                    (uint)SmbStatus.STATUS_SUCCESS,
                    createResponse5.SmbHeader.Status,
                    "SMB_COM_NT_CREATE_ANDX response status should be SUCCESS.");

                #region Cpature requirements r9459, r119551

                //
                // Add the comment information for debugging
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-SMB_R9459");

                //
                // Verify MS-SMB requirement: MS-SMB_R9459
                //
                bool isVerifyR9549 = createResponse5.SmbHeader.Status == (uint)SmbStatus.STATUS_SUCCESS;

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR9549,
                    9549,
                    @"<51> Section 2.2.4.9.1: Windows 7 and Windows Server 2008 R2 also support 
                    two new CreateOptions flags:FILE_OPEN_REQUIRING_OPLOCK (0x00010000),FILE_DISALLOW_EXCLUSIVE (0x00020000).");

                //
                // Add the comment information for debugging
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-SMB_R119551");

                //
                // Verify MS-SMB requirement: MS-SMB_R119551
                //
                bool isVerifyR119551 = VerifyCreateResponse(createResponse4, createResponse5);
                Site.CaptureRequirementIfIsTrue(
                isVerifyR119551,
                119551,
                @"<51> Section 2.2.4.9.1: FILE_DISALLOW_EXCLUSIVE (0x00020000) reply from Windows 7 
                and Windows Server 2008 R2 is the same whether zero or non-zero is used in this field.");

                #endregion
            }
            
            #endregion

            #region Send the SMB_CLOSE request

            // Create the SMB_CLOSE request.
            fileId = createResponse5.SmbParameters.FID;
            CloseRequest = smbClientStack.CreateCloseRequest(fileId);

            // Send the Close request and expect the response in timeout milliseconds.
            smbClientStack.SendPacket(CloseRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server returns a response.
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

            #endregion

            #region Send the SMB_COM_TREE_DISCONNECT request

            // Create the SMB_COM_TREE_DISCONNECT request.
            SmbTreeDisconnectRequestPacket treeDisconnectRequest = smbClientStack.CreateTreeDisconnectRequest(treeId);

            // Send the TreeDisconnect request and expect the response in timeout milliseconds.
            smbClientStack.SendPacket(treeDisconnectRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server returns a response.
            Site.Assert.IsNotNull(
                response,
                "SMB_COM_TREE_DISCONNECT response should not be null.");

            // Check whether server returns a SMB_COM_TREE_DISCONNECT response.
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

            #endregion

            #region Send the SMB_COM_LOGOFF_ANDX request

            // Create the SMB_COM_LOGOFF_ANDX request.
            SmbLogoffAndxRequestPacket logoffRequest = smbClientStack.CreateLogoffRequest(sessionUid);

            // Send the LogOff request and expect the response in timeout milliseconds.
            smbClientStack.SendPacket(logoffRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server returns a response.
            Site.Assert.IsNotNull(
                response,
                "SMB_COM_LOGOFF_ANDX response should not be null.");

            // Check whether server returns a SMB_COM_LOGOFF_ANDX response.
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

            #endregion

            #region Disconnect

            // Disconnect
            smbClientStack.Disconnect();

            Site.Assert.IsFalse(
               smbClientStack.IsDataAvailable,
               "SmbClient should not receive any packet after Disconnect method is called.");

            #endregion
        }
    }
}
