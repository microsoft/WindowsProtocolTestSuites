// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
        /// Test case of verifying SMB_COM_NEGOTIATE request.
        /// </summary>
        [TestMethod]
        [TestCategory("Traditional")]
        public void TraditionalTestCase_IgnoreFields_Negotiate_03_Case()
        {
            #region Connect

            smbClientStack.Connect(serverName, serverPort, ipVersion, bufferSize);

            #endregion

            #region Send the SMB_COM_NEGOTIATE request

            // Create the SMB_COM_NEGOTIATE request.
            SmbNegotiateRequestPacket negotiateRequest =
                smbClientStack.CreateNegotiateRequest(StackSmb.SignState.NONE,
                new string[] {
                    DialectNameString.PCNET1,
                    DialectNameString.LANMAN10,
                    DialectNameString.WFW10,
                    DialectNameString.LANMAN12,
                    DialectNameString.LANMAN21,
                    DialectNameString.NTLANMAN
                });

            #region Set the request parameters

            StackCifs.SmbHeader negotiateHeader = negotiateRequest.SmbHeader;

            // Set an invalid flag value of 0xFC4C.
            ushort unUsedAndSecSigReq0 = 0xFC4C;
            ushort flag2 = (ushort)negotiateHeader.Flags2;
            negotiateHeader.Flags2 = (StackCifs.SmbFlags2)(ushort)(flag2 & unUsedAndSecSigReq0);

            negotiateRequest.SmbHeader = negotiateHeader;

            #endregion

            // Send the SMB_COM_NEGOTIATE request and expect a response in timeout milliseconds.
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

            #region Disconnect

            smbClientStack.Disconnect();

            #endregion

            #region Connect

            smbClientStack.Connect(serverName, serverPort, ipVersion, bufferSize);

            #endregion

            #region Send the SMB_COM_NEGOTIATE request

            // Create a SMB_COM_NEGOTIATE request.
            SmbNegotiateRequestPacket negotiateRequest2 =
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

            #region Set the request parameters

            StackCifs.SmbHeader negotiateHeader2 = negotiateRequest2.SmbHeader;

            // Set an invalid flag value of 0x3B3.
            ushort unUsedAndSecSigReq1 = 0x03B3;
            flag2 = (ushort)negotiateHeader2.Flags2;
            negotiateHeader2.Flags2 = (StackCifs.SmbFlags2)(ushort)(flag2 | unUsedAndSecSigReq1);

            negotiateRequest2.SmbHeader = negotiateHeader2;

            #endregion

            // Send the SMB_COM_NEGOTIATE request and expect a response in the timeout milliseconds.
            smbClientStack.SendPacket(negotiateRequest2);
            StackPacket response2 = smbClientStack.ExpectPacket(timeout);

            // Check whether server returns a response.
            Site.Assert.IsNotNull(
                response2,
                "SMB_COM_NEGOTIATE response should not be null.");

            // Check whether server returns a SMB_COM_NEGOTIATE response.
            Site.Assert.IsInstanceOfType(
                response2,
                typeof(SmbNegotiateResponsePacket),
                "SMB_COM_NEGOTIATE response should be received.");

            // Check the response validity by verifying the Status field in the SMB Header packet.
            SmbNegotiateResponsePacket negotiateResponse2 = (SmbNegotiateResponsePacket)response2;
            Site.Assert.AreEqual<uint>(
                (uint)SmbStatus.STATUS_SUCCESS,
                negotiateResponse2.SmbHeader.Status,
                "SMB_COM_NEGOTIATE response status should be SUCCESS.");

            #endregion

            #region Capture requirements r5277, r5298, 108

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-SMB_R5277");

            //
            // Verify MS-SMB requirement: MS-SMB_R5277
            //
            bool isVerifyR5277 = VerifyNegotiateResponse(negotiateResponse, negotiateResponse2);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR5277,
                5277,
                @"[In SMB Header Extensions]Flags2 (2 bytes): 
                Reply is the same whether 0 or non-zero is used for Field.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-SMB_R5298");

            //
            // Verify MS-SMB requirement: MS-SMB_R5298
            //
            bool isVerifyR5298 = VerifyNegotiateResponse(negotiateResponse, negotiateResponse2);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR5298,
                5298,
                @"[In SMB Header Extensions]Whatever the values of this flag
                [Flags2: SMB_FLAGS2_SMB_SECURITY_SIGNATURE_REQUIRED 0x0010] on other requests[except the first
                SMB_COM_SESSION_SETUP_ANDX request], servers' reply are the same.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-SMB_R108");

            //
            // Verify MS-SMB requirement: MS-SMB_R108
            //
            bool isVerifyR108 = VerifyNegotiateResponse(negotiateResponse, negotiateResponse2);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR108,
                108,
                @"<19> Section 2.2.3.1: Windows-based  Windows servers set the bits[Unused bit fields]
                in the Flags2 field with the same value(s) that were sent by the client in the request.");
            #endregion

            #region Disconnect

            // 7.Disconnect.
            smbClientStack.Disconnect();

            #endregion
        }
    }
}
