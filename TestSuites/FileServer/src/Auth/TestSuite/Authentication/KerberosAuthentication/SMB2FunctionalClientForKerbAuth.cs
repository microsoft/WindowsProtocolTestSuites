// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestSuites.FileSharing.Auth.TestSuite
{
    /// <summary>
    /// Extended Smb2FunctionalClient for Kerberos Authentication
    /// </summary>
    public class Smb2FunctionalClientForKerbAuth : Smb2FunctionalClient
    {
        /// <summary>
        /// Initializes a new instance of the Smb2FunctionalClientForKerbAuth class that
        /// has specified Timeout and TestSite.
        /// </summary>
        /// <param name="timeout">The specified Timeout when receiving request</param>
        /// <param name="baseTestSite">The specified TestSite</param>
        public Smb2FunctionalClientForKerbAuth(TimeSpan timeout, TestConfigBase testConfig, ITestSite baseTestSite)
            : base(timeout, testConfig, baseTestSite)
        {

        }

        /// <summary>
        /// Send an SMB2 SESSION_SETUP request with specified parameters
        /// </summary>
        /// <param name="headerFlags">A Flags field indicates how to process the operation.</param>
        /// <param name="sessionSetupFlags">To bind an existing session to a new connection,set to SMB2_SESSION_FLAG_BINDING to bind; otherwise set it to NONE.</param>
        /// <param name="securityMode">The security mode field specifies whether SMB signing is enabled, required at the server, or both</param>
        /// <param name="capabilities">Specifies protocol capabilities for the client.</param>
        /// <param name="token">Gss token to send</param>
        /// <param name="serverToken">GssToken returned from server</param>
        /// <param name="creditRequest">The number of credits the client is requesting. Default value is 64.</param>
        /// <param name="previousSessionId">For reconnect, set it to previous sessionId, otherwise set it to 0. Default value is 0.</param>
        /// <returns>The status code for SESSION_SETUP Response.</returns>
        public uint SessionSetup(
            Packet_Header_Flags_Values headerFlags,
            SESSION_SETUP_Request_Flags sessionSetupFlags,
            SESSION_SETUP_Request_SecurityMode_Values securityMode,
            SESSION_SETUP_Request_Capabilities_Values capabilities,
            byte[] token,
            out byte[] serverToken,
            ushort creditRequest = 64,
            ulong previousSessionId = 0)
        {
            Packet_Header header;
            SESSION_SETUP_Response sessionSetupResponse;

            ulong messageId = generateMessageId(sequenceWindow);
            ushort creditCharge = generateCreditCharge(1);

            // Need to consume credit from sequence window first according to TD
            ConsumeCredit(messageId, creditCharge);

            uint status = Smb2Client.SessionSetup(
                1,
                creditRequest,
                headerFlags,
                messageId,
                sessionId,
                sessionSetupFlags,
                securityMode,
                capabilities,
                previousSessionId,
                token,
                out sessionId,
                out serverToken,
                out header,
                out sessionSetupResponse);

            ProduceCredit(messageId, header);
            
            return status;
        }

        /// <summary>
        /// Set signing and encryption session wide.
        /// </summary>
        /// <param name="enableSigning">True if signing is enabled, false otherwise. Only valid when encryption is disabled.</param>
        /// <param name="enableEncryption"True if encryption is enabled, false otherwise.></param>
        /// <param name="key">Crypto key used for computing SessionKey</param>
        public void SetSessionSigningAndEncryption(
            bool enableSigning,
            bool enableEncryption,
            byte[] key)
        {
            client.GenerateCryptoKeys(
                sessionId,
                key,
                enableSigning,
                enableEncryption,
                null,
                false);
        }

    }
}
