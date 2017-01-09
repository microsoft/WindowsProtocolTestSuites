// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using System.Text;

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Security.Apds;
using Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Apds
{
    /// <summary>
    /// The capture cases for message adapter
    /// </summary>
    public partial class ApdsServerAdapter
    {
        /// <summary>
        /// Verify response data structure for NTLM is NetlogonValidationSamInfo2 or NetlogonValidationSamInfo4
        /// </summary>
        /// <param name="validationLevel">request validation data structure level</param>
        /// <param name="logonLevel">request logon level</param>
        private void VerifyNTLMDataStructure(
            _NETLOGON_VALIDATION_INFO_CLASS validationLevel,
            _NETLOGON_LOGON_INFO_CLASS logonLevel)
        {
            if (validationLevel == _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo)
            {
                //
                //Verify MS-APDS requirment:MS-APDS_R100136
                //
                Site.CaptureRequirementIfAreEqual<_NETLOGON_VALIDATION_INFO_CLASS>(
                    _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo,
                    validationLevel,
                    100136,
                    @"If there is a successful match[the local copy of password matches with the one sent in the 
                    request ], the domain controller MUST return data with ValidationInformation containing a reference 
                    to NETLOGON_VALIDATION_SAM_INFO ([MS-NRPC] section 2.2.1.4.11), if the ValidationLevel in the 
                    request is NetlogonValidationSamInfo.");
            }

            if (validationLevel == _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2)
            {
                //
                //Verify MS-APDS requirment:MS-APDS_R136 
                //this capture function is invoked when the return value is success,
                //It means that the local copy of password must match with the one sent in the request
                //
                Site.CaptureRequirementIfAreEqual<_NETLOGON_VALIDATION_INFO_CLASS>(
                    _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2,
                    validationLevel,
                    136,
                    @"If the local copy of password matches with the one sent in the request, and the  ValidationLevel in 
                    the request is NetlogonValidationSamInfo2, then domain controller MUST return data with ValidationInformation 
                    containing a reference to  NETLOGON_VALIDATION_SAM_INFO2.");
            }

            if (validationLevel == _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4)
            {
                //
                //Verify MS-APDS requirment:MS-APDS_R135
                //
                Site.CaptureRequirementIfAreEqual<_NETLOGON_VALIDATION_INFO_CLASS>(
                    _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4,
                    validationLevel,
                    135,
                    @"If the local copy of password matches with the one sent in the request, and the  ValidationLevel 
                    in the request is NetlogonValidationSamInfo4, then domain controller MUST return data with 
                    ValidationInformation containing a reference to NETLOGON_VALIDATION_SAM_INFO4.");
                //
                //Verify MS-APDS requirment:MS-APDS_R122
                //
                Site.CaptureRequirementIfAreEqual<_NETLOGON_VALIDATION_INFO_CLASS>(
                    _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4,
                    validationLevel,
                    122,
                    @"The DC MUST send the NETLOGON_VALIDATION_SAM_INFO4 structures back to the NTLM server");

                if (logonLevel == _NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation)
                {
                    //
                    //Configured in PTFConfig file,default SHOULD to true and MAY to false.
                    //
                    string isR120Implemented = "true";
                    //
                    //Check OS version
                    //
                    if (isWindows)
                    {
                        //
                        //Verify MS-APDS requirment:MS-APDS_R100120
                        //
                        Site.CaptureRequirementIfAreEqual<int>(
                            6,
                            (int)validationLevel,
                            100120,
                            @"In Windows, in NTLM Network logon, NETLOGON_VALIDATION_SAM_INFO4 data structures are 
                            returned by the server.");

                        if (null == isR120Implemented)
                        {
                            Site.Properties.Add("R120Implemented", Boolean.TrueString);
                            isR120Implemented = Boolean.TrueString;
                        }
                    }
                    if (null != isR120Implemented)
                    {
                        bool implSigns = Boolean.Parse(isR120Implemented);
                        bool isSatisfied = ((int)_NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4 == 6);
                        //
                        //Verify MS-APDS requirment:MS-APDS_R120
                        //
                        Site.CaptureRequirementIfAreEqual<Boolean>(
                            implSigns,
                            isSatisfied,
                            120,
                            string.Format(@"In NTLM Network logon, NETLOGON_VALIDATION_SAM_INFO4 data structures SHOULD be returned 
                                            by the server. This requirement is {0} implemented.",implSigns? "":"not"));
                    }
                }
            }
        }

        /// <summary>
        /// Verify teh message syntx NTML server response NETLOGON_VALIDATION_SAM_INFO4 data structure.
        /// </summary>
        /// <param name="sessionPacket">userSessonkey pack</param>
        /// <param name="userName">the user logon information</param>
        private void VerifyMessageSyntxNTLMValidV4Resp(byte[] sessionPacket, string userName)
        {

            //
            //Verify MS-APDS requirment:MS-APDS_R121
            //username is the important kind of user logging information. 
            //
            Site.CaptureRequirementIfIsNotNull(
                userName,
                121,
                @"The DC MUST populate the NETLOGON_VALIDATION_SAM_INFO4 with the information for the user logging on.");

            //
            //Verify MS-APDS requirment:MS-APDS_R258
            //to verify with IfIsTrue method have three reasons:
            //Firstly.the SessionBaseKey structure do not define in MS-APDS,so it could not be verified by type;
            //Secondly,the userSessionKey's structure is encoded as opaque blobs;
            //Finally,the userSessionKey must be returned with message,so length of pack's byte must more than 0 .
            //
            Site.Log.Add(LogEntryKind.Comment, "R258, session Packet length value: {0}.", sessionPacket.Length);
            Site.CaptureRequirementIfIsTrue(
                sessionPacket.Length > 0,
                258,
                @"The DC MUST return the SessionBaseKey ([MS-NLMP] section 3.3.1 and 3.3.2) in the UserSessionKey field 
                in NETLOGON_VALIDATION_SAM_INFO4.");
        }

        /// <summary>
        /// verify all the server end-point requirements for Digest Validation
        /// when invalid credentials are provided.
        /// </summary>
        /// <param name="statusValue">returned status value</param>
        /// <param name="authdataSize">returned size of authdata</param>
        private void VerifyMessageSyntxDigestInvalidResp(uint statusValue, uint authdataSize)
        {
            //
            //Verify MS-APDS requirment:MS-APDS_R114
            //
            if (statusValue == (uint)Status.LogonFailure)
            {
                Site.CaptureRequirementIfAreEqual<uint>(
                    0,
                    authdataSize,
                    114,
                    @"The length of AuthData field[in the DIGEST_VALIDATION_RESP Message] MUST be 0 if the value of 
                    the 'Status field' is STATUS_LOGON_FAILURE");
            }

        }

        /// <summary>
        /// Helper function to verify all the server end-point requirements for Digest Validation
        /// when valid credentials are provided.
        /// </summary>
        /// <param name="digestResponseStructure">The digest response structure as DIGEST_VALIDATION_RESP.</param>
        /// <param name="validationlevel">The validation level for digest validation.</param>
        /// <param name="digestResponseLength">The length of digest response.</param>
        private void VerifyMessageSyntaxDigestValidCredential(
            DIGEST_VALIDATION_RESP digestResponseStructure,
            _NETLOGON_VALIDATION_INFO_CLASS validationlevel,
            uint digestResponseLength)
        {
           

            //
            // Verify R95
            //
            Site.CaptureRequirementIfAreEqual<uint>(
                // The expected MessageType V2 is equal to 0x0000000a.
                (uint)MessageType.V2,
                //digestResponseStructure.messageType,
                (uint)digestResponseStructure.MessageType,
                95,
                @"'MessageType' filed in the DIGEST_VALIDATION_RESP Message is a 32-bit unsigned integer that MUST 
                specify the Digest validation message type.");

            //
            // Verify R96
            //
            Site.CaptureRequirementIfAreEqual<uint>(
                // The expected MessageType V2 is equal to 0x0000000a.
                (uint)MessageType.V2,
                (uint)digestResponseStructure.MessageType,
                96,
                @"The value of the 'MessageType' field in the DIGEST_VALIDATION_RESP Message MUST 
                be 0x00000000A.");

            //
            // Verify R97
            //
            Site.CaptureRequirementIfAreEqual<ushort>(
                // The expected Version V1 is equal to 0x0001.
                (ushort)Version.V1,
                (ushort)digestResponseStructure.Version,
                97,
                @"'Version' filed in the DIGEST_VALIDATION_RESP Message is a 16-bit unsigned integer
                that MUST specify the version of the Digest validation protocol.");

            //
            // Verify R98
            //
            Site.CaptureRequirementIfAreEqual<ushort>(
                // The expected Version V1 is equal to 0x0001.
                (ushort)Version.V1,
                (ushort)digestResponseStructure.Version,
                98,
                @"The value of 'Version' field in the DIGEST_VALIDATION_RESP Message MUST be 0x0001.");

            //
            // Verify R249
            //
            Site.CaptureRequirementIfAreEqual<uint>(
                // The expected Pad2 equals 0.
                0,
                (uint)digestResponseStructure.Pad2,
                249,
                @"[For 'Pad2' filed in the DIGEST_VALIDATION_RESP Message] The value of this member MUST be set to 
                zero when sent.");

            // The length of expected session key.
            ushort currentSessionKeyLength = (ushort)(digestResponseStructure.SessionKey.Length +
                //Marshal.SizeOf(digestResponseStructure.SessionKey_NULL_terminator));
                Marshal.SizeOf((byte)digestResponseStructure.SessionKey_NULL_terminator));

            //
            // Verify R101
            //
            Site.CaptureRequirementIfAreEqual<ushort>(
                currentSessionKeyLength,
                digestResponseStructure.SessionKeyLength,
                101,
                @"'SessionKeyLength' field in the DIGEST_VALIDATION_RESP Message is a 16-bit unsigned integer that MUST 
                specify the number of bytes of the 'SessionKey field' in the DIGEST_VALIDATION_RESP message plus a 
                terminating null character.");

            //
            // Verify R252
            //
            Site.CaptureRequirementIfAreEqual<uint>(
                // The length of digest session buff.
                33,
                digestResponseStructure.SessionKeyLength,
                252,
                @"For the value in SessionKeyLength field of the DIGEST_VALIDATION_RESP Message]It 
                MUST be equal to 33.");

            //
            // Verify R254
            //
            Site.CaptureRequirementIfAreEqual<uint>(
                // The expected Pad3 equals 0.
                0,
                (uint)digestResponseStructure.Pad3,
                254,
                @"[For 'Pad3' filed in the DIGEST_VALIDATION_RESP Message] The value of this member MUST be set to zero when sent.");

            //
            // Verify R102
            //
            Site.CaptureRequirementIfAreEqual<uint>(
                (uint)digestResponseStructure.AuthData.Length,
                digestResponseStructure.AuthDataSize,
                102,
                @"'AuthDataSize' field in the DIGEST_VALIDATION_RESP Message is a 32-bit unsigned integer that MUST 
                specify the number of bytes of the 'AuthData' field in this( DIGEST_VALIDATION_RESP ) message.");

            //
            // Verify R103
            //
            Site.CaptureRequirementIfAreEqual<ushort>(
                (ushort)digestResponseStructure.AccountName.Length,
                digestResponseStructure.AcctNameSize,
                103,
                @"'AcctNameSize' field in the DIGEST_VALIDATION_RESP Message is a 16-bit unsigned integer that MUST 
                specify the number of bytes of the AccountName field in this( DIGEST_VALIDATION_RESP ) message.");

            //
            // Verify R104
            //
            Site.CaptureRequirementIfAreEqual<ushort>(
                // The expected Reserved1 V1 is equal to 0.
                (ushort)Reserved1.V1,
                (ushort)digestResponseStructure.Reserved1,
                104,
                @"The value of this member['Reserved1' field in the DIGEST_VALIDATION_RESP Message] MUST be set to zero when sent.");

            //
            // Verify R106
            //
            Site.CaptureRequirementIfAreEqual<uint>(
                digestResponseLength,
                digestResponseStructure.MessageSize,
                106,
                @"'MessageSize' field in the DIGEST_VALIDATION_RESP Message is a 32-bit unsigned integer that MUST 
                specify the number of bytes in the entire DIGEST_VALIDATION_RESP message.");

            //
            // Verify R107
            //
            Site.CaptureRequirementIfAreEqual<uint>(
                // The expected Reserved3 V1 is equal to 0.
                (uint)Resp_Reserved3.V1,
                (uint)digestResponseStructure.Reserved3,
                107,
                @"'Reserved3' field in the DIGEST_VALIDATION_RESP Message is a 32-bit unsigned integer field reserved 
                for future use , the value of this member MUST be set to zero when sent.");

            //
            // Verify R109
            //
            Site.CaptureRequirementIfAreEqual<string>(
                digestSessionKey.ToString(),
                Encoding.ASCII.GetString(digestResponseStructure.SessionKey),
                109,
                @"'SessionKey' field in the DIGEST_VALIDATION_RESP Message is a 32-byte buffer that 
                MUST contain the Digest SessionKey.");

            // Get the max length of session key.
            int maxSessionKeyLength = 32;

            //
            // Verify R110
            // Judge whether the size of SessionKeyLength in response structure equals the actual session key length.
            Boolean isSessionkeyVerified = ((digestResponseStructure.SessionKey.Length <= maxSessionKeyLength) 
                && (digestResponseStructure.SessionKeyLength == digestResponseStructure.SessionKey.Length 
                        + Marshal.SizeOf((byte)digestResponseStructure.SessionKey_NULL_terminator)));

            Site.Log.Add(
                LogEntryKind.Comment, 
                "R110, sessionKey length value: {0}, max session length value: {1}",
                digestResponseStructure.SessionKey.Length.ToString(),
                maxSessionKeyLength.ToString());

            Site.CaptureRequirementIfIsTrue(
                isSessionkeyVerified,
                110,
                @"The size of the 'SessionKey' buffer in the DIGEST_VALIDATION_RESP Message is 
                specified by the 'SessionKeyLength' field.");

            //
            // Verify R257
            //
            Site.CaptureRequirementIfAreEqual<uint>(
                // If the validation status is success, the essionKey NULL terminator must be 0.
                0,
                (uint)digestResponseStructure.SessionKey_NULL_terminator,
                257,
                @"For the 'SessionKey NULL terminator' field in the DIGEST_VALIDATION_RESP Message, 
                the value MUST be set to 0.");

            //
            // Verify R111
            //
            Site.CaptureRequirementIfAreEqual<ulong>(
                // The expected Pad1 V1 is equal to 0.
                (ulong)Pad1.V1,
                (ulong)digestResponseStructure.Pad1,
                111,
                @"'Pad1' field in the DIGEST_VALIDATION_RESP Message is an unused 64-bit unsigned integer. 
                The value of this member MUST be set to zero when sent.");

            // Judge whether AuthData in response contains PAC.
            Boolean isAuthDataContainPAC = digestResponseStructure.AuthData.Length > 0 
                && digestResponseStructure.AuthDataSize == (uint)digestResponseStructure.AuthData.Length;

            //
            // Verify R113
            //
            Site.Log.Add(
                LogEntryKind.Comment, 
                "R113, expected value: {0}, actual value: {1}, the authdata size is {2}.","true", 
                isAuthDataContainPAC, digestResponseStructure.AuthDataSize.ToString());

            // If the validation status is success, the judge variable isAuthDataContainPAC must be true.
            Site.CaptureRequirementIfIsTrue(
                isAuthDataContainPAC,
                113,
                @"'AuthData' field in the DIGEST_VALIDATION_RESP Message MUST contain a PACTYPE structure ([MS-PAC]).");

            //
            // Verify R100113
            //
            Site.Log.Add(LogEntryKind.Comment, 
                "R100113, expected value: {0}, actual value: {1}, the authdata size is {2}.", "true", 
                isAuthDataContainPAC, digestResponseStructure.AuthDataSize.ToString());

            // If the validation status is success, the judge variable isAuthDataContainPAC must be true.
            Site.CaptureRequirementIfIsTrue(
                isAuthDataContainPAC,
                100113,
                @"The length of the PACTYPE structure MUST be specified by the 'AuthDataSize' field.");

            //Get the string of NetBIOS name of user account.
            string[] str1 = sutDomainName.Split('.');
            string NetBIOSUserName = str1[0].ToLower() + "\\" + currentUserName.ToLower() + "\0";

            //The string of NetBIOS name of user account in response structure.
            string actualUserName = Encoding.Unicode.GetString(digestResponseStructure.AccountName).ToLower();

            //
            // Verify R115
            //
            Site.CaptureRequirementIfAreEqual<string>(
                NetBIOSUserName,
                actualUserName,
                115,
                @"The length of 'AccountName' [in the DIGEST_VALIDATION_RESP Message] MUST be specified by the 'AcctNameSize' field.");

            //
            // Verify R116
            //
            Site.CaptureRequirementIfAreEqual<ushort>(
                digestResponseStructure.AcctNameSize,
                (ushort)digestResponseStructure.AccountName.Length,
                116,
                @"The length of 'AccountName'[in the DIGEST_VALIDATION_RESP Message] MUST be 
                specified by the 'AcctNameSize' field.");
        
            //
            // Verify R100171
            //
            Site.CaptureRequirementIfAreEqual<uint>(
                (uint)Status.Success,
                digestResponseStructure.Status,
                171,
                @"The DIGEST_VALIDATION_RESP message MUST contain the status of authentication validation");

            //
            // Verify R187
            //
            Site.Log.Add(LogEntryKind.Comment, "R187, whether response structure is null.");

            // The DIGEST_VALIDATION_RESP structure is a contiguous structure.
            // If the validation status is success, the response buffer must be not null.
            Site.CaptureRequirementIfIsNotNull(
                digestResponseStructure,
                187,
                @"The Digest validation response message DIGEST_VALIDATION_RESP MUST be packed as a contiguous buffer");

            //
            // Verify R189
            //
            Site.CaptureRequirementIfAreEqual<_NETLOGON_VALIDATION_INFO_CLASS>(
                _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationGenericInfo2,
                validationlevel,
                189,
                @"The Digest validation response message is sent by using the generic pass-through 
                 mechanism, as specified in [MS-NRPC] section 3.2.4.1.");
            //
            // Verify R172
            //For [MS-APDS] section 2.2.3.2: The AuthData field MUST contain a PACTYPE structure ([MS-PAC] section 2.3). 
            //It means the AuthData field contains the authorization information.
            //So AuthData field which specify the number of bytes of the AuthData field AuthDataSize is more than 0.
            //
            Site.CaptureRequirementIfIsTrue(
                digestResponseStructure.AuthDataSize> 0,
                172,
                "On success of authentication, the DIGEST_VALIDATION_RESP message MUST also contain the user's authorization information<19>");

            //
            // Verify R100185
            //For [MS-APDS] section 2.2.3.2: The AuthData field MUST contain a PACTYPE structure ([MS-PAC] section 2.3). 
            //It means the AuthData field contains the authorization information.
            //So AuthData field which specify the number of bytes of the AuthData field AuthDataSize is more than 0.
            Site.CaptureRequirementIfIsTrue(
                digestResponseStructure.AuthDataSize > 0,
                100185,
                @"If validation is successful, authorization information for the user's account (the PAC) MUST be sent back to the Digest server.");
        }
    }
}
