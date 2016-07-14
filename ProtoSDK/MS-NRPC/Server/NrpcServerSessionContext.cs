// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc
{
    /// <summary>
    /// A class contains context information of a NRPC session
    /// </summary>
    public class NrpcServerSessionContext : NrpcContext
    {
        //The last request received.
        private NrpcRequestStub requestReceived;

        private RpceServerSessionContext underlyingSessionContext;

        private Dictionary<string, NrpcClientSessionInfo> clientSessionInfoTable;

        /// <summary>
        /// The last RPC request received.
        /// </summary>
        internal Dictionary<string, NrpcClientSessionInfo> ClientSessionInfoTable
        {
            set
            {
                clientSessionInfoTable = value;
            }
        }

        /// <summary>
        /// The last RPC request received.
        /// </summary>
        public NrpcRequestStub LastRequestReceived
        {
            get
            {
                return requestReceived;
            }
            set
            {
                requestReceived = value;
            }
        }


        /// <summary>
        /// The corresponding RPCE layer session context
        /// </summary>
        public RpceServerSessionContext RpceLayerSessionContext
        {
            get
            {
                return underlyingSessionContext;
            }
            internal set
            {
                underlyingSessionContext = value;
            }
        }


        /// <summary>
        /// Initialize a NRPC server session context class.
        /// </summary>
        internal NrpcServerSessionContext()
        {
        }


        /// <summary>
        /// Calculates session key based on negotiation flags
        /// </summary>
        private void ComputeSessionKey()
        {
            if (SharedSecret == null)
            {
                throw new InvalidOperationException("Not able to calculate session key, SharedSecret is null");
            }

            if (ClientChallenge == null)
            {
                throw new InvalidOperationException("Not able to calculate session key, ClientChallenge is null");
            }

            if (ServerChallenge == null)
            {
                throw new InvalidOperationException("Not able to calculate session key, ServerChallenge is null");
            }

            if ((NegotiateFlags & NrpcNegotiateFlags.SupportsAESAndSHA2) != 0)
            {
                SessionKey = NrpcUtility.ComputeSessionKey(NrpcComputeSessionKeyAlgorithm.HMACSHA256,
                            SharedSecret, ClientChallenge, ServerChallenge);
            }
            else if ((NegotiateFlags & NrpcNegotiateFlags.SupportsStrongKeys) != 0)
            {
                SessionKey = NrpcUtility.ComputeSessionKey(NrpcComputeSessionKeyAlgorithm.MD5,
                            SharedSecret, ClientChallenge, ServerChallenge);
            }
            else
            {
                SessionKey = NrpcUtility.ComputeSessionKey(NrpcComputeSessionKeyAlgorithm.DES,
                            SharedSecret, ClientChallenge, ServerChallenge);
            }
        }


        /// <summary>
        /// Validates the client credential.
        /// </summary>
        /// <param name="sessionKey">the session key negotiated</param>
        /// <param name="negotiateFlags">the negotiation flags</param>
        ///  <param name="clientChallenge">client challenge received</param>
        /// <param name="clientCredentialReceived">the client credential received</param>
        private void ValidateClientCredential(byte[] sessionKey, uint negotiateFlags, byte[] clientChallenge,
            _NETLOGON_CREDENTIAL? clientCredentialReceived)
        {
            if (clientCredentialReceived == null || clientCredentialReceived.Value.data == null)
            {
                throw new InvalidOperationException("Client Credential doesn't match");
            }

            byte[] clientCredentialComputed = NrpcUtility.ComputeNetlogonCredential(sessionKey, negotiateFlags,
                clientChallenge);

            bool compareResult = ArrayUtility.CompareArrays<byte>(clientCredentialReceived.Value.data,
                clientCredentialComputed);

            if (!compareResult)
            {
                throw new InvalidOperationException("Client Credential doesn't match");
            }
            StoredCredential = clientCredentialComputed;
        }


        /// <summary>
        /// Validates the client authenticator
        /// </summary>
        /// <param name="clientAuthenticator">the client authenticator</param>
        private void ValidateNetlogonAuthenticator(_NETLOGON_AUTHENTICATOR? clientAuthenticator)
        {
            if (SessionKey == null || StoredCredential == null)
            {
                throw new InvalidOperationException("Unable to validate client authenticator");
            }

            if (!clientAuthenticator.HasValue)
            {
                return;
            }

            NrpcComputeNetlogonCredentialAlgorithm algorithm;

            if ((NegotiateFlags & NrpcNegotiateFlags.SupportsAESAndSHA2) != 0)
            {
                algorithm = NrpcComputeNetlogonCredentialAlgorithm.AES128;
            }
            else
            {
                algorithm = NrpcComputeNetlogonCredentialAlgorithm.DESECB;
            }

            byte[] serverStoredCredential = StoredCredential;
            if (!NrpcUtility.ValidateClientNetlogonAuthenticator(
                clientAuthenticator.Value, algorithm, ref serverStoredCredential, SessionKey))
            {
                throw new InvalidOperationException("Client authenticator isn't correct");
            }
            StoredCredential = serverStoredCredential;
        }


        /// <summary>
        /// Check whether secure channel exists for a nrpc session.
        /// </summary>
        private void CheckForSecureChannel()
        {
            if (SessionKey == null || StoredCredential == null)
            {
                if (RpceLayerSessionContext == null || RpceLayerSessionContext.SecurityContext == null ||
                    !(RpceLayerSessionContext.SecurityContext is NrpcServerSecurityContext))
                {
                    throw new InvalidOperationException(
                        "A call to a method which requires a secure channel is received, "
                        + "but the secure channel isn't established yet");
                }
                else
                {
                    //copy session key and stored credential from security context
                    SessionKey = RpceLayerSessionContext.SecurityContext.SessionKey;
                    StoredCredential = RpceLayerSessionContext.SecurityContext.Token;
                }
            }
        }


        /// <summary>
        ///  Update the session context after receiving a request from the client.
        /// </summary>
        /// <param name="messageReceived">The NRPC request received</param>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmantainableCode")]
        internal void UpdateSessionContextWithMessageReceived(NrpcRequestStub messageReceived)
        {
            requestReceived = messageReceived;

            switch(messageReceived.Opnum)
            {
                case NrpcMethodOpnums.NetrLogonSamLogon:
                    CheckForSecureChannel();
                    NrpcNetrLogonSamLogonRequest nrpcNetrLogonSamLogonRequest = 
                        messageReceived as NrpcNetrLogonSamLogonRequest;

                    if (nrpcNetrLogonSamLogonRequest == null)
                    {
                        throw new InvalidOperationException("messageReceived doesn't match its MethodOpnums");
                    }

                    ValidateNetlogonAuthenticator(nrpcNetrLogonSamLogonRequest.Authenticator);
                    break;

                case NrpcMethodOpnums.NetrLogonSamLogoff:
                    CheckForSecureChannel();
                    NrpcNetrLogonSamLogoffRequest nrpcNetrLogonSamLogoffRequest =
                        messageReceived as NrpcNetrLogonSamLogoffRequest;

                    if (nrpcNetrLogonSamLogoffRequest == null)
                    {
                        throw new InvalidOperationException("messageReceived doesn't match its MethodOpnums");
                    }

                    ValidateNetlogonAuthenticator(nrpcNetrLogonSamLogoffRequest.Authenticator);
                    break;

                case NrpcMethodOpnums.NetrServerReqChallenge:
                    NrpcNetrServerReqChallengeRequest nrpcNetrServerReqChallengeRequest =
                        messageReceived as NrpcNetrServerReqChallengeRequest;

                    if (nrpcNetrServerReqChallengeRequest == null)
                    {
                        throw new InvalidOperationException("messageReceived doesn't match its MethodOpnums");
                    }
                    ClientComputerName = nrpcNetrServerReqChallengeRequest.ComputerName;
                    ClientChallenge = nrpcNetrServerReqChallengeRequest.ClientChallenge.Value.data;
                    break;

                case NrpcMethodOpnums.NetrServerAuthenticate:
                    NrpcNetrServerAuthenticateRequest authenticateRequest =
                        (messageReceived as NrpcNetrServerAuthenticateRequest);

                    if (authenticateRequest == null)
                    {
                        throw new InvalidOperationException("messageReceived doesn't match its MethodOpnums");
                    }
                    ClientComputerName = authenticateRequest.ComputerName;
                    AccountName = authenticateRequest.AccountName;
                    SecureChannelType = authenticateRequest.SecureChannelType;
                    ComputeSessionKey();
                    ValidateClientCredential(SessionKey, (uint)NegotiateFlags, ClientChallenge,
                        authenticateRequest.ClientCredential);
                    break;

                case NrpcMethodOpnums.NetrServerPasswordSet:
                    CheckForSecureChannel();
                    NrpcNetrServerPasswordSetRequest nrpcNetrServerPasswordSetRequest =
                        messageReceived as NrpcNetrServerPasswordSetRequest;

                    if (nrpcNetrServerPasswordSetRequest == null)
                    {
                        throw new InvalidOperationException("messageReceived doesn't match its MethodOpnums");
                    }

                    ValidateNetlogonAuthenticator(nrpcNetrServerPasswordSetRequest.Authenticator);
                    break;

                case NrpcMethodOpnums.NetrDatabaseDeltas:
                    CheckForSecureChannel();
                    NrpcNetrDatabaseDeltasRequest nrpcNetrDatabaseDeltasRequest =
                        messageReceived as NrpcNetrDatabaseDeltasRequest;

                    if (nrpcNetrDatabaseDeltasRequest == null)
                    {
                        throw new InvalidOperationException("messageReceived doesn't match its MethodOpnums");
                    }

                    ValidateNetlogonAuthenticator(nrpcNetrDatabaseDeltasRequest.Authenticator);
                    break;

                case NrpcMethodOpnums.NetrDatabaseSync:
                    CheckForSecureChannel();
                    NrpcNetrDatabaseSyncRequest nrpcNetrDatabaseSyncRequest =
                        messageReceived as NrpcNetrDatabaseSyncRequest;

                    if (nrpcNetrDatabaseSyncRequest == null)
                    {
                        throw new InvalidOperationException("messageReceived doesn't match its MethodOpnums");
                    }
                    ValidateNetlogonAuthenticator(nrpcNetrDatabaseSyncRequest.Authenticator);
                    break;

                case NrpcMethodOpnums.NetrAccountDeltas:
                    CheckForSecureChannel();
                    NrpcNetrAccountDeltasRequest nrpcNetrAccountDeltasRequest =
                        messageReceived as NrpcNetrAccountDeltasRequest;

                    if (nrpcNetrAccountDeltasRequest == null)
                    {
                        throw new InvalidOperationException("messageReceived doesn't match its MethodOpnums");
                    }

                    ValidateNetlogonAuthenticator(nrpcNetrAccountDeltasRequest.Authenticator);
                    break;

                case NrpcMethodOpnums.NetrAccountSync:
                    CheckForSecureChannel();
                    NrpcNetrAccountSyncRequest nrpcNetrAccountSyncRequest =
                        messageReceived as NrpcNetrAccountSyncRequest;

                    if (nrpcNetrAccountSyncRequest == null)
                    {
                        throw new InvalidOperationException("messageReceived doesn't match its MethodOpnums");
                    }

                    ValidateNetlogonAuthenticator(nrpcNetrAccountSyncRequest.Authenticator);
                    break;

                case NrpcMethodOpnums.NetrServerAuthenticate2:
                    NrpcNetrServerAuthenticate2Request authenticate2Request =
                        (messageReceived as NrpcNetrServerAuthenticate2Request);

                    if (authenticate2Request == null)
                    {
                        throw new InvalidOperationException("messageReceived doesn't match its MethodOpnums");
                    }

                    if (authenticate2Request.NegotiateFlags == null)
                    {
                        throw new InvalidOperationException("NegotiateFlags in the request isn't present");
                    }
                    NegotiateFlags = (NrpcNegotiateFlags)authenticate2Request.NegotiateFlags.Value;
                    SecureChannelType = authenticate2Request.SecureChannelType;
                    AccountName = authenticate2Request.AccountName;
                    ClientComputerName = authenticate2Request.ComputerName;

                    ComputeSessionKey();
                    ValidateClientCredential(SessionKey, (uint)NegotiateFlags, ClientChallenge,
                        authenticate2Request.ClientCredential);
                    break;

                case NrpcMethodOpnums.NetrDatabaseSync2:
                    CheckForSecureChannel();
                    NrpcNetrDatabaseSync2Request nrpcNetrDatabaseSync2Request =
                        messageReceived as NrpcNetrDatabaseSync2Request;

                    if (nrpcNetrDatabaseSync2Request == null)
                    {
                        throw new InvalidOperationException("messageReceived doesn't match its MethodOpnums");
                    }

                    ValidateNetlogonAuthenticator(nrpcNetrDatabaseSync2Request.Authenticator);
                    break;

                case NrpcMethodOpnums.NetrDatabaseRedo:
                    CheckForSecureChannel();
                    NrpcNetrDatabaseRedoRequest nrpcNetrDatabaseRedoRequest =
                        messageReceived as NrpcNetrDatabaseRedoRequest;

                    if (nrpcNetrDatabaseRedoRequest == null)
                    {
                        throw new InvalidOperationException("messageReceived doesn't match its MethodOpnums");
                    }

                    ValidateNetlogonAuthenticator(nrpcNetrDatabaseRedoRequest.Authenticator);
                    break;

                case NrpcMethodOpnums.NetrLogonGetCapabilities:
                    CheckForSecureChannel();
                    NrpcNetrLogonGetCapabilitiesRequest nrpcNetrLogonGetCapabilitiesRequest =
                        messageReceived as NrpcNetrLogonGetCapabilitiesRequest;

                    if (nrpcNetrLogonGetCapabilitiesRequest == null)
                    {
                        throw new InvalidOperationException("messageReceived doesn't match its MethodOpnums");
                    }

                    ValidateNetlogonAuthenticator(nrpcNetrLogonGetCapabilitiesRequest.Authenticator);
                    break;

                case NrpcMethodOpnums.NetrServerAuthenticate3:
                    NrpcNetrServerAuthenticate3Request authenticate3Request = 
                        messageReceived as NrpcNetrServerAuthenticate3Request;

                    if (authenticate3Request == null)
                    {
                        throw new InvalidOperationException("messageReceived doesn't match its MethodOpnums");
                    }

                    if (authenticate3Request.NegotiateFlags == null)
                    {
                        throw new InvalidOperationException("NegotiateFlags in the request isn't present");
                    }
                    NegotiateFlags = (NrpcNegotiateFlags)authenticate3Request.NegotiateFlags.Value;
                    ClientComputerName = authenticate3Request.ComputerName;
                    SecureChannelType = authenticate3Request.SecureChannelType;
                    AccountName = authenticate3Request.AccountName;

                    ComputeSessionKey();
                    ValidateClientCredential(SessionKey, (uint)NegotiateFlags,ClientChallenge,
                        authenticate3Request.ClientCredential);
                    break;

                case NrpcMethodOpnums.NetrLogonGetDomainInfo:
                    CheckForSecureChannel();
                    NrpcNetrLogonGetDomainInfoRequest nrpcNetrLogonGetDomainInfoRequest =
                        messageReceived as NrpcNetrLogonGetDomainInfoRequest;

                    if (nrpcNetrLogonGetDomainInfoRequest == null)
                    {
                        throw new InvalidOperationException("messageReceived doesn't match its MethodOpnums");
                    }

                    ValidateNetlogonAuthenticator(nrpcNetrLogonGetDomainInfoRequest.Authenticator);
                    break;

                case NrpcMethodOpnums.NetrServerPasswordSet2:
                    CheckForSecureChannel();
                    NrpcNetrServerPasswordSet2Request nrpcNetrServerPasswordSet2Request =
                        messageReceived as NrpcNetrServerPasswordSet2Request;

                    if (nrpcNetrServerPasswordSet2Request == null)
                    {
                        throw new InvalidOperationException("messageReceived doesn't match its MethodOpnums");
                    }

                    ValidateNetlogonAuthenticator(nrpcNetrServerPasswordSet2Request.Authenticator);
                    break;

                case NrpcMethodOpnums.NetrServerPasswordGet:
                    CheckForSecureChannel();
                    NrpcNetrServerPasswordGetRequest nrpcNetrServerPasswordGetRequest =
                        messageReceived as NrpcNetrServerPasswordGetRequest;

                    if (nrpcNetrServerPasswordGetRequest == null)
                    {
                        throw new InvalidOperationException("messageReceived doesn't match its MethodOpnums");
                    }

                    ValidateNetlogonAuthenticator(nrpcNetrServerPasswordGetRequest.Authenticator);
                    break;

                case NrpcMethodOpnums.NetrLogonSendToSam:
                    CheckForSecureChannel();
                    NrpcNetrLogonSendToSamRequest nrpcNetrLogonSendToSamRequest =
                        messageReceived as NrpcNetrLogonSendToSamRequest;

                    if (nrpcNetrLogonSendToSamRequest == null)
                    {
                        throw new InvalidOperationException("messageReceived doesn't match its MethodOpnums");
                    }

                    ValidateNetlogonAuthenticator(nrpcNetrLogonSendToSamRequest.Authenticator);
                    break;

                case NrpcMethodOpnums.NetrLogonSamLogonEx:
                    CheckForSecureChannel();
                    break;

                case NrpcMethodOpnums.NetrServerTrustPasswordsGet:
                    CheckForSecureChannel();
                    NrpcNetrServerTrustPasswordsGetRequest nrpcNetrServerTrustPasswordsGetRequest =
                        messageReceived as NrpcNetrServerTrustPasswordsGetRequest;

                    if (nrpcNetrServerTrustPasswordsGetRequest == null)
                    {
                        throw new InvalidOperationException("messageReceived doesn't match its MethodOpnums");
                    }

                    ValidateNetlogonAuthenticator(nrpcNetrServerTrustPasswordsGetRequest.Authenticator);
                    break;

                case NrpcMethodOpnums.NetrGetForestTrustInformation:
                    CheckForSecureChannel();
                    NrpcNetrGetForestTrustInformationRequest nrpcNetrGetForestTrustInformationRequest =
                        messageReceived as NrpcNetrGetForestTrustInformationRequest;

                    if (nrpcNetrGetForestTrustInformationRequest == null)
                    {
                        throw new InvalidOperationException("messageReceived doesn't match its MethodOpnums");
                    }

                    ValidateNetlogonAuthenticator(nrpcNetrGetForestTrustInformationRequest.Authenticator);
                    break;

                case NrpcMethodOpnums.NetrLogonSamLogonWithFlags:
                    CheckForSecureChannel();
                    NrpcNetrLogonSamLogonWithFlagsRequest nrpcNetrLogonSamLogonWithFlagsRequest =
                        messageReceived as NrpcNetrLogonSamLogonWithFlagsRequest;

                    if (nrpcNetrLogonSamLogonWithFlagsRequest == null)
                    {
                        throw new InvalidOperationException("messageReceived doesn't match its MethodOpnums");
                    }

                    ValidateNetlogonAuthenticator(nrpcNetrLogonSamLogonWithFlagsRequest.Authenticator);
                    break;

                case NrpcMethodOpnums.NetrServerGetTrustInfo:
                    CheckForSecureChannel();
                    NrpcNetrServerGetTrustInfoRequest nrpcNetrServerGetTrustInfoRequest =
                        messageReceived as NrpcNetrServerGetTrustInfoRequest;

                    if (nrpcNetrServerGetTrustInfoRequest == null)
                    {
                        throw new InvalidOperationException("messageReceived doesn't match its MethodOpnums");
                    }

                    ValidateNetlogonAuthenticator(nrpcNetrServerGetTrustInfoRequest.Authenticator);
                    break;

                case NrpcMethodOpnums.DsrUpdateReadOnlyServerDnsRecords:
                    CheckForSecureChannel();
                    NrpcDsrUpdateReadOnlyServerDnsRecordsRequest nrpcDsrUpdateReadOnlyServerDnsRecordsRequest =
                        messageReceived as NrpcDsrUpdateReadOnlyServerDnsRecordsRequest;

                    if (nrpcDsrUpdateReadOnlyServerDnsRecordsRequest == null)
                    {
                        throw new InvalidOperationException("messageReceived doesn't match its MethodOpnums");
                    }

                    ValidateNetlogonAuthenticator(nrpcDsrUpdateReadOnlyServerDnsRecordsRequest.Authenticator);
                    break;

                case NrpcMethodOpnums.NetrChainSetClientAttributes:
                    CheckForSecureChannel();
                    NrpcNetrChainSetClientAttributesRequest nrpcNetrChainSetClientAttributesRequest =
                        messageReceived as NrpcNetrChainSetClientAttributesRequest;

                    if (nrpcNetrChainSetClientAttributesRequest == null)
                    {
                        throw new InvalidOperationException("messageReceived doesn't match its MethodOpnums");
                    }

                    ValidateNetlogonAuthenticator(nrpcNetrChainSetClientAttributesRequest.Authenticator);
                    break;

                default:
                    break;
            }
        }


        /// <summary>
        ///  Update the session context before sending a response to the client.
        /// </summary>
        /// <param name="messageToSend">The NRPC response to be sent</param>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        internal void UpdateSessionContextWithMessageSent(NrpcResponseStub messageToSend)
        {
            NrpcClientSessionInfo clientSessionInfo;

            switch (messageToSend.Opnum)
            {
                case NrpcMethodOpnums.NetrServerReqChallenge:
                    NrpcNetrServerReqChallengeResponse netrServerReqChallengeResponse =
                        messageToSend as NrpcNetrServerReqChallengeResponse;

                    if (netrServerReqChallengeResponse == null)
                    {
                        throw new InvalidOperationException("messageToSend doesn't match its MethodOpnums");
                    }

                    ServerChallenge = netrServerReqChallengeResponse.ServerChallenge.Value.data;
                    break;

                case NrpcMethodOpnums.NetrServerAuthenticate:
                    NrpcNetrServerAuthenticateResponse netrServerAuthenticateResponse =
                        (messageToSend as NrpcNetrServerAuthenticateResponse);

                    if (netrServerAuthenticateResponse == null)
                    {
                        throw new InvalidOperationException("messageToSend doesn't match its MethodOpnums");
                    }

                    //if authentication is successfully finished, store the secure channel to the session table
                    if (netrServerAuthenticateResponse.Status == NtStatus.STATUS_SUCCESS)
                    {
                        clientSessionInfo.ComputerName = ClientComputerName;
                        clientSessionInfo.AccountRid = AccountRid;
                        clientSessionInfo.NegotiateFlags = NegotiateFlags;
                        clientSessionInfo.SecureChannelType = SecureChannelType;
                        clientSessionInfo.ServerStoredCredential = StoredCredential;
                        clientSessionInfo.SessionKey = SessionKey;
                        clientSessionInfo.SharedSecret = SharedSecret;
                        clientSessionInfo.ClientSequenceNumber = null;
                        clientSessionInfo.ServerSequenceNumber = null;

                        //if this client computer already established a secure channel, remove it first
                        if (clientSessionInfoTable.ContainsKey(ClientComputerName))
                        {
                            clientSessionInfoTable.Remove(ClientComputerName);
                        }

                        clientSessionInfoTable[ClientComputerName] = clientSessionInfo;
                    }
                    break;

                case NrpcMethodOpnums.NetrServerAuthenticate2:
                    NrpcNetrServerAuthenticate2Response netrServerAuthenticate2Response =
                        (messageToSend as NrpcNetrServerAuthenticate2Response);

                    if (netrServerAuthenticate2Response == null)
                    {
                        throw new InvalidOperationException("messageToSend doesn't match its MethodOpnums");
                    }

                    NegotiateFlags = (NrpcNegotiateFlags)netrServerAuthenticate2Response.NegotiateFlags;

                    //if authentication is successfully finished, store the secure channel to the session table
                    if (netrServerAuthenticate2Response.Status == NtStatus.STATUS_SUCCESS)
                    {
                        clientSessionInfo.ComputerName = ClientComputerName;
                        clientSessionInfo.AccountRid = AccountRid;
                        clientSessionInfo.NegotiateFlags = NegotiateFlags;
                        clientSessionInfo.SecureChannelType = SecureChannelType;
                        clientSessionInfo.ServerStoredCredential = StoredCredential;
                        clientSessionInfo.SessionKey = SessionKey;
                        clientSessionInfo.SharedSecret = SharedSecret;
                        clientSessionInfo.ClientSequenceNumber = null;
                        clientSessionInfo.ServerSequenceNumber = null;

                        //if this client computer already established a secure channel, remove it first
                        if (clientSessionInfoTable.ContainsKey(ClientComputerName))
                        {
                            clientSessionInfoTable.Remove(ClientComputerName);
                        }

                        clientSessionInfoTable[ClientComputerName] = clientSessionInfo;
                    }
                    break;

                case NrpcMethodOpnums.NetrLogonGetCapabilities:
                    NrpcNetrLogonGetCapabilitiesResponse nrpcNetrLogonGetCapabilitiesResponse =
                        (messageToSend as NrpcNetrLogonGetCapabilitiesResponse);

                    if (nrpcNetrLogonGetCapabilitiesResponse == null)
                    {
                        throw new InvalidOperationException("messageToSend doesn't match its MethodOpnums");
                    }

                    NegotiateFlags = 
                        (NrpcNegotiateFlags)nrpcNetrLogonGetCapabilitiesResponse.ServerCapabilities.Value.ServerCapabilities;
                    break;

                case NrpcMethodOpnums.NetrServerAuthenticate3:
                    NrpcNetrServerAuthenticate3Response netrServerAuthenticate3Response =
                        (messageToSend as NrpcNetrServerAuthenticate3Response);

                    if (netrServerAuthenticate3Response == null)
                    {
                        throw new InvalidOperationException("messageToSend doesn't match its MethodOpnums");
                    }

                    NegotiateFlags = (NrpcNegotiateFlags)netrServerAuthenticate3Response.NegotiateFlags;
                    if (netrServerAuthenticate3Response.AccountRid.HasValue)
                    {
                        AccountRid = netrServerAuthenticate3Response.AccountRid.Value;
                    }
                    //if authentication is successfully finished, store the secure channel to the session table
                    if (netrServerAuthenticate3Response.Status == NtStatus.STATUS_SUCCESS)
                    {
                        clientSessionInfo.ComputerName = ClientComputerName;
                        clientSessionInfo.AccountRid = AccountRid;
                        clientSessionInfo.NegotiateFlags = NegotiateFlags;
                        clientSessionInfo.SecureChannelType = SecureChannelType;
                        clientSessionInfo.ServerStoredCredential = StoredCredential;
                        clientSessionInfo.SessionKey = SessionKey;
                        clientSessionInfo.SharedSecret = SharedSecret;
                        clientSessionInfo.ClientSequenceNumber = null;
                        clientSessionInfo.ServerSequenceNumber = null;

                        //if this client computer already established a secure channel, remove it first
                        if (clientSessionInfoTable.ContainsKey(ClientComputerName))
                        {
                            clientSessionInfoTable.Remove(ClientComputerName);
                        }

                        clientSessionInfoTable[ClientComputerName] = clientSessionInfo;
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
