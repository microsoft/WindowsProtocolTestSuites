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
        /// Test case of verifying SMB_COM_TREE_CONNECT_ANDX request.
        /// </summary>
        [TestMethod]
        [TestCategory("Traditional")]
        public void TraditionalTestCase_IgnoreFields_Tree_04_Case()
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

            // Create a SMB_COM_TREE_CONNECT_ANDX request.
            string path = Site.Properties["SutNtfsShare1FullName"];
            SmbTreeConnectAndxRequestPacket treeconnectRequest =
                smbClientStack.CreateTreeConnectRequest(sessionUid, path);

            #region Set the request parameters

            StackCifs.SMB_COM_TREE_CONNECT_ANDX_Request_SMB_Parameters treeconnectRequestSmbParameters
                = treeconnectRequest.SmbParameters;

            // The Flags field is set to:TREE_CONNECT_ANDX_DISCONNECT_TID(0x0001),
            // TREE_CONNECT_ANDX_EXTENDED_SIGNATURES(0x0004), TREE_CONNECT_ANDX_EXTENDED_RESPONSE(0x0008).
            treeconnectRequestSmbParameters.Flags =
                (ushort)(treeconnectRequestSmbParameters.Flags & SmbTreeConnectAndxFlags);

            treeconnectRequest.SmbParameters = treeconnectRequestSmbParameters;

            #endregion

            // Send the SMB_COM_TREE_CONNECT_ANDX request and expect a response in the timeout milliseconds.
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

            #region Send the SMB_COM_TREE_CONNECT_ANDX request

            // Create the SMB_COM_TREE_CONNECT_ANDX request.
            path = Site.Properties["SutNtfsShare1FullName"];
            treeconnectRequest = smbClientStack.CreateTreeConnectRequest(sessionUid, path);

            //The bits not listed in TD 2.2.4.7.1 of Flags field set to 0.
            //Other fields set according to TD.
            treeconnectRequestSmbParameters = treeconnectRequest.SmbParameters;

            // The Flags field is set to:TREE_CONNECT_ANDX_DISCONNECT_TID(0x0001),
            // TREE_CONNECT_ANDX_EXTENDED_SIGNATURES(0x0004), TREE_CONNECT_ANDX_EXTENDED_RESPONSE(0x0008).
            treeconnectRequestSmbParameters.Flags =
                (ushort)(treeconnectRequestSmbParameters.Flags & SmbTreeConnectAndxFlags);

            treeconnectRequest.SmbParameters = treeconnectRequestSmbParameters;

            // Send the SMB_COM_TREE_CONNECT_ANDX request and expect the response in the timeout milliseconds.
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
            SmbTreeConnectAndxResponsePacket treeConnectResponse2 = (SmbTreeConnectAndxResponsePacket)response;
            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                treeConnectResponse2.SmbHeader.Status,
                "SMB_COM_TREE_CONNECT_ANDX response status should be SUCCESS.");

            #endregion

            #region Capture requirement r5596

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-SMB_R5596");

            //
            // Verify MS-SMB requirement: MS-SMB_R5596
            //
            bool isVerifyR5596 = VerifyTreeConnectResponse(treeConnectResponse, treeConnectResponse2);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR5596,
                5596,
                @"[In Client Request Extensions]Flags (2 bytes):
                whatever these values are, the server's reply is the same.");

            #endregion
        }
    }
}
