// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using StackCifs = Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;
using StackFscc = Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using StackSmb = Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb;

namespace Microsoft.Protocol.TestSuites.Smb
{
    /// <summary>
    /// Summary description for TraditionalTestCases
    /// </summary>
    public partial class TestSuite : TestClassBase
    {
        /// <summary>
        /// Test case of verifying TRANS2_SET_FILE_INFORMATION request.
        /// </summary>
        [TestMethod]
        [TestCategory("Traditional")]
        public void TraditionalTestCase_IgnoreFields_SET_FILE_09_Case()
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

                // Check whether server returns a response.
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
            string path = Site.Properties["SutNtfsShare1FullName"];
            SmbTreeConnectAndxRequestPacket treeconnectRequest =
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
                StackCifs.NtTransactDesiredAccess.GENERIC_ALL,
                StackCifs.SMB_EXT_FILE_ATTR.NONE,
                StackCifs.NtTransactShareAccess.FILE_SHARE_DELETE
                | StackCifs.NtTransactShareAccess.FILE_SHARE_READ
                | StackCifs.NtTransactShareAccess.FILE_SHARE_WRITE,
                StackCifs.NtTransactCreateDisposition.FILE_OPEN_IF,
                NtTransactCreateOptions.FILE_NON_DIRECTORY_FILE,
                StackCifs.NtTransactImpersonationLevel.SEC_IDENTIFY,
                CreateFlags.NT_CREATE_REQUEST_OPLOCK);

            // Send the SMB_COM_NT_CREATE_ANDX request and expect the response in timeout milliseconds.
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

            #region Send the TRANS2_SET_FILE_INFORMATION request

            // Create a TRANS2_SET_FILE_INFORMATION request.
            ushort fileId = createResponse.SmbParameters.FID;
            StackFscc.FileLinkInformation fileLinkInformation = new StackFscc.FileLinkInformation();

            // 1 indicates that if the link already exists, it should be replaced with the new link.
            fileLinkInformation.ReplaceIfExists = 1;

            // The Reserved filed is a 7 bytes array
            fileLinkInformation.Reserved = new byte[7];

            // The name of the newly created link.
            fileLinkInformation.FileName = Encoding.Unicode.GetBytes("wl.txt.lnk");
            fileLinkInformation.FileNameLength = (uint)fileLinkInformation.FileName.Length;

            // 0 indicates a network operations.
            fileLinkInformation.RootDirectory = 0;

            StackFscc.FsccFileLinkInformationRequestPacket linkPacket = 
                new StackFscc.FsccFileLinkInformationRequestPacket();

            linkPacket.Payload = TypeMarshal.ToBytes<StackFscc.FileLinkInformation>(fileLinkInformation);
            byte[] data = linkPacket.ToBytes();
            smbClientStack.Capability.IsUsePassThrough = false;

            // Create a TRANS2_SET_FILE_INFORMATION Request. 
            SmbTrans2SetFileInformationRequestPacket trans2SetFileInformationRequest =
                smbClientStack.CreateTrans2SetFileInformationRequest(
                fileId,
                StackCifs.Trans2SmbParametersFlags.NONE,
                StackCifs.SetInformationLevel.SMB_INFO_STANDARD,
                data);

            // Send the TRANS2_SET_FILE_INFORMATION request and expect the response in timeout milliseconds.
            smbClientStack.SendPacket(trans2SetFileInformationRequest);
            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server returns a response.
            Site.Assert.IsNotNull(
                 response,
                "TRANS2_SET_FILE_INFORMATION response should not be null.");

            // Check whether server returns a TRANS2_SET_FILE_INFORMATION response.
            Site.Assert.IsInstanceOfType(
                response,
                typeof(SmbTrans2SetFileInformationResponsePacket),
                "TRANS2_SET_FILE_INFORMATION response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            SmbTrans2SetFileInformationResponsePacket trans2SetFileInformationResponse =
                (SmbTrans2SetFileInformationResponsePacket)response;
            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                trans2SetFileInformationResponse.SmbHeader.Status,
                "TRANS2_SET_FILE_INFORMATION response status should be SUCCESS.");

            #endregion

            #region Send the TRANS2_SET_FILE_INFORMATION request

            StackFscc.FsccFileLinkInformationRequestPacket linkPacket2 =
                new StackFscc.FsccFileLinkInformationRequestPacket();
            fileLinkInformation = new StackFscc.FileLinkInformation();

            // 1 indicates that if the link already exists, it should be replaced with the new link.
            fileLinkInformation.ReplaceIfExists = 1;

            // The Reserved filed is a 7 bytes array
            fileLinkInformation.Reserved = new byte[7];

            // The name of the newly created link.
            fileLinkInformation.FileName = Encoding.Unicode.GetBytes("wl.txt.lnk");
            fileLinkInformation.FileNameLength = (uint)fileLinkInformation.FileName.Length;

            // 0 indicates a network operations.
            fileLinkInformation.RootDirectory = 0;

            linkPacket2.Payload = TypeMarshal.ToBytes<StackFscc.FileLinkInformation>(fileLinkInformation);
            smbClientStack.Capability.IsUsePassThrough = false;

            // Create a TRANS2_SET_FILE_INFORMATION request.
            SmbTrans2SetFileInformationRequestPacket trans2SetFileInformationRequest2 =
                smbClientStack.CreateTrans2SetFileInformationRequest(
                fileId,
                StackCifs.Trans2SmbParametersFlags.NONE,
                StackCifs.SetInformationLevel.SMB_INFO_STANDARD,
                 linkPacket2.ToBytes());

            // Send the TRANS2_SET_FILE_INFORMATION request and expect the response in timeout milliseconds.
            smbClientStack.SendPacket(trans2SetFileInformationRequest2);
            StackPacket response2 = smbClientStack.ExpectPacket(timeout);

            // Check whether server returns a response.
            Site.Assert.IsNotNull(
                 response2,
                "TRANS2_SET_FILE_INFORMATION response should not be null.");

            // Check whether server returns a TRANS2_SET_FILE_INFORMATION response.
            Site.Assert.IsInstanceOfType(
                response,
                typeof(SmbTrans2SetFileInformationResponsePacket),
                "TRANS2_SET_FILE_INFORMATION response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            SmbTrans2SetFileInformationResponsePacket trans2SetFileInformationResponse2 =
                (SmbTrans2SetFileInformationResponsePacket)response2;
            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                trans2SetFileInformationResponse.SmbHeader.Status,
                "TRANS2_SET_FILE_INFORMATION response status should be SUCCESS.");

            #endregion

            #region Send the SMB_CLOSE request

            // Create the SMB_CLOSE request.
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

            // Check whether server returns a response.
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

            #region Capture requirement r109590

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-SMB_R109590");

            //
            // Verify MS-SMB requirement: MS-SMB_R109590
            //
            bool isVerifyR109590 =
                VerifyisVerifyTrans2SetFileInformation(
                trans2SetFileInformationResponse,
                trans2SetFileInformationResponse2);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR109590,
                109590,
                @"<77> Section 2.2.8.4: Reserved (3 bytes): reply is the same whether zero or non-zero");

            #endregion
        }
    }
}
