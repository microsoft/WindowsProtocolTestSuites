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
        /// Test case of verifying TRANS2_FIND_FIRST2 request.
        /// </summary>
        [TestMethod]
        [TestCategory("Traditional")]
        public void TraditionalTestCase_IgnoreFields_FIND_FIRST2_02_Case()
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

            // TreeConnect
            string path = Site.Properties["SutNtfsShare1FullName"];

            SmbTreeConnectAndxRequestPacket treeconnectRequest =
            smbClientStack.CreateTreeConnectRequest(
            sessionUid,
            path);

            smbClientStack.SendPacket(treeconnectRequest);

            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server return a response.
            Site.Assert.IsNotNull(
                response,
                "SMB_COM_TREE_CONNECT_ANDX response should not be null.");

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

            // The file name can be any string.
            string fileName = "Dir1";

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
                NtTransactCreateOptions.FILE_DIRECTORY_FILE,
                StackCifs.NtTransactImpersonationLevel.SEC_ANONYMOUS,
                CreateFlags.NT_CREATE_REQUEST_OPLOCK);

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
            SmbNtCreateAndxResponsePacket createResponse = (SmbNtCreateAndxResponsePacket)response;
            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                createResponse.SmbHeader.Status,
                "SMB_COM_NT_CREATE_ANDX response status should be SUCCESS.");
            #endregion

            #region Send the TRANS2_FIND_FIRST2 request
            ushort searchCount = ushort.Parse(Site.Properties["SmbTransportSearchCount"]);
            ushort fileId = createResponse.SmbParameters.FID;
            byte length = createResponse.SmbParameters.WordCount;
            SmbTrans2FindFirst2RequestPacket Trans2FindFirst2Request =
                smbClientStack.CreateTrans2FindFirst2Request(
                treeId,
                fileName,
                StackCifs.Trans2SmbParametersFlags.NONE,
                searchCount,
                StackCifs.Trans2FindFlags.SMB_FIND_CONTINUE_FROM_LAST,
                (StackCifs.SmbFileAttributes)0x10,
                StackCifs.Trans2FindFirst2SearchStorageType.FILE_DIRECTORY_FILE,
                true,
                true,
                FindInformationLevel.SMB_INFO_STANDARD);

            smbClientStack.SendPacket(Trans2FindFirst2Request);

            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server return a response.
            Site.Assert.IsNotNull(
                response,
                "TRANS2_FIND_FIRST2 response should not be null.");

            Site.Assert.IsInstanceOfType(
                response,
                typeof(SmbTrans2FindFirst2ResponsePacket),
                "TRANS2_FIND_FIRST2 response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            SmbTrans2FindFirst2ResponsePacket trans2FindFirst2Response = (SmbTrans2FindFirst2ResponsePacket)response;
            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                trans2FindFirst2Response.SmbHeader.Status,
                "TRANS2_FIND_FIRST2 response status should be SUCCESS.");

            #endregion

            #region Send the TRANS2_FIND_NEXT2 request

            ushort searchId = trans2FindFirst2Response.Trans2Parameters.SID;

            // Trans2FindNext2
            uint resumeKey = 0;

            SmbTrans2FindNext2RequestPacket trans2FindNext2Request =
                smbClientStack.CreateTrans2FindNext2Request(
                treeId,
                fileName,
                StackCifs.Trans2SmbParametersFlags.NONE,
                searchCount,
                searchId,
                resumeKey,
                StackCifs.Trans2FindFlags.SMB_FIND_CONTINUE_FROM_LAST,
                false,
                true,
                FindInformationLevel.SMB_INFO_STANDARD);

            smbClientStack.SendPacket(trans2FindNext2Request);

            response = smbClientStack.ExpectPacket(timeout);

            // Check whether server return a response.
            Site.Assert.IsNotNull(
                response,
                "TRANS2_FIND_NEXT2 response should not be null.");

            Site.Assert.IsInstanceOfType(
                response,
                typeof(SmbTrans2FindNext2ResponsePacket),
                "TRANS2_FIND_NEXT2 response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            SmbTrans2FindNext2ResponsePacket smbTrans2FindNext2ResponsePacket1 =
                (SmbTrans2FindNext2ResponsePacket)response;
            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                smbTrans2FindNext2ResponsePacket1.SmbHeader.Status,
                "TRANS2_FIND_NEXT2 response status should be SUCCESS.");

            #endregion

            #region Send the TRANS2_FIND_FIRST2

            SmbTrans2FindFirst2RequestPacket Trans2FindFirst2Request2 =
                smbClientStack.CreateTrans2FindFirst2Request(
                treeId,
                fileName,
                StackCifs.Trans2SmbParametersFlags.NONE,
                searchCount,
                StackCifs.Trans2FindFlags.SMB_FIND_CONTINUE_FROM_LAST,
                (StackCifs.SmbFileAttributes)0x7F,
                StackCifs.Trans2FindFirst2SearchStorageType.FILE_DIRECTORY_FILE,
                true,
                true,
                FindInformationLevel.SMB_INFO_STANDARD);

            smbClientStack.SendPacket(Trans2FindFirst2Request2);

            StackPacket response2 = smbClientStack.ExpectPacket(timeout);

            // Check whether server return a response.
            Site.Assert.IsNotNull(
                response2,
                "TRANS2_FIND_FIRST2 response should not be null.");

            Site.Assert.IsInstanceOfType(
                response2,
                typeof(SmbTrans2FindFirst2ResponsePacket),
                "TRANS2_FIND_FIRST2 response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            SmbTrans2FindFirst2ResponsePacket trans2FindFirst2Response2 = (SmbTrans2FindFirst2ResponsePacket)response2;
            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                trans2FindFirst2Response2.SmbHeader.Status,
                "TRANS2_FIND_FIRST2 response status should be SUCCESS.");

            ushort searchId2 = trans2FindFirst2Response2.Trans2Parameters.SID;

            SmbTrans2FindNext2RequestPacket trans2FindNext2Request2 =
                smbClientStack.CreateTrans2FindNext2Request(
                treeId,
                fileName,
                StackCifs.Trans2SmbParametersFlags.NONE,
                searchCount,
                searchId2,
                resumeKey,
                StackCifs.Trans2FindFlags.SMB_FIND_CONTINUE_FROM_LAST,
                false,
                true,
                FindInformationLevel.SMB_INFO_STANDARD);

            smbClientStack.SendPacket(trans2FindNext2Request2);

            response2 = smbClientStack.ExpectPacket(timeout);

            // Check whether server return a response.
            Site.Assert.IsNotNull(
                response2,
                "TRANS2_FIND_NEXT2 response should not be null.");

            Site.Assert.IsInstanceOfType(
                response2,
                typeof(SmbTrans2FindNext2ResponsePacket),
                "TRANS2_FIND_NEXT2 response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            SmbTrans2FindNext2ResponsePacket smbTrans2FindNext2ResponsePacket2 =
                (SmbTrans2FindNext2ResponsePacket)response2;
            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                smbTrans2FindNext2ResponsePacket2.SmbHeader.Status,
                "TRANS2_FIND_NEXT2 response status should be SUCCESS.");

            #endregion

            #region Capture requirements r109033, r109056

            // if they are the same, verify R109033(R9033) and R109056(R9056).

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-SMB_R109033");

            //
            // Verify MS-SMB requirement: MS-SMB_R109033
            //
            bool isVerifyR109033 =
                VerifySmbTrans2FindNext2ResponsePacket(
                smbTrans2FindNext2ResponsePacket1,
                smbTrans2FindNext2ResponsePacket2);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR109033,
                109033,
                @"[In File System Attribute Extensions]
                Whatever the value of it[any bit that is not listed in this section] is, 
                the receiver's reply is the same.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-SMB_R109056");

            //
            // Verify MS-SMB requirement: MS-SMB_R109056
            //
            bool isVerifyR109056 =
                VerifySmbTrans2FindNext2ResponsePacket(
                smbTrans2FindNext2ResponsePacket1,
                smbTrans2FindNext2ResponsePacket2);

            Site.CaptureRequirementIfIsTrue(
               isVerifyR109056,
               109056,
               @"[In File System Attribute Extensions] whatever the values of Reserved 0xFE007E00: 
               replies are the same no matter what value is used when the message is received.");

            #endregion

            #region Send the SMB_CLOSE request


            // Close
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

            #endregion

            #region Send the SMB_COM_LOGOFF_ANDX request

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
