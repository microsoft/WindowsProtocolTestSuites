// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Security.Dpsp;
using Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp;
using Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc;
using Microsoft.Protocols.TestTools.StackSdk.Security.Pac;
using System;
using System.IO;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Apds
{
    /// <summary>
    /// Apds utility class.
    /// </summary>
    public static class ApdsUtility
    {
        #region Fields and Consts
        // [ISO/IEC-8859-1] International Organization for Standardization, 
        // "Information Technology -- 8-Bit Single-Byte Coded Graphic Character Sets         
        internal const string ISO_8859_1 = "iso-8859-1";

        // for DPSP, The PackageName ([MS-NRPC] section 3.2.4.1) in the 
        // NETLOGON_GENERIC_INFO structure ([MS-NRPC] section 2.2.1.4.2) MUST be WDigest.         
        internal const string DIGEST_PACKAGENAME = "WDigest";

        // This exchange MUST be layered on top of the Netlogon generic pass-through 
        // ([MS-NRPC] section 3.2.4.1). The server operating system MUST supply a 
        // KERB_VERIFY_PAC_REQUEST structure, packed as a single buffer, 
        // as the LogonData field. The PackageName field MUST be set to a UNICODE_STRING 
        // with a buffer of Kerberos
        internal const string KERBEROS_PACKAGENAME = "Kerberos";

        // Qoptype string value
        // A 16-bit unsigned integer specifying the Quality of Protection (QoP) 
        // requested by the Digest client ([RFC2617] section 3.2.1 and [RFC2831] section 2.1.2.1)
        internal const string QOP_AUTHENTICATION_NAME = "auth";
        internal const string QOP_AUTHENTICATION_AND_INTEGRITY_NAME = "auth-int";
        internal const string QOP_AUTHENTICATION_WITH_INTEGRITY_AND_ENCRYPTION_NAME = "auth-conf";

        // algorithm string name
        internal const string MD5_ALGORITHM_NAME = "md5";
        internal const string MD5_SESS_ALGORITHM_NAME = "md5-sess";

        // SessionKey (32 bytes): A 32-byte buffer that MUST contain the Digest SessionKey 
        // ([RFC2617] section 3.2.2.2).
        internal const int DIGEST_SESSION_KEY_LENGTH = 32;

        internal const int DIGEST_VALIDATION_RESPONSE_PAD4_LENGTH = 7;
        internal const int NLMP_SERVER_CHALLENGE_LENGTH = 8;
        internal const string SPACE = " ";
        internal const string QUOTATION = "\"";
        internal const string HTTP_GET = "GET";
        internal const string HTTP_PUT = "PUT";
        internal const string SASL_AUTHENTICATE = "AUTHENTICATE";

        /// <summary>
        /// null terminator byte
        /// </summary>
        internal const string NULL_TERMINATOR_STRING = "\0";
        #endregion

        /// <summary>
        /// Construct Nlmp pass-through network logon information structure 
        /// from client NTLM authenticate response message
        /// </summary>
        /// <param name="parameterControl">
        /// A set of bit flags that contain information pertaining to the logon validation processing.
        /// </param>
        /// <param name="nlmpAuthenticatePacket">
        /// nlmp authenticate response packet sent from client machine
        /// </param>
        /// <param name="lmChallenge">
        /// nlmp challenge sent from server to client
        /// </param>
        /// <returns>
        /// Nlmp pass-through network logon information
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when nlmpAuthenticatePacket or lmChallenge is null.
        /// </exception>        
        /// <exception cref="ArgumentException">
        /// Thrown when the length of lmChallenge is not equal to 8 bytes
        /// </exception>
        public static _NETLOGON_LEVEL CreateNlmpNetworkLogonInfo(
            NrpcParameterControlFlags parameterControl,
            NlmpAuthenticatePacket nlmpAuthenticatePacket,
            byte[] lmChallenge
            )
        {
            if (nlmpAuthenticatePacket == null)
            {
                throw new ArgumentNullException("nlmpAuthenticatePacket");
            }

            if (lmChallenge == null)
            {
                throw new ArgumentNullException("lmChallenge");
            }

            // ServerChallenge (8 bytes): A 64-bit value that contains the NTLM challenge. 
            // The challenge is a 64-bit nonce. The processing of the 
            // ServerChallenge is specified in sections 3.1.5 and 3.2.5.
            if (lmChallenge.Length != NLMP_SERVER_CHALLENGE_LENGTH)
            {
                throw new ArgumentException(
                    "the length of lmChallenge should be 8 bytes",
                    "lmChallenge");
            }

            string domainName;
            string userName;
            string logonWorkStation;

            _NETLOGON_LEVEL netLogonLevel = new _NETLOGON_LEVEL();
            if (nlmpAuthenticatePacket.Payload.DomainName != null)
            {
                domainName = Encoding.Unicode.GetString(nlmpAuthenticatePacket.Payload.DomainName);
            }
            else
            {
                throw new ArgumentException(
                    "DomainName field should not be null",
                    "nlmpAuthenticatePacket");
            }

            if (nlmpAuthenticatePacket.Payload.UserName != null)
            {
                userName = Encoding.Unicode.GetString(nlmpAuthenticatePacket.Payload.UserName);
            }
            else
            {
                throw new ArgumentException(
                    "UserName field should not be null",
                    "nlmpAuthenticatePacket");
            }

            if (nlmpAuthenticatePacket.Payload.Workstation != null)
            {
                logonWorkStation = Encoding.Unicode.GetString(nlmpAuthenticatePacket.Payload.Workstation);
            }
            else
            {
                throw new ArgumentException(
                    "WorkStation field should not be null",
                    "nlmpAuthenticatePacket");
            }

            //Identity: A NETLOGON_LOGON_IDENTITY_INFO structure, as specified in section MS-NRPC 2.2.1.4.15, 
            //that contains information about the logon identity.
            _NETLOGON_LOGON_IDENTITY_INFO identityInfo = NrpcUtility.CreateNetlogonIdentityInfo(
                parameterControl,
                domainName,
                userName,
                logonWorkStation);

            netLogonLevel.LogonNetwork = new _NETLOGON_NETWORK_INFO[1];
            netLogonLevel.LogonNetwork[0].Identity = identityInfo;
            netLogonLevel.LogonNetwork[0].LmChallenge = new LM_CHALLENGE();
            netLogonLevel.LogonNetwork[0].LmChallenge.data = lmChallenge;
            netLogonLevel.LogonNetwork[0].LmChallengeResponse =
                NrpcUtility.CreateString(nlmpAuthenticatePacket.Payload.LmChallengeResponse);
            netLogonLevel.LogonNetwork[0].NtChallengeResponse =
                NrpcUtility.CreateString(nlmpAuthenticatePacket.Payload.NtChallengeResponse);

            return netLogonLevel;
        }


        /// <summary>
        /// Construct Nlmp pass-through interactive logon information structure 
        /// from client NTLM authenticate response message
        /// </summary>
        /// <param name="parameterControl">A set of bit flags 
        /// that contain information pertaining to the logon validation processing.
        /// </param>
        /// <param name="domainName">domain name</param>
        /// <param name="userName">user name</param>
        /// <param name="password">password</param>
        /// <param name="serverName">NetBIOS name of server </param>
        /// <returns>nlmp interactive logon information structure</returns>        
        public static _NETLOGON_LEVEL CreateNlmpInteractiveLogonInfo(
            NrpcParameterControlFlags parameterControl,
            string domainName,
            string userName,
            string password,
            string serverName
            )
        {
            _NETLOGON_LEVEL netLogonLevel = new _NETLOGON_LEVEL();

            //LmOwfPassword: LM_OWF_PASSWORD structure, as specified in section 2.2.1.1.3, 
            //that contains the LMOWFv1 of a password. 
            //LMOWFv1 is specified in NTLM v1 Authentication in [MS-NLMP] section 3.3.1.
            byte[] lmOwf = NlmpUtility.LmOWF(NlmpVersion.v1, domainName, userName, password);
            //NtOwfPassword: An NT_OWF_PASSWORD structure, as specified in section 2.2.1.1.4, 
            //that contains the NTOWFv1 of a password. 
            //NTOWFv1 is specified in NTLM v1 Authentication in [MS-NLMP] section 3.3.1.
            byte[] ntOwf = NlmpUtility.NtOWF(NlmpVersion.v1, domainName, userName, password);

            //Identity: A NETLOGON_LOGON_IDENTITY_INFO structure, as specified in section MS-NRPC 2.2.1.4.15, 
            //that contains information about the logon identity.
            _NETLOGON_LOGON_IDENTITY_INFO identityInfo = NrpcUtility.CreateNetlogonIdentityInfo(
                parameterControl,
                domainName,
                userName,
                serverName);

            netLogonLevel.LogonInteractive = new _NETLOGON_INTERACTIVE_INFO[1];
            netLogonLevel.LogonInteractive[0].Identity = identityInfo;
            netLogonLevel.LogonInteractive[0].LmOwfPassword = new _LM_OWF_PASSWORD();
            netLogonLevel.LogonInteractive[0].LmOwfPassword.data = NrpcUtility.CreateCypherBlocks(lmOwf);
            netLogonLevel.LogonInteractive[0].NtOwfPassword = new _NT_OWF_PASSWORD();
            netLogonLevel.LogonInteractive[0].NtOwfPassword.data = NrpcUtility.CreateCypherBlocks(ntOwf);

            return netLogonLevel;
        }


        /// <summary>
        /// Create DPSP logon information structure
        /// </summary>
        /// <param name="parameterControl">
        /// A set of bit flags that contain information pertaining to the logon validation processing.
        /// </param>
        /// <param name="digestValidationReq">DIGEST_VALIDATION_REQ structure</param>
        /// <returns>Dpsp netlogon information structure</returns>
        public static _NETLOGON_LEVEL CreateDpspLogonInfo(
            NrpcParameterControlFlags parameterControl,
            DIGEST_VALIDATION_REQ digestValidationReq)
        {
            if (digestValidationReq.Payload == null)
            {
                throw new ArgumentException(
                    "invalid digestValidationReq parameter: the payload field is null",
                    "digestValidationReq");
            }

            _NETLOGON_LEVEL netLogonLevel = new _NETLOGON_LEVEL();

            DIGEST_VALIDATION_REQ_Payload payload =
                DIGEST_VALIDATION_REQ_Payload.Parse(digestValidationReq.Payload);
            byte[] logonData = TypeMarshal.ToBytes<DIGEST_VALIDATION_REQ>(digestValidationReq);

            //Identity: A NETLOGON_LOGON_IDENTITY_INFO structure, as specified in section MS-NRPC 2.2.1.4.15, 
            //that contains information about the logon identity.
            _NETLOGON_LOGON_IDENTITY_INFO identityInfo = NrpcUtility.CreateNetlogonIdentityInfo(
                parameterControl,
                payload.Domain,
                payload.Username,
                payload.ServerName);

            netLogonLevel.LogonGeneric = new _NETLOGON_GENERIC_INFO[1];
            netLogonLevel.LogonGeneric[0].Identity = identityInfo;
            netLogonLevel.LogonGeneric[0].PackageName = DtypUtility.ToRpcUnicodeString(DIGEST_PACKAGENAME);
            netLogonLevel.LogonGeneric[0].LogonData = logonData;
            netLogonLevel.LogonGeneric[0].DataLength = (uint)logonData.Length;

            return netLogonLevel;
        }


        /// <summary>
        /// Construct DIGEST_VALIDATION_REQ structure
        /// </summary>
        /// <param name="serverName">server name</param>
        /// <param name="domainName">domain name</param>
        /// <param name="accountName">account name</param>
        /// <param name="httpMethod">http method</param>
        /// <param name="digestType">digest type</param>
        /// <param name="digestChallengeAlgorithm">digest challenge algorithm</param>
        /// <param name="dpspResponse">dpspResponse class instance</param>
        /// <returns>DIGEST_VALIDATION_REQ structure</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when dpspResponse or httpMethod is null  
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when httpMethod or digestType input is invalid
        /// </exception>        
        public static DIGEST_VALIDATION_REQ CreateDigestValidationRequestPacket(
            string serverName,
            string domainName,
            string accountName,
            string httpMethod,
            DigestType_Values digestType,
            string digestChallengeAlgorithm,
            DpspResponse dpspResponse)
        {
            if (dpspResponse == null)
            {
                throw new ArgumentNullException("dpspResponse");
            }

            if (httpMethod == null)
            {
                throw new ArgumentNullException("httpMethod");
            }

            DIGEST_VALIDATION_REQ_Payload payload = new DIGEST_VALIDATION_REQ_Payload();

            DIGEST_VALIDATION_REQ digestValidationReq = new DIGEST_VALIDATION_REQ();
            if (digestType == DigestType_Values.Basic)
            {
                EncodeCommonPartOfDigestValidationRequest(
                    serverName,
                    domainName,
                    accountName,
                    digestType,
                    digestChallengeAlgorithm,
                    dpspResponse,
                    ref payload,
                    ref digestValidationReq);

                payload.Algorithm = dpspResponse.GetAttributeValue(DpspUtility.ALGORITHM_DIRECTIVE);
                payload.URI = dpspResponse.GetAttributeValue(DpspUtility.BASIC_DIGEST_URI_DIRECTIVE);
                if (httpMethod.ToUpper().Equals(HTTP_GET) || httpMethod.ToLower().Equals(HTTP_PUT))
                {
                    payload.Method = httpMethod;
                }
                else
                {
                    throw new ArgumentException("invalid http method", "httpMethod");
                }
            }
            else if (digestType == DigestType_Values.SASL)
            {
                EncodeCommonPartOfDigestValidationRequest(
                   serverName,
                   domainName,
                   accountName,
                   digestType,
                   digestChallengeAlgorithm,
                   dpspResponse,
                   ref payload,
                   ref digestValidationReq);

                payload.Method = SASL_AUTHENTICATE;
                payload.URI = dpspResponse.GetAttributeValue(DpspUtility.SASL_DIGEST_URI_DIRECTIVE);
                payload.Authzid = dpspResponse.GetAttributeValue(DpspUtility.AUTHZID_DIRECTIVE);
                payload.Hentity = dpspResponse.GetAttributeValue(DpspUtility.HENTITY_DIRECTIVE);

            }
            else
            {
                throw new ArgumentException(
                    "invalid digestType value",
                    "digestType");
            }

            digestValidationReq.Payload = payload.GetBytes();
            digestValidationReq = UpdatePacketInfo(digestValidationReq);

            return digestValidationReq;
        }


        /// <summary>
        /// Construct DIGEST_VALIDATION_RESP structure from Dpsp pass-through validation data 
        /// returned from domain controller.
        /// </summary>
        /// <param name="validationInfo">validation information 
        /// returned from domain controller in Dpsp pass-through authentication
        /// </param>
        /// <returns>DIGEST_VALIDATION_RESP structure</returns>
        /// <exception cref="ArgumentException">
        /// throw when validationInfo input is invalid.
        /// </exception>        
        public static DIGEST_VALIDATION_RESP ConvertDataToDigestValidationResponse(_NETLOGON_VALIDATION validationInfo)
        {
            if (validationInfo.ValidationGeneric2 == null)
            {
                throw new ArgumentException(
                    "ValidationGeneric2 array should not be null",
                    "validationInfo");
            }

            if (validationInfo.ValidationGeneric2[0].ValidationData == null)
            {
                throw new ArgumentException(
                    "ValidationData byte array in ValidationGeneric2 should not be null",
                    "validationInfo");
            }

            DIGEST_VALIDATION_RESP digestValidationResp = new DIGEST_VALIDATION_RESP();
            byte[] validationData = validationInfo.ValidationGeneric2[0].ValidationData;

            using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(validationData)))
            {
                digestValidationResp.MessageType = (MessageType_Values)binaryReader.ReadInt32();
                digestValidationResp.Version = (Version_Values)binaryReader.ReadInt16();
                digestValidationResp.Pad2 = (Pad2_Values)binaryReader.ReadInt16();
                digestValidationResp.Status = (uint)binaryReader.ReadInt32();
                digestValidationResp.SessionKeyLength = (ushort)binaryReader.ReadInt16();

                digestValidationResp.Pad3 = (Pad3_Values)binaryReader.ReadInt16();
                digestValidationResp.AuthDataSize = (uint)binaryReader.ReadInt32();
                digestValidationResp.AcctNameSize = (ushort)binaryReader.ReadInt16();
                digestValidationResp.Reserved1 = (Reserved1_Values)binaryReader.ReadInt16();
                digestValidationResp.MessageSize = (uint)binaryReader.ReadInt32();

                digestValidationResp.Reserved3 = (Reserved3_Values)binaryReader.ReadInt32();
                digestValidationResp.SessionKey = binaryReader.ReadBytes(DIGEST_SESSION_KEY_LENGTH);
                digestValidationResp.SessionKey_NULL_terminator =
                    (SessionKey_NULL_terminator_Values)binaryReader.ReadByte();
                digestValidationResp.Pad4 = new Pad4_Values[DIGEST_VALIDATION_RESPONSE_PAD4_LENGTH];
                byte[] pad4Data = binaryReader.ReadBytes(DIGEST_VALIDATION_RESPONSE_PAD4_LENGTH);
                for (int i = 0; i < DIGEST_VALIDATION_RESPONSE_PAD4_LENGTH; i++)
                {
                    digestValidationResp.Pad4[i] = (Pad4_Values)pad4Data[i];
                }
                digestValidationResp.Pad1 = (Pad1_Values)binaryReader.ReadInt64();
                digestValidationResp.AuthData = binaryReader.ReadBytes((int)digestValidationResp.AuthDataSize);
                digestValidationResp.AccountName = binaryReader.ReadBytes((int)digestValidationResp.AcctNameSize);
            }

            return digestValidationResp;
        }


        /// <summary>
        /// Construct KERB_VERIFY_PAC_REQUEST structure
        /// </summary>
        /// <param name="serverSignature">
        /// PAC_SIGNATURE_DATA Signature value ([MS-PAC] section 2.8) 
        /// for the Server Signature ([MS-PAC] section 2.8.1)
        /// </param>
        /// <param name="kdcSignature">
        /// PAC_SIGNATURE_DATA SignatureType value ([MS-PAC] section 2.8)
        /// for the Key Distribution Center (KDC) Signature ([MS-PAC] section 2.8.1)
        /// </param>
        /// <returns>KERB_VERIFY_PAC_REQUEST structure</returns>        
        public static KERB_VERIFY_PAC_REQUEST CreateKerbVerifyPacRequest(
            PAC_SIGNATURE_DATA serverSignature,
            PAC_SIGNATURE_DATA kdcSignature)
        {
            KERB_VERIFY_PAC_REQUEST kerbVerifyPacRequest = new KERB_VERIFY_PAC_REQUEST();

            // ChecksumAndSignature (variable): The PAC_SIGNATURE_DATA Signature value 
            // ([MS-PAC] section 2.8) for the Server Signature ([MS-PAC] section 2.8.1) in the PAC. 
            // It MUST be followed by the PAC_SIGNATURE_DATA Signature value ([MS-PAC] section 2.8) 
            // for the KDC Signature ([MS-PAC] section 2.8.1) in the PAC.
            int checksumAndSignatureLength = serverSignature.Signature.Length + kdcSignature.Signature.Length;
            byte[] checksumAndSignature = new byte[checksumAndSignatureLength];

            checksumAndSignature = ArrayUtility.ConcatenateArrays<byte>(
                  serverSignature.Signature,
                  kdcSignature.Signature);

            kerbVerifyPacRequest.MessageType = KERB_VERIFY_PAC_REQUEST_MessageType_Values.Default;
            kerbVerifyPacRequest.SignatureType = (uint)kdcSignature.SignatureType;
            kerbVerifyPacRequest.SignatureLength = (uint)kdcSignature.Signature.Length;
            kerbVerifyPacRequest.ChecksumLength = (uint)serverSignature.Signature.Length;
            kerbVerifyPacRequest.ChecksumAndSignature = checksumAndSignature;

            return kerbVerifyPacRequest;
        }


        /// <summary>
        ///  Construct Kerberos PAC pass-through logon information
        /// </summary>
        /// <param name="parameterControl">
        /// A set of bit flags that contain information pertaining to the logon validation processing.
        /// </param>
        /// <param name="domainName">domain name</param>
        /// <param name="userName">user name</param>        
        /// <param name="serverName">NetBIOS name of server </param>
        /// <param name="kerbVerifyPacRequest">KERB_VERIFY_PAC_REQUEST packet</param>
        /// <returns>Kerberos PAC netlogon information structure </returns>        
        public static _NETLOGON_LEVEL CreatePacLogonInfo(
            NrpcParameterControlFlags parameterControl,
            string domainName,
            string userName,
            string serverName,
            KERB_VERIFY_PAC_REQUEST kerbVerifyPacRequest)
        {
            _NETLOGON_LEVEL netLogonLevel = new _NETLOGON_LEVEL();
            byte[] logonData = TypeMarshal.ToBytes<KERB_VERIFY_PAC_REQUEST>(kerbVerifyPacRequest);

            //Identity: A NETLOGON_LOGON_IDENTITY_INFO structure, as specified in section MS-NRPC 2.2.1.4.15, 
            //that contains information about the logon identity.
            _NETLOGON_LOGON_IDENTITY_INFO identityInfo = NrpcUtility.CreateNetlogonIdentityInfo(
                parameterControl,
                domainName,
                userName,
                serverName);

            netLogonLevel.LogonGeneric = new _NETLOGON_GENERIC_INFO[1];
            netLogonLevel.LogonGeneric[0].Identity = identityInfo;
            netLogonLevel.LogonGeneric[0].PackageName = DtypUtility.ToRpcUnicodeString(KERBEROS_PACKAGENAME);
            netLogonLevel.LogonGeneric[0].LogonData = logonData;
            netLogonLevel.LogonGeneric[0].DataLength = (uint)logonData.Length;

            return netLogonLevel;
        }


        /// <summary>
        /// Update the DIGEST_VALIDATION_REQ structure info after modification.
        /// For negative testing, user might need to modify the field of the structure. 
        /// For example, if user change the domain name value, the message size need to 
        /// recaculate after the modification.
        /// </summary>        
        /// <param name="digestValidationReq">DIGEST_VALIDATION_REQ structure</param>
        public static DIGEST_VALIDATION_REQ UpdatePacketInfo(
            DIGEST_VALIDATION_REQ digestValidationReq)
        {
            if (digestValidationReq.Payload == null)
            {
                throw new ArgumentException(
                    "The payload field should not be null",
                    "digestValidationReq");
            }

            DIGEST_VALIDATION_REQ_Payload payload =
                DIGEST_VALIDATION_REQ_Payload.Parse(digestValidationReq.Payload);

            digestValidationReq.AccountNameLength =
                (ushort)ConvertStringToByteArray(payload.AccountName, Encoding.Unicode).Length;
            digestValidationReq.DomainLength =
                (ushort)ConvertStringToByteArray(payload.Domain, Encoding.Unicode).Length;
            digestValidationReq.ServerNameLength =
                (ushort)ConvertStringToByteArray(payload.ServerName, Encoding.Unicode).Length;

            // cacluate the size of payload
            // CharValuesLength specify the number of bytes in the Payload field of the 
            // DIGEST_VALIDATION_REQ message, and MUST NOT exceed the total size in MsgSize
            digestValidationReq.CharValuesLength = (ushort)digestValidationReq.Payload.Length;

            // 16-bit unsigned integer that MUST specify 
            // the total number of bytes in the DIGEST_VALIDATION_REQ message
            byte[] msgData = TypeMarshal.ToBytes<DIGEST_VALIDATION_REQ>(digestValidationReq);
            digestValidationReq.MsgSize = (ushort)msgData.Length;

            return digestValidationReq;
        }


        /// <summary>
        /// Encode common part of DigestValidationRequest
        /// </summary>
        /// <param name="serverName">server name</param>
        /// <param name="domainName">domain name</param>
        /// <param name="accountName">account name</param>
        /// <param name="digestType">digest type</param>
        /// <param name="digestChallengeAlgorithm">digest challenge algorithm</param>
        /// <param name="dpspResponse">dpspResponse</param>
        /// <param name="payload">DIGEST_VALIDATION_REQ_Payload structure</param>
        /// <param name="digestValidationReq">digestValidationReq</param>
        private static void EncodeCommonPartOfDigestValidationRequest(
            string serverName,
            string domainName,
            string accountName,
            DigestType_Values digestType,
            string digestChallengeAlgorithm,
            DpspResponse dpspResponse,
            ref DIGEST_VALIDATION_REQ_Payload payload,
            ref DIGEST_VALIDATION_REQ digestValidationReq
            )
        {
            digestValidationReq.MessageType = DIGEST_VALIDATION_REQ_MessageType_Values.Default;
            digestValidationReq.Version = DIGEST_VALIDATION_REQ_Version_Values.Default;
            digestValidationReq.DigestType = digestType;
            digestValidationReq = SetQopType(dpspResponse, digestValidationReq);
            digestValidationReq = SetAlgType(digestChallengeAlgorithm, digestValidationReq);
            digestValidationReq.CharsetType = CharsetType_Values.UTF8;
            digestValidationReq.NameFormat = NameFormat_Values.None;
            digestValidationReq.Flags =
                DIGEST_VALIDATION_FLAGS.FormatOfUserNameAndRealmIsDeterminedByDC
                | DIGEST_VALIDATION_FLAGS.RequestIsSentFromServer;

            digestValidationReq.Reserved3 = DIGEST_VALIDATION_REQ_Reserved3_Values.Default;
            digestValidationReq.Reserved4 = Reserved4_Values.Default;
            digestValidationReq.Pad1 = DIGEST_VALIDATION_REQ_Pad1_Values.Default;

            // Each of the strings MUST be included. If the string value is empty, 
            // then a terminating null character MUST be used for the value.            
            payload.Username = dpspResponse.GetAttributeValue(DpspUtility.USER_NAME_DIRECTIVE);
            payload.Realm = dpspResponse.GetAttributeValue(DpspUtility.REALM_DIRECTIVE);
            payload.Nonce = dpspResponse.GetAttributeValue(DpspUtility.NONCE_DIRECTIVE);
            payload.CNonce = dpspResponse.GetAttributeValue(DpspUtility.CNONCE_DIRECTIVE);
            payload.NonceCount = dpspResponse.GetAttributeValue(DpspUtility.NONCE_COUNT_DIRECTIVE);
            payload.QOP = dpspResponse.GetAttributeValue(DpspUtility.MESSAGE_QOP_DIRECTIVE);
            payload.Response = dpspResponse.GetAttributeValue(DpspUtility.RESPONSE_DIRECTIVE);

            payload.AccountName = accountName; ;
            payload.ServerName = serverName;
            payload.Domain = domainName;
        }


        /// <summary>
        /// Set alg type
        /// </summary>
        /// <param name="digestChallengeAlgorithm">digest challenge algorithm</param>
        /// <param name="digestValidationReq">DIGEST_VALIDATION_REQ structure</param>
        /// <returns>DIGEST_VALIDATION_REQ structure</returns>
        private static DIGEST_VALIDATION_REQ SetAlgType(
            string digestChallengeAlgorithm,
            DIGEST_VALIDATION_REQ digestValidationReq)
        {
            if (digestChallengeAlgorithm == null || digestChallengeAlgorithm.Length == 0)
            {
                digestValidationReq.AlgType = AlgType_Values.NotPresentAndMD5Assumed;
            }
            else
            {
                switch (digestChallengeAlgorithm.ToLower())
                {
                    case MD5_ALGORITHM_NAME:
                        digestValidationReq.AlgType = AlgType_Values.MD5DigestAndChecksum;
                        break;
                    case MD5_SESS_ALGORITHM_NAME:
                        digestValidationReq.AlgType = AlgType_Values.MD5SessDigestAndChecksum;
                        break;
                    default:
                        throw new ArgumentException("invalid digestChallengeAlgorithm parameter");
                }
            }
            return digestValidationReq;
        }


        /// <summary>
        /// Set Qop type
        /// </summary>
        /// <param name="dpspResponse">DpspResponse instance</param>
        /// <param name="digestValidationReq">DIGEST_VALIDATION_REQ structure</param>
        /// <returns>DIGEST_VALIDATION_REQ structure</returns>
        private static DIGEST_VALIDATION_REQ SetQopType(
            DpspResponse dpspResponse, DIGEST_VALIDATION_REQ digestValidationReq)
        {
            if (dpspResponse.MessageQop == null || dpspResponse.MessageQop.Length == 0)
            {
                digestValidationReq.QopType = QopType_Values.None;
            }
            else
            {
                switch (dpspResponse.MessageQop.ToLower())
                {
                    case QOP_AUTHENTICATION_NAME:
                        digestValidationReq.QopType = QopType_Values.Authenticate;
                        break;
                    case QOP_AUTHENTICATION_AND_INTEGRITY_NAME:
                        digestValidationReq.QopType = QopType_Values.AuthenticateAndIntegrity;
                        break;
                    case QOP_AUTHENTICATION_WITH_INTEGRITY_AND_ENCRYPTION_NAME:
                        digestValidationReq.QopType =
                            QopType_Values.AuthenticateWithIntegrityProtectionAndEncryption;
                        break;
                    default:
                        throw new ArgumentException("invalid QopType value in digestResponse parameter");
                }
            }
            return digestValidationReq;
        }


        /// <summary>
        /// Convert string to byte array
        /// </summary>        
        /// <param name="value">string value</param>
        /// <param name="payloadTextEncoding">payload text encoding</param>
        /// <returns>byte array</returns>
        public static byte[] ConvertStringToByteArray(
            string value,
            Encoding payloadTextEncoding)
        {
            // All strings are the unquoted directive value. All strings MUST be null-terminated; 
            // strings MUST be encoded by using [ISO/IEC-8859-1], unless specified as Unicode. 
            // Each of the strings MUST be included. If the string value is empty, 
            // then a terminating null character MUST be used for the value. 
            // Remember that the last three strings are Unicode strings, 
            // so they have a Unicode terminating null character.
            string payloadString = DpspUtility.TrimQuotationMarks(value) + NULL_TERMINATOR_STRING;

            return payloadTextEncoding.GetBytes(payloadString);
        }
    }
}