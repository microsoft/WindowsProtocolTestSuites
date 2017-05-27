// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Security.Apds;
using Microsoft.Protocols.TestTools.StackSdk.Security.Dpsp;
using Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp;
using Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;
using Microsoft.Protocols.TestTools.StackSdk.Security.Pac;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Apds
{
    /// <summary>
    /// Implementation class of IMessageAdapter.
    /// </summary>
    public partial class ApdsServerAdapter : ADCommonServerAdapter, IApdsServerAdapter
    { 
        #region Member Variable Declarations

        /// <summary>
        /// The string of protocol trust domain.
        /// </summary>
        private string sutTrustDomainName;

        /// <summary>
        /// The string of protocol domain name.
        /// </summary>
        private string sutDomainName;

        /// <summary>
        /// The string of protocol client name.
        /// </summary>
        private string clientComputerName;

        /// <summary>
        /// The string of protocol sever name.
        /// </summary>
        private string sutComputerName;

        /// <summary>
        /// The string of protocol sever full name.
        /// </summary>
        private string serverName;

        /// <summary>
        /// The string of tested user name.
        /// </summary>
        private string currentUserName;

        /// <summary>
        /// The string of tested password.
        /// </summary>
        private string currentPassword;

        /// <summary>
        /// The string of Client Computer password.
        /// </summary>
        private string clientComputerPassword;

        /// <summary>
        /// The NtStatus of test returned value from NRPC.
        /// </summary>
        private NtStatus result = NtStatus.STATUS_SUCCESS;

        /// <summary>
        /// The instantce of NrpcClient.
        /// </summary>
        private NrpcClient nrpcClient;

        /// <summary>
        /// The timeout in milliseconds.
        /// </summary>
        private TimeSpan timeout;

        /// <summary>
        /// Whether the server OS is Windows.
        /// </summary>
        private bool isWindows;

        /// <summary>
        /// Digest Session Key.
        /// </summary>
        private string digestSessionKey;

        /// <summary>
        /// The account name for the Managed Service Account for testing authentication policy.
        /// </summary>
        public string ManagedServiceAccountName { get; set; }

        /// <summary>
        /// The account password for the Managed Service Account for testing authentication policy.
        /// </summary>
        public string ManagedServiceAccountPassword { get; set; }

        #endregion

        #region Const strings

        /// <summary>
        /// The random number for client challenge.
        /// </summary>
        private const string clinetChallengeString = "01234567";

        /// <summary>
        /// The packet name of generic information for digest validation.
        /// </summary>
        private const string genericInfoPackageName = "WDigest";

        /// <summary>
        /// The string of nonce count for digest validation.
        /// </summary>
        private const string nonceCountString = "00000001";

        /// <summary>
        /// The accepted AlgType for digest validation.
        /// </summary>
        private const string acceptedAlgType = "MD5-sess";

        /// <summary>
        /// The code page identifier of the preferred encoding, to use the default encoding.
        /// </summary>
        private const string encodingCodePageString = "iso-8859-1";

        /// <summary>
        /// The request method when digest validation based on HTTP.
        /// </summary>
        private const string httpRequestMethodString = "Get";

        /// <summary>
        /// The request method when digest validation based on SASL.
        /// </summary>
        private const string saslRequestMethodString = "AUTHENTICATE";

        /// <summary>
        /// Authentication and integrity protection for the Quality of Protection (QoP) requested by the digest client.
        /// </summary>
        private const string qopTypeV3String = "auth-int";

        /// <summary>
        /// Authentication only for the Quality of Protection (QoP) requested by the digest client.
        /// </summary>
        private const string qopTypeV2String = "auth";

        /// <summary>
        /// Authentication with integrity protection and encryption for the Quality of Protection (QoP) requested by the digest client.
        /// </summary>
        private const string qopTypeV4String = "auth-conf";

        /// <summary>
        /// The zero number for MD5 encryption.
        /// </summary>
        private const string md5EncryptString = "00000000000000000000000000000000";

        #endregion 

        #region Adapter Initialization

        /// <summary>
        /// Initialize the parameters for validation adapter.
        /// </summary>
        /// <param name="testSite">Provides logging, assertions, and SUT adapters for test code onto its execution context.</param>
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);

            Site.DefaultProtocolDocShortName = "MS-APDS";
            if (PDCOSVersion == ServerVersion.NonWin)
            {
                isWindows = false;
            }
            else
            {
                isWindows = true;
            }
            sutComputerName = PDCNetbiosName;
            sutDomainName = PrimaryDomainDnsName;
            sutTrustDomainName = TrustDomainDnsName;
            clientComputerName = ENDPOINTNetbiosName;
            clientComputerPassword = DomainUserPassword;
            timeout = TimeSpan.FromMilliseconds(600000);

            serverName = sutComputerName + "." + sutDomainName;

            ManagedServiceAccountName = GetProperty("MS_APDS.ManagedServiceAccountName");
            ManagedServiceAccountPassword = GetProperty("MS_APDS.ManagedServiceAccountPassword");
        }

        /// <summary>
        /// This method is used to call Dispose(bool) to clean-up codes.
        /// </summary>
        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// This method is used to implement clean-up codes.
        /// </summary>
        /// <param name="disposing">Set TRUE to dispose resource, otherwise set FALSE</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.nrpcClient != null)
                {
                    this.nrpcClient.Dispose();
                    this.nrpcClient = null;
                }
            }
            
        }

        /// <summary>
        /// This method is used to reset the adapter.
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            if (this.nrpcClient != null)
            {
                this.nrpcClient.Dispose();
                this.nrpcClient = null;
            }
        }

        #endregion

        #region Public Interface APIs

        #region NTLMLogon

        /// <summary>
        /// Send NTLM request message to DC for validation.
        /// </summary>
        /// <param name="logonLevel">Indicates the logon level.</param>
        /// <param name="accountInfo">Indicates whether this account is valid in DC validation.</param>       
        /// <param name="isAccessCheckSuccess">Indicates whether the access checked success.</param>
        /// <param name="validationLevel">Indicates the validation level.</param>
        /// <returns>Indicates the result status of DC response.</returns>
        public Status NTLMLogon(
            _NETLOGON_LOGON_INFO_CLASS logonLevel,
            AccountInformation accountInfo,
            Boolean isAccessCheckSuccess,
            _NETLOGON_VALIDATION_INFO_CLASS validationLevel)
        {            
            //Compute the password
            GenerateCurrentCredentials(accountInfo);
            
            //Create Secure Channel
            EstablishSecureChannel();

            _NETLOGON_LEVEL netLogonLevel = new _NETLOGON_LEVEL();
            if (logonLevel == _NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveInformation)
            {
                //Fill InteractiveInfo
                netLogonLevel = ApdsUtility.CreateNlmpInteractiveLogonInfo(
                    NrpcParameterControlFlags.AllowLogonWithComputerAccount,
                    sutTrustDomainName,
                    currentUserName,
                    currentPassword,
                    sutComputerName);
            }
            else if (logonLevel == _NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation)
            {
                //Fill Netlogon_Network_Info
                if (accountInfo == AccountInformation.ManagedServiceAccount)
                {
                    netLogonLevel = nrpcClient.CreateNetlogonLevel(
                        _NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation,
                        NrpcParameterControlFlags.AllowLogonWithComputerAccount,
                        sutDomainName, // managed service account is created in the local domain
                        currentUserName,
                        currentPassword);
                }
                else
                {
                    netLogonLevel = nrpcClient.CreateNetlogonLevel(
                        _NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation,
                        NrpcParameterControlFlags.AllowLogonWithComputerAccount,
                        sutTrustDomainName,
                        currentUserName,
                        currentPassword);
                }
            }

            _NETLOGON_LEVEL encryptedLogonLevel = nrpcClient.EncryptNetlogonLevel(
                logonLevel,
                netLogonLevel);

            byte? authoritative;
            _NETLOGON_VALIDATION? validationInformation = new _NETLOGON_VALIDATION();            
            NrpcNetrLogonSamLogonExtraFlags? extraFlags = NrpcNetrLogonSamLogonExtraFlags.None;

            result = nrpcClient.NetrLogonSamLogonEx(
                nrpcClient.Handle,
                sutComputerName,
                clientComputerName,
                logonLevel,
                encryptedLogonLevel,
                validationLevel,
                out validationInformation,
                out authoritative,
                ref extraFlags);                      
           
            // Whether this method use pass-through mechanism or not.
            bool isPassThroughMethod = false;

            //
            //For NTLM pass-through, the LogonLevel is 1(NetlogonInteractiveInformation) or 2(NetlogonNetworkInformation) 
            //based on Interactive Logono and Network Logon,as defined in [MS-APDS] section 3.1.5.1 and section 3.1.5.2.
            //So when the LogonLevel is 1 or 2, we can say that NRPC pass-through authentication method is used.
            //
            if ((int)logonLevel == 1 || (int)logonLevel == 2 )
            {
                isPassThroughMethod = true;
            }

            //
            //Verify MS-APDS requirment:MS-APDS_R5
            //
            Site.CaptureRequirementIfIsTrue(
                isPassThroughMethod,
                5,
                @"For domain support, authentication protocols MUST use an NRPC pass-through authentication 
                ([MS-NRPC] section 3.2) method with parameters determined by the authentication protocol being 
                used[to return the response structures to member server].");

            //
            //Verify MS-APDS requirment:MS-APDS_R118
            //if isPassThroughMethod is true, this requirement can be verified. 
            //
            Site.CaptureRequirementIfIsTrue(
                isPassThroughMethod,
                118,
                @"NT LAN Manager (NTLM) interactive logon and network logon MUST receive the authentication 
                response sequence by contacting the DC using an NRPC pass-through authentication method .");

            if (result == NtStatus.STATUS_SUCCESS)
            {
                if (validationLevel == _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4)
                {                  
                    VerifyNTLMDataStructure(validationLevel, logonLevel);

                    VerifyMessageSyntxNTLMValidV4Resp(
                        validationInformation.Value.ValidationSam4[0].UserSessionKey.data[0].data,
                        validationInformation.Value.ValidationSam4[0].EffectiveName.ToString());
                }
                else if (validationLevel == _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2)
                {
                    VerifyNTLMDataStructure(validationLevel, logonLevel);
                }
                else if (validationLevel == _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo)
                {
                    VerifyNTLMDataStructure(validationLevel, logonLevel);
                }
            }

            return (Status)result;
        }

        #endregion

        #region GenerateDigestRequest

        /// <summary>
        /// Send Digest request message to DC, and validate the credentials.
        /// </summary>
        /// <param name="isValidationSuccess">Indicates whether the validation from server will be success.</param>
        /// <param name="digestType">Indicates the DigestType field of the request.</param>
        /// <param name="algType">Indicates the AlgType field of the request.</param>
        /// <param name="ignoredFields">It indicates the fields that should be ignored by DC in DIGEST_VALIDATION_REQ.</param>
        /// <returns>Indicates the result status of DC response.</returns>
        public Status GenerateDigestRequest(
            Boolean isValidationSuccess,
            AccountInformation accountInfo,
            DigestType_Values digestType,
            AlgType_Values algType,
            IgnoredFields ignoredFields)
        {
            DIGEST_VALIDATION_REQ digestReq;
            _NETLOGON_LOGON_INFO_CLASS logonLevel = _NETLOGON_LOGON_INFO_CLASS.NetlogonGenericInformation;
            _NETLOGON_VALIDATION_INFO_CLASS validationLevel = _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationGenericInfo2;

            //Compute the password
            GenerateCurrentCredentials(accountInfo);

            //Get Digest Request
            GetDigestRequest(digestType, algType, ignoredFields, out digestReq);

            //Create digest validation logon info
            _NETLOGON_LEVEL netlogonLevel = ApdsUtility.CreateDpspLogonInfo(
                NrpcParameterControlFlags.AllowLogonWithComputerAccount,
                digestReq);
            
            //Create Secure Channel
            EstablishSecureChannel();

            //Client calls EncryptNetlogonLevel
            _NETLOGON_LEVEL encryptedLogonLevel = nrpcClient.EncryptNetlogonLevel(
                (_NETLOGON_LOGON_INFO_CLASS)logonLevel,
                netlogonLevel);

            //Client calls NetrLogonSamLogonEx
            _NETLOGON_VALIDATION? validationInfomation;
            byte? authoritative;
            NrpcNetrLogonSamLogonExtraFlags? extraFlags = NrpcNetrLogonSamLogonExtraFlags.None;
            
            result = nrpcClient.NetrLogonSamLogonEx(
                nrpcClient.Handle,
                sutComputerName,
                clientComputerName,
                logonLevel,
                encryptedLogonLevel,
                validationLevel,
                out validationInfomation,
                out authoritative,
                ref extraFlags);

            // Whether this method use pass-through mechanism or not.
            bool isPassThroughMethod = false;

            //
            //The Digest validation protocol SHOULD use the generic pass-through mechanism.
            //For generic pass-through, the LogonLevel is 4(NetlogonGenericInformation) as defined in [MS-NRPC] section 3.2.4.1.
            //So when the LogonLevel is  4, we can say that NRPC pass-through authentication method is used.
            //
            if ((int)logonLevel == 4)
            {
                isPassThroughMethod = true;
            }
            //
            //Verify MS-APDS requirment:MS-APDS_R5
            //
            Site.CaptureRequirementIfIsTrue(
                isPassThroughMethod,
                5,
                @"For domain support, authentication protocols MUST use an NRPC pass-through authentication 
                ([MS-NRPC] section 3.2) method with parameters determined by the authentication protocol being 
                used[to return the response structures to member server].");

            //
            //Verify MS-APDS requirment:MS-APDS_R15
            //
            Site.CaptureRequirementIfAreEqual<int>(
                5,
                (int)validationLevel,
                15,
                @"Digest response messages MUST be encoded as opaque blobs and transported 
                by the generic pass-through capability of Netlogon.");

            //
            //Configured in PTFConfig file,default SHOULD to true and MAY to false.
            //
            string isR169Implemented = "true";
            //
            //Check OS version
            //
            if (isWindows)
            {
                //
                //Verify MS-APDS requirment:MS-APDS_R100169
                //
                Site.CaptureRequirementIfAreEqual<int>(
                    5,
                    (int)validationLevel,
                    100169,
                    @"In Windows, the Digest validation protocol uses the generic pass-through mechanism.");

                if (null == isR169Implemented)
                {
                    Site.Properties.Add("R169Implemented", Boolean.TrueString);
                    isR169Implemented = Boolean.TrueString;
                }
            }
            if (null != isR169Implemented)
            {
                bool implSigns = Boolean.Parse(isR169Implemented);
                bool isSatisfied = ((int)_NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationGenericInfo2 == 5);
                //
                //Verify MS-APDS requirment:MS-APDS_R169
                //
                Site.CaptureRequirementIfAreEqual<Boolean>(
                    implSigns,
                    isSatisfied,
                    169,
                   string.Format(@"The Digest validation protocol SHOULD use the generic pass-through mechanism. 
                   This requirement is {0} implemented.", implSigns ? "" : "not"));
            }

            byte[] buf;
            bool isDigestResStructure = true;
            DIGEST_VALIDATION_RESP digestValidationResponse = new DIGEST_VALIDATION_RESP(); 
            
            // Judge whether the digest response is exist, validationData is response point. 
            // if validationData equals 0, no response return from NRPC.
            if (validationInfomation.Value.ValidationGeneric2 == null)
            {
                isDigestResStructure = false;
            }
            else
            {
                // decrypt validation info
                buf = NrpcUtility.DecryptBuffer(
                    (nrpcClient.Context.NegotiateFlags & NrpcNegotiateFlags.SupportsAESAndSHA2) == NrpcNegotiateFlags.SupportsAESAndSHA2,
                    nrpcClient.Context.SessionKey,
                    validationInfomation.Value.ValidationGeneric2[0].ValidationData);

                _NETLOGON_VALIDATION decryptValidationInfo = new _NETLOGON_VALIDATION();
                decryptValidationInfo.ValidationGeneric2 = new _NETLOGON_VALIDATION_GENERIC_INFO2[1];
                decryptValidationInfo.ValidationGeneric2[0].ValidationData = buf;

                digestValidationResponse =
                    ApdsUtility.ConvertDataToDigestValidationResponse(decryptValidationInfo);

                VerifyMessageSyntaxDigestValidCredential(digestValidationResponse, validationLevel, (uint)buf.Length);
            }

            if (result != NtStatus.STATUS_SUCCESS)
            {
                //
                //Verify MS-APDS requirment:MS-APDS_R292
                //
                Site.CaptureRequirementIfIsFalse(
                    isDigestResStructure,
                    292,
                    "[If unsuccessful]It[DC] MUST NOT send back the DIGEST_VALIDATION_RESP message.");

                VerifyMessageSyntxDigestInvalidResp((uint)result, digestValidationResponse.AuthDataSize);
            }

            return (Status)result;
        }

        #endregion GenerateDigestRequest

        #region GenerateKerberosValidationRequest

        /// <summary>
        /// Send KERB_VERIFY_PAC_REQUEST message to DC through generic pass-through mechanism
        /// </summary>
        /// <param name="serverSignature">Server Signature in the privilege attribute certificate (PAC)</param>
        /// <param name="kdcSignature">Key Distribution Center (KDC) Signature in the PAC</param>
        /// <returns></returns>
        public Status GenerateKerberosValidationRequest(
                PAC_SIGNATURE_DATA serverSignature,
                PAC_SIGNATURE_DATA kdcSignature)
        {

            _NETLOGON_LOGON_INFO_CLASS logonLevel = _NETLOGON_LOGON_INFO_CLASS.NetlogonGenericInformation;
            _NETLOGON_VALIDATION_INFO_CLASS validationLevel = _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationGenericInfo2;

            //Get Kerberos Request
            KERB_VERIFY_PAC_REQUEST kerberosReq = ApdsUtility.CreateKerbVerifyPacRequest(serverSignature, kdcSignature);

            //Create digest validation logon info
            _NETLOGON_LEVEL netlogonLevel = ApdsUtility.CreatePacLogonInfo(
                NrpcParameterControlFlags.AllowLogonWithComputerAccount,
                PrimaryDomainDnsName,
                DomainAdministratorName,
                PDCNetbiosName,
                kerberosReq);

            //Create Secure Channel
            EstablishSecureChannel();

            //Client calls EncryptNetlogonLevel
            _NETLOGON_LEVEL encryptedLogonLevel = nrpcClient.EncryptNetlogonLevel(
                (_NETLOGON_LOGON_INFO_CLASS)logonLevel,
                netlogonLevel);

            //Client calls NetrLogonSamLogonEx
            _NETLOGON_VALIDATION? validationInfomation;
            byte? authoritative;
            NrpcNetrLogonSamLogonExtraFlags? extraFlags = NrpcNetrLogonSamLogonExtraFlags.None;

            result = nrpcClient.NetrLogonSamLogonEx(
                nrpcClient.Handle,
                sutComputerName,
                clientComputerName,
                logonLevel,
                encryptedLogonLevel,
                validationLevel,
                out validationInfomation,
                out authoritative,
                ref extraFlags);

            // Whether this method use pass-through mechanism or not.
            bool isPassThroughMethod = false;

            //Kerberos PAC validation SHOULD use the generic pass-through mechanism ([MS-NRPC] section 3.2.4.1). 
            //For generic pass-through, the LogonLevel is 4(NetlogonGenericInformation) as defined in [MS-NRPC] section 3.2.4.1.
            //So when the LogonLevel is  4, we can say that NRPC pass-through authentication method is used.
            if ((int)logonLevel == 4)
            {
                isPassThroughMethod = true;
            }
            Site.CaptureRequirementIfIsTrue(
                isPassThroughMethod,
                5,
                @"For domain support, authentication protocols MUST use an NRPC pass-through authentication 
                ([MS-NRPC] section 3.2) method with parameters determined by the authentication protocol being 
                used[to return the response structures to member server].");
            Site.CaptureRequirementIfAreEqual<int>(
                5,
                (int)validationLevel,
                402,
                @"The encoded data SHOULD be sent by using the generic pass-through mechanism ([MS-NRPC] section 3.2.4.1).");

            if (result == NtStatus.STATUS_SUCCESS)
            {
                Site.CaptureRequirementIfIsNull(validationInfomation.Value.ValidationGeneric2[0].ValidationData,403,
                    @"If the checksum is verified, the DC MUST return STATUS_SUCCESS. There is no return message.");
            }
            return (Status)result;
        }

        #endregion GenerateKerberosValidationRequest
        #endregion

        #region Message adapter private Methods supports interface APIs

        /// <summary>
        /// Create a credential for current request according to the account information.
        /// </summary>
        /// <param name="accountInfo">Indicates whether this account is valid in DC validation.</param>
        private void GenerateCurrentCredentials(AccountInformation accountInfo)
        {
            if (accountInfo == AccountInformation.Valid)
            {
                currentUserName = DomainAdministratorName;
                currentPassword = DomainUserPassword;
            }
            else if (accountInfo == AccountInformation.WrongPassword)
            {
                currentUserName = DomainAdministratorName;
                currentPassword = "InvalidPassword";
            }
            else if (accountInfo == AccountInformation.AccountNotExist)
            {
                currentUserName = "InvalidUserName";
                currentPassword = DomainUserPassword;
            }
            else if (accountInfo == AccountInformation.ManagedServiceAccount)
            {
                currentUserName = ManagedServiceAccountName + "$"; // "$" must be appended for managed service account
                currentPassword = ManagedServiceAccountPassword;
            }
        }      

        /// <summary>
        /// Call to this method establishes a secure channel between protocol client and
        /// protocol server and generates a session key, ehich is used for encryption of
        /// request packets and decryption of response packets.
        /// </summary>
        private void EstablishSecureChannel()
        {
            nrpcClient = NrpcClient.CreateNrpcClient(sutDomainName);
            ushort[] endPointList = NrpcUtility.QueryNrpcTcpEndpoint(serverName);

            ushort endPoint = endPointList[0];

            MachineAccountCredential machineCredential = new MachineAccountCredential(
                sutDomainName,
                clientComputerName,
                clientComputerPassword);

            nrpcClient.Context.NegotiateFlags = NrpcNegotiateFlags.SupportsAESAndSHA2
                | NrpcNegotiateFlags.SupportsConcurrentRpcCalls
                | NrpcNegotiateFlags.SupportsCrossForestTrusts
                | NrpcNegotiateFlags.SupportsGenericPassThroughAuthentication
                | NrpcNegotiateFlags.SupportsNetrLogonGetDomainInfo
                | NrpcNegotiateFlags.SupportsNetrLogonSendToSam
                | NrpcNegotiateFlags.SupportsNetrServerPasswordSet2
                | NrpcNegotiateFlags.SupportsRC4
                | NrpcNegotiateFlags.SupportsRefusePasswordChange
                | NrpcNegotiateFlags.SupportsRodcPassThroughToDifferentDomains
                | NrpcNegotiateFlags.SupportsSecureRpc
                | NrpcNegotiateFlags.SupportsStrongKeys
                | NrpcNegotiateFlags.SupportsTransitiveTrusts;

            NrpcClientSecurityContext securityContext = new NrpcClientSecurityContext(
                sutDomainName,
                sutComputerName,
                machineCredential,
                true,
                nrpcClient.Context.NegotiateFlags);

            nrpcClient.BindOverTcp(serverName, endPoint, securityContext, timeout);
        }


        /// <summary>
        /// This method constructs the digest request structure which is required to send from protocol client to protocol server.
        /// </summary>
        /// <param name="digestType">It indicates the DigestType field of the DIGEST_VALIDATION_REQ</param>
        /// <param name="algType">It indicates the AlgType field of the DIGEST_VALIDATION_REQ </param>
        /// <param name="ignoredFields">It indicates the fields that should be ignored by DC in DIGEST_VALIDATION_REQ </param>
        /// <param name="digestReq">The request structure for digest validation</param>
        private void GetDigestRequest(
             DigestType_Values digestType,
             AlgType_Values algType,
             IgnoredFields ignoredFields,
             out DIGEST_VALIDATION_REQ digestReq)
        {
            // create a temporary payload structure
            DIGEST_VALIDATION_REQ_Payload reqLoad = new DIGEST_VALIDATION_REQ_Payload();

            reqLoad.AccountName = currentUserName;
            reqLoad.Algorithm = acceptedAlgType;
            reqLoad.Authzid = "";
            reqLoad.Nonce = GenerateNonce();
            reqLoad.CNonce = reqLoad.Nonce;
            reqLoad.Hentity = "";
            reqLoad.NonceCount = nonceCountString;
            reqLoad.QOP = "";
            reqLoad.Realm = sutDomainName;
            reqLoad.URI = "/";
            reqLoad.Username = currentUserName;

            string servicename = serverName;
            string response = string.Empty;

            if (digestType == DigestType_Values.Basic)
            {
                reqLoad.Method = httpRequestMethodString;

                // calling RFC 2617 algorithms for calculating H(A1) and Response, when HTTP is involved.
                string strHA1 = DigestCalcHA1HTTP(
                    reqLoad.Algorithm,
                    reqLoad.Username,
                    reqLoad.Realm,
                    currentPassword,
                    reqLoad.Nonce,
                    reqLoad.CNonce,
                    false);

                digestSessionKey = strHA1;

                response = DigestCalcResponseHTTP(
                    strHA1,
                    reqLoad.Nonce,
                    reqLoad.NonceCount,
                    reqLoad.CNonce,
                    reqLoad.QOP,
                    reqLoad.Method,
                    reqLoad.URI,
                    reqLoad.Hentity,
                    false);
            }
            else if (digestType == DigestType_Values.SASL)
            {
                reqLoad.Method = saslRequestMethodString;

                // calling RFC 2617 algorithms for calculating H(A1) and Response, when SASL is involved.
                string strHA1 = DigestCalcHA1SASL(
                    reqLoad.Authzid,
                    reqLoad.Username,
                    reqLoad.Realm,
                    currentPassword,
                    reqLoad.Nonce,
                    reqLoad.CNonce,
                    false);

                digestSessionKey = strHA1;

                response = DigestCalcResponseSASL(
                    strHA1,
                    reqLoad.Nonce,
                    reqLoad.NonceCount,
                    reqLoad.CNonce,
                    reqLoad.QOP,
                    reqLoad.URI,
                    false,
                    false);
            }
            string basicDigestCredentialString = "Digest username=" + reqLoad.Username
                + ",realm=" + reqLoad.Realm
                + ",nonce=" + reqLoad.Nonce
                + ",uri=" + reqLoad.URI
                + ",cnonce=" + reqLoad.CNonce
                + ",nc=" + reqLoad.NonceCount
                + ",algorithm=" + reqLoad.Algorithm
                + ",response=" + response
                + ",qop=" + reqLoad.QOP
                + ",charset=" + encodingCodePageString
                + ",service-name=" + servicename;

            DpspResponse dpspResponse = DpspBasicResponse.Decode(basicDigestCredentialString);

            DIGEST_VALIDATION_REQ digestValidationReq = ApdsUtility.CreateDigestValidationRequestPacket(
                clientComputerName,
                string.Empty,
                string.Empty,
                reqLoad.Method,
                digestType,
                acceptedAlgType,
                dpspResponse);

            digestValidationReq.CharsetType = CharsetType_Values.ISO8859_1;
            digestValidationReq.AlgType = algType;
            digestValidationReq.Flags = (DIGEST_VALIDATION_FLAGS)5;
            digestValidationReq.Reserved3 = (DIGEST_VALIDATION_REQ_Reserved3_Values)ignoredFields.reserved3;
            digestValidationReq.Reserved4 = (Reserved4_Values)ignoredFields.reserved4;

            digestReq = digestValidationReq;
        }

        /// <summary>
        /// Generates nonce. Used for nonce and cnonce calculation when Digest Validation protocol is involved.
        /// </summary>
        /// <returns>returns nonce as a string</returns>
        private string GenerateNonce()
        {
            string strNonce;
            Random _fixRand = new Random(500000);
            byte[] binaryData = new byte[24];
            string sTime = DateTime.Now.ToString() + _fixRand.Next().ToString();
            byte[] bData = Encoding.ASCII.GetBytes(sTime.ToCharArray());

            for (int i = 0; i < 24; i++)
            {
                binaryData[i] = bData[i];
            }
            strNonce = System.Convert.ToBase64String(binaryData);
            strNonce = strNonce.ToLower();

            return strNonce;
        }

        /// <summary>
        /// This method is used to calculate HA1 of the Digest request structure, which is one of the parameters 
        /// in the digest request structure during Digest over HTTP. 
        /// This method must be called from GenerateDigestRequest () method.
        /// </summary>
        /// <param name="strAlg">The Alg Type string.</param>
        /// <param name="strUser">The user name string.</param>
        /// <param name="strRealm">The protocol domain name string.</param>
        /// <param name="strPass">The password string.</param>
        /// <param name="strNonce">The Nonce string.</param>
        /// <param name="strCNonce">The Nonce string.</param>
        /// <param name="IsUTF8">Whether the data format is UTF8.</param>
        /// <returns>returns digest HA1 as a string</returns>
        private string DigestCalcHA1HTTP(
            string strAlg,
            string strUser,
            string strRealm,
            string strPass,
            string strNonce,
            string strCNonce,
            bool IsUTF8)
        {
            string sResult = string.Empty;
            MD5 DigestMd5 = MD5.Create();
            byte[] bHA1;

            if (IsUTF8)
            {
                bHA1 = Encoding.UTF8.GetBytes(strUser + ":" + strRealm + ":" + strPass);
            }
            else
            {
                bHA1 = Encoding.GetEncoding(encodingCodePageString).GetBytes(strUser + ":" + strRealm + ":" + strPass);
            }

            byte[] bResult = DigestMd5.ComputeHash(bHA1);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bResult.Length; i++)
            {
                sb.AppendFormat("{0:x2}", bResult[i]);
            }

            if (String.Compare(strAlg, acceptedAlgType) == 0)
            {
                sResult = sb.ToString() + ":" + strNonce + ":" + strCNonce;
                if (IsUTF8)
                {
                    bResult = DigestMd5.ComputeHash(Encoding.UTF8.GetBytes(sResult));
                }
                else
                {
                    bResult = DigestMd5.ComputeHash(Encoding.GetEncoding(encodingCodePageString).GetBytes(sResult));
                }

                sb = new StringBuilder();

                for (int i = 0; i < bResult.Length; i++)
                {
                    sb.AppendFormat("{0:x2}", bResult[i]);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// This method is used to calculate Digest Response of the Digest request structure, 
        /// which is one of the parameters in the digest request structure during Digest over HTTP.
        /// This method must be called from GenerateDigestRequest () method.
        /// </summary>
        /// <param name="strHA1">Digest HA1 as a string.</param>
        /// <param name="strNonce">The Nonce string.</param>
        /// <param name="strNC">The Nonce count string.</param>
        /// <param name="strCNonce">The CNonce string.</param>
        /// <param name="strQop">The QopType string.</param>
        /// <param name="strMethod">The request method</param>
        /// <param name="strURI">The request URI.</param>
        /// <param name="strEntity">The request entity.</param>
        /// <param name="IsUTF8">Whether the data format is UTF8.</param>
        /// <returns>returns digest response as in digest protocl hand shake.</returns>
        private string DigestCalcResponseHTTP(
            string strHA1,
            string strNonce,
            string strNC,
            string strCNonce,
            string strQop,
            string strMethod,
            string strURI,
            string strEntity,
            bool IsUTF8)
        {
            string strHA2 = string.Empty;
            string strAll = string.Empty;

            byte[] bResultHA2;

            //Calculate HA2 for Digest Response
            strHA2 = strMethod + ":" + strURI;
            if (String.Compare(strQop, qopTypeV3String) == 0)
            {
                MD5 Md5Result = MD5.Create();
                string strEntitytemp = string.Empty;
                byte[] bResult;

                if (IsUTF8)
                {
                    bResult = Md5Result.ComputeHash(Encoding.UTF8.GetBytes(strEntity));
                    strEntitytemp = Encoding.UTF8.GetString(bResult);
                }
                else
                {
                    bResult = Md5Result.ComputeHash(Encoding.GetEncoding(encodingCodePageString).GetBytes(strEntity));
                    strEntitytemp = Encoding.GetEncoding(encodingCodePageString).GetString(bResult);
                }
                strHA2 = strHA2 + ":" + strEntitytemp;
            }

            MD5 md5 = MD5.Create();
            if (IsUTF8)
            {
                bResultHA2 = md5.ComputeHash(Encoding.UTF8.GetBytes(strHA2));
            }
            else
            {
                bResultHA2 = md5.ComputeHash(Encoding.GetEncoding(encodingCodePageString).GetBytes(strHA2));
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bResultHA2.Length; i++)
            {
                sb.AppendFormat("{0:x2}", bResultHA2[i]);
            }

            //Calculate HA for Digest Response
            strAll = strHA1 + ":" + strNonce + ":";
            if (!String.IsNullOrEmpty(strQop))
            {
                strAll = strAll + strNC + ":" + strCNonce + ":" + strQop + ":";
            }

            strAll = strAll + sb.ToString();

            MD5 DigestMd5 = MD5.Create();
            byte[] bResultHA;
            if (IsUTF8)
            {
                bResultHA = DigestMd5.ComputeHash(Encoding.UTF8.GetBytes(strAll));
            }
            else
            {
                bResultHA = DigestMd5.ComputeHash(Encoding.GetEncoding(encodingCodePageString).GetBytes(strAll));
            }

            sb = new StringBuilder();
            for (int i = 0; i < bResultHA.Length; i++)
            {
                sb.AppendFormat("{0:x2}", bResultHA[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// This method is used to calculate HA1 of the Digest request structure, which is one of the parameters 
        /// in the digest request structure during Digest over HTTP. 
        /// This method must be called from GenerateDigestRequest () method.
        /// </summary>
        /// <param name="strAuthenzid">The Authenzid string.</param>
        /// <param name="strUser">The user name string.</param>
        /// <param name="strRealm">The protocol domain name string.</param>
        /// <param name="strPass">The password string.</param>
        /// <param name="strNonce">The Nonce string.</param>
        /// <param name="strCNonce">The CNonce string.</param>
        /// <param name="IsUTF8">Whether the data format is UTF8.</param>
        /// <returns>returns digest HA1 as a string</returns>
        private string DigestCalcHA1SASL(
            string strAuthenzid,
            string strUser,
            string strRealm,
            string strPass,
            string strNonce,
            string strCNonce,
            bool IsUTF8)
        {
            string sResult = string.Empty;
            MD5 DigestMd5 = MD5.Create();

            StringBuilder sb;
            List<byte> listTemp = new List<byte>();

            byte[] bHA1;//UTF8
            if (IsUTF8)
            {
                bHA1 = Encoding.UTF8.GetBytes(strUser + ":" + strRealm + ":" + strPass);
            }
            else
            {
                bHA1 = Encoding.GetEncoding(encodingCodePageString).GetBytes(strUser + ":" + strRealm + ":" + strPass);
            }

            byte[] bResult = DigestMd5.ComputeHash(bHA1);
            sResult = ":" + strNonce + ":" + strCNonce;

            if (String.Compare(strAuthenzid, "") != 0)
            {
                sResult = sResult + ":" + strAuthenzid;
            }

            listTemp.AddRange(bResult);
            if (IsUTF8)
            {
                listTemp.AddRange(Encoding.UTF8.GetBytes(sResult));
            }
            else
            {
                listTemp.AddRange(Encoding.GetEncoding(encodingCodePageString).GetBytes(sResult));
            }

            DigestMd5 = MD5.Create();
            bResult = DigestMd5.ComputeHash(listTemp.ToArray());

            sb = new StringBuilder();
            for (int i = 0; i < bResult.Length; i++)
            {
                sb.AppendFormat("{0:x2}", bResult[i]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// This method is used to calculate Digest Response of the Digest request structure, 
        /// which is one of the parameters in the digest request structure during Digest over SASL.
        /// This method must be called from GenerateDigestRequest () method.
        /// </summary>
        /// <param name="strHA1">Digest HA1 as a string.</param>
        /// <param name="strNonce">The Nonce string.</param>
        /// <param name="strNC">The Nonce count string.</param>
        /// <param name="strCNonce">The CNonce string.</param>
        /// <param name="strQop">The QopType string.</param>
        /// <param name="strURI">The request URI.</param>
        /// <param name="IsUTF8">Whether the data format is UTF8.</param>
        /// <param name="IsResponseAuth">Whether the ResponseAuth is true.</param>
        /// <returns>returns digest response as in digest protocl hand shake.</returns>
        private string DigestCalcResponseSASL(
            string strHA1,
            string strNonce,
            string strNC,
            string strCNonce,
            string strQop,
            string strURI,
            bool IsUTF8,
            bool IsResponseAuth)
        {
            string strHA2 = string.Empty;
            string strAll = string.Empty;

            //Calculate HA2 for Digest Response
            if (String.Compare(strQop, qopTypeV2String) == 0)
            {
                if (IsResponseAuth)
                {
                    strHA2 = ":" + strURI;//AUTHENTICATE
                }
                else
                {
                    strHA2 = saslRequestMethodString + ":" + strURI;//AUTHENTICATE
                }
            }

            if (String.Compare(strQop, qopTypeV3String) == 0 || strQop.CompareTo(qopTypeV4String) == 0)
            {
                if (IsResponseAuth)
                {
                    strHA2 = ":" + strURI + ":" + md5EncryptString;//DIGEST-MD5
                }
                else
                {
                    strHA2 = saslRequestMethodString + ":" + strURI + ":" + md5EncryptString;//DIGEST-MD5
                }
            }

            MD5 md5 = MD5.Create();
            byte[] bResultHA2;
            if (IsUTF8)
            {
                bResultHA2 = md5.ComputeHash(Encoding.UTF8.GetBytes(strHA2));
            }
            else
            {
                bResultHA2 = md5.ComputeHash(Encoding.GetEncoding(encodingCodePageString).GetBytes(strHA2));
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bResultHA2.Length; i++)
            {
                sb.AppendFormat("{0:x2}", bResultHA2[i]);
            }

            strAll = strHA1 + ":" + strNonce + ":" + strNC + ":" + strCNonce + ":" + strQop + ":" + sb.ToString();

            MD5 DigestMd5 = MD5.Create();
            byte[] bResultHA;
            if (IsUTF8)
            {
                bResultHA = DigestMd5.ComputeHash(Encoding.UTF8.GetBytes(strAll));
            }
            else
            {
                bResultHA = DigestMd5.ComputeHash(Encoding.GetEncoding(encodingCodePageString).GetBytes(strAll));
            }

            sb = new StringBuilder();
            for (int i = 0; i < bResultHA.Length; i++)
            {
                sb.AppendFormat("{0:x2}", bResultHA[i]);
            }

            return sb.ToString();
        }

        #endregion
    }
}
