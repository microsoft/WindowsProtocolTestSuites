// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.PrintService.Rprn;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp;
using Microsoft.Protocols.TestTools.StackSdk.Security.Pac;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc
{
    /// <summary>
    /// NRPC client.
    /// You can call following methods from this class.<para/>
    /// RPC bind methods.<para/>
    /// NRPC RPC methods.<para/>
    /// Compute session-key, credential and authenticator methods.<para/>
    /// Integrity / confidentiality methods.<para/>
    /// Codec help methods.
    /// </summary>
    // We have to use so many types and namespaces.
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public class NrpcClient : IDisposable
    {
        static List<NrpcClient> m_nrpcClients = new List<NrpcClient>();

        /// <summary>
        /// used to request a managed Nrpc client
        /// </summary>
        /// <param name="name">domain name</param>
        /// <returns>Nrpc client instance</returns>
        public static NrpcClient CreateNrpcClient(string dn)
        {
            NrpcClient c = new NrpcClient(dn);
            m_nrpcClients.Add(c);
            return c;
        }
        public static void CleanAll()
        {
            foreach (NrpcClient c in m_nrpcClients)
            {
                try
                {
                    c.Dispose();
                }
                catch
                {

                }
            }
            m_nrpcClients.Clear();
        }

        // client context
        private NrpcClientContext context;

        // actual rpc adapter
        private INrpcRpcAdapter rpc;

        // client security context
        private NrpcClientSecurityContext nrpcSecurityContext;


        #region Constructor

        /// <summary>
        /// Constructor, initialize a NRPC client.<para/>
        /// Create the instance will not connect to server, 
        /// you should call one of BindOverTcp or BindOverNamedPipe 
        /// to actually connect to NRPC server.
        /// </summary>
        /// <param name="domainName">Domain name</param>
        private NrpcClient(string domainName)
        {
            context = new NrpcClientContext();
            context.DomainName = domainName;
            rpc = new NrpcRpcAdapter();
        }

        #endregion


        #region RPC bind methods

        /// <summary>
        /// RPC bind over named pipe, using well-known endpoint "\PIPE\NETLOGON".
        /// </summary>
        /// <param name="serverName">NRPC server machine name.</param>
        /// <param name="transportCredential">
        /// If connect by SMB/SMB2, it's the security credential 
        /// used by under layer transport (SMB/SMB2). 
        /// If connect by TCP, this parameter is ignored.
        /// </param>
        /// <param name="securityContext">
        /// Security provider for RPC. 
        /// Set the value to null to disable authentication.
        /// </param>
        /// <param name="timeout">Timeout for bind and all future requests.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when serverName is null.
        /// </exception>
        public void BindOverNamedPipe(
            string serverName,
            AccountCredential transportCredential,
            ClientSecurityContext securityContext,
            TimeSpan timeout)
        {
            if (serverName == null)
            {
                throw new ArgumentNullException("serverName");
            }

            nrpcSecurityContext = securityContext as NrpcClientSecurityContext;
            if (nrpcSecurityContext != null)
            {
                context = nrpcSecurityContext.nrpc.context;
            }

            rpc.Bind(
                RpceUtility.RPC_OVER_NAMED_PIPE_PROTOCOL_SEQUENCE,
                serverName,
                NrpcUtility.NETLOGON_RPC_OVER_NP_WELLKNOWN_ENDPOINT,
                transportCredential,
                securityContext,
                securityContext == null
                    ? RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_NONE
                    : (context.SealSecureChannel
                        ? RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_PKT_PRIVACY
                        : RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_PKT_INTEGRITY),
                timeout);

            context.PrimaryName = serverName;
            NrpcRpcAdapter nrpcRpcAdapter = rpc as NrpcRpcAdapter;
            if (nrpcRpcAdapter != null)
            {
                context.rpceTransportContext = nrpcRpcAdapter.rpceClientTransport.Context;
            }
        }


        /// <summary>
        /// RPC bind over TCP/IP, using specified endpoint and authenticate provider.
        /// </summary>
        /// <param name="serverName">NRPC server machine name.</param>
        /// <param name="endpoint">RPC endpoints, it's the port on TCP/IP.</param>
        /// <param name="securityContext">
        /// Security provider for RPC. 
        /// Set the value to null to disable authentication.
        /// </param>
        /// <param name="timeout">Timeout for bind and all future requests.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when serverName is null.
        /// </exception>
        public void BindOverTcp(
            string serverName,
            ushort endpoint,
            ClientSecurityContext securityContext,
            TimeSpan timeout)
        {
            if (serverName == null)
            {
                throw new ArgumentNullException("serverName");
            }

            nrpcSecurityContext = securityContext as NrpcClientSecurityContext;
            if (nrpcSecurityContext != null)
            {
                context = nrpcSecurityContext.nrpc.context;
            }

            rpc.Bind(
                RpceUtility.RPC_OVER_TCPIP_PROTOCOL_SEQUENCE,
                serverName,
                endpoint.ToString(CultureInfo.InvariantCulture),
                null,
                securityContext,
                securityContext == null
                    ? RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_NONE
                    : (context.SealSecureChannel
                        ? RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_PKT_PRIVACY
                        : RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_PKT_INTEGRITY),
                timeout);

            context.PrimaryName = serverName;
            NrpcRpcAdapter nrpcRpcAdapter = rpc as NrpcRpcAdapter;
            if (nrpcRpcAdapter != null)
            {
                context.rpceTransportContext = nrpcRpcAdapter.rpceClientTransport.Context;
            }
        }


        /// <summary>
        /// Get the RPC handle.
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                return rpc.Handle;
            }
        }

        #endregion


        #region Properties - Context

        /// <summary>
        /// NRPC client context.
        /// </summary>
        public NrpcClientContext Context
        {
            get
            {
                return context;
            }
        }

        #endregion


        #region Short-version methods


        /// <summary>
        /// Create an empty NETLOGON_AUTHENTICATOR struct.
        /// </summary>
        /// <returns>
        /// An empty NETLOGON_AUTHENTICATOR.
        /// </returns>
        // Suppress CA1822 to keep design consistent.
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public _NETLOGON_AUTHENTICATOR CreateEmptyNetlogonAuthenticator()
        {
            _NETLOGON_AUTHENTICATOR authenticator = new _NETLOGON_AUTHENTICATOR();
            authenticator.Timestamp = 0;
            authenticator.Credential = new _NETLOGON_CREDENTIAL();
            authenticator.Credential.data = new byte[NrpcUtility.NL_CREDENTIAL_LENGTH];
            return authenticator;
        }


        /// <summary>
        /// Compute the Netlogon Authenticator.
        /// </summary>
        /// <returns>Netlogon Authenticator</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the method is called before establishing a NRPC secure channel.
        /// </exception>
        public _NETLOGON_AUTHENTICATOR ComputeNetlogonAuthenticator()
        {
            ValidateSecureChannelExists();

            byte[] clientStoredCredential;
            _NETLOGON_AUTHENTICATOR authenticator;

            if ((context.NegotiateFlags & NrpcNegotiateFlags.SupportsAESAndSHA2)
                == NrpcNegotiateFlags.SupportsAESAndSHA2)
            {
                clientStoredCredential = context.StoredCredential;
                authenticator = NrpcUtility.ComputeClientNetlogonAuthenticator(
                    NrpcComputeNetlogonCredentialAlgorithm.AES128,
                    context.UtcNow,
                    ref clientStoredCredential,
                    context.SessionKey);
                context.StoredCredential = clientStoredCredential;
            }
            else
            {
                clientStoredCredential = context.StoredCredential;
                authenticator = NrpcUtility.ComputeClientNetlogonAuthenticator(
                    NrpcComputeNetlogonCredentialAlgorithm.DESECB,
                    context.UtcNow,
                    ref clientStoredCredential,
                    context.SessionKey);
                context.StoredCredential = clientStoredCredential;
            }

            return authenticator;
        }


        /// <summary>
        /// Validates the returned Netlogon authenticator.
        /// </summary>
        /// <param name="serverAuthenticator">
        /// Netlogon authenticator returned from server.
        /// </param>
        /// <returns>Return true if validate successfully; otherwise, false.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the method is called before establishing a NRPC secure channel.
        /// </exception>
        public bool ValidateServerNetlogonAuthenticator(
            _NETLOGON_AUTHENTICATOR serverAuthenticator)
        {
            ValidateSecureChannelExists();

            byte[] clientStoredCredential;
            bool result;

            if ((context.NegotiateFlags & NrpcNegotiateFlags.SupportsAESAndSHA2)
                == NrpcNegotiateFlags.SupportsAESAndSHA2)
            {
                clientStoredCredential = context.StoredCredential;
                result = NrpcUtility.ValidateServerNetlogonAuthenticator(
                    serverAuthenticator,
                    NrpcComputeNetlogonCredentialAlgorithm.AES128,
                    ref clientStoredCredential,
                    context.SessionKey);
                context.StoredCredential = clientStoredCredential;
            }
            else
            {
                clientStoredCredential = context.StoredCredential;
                result = NrpcUtility.ValidateServerNetlogonAuthenticator(
                    serverAuthenticator,
                    NrpcComputeNetlogonCredentialAlgorithm.DESECB,
                    ref clientStoredCredential,
                    context.SessionKey);
                context.StoredCredential = clientStoredCredential;
            }

            return result;
        }


        /// <summary>
        /// Call NetrServerReqChallenge, get necessary information from context.
        /// </summary>
        /// <param name="computerName">
        /// A null-terminated Unicode string that contains the NetBIOS name of 
        /// the client computer calling this method.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when computerName is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when server returned an error code.
        /// </exception>
        public void NetrServerReqChallenge(string computerName)
        {
            if (computerName == null)
            {
                throw new ArgumentNullException("computerName");
            }

            _NETLOGON_CREDENTIAL clientChallenge = new _NETLOGON_CREDENTIAL();
            clientChallenge.data = NrpcUtility.GenerateNonce(NrpcUtility.NL_CREDENTIAL_LENGTH);
            _NETLOGON_CREDENTIAL? serverChallenge;

            NtStatus status = NetrServerReqChallenge(
                context.PrimaryName,
                computerName,
                clientChallenge,
                out serverChallenge);

            if (status != NtStatus.STATUS_SUCCESS)
            {
                NrpcUtility.ThrowExceptionOnStatus("NetrServerReqChallenge", (int)status);
            }
        }


        /// <summary>
        /// Call NetrServerAuthenticate3, get necessary information from context.
        /// </summary>
        /// <param name="accountName">
        /// A null-terminated Unicode string that identifies the name of 
        /// the account that contains the secret key (password) that is shared 
        /// between the client and the server.
        /// </param>
        /// <param name="secureChannelType">
        /// A NETLOGON_SECURE_CHANNEL_TYPE enumerated value, 
        /// that indicates the type of the
        /// secure channel being established by this call.
        /// </param>
        /// <param name="clientCapabilities">
        /// A pointer to a 32-bit set of bit flags in little-endian format that 
        /// indicate features supported. As input, the set of flags are those 
        /// requested by the client and SHOULD be the same as ClientCapabilities. 
        /// As output, they are the bit-wise AND of the client's requested capabilities 
        /// and the server's ServerCapabilities.
        /// </param>
        /// <param name="sharedSecret">
        /// An even-numbered sequence of bytes, with no embedded zero values, 
        /// that is a plain-text secret (password) shared between the client and the server.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when account or sharedSecret is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when server returned an error code or 
        /// server returned credential validation failed.
        /// </exception>
        public void NetrServerAuthenticate3(
            string accountName,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            ref NrpcNegotiateFlags clientCapabilities,
            string sharedSecret)
        {
            if (accountName == null)
            {
                throw new ArgumentNullException("accountName");
            }
            if (sharedSecret == null)
            {
                throw new ArgumentNullException("sharedSecret");
            }

            context.SharedSecret = sharedSecret;

            NrpcComputeSessionKeyAlgorithm computeSessionKeyAlgorithm;
            NrpcComputeNetlogonCredentialAlgorithm computeNetlogonCredentialAlgorithm;

            if ((clientCapabilities & NrpcNegotiateFlags.SupportsAESAndSHA2) ==
                NrpcNegotiateFlags.SupportsAESAndSHA2)
            {
                computeSessionKeyAlgorithm = NrpcComputeSessionKeyAlgorithm.HMACSHA256;
                computeNetlogonCredentialAlgorithm = NrpcComputeNetlogonCredentialAlgorithm.AES128;
            }
            else if ((clientCapabilities & NrpcNegotiateFlags.SupportsStrongKeys) ==
                NrpcNegotiateFlags.SupportsStrongKeys)
            {
                computeSessionKeyAlgorithm = NrpcComputeSessionKeyAlgorithm.MD5;
                computeNetlogonCredentialAlgorithm = NrpcComputeNetlogonCredentialAlgorithm.DESECB;
            }
            else
            {
                computeSessionKeyAlgorithm = NrpcComputeSessionKeyAlgorithm.DES;
                computeNetlogonCredentialAlgorithm = NrpcComputeNetlogonCredentialAlgorithm.DESECB;
            }

            context.SessionKey = NrpcUtility.ComputeSessionKey(
                computeSessionKeyAlgorithm,
                context.SharedSecret,
                context.ClientChallenge,
                context.ServerChallenge);

            _NETLOGON_CREDENTIAL clientCredential = new _NETLOGON_CREDENTIAL();
            clientCredential.data = NrpcUtility.ComputeNetlogonCredential(
                computeNetlogonCredentialAlgorithm,
                context.ClientChallenge,
                context.SessionKey);

            _NETLOGON_CREDENTIAL? serverCredential;

            NrpcNegotiateFlags? flags = clientCapabilities;
            uint? accountRid;

            NtStatus status = NetrServerAuthenticate3(
                context.PrimaryName,
                accountName,
                secureChannelType,
                context.ClientComputerName,
                clientCredential,
                out serverCredential,
                ref flags,
                out accountRid);

            if (status != NtStatus.STATUS_SUCCESS)
            {
                NrpcUtility.ThrowExceptionOnStatus("NetrServerAuthenticate3", (int)status);
            }
            if (serverCredential == null
                || !ArrayUtility.CompareArrays(
                    serverCredential.Value.data,
                    NrpcUtility.ComputeNetlogonCredential(
                        computeNetlogonCredentialAlgorithm,
                        context.ServerChallenge,
                        context.SessionKey)))
            {
                NrpcUtility.ThrowExceptionOnValidationFail("NetrServerAuthenticate3");
            }
        }


        /// <summary>
        /// Call NetrLogonGetCapabilities, get necessary information from context.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when server returned an error code or 
        /// server returned authenticator validation failed.
        /// </exception>
        public void NetrLogonGetCapabilities()
        {
            _NETLOGON_AUTHENTICATOR clientAuthenticator = ComputeNetlogonAuthenticator();
            _NETLOGON_AUTHENTICATOR? serverAuthenticator = CreateEmptyNetlogonAuthenticator();
            const uint serverCapabilitiesQueryLevel = 1;
            _NETLOGON_CAPABILITIES? serverCapabilities;

            NtStatus status = NetrLogonGetCapabilities(
                context.PrimaryName,
                context.ClientComputerName,
                clientAuthenticator,
                ref serverAuthenticator,
                serverCapabilitiesQueryLevel,
                out serverCapabilities);

            if (status != NtStatus.STATUS_SUCCESS)
            {
                NrpcUtility.ThrowExceptionOnStatus("NetrServerReqChallenge", (int)status);
            }
            if (serverAuthenticator == null ||
                !ValidateServerNetlogonAuthenticator(serverAuthenticator.Value))
            {
                NrpcUtility.ThrowExceptionOnValidationFail("NetrLogonGetCapabilities");
            }
        }


        /// <summary>
        /// Generating an Initial Netlogon Signature Token, get necessary information from context.
        /// </summary>
        /// <param name="securityBuffers">
        /// Security buffers, contains input plain-text and output signature.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when securityBuffers is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the method is called before establishing a NRPC secure channel.
        /// </exception>
        public void Sign(params SecurityBuffer[] securityBuffers)
        {
            if (securityBuffers == null || securityBuffers.Length == 0)
            {
                throw new ArgumentNullException("securityBuffers");
            }
            for (int i = 0; i < securityBuffers.Length; i++)
            {
                if (securityBuffers[i] == null)
                {
                    throw new ArgumentNullException("securityBuffers");
                }
            }
            ValidateSecureChannelExists();

            ulong sequenceNumber = context.SequenceNumber;
            NrpcUtility.InitialNetlogonSignatureToken(
                ((context.NegotiateFlags & NrpcNegotiateFlags.SupportsAESAndSHA2) != 0),
                ref sequenceNumber,
                context.SessionKey,
                false,
                true,
                securityBuffers);
            context.SequenceNumber = sequenceNumber;
        }


        /// <summary>
        /// Verify a Netlogon Signature Token, get necessary information from context.
        /// </summary>
        /// <param name="securityBuffers">
        /// Security buffers, contains input plain-text and signature.
        /// </param>
        /// <returns>
        /// Returns true if signature is correct; otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when securityBuffers is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the method is called before establishing a NRPC secure channel.
        /// </exception>
        public bool Verify(params SecurityBuffer[] securityBuffers)
        {
            if (securityBuffers == null || securityBuffers.Length == 0)
            {
                throw new ArgumentNullException("securityBuffers");
            }
            for (int i = 0; i < securityBuffers.Length; i++)
            {
                if (securityBuffers[i] == null)
                {
                    throw new ArgumentNullException("securityBuffers");
                }
            }
            ValidateSecureChannelExists();

            ulong sequenceNumber = context.SequenceNumber;

            bool result = NrpcUtility.ValidateNetlogonSignatureToken(
                ((context.NegotiateFlags & NrpcNegotiateFlags.SupportsAESAndSHA2) != 0),
                ref sequenceNumber,
                context.SessionKey,
                false,
                false,
                securityBuffers);

            context.SequenceNumber = sequenceNumber;
            return result;
        }


        /// <summary>
        /// Encrypt a package, get necessary information from context.
        /// </summary>
        /// <param name="securityBuffers">
        /// Security buffers, contains input plain-text; output cipher-text and signature.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when securityBuffers is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the method is called before establishing a NRPC secure channel.
        /// </exception>
        public void Encrypt(params SecurityBuffer[] securityBuffers)
        {
            if (securityBuffers == null || securityBuffers.Length == 0)
            {
                throw new ArgumentNullException("securityBuffers");
            }
            for (int i = 0; i < securityBuffers.Length; i++)
            {
                if (securityBuffers[i] == null)
                {
                    throw new ArgumentNullException("securityBuffers");
                }
            }
            ValidateSecureChannelExists();

            ulong sequenceNumber = context.SequenceNumber;

            NrpcUtility.InitialNetlogonSignatureToken(
                ((context.NegotiateFlags & NrpcNegotiateFlags.SupportsAESAndSHA2) != 0),
                ref sequenceNumber,
                context.SessionKey,
                true,
                true,
                securityBuffers);

            context.SequenceNumber = sequenceNumber;
        }


        /// <summary>
        /// Decrypt a package, get necessary information from context.
        /// </summary>
        /// <param name="securityBuffers">
        /// Security buffers, contains input cipher-text and output plain-text.
        /// </param>
        /// <returns>
        /// Returns true if signature is correct; otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when securityBuffers is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the method is called before establishing a NRPC secure channel.
        /// </exception>
        public bool Decrypt(params SecurityBuffer[] securityBuffers)
        {
            if (securityBuffers == null || securityBuffers.Length == 0)
            {
                throw new ArgumentNullException("securityBuffers");
            }
            for (int i = 0; i < securityBuffers.Length; i++)
            {
                if (securityBuffers[i] == null)
                {
                    throw new ArgumentNullException("securityBuffers");
                }
            }
            ValidateSecureChannelExists();

            ulong sequenceNumber = context.SequenceNumber;

            bool result = NrpcUtility.ValidateNetlogonSignatureToken(
                ((context.NegotiateFlags & NrpcNegotiateFlags.SupportsAESAndSHA2) != 0),
                ref sequenceNumber,
                context.SessionKey,
                true,
                false,
                securityBuffers);

            context.SequenceNumber = sequenceNumber;

            return result;
        }
        #endregion


        #region Codec helper methods


        /// <summary>
        /// Create an instance of NETLOGON_LEVEL structure 
        /// based on logonLevel parameter passed to the method.<para/>
        /// This method only supports logonLevel equals to 
        /// NetlogonInteractiveInformation or 
        /// NetlogonNetworkInformation or 
        /// NetlogonServiceInformation or 
        /// NetlogonInteractiveTransitiveInformation or
        /// NetlogonNetworkTransitiveInformation or
        /// NetlogonServiceTransitiveInformation.
        /// </summary>
        /// <param name="logonLevel">
        /// A NETLOGON_LOGON_INFO_CLASS structure, as specified in section 2.2.1.4.16, 
        /// that specifies the type of the logon information.
        /// This method only supports logonLevel equals to 
        /// NetlogonInteractiveInformation or 
        /// NetlogonNetworkInformation or 
        /// NetlogonServiceInformation or 
        /// NetlogonInteractiveTransitiveInformation or
        /// NetlogonNetworkTransitiveInformation or
        /// NetlogonServiceTransitiveInformation.
        /// </param>
        /// <param name="parameterControl">
        /// A set of bit flags that contain information pertaining 
        /// to the logon validation processing.
        /// </param>
        /// <param name="domainName">
        /// Contains the NetBIOS name of the domain of the account.
        /// </param>
        /// <param name="userName">
        /// Contains the name of the user.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// Created but not-yet-encrypted NETLOGON_LEVEL structure.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when logonLevel is not supported by the method.
        /// </exception>
        public _NETLOGON_LEVEL CreateNetlogonLevel(
            _NETLOGON_LOGON_INFO_CLASS logonLevel,
            NrpcParameterControlFlags parameterControl,
            string domainName,
            string userName,
            string password)
        {
            _NETLOGON_LEVEL netlogonLevel = new _NETLOGON_LEVEL();
            byte[] lmOwf;
            byte[] ntOwf;
            byte[] lmChallenge;
            byte[] ntChallengeResponse;
            byte[] lmChallengeResponse;

            //Identity: A NETLOGON_LOGON_IDENTITY_INFO structure, as specified in section 2.2.1.4.15, 
            //that contains information about the logon identity.
            _NETLOGON_LOGON_IDENTITY_INFO identityInfo = NrpcUtility.CreateNetlogonIdentityInfo(
                parameterControl,
                domainName,
                userName,
                context.ClientComputerName);

            switch (logonLevel)
            {
                case _NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveInformation:
                    //LmOwfPassword: LM_OWF_PASSWORD structure, as specified in section 2.2.1.1.3, 
                    //that contains the LMOWFv1 of a password. 
                    //LMOWFv1 is specified in NTLM v1 Authentication in [MS-NLMP] section 3.3.1.
                    lmOwf = NlmpUtility.LmOWF(NlmpVersion.v1, domainName, userName, password);
                    //NtOwfPassword: An NT_OWF_PASSWORD structure, as specified in section 2.2.1.1.4, 
                    //that contains the NTOWFv1 of a password. 
                    //NTOWFv1 is specified in NTLM v1 Authentication in [MS-NLMP] section 3.3.1.
                    ntOwf = NlmpUtility.NtOWF(NlmpVersion.v1, domainName, userName, password);

                    netlogonLevel.LogonInteractive = new _NETLOGON_INTERACTIVE_INFO[1];
                    netlogonLevel.LogonInteractive[0].Identity = identityInfo;
                    netlogonLevel.LogonInteractive[0].LmOwfPassword = new _LM_OWF_PASSWORD();
                    netlogonLevel.LogonInteractive[0].LmOwfPassword.data = NrpcUtility.CreateCypherBlocks(lmOwf);
                    netlogonLevel.LogonInteractive[0].NtOwfPassword = new _NT_OWF_PASSWORD();
                    netlogonLevel.LogonInteractive[0].NtOwfPassword.data = NrpcUtility.CreateCypherBlocks(ntOwf);
                    break;

                case _NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation:
                    //LmChallenge: LM_CHALLENGE structure, as specified in section 2.2.1.4.1, 
                    //that contains the network authentication challenge. 
                    //For details about challenges, see [MS-NLMP].
                    lmChallenge = NrpcUtility.GenerateNonce(NrpcUtility.NL_CREDENTIAL_LENGTH);
                    //NtChallengeResponse: String that contains the NT response (see [MS-NLMP]) 
                    //to the network authentication challenge.
                    lmOwf = NlmpUtility.GetResponseKeyLm(NlmpVersion.v1, domainName, userName, password);
                    lmChallengeResponse = NlmpUtility.DESL(lmOwf, lmChallenge);
                    //LmChallengeResponse: String that contains the LAN Manager response 
                    //(see [MS-NLMP]) to the network authentication challenge.
                    ntOwf = NlmpUtility.GetResponseKeyNt(NlmpVersion.v1, domainName, userName, password);
                    ntChallengeResponse = NlmpUtility.DESL(ntOwf, lmChallenge);

                    netlogonLevel.LogonNetwork = new _NETLOGON_NETWORK_INFO[1];
                    netlogonLevel.LogonNetwork[0].Identity = identityInfo;
                    netlogonLevel.LogonNetwork[0].LmChallenge = new LM_CHALLENGE();
                    netlogonLevel.LogonNetwork[0].LmChallenge.data = lmChallenge;
                    netlogonLevel.LogonNetwork[0].LmChallengeResponse = NrpcUtility.CreateString(lmChallengeResponse);
                    netlogonLevel.LogonNetwork[0].NtChallengeResponse = NrpcUtility.CreateString(ntChallengeResponse);
                    break;

                case _NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation:
                    //LmOwfPassword: LM_OWF_PASSWORD structure, as specified in section 2.2.1.1.3, 
                    //that contains the LMOWFv1 of a password. 
                    //LMOWFv1 is specified in NTLM v1 Authentication in [MS-NLMP] section 3.3.1.
                    lmOwf = NlmpUtility.LmOWF(NlmpVersion.v1, domainName, userName, password);
                    //NtOwfPassword: NT_OWF_PASSWORD structure, as specified in section 2.2.1.1.4, 
                    //that contains the NTOWFv1 of a password. 
                    //NTOWFv1 is specified in NTLM v1 Authentication in [MS-NLMP] section 3.3.1.
                    ntOwf = NlmpUtility.NtOWF(NlmpVersion.v1, domainName, userName, password);

                    netlogonLevel.LogonService = new _NETLOGON_SERVICE_INFO[1];
                    netlogonLevel.LogonService[0].Identity = identityInfo;
                    netlogonLevel.LogonService[0].LmOwfPassword = new _LM_OWF_PASSWORD();
                    netlogonLevel.LogonService[0].LmOwfPassword.data = NrpcUtility.CreateCypherBlocks(lmOwf);
                    netlogonLevel.LogonService[0].NtOwfPassword = new _NT_OWF_PASSWORD();
                    netlogonLevel.LogonService[0].NtOwfPassword.data = NrpcUtility.CreateCypherBlocks(ntOwf);
                    break;

                case _NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation:
                    lmOwf = NlmpUtility.LmOWF(NlmpVersion.v1, domainName, userName, password);
                    ntOwf = NlmpUtility.NtOWF(NlmpVersion.v1, domainName, userName, password);

                    netlogonLevel.LogonInteractiveTransitive = new _NETLOGON_INTERACTIVE_INFO[1];
                    netlogonLevel.LogonInteractiveTransitive[0].Identity = identityInfo;
                    netlogonLevel.LogonInteractiveTransitive[0].LmOwfPassword = new _LM_OWF_PASSWORD();
                    netlogonLevel.LogonInteractiveTransitive[0].LmOwfPassword.data
                        = NrpcUtility.CreateCypherBlocks(lmOwf);
                    netlogonLevel.LogonInteractiveTransitive[0].NtOwfPassword = new _NT_OWF_PASSWORD();
                    netlogonLevel.LogonInteractiveTransitive[0].NtOwfPassword.data
                        = NrpcUtility.CreateCypherBlocks(ntOwf);
                    break;

                case _NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation:
                    lmChallenge = NrpcUtility.GenerateNonce(NrpcUtility.NL_CREDENTIAL_LENGTH);
                    lmOwf = NlmpUtility.GetResponseKeyLm(NlmpVersion.v1, domainName, userName, password);
                    lmChallengeResponse = NlmpUtility.DESL(lmOwf, lmChallenge);
                    ntOwf = NlmpUtility.GetResponseKeyNt(NlmpVersion.v1, domainName, userName, password);
                    ntChallengeResponse = NlmpUtility.DESL(ntOwf, lmChallenge);

                    netlogonLevel.LogonNetworkTransitive = new _NETLOGON_NETWORK_INFO[1];
                    netlogonLevel.LogonNetworkTransitive[0].Identity = identityInfo;
                    netlogonLevel.LogonNetworkTransitive[0].LmChallenge = new LM_CHALLENGE();
                    netlogonLevel.LogonNetworkTransitive[0].LmChallenge.data = lmChallenge;
                    netlogonLevel.LogonNetworkTransitive[0].LmChallengeResponse
                        = NrpcUtility.CreateString(lmChallengeResponse);
                    netlogonLevel.LogonNetworkTransitive[0].NtChallengeResponse
                        = NrpcUtility.CreateString(ntChallengeResponse);
                    break;

                case _NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation:
                    lmOwf = NlmpUtility.LmOWF(NlmpVersion.v1, domainName, userName, password);
                    ntOwf = NlmpUtility.NtOWF(NlmpVersion.v1, domainName, userName, password);

                    netlogonLevel.LogonServiceTransitive = new _NETLOGON_SERVICE_INFO[1];
                    netlogonLevel.LogonServiceTransitive[0].Identity = identityInfo;
                    netlogonLevel.LogonServiceTransitive[0].LmOwfPassword = new _LM_OWF_PASSWORD();
                    netlogonLevel.LogonServiceTransitive[0].LmOwfPassword.data = NrpcUtility.CreateCypherBlocks(lmOwf);
                    netlogonLevel.LogonServiceTransitive[0].NtOwfPassword = new _NT_OWF_PASSWORD();
                    netlogonLevel.LogonServiceTransitive[0].NtOwfPassword.data = NrpcUtility.CreateCypherBlocks(ntOwf);
                    break;

                case _NETLOGON_LOGON_INFO_CLASS.NetlogonGenericInformation:
                default:
                    throw new ArgumentException("logonLevel is not supported by the method.", "logonLevel");
            }

            return netlogonLevel;
        }


        /// <summary>
        /// Create an instance of NETLOGON_LEVEL structure 
        /// based on logonLevel parameter passed to the method.<para/>
        /// This method only supports logonLevel equals to 
        /// NetlogonGenericInformation.
        /// </summary>
        /// <param name="logonLevel">
        /// A NETLOGON_LOGON_INFO_CLASS structure, as specified in section 2.2.1.4.16, 
        /// that specifies the type of the logon information.
        /// This method only supports logonLevel equals to 
        /// NetlogonGenericInformation.
        /// </param>
        /// <param name="logonDomainName">
        /// Contains the NetBIOS name of the domain of the account.
        /// </param>
        /// <param name="parameterControl">
        /// A set of bit flags that contain information pertaining 
        /// to the logon validation processing.
        /// </param>
        /// <param name="userName">
        /// Contains the name of the user.
        /// </param>
        /// <param name="workstation">
        /// Contains the NetBIOS name of the workstation from which the user is logging on.
        /// </param>
        /// <param name="packageName">
        /// Contains the name of the security provider, such as Kerberos, 
        /// to which the data will be delivered on the domain controller 
        /// in the target domain that was specified in the Identity field. 
        /// This name MUST match the name of an existing security provider; 
        /// otherwise, the Security Support Provider Interface (SSPI) 
        /// returns a package not found error.
        /// </param>
        /// <param name="logonData">
        /// A pointer to a block of binary data that contains the information 
        /// to be sent to the security package referenced in PackageName. 
        /// This data is opaque to Netlogon.
        /// </param>
        /// <returns>Created but not-yet-encrypted NETLOGON_LEVEL structure.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when logonLevel is not supported by the method.
        /// </exception>
        // Suppress CA1822 to keep design consistent.
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public _NETLOGON_LEVEL CreateNetlogonLevel(
            _NETLOGON_LOGON_INFO_CLASS logonLevel,
            string logonDomainName,
            NrpcParameterControlFlags parameterControl,
            string userName,
            string workstation,
            string packageName,
            byte[] logonData)
        {
            if (logonLevel != _NETLOGON_LOGON_INFO_CLASS.NetlogonGenericInformation)
            {
                throw new ArgumentException("logonLevel is not supported by the method.", "logonLevel");
            }

            _NETLOGON_LEVEL netlogonLevel = new _NETLOGON_LEVEL();

            netlogonLevel.LogonGeneric = new _NETLOGON_GENERIC_INFO[1];
            netlogonLevel.LogonGeneric[0].Identity = new _NETLOGON_LOGON_IDENTITY_INFO();
            netlogonLevel.LogonGeneric[0].Identity.LogonDomainName = DtypUtility.ToRpcUnicodeString(logonDomainName);
            netlogonLevel.LogonGeneric[0].Identity.ParameterControl = (uint)parameterControl;
            netlogonLevel.LogonGeneric[0].Identity.UserName = DtypUtility.ToRpcUnicodeString(userName);
            netlogonLevel.LogonGeneric[0].Identity.Workstation = DtypUtility.ToRpcUnicodeString(workstation);
            netlogonLevel.LogonGeneric[0].Identity.Reserved = DtypUtility.ToOldLargeInteger(0);
            netlogonLevel.LogonGeneric[0].PackageName = DtypUtility.ToRpcUnicodeString(packageName);
            netlogonLevel.LogonGeneric[0].DataLength = (uint)logonData.Length;
            netlogonLevel.LogonGeneric[0].LogonData = logonData;

            return netlogonLevel;
        }


        /// <summary>
        /// Encrypt LM_OWF_PASSWORD and NT_OWF_PASSWORD by RC4 and session key.
        /// </summary>
        /// <param name="lmOwfPassword">LM_OWF_PASSWORD</param>
        /// <param name="ntOwfPassword">NT_OWF_PASSWORD</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the method is called before establishing a NRPC secure channel.<para/>
        /// Thrown when neither AES nore RC4 is negotiated.
        /// </exception>
        private void EncryptLmAndNtOwfPassword(
            ref _LM_OWF_PASSWORD lmOwfPassword,
            ref _NT_OWF_PASSWORD ntOwfPassword)
        {
            if (lmOwfPassword.data == null || lmOwfPassword.data.Length != 2)
            {
                throw new ArgumentException("lmOwfPassword is invalid", "lmOwfPassword");
            }
            if (ntOwfPassword.data == null || ntOwfPassword.data.Length != 2)
            {
                throw new ArgumentException("ntOwfPassword is invalid", "ntOwfPassword");
            }
            ValidateSecureChannelExists();

            //For all versions of Windows except Windows NT 3.1, 
            //encrypt by using RC4 and the session key.
            byte[] lmOwf = ArrayUtility.ConcatenateArrays(
                lmOwfPassword.data[0].data,
                lmOwfPassword.data[1].data);
            byte[] ntOwf = ArrayUtility.ConcatenateArrays(
                ntOwfPassword.data[0].data,
                ntOwfPassword.data[1].data);

            bool isAesNegotiated = IsAesOrRc4Negotiated();

            lmOwf = NrpcUtility.EncryptBuffer(isAesNegotiated, context.SessionKey, lmOwf);
            ntOwf = NrpcUtility.EncryptBuffer(isAesNegotiated, context.SessionKey, ntOwf);

            lmOwfPassword.data = NrpcUtility.CreateCypherBlocks(lmOwf);
            ntOwfPassword.data = NrpcUtility.CreateCypherBlocks(ntOwf);
        }


        /// <summary>
        /// Encrypt a NETLOGON_LEVEL structure based on 
        /// logonLevel parameter passed to the method. 
        /// This method support RC4 encryption only.
        /// </summary>
        /// <param name="logonLevel">
        /// A NETLOGON_LOGON_INFO_CLASS structure, as specified in section 2.2.1.4.16, 
        /// that specifies the type of the logon information.
        /// </param>
        /// <param name="logonInformation">
        /// A NETLOGON_LEVEL structure to be encrypted.
        /// </param>
        /// <returns>The encrypted NETLOGON_LEVEL structure.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when logonLevel or logonInformation is invalid.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when session key is not negotiated.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the method is called before establishing a NRPC secure channel. 
        /// Thrown when neither AES nor RC4 is negotiated.
        /// </exception>
        public _NETLOGON_LEVEL EncryptNetlogonLevel(
            _NETLOGON_LOGON_INFO_CLASS logonLevel,
            _NETLOGON_LEVEL logonInformation)
        {
            ValidateSecureChannelExists();

            switch (logonLevel)
            {
                case _NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveInformation:
                    //If the LogonLevel is NetlogonInteractiveInformation or 
                    //NetlogonInteractiveTransitiveInformation, then encrypt<102> 
                    //the LmOwfPassword and NtOwfPassword members in the 
                    //NETLOGON_INTERACTIVE_INFO structure.
                    if (logonInformation.LogonInteractive == null
                        || logonInformation.LogonInteractive.Length != 1)
                    {
                        throw new ArgumentException("logonInformation is invalid.", "logonInformation");
                    }

                    EncryptLmAndNtOwfPassword(
                        ref logonInformation.LogonInteractive[0].LmOwfPassword,
                        ref logonInformation.LogonInteractive[0].NtOwfPassword);
                    break;

                case _NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation:
                    if (logonInformation.LogonInteractiveTransitive == null
                        || logonInformation.LogonInteractiveTransitive.Length != 1)
                    {
                        throw new ArgumentException("logonInformation is invalid.", "logonInformation");
                    }

                    EncryptLmAndNtOwfPassword(
                        ref logonInformation.LogonInteractiveTransitive[0].LmOwfPassword,
                        ref logonInformation.LogonInteractiveTransitive[0].NtOwfPassword);
                    break;

                case _NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation:
                    //If the LogonLevel is NetlogonServiceInformation or 
                    //NetlogonServiceTransitiveInformation, then encrypt<103> 
                    //the LmOwfPassword and NtOwfPassword members in the 
                    //NETLOGON_SERVICE_INFO structure.
                    if (logonInformation.LogonService == null
                        || logonInformation.LogonService.Length != 1)
                    {
                        throw new ArgumentException("logonInformation is invalid.", "logonInformation");
                    }

                    EncryptLmAndNtOwfPassword(
                        ref logonInformation.LogonService[0].LmOwfPassword,
                        ref logonInformation.LogonService[0].NtOwfPassword);
                    break;

                case _NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation:
                    if (logonInformation.LogonServiceTransitive == null
                        || logonInformation.LogonServiceTransitive.Length != 1)
                    {
                        throw new ArgumentException("logonInformation is invalid.", "logonInformation");
                    }

                    EncryptLmAndNtOwfPassword(
                        ref logonInformation.LogonServiceTransitive[0].LmOwfPassword,
                        ref logonInformation.LogonServiceTransitive[0].NtOwfPassword);
                    break;

                case _NETLOGON_LOGON_INFO_CLASS.NetlogonGenericInformation:
                    //If the LogonLevel is NetlogonGenericInformation, 
                    //then encrypt<104> the LogonData member in the 
                    //NETLOGON_GENERIC_INFO structure.
                    if (logonInformation.LogonGeneric == null
                        || logonInformation.LogonGeneric.Length != 1)
                    {
                        throw new ArgumentException("logonInformation is invalid.", "logonInformation");
                    }

                    //Encrypt the ClearNewPassword parameter using the RC4 algorithm 
                    //and the session key established as the encryption key.
                    logonInformation.LogonGeneric[0].LogonData = NrpcUtility.EncryptBuffer(
                        IsAesOrRc4Negotiated(),
                        context.SessionKey,
                        logonInformation.LogonGeneric[0].LogonData);
                    break;

                case _NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation:
                case _NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation:
                    //If the LogonLevel is NetlogonNetworkInformation or 
                    //NetlogonNetworkTransitiveInformation, then encrypt the 
                    //UserSessionKey and the first two elements of the 
                    //ExpansionRoom array in the NETLOGON_VALIDATION_SAM_INFO 
                    //(section 2.2.1.4.11) or in the NETLOGON_VALIDATION_SAM_INFO2 
                    //(section 2.2.1.4.12) structure.


                    break;

                default:
                    throw new ArgumentException("logonLevel is invalid.", "logonLevel");
            }

            return logonInformation;
        }

        /// <summary>
        /// Decrypt a _NETLOGON_VALIDATION structure based on
        /// validationInformation parameter in Nrpc NetrlogonSamLogonXXX API.
        /// </summary>        
        /// <param name="validationInfoClass">
        /// Validation Info Class
        /// </param>
        /// <param name="validationInformation">
        /// Validation info returned from NetrlogonSamLogonXXX API
        /// </param>
        /// <returns>Decrypted _NETLOGON_VALIDATION data structure</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the method is called before establishing a NRPC secure channel.
        /// </exception>>
        ///  <exception cref="ArgumentException">
        /// Thrown when input invalid validationInformation argument
        /// </exception>>
        public _NETLOGON_VALIDATION DecryptNetlogonValidation(
            _NETLOGON_VALIDATION_INFO_CLASS validationInfoClass,
            _NETLOGON_VALIDATION validationInformation)
        {
            ValidateSecureChannelExists();

            switch (validationInfoClass)
            {
                case _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationGenericInfo:
                case _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationGenericInfo2:
                    //If the LogonLevel is NetlogonGenericInformation, 
                    //then encrypt<104> the LogonData member in the 
                    //NETLOGON_GENERIC_INFO structure.
                    if (validationInformation.ValidationGeneric2 == null
                     || validationInformation.ValidationGeneric2.Length != 1)
                    {
                        throw new ArgumentException("validationInformation is invalid.", "validationInformation");
                    }

                    if (validationInformation.ValidationGeneric2[0].ValidationData == null)
                    {
                        throw new ArgumentException("validationInformation is invalid.", "validationInformation");
                    }

                    validationInformation.ValidationGeneric2[0].ValidationData = NrpcUtility.DecryptBuffer(
                        (context.NegotiateFlags & NrpcNegotiateFlags.SupportsAESAndSHA2)
                        == NrpcNegotiateFlags.SupportsAESAndSHA2,
                        context.SessionKey,
                        validationInformation.ValidationGeneric2[0].ValidationData);
                    break;

                //If the LogonLevel is NetlogonNetworkInformation or 
                //NetlogonNetworkTransitiveInformation, then encrypt the 
                //UserSessionKey and the first two elements of the 
                //ExpansionRoom array in the NETLOGON_VALIDATION_SAM_INFO 
                //(section 2.2.1.4.11) or in the NETLOGON_VALIDATION_SAM_INFO2 
                //(section 2.2.1.4.12) structure.
                case _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo:
                    if (validationInformation.ValidationSam == null
                    || validationInformation.ValidationSam.Length != 1)
                    {
                        throw new ArgumentException("validationInformation is invalid.", "validationInformation");
                    }
                    validationInformation.ValidationSam[0].UserSessionKey =
                        DecryptUserSessionKey(validationInformation.ValidationSam[0].UserSessionKey);
                    validationInformation.ValidationSam[0].ExpansionRoom =
                        DecryptExpansionRoom(validationInformation.ValidationSam[0].ExpansionRoom);
                    break;

                case _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2:
                    if (validationInformation.ValidationSam2 == null
                    || validationInformation.ValidationSam2.Length != 1)
                    {
                        throw new ArgumentException("validationInformation is invalid.", "validationInformation");
                    }
                    validationInformation.ValidationSam2[0].UserSessionKey =
                        DecryptUserSessionKey(validationInformation.ValidationSam2[0].UserSessionKey);
                    validationInformation.ValidationSam2[0].ExpansionRoom =
                        DecryptExpansionRoom(validationInformation.ValidationSam2[0].ExpansionRoom);
                    break;

                case _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4:
                case _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationUasInfo:
                    // no encrypted field
                    break;

                default:
                    throw new ArgumentException("validationInfoClass is invalid.", "validationInfoClass");
            }

            return validationInformation;
        }


        /// <summary>
        /// Decrypt expansionRoom field in NETLOGON_VALIDATION_SAM_INFO or
        /// NETLOGON_VALIDATION_SAM_INFO2
        /// </summary>
        /// <param name="expansionRoom">expansionRoom structure</param>
        /// <exception cref="ArgumentException">
        /// Thrown when input invalid expansionRoom argument
        /// </exception>>
        /// <returns>Decrypted expansionRoom structure</returns>
        private uint[] DecryptExpansionRoom(uint[] expansionRoom)
        {
            if (expansionRoom == null || expansionRoom.Length != 10)
            {
                throw new ArgumentException("Invalid ExpansionRoom", "expansionRoom");
            }

            // The first 2 bytes of ExpansionRoom field are reserved for SAMINFO_LM_SESSION_KEY
            byte[] sessionKeyArray = ArrayUtility.ConcatenateArrays<byte>(
                BitConverter.GetBytes(expansionRoom[0]),
                BitConverter.GetBytes(expansionRoom[1]));

            byte[] decryptedSessionKeyArray = NrpcUtility.DecryptBuffer(
                (context.NegotiateFlags & NrpcNegotiateFlags.SupportsAESAndSHA2)
                == NrpcNegotiateFlags.SupportsAESAndSHA2,
                context.SessionKey,
                sessionKeyArray);

            expansionRoom[0] = BitConverter.ToUInt32(
                ArrayUtility.SubArray<byte>(decryptedSessionKeyArray, 0, 8), 0);
            expansionRoom[1] = BitConverter.ToUInt32(
                ArrayUtility.SubArray<byte>(decryptedSessionKeyArray, 8, 8), 0);

            return expansionRoom;
        }


        /// <summary>
        /// Decrypt UserSessionKey structure in NETLOGON_VALIDATION_SAM_INFO or
        /// NETLOGON_VALIDATION_SAM_INFO2
        /// </summary>
        /// <param name="userSessionKey">User session key</param>
        /// <returns>Decrypted user session key</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when input invalid userSessionKey argument
        /// </exception>>
        private _USER_SESSION_KEY DecryptUserSessionKey(_USER_SESSION_KEY userSessionKey)
        {
            //  A two-element CYPHER_BLOCK structure, as specified in
            //  section , that contains the 16-byte encrypted user
            //  session key.
            if (userSessionKey.data == null || userSessionKey.data.Length != 2)
            {
                throw new ArgumentException("Invalid userSessionKey.data element", "userSessionKey");
            }

            if (userSessionKey.data[0].data == null
                || userSessionKey.data[0].data.Length != 8)
            {
                throw new ArgumentException("Invalid userSessionKey.data[0] element", "userSessionKey");
            }

            if (userSessionKey.data[1].data == null ||
                userSessionKey.data[1].data.Length != 8)
            {
                throw new ArgumentException("Invalid userSessionKey data[1] element", "userSessionKey");
            }

            byte[] userSessionKeyArray = ArrayUtility.ConcatenateArrays<byte>(
               userSessionKey.data[0].data,
               userSessionKey.data[1].data);

            // decrypt the session key
            byte[] decryptedBuffer = NrpcUtility.DecryptBuffer(
                (context.NegotiateFlags & NrpcNegotiateFlags.SupportsAESAndSHA2)
                == NrpcNegotiateFlags.SupportsAESAndSHA2,
                context.SessionKey,
                userSessionKeyArray);

            userSessionKey.data[0].data = ArrayUtility.SubArray<byte>(decryptedBuffer, 0, 8);
            userSessionKey.data[1].data = ArrayUtility.SubArray<byte>(decryptedBuffer, 8, 8);

            return userSessionKey;
        }


        /// <summary>
        /// Create an instance of NL_TRUST_PASSWORD structure. 
        /// The data is not encrypted, the caller should call 
        /// EncryptNlTrustPassword to encrypt it.
        /// </summary>
        /// <param name="password">The clear text password.
        /// </param>
        /// <param name="isInterdomainAccount">
        /// If the password is for an interdomain account, 
        /// the TrustPasswordVersion in context will be added to 
        /// NL_TRUST_PASSWORD structure and updated accordingly.
        /// For any other type of account, TrustPasswordVersion is not used.
        /// </param>
        /// <returns>Created but not-yet-encrypted NL_TRUST_PASSWORD structure.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when password is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when password length exceeds the limit.
        /// </exception>
        public _NL_TRUST_PASSWORD CreateNlTrustPassword(
            string password,
            bool isInterdomainAccount)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            _NL_TRUST_PASSWORD nlTrustPassword = new _NL_TRUST_PASSWORD();

            //The ClearNewPassword parameter is constructed as follows, 
            //assuming a WCHAR-represented password of length X bytes.
            byte[] passwordBuffer = Encoding.Unicode.GetBytes(password);
            int X = passwordBuffer.Length;

            if (X > 512)
            {
                throw new ArgumentException("password length exceeds the limit.", "password");
            }

            byte[] buffer;

            if (isInterdomainAccount)
            {
                //If the password is for an interdomain account:

                //An NL_PASSWORD_VERSION structure, as specified in section 2.2.1.3.8, 
                //is prepared. The PasswordVersionNumber field of the structure is set to 
                //the value of the TrustPasswordVersion variable corresponding to 
                //the password being set. The first trust password generated has 
                //TrustPasswordVersion equal to one. Each time a new trust password 
                //is generated, its TrustPasswordVersion is computed by adding one 
                //to the value of TrustPasswordVersion of the previous password. 
                //The NL_PASSWORD_VERSION structure is copied into ClearNewPassword.Buffer 
                //starting at byte offset (512 - X - size of (NL_PASSWORD_VERSION)). 
                _NL_PASSWORD_VERSION nlPasswordVersion = new _NL_PASSWORD_VERSION();
                nlPasswordVersion.ReservedField = ReservedField_Values.V1;
                nlPasswordVersion.PasswordVersionNumber = context.TrustPasswordVersion;
                nlPasswordVersion.PasswordVersionPresent = PasswordVersionPresent_Values.V1;
                byte[] nlPasswordVersionBuf = TypeMarshal.ToBytes(nlPasswordVersion);

                if ((X + nlPasswordVersionBuf.Length) > 512)
                {
                    throw new ArgumentException("password length exceeds the limit.", "password");
                }

                //The password is copied into the Buffer field of ClearNewPassword, 
                //which is treated as an array of bytes, starting at byte offset (512 - X).
                //The first (512 - X) - size of (NL_PASSWORD_VERSION) bytes of 
                //ClearNewPassword.Buffer are filled with randomly generated data.
                buffer = ArrayUtility.ConcatenateArrays(
                    NrpcUtility.GenerateNonce(512 - X - nlPasswordVersionBuf.Length),
                    nlPasswordVersionBuf,
                    passwordBuffer);

                context.TrustPasswordVersion += 1;
            }
            else
            {
                //If the password is not for an interdomain account:
                buffer = ArrayUtility.ConcatenateArrays(
                    NrpcUtility.GenerateNonce(512 - X),
                    passwordBuffer);
            }

            nlTrustPassword.Buffer = new ushort[256];
            Buffer.BlockCopy(buffer, 0, nlTrustPassword.Buffer, 0, 512);

            //ClearNewPassword.Length is set to X.
            nlTrustPassword.Length = (uint)X;

            return nlTrustPassword;
        }


        /// <summary>
        /// Encrypt a NL_TRUST_PASSWORD structure.
        /// </summary>
        /// <param name="nlTrustPassword">
        /// A NL_TRUST_PASSWORD structure which contains data to be encrypted.
        /// </param>
        /// <returns>Encrypted NL_TRUST_PASSWORD.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when nlTrustPassword is invalid.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the method is called before establishing a NRPC secure channel. 
        /// Thrown when neither AES nor RC4 is negotiated.
        /// </exception>
        public _NL_TRUST_PASSWORD EncryptNlTrustPassword(_NL_TRUST_PASSWORD nlTrustPassword)
        {
            if (nlTrustPassword.Buffer == null)
            {
                throw new ArgumentException("nlTrustPassword is invalid.", "nlTrustPassword");
            }
            ValidateSecureChannelExists();

            //Encrypt the ClearNewPassword parameter using the RC4 algorithm 
            //and the session key established as the encryption key.
            using (SafeIntPtr ptr = TypeMarshal.ToIntPtr(nlTrustPassword))
            {
                byte[] buf = new byte[
                    nlTrustPassword.Buffer.Length * sizeof(ushort)
                    + Marshal.SizeOf(nlTrustPassword.Length)];
                Marshal.Copy(ptr, buf, 0, buf.Length);
                buf = NrpcUtility.EncryptBuffer(IsAesOrRc4Negotiated(), context.SessionKey, buf);
                Marshal.Copy(buf, 0, ptr, buf.Length);
                return TypeMarshal.ToStruct<_NL_TRUST_PASSWORD>(ptr);
            }
        }


        /// <summary>
        /// Encrypt password before calling NetrServerPasswordSet.
        /// </summary>
        /// <param name="password">
        /// The password to be encrypted.
        /// </param>
        /// <returns>
        /// A LM_OWF_PASSWORD struct with encrypted password.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when password is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when SAMR failed to encrypt password.
        /// Thrown when the method is called before establishing a NRPC secure channel.
        /// </exception>
        public _LM_OWF_PASSWORD EncryptUasNewPassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            ValidateSecureChannelExists();

            //Compute the NTOWFv1 ([MS-NLMP] section 3.3.1) of the new password.
            byte[] ntOwfV1 = NlmpUtility.NtOWF(NlmpVersion.v1, null, null, password);

            //Encrypt ([MS-SAMR] section 2.2.11.1.1) the result of step 1 using 
            //the Session-Key for the secure channel as the Specified Key.
            _ENCRYPTED_LM_OWF_PASSWORD samrEncryptedLmOwfPassword
                    = SamrCryptography.EncryptBlockWithKey(ntOwfV1, context.SessionKey);

            if (samrEncryptedLmOwfPassword.data == null
                && samrEncryptedLmOwfPassword.data.Length != NrpcUtility.OWF_PASSWORD_LENGTH)
            {
                throw new InvalidOperationException("SAMR failed to encrypt LM_OWF_PASSWORD.");
            }

            _LM_OWF_PASSWORD encryptedLmOwfPassword = new _LM_OWF_PASSWORD();
            encryptedLmOwfPassword.data = NrpcUtility.CreateCypherBlocks(samrEncryptedLmOwfPassword.data);
            return encryptedLmOwfPassword;
        }


        /// <summary>
        /// Decrypt password after calling NetrServerPasswordGet.
        /// </summary>
        /// <param name="ntOwfPassword">
        /// The password to be decrypted, both input and output.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when ntOwfPassword is invalid.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the method is called before establishing a NRPC secure channel.
        /// </exception>
        public void DecryptNtOwfPassword(ref _NT_OWF_PASSWORD ntOwfPassword)
        {
            if (ntOwfPassword.data == null
                || ntOwfPassword.data.Length != 2)
            {
                throw new ArgumentException("Invalid NT_OWF_PASSWORD", "ntOwfPassword");
            }
            ValidateSecureChannelExists();

            //The client MUST decrypt the EncryptedNtOwfPassword 
            //return parameter using DES in ECB mode [FIPS81] 
            //and the session key established as the decryption key.

            //TDI 49785, DES key is 8 bytes, but session key is 16 bytes.
            using (DES des = DES.Create())
            {
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.None;
                des.Key = ArrayUtility.SubArray(
                    context.SessionKey,
                    0,
                    NrpcUtility.NL_CREDENTIAL_LENGTH);

                ntOwfPassword.data[0].data = des.CreateDecryptor().TransformFinalBlock(
                    ntOwfPassword.data[0].data,
                    0,
                    ntOwfPassword.data[0].data.Length);
            }

            using (DES des = DES.Create())
            {
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.None;
                des.Key = ArrayUtility.SubArray(
                    context.SessionKey,
                    NrpcUtility.NL_CREDENTIAL_LENGTH,
                    NrpcUtility.NL_CREDENTIAL_LENGTH);

                ntOwfPassword.data[1].data = des.CreateDecryptor().TransformFinalBlock(
                    ntOwfPassword.data[1].data,
                    0,
                    ntOwfPassword.data[1].data.Length);
            }
        }


        /// <summary>
        /// Encrypt OpaqueBuffer before calling NetrLogonSendToSam.
        /// </summary>
        /// <param name="buffer">
        /// A buffer to be encrypted.
        /// </param>
        /// <returns>
        /// The encrypted buffer.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the method is called before establishing a NRPC secure channel. 
        /// Thrown when neither AES nor RC4 is negotiated.
        /// </exception>
        public byte[] EncryptOpaqueBuffer(byte[] buffer)
        {
            ValidateSecureChannelExists();

            //Encrypt the OpaqueBuffer parameter using the RC4 algorithm 
            //and the session key established as the encryption key.
            return NrpcUtility.EncryptBuffer(IsAesOrRc4Negotiated(), context.SessionKey, buffer);
        }


        /// <summary>
        /// Create an instance of UAS_INFO_0 structure.
        /// </summary>
        /// <param name="computerName">
        /// The definition is beyond the scope of this document.
        /// </param>
        /// <param name="timeCreated">
        /// The definition is beyond the scope of this document.
        /// </param>
        /// <param name="serialNumber">
        /// The definition is beyond the scope of this document.
        /// </param>
        /// <returns>Created UAS_INFO_0 structure.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when computerName is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when computerName exceeded the max length (16 bytes).
        /// </exception>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public _UAS_INFO_0 CreateUasInfo0(
            string computerName,
            uint timeCreated,
            uint serialNumber)
        {
            const int COMPUTER_NAME_LENGTH = 16;

            if (computerName == null)
            {
                throw new ArgumentNullException("computerName");
            }
            if (computerName.Length > COMPUTER_NAME_LENGTH)
            {
                throw new ArgumentException("computerName exceeded its max length (16 bytes).", "computerName");
            }

            _UAS_INFO_0 uasInfo0 = new _UAS_INFO_0();
            uasInfo0.ComputerName = ArrayUtility.ConcatenateArrays(
                Encoding.ASCII.GetBytes(computerName),
                new byte[COMPUTER_NAME_LENGTH - computerName.Length]);
            uasInfo0.TimeCreated = timeCreated;
            uasInfo0.SerialNumber = serialNumber;
            return uasInfo0;
        }


        /// <summary>
        /// Create an instance of NETLOGON_CONTROL_DATA_INFORMATION structure 
        /// based on functionCode parameter passed to the method.<para/>
        /// This method only supports functionCode equals to 
        /// NETLOGON_CONTROL_REDISCOVER (0x00000005) or 
        /// NETLOGON_CONTROL_TC_QUERY (0x00000006) or 
        /// NETLOGON_CONTROL_FIND_USER (0x00000008) or 
        /// NETLOGON_CONTROL_CHANGE_PASSWORD (0x000000009) or 
        /// NETLOGON_CONTROL_TC_VERIFY (0x0000000A) or 
        /// NETLOGON_CONTROL_SET_DBFLAG (0x0000FFFE).
        /// </summary>
        /// <param name="functionCode">
        /// The control operation to be performed.
        /// </param>
        /// <param name="trustedDomainNameOrUserName">
        /// If functionCode equals to NETLOGON_CONTROL_REDISCOVER (0x00000005) or 
        /// NETLOGON_CONTROL_TC_QUERY (0x00000006) or 
        /// NETLOGON_CONTROL_CHANGE_PASSWORD (0x000000009) or 
        /// NETLOGON_CONTROL_TC_VERIFY (0x0000000A), this parameter means 
        /// A pointer to a null-terminated Unicode string that contains a trusted domain name.<para/>
        /// If functionCode equals to NETLOGON_CONTROL_FIND_USER (0x00000008) 
        /// this parameter means 
        /// A pointer to null-terminated Unicode string that contains a user name.<para/>
        /// If functionCode equals to NETLOGON_CONTROL_SET_DBFLAG (0x0000FFFE) 
        /// this parameter is ignored.
        /// </param>
        /// <returns>Created NETLOGON_CONTROL_DATA_INFORMATION structure.</returns>
        // Suppress CA1822 to keep design consistent.
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public _NETLOGON_CONTROL_DATA_INFORMATION CreateNetlogonControlDataInformation(
            FunctionCode_Values functionCode,
            string trustedDomainNameOrUserName)
        {
            _NETLOGON_CONTROL_DATA_INFORMATION netlogonControlDataInfo = new _NETLOGON_CONTROL_DATA_INFORMATION();

            switch (functionCode)
            {
                case FunctionCode_Values.NETLOGON_CONTROL_REDISCOVER:
                case FunctionCode_Values.NETLOGON_CONTROL_TC_QUERY:
                case FunctionCode_Values.NETLOGON_CONTROL_CHANGE_PASSWORD:
                case FunctionCode_Values.NETLOGON_CONTROL_TC_VERIFY:
                    netlogonControlDataInfo.TrustedDomainName = trustedDomainNameOrUserName;
                    break;

                case FunctionCode_Values.NETLOGON_CONTROL_SET_DBFLAG:
                    //When functionCode is 0x0000FFFE, set debugFlags to 0.
                    netlogonControlDataInfo.DebugFlag = 0;
                    break;

                case FunctionCode_Values.NETLOGON_CONTROL_FIND_USER:
                    netlogonControlDataInfo.UserName = trustedDomainNameOrUserName;
                    break;

                default:
                    //do nothing
                    break;
            }

            return netlogonControlDataInfo;
        }


        /// <summary>
        /// Create an instance of OSVERSIONINFOEX structure.
        /// </summary>
        /// <param name="majorVersion">
        /// The major OS version.
        /// </param>
        /// <param name="minorVersion">
        /// The minor OS version.
        /// </param>
        /// <param name="buildNumber">
        /// The build number of the OS.
        /// </param>
        /// <param name="platformId">
        /// The OS platform.
        /// </param>
        /// <param name="csdVersion">
        /// A maintenance string for Microsoft Product Support Services (PSS) use.
        /// </param>
        /// <param name="servicePackMajor">
        /// The major version number of the latest Service Pack installed on the system. 
        /// For example, for Service Pack 3, the major version number is 3. 
        /// If no Service Pack has been installed, the value is zero.
        /// </param>
        /// <param name="servicePackMinor">
        /// The minor version number of the latest Service Pack installed on the system. 
        /// For example, for Service Pack 3, the minor version number is 0.
        /// </param>
        /// <param name="suiteMask">
        /// A value that identifies the product suites available on the system, 
        /// consisting of Product Suite Flags (section 2.2.3.10.5).
        /// </param>
        /// <param name="productType">
        /// Additional information about the OS, which MUST be an 
        /// OS_TYPE enumeration (section 2.2.3.10.3) value.
        /// </param>
        /// <returns>Created OSVERSIONINFOEX structure.</returns>
        // Suppress CA1822 to keep design consistent.
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public OSVERSIONINFOEX CreateOsVersionInfoEx(
            uint majorVersion,
            uint minorVersion,
            uint buildNumber,
            RprnProcessorArchitecture platformId,
            string csdVersion,
            ushort servicePackMajor,
            ushort servicePackMinor,
            RprnProductSuiteFlags suiteMask,
            OS_TYPE productType
            )
        {
            OSVERSIONINFOEX osVersionInfoEx = new OSVERSIONINFOEX();

            osVersionInfoEx.OSVersionInfo = new OSVERSIONINFO();
            osVersionInfoEx.OSVersionInfo.dwMajorVersion = majorVersion;
            osVersionInfoEx.OSVersionInfo.dwMinorVersion = minorVersion;
            osVersionInfoEx.OSVersionInfo.dwBuildNumber = buildNumber;
            osVersionInfoEx.OSVersionInfo.dwPlatformId = (uint)platformId;
            //szCSDVersion (256 bytes): A maintenance string for Microsoft Product 
            //Support Services (PSS) use.
            osVersionInfoEx.OSVersionInfo.szCSDVersion = new ushort[128];
            if (csdVersion != null)
            {
                Buffer.BlockCopy(
                    Encoding.Unicode.GetBytes(csdVersion),
                    0,
                    osVersionInfoEx.OSVersionInfo.szCSDVersion,
                    0,
                    sizeof(ushort) * Math.Min(
                        osVersionInfoEx.OSVersionInfo.szCSDVersion.Length,
                        csdVersion.Length));
            }
            osVersionInfoEx.OSVersionInfo.dwOSVersionInfoSize
                = (uint)TypeMarshal.GetBlockMemorySize(osVersionInfoEx.OSVersionInfo);

            osVersionInfoEx.wServicePackMajor = servicePackMajor;
            osVersionInfoEx.wServicePackMinor = servicePackMinor;
            osVersionInfoEx.wSuiteMask = (ushort)suiteMask;
            osVersionInfoEx.wProductType = (byte)productType;
            osVersionInfoEx.wReserved = 0;

            return osVersionInfoEx;
        }


        /// <summary>
        /// Create an instance of NETLOGON_WORKSTATION_INFORMATION class 
        /// based on level parameter passed to the method.
        /// </summary>
        /// <param name="level">
        /// The information level requested by the client.
        /// </param>
        /// <param name="dnsHostName">
        /// A null-terminated Unicode string that contains the DNS host name of the client.
        /// </param>
        /// <param name="siteName">
        /// A null-terminated Unicode string that contains the name of 
        /// the site where the workstation resides.
        /// </param>
        /// <param name="osVersion">
        /// An RPC_UNICODE_STRING structure in which the Length and MaximumLength 
        /// fields are set to the size of an OSVERSIONINFOEX structure and 
        /// the Buffer field points to an OSVERSIONINFOEX ([MS-RPRN] section 
        /// 2.2.3.10.2) structure. OsVersion contains the version number of 
        /// the operating system installed on the client machine.
        /// </param>
        /// <param name="osName">
        /// A null-terminated Unicode string that contains the name of 
        /// the operating system installed on the client machine.
        /// </param>
        /// <param name="workstationFlags">
        /// A set of bit flags specifying workstation behavior.
        /// </param>
        /// <returns>
        /// Created NETLOGON_WORKSTATION_INFORMATION class.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when level is invalid.
        /// Thrown when szCSDVersion field of osVersion is null or the length of szCSDVersion is not 256 bytes.
        /// </exception>
        // Suppress CA1822 to keep design consistent.
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public _NETLOGON_WORKSTATION_INFORMATION CreateNetlogonWorkstationInformation(
            Level_Values level,
            string dnsHostName,
            string siteName,
            OSVERSIONINFOEX? osVersion,
            string osName,
            NrpcWorkstationFlags workstationFlags)
        {
            _NETLOGON_WORKSTATION_INFORMATION netlogonWorkstationInfo = new _NETLOGON_WORKSTATION_INFORMATION();

            //The WkstaBuffer parameter contains one of the following structures, 
            //based on the value of level field. Value Meaning
            //0x00000001: The WkstaBuffer contains a NETLOGON_DOMAIN_INFO structure.
            //0x00000002: The WkstaBuffer contains a NETLOGON_LSA_POLICY_INFO structure.
            //
            //But in 2.2.1.3.9, 
            //union _NETLOGON_WORKSTATION_INFORMATION {
            //  [case(1)]
            //  PNETLOGON_WORKSTATION_INFO WorkstationInfo;
            //  [case(2)]
            //  PNETLOGON_WORKSTATION_INFO LsaPolicyInfo;
            //}

            switch (level)
            {
                case Level_Values.NetlogonDomainInfo:
                    netlogonWorkstationInfo.WorkstationInfo = new _NETLOGON_WORKSTATION_INFO[1];
                    netlogonWorkstationInfo.WorkstationInfo[0] = new _NETLOGON_WORKSTATION_INFO();
                    netlogonWorkstationInfo.WorkstationInfo[0].DnsHostName = dnsHostName;
                    netlogonWorkstationInfo.WorkstationInfo[0].SiteName = siteName;
                    netlogonWorkstationInfo.WorkstationInfo[0].Dummy1 = null;
                    netlogonWorkstationInfo.WorkstationInfo[0].Dummy2 = null;
                    netlogonWorkstationInfo.WorkstationInfo[0].Dummy3 = null;
                    netlogonWorkstationInfo.WorkstationInfo[0].Dummy4 = null;
                    if (osVersion != null)
                    {
                        const int CSD_VERSION_LENGTH = 256;
                        if (osVersion.Value.OSVersionInfo.szCSDVersion == null)
                        {
                            throw new ArgumentException("szCSDVersion of osVersion cannot be null.", "osVersion");
                        }
                        if (osVersion.Value.OSVersionInfo.szCSDVersion.Length != CSD_VERSION_LENGTH / sizeof(ushort))
                        {
                            throw new ArgumentException("Length of szCSDVersion should be 256 bytes.", "osVersion");
                        }

                        netlogonWorkstationInfo.WorkstationInfo[0].OsVersion
                            = DtypUtility.ToRpcUnicodeString(
                                Encoding.Unicode.GetString(TypeMarshal.ToBytes(osVersion.Value)));
                    }
                    else
                    {
                        netlogonWorkstationInfo.WorkstationInfo[0].OsVersion
                            = DtypUtility.ToRpcUnicodeString(null);
                    }
                    netlogonWorkstationInfo.WorkstationInfo[0].OsName
                        = DtypUtility.ToRpcUnicodeString(osName);
                    netlogonWorkstationInfo.WorkstationInfo[0].DummyString3
                        = DtypUtility.ToRpcUnicodeString((string)null);
                    netlogonWorkstationInfo.WorkstationInfo[0].DummyString4
                        = DtypUtility.ToRpcUnicodeString((string)null);
                    netlogonWorkstationInfo.WorkstationInfo[0].WorkstationFlags
                        = (uint)workstationFlags;
                    netlogonWorkstationInfo.WorkstationInfo[0].KerberosSupportedEncryptionTypes = 0;
                    netlogonWorkstationInfo.WorkstationInfo[0].DummyLong3 = 0;
                    netlogonWorkstationInfo.WorkstationInfo[0].DummyLong4 = 0;
                    break;

                case Level_Values.NetlogonLsaPolicyInfo:
                    netlogonWorkstationInfo.LsaPolicyInfo = new _NETLOGON_WORKSTATION_INFO[1];
                    netlogonWorkstationInfo.LsaPolicyInfo[0] = new _NETLOGON_WORKSTATION_INFO();
                    netlogonWorkstationInfo.LsaPolicyInfo[0].LsaPolicy = new _NETLOGON_LSA_POLICY_INFO();
                    netlogonWorkstationInfo.LsaPolicyInfo[0].LsaPolicy.LsaPolicy = new byte[0];
                    netlogonWorkstationInfo.LsaPolicyInfo[0].LsaPolicy.LsaPolicySize = 0;
                    break;

                default:
                    throw new ArgumentException("level is invalid.", "level");
            }

            return netlogonWorkstationInfo;
        }

        /// <summary>
        /// Create an instance of NETLOGON_WORKSTATION_INFORMATION class 
        /// based on level parameter passed to the method.
        /// Windows 2012 R2 updated the DummyLong2 field to KerberosSupportedEncryptionTypes,
        /// add one parameter to define the new updated field.
        /// </summary>
        /// <param name="level">
        /// The information level requested by the client.
        /// </param>
        /// <param name="dnsHostName">
        /// A null-terminated Unicode string that contains the DNS host name of the client.
        /// </param>
        /// <param name="siteName">
        /// A null-terminated Unicode string that contains the name of 
        /// the site where the workstation resides.
        /// </param>
        /// <param name="osVersion">
        /// An RPC_UNICODE_STRING structure in which the Length and MaximumLength 
        /// fields are set to the size of an OSVERSIONINFOEX structure and 
        /// the Buffer field points to an OSVERSIONINFOEX ([MS-RPRN] section 
        /// 2.2.3.10.2) structure. OsVersion contains the version number of 
        /// the operating system installed on the client machine.
        /// </param>
        /// <param name="osName">
        /// A null-terminated Unicode string that contains the name of 
        /// the operating system installed on the client machine.
        /// </param>
        /// <param name="workstationFlags">
        /// A set of bit flags specifying workstation behavior.
        /// </param>
        /// <param name="kerberosSupportedEncryptionTypes">
        /// The msDS-SupportedEncryptionTypes attribute of the client's machine account object in Active Directory.
        /// </param>
        /// <returns>
        /// Created NETLOGON_WORKSTATION_INFORMATION class.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when level is invalid.
        /// Thrown when szCSDVersion field of osVersion is null or the length of szCSDVersion is not 256 bytes.
        /// </exception>
        // Suppress CA1822 to keep design consistent.
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public _NETLOGON_WORKSTATION_INFORMATION CreateNetlogonWorkstationInformation(
            Level_Values level,
            string dnsHostName,
            string siteName,
            OSVERSIONINFOEX? osVersion,
            string osName,
            NrpcWorkstationFlags workstationFlags,
            uint kerberosSupportedEncryptionTypes)
        {
            _NETLOGON_WORKSTATION_INFORMATION netlogonWorkstationInfo = new _NETLOGON_WORKSTATION_INFORMATION();

            //The WkstaBuffer parameter contains one of the following structures, 
            //based on the value of level field. Value Meaning
            //0x00000001: The WkstaBuffer contains a NETLOGON_DOMAIN_INFO structure.
            //0x00000002: The WkstaBuffer contains a NETLOGON_LSA_POLICY_INFO structure.
            //
            //But in 2.2.1.3.9, 
            //union _NETLOGON_WORKSTATION_INFORMATION {
            //  [case(1)]
            //  PNETLOGON_WORKSTATION_INFO WorkstationInfo;
            //  [case(2)]
            //  PNETLOGON_WORKSTATION_INFO LsaPolicyInfo;
            //}

            switch (level)
            {
                case Level_Values.NetlogonDomainInfo:
                    netlogonWorkstationInfo.WorkstationInfo = new _NETLOGON_WORKSTATION_INFO[1];
                    netlogonWorkstationInfo.WorkstationInfo[0] = new _NETLOGON_WORKSTATION_INFO();
                    netlogonWorkstationInfo.WorkstationInfo[0].DnsHostName = dnsHostName;
                    netlogonWorkstationInfo.WorkstationInfo[0].SiteName = siteName;
                    netlogonWorkstationInfo.WorkstationInfo[0].Dummy1 = null;
                    netlogonWorkstationInfo.WorkstationInfo[0].Dummy2 = null;
                    netlogonWorkstationInfo.WorkstationInfo[0].Dummy3 = null;
                    netlogonWorkstationInfo.WorkstationInfo[0].Dummy4 = null;
                    if (osVersion != null)
                    {
                        const int CSD_VERSION_LENGTH = 256;
                        if (osVersion.Value.OSVersionInfo.szCSDVersion == null)
                        {
                            throw new ArgumentException("szCSDVersion of osVersion cannot be null.", "osVersion");
                        }
                        if (osVersion.Value.OSVersionInfo.szCSDVersion.Length != CSD_VERSION_LENGTH / sizeof(ushort))
                        {
                            throw new ArgumentException("Length of szCSDVersion should be 256 bytes.", "osVersion");
                        }

                        netlogonWorkstationInfo.WorkstationInfo[0].OsVersion
                            = DtypUtility.ToRpcUnicodeString(
                                Encoding.Unicode.GetString(TypeMarshal.ToBytes(osVersion.Value)));
                    }
                    else
                    {
                        netlogonWorkstationInfo.WorkstationInfo[0].OsVersion
                            = DtypUtility.ToRpcUnicodeString(null);
                    }
                    netlogonWorkstationInfo.WorkstationInfo[0].OsName
                        = DtypUtility.ToRpcUnicodeString(osName);
                    netlogonWorkstationInfo.WorkstationInfo[0].DummyString3
                        = DtypUtility.ToRpcUnicodeString((string)null);
                    netlogonWorkstationInfo.WorkstationInfo[0].DummyString4
                        = DtypUtility.ToRpcUnicodeString((string)null);
                    netlogonWorkstationInfo.WorkstationInfo[0].WorkstationFlags
                        = (uint)workstationFlags;
                    netlogonWorkstationInfo.WorkstationInfo[0].KerberosSupportedEncryptionTypes
                        = kerberosSupportedEncryptionTypes;
                    netlogonWorkstationInfo.WorkstationInfo[0].DummyLong3 = 0;
                    netlogonWorkstationInfo.WorkstationInfo[0].DummyLong4 = 0;
                    break;

                case Level_Values.NetlogonLsaPolicyInfo:
                    netlogonWorkstationInfo.LsaPolicyInfo = new _NETLOGON_WORKSTATION_INFO[1];
                    netlogonWorkstationInfo.LsaPolicyInfo[0] = new _NETLOGON_WORKSTATION_INFO();
                    netlogonWorkstationInfo.LsaPolicyInfo[0].LsaPolicy = new _NETLOGON_LSA_POLICY_INFO();
                    netlogonWorkstationInfo.LsaPolicyInfo[0].LsaPolicy.LsaPolicy = new byte[0];
                    netlogonWorkstationInfo.LsaPolicyInfo[0].LsaPolicy.LsaPolicySize = 0;
                    break;

                default:
                    throw new ArgumentException("level is invalid.", "level");
            }

            return netlogonWorkstationInfo;
        }

        /// <summary>
        /// Create an instance of _NL_DNS_NAME_INFO structure.
        /// </summary>
        /// <param name="type">
        /// The type of DNS name.
        /// </param>
        /// <param name="dnsDomainInfo">
        /// The string that will be based on the DnsDomainInfoType.
        /// </param>
        /// <param name="dnsDomainInfoType">
        /// The type of DnsDomainInfo.
        /// </param>
        /// <param name="port">
        /// The port for the DNS SRV record.
        /// </param>
        /// <param name="register">
        /// TRUE indicates to register the DNS name; FALSE indicates
        /// to deregister the DNS name.
        /// </param>
        /// <returns>Created _NL_DNS_NAME_INFO structure.</returns>
        // Suppress CA1822 to keep design consistent.
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public _NL_DNS_NAME_INFO CreateNlDnsNameInfo(
            Type_Values type,
            string dnsDomainInfo,
            DnsDomainInfoType_Values dnsDomainInfoType,
            uint port,
            bool register
            )
        {
            _NL_DNS_NAME_INFO info = new _NL_DNS_NAME_INFO();
            info.Type = type;
            info.DnsDomainInfo = dnsDomainInfo;
            info.DnsDomainInfoType = dnsDomainInfoType;
            info.Priority = 0;
            info.Weight = 1;
            info.Port = port;
            info.Register = register ? (byte)1 : (byte)0;
            info.Status = 0;
            return info;
        }


        /// <summary>
        /// Create an instance of _NL_DNS_NAME_INFO_ARRAY structure.
        /// </summary>
        /// <param name="nlDnsNameInfoArray">A list of _NL_DNS_NAME_INFO structure.</param>
        /// <returns>Created _NL_DNS_NAME_INFO_ARRAY structure.</returns>
        // Suppress CA1822 to keep design consistent.
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public _NL_DNS_NAME_INFO_ARRAY CreateNlDnsNameInfoArray(params _NL_DNS_NAME_INFO[] nlDnsNameInfoArray)
        {
            _NL_DNS_NAME_INFO_ARRAY array = new _NL_DNS_NAME_INFO_ARRAY();
            array.EntryCount = (uint)nlDnsNameInfoArray.Length;
            array.DnsNamesInfo = nlDnsNameInfoArray;
            return array;
        }


        /// <summary>
        /// Create an instance of NL_IN_CHAIN_SET_CLIENT_ATTRIBUTES structure.
        /// </summary>
        /// <param name="clientDnsHostName">
        /// A NULL or null-terminated Unicode string that is used
        /// to update the attribute dNSHostName on the client's
        /// computer account object in Active Directory.
        /// </param>
        /// <param name="osVersionInfo">
        /// If not NULL, the attribute operatingSystemVersion on
        /// the client's computer account in Active Directory (using
        /// the ABNF Syntax as specified in [RFC2234]) is set to.
        /// </param>
        /// <param name="osName">
        /// NULL or a null-terminated Unicode string that is used
        /// to update the attribute operatingSystem on the client's
        /// computer account object in Active Directory. Added in
        /// longhorn_server.
        /// </param>
        /// <returns>Created NL_IN_CHAIN_SET_CLIENT_ATTRIBUTES structure.</returns>
        // Suppress CA1822 to keep design consistent.
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public NL_IN_CHAIN_SET_CLIENT_ATTRIBUTES CreateNlInChainSetClientAttributes(
            string clientDnsHostName,
            OSVERSIONINFOEX? osVersionInfo,
            string osName)
        {
            NL_IN_CHAIN_SET_CLIENT_ATTRIBUTES nlInChainSetClientAttributes = new NL_IN_CHAIN_SET_CLIENT_ATTRIBUTES();

            nlInChainSetClientAttributes.V1 = new _NL_IN_CHAIN_SET_CLIENT_ATTRIBUTES_V1();
            nlInChainSetClientAttributes.V1.ClientDnsHostName = clientDnsHostName;
            nlInChainSetClientAttributes.V1.OsName = osName;

            if (osVersionInfo != null)
            {
                nlInChainSetClientAttributes.V1.OsVersionInfo = new _NL_OSVERSIONINFO_V1[1];
                nlInChainSetClientAttributes.V1.OsVersionInfo[0].dwBuildNumber
                    = osVersionInfo.Value.OSVersionInfo.dwBuildNumber;
                nlInChainSetClientAttributes.V1.OsVersionInfo[0].dwMajorVersion
                    = (dwMajorVersion_Values)osVersionInfo.Value.OSVersionInfo.dwMajorVersion;
                nlInChainSetClientAttributes.V1.OsVersionInfo[0].dwMinorVersion
                    = (dwMinorVersion_Values)osVersionInfo.Value.OSVersionInfo.dwMinorVersion;
                nlInChainSetClientAttributes.V1.OsVersionInfo[0].dwPlatformId
                    = osVersionInfo.Value.OSVersionInfo.dwPlatformId;
                nlInChainSetClientAttributes.V1.OsVersionInfo[0].szCSDVersion
                    = osVersionInfo.Value.OSVersionInfo.szCSDVersion;
                nlInChainSetClientAttributes.V1.OsVersionInfo[0].wProductType
                    = (wProductType_Values)osVersionInfo.Value.wProductType;
                nlInChainSetClientAttributes.V1.OsVersionInfo[0].wReserved = 0;
                nlInChainSetClientAttributes.V1.OsVersionInfo[0].wServicePackMajor
                    = osVersionInfo.Value.wServicePackMajor;
                nlInChainSetClientAttributes.V1.OsVersionInfo[0].wServicePackMinor
                    = osVersionInfo.Value.wServicePackMinor;
                nlInChainSetClientAttributes.V1.OsVersionInfo[0].wSuiteMask
                    = (wSuiteMask_Values)osVersionInfo.Value.wSuiteMask;
                nlInChainSetClientAttributes.V1.OsVersionInfo[0].dwOSVersionInfoSize
                    = (uint)TypeMarshal.GetBlockMemorySize(nlInChainSetClientAttributes.V1.OsVersionInfo[0]);
            }

            return nlInChainSetClientAttributes;
        }


        /// <summary>
        /// Create an instance of NL_OUT_CHAIN_SET_CLIENT_ATTRIBUTES structure.
        /// </summary>
        /// <param name="hubName">
        /// The NetBIOS name of the writable domain controller receiving 
        /// NetrChainSetClientAttributes.
        /// </param>
        /// <param name="oldDnsHostName">
        /// The client's DNS host name, if any, from the attribute dNSHostName 
        /// on the client's computer account object in Active Directory on the 
        /// writable domain controller. If there was an update to the attribute 
        /// dNSHostName by the writable domain controller as a result of receiving 
        /// NetrChainSetClientAttributes (section 3.5.5.3.11), this value will 
        /// hold the previous value of that attribute.
        /// </param>
        /// <param name="supportedEncTypes">
        /// A set of bit flags that specify the encryption types supported, 
        /// as specified in [MS-LSAD] section 2.2.7.18. 
        /// See [MS-LSAD] for a specification of these bit values and their 
        /// allowed combinations.
        /// </param>
        /// <returns>Created NL_OUT_CHAIN_SET_CLIENT_ATTRIBUTES structure.</returns>
        // Suppress CA1822 to keep design consistent.
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public NL_OUT_CHAIN_SET_CLIENT_ATTRIBUTES CreateNlOutChainSetClientAttributes(
            string hubName,
            string oldDnsHostName,
            uint? supportedEncTypes)
        {
            NL_OUT_CHAIN_SET_CLIENT_ATTRIBUTES nlOutChainSetClientAttributes = new NL_OUT_CHAIN_SET_CLIENT_ATTRIBUTES();

            nlOutChainSetClientAttributes.V1 = new _NL_OUT_CHAIN_SET_CLIENT_ATTRIBUTES_V1();
            nlOutChainSetClientAttributes.V1.HubName = hubName;
            if (oldDnsHostName != null)
            {
                nlOutChainSetClientAttributes.V1.OldDnsHostName = new string[1];
                nlOutChainSetClientAttributes.V1.OldDnsHostName[0] = oldDnsHostName;
            }
            if (supportedEncTypes != null)
            {
                nlOutChainSetClientAttributes.V1.SupportedEncTypes = new uint[1];
                nlOutChainSetClientAttributes.V1.SupportedEncTypes[0] = supportedEncTypes.Value;
            }

            return nlOutChainSetClientAttributes;
        }


        /// <summary>
        /// Create an instance of NL_SOCKET_ADDRESS structure.
        /// </summary>
        /// <param name="sockaddr">
        /// A pointer to an octet string. 
        /// The format of the lpSockaddr member when an IPv4 socket 
        /// address is used is specified in section 2.2.1.2.4.1. 
        /// The format of the lpSockaddr member when an IPv6 socket 
        /// address is used is specified in section 2.2.1.2.4.2.
        /// </param>
        /// <returns>Created NL_SOCKET_ADDRESS structure.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when sockaddr is neither IPv4 nore IPv6.
        /// </exception>
        // Suppress CA1822 to keep design consistent.
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public _NL_SOCKET_ADDRESS CreateNlSocketAddress(IPEndPoint sockaddr)
        {
            _NL_SOCKET_ADDRESS nlSocketAddress = new _NL_SOCKET_ADDRESS();

            if (sockaddr == null)
            {
                nlSocketAddress.lpSockaddr = null;
                nlSocketAddress.iSockaddrLength = 0;
            }
            else
            {
                switch (sockaddr.AddressFamily)
                {
                    case AddressFamily.InterNetwork:
                        nlSocketAddress.lpSockaddr = ArrayUtility.ConcatenateArrays(
                            BitConverter.GetBytes((ushort)(AddressFamily.InterNetwork)),
                            BitConverter.GetBytes((ushort)sockaddr.Port),
                            sockaddr.Address.GetAddressBytes(),
                            new byte[8]); // 8-bytes padding, ignored by the server.
                        break;

                    case AddressFamily.InterNetworkV6:
                        nlSocketAddress.lpSockaddr = ArrayUtility.ConcatenateArrays(
                            BitConverter.GetBytes((ushort)(AddressFamily.InterNetworkV6)),
                            BitConverter.GetBytes((ushort)sockaddr.Port),
                            new byte[4], //FlowInfo (4 bytes): Flow information. 
                            //This field is not currently used by the protocol. 
                            //The field MUST be set to zero and MUST be ignored on receipt.
                            sockaddr.Address.GetAddressBytes(),
                            BitConverter.GetBytes((uint)sockaddr.Address.ScopeId));
                        break;

                    default:
                        throw new ArgumentException("sockaddr is neither IPv4 nor IPv6.", "sockaddr");
                }
                nlSocketAddress.iSockaddrLength = (uint)nlSocketAddress.lpSockaddr.Length;
            }

            return nlSocketAddress;
        }


        /// <summary>
        /// Create an instance of NL_AUTH_MESSAGE.
        /// </summary>
        /// <returns>Created NL_AUTH_MESSAGE structure.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the method is called before establishing a NRPC secure channel.
        /// </exception>
        public NL_AUTH_MESSAGE CreateNlAuthMessage()
        {
            const char NULL = '\0';

            ValidateSecureChannelExists();

            NL_AUTH_MESSAGE nlAuthMessage = new NL_AUTH_MESSAGE();

            nlAuthMessage.MessageType = MessageType_Values.NegotiateRequest;

            if (context.DomainName.Contains("."))
            {
                // context.DomainName is a DNS name.
                nlAuthMessage.Flags =
                    NL_AUTH_MESSAGE_Flags_Value.NetbiosOemComputerName
                    | NL_AUTH_MESSAGE_Flags_Value.DnsCompressedDomainName;
                nlAuthMessage.Buffer = ArrayUtility.ConcatenateArrays(
                    Encoding.ASCII.GetBytes(context.ClientComputerName + NULL),
                    Rfc1035Utility.ToCompressedUtf8String(context.DomainName));
            }
            else
            {
                // context.DomainName is a NetBIOS name.
                nlAuthMessage.Flags =
                    NL_AUTH_MESSAGE_Flags_Value.NetbiosOemDomainName
                    | NL_AUTH_MESSAGE_Flags_Value.NetbiosOemComputerName;
                nlAuthMessage.Buffer = ArrayUtility.ConcatenateArrays(
                    Encoding.ASCII.GetBytes(context.DomainName + NULL),
                    Encoding.ASCII.GetBytes(context.ClientComputerName + NULL));
            }

            return nlAuthMessage;
        }


        /// <summary>
        /// Create an instance of CHANGELOG_ENTRY.
        /// </summary>
        /// <param name="serialNumber">
        /// The database serial number that corresponds to this 
        /// account object (64-bit integer).
        /// </param>
        /// <param name="objectRid">
        /// The RID of the object (32-bit integer).
        /// </param>
        /// <param name="dbIndex">
        /// The 8-bit integer identifier of the database containing the object. 
        /// MUST be one, and only one, of the following values.
        /// </param>
        /// <param name="deltaType">
        /// One of the NETLOGON_DELTA_TYPE values specified in section 2.2.1.5.28.
        /// </param>
        /// <param name="objectSid">
        /// The SID of the object. Included only if flag C is set. This is 
        /// an RPC_SID structure, as defined in [MS-DTYP] section 2.4.2.2. 
        /// This parameter cannot combined with objectName parameter.
        /// </param>
        /// <param name="objectName">
        /// The name of the object. ObjectName is a null-terminated Unicode 
        /// string, and is included only if flag D is set. 
        /// This parameter cannot combined with objectSid parameter.
        /// </param>
        /// <returns>Created CHANGELOG_ENTRY structure.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when objectName parameter is combined with objectSid parameter.
        /// </exception>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public CHANGELOG_ENTRY CreateChangeLogEntry(
            ulong serialNumber,
            uint objectRid,
            DBIndex_Values dbIndex,
            _NETLOGON_DELTA_TYPE deltaType,
            Dtyp._RPC_SID? objectSid,
            string objectName)
        {
            if (objectSid != null && objectName != null)
            {
                throw new ArgumentException("ObjectName cannot combined with ObjectSid.", "objectName");
            }

            CHANGELOG_ENTRY changeLogEntry = new CHANGELOG_ENTRY();

            changeLogEntry.SerialNumber = serialNumber;
            changeLogEntry.ObjectRid = objectRid;
            changeLogEntry.DBIndex = dbIndex;
            changeLogEntry.DeltaType = deltaType;
            if (objectSid != null)
            {
                changeLogEntry.ObjectSid = objectSid.Value;
                changeLogEntry.Flags |= CHANGELOG_ENTRY_Flags_Values.IncludeObjectSid;
            }
            else if (objectName != null)
            {
                changeLogEntry.ObjectName = objectName;
                changeLogEntry.Flags |= CHANGELOG_ENTRY_Flags_Values.IncludeObjectName;
            }

            return changeLogEntry;
        }


        /// <summary>
        /// Validate NL_AUTH_MESSAGE when the client receives the return token.
        /// </summary>
        /// <param name="nlAuthMessage">NL_AUTH_MESSAGE token.</param>
        /// <returns>True if validate pass; otherwise, false.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the method is called before establishing a NRPC secure channel.
        /// </exception>
        public bool ValidateNlAuthMessage(NL_AUTH_MESSAGE nlAuthMessage)
        {
            ValidateSecureChannelExists();

            //When the client receives the return token, it verifies that:
            //the NL_AUTH_MESSAGE token is at least 12 bytes in length, and
            //the MessageType is set to 1.
            if (nlAuthMessage.MessageType == MessageType_Values.NegotiateResponse)
            {
                //NL_AUTH_MESSAGE is always at least 12 bytes.

                //the client initializes ClientSequenceNumber to 0, 
                //which is used to detect out-of-order messages.
                context.SequenceNumber = 0;
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Check whether AES or RC4 is negotiated.
        /// </summary>
        /// <returns>
        /// Returns true if AES is negotiated, 
        /// returns false if RC4 is negotiated, 
        /// otherwise, throw exception.
        /// </returns>
        private bool IsAesOrRc4Negotiated()
        {
            bool isAesNegotiated;
            if ((context.NegotiateFlags & NrpcNegotiateFlags.SupportsAESAndSHA2) != 0)
            {
                isAesNegotiated = true;
            }
            else if ((context.NegotiateFlags & NrpcNegotiateFlags.SupportsRC4) != 0)
            {
                isAesNegotiated = false;
            }
            else
            {
                throw new InvalidOperationException("Neither AES nor RC4 is negotiated.");
            }
            return isAesNegotiated;
        }


        /// <summary>
        /// Verify if secure channel exists.
        /// </summary>
        private void ValidateSecureChannelExists()
        {
            if (context.SessionKey == null
                || context.StoredCredential == null)
            {
                throw new InvalidOperationException(
                    "This method is not supported before establishing a NRPC secure channel.");
            }
        }


        #endregion


        #region NRPC RPC methods

        /// <summary>
        ///  NetrLogonUasLogon IDL method. Opnum: 0 
        /// </summary>
        /// <param name="serverName">
        ///  ServerName parameter.
        /// </param>
        /// <param name="userName">
        ///  UserName parameter.
        /// </param>
        /// <param name="workstation">
        ///  Workstation parameter.
        /// </param>
        /// <param name="validationInformation">
        ///  ValidationInformation parameter.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NetApiStatus NetrLogonUasLogon(
            string serverName,
            string userName,
            string workstation,
            out _NETLOGON_VALIDATION_UAS_INFO? validationInformation)
        {
            return rpc.NetrLogonUasLogon(
                serverName,
                userName,
                workstation,
                out validationInformation);
        }


        /// <summary>
        ///  NetrLogonUasLogoff IDL method. Opnum: 1 
        /// </summary>
        /// <param name="serverName">
        ///  ServerName parameter.
        /// </param>
        /// <param name="userName">
        ///  UserName parameter.
        /// </param>
        /// <param name="workstation">
        ///  Workstation parameter.
        /// </param>
        /// <param name="logoffInformation">
        ///  LogoffInformation parameter.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NetApiStatus NetrLogonUasLogoff(
            string serverName,
            string userName,
            string workstation,
            out _NETLOGON_LOGOFF_UAS_INFO? logoffInformation)
        {
            return rpc.NetrLogonUasLogoff(
                serverName,
                userName,
                workstation,
                out logoffInformation);
        }


        /// <summary>
        ///  The NetrLogonSamLogon method This method was used in
        ///  windows_nt_4_0. It was superseded by the NetrLogonSamLogonWithFlags
        ///  method in windows_2000_server, windows_xp,
        ///  windows_server_2003, windows_vista, and windows_server_2008,
        ///  windows_7, and windows_server_7. is a predecessor to
        ///  the NetrLogonSamLogonWithFlags method. All
        ///  parameters of this method have the same meanings as
        ///  the identically named parameters of the NetrLogonSamLogonWithFlags
        ///  method. Opnum: 2 
        /// </summary>
        /// <param name="logonServer">
        ///  LogonServer parameter.
        /// </param>
        /// <param name="computerName">
        ///  ComputerName parameter.
        /// </param>
        /// <param name="authenticator">
        ///  Authenticator parameter.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  ReturnAuthenticator parameter.
        /// </param>
        /// <param name="logonLevel">
        ///  LogonLevel parameter.
        /// </param>
        /// <param name="logonInformation">
        ///  LogonInformation parameter.
        /// </param>
        /// <param name="validationLevel">
        ///  ValidationLevel parameter.
        /// </param>
        /// <param name="validationInformation">
        ///  ValidationInformation parameter.
        /// </param>
        /// <param name="authoritative">
        ///  Authoritative parameter.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NtStatus NetrLogonSamLogon(
            string logonServer,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            ref _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            _NETLOGON_LOGON_INFO_CLASS logonLevel,
            _NETLOGON_LEVEL? logonInformation,
            _NETLOGON_VALIDATION_INFO_CLASS validationLevel,
            out _NETLOGON_VALIDATION? validationInformation,
            out byte? authoritative)
        {
            NtStatus status = rpc.NetrLogonSamLogon(
                logonServer,
                computerName,
                authenticator,
                ref returnAuthenticator,
                logonLevel,
                logonInformation,
                validationLevel,
                out validationInformation,
                out authoritative);

            //ConnectionStatus: A 4-byte value that contains the most recent 
            //connection status return value (section 3.4.5.3.1) last returned 
            //during secure channel establishment or by a method requiring 
            //session key establishment (section 3.1.4.6).
            context.ConnectionStatus = status;
            return status;
        }


        /// <summary>
        ///  The NetrLogonSamLogoff method handles logoff requests
        ///  for the SAM accounts. Opnum: 3 
        /// </summary>
        /// <param name="logonServer">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="computerName">
        ///  The Unicode string that contains the NetBIOS name of
        ///  the client computer calling this method.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, 
        ///  that contains the client authenticator.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, 
        ///  that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="logonLevel">
        ///  A NETLOGON_LOGON_INFO_CLASS structure, 
        ///  that identifies the type of logon information
        ///  in the LogonInformation union.
        /// </param>
        /// <param name="logonInformation">
        ///  A pointer to a NETLOGON_LEVEL structure, 
        ///  that describes the logon information.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NtStatus NetrLogonSamLogoff(
            string logonServer,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            ref _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            _NETLOGON_LOGON_INFO_CLASS logonLevel,
            _NETLOGON_LEVEL? logonInformation)
        {
            NtStatus status = rpc.NetrLogonSamLogoff(
                logonServer,
                computerName,
                authenticator,
                ref returnAuthenticator,
                logonLevel,
                logonInformation);

            context.ConnectionStatus = status;
            return status;
        }


        /// <summary>
        ///  The NetrServerReqChallenge method receives a client
        ///  challenge and returns a server challenge. Opnum: 4<para/>
        /// </summary>
        /// <param name="primaryName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="computerName">
        ///  A Unicode string that contains the NetBIOS name of the
        ///  client computer calling this method.
        /// </param>
        /// <param name="clientChallenge">
        ///  A pointer to a NETLOGON_CREDENTIAL structure, 
        ///  that contains the client challenge.
        /// </param>
        /// <param name="serverChallenge">
        ///  A pointer to a NETLOGON_CREDENTIAL structure, 
        ///  that contains the server challenge response.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NtStatus NetrServerReqChallenge(
            string primaryName,
            string computerName,
            _NETLOGON_CREDENTIAL? clientChallenge,
            out _NETLOGON_CREDENTIAL? serverChallenge)
        {
            context.PrimaryName = primaryName;
            context.ClientComputerName = computerName;
            if (clientChallenge != null)
            {
                context.ClientChallenge = clientChallenge.Value.data;
            }

            NtStatus status = rpc.NetrServerReqChallenge(
                primaryName,
                computerName,
                clientChallenge,
                out serverChallenge);

            context.ConnectionStatus = status;
            if (status == NtStatus.STATUS_SUCCESS && serverChallenge != null)
            {
                context.ServerChallenge = serverChallenge.Value.data;
            }

            return status;
        }


        /// <summary>
        ///  The NetrServerAuthenticate method This method was used
        ///  in windows_nt_server_3_1. In windows_nt_server_3_5,
        ///  it was superseded by the NetrServerAuthenticate2 method.
        ///  In windows_2000_server, windows_xp, windows_server_2003,
        ///  windows_vista, windows_server_2008, windows_7, and
        ///  windows_server_7, the NetrServerAuthenticate2 method
        ///  was superseded by the NetrServerAuthenticate3
        ///  method. is a predecessor to the NetrServerAuthenticate3
        ///  method. All parameters of this method have
        ///  the same meanings as the identically named parameters
        ///  of the NetrServerAuthenticate3 method. Opnum: 5 
        /// </summary>
        /// <param name="primaryName">
        ///  PrimaryName parameter.
        /// </param>
        /// <param name="accountName">
        ///  AccountName parameter.
        /// </param>
        /// <param name="secureChannelType">
        ///  SecureChannelType parameter.
        /// </param>
        /// <param name="computerName">
        ///  ComputerName parameter.
        /// </param>
        /// <param name="clientCredential">
        ///  ClientCredential parameter.
        /// </param>
        /// <param name="serverCredential">
        ///  ServerCredential parameter.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NtStatus NetrServerAuthenticate(
            string primaryName,
            string accountName,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            string computerName,
            _NETLOGON_CREDENTIAL? clientCredential,
            out _NETLOGON_CREDENTIAL? serverCredential)
        {
            NtStatus status = rpc.NetrServerAuthenticate(
                primaryName,
                accountName,
                secureChannelType,
                computerName,
                clientCredential,
                out serverCredential);

            context.ConnectionStatus = status;
            return status;
        }


        /// <summary>
        ///  The NetrServerPasswordSet method sets a new one-way
        ///  function (OWF) of a password for an account used by
        ///  the domain controller for
        ///  setting up the secure channel from the client. Opnum: 6 
        /// </summary>
        /// <param name="primaryName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="accountName">
        ///  The null-terminated Unicode string that contains the
        ///  name of the account whose password is being changed. In
        ///  windows, all machine account names are the name of
        ///  the machine with a $ (dollar sign) appended.
        /// </param>
        /// <param name="secureChannelType">
        ///  An enumerated value that indicates
        ///  the type of secure channel used by the client.
        /// </param>
        /// <param name="computerName">
        ///  A null-terminated Unicode string that contains the NetBIOS
        ///  name of the client computer calling this method.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, 
        ///  that contains the client authenticator.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, 
        ///  that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="uasNewPassword">
        ///  A pointer to an ENCRYPTED_LM_OWF_PASSWORD structure,
        ///  and encrypted.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NtStatus NetrServerPasswordSet(
            string primaryName,
            string accountName,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            out _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            _LM_OWF_PASSWORD? uasNewPassword)
        {
            NtStatus status = rpc.NetrServerPasswordSet(
                primaryName,
                accountName,
                secureChannelType,
                computerName,
                authenticator,
                out returnAuthenticator,
                uasNewPassword);

            context.ConnectionStatus = status;
            return status;
        }


        /// <summary>
        /// The NetrDatabaseDeltas method returns a set of changes (or deltas) 
        /// performed to the SAM, SAM built-in, or LSA databases after a particular 
        /// value of the database serial number. It is used by BDCs to request 
        /// database changes from the PDC that are missing on the BDC. Opnum: 7
        /// </summary>
        /// <param name="PrimaryName">
        /// The custom RPC binding handle (as specified in section 3.5.5.1) that 
        /// represents the connection to the PDC.
        /// </param>
        /// <param name="ComputerName">
        /// The null-terminated Unicode string that contains the NetBIOS name of 
        /// the BDC calling this method.
        /// </param>
        /// <param name="Authenticator">
        /// A pointer to a NETLOGON_AUTHENTICATOR structure that contains the 
        /// client authenticator.
        /// </param>
        /// <param name="ReturnAuthenticator">
        /// A pointer to a NETLOGON_AUTHENTICATOR structure that contains the 
        /// server return authenticator.
        /// </param>
        /// <param name="DatabaseID">
        /// The identifier for a specific account database set as follows: 
        /// 0x00000000
        /// Indicates the SAM database.
        /// 0x00000001
        /// Indicates the SAM built-in database.
        /// 0x00000002
        /// Indicates the LSA database.
        /// </param>
        /// <param name="DomainModifiedCount">
        /// A pointer to an NLPR_MODIFIED_COUNT structure, as specified in section 
        /// 2.2.1.5.26, that contains the database serial number. On input, this is 
        /// the value of the database serial number on the client. On output, this is 
        /// the value of the database serial number corresponding to the last element 
        /// (delta) returned in the DeltaArray parameter.
        /// </param>
        /// <param name="DeltaArray">
        /// A pointer to a NETLOGON_DELTA_ENUM_ARRAY structure that contains an array 
        /// of enumerated changes (deltas) to the specified database with database 
        /// serial numbers larger than the database serial number value specified in 
        /// the input value of the DomainModifiedCount parameter.
        /// </param>
        /// <param name="PreferredMaximumLength">
        /// The value that specifies the preferred maximum size, in bytes, of data to 
        /// return in the DeltaArray parameter. This is not a hard upper limit, but 
        /// serves as a guide to the server. The server SHOULD stop including 
        /// elements in the returned DeltaArray after the size of the returned data 
        /// equals or exceeds the value of the PreferredMaximumLength parameter. It is 
        /// up to the client implementation to choose the value for this parameter.
        /// </param>
        public NtStatus NetrDatabaseDeltas(
            string PrimaryName,
            string ComputerName,
            _NETLOGON_AUTHENTICATOR? Authenticator,
            ref _NETLOGON_AUTHENTICATOR? ReturnAuthenticator,
            DatabaseID_Values DatabaseID,
            ref _NLPR_MODIFIED_COUNT? DomainModifiedCount,
            out _NETLOGON_DELTA_ENUM_ARRAY? DeltaArray,
            uint PreferredMaximumLength)
        {
            NtStatus status = rpc.NetrDatabaseDeltas(
                PrimaryName,
                ComputerName,
                Authenticator,
                ref ReturnAuthenticator,
                DatabaseID,
                ref DomainModifiedCount,
                out DeltaArray,
                PreferredMaximumLength);

            context.ConnectionStatus = status;
            return status;
        }


        /// <summary>
        ///  The NetrDatabaseSync method is a predecessor to the NetrDatabaseSync2 method, 
        ///  as specified in section 3.5.5.5.2. All parameters of this method have the same 
        ///  meanings as the identically named parameters of the NetrDatabaseSync2 method. 
        ///  Opnum: 8
        /// </summary>
        /// <param name="PrimaryName">
        ///  The custom RPC binding handle, as specified in section
        ///  , representing the connection to the PDC.
        /// </param>
        /// <param name="ComputerName">
        ///  The null-terminated Unicode string that contains the
        ///  NetBIOS name of the BDC calling this method.
        /// </param>
        /// <param name="Authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the client authenticator.
        /// </param>
        /// <param name="ReturnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="DatabaseID">
        ///  The identifier for a specific database for which the
        ///  changes are requested. It MUST be one of the following
        ///  values.
        /// </param>
        /// <param name="SyncContext">
        ///  Specifies context needed to continue the operation.
        ///  The value MUST be set to zero on the first call. The
        ///  caller MUST treat this as an opaque value, unless this
        ///  call is a restart of the series of synchronization
        ///  calls. The value returned is to be used on input for
        ///  the next call in the series of synchronization calls. If
        ///  this call is the restart of the series, the values
        ///  of the RestartState and the SyncContext parameters
        ///  are dependent on the DeltaType value received on the
        ///  last call before the restart and MUST be set as follows.
        ///  Find the last NETLOGON_DELTA_ENUM structure in the
        ///  DeltaArray parameter of the call. The DeltaType field
        ///  of this NETLOGON_DELTA_ENUM structure, as specified
        ///  in section , is the DeltaType needed for the restart.
        ///  The values of RestartState and SyncContext are then
        ///  determined from the following table. DeltaTypeRestartStateSyncContextAddOrChangeGroupGroupStateThe
        ///  value of the RID field of the last element AddOrChangeUserUserStateThe
        ///  value of the RID field of the last element ChangeGroupMembershipGroupMemberStateThe
        ///  value of the RID field of the last element AddOrChangeAliasAliasState0x00000000ChangeAliasMembershipAliasMemberState0x00000000Any
        ///  other value not previously listedNormalState0x00000000
        /// </param>
        /// <param name="DeltaArray">
        ///  A pointer to a NETLOGON_DELTA_ENUM_ARRAY structure,
        ///  as specified in section , that contains an array of
        ///  enumerated changes (deltas) to the specified database.
        /// </param>
        /// <param name="PreferredMaximumLength">
        ///  The value that specifies the preferred maximum size,
        ///  in bytes, of data referenced in the DeltaArray parameter.
        ///  This is not a hard upper limit, but serves as a guide
        ///  to the server. The server SHOULDwindows stops including
        ///  elements in the returned DeltaArray once the size of
        ///  the returned data equals or exceeds the value of the
        ///  PreferredMaximumLength parameter. The server SHOULD stop including elements
        ///  in the returned DeltaArray once the size of the returned
        ///  data equals or exceeds the value of the PreferredMaximumLength
        ///  parameter. It is up to the client implementation to
        ///  choose the value for this parameter.
        /// </param>
        public NtStatus NetrDatabaseSync(
            string PrimaryName,
            string ComputerName,
            _NETLOGON_AUTHENTICATOR? Authenticator,
            ref _NETLOGON_AUTHENTICATOR? ReturnAuthenticator,
            DatabaseID_Values DatabaseID,
            ref uint? SyncContext,
            out _NETLOGON_DELTA_ENUM_ARRAY? DeltaArray,
            uint PreferredMaximumLength)
        {
            NtStatus status = rpc.NetrDatabaseSync(
                PrimaryName,
                ComputerName,
                Authenticator,
                ref ReturnAuthenticator,
                DatabaseID,
                ref SyncContext,
                out DeltaArray,
                PreferredMaximumLength);

            context.ConnectionStatus = status;
            return status;
        }


        /// <summary>
        ///  NetrAccountDeltas IDL method. Opnum: 9 
        /// </summary>
        /// <param name="primaryName">
        ///  PrimaryName parameter.
        /// </param>
        /// <param name="computerName">
        ///  ComputerName parameter.
        /// </param>
        /// <param name="authenticator">
        ///  Authenticator parameter.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  ReturnAuthenticator parameter.
        /// </param>
        /// <param name="recordID">
        ///  RecordID parameter.
        /// </param>
        /// <param name="count">
        ///  Count parameter.
        /// </param>
        /// <param name="level">
        ///  Level parameter.
        /// </param>
        /// <param name="buffer">
        ///  Buffer parameter.
        /// </param>
        /// <param name="bufferSize">
        ///  BufferSize parameter.
        /// </param>
        /// <param name="countReturned">
        ///  CountReturned parameter.
        /// </param>
        /// <param name="totalEntries">
        ///  TotalEntries parameter.
        /// </param>
        /// <param name="nextRecordId">
        ///  NextRecordId parameter.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NtStatus NetrAccountDeltas(
            string primaryName,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            ref _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            _UAS_INFO_0? recordID,
            uint count,
            uint level,
            out byte[] buffer,
            uint bufferSize,
            out uint? countReturned,
            out uint? totalEntries,
            out _UAS_INFO_0? nextRecordId)
        {
            NtStatus status = rpc.NetrAccountDeltas(
                primaryName,
                computerName,
                authenticator,
                ref returnAuthenticator,
                recordID,
                count,
                level,
                out buffer,
                bufferSize,
                out countReturned,
                out totalEntries,
                out nextRecordId);

            context.ConnectionStatus = status;
            return status;
        }


        /// <summary>
        ///  NetrAccountSync IDL method. Opnum: 10 
        /// </summary>
        /// <param name="primaryName">
        ///  PrimaryName parameter.
        /// </param>
        /// <param name="computerName">
        ///  ComputerName parameter.
        /// </param>
        /// <param name="authenticator">
        ///  Authenticator parameter.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  ReturnAuthenticator parameter.
        /// </param>
        /// <param name="reference">
        ///  Reference parameter.
        /// </param>
        /// <param name="level">
        ///  Level parameter.
        /// </param>
        /// <param name="buffer">
        ///  Buffer parameter.
        /// </param>
        /// <param name="bufferSize">
        ///  BufferSize parameter.
        /// </param>
        /// <param name="countReturned">
        ///  CountReturned parameter.
        /// </param>
        /// <param name="totalEntries">
        ///  TotalEntries parameter.
        /// </param>
        /// <param name="nextReference">
        ///  NextReference parameter.
        /// </param>
        /// <param name="lastRecordId">
        ///  LastRecordId parameter.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NtStatus NetrAccountSync(
            string primaryName,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            ref _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            uint reference,
            uint level,
            out byte[] buffer,
            uint bufferSize,
            out uint? countReturned,
            out uint? totalEntries,
            out uint? nextReference,
            out _UAS_INFO_0? lastRecordId)
        {
            NtStatus status = rpc.NetrAccountSync(
                primaryName,
                computerName,
                authenticator,
                ref returnAuthenticator,
                reference,
                level,
                out buffer,
                bufferSize,
                out countReturned,
                out totalEntries,
                out nextReference,
                out lastRecordId);

            context.ConnectionStatus = status;
            return status;
        }


        /// <summary>
        ///  The NetrGetDCName method This method was used in windows_nt_server_3_1
        ///  and is supported in windows_nt_server_3_1 versions.
        ///  It was superseded by the DsrGetDcNameEx2 method 
        ///  in windows_2000, windows_xp, windows_server_2003,
        ///  windows_vista,  and windows_server_2008windows_7, and
        ///  windows_server_7. retrieves the NetBIOS name of the
        ///  PDC for the specified domain. Opnum: 11 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle, 
        ///  that represents the connection to a domain controller.
        /// </param>
        /// <param name="domainName">
        ///  A null-terminated Unicode string that specifies the
        ///  NetBIOS name of the domain.
        /// </param>
        /// <param name="buffer">
        ///  A pointer to a null-terminated Unicode string that contains
        ///  the NetBIOS name of the PDC for the specified domain.
        ///  The server name returned by this method is prefixed
        ///  by two backslashes (\\).
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NetApiStatus NetrGetDCName(
            string serverName,
            string domainName,
            out string buffer)
        {
            return rpc.NetrGetDCName(
                serverName,
                domainName,
                out buffer);
        }

        /// <summary>
        ///  The NetrLogonControl method is a predecessor to the
        ///  NetrLogonControl2Ex method. 
        ///  All parameters of this method have the same meanings
        ///  as the identically named parameters of the NetrLogonControl2Ex
        ///  method. Opnum: 12 
        /// </summary>
        /// <param name="serverName">
        ///  ServerName parameter.
        /// </param>
        /// <param name="functionCode">
        ///  FunctionCode parameter.
        /// </param>
        /// <param name="queryLevel">
        ///  QueryLevel parameter.
        /// </param>
        /// <param name="buffer">
        ///  Buffer parameter.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NetApiStatus NetrLogonControl(
            string serverName,
            FunctionCode_Values functionCode,
            QueryLevel_Values queryLevel,
            out _NETLOGON_CONTROL_QUERY_INFORMATION? buffer)
        {
            return rpc.NetrLogonControl(
                serverName,
                (uint)functionCode,
                (uint)queryLevel,
                out buffer);
        }

        /// <summary>
        ///  The NetrGetAnyDCName method This method was introduced
        ///  in windows_nt_server_3_1 and is supported in windows_nt_server_3_1
        ///  versions. It was superseded by the DsrGetDcNameEx2
        ///  method in windows_2000, windows_xp, windows_server_2003,
        ///  windows_vista, and windows_server_2008, windows_7,
        ///  and windows_server_7. retrieves the name of a domain
        ///  controller in the specified primary or directly trusted
        ///  domain. Only DCs can return the name of a DC in a specified
        ///  directly trusted domain. Opnum: 13 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="domainName">
        ///  A null-terminated Unicode string that contains the name
        ///  of the primary or directly trusted domain. If the string
        ///  is NULL or empty (that is, the first character in the
        ///  string is the null-terminator character), the primary
        ///  domain name (3) is assumed.
        /// </param>
        /// <param name="buffer">
        ///  A pointer to an allocated buffer that contains the null-terminated
        ///  Unicode string containing the NetBIOS name of a DC
        ///  in the specified domain. The DC name is prefixed by
        ///  two backslashes (\\).
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NetApiStatus NetrGetAnyDCName(
            string serverName,
            string domainName,
            out string buffer)
        {
            return rpc.NetrGetAnyDCName(
                serverName,
                domainName,
                out buffer);
        }

        /// <summary>
        ///  The NetrLogonControl2 method is a predecessor to the
        ///  NetrLogonControl2Ex method. 
        ///  All parameters of this method have the same meanings
        ///  as the identically named parameters of the NetrLogonControl2Ex
        ///  method. Opnum: 14 
        /// </summary>
        /// <param name="serverName">
        ///  ServerName parameter.
        /// </param>
        /// <param name="functionCode">
        ///  FunctionCode parameter.
        /// </param>
        /// <param name="queryLevel">
        ///  QueryLevel parameter.
        /// </param>
        /// <param name="data">
        ///  Data parameter.
        /// </param>
        /// <param name="buffer">
        ///  Buffer parameter.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NetApiStatus NetrLogonControl2(
            string serverName,
            FunctionCode_Values functionCode,
            QueryLevel_Values queryLevel,
            _NETLOGON_CONTROL_DATA_INFORMATION? data,
            out _NETLOGON_CONTROL_QUERY_INFORMATION? buffer)
        {
            return rpc.NetrLogonControl2(
                serverName,
                (uint)functionCode,
                (uint)queryLevel,
                data,
                out buffer);
        }


        /// <summary>
        ///  The NetrServerAuthenticate2 method This method was used
        ///  in windows_nt_3_5 and windows_nt_4_0. In windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7, it was superseded
        ///  by the NetrServerAuthenticate3 method. is
        ///  a predecessor to the NetrServerAuthenticate3 method. 
        ///  All parameters of this method
        ///  have the same meanings as the identically named parameters
        ///  of the NetrServerAuthenticate3 method. Opnum: 15 
        /// </summary>
        /// <param name="primaryName">
        ///  PrimaryName parameter.
        /// </param>
        /// <param name="accountName">
        ///  AccountName parameter.
        /// </param>
        /// <param name="secureChannelType">
        ///  SecureChannelType parameter.
        /// </param>
        /// <param name="computerName">
        ///  ComputerName parameter.
        /// </param>
        /// <param name="clientCredential">
        ///  ClientCredential parameter.
        /// </param>
        /// <param name="serverCredential">
        ///  ServerCredential parameter.
        /// </param>
        /// <param name="negotiateFlags">
        ///  NegotiateFlags parameter.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NtStatus NetrServerAuthenticate2(
            string primaryName,
            string accountName,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            string computerName,
            _NETLOGON_CREDENTIAL? clientCredential,
            out _NETLOGON_CREDENTIAL? serverCredential,
            ref NrpcNegotiateFlags? negotiateFlags)
        {
            uint? flags = (uint?)negotiateFlags;

            NtStatus status = rpc.NetrServerAuthenticate2(
                primaryName,
                accountName,
                secureChannelType,
                computerName,
                clientCredential,
                out serverCredential,
                ref flags);

            context.ConnectionStatus = status;
            negotiateFlags = (NrpcNegotiateFlags?)flags;
            return status;
        }


        /// <summary>
        ///  The NetrDatabaseSync2 method returns a set of all changes
        ///  applied to the specified database since its creation.
        ///  It provides an interface for a BDC to fully synchronize
        ///  its databases to those of the PDC. Because returning
        ///  all changes in one call might be prohibitively expensive
        ///  due to a large amount of data being returned, this
        ///  method supports retrieving portions of the database
        ///  changes in a series of calls using a continuation context
        ///  until all changes are received. It is possible for
        ///  the series of calls to be terminated prematurely due
        ///  to external events, such as system restarts. For that
        ///  reason, the method also supports restarting the series
        ///  of calls at a particular point specified by the caller.
        ///  The caller MUST keep track of synchronization progress
        ///  during the series of calls.
        ///  Opnum: 16 
        /// </summary>
        /// <param name="primaryName">
        ///  The custom RPC binding handle, 
        ///  that represents the connection to the PDC.
        /// </param>
        /// <param name="computerName">
        ///  The null-terminated Unicode string that contains the
        ///  NetBIOS name of the BDC calling this method.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, 
        ///  that contains the client authenticator.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, 
        ///  that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="databaseID">
        ///  The identifier for a specific database for which the
        ///  changes are requested. It MUST be one of the following
        ///  values.
        /// </param>
        /// <param name="restartState">
        ///  Specifies whether this is a restart of the series of
        ///  the synchronization calls and how to interpret SyncContext.
        ///  This value MUST be NormalState unless this is the restart,
        ///  in which case the value MUST be set as specified in
        ///  the description of the SyncContext parameter.
        /// </param>
        /// <param name="syncContext">
        ///  Specifies context needed to continue the operation.
        ///  The value MUST be set to zero on the first call. The
        ///  caller MUST treat this as an opaque value, unless this
        ///  call is a restart of the series of synchronization
        ///  calls. The value returned is to be used on input for
        ///  the next call in the series of synchronization calls. If
        ///  this call is the restart of the series, the values
        ///  of the RestartState and the SyncContext parameters
        ///  are dependent on the DeltaType value received on the
        ///  last call before the restart and MUST be set as follows.
        ///  Find the last NETLOGON_DELTA_ENUM structure in the
        ///  DeltaArray parameter of the call. The DeltaType field
        ///  of this NETLOGON_DELTA_ENUM structure 
        ///  is the DeltaType needed for the restart.
        ///  The values of RestartState and SyncContext are then
        ///  determined from the following table.<para/>
        ///  DeltaType  RestartState    SyncContext<para/>
        ///  AddOrChangeGroup   GroupState  The value of the RID field of the last element<para/>
        ///  AddOrChangeUser    UserState   The value of the RID field of the last element<para/>
        ///  ChangeGroupMembership  GroupMemberState    The value of the RID field of the last element<para/>
        ///  AddOrChangeAlias   AliasState 0x00000000<para/>
        ///  ChangeAliasMembership  AliasMemberState    0x00000000<para/>
        ///  Any other value not previously listed  NormalState 0x00000000
        /// </param>
        /// <param name="deltaArray">
        ///  A pointer to a NETLOGON_DELTA_ENUM_ARRAY structure,
        ///  that contains an array of
        ///  enumerated changes (deltas) to the specified database.
        /// </param>
        /// <param name="preferredMaximumLength">
        ///  The value that specifies the preferred maximum size,
        ///  in bytes, of data referenced in the DeltaArray parameter.
        ///  This is not a hard upper limit, but serves as a guide
        ///  to the server. The server SHOULDwindows stops including
        ///  elements in the returned DeltaArray once the size of
        ///  the returned data equals or exceeds the value of the
        ///  PreferredMaximumLength parameter. The server SHOULD stop including elements
        ///  in the returned DeltaArray once the size of the returned
        ///  data equals or exceeds the value of the PreferredMaximumLength
        ///  parameter. It is up to the client implementation to
        ///  choose the value for this parameter.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NtStatus NetrDatabaseSync2(
            string primaryName,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            ref _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            DatabaseID_Values databaseID,
            _SYNC_STATE restartState,
            ref uint? syncContext,
            out _NETLOGON_DELTA_ENUM_ARRAY? deltaArray,
            uint preferredMaximumLength)
        {
            NtStatus status = rpc.NetrDatabaseSync2(
                primaryName,
                computerName,
                authenticator,
                ref returnAuthenticator,
                databaseID,
                restartState,
                ref syncContext,
                out deltaArray,
                preferredMaximumLength);

            context.ConnectionStatus = status;
            return status;
        }


        /// <summary>
        ///  The NetrDatabaseRedo method is used by a BDC to request
        ///  information about a single account from the PDC. Opnum: 17 
        /// </summary>
        /// <param name="primaryName">
        ///  The custom RPC binding handle 
        ///  representing the connection to the PDC.
        /// </param>
        /// <param name="computerName">
        ///  The null-terminated Unicode string that contains the
        ///  NetBIOS name of the BDC calling this method.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, 
        ///  that contains the client authenticator.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, 
        ///  that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="changeLogEntry">
        ///  A pointer to a buffer that contains a CHANGELOG_ENTRY
        ///  structure, specified as follows, for the account being
        ///  queried.
        /// </param>
        /// <param name="changeLogEntrySize">
        ///  The size, in bytes, of the buffer pointed to by the
        ///  ChangeLogEntry parameter.
        /// </param>
        /// <param name="deltaArray">
        ///  A pointer to a NETLOGON_DELTA_ENUM_ARRAY structure,
        ///  that contains an array of
        ///  enumerated database changes for the account being queried.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NtStatus NetrDatabaseRedo(
            string primaryName,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            ref _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            byte[] changeLogEntry,
            uint changeLogEntrySize,
            out _NETLOGON_DELTA_ENUM_ARRAY? deltaArray)
        {
            NtStatus status = rpc.NetrDatabaseRedo(
                primaryName,
                computerName,
                authenticator,
                ref returnAuthenticator,
                changeLogEntry,
                changeLogEntrySize,
                out deltaArray);

            context.ConnectionStatus = status;
            return status;
        }


        /// <summary>
        ///  The NetrLogonControl2Ex method executes windows-specific
        ///  administrative actions that pertain to the Netlogon
        ///  server operation. It is used to query the status and
        ///  control the actions of the Netlogon server. Opnum: 18 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="functionCode">
        ///  The control operation to be performed; MUST be one of
        ///  the following values. The following restrictions apply
        ///  to the values of the FunctionCode parameter in windows_nt_4_0,
        ///  windows_2000, windows_7, and windows_server_7. There
        ///  are no restrictions in windows_server_2003, windows_vista,
        ///  and windows_server_2008. The following values are not
        ///  supported on windows_nt_4_0: NETLOGON_CONTROL_CHANGE_PASSWORD
        ///  (0x00000009) NETLOGON_CONTROL_TC_VERIFY (0x0000000A) NETLOGON_CONTROL_FORCE_DNS_REG
        ///  (0x0000000B) NETLOGON_CONTROL_QUERY_DNS_REG (0x0000000C) NETLOGON_CONTROL_BACKUP_CHANGE_LOG
        ///  (0x0000FFFC) NETLOGON_CONTROL_TRUNCATE_LOG (0x0000FFFD) NETLOGON_CONTROL_SET_DBFLAG
        ///  (0x0000FFFE) NETLOGON_CONTROL_BREAKPOINT (0x0000FFFF).<para/>
        ///  The error ERROR_NOT_SUPPORTED is returned if one of these
        ///  values is used.<para/>
        ///  The following values are not supported
        ///  on windows_2000_server: NETLOGON_CONTROL_TC_VERIFY (0x0000000A) NETLOGON_CONTROL_FORCE_DNS_REG
        ///  (0x0000000B) NETLOGON_CONTROL_QUERY_DNS_REG (0x0000000C).<para/>
        ///  The error ERROR_NOT_SUPPORTED is returned if one of these
        ///  values is used.<para/>
        ///  The following values are not supported
        ///  on windows_7 or windows_server_7: NETLOGON_CONTROL_REPLICATE
        ///  (0x00000002) NETLOGON_CONTROL_SYNCHRONIZE (0x00000003) NETLOGON_CONTROL_PDC_REPLICATE
        ///  (0x00000004) NETLOGON_CONTROL_BACKUP_CHANGE_LOG (0x0000FFFC).<para/>
        ///  The error ERROR_NOT_SUPPORTED is returned if one of these
        ///  values is used.
        /// </param>
        /// <param name="queryLevel">
        ///  Information query level requested by the client. The
        ///  buffer returned in the Buffer parameter contains one
        ///  of the following structures, based on the value of
        ///  this field.
        /// </param>
        /// <param name="data">
        ///  NETLOGON_CONTROL_DATA_INFORMATION structure, 
        ///  that contains specific data required by
        ///  the query.
        /// </param>
        /// <param name="buffer">
        ///  NETLOGON_CONTROL_QUERY_INFORMATION structure, 
        ///  that contains the specific query results,
        ///  with a level of verbosity as specified in QueryLevel.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NetApiStatus NetrLogonControl2Ex(
            string serverName,
            FunctionCode_Values functionCode,
            QueryLevel_Values queryLevel,
            _NETLOGON_CONTROL_DATA_INFORMATION? data,
            out _NETLOGON_CONTROL_QUERY_INFORMATION? buffer)
        {
            return rpc.NetrLogonControl2Ex(
                serverName,
                functionCode,
                queryLevel,
                data,
                out buffer);
        }


        /// <summary>
        ///  The NetrEnumerateTrustedDomains method returns a set
        ///  of trusted domain names. Opnum: 19 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="domainNameBuffer">
        ///  A pointer to a DOMAIN_NAME_BUFFER structure, 
        ///  that contains a list of trusted domain
        ///  names.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NtStatus NetrEnumerateTrustedDomains(
            string serverName,
            out _DOMAIN_NAME_BUFFER? domainNameBuffer)
        {
            return rpc.NetrEnumerateTrustedDomains(
                serverName,
                out domainNameBuffer);
        }


        /// <summary>
        ///  The DsrGetDcName method Supported in windows_2000, windows_xp,
        ///  windows_server_2003, windows_vista, and windows_server_2008,
        ///  windows_7, and windows_server_7. is a predecessor to
        ///  the DsrGetDcNameEx2 method. The method returns
        ///  information about a domain controller in the specified
        ///  domain. All parameters of this method have the same
        ///  meanings as the identically named parameters of the
        ///  DsrGetDcNameEx2 method, except for the SiteGuid parameter,
        ///  detailed as follows. Opnum: 20 
        /// </summary>
        /// <param name="computerName">
        ///  ComputerName parameter.
        /// </param>
        /// <param name="domainName">
        ///  DomainName parameter.
        /// </param>
        /// <param name="domainGuid">
        ///  DomainGuid parameter.
        /// </param>
        /// <param name="siteGuid">
        ///  This parameter MUST be NULL and ignored upon receipt.
        /// </param>
        /// <param name="flags">
        ///  Flags parameter.
        /// </param>
        /// <param name="domainControllerInfo">
        ///  DomainControllerInfo parameter.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NetApiStatus DsrGetDcName(
            string computerName,
            string domainName,
            Guid? domainGuid,
            Guid? siteGuid,
            NrpcDsrGetDcNameFlags flags,
            out _DOMAIN_CONTROLLER_INFOW? domainControllerInfo)
        {
            return rpc.DsrGetDcName(
                computerName,
                domainName,
                domainGuid,
                siteGuid,
                (uint)flags,
                out domainControllerInfo);
        }


        /// <summary>
        ///  The NetrLogonGetCapabilities method is used by clients 
        ///  to confirm the server capabilities after a secure channel 
        ///  has been established. Opnum: 21 
        /// </summary>
        /// <param name="serverName">
        ///  A LOGONSRV_HANDLE Unicode string handle of the server
        ///  that is handling the request.
        /// </param>
        /// <param name="computerName">
        ///  A string that contains the name of the computer.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure that
        ///  contains the client authenticator.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure that
        ///  contains the server return authenticator.
        /// </param>
        /// <param name="queryLevel">
        ///  Specifies the level of information to return from the
        ///  domain controller being queried. A value of 0x00000001
        ///  causes a NETLOGON_DOMAIN_INFO structure that contains
        ///  information about the DC to be returned.
        /// </param>
        /// <param name="serverCapabilities">
        ///  A pointer to a 32-bit set of bit flags that identify 
        ///  the server's capabilities.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NtStatus NetrLogonGetCapabilities(
            string serverName,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            ref _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            uint queryLevel,
            out _NETLOGON_CAPABILITIES? serverCapabilities)
        {
            context.PrimaryName = serverName;
            context.ClientComputerName = computerName;

            NtStatus status = rpc.NetrLogonGetCapabilities(
                serverName,
                computerName,
                authenticator,
                ref returnAuthenticator,
                queryLevel,
                out serverCapabilities);

            //ConnectionStatus: A 4-byte value that contains the most recent 
            //connection status return value (section 3.4.5.3.1) last returned 
            //during secure channel establishment or by a method requiring 
            //session key establishment (section 3.1.4.6).
            context.ConnectionStatus = status;
            if (status == NtStatus.STATUS_SUCCESS
                && queryLevel == 1
                && serverCapabilities != null)
            {
                context.NegotiateFlags = (NrpcNegotiateFlags)serverCapabilities.Value.ServerCapabilities;
            }

            return status;
        }


        /// <summary>
        ///  The NetrLogonSetServiceBitsSupported in windows_2000_server,
        ///  windows_xp, windows_server_2003. This method is used to
        ///  notify Netlogon whether a domain controller is running
        ///  specified services.
        ///  Opnum: 22 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle,
        ///  representing the connection to a DC.
        /// </param>
        /// <param name="serviceBitsOfInterest">
        ///  A set of bit flags used as a mask to indicate which
        ///  service's state (running or not running) is being set
        ///  by this call. The value is constructed from zero or
        ///  more bit flags from the following table.
        /// </param>
        /// <param name="serviceBits">
        ///  A set of bit flags used as a mask to indicate whether
        ///  the service indicated by ServiceBitsOfInterest is running
        ///  or not. If the flag is set to 0, the corresponding
        ///  service indicated by ServiceBitsOfInterest is not running.
        ///  Otherwise, if the flag is set to 1, the corresponding
        ///  service indicated by ServiceBitsOfInterest is running.
        ///  The value is constructed from zero or more bit flags
        ///  from the following table.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NtStatus NetrLogonSetServiceBits(
            string serverName,
            uint serviceBitsOfInterest,
            uint serviceBits)
        {
            return rpc.NetrLogonSetServiceBits(
                serverName,
                serviceBitsOfInterest,
                serviceBits);
        }


        /// <summary>
        ///  The NetrLogonGetTrustRid method Supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7.windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. is used to obtain
        ///  the RID of the account whose password is used by domain
        ///  controllers in the specified domain for establishing
        ///  the secure channel from the server receiving this call.
        ///  Opnum: 23 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle.
        ///  ServerName SHOULD be NULL. In windows_server_2008
        ///  and windows_server_7, ServerName is NULL because this
        ///  method is restricted to local callers.
        /// </param>
        /// <param name="domainName">
        ///  The null-terminated Unicode string that contains the
        ///  DNS or NetBIOS name of the primary or trusted domain.
        ///  If this parameter is NULL, this method uses the name
        ///  of the primary domain of the server.
        /// </param>
        /// <param name="rid">
        ///  A pointer to an unsigned long that receives the RID
        ///  of the account.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NetApiStatus NetrLogonGetTrustRid(
            string serverName,
            string domainName,
            out uint? rid)
        {
            return rpc.NetrLogonGetTrustRid(
                serverName,
                domainName,
                out rid);
        }


        /// <summary>
        ///  The NetrLogonComputeServerDigest methodSupported in
        ///  windows_2000_server, windows_xp, windows_server_2003,
        ///  windows_vista, windows_server_2008, windows_7, and
        ///  windows_server_7. computes a cryptographic digest of
        ///  a message by using the MD5 message-digest algorithm,
        ///  as specified in [RFC1321]. This method is called by
        ///  a client computer against a server and is used to compute
        ///  a message digest. The
        ///  client MAY then call the NetrLogonComputeClientDigest
        ///  method and compare the digests
        ///  to ensure that the server that it communicates with
        ///  knows the shared secret between the client machine
        ///  and the domain. Opnum: 24 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="rid">
        ///  The RID of the machine account for which the digest
        ///  is to be computed. The NetrLogonGetTrustRid method 
        ///  is used to obtain the RID.
        /// </param>
        /// <param name="message">
        ///  A pointer to buffer that contains the message to compute
        ///  the digest.
        /// </param>
        /// <param name="messageSize">
        ///  The length of the data referenced by the Message parameter,
        ///  in bytes.
        /// </param>
        /// <param name="newMessageDigest">
        ///  A 128-bit MD5 digest of the current machine account
        ///  password and the message in the Message buffer. The
        ///  machine account is identified by the Rid parameter.
        /// </param>
        /// <param name="oldMessageDigest">
        ///  A 128-bit MD5 digest of the previous machine account
        ///  password, if present, and the message in the Message
        ///  buffer. If no previous machine account password exists,
        ///  then the current password is used. The machine account
        ///  is identified by the Rid parameter.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NetApiStatus NetrLogonComputeServerDigest(
            string serverName,
            uint rid,
            byte[] message,
            uint messageSize,
            out byte[] newMessageDigest,
            out byte[] oldMessageDigest)
        {
            return rpc.NetrLogonComputeServerDigest(
                serverName,
                rid,
                message,
                messageSize,
                out newMessageDigest,
                out oldMessageDigest);
        }


        /// <summary>
        ///  The NetrLogonComputeClientDigest methodSupported in
        ///  windows_2000_server, windows_xp, windows_server_2003,
        ///  windows_vista, windows_server_2008, windows_7, and
        ///  windows_server_7. is used by a client to compute a
        ///  cryptographic digest of a message by using the MD5
        ///  message-digest algorithm, as specified in [RFC1321].
        ///  This method is called by a client to compute a message
        ///  digest. The client SHOULD
        ///  use this digest to compare against one that is returned
        ///  by a call to NetrLogonComputeServerDigest. This comparison
        ///  allows the client to ensure that the server that it
        ///  communicates with knows the shared secret between the
        ///  client machine and the domain. Opnum: 25 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="domainName">
        ///  A pointer to a null-terminated Unicode string that contains
        ///  the DNS or NetBIOS name of the trusted domain. If this
        ///  parameter is NULL, the domain of which the client computer
        ///  is a member is used.
        /// </param>
        /// <param name="message">
        ///  A pointer to a buffer that contains the message for
        ///  which the digest is to be computed.
        /// </param>
        /// <param name="messageSize">
        ///  The length, in bytes, of the Message parameter.
        /// </param>
        /// <param name="newMessageDigest">
        ///  A 128-bit MD5 digest of the current computer account
        ///  password and the message in the Message buffer.
        /// </param>
        /// <param name="oldMessageDigest">
        ///  A 128-bit MD5 digest of the previous machine account
        ///  password and the message in the Message buffer. If
        ///  no previous computer account password exists, the current
        ///  password is used.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NetApiStatus NetrLogonComputeClientDigest(
            string serverName,
            string domainName,
            byte[] message,
            uint messageSize,
            out byte[] newMessageDigest,
            out byte[] oldMessageDigest)
        {
            return rpc.NetrLogonComputeClientDigest(
                serverName,
                domainName,
                message,
                messageSize,
                out newMessageDigest,
                out oldMessageDigest);
        }


        /// <summary>
        ///  The NetrServerAuthenticate3 method mutually authenticates
        ///  the client and the server and establishes the session
        ///  key to be used for the secure channel message protection
        ///  between the client and the server. Supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. It is called after
        ///  the NetrServerReqChallenge method.
        ///  Opnum: 26 
        /// </summary>
        /// <param name="primaryName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="accountName">
        ///  A null-terminated Unicode string that identifies the
        ///  name of the account that contains the secret key (password)
        ///  that is shared between the client and the server. 
        ///  In windows, all machine account
        ///  names are the name of the machine with a $ (dollar
        ///  sign) appended. If there is a period at the end of
        ///  the account name, it is ignored during processing.
        /// </param>
        /// <param name="secureChannelType">
        ///  A NETLOGON_SECURE_CHANNEL_TYPE enumerated value, 
        ///  that indicates the type of the
        ///  secure channel being established by this call.
        /// </param>
        /// <param name="computerName">
        ///  A null-terminated Unicode string that contains the NetBIOS
        ///  name of the client computer calling this method.
        /// </param>
        /// <param name="clientCredential">
        ///  A pointer to a NETLOGON_CREDENTIAL structure, 
        ///  that contains the supplied client credentials.
        /// </param>
        /// <param name="serverCredential">
        ///  A pointer to a NETLOGON_CREDENTIAL structure, 
        ///  that contains the returned server credentials.
        /// </param>
        /// <param name="negotiateFlags">
        ///  A pointer to a 32-bit set of bit flags that indicate
        ///  features supported. As input, the set of flags are
        ///  those requested by the client and SHOULD be the same
        ///  as ClientCapabilities. As output, they are the bit-wise
        ///  AND of the client's requested capabilities and the
        ///  server's ServerCapabilities.
        /// </param>
        /// <param name="accountRid">
        ///  A pointer that receives the RID of the account specified
        ///  by the AccountName parameter. ([MS-ADTS]
        ///  describes how this RID is assigned at account creation
        ///  time.) This value is stored in the AccountRid ADM element
        ///  within the ClientSessionInfo table.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NtStatus NetrServerAuthenticate3(
            string primaryName,
            string accountName,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            string computerName,
            _NETLOGON_CREDENTIAL? clientCredential,
            out _NETLOGON_CREDENTIAL? serverCredential,
            ref NrpcNegotiateFlags? negotiateFlags,
            out uint? accountRid)
        {
            context.PrimaryName = primaryName;
            context.AccountName = accountName;
            context.SecureChannelType = secureChannelType;
            context.ClientComputerName = computerName;
            if (clientCredential != null)
            {
                context.StoredCredential = clientCredential.Value.data;
            }
            if (negotiateFlags != null)
            {
                context.NegotiateFlags = (NrpcNegotiateFlags)negotiateFlags.Value;
            }

            uint? flags = (uint?)negotiateFlags;

            NtStatus status = rpc.NetrServerAuthenticate3(
                primaryName,
                accountName,
                secureChannelType,
                computerName,
                clientCredential,
                out serverCredential,
                ref flags,
                out accountRid);

            context.ConnectionStatus = status;
            if (status == NtStatus.STATUS_SUCCESS)
            {
                negotiateFlags = (NrpcNegotiateFlags?)flags;
                if (negotiateFlags != null)
                {
                    context.NegotiateFlags = (NrpcNegotiateFlags)negotiateFlags.Value;
                }
                if (accountRid != null)
                {
                    context.AccountRid = accountRid.Value;
                }
                if (context.SessionKey == null
                    && context.SharedSecret != null
                    && context.ClientChallenge != null
                    && context.ServerChallenge != null)
                {
                    if ((context.NegotiateFlags & NrpcNegotiateFlags.SupportsAESAndSHA2) != 0)
                    {
                        context.SessionKey = NrpcUtility.ComputeSessionKey(
                            NrpcComputeSessionKeyAlgorithm.HMACSHA256,
                            context.SharedSecret,
                            context.ClientChallenge,
                            context.ServerChallenge);
                    }
                    else if ((context.NegotiateFlags & NrpcNegotiateFlags.SupportsStrongKeys) != 0)
                    {
                        context.SessionKey = NrpcUtility.ComputeSessionKey(
                            NrpcComputeSessionKeyAlgorithm.MD5,
                            context.SharedSecret,
                            context.ClientChallenge,
                            context.ServerChallenge);
                    }
                    else
                    {
                        context.SessionKey = NrpcUtility.ComputeSessionKey(
                            NrpcComputeSessionKeyAlgorithm.DES,
                            context.SharedSecret,
                            context.ClientChallenge,
                            context.ServerChallenge);
                    }
                }
            }

            return status;
        }


        /// <summary>
        ///  The DsrGetDcNameEx method Supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. is a predecessor to
        ///  the DsrGetDcNameEx2 method. The method returns information
        ///  about a domain controller in the specified domain and
        ///  site. All parameters of this method have the same meanings
        ///  as the identically named parameters of the DsrGetDcNameEx2
        ///  method. Opnum: 27 
        /// </summary>
        /// <param name="computerName">
        ///  ComputerName parameter.
        /// </param>
        /// <param name="domainName">
        ///  DomainName parameter.
        /// </param>
        /// <param name="domainGuid">
        ///  DomainGuid parameter.
        /// </param>
        /// <param name="siteName">
        ///  SiteName parameter.
        /// </param>
        /// <param name="flags">
        ///  Flags parameter.
        /// </param>
        /// <param name="domainControllerInfo">
        ///  DomainControllerInfo parameter.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NetApiStatus DsrGetDcNameEx(
            string computerName,
            string domainName,
            Guid? domainGuid,
            string siteName,
            NrpcDsrGetDcNameFlags flags,
            out _DOMAIN_CONTROLLER_INFOW? domainControllerInfo)
        {
            return rpc.DsrGetDcNameEx(
                computerName,
                domainName,
                domainGuid,
                siteName,
                (uint)flags,
                out domainControllerInfo);
        }


        /// <summary>
        ///  The DsrGetSiteName method Supported in windows_2000,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. returns the site name
        ///  for the specified computer that receives this call.
        ///  Opnum: 28 
        /// </summary>
        /// <param name="computerName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="siteName">
        ///  A null-terminated Unicode string that contains the name
        ///  of the site in which the computer that receives this
        ///  call resides.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NetApiStatus DsrGetSiteName(
            string computerName,
            out string siteName)
        {
            return rpc.DsrGetSiteName(
                computerName,
                out siteName);
        }


        /// <summary>
        ///  The NetrLogonGetDomainInfo method Supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, and
        ///  windows_server_2008, windows_7, and windows_server_7.
        ///  returns information that describes the current domain
        ///  to which the specified client belongs. Opnum: 29 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="computerName">
        ///  The null-terminated Unicode string that contains the
        ///  name of the client computer issuing the request.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, 
        ///  that contains the client authenticator.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, 
        ///  that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="level">
        ///  The information level requested by the client. The buffer
        ///  contains one of the following structures, based on
        ///  the value of this field.
        /// </param>
        /// <param name="wkstaBuffer">
        ///  A pointer to a NETLOGON_WORKSTATION_INFORMATION structure,
        ///  that contains information
        ///  about the client workstation.
        /// </param>
        /// <param name="domBuffer">
        ///  A pointer to a NETLOGON_DOMAIN_INFORMATION structure,
        ///  that contains information
        ///  about the domain or policy information.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NtStatus NetrLogonGetDomainInfo(
            string serverName,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            ref _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            Level_Values level,
            _NETLOGON_WORKSTATION_INFORMATION? wkstaBuffer,
            out _NETLOGON_DOMAIN_INFORMATION? domBuffer)
        {
            NtStatus status = rpc.NetrLogonGetDomainInfo(
                serverName,
                computerName,
                authenticator,
                ref returnAuthenticator,
                level,
                wkstaBuffer,
                out domBuffer);

            context.ConnectionStatus = status;
            return status;
        }


        /// <summary>
        ///  The NetrServerPasswordSet2 method Supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. allows the client
        ///  to set a new clear text password for an account used
        ///  by the domain controller 
        ///  for setting up the secure channel from the client. A
        ///  domain member uses this function to periodically change
        ///  its machine account password. A PDC uses this function
        ///  to periodically change the trust password for all directly
        ///  trusted domains. By default, the period is 30 days
        ///  in windows_2000_server, windows_xp, windows_server_2003,
        ///  windows_vista, windows_server_2008, windows_7, and
        ///  windows_server_7. Opnum: 30 
        /// </summary>
        /// <param name="primaryName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="accountName">
        ///  The null-terminated Unicode string that contains the
        ///  name of the account whose password is being changed. In
        ///  windows, all machine account names are the name of
        ///  the machine with a $ (dollar sign) appended.
        /// </param>
        /// <param name="secureChannelType">
        ///  An enumerated value that describes the secure channel
        ///  to be used for authentication.
        /// </param>
        /// <param name="computerName">
        ///  The null-terminated Unicode string that contains the
        ///  NetBIOS name of the computer making the request.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, 
        ///  that contains the encrypted
        ///  logon credential and a time stamp.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, 
        ///  that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="clearNewPassword">
        ///  A pointer to an NL_TRUST_PASSWORD structure, 
        ///  that contains the new password encrypted
        ///  as specified in Calling NetrServerPasswordSet2.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NtStatus NetrServerPasswordSet2(
            string primaryName,
            string accountName,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            out _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            _NL_TRUST_PASSWORD? clearNewPassword)
        {
            NtStatus status = rpc.NetrServerPasswordSet2(
                primaryName,
                accountName,
                secureChannelType,
                computerName,
                authenticator,
                out returnAuthenticator,
                clearNewPassword);

            context.ConnectionStatus = status;
            return status;
        }


        /// <summary>
        ///  The NetrServerPasswordGet method Supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. allows a domain controller
        ///  to get a machine account password from the DC with
        ///  the PDC role in the domain. Opnum: 31 
        /// </summary>
        /// <param name="primaryName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="accountName">
        ///  A null-terminated Unicode string that contains the name
        ///  of the account to retrieve the password for. For machine
        ///  accounts, the account name is the machine name appended
        ///  with a "$" character.
        /// </param>
        /// <param name="accountType">
        ///  A NETLOGON_SECURE_CHANNEL_TYPE enumerated value, 
        ///  that describes the secure channel
        ///  to be used for authentication.
        /// </param>
        /// <param name="computerName">
        ///  A null-terminated Unicode string that contains the NetBIOS
        ///  name of the BDC making the call.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, 
        ///  that contains the encrypted
        ///  logon credential and a time stamp.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, 
        ///  that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="encryptedNtOwfPassword">
        ///  A pointer to an ENCRYPTED_NT_OWF_PASSWORD structure,
        ///  as specified in [MS-SAMR], that contains the
        ///  OWF password of the account.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NtStatus NetrServerPasswordGet(
            string primaryName,
            string accountName,
            _NETLOGON_SECURE_CHANNEL_TYPE accountType,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            out _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            out _NT_OWF_PASSWORD? encryptedNtOwfPassword)
        {
            NtStatus status = rpc.NetrServerPasswordGet(
                primaryName,
                accountName,
                accountType,
                computerName,
                authenticator,
                out returnAuthenticator,
                out encryptedNtOwfPassword);

            context.ConnectionStatus = status;
            return status;
        }


        /// <summary>
        ///  The NetrLogonSendToSamSupported in windows_2000_server,
        ///  windows_xp, windows_server_2003, and windows_server_2008windows_server_2008,
        ///  windows_7, and windows_server_7. method allows a BDC
        ///  to forward user account password changes to the PDC.
        ///  It is used by the client to deliver an opaque buffer
        ///  to the SAM database ([MS-SAMR]) on the server side.
        ///  Opnum: 32 
        /// </summary>
        /// <param name="primaryName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="computerName">
        ///  A null-terminated Unicode string that contains the NetBIOS
        ///  name of the client computer making the call.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, 
        ///  that contains the client authenticator.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, 
        ///  that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="opaqueBuffer">
        ///  A buffer to be passed to the Security Account Manager
        ///  (SAM) service on the PDC. The buffer is encrypted on
        ///  the wire.
        /// </param>
        /// <param name="opaqueBufferSize">
        ///  The size, in bytes, of the OpaqueBuffer parameter.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NtStatus NetrLogonSendToSam(
            string primaryName,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            out _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            byte[] opaqueBuffer,
            uint opaqueBufferSize)
        {
            NtStatus status = rpc.NetrLogonSendToSam(
                primaryName,
                computerName,
                authenticator,
                out returnAuthenticator,
                opaqueBuffer,
                opaqueBufferSize);

            context.ConnectionStatus = status;
            return status;
        }


        /// <summary>
        ///  The DsrAddressToSiteNamesW method Supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista,  windows_server_2008,
        ///  windows_7, and windows_server_7. translates a list
        ///  of socket addresses into their corresponding site names.
        ///  For information about the mapping from socket address
        ///  to subnet/site name.
        ///  Opnum: 33 
        /// </summary>
        /// <param name="computerName">
        ///  The custom RPC binding handle that represents
        ///  the connection to a domain controller.
        /// </param>
        /// <param name="entryCount">
        ///  The number of socket addresses specified in SocketAddresses.
        ///  The maximum value for EntryCount is 32000.To avoid
        ///  large memory allocations, the number of 32,000 was
        ///  chosen as a reasonable limit for the maximum number
        ///  of socket addresses that this method accepts.
        /// </param>
        /// <param name="socketAddresses">
        ///  An array of NL_SOCKET_ADDRESS structures 
        ///  that contains socket addresses to translate. The number
        ///  of addresses specified MUST be equal to EntryCount.
        /// </param>
        /// <param name="siteNames">
        ///  A pointer to an NL_SITE_NAME_ARRAY structure 
        ///  that contains a corresponding array of site names.
        ///  The number of entries returned is equal to EntryCount.
        ///  An entry is returned as NULL if the corresponding socket
        ///  address does not map to any site, or if the address
        ///  family of the socket address is not IPV4 or IPV6. The
        ///  mapping of IP addresses to sites is specified in [MS-ADTS].
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NetApiStatus DsrAddressToSiteNamesW(
            string computerName,
            uint entryCount,
            _NL_SOCKET_ADDRESS[] socketAddresses,
            out _NL_SITE_NAME_ARRAY? siteNames)
        {
            return rpc.DsrAddressToSiteNamesW(
                computerName,
                entryCount,
                socketAddresses,
                out siteNames);
        }


        /// <summary>
        ///  The DsrGetDcNameEx2 method returns information about
        ///  a domain controller in the specified domain and site. Supported
        ///  in windows_2000_server, windows_xp, windows_server_2003,
        ///  windows_vista, and windows_server_2008, windows_7,
        ///  and windows_server_7. The method will also verify that
        ///  the responding DC database contains an account if AccountName
        ///  is specified. The server that receives this call is
        ///  not required to be a DC. Opnum: 34 
        /// </summary>
        /// <param name="computerName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="accountName">
        ///  A null-terminated Unicode string that contains the name
        ///  of the account that MUST exist and be enabled on the
        ///  DC.
        /// </param>
        /// <param name="allowableAccountControlBits">
        ///  A set of bit flags that list properties of the AccountName
        ///  account. A flag is TRUE (or set) if its value is equal
        ///  to 1. If the flag is set, then the account MUST have
        ///  that property; otherwise, the property is ignored.
        ///  The value is constructed from zero or more bit flags
        ///  from the following table.
        /// </param>
        /// <param name="domainName">
        ///  A null-terminated Unicode string that contains the domain
        ///  name (3).
        /// </param>
        /// <param name="domainGuid">
        ///  A pointer to a GUID structure that specifies the GUID
        ///  of the domain queried. If DomainGuid is not NULL and
        ///  the domain specified by DomainName cannot be found,
        ///  the DC locator attempts to locate a DC in the domain
        ///  that has the GUID specified by DomainGuid. This allows
        ///  renamed domains to be found by their GUID.
        /// </param>
        /// <param name="siteName">
        ///  A null-terminated string that contains the name of the
        ///  site in which the DC MUST be located.
        /// </param>
        /// <param name="flags">
        ///  A set of bit flags that provide additional data that
        ///  is used to process the request. A flag is TRUE (or
        ///  set) if its value is equal to 1. The value is constructed
        ///  from zero or more bit flags from the following table,
        ///  with the exceptions that bits D, E, and H cannot be
        ///  combined; S and R cannot be combined; and N and O cannot
        ///  be combined.
        /// </param>
        /// <param name="domainControllerInfo">
        ///  A pointer to a DOMAIN_CONTROLLER_INFOW structure 
        ///  containing data about the DC.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NetApiStatus DsrGetDcNameEx2(
            string computerName,
            string accountName,
            NrpcAllowableAccountControlBits allowableAccountControlBits,
            string domainName,
            Guid? domainGuid,
            string siteName,
            NrpcDsrGetDcNameFlags flags,
            out _DOMAIN_CONTROLLER_INFOW? domainControllerInfo)
        {
            return rpc.DsrGetDcNameEx2(
                computerName,
                accountName,
                (uint)allowableAccountControlBits,
                domainName,
                domainGuid,
                siteName,
                (uint)flags,
                out domainControllerInfo);
        }


        /// <summary>
        ///  The NetrLogonGetTimeServiceParentDomain methodSupported
        ///  in windows_2000_server, windows_xp and windows_server_2003.
        ///  returns the name of the parent domain of the current
        ///  domain. The domain name returned by this method is
        ///  suitable for passing into the NetrLogonGetTrustRid
        ///  method and NetrLogonComputeClientDigest method. Opnum: 35 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="domainName">
        ///  A pointer to the buffer that receives the null-terminated
        ///  Unicode string that contains the name of the parent
        ///  domain. If the DNSdomain name is available, it is returned
        ///  through this parameter; otherwise, the NetBIOS domain
        ///  name is returned.
        /// </param>
        /// <param name="pdcSameSite">
        ///  A pointer to the buffer that receives the value that
        ///  indicates whether the PDC for the domainDomainName
        ///  is in the same site as the server specified by ServerName.
        ///  This value SHOULD The Netlogon client ignores this value
        ///  if ServerName is not a domain controller.  be ignored
        ///  if ServerName is not a domain controller.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NetApiStatus NetrLogonGetTimeServiceParentDomain(
                 string serverName,
                 out string domainName,
                 out PdcSameSite_Values? pdcSameSite)
        {
            return rpc.NetrLogonGetTimeServiceParentDomain(
                serverName,
                out domainName,
                out pdcSameSite);
        }


        /// <summary>
        ///  The NetrEnumerateTrustedDomainsEx methodSupported in
        ///  windows_2000_server, windows_xp, windows_server_2003,
        ///  windows_vista, windows_server_2008, windows_7, and
        ///  windows_server_7. returns a list of trusted domains
        ///  from a specified server. This method extends NetrEnumerateTrustedDomains
        ///  by returning an array of domains in a more flexible
        ///  DS_DOMAIN_TRUSTSW structure, 
        ///  rather than the array of strings in DOMAIN_NAME_BUFFER
        ///  structure. The array is returned
        ///  as part of the NETLOGON_TRUSTED_DOMAIN_ARRAY structure.
        ///  Opnum: 36 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="domains">
        ///  A pointer to a NETLOGON_TRUSTED_DOMAIN_ARRAY structure,
        ///  that contains an array of
        ///  DS_DOMAIN_TRUSTSW structures, one for each trusted domain.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NetApiStatus NetrEnumerateTrustedDomainsEx(
            string serverName,
            out _NETLOGON_TRUSTED_DOMAIN_ARRAY? domains)
        {
            return rpc.NetrEnumerateTrustedDomainsEx(
                serverName,
                out domains);
        }


        /// <summary>
        ///  The DsrAddressToSiteNamesExW method Supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, windows_server_7. translates a list of socket
        ///  addresses into their corresponding site names and subnet
        ///  names. For information about the mapping from socket
        ///  address to subnet/site name, see [MS-ADTS].
        ///  Opnum: 37 
        /// </summary>
        /// <param name="computerName">
        ///  The custom RPC binding handle that represents
        ///  the connection to a domain controller.
        /// </param>
        /// <param name="entryCount">
        ///  The number of socket addresses specified in SocketAddresses.
        ///  The maximum value for EntryCount is 32000.To avoid
        ///  large memory allocations, the number of 32,000 was
        ///  chosen as a reasonable limit for the maximum number
        ///  of socket addresses that this method accepts.
        /// </param>
        /// <param name="socketAddresses">
        ///  An array of NL_SOCKET_ADDRESS structures 
        ///  that contains socket addresses to translate. The number
        ///  of addresses specified MUST be equal to EntryCount.
        /// </param>
        /// <param name="siteNames">
        ///  A pointer to an NL_SITE_NAME_EX_ARRAY structure 
        ///  that contains an array of site names and an array
        ///  of subnet names that correspond to socket addresses
        ///  in SocketAddresses. The number of entries returned
        ///  is equal to EntryCount. An entry is returned as NULL
        ///  if the corresponding socket address does not map to
        ///  any site, or if the address family of the socket address
        ///  is not IPV4 or IPV6. The mapping of IP addresses to
        ///  sites is specified in [MS-ADTS].
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NetApiStatus DsrAddressToSiteNamesExW(
            string computerName,
            uint entryCount,
            _NL_SOCKET_ADDRESS[] socketAddresses,
            out _NL_SITE_NAME_EX_ARRAY? siteNames)
        {
            return rpc.DsrAddressToSiteNamesExW(
                computerName,
                entryCount,
                socketAddresses,
                out siteNames);
        }


        /// <summary>
        ///  The DsrGetDcSiteCoverageW method Supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. returns a list of
        ///  sites covered by a domain controller. Site coverage
        ///  is detailed in [MS-ADTS]. Opnum: 38 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle that represents
        ///  the connection to a DC.
        /// </param>
        /// <param name="siteNames">
        ///  A pointer to an NL_SITE_NAME_ARRAY structure 
        ///  that contains an array of site name strings.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NetApiStatus DsrGetDcSiteCoverageW(
            string serverName,
            out _NL_SITE_NAME_ARRAY? siteNames)
        {
            return rpc.DsrGetDcSiteCoverageW(
                serverName,
                out siteNames);
        }


        /// <summary>
        ///  The NetrLogonSamLogonEx method Supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. provides an extension
        ///  to NetrLogonSamLogon that accepts an extra flags parameter
        ///  and uses Secure RPC ([MS-RPCE]) instead of
        ///  Netlogon authenticators. This method handles logon
        ///  requests for the SAM accounts and allows for generic
        ///  pass-through authentication.
        ///  For more information about fields and structures
        ///  used by Netlogon pass-through methods.
        ///  Opnum: 39 
        /// </summary>
        /// <param name="contextHandle">
        ///  A primitive RPC handle that identifies a particular
        ///  client/server binding.
        /// </param>
        /// <param name="logonServer">
        ///  The null-terminated Unicode string that contains the
        ///  NetBIOS name of the server that will handle the logon
        ///  request.
        /// </param>
        /// <param name="computerName">
        ///  The null-terminated Unicode string that contains the
        ///  NetBIOS name of the client computer sending the logon
        ///  request.
        /// </param>
        /// <param name="logonLevel">
        ///  A NETLOGON_LOGON_INFO_CLASS structure, 
        ///  that specifies the type of the logon information
        ///  passed in the LogonInformation parameter.
        /// </param>
        /// <param name="logonInformation">
        ///  A pointer to a NETLOGON_LEVEL structure, 
        ///  that describes the logon request information.
        /// </param>
        /// <param name="validationLevel">
        ///  A NETLOGON_VALIDATION_INFO_CLASS enumerated type, 
        ///  that contains the validation
        ///  level requested by the client.
        /// </param>
        /// <param name="validationInformation">
        ///  A pointer to a NETLOGON_VALIDATION structure, 
        ///  that describes the user validation information
        ///  returned to the client. The type of the NETLOGON_VALIDATION
        ///  used is determined by the value of the ValidationLevel
        ///  parameter.
        /// </param>
        /// <param name="authoritative">
        ///  A pointer to a char value that represents a Boolean
        ///  condition. FALSE is indicated by the value 0x00, and
        ///  TRUE SHOULDwindows uses the value 0x01 as the representation
        ///  of TRUE and 0x00 for FALSE. be indicated by the value
        ///  0x01 and MAY also be indicated by any nonzero value.
        ///  This Boolean value indicates whether the validation
        ///  information is final. This field is necessary because
        ///  the request might be forwarded through multiple servers.
        ///  The value TRUE indicates that the validation information
        ///  is final and MUST remain unchanged. The Authoritative
        ///  parameter indicates whether the response to this call
        ///  is final or if the same request can be sent to another
        ///  server. The value SHOULD be set to FALSE if the server
        ///  encounters a transient error, and the client can resend
        ///  the request to another server. If the same request
        ///  is known to fail in all subsequent requests, the server
        ///  SHOULD return TRUE.
        /// </param>
        /// <param name="extraFlags">
        ///  A pointer to a set of bit flags that specify delivery
        ///  settings. A flag is TRUE (or set) if its value is equal
        ///  to 1. Output flags MUST be the same as input. The value
        ///  is constructed from zero or more bit flags from the
        ///  following table.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NtStatus NetrLogonSamLogonEx(
            IntPtr contextHandle,
            string logonServer,
            string computerName,
            _NETLOGON_LOGON_INFO_CLASS logonLevel,
            _NETLOGON_LEVEL? logonInformation,
            _NETLOGON_VALIDATION_INFO_CLASS validationLevel,
            out _NETLOGON_VALIDATION? validationInformation,
            out byte? authoritative,
            ref NrpcNetrLogonSamLogonExtraFlags? extraFlags)
        {
            uint? flags = (uint?)extraFlags;

            NtStatus status = rpc.NetrLogonSamLogonEx(
                contextHandle,
                logonServer,
                computerName,
                logonLevel,
                logonInformation,
                validationLevel,
                out validationInformation,
                out authoritative,
                ref flags);

            context.ConnectionStatus = status;
            extraFlags = (NrpcNetrLogonSamLogonExtraFlags?)flags;
            return status;
        }


        /// <summary>
        ///  The DsrEnumerateDomainTrusts method Supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. returns an enumerated
        ///  list of domain trusts, filtered by a set of flags, from
        ///  the specified server. Opnum: 40 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="flags">
        ///  A set of bit flags that specify properties that MUST
        ///  be true for a domain trust to be part of the returned
        ///  domain name list. A flag is TRUE (or set) if its value
        ///  is equal to 1. Flags MUST contain one or more of the
        ///  following bits.
        /// </param>
        /// <param name="domains">
        ///  A pointer to a NETLOGON_TRUSTED_DOMAIN_ARRAY structure,
        ///  that contains a list of trusted
        ///  domains.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NetApiStatus DsrEnumerateDomainTrusts(
            string serverName,
            NrpcDsrEnumerateDomainTrustsFlags flags,
            out _NETLOGON_TRUSTED_DOMAIN_ARRAY? domains)
        {
            return rpc.DsrEnumerateDomainTrusts(
                serverName,
                (uint)flags,
                out domains);
        }


        /// <summary>
        ///  The DsrDeregisterDnsHostRecords method Supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. deletes all of the
        ///  DNS SRV records registered by a specified domain controller.
        ///  For the list of SRV records that a domain registers,
        ///  see [MS-ADTS], SRV Records Registered by DC.
        ///  Opnum: 41 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle, 
        ///  that represents the connection to the DC.
        /// </param>
        /// <param name="dnsDomainName">
        ///  A null-terminated Unicode string that specifies the
        ///  fully qualified domain name (FQDN) (2).
        /// </param>
        /// <param name="domainGuid">
        ///  A pointer to the domainGUID. If the value is not NULL,
        ///  the DNS SRV record of type _ldap._tcp.DomainGuid.domains._msdcs.DnsDomainName
        ///  is also deregistered.
        /// </param>
        /// <param name="dsaGuid">
        ///  A pointer to the objectGUID of the DC's TDSDSA object.
        ///  For information about the TDSDSA object, see [MS-ADTS]. 
        ///  If the value is not NULL, the CNAME [RFC1035]
        ///  record of the domain in the form of DsaGuid._msdcs.DnsDomainName
        ///  is also deregistered.
        /// </param>
        /// <param name="dnsHostName">
        ///  A null-terminated Unicode string that specifies the
        ///  fully qualified domain name (FQDN) (1) of the DC whose
        ///  records are being deregistered. If the value is NULL,
        ///  ERROR_INVALID_PARAMETER is returned.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NetApiStatus DsrDeregisterDnsHostRecords(
            string serverName,
            string dnsDomainName,
            Guid? domainGuid,
            Guid? dsaGuid,
            string dnsHostName)
        {
            return rpc.DsrDeregisterDnsHostRecords(
                serverName,
                dnsDomainName,
                domainGuid,
                dsaGuid,
                dnsHostName);
        }


        /// <summary>
        ///  The NetrServerTrustPasswordsGet method Supported in windows_2000_server_sp4,
        ///  windows_xp, and windows_server_2003, windows_vista,
        ///  windows_server_2008, windows_7, and windows_server_7.
        ///  returns the encrypted current and previous passwords
        ///  for an account in the domain. This method is called
        ///  by a client to retrieve the current and previous account
        ///  passwords from a domain controller. The account name
        ///  requested MUST be the name used when the secure channel
        ///  was created, unless the method is called on a PDC by
        ///  a DC, in which case it can be any valid account name.
        ///  Opnum: 42 
        /// </summary>
        /// <param name="trustedDcName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="accountName">
        ///  The null-terminated Unicode string that contains the
        ///  name of the client account in the domain for which
        ///  the trust password MUST be returned. In windows, all
        ///  machine account names are the name of the machine with
        ///  a $ (dollar sign) appended.
        /// </param>
        /// <param name="secureChannelType">
        ///  A NETLOGON_SECURE_CHANNEL_TYPE enumerated value, 
        ///  that indicates the type of the
        ///  secure channel being established by this call.
        /// </param>
        /// <param name="computerName">
        ///  The null-terminated Unicode string that contains the
        ///  NetBIOS name of the client computer.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, 
        ///  that contains the client authenticator.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, 
        ///  that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="encryptedNewOwfPassword">
        ///  A pointer to an ENCRYPTED_NT_OWF_PASSWORD structure,
        ///  that contains the NTOWFv1
        ///  (as specified in NTLM v1 Authentication in [MS-NLMP])
        ///  of the current password, encrypted as specified
        ///  in [MS-SAMR], Encrypting an NT Hash or LM
        ///  Hash Value with a Specified Key. The session key is
        ///  the specified 16-byte key that is used to derive the
        ///  password's keys. The specified 16-byte key uses the
        ///  16-byte value process, as specified in [MS-SAMR].
        /// </param>
        /// <param name="encryptedOldOwfPassword">
        ///  A pointer to an ENCRYPTED_NT_OWF_PASSWORD structure,
        ///  that contains the NTOWFv1
        ///  (as specified in NTLM v1 Authentication in [MS-NLMP])
        ///  of the previous password, encrypted as specified
        ///  in [MS-SAMR], Encrypting an NT Hash or LM
        ///  Hash Value with a Specified Key. The session key is
        ///  the specified 16-byte key that is used to derive the
        ///  password's keys. The specified 16-byte key uses the
        ///  16-byte value process, as specified in [MS-SAMR].
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NtStatus NetrServerTrustPasswordsGet(
            string trustedDcName,
            string accountName,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            out _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            out _NT_OWF_PASSWORD? encryptedNewOwfPassword,
            out _NT_OWF_PASSWORD? encryptedOldOwfPassword)
        {
            NtStatus status = rpc.NetrServerTrustPasswordsGet(
                trustedDcName,
                accountName,
                secureChannelType,
                computerName,
                authenticator,
                out returnAuthenticator,
                out encryptedNewOwfPassword,
                out encryptedOldOwfPassword);

            context.ConnectionStatus = status;
            return status;
        }


        /// <summary>
        ///  The DsrGetForestTrustInformation methodSupported in
        ///  windows_xpwindows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. retrieves the trust
        ///  information for the forest of the specified domain
        ///  controller, or for a forest trusted by the forest of
        ///  the specified DC. Opnum: 43 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="trustedDomainName">
        ///  The optional null-terminated Unicode string that contains
        ///  the DNS or NetBIOS name of the trusted domain for which
        ///  the forest trust information is to be gathered.
        /// </param>
        /// <param name="flags">
        ///  A set of bit flags that specify additional applications
        ///  for the forest trust information. A flag is TRUE (or
        ///  set) if its value is equal to 1.
        /// </param>
        /// <param name="forestTrustInfo">
        ///  A pointer to an LSA_FOREST_TRUST_INFORMATION structure,
        ///  as specified in [MS-LSAD], that contains data
        ///  for each foresttrust.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NetApiStatus DsrGetForestTrustInformation(
            string serverName,
            string trustedDomainName,
            NrpcDsrGetForestTrustInformationFlags flags,
            out _LSA_FOREST_TRUST_INFORMATION? forestTrustInfo)
        {
            return rpc.DsrGetForestTrustInformation(
                serverName,
                trustedDomainName,
                (uint)flags,
                out forestTrustInfo);
        }


        /// <summary>
        ///  The NetrGetForestTrustInformationSupported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, and
        ///  windows_server_2008, windows_7, and windows_server_7.
        ///  method retrieves the trust information for the forest
        ///  of which the member's domain is itself a member. Opnum: 44 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="computerName">
        ///  The null-terminated Unicode string that contains the
        ///  client computer NetBIOS name.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, 
        ///  that contains the client authenticator.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, 
        ///  that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="flags">
        ///  MUST be set to zero and MUST be ignored on receipt.
        /// </param>
        /// <param name="forestTrustInfo">
        ///  A pointer to an LSA_FOREST_TRUST_INFORMATION structure,
        ///  as specified in [MS-LSAD], that contains data
        ///  for each foresttrust.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NtStatus NetrGetForestTrustInformation(
            string serverName,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            out _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            uint flags,
            out _LSA_FOREST_TRUST_INFORMATION? forestTrustInfo)
        {
            return rpc.NetrGetForestTrustInformation(
                serverName,
                computerName,
                authenticator,
                out returnAuthenticator,
                flags,
                out forestTrustInfo);
        }


        /// <summary>
        ///  The NetrLogonSamLogonWithFlags method Supported in windows_xpwindows_server_2003,
        ///  windows_vista, windows_server_2008, windows_7, and
        ///  windows_server_7. handles logon requests for the SAM
        ///  accounts. Opnum: 45 
        /// </summary>
        /// <param name="logonServer">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="computerName">
        ///  The Unicode string that contains the NetBIOS name of
        ///  the client computer calling this method.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, 
        ///  that contains the client authenticator.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, 
        ///  that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="logonLevel">
        ///  A NETLOGON_LOGON_INFO_CLASS structure, 
        ///  that specifies the type of logon information
        ///  passed in the LogonInformation parameter.
        /// </param>
        /// <param name="logonInformation">
        ///  A pointer to a NETLOGON_LEVEL structure, 
        ///  that describes the logon request information.
        /// </param>
        /// <param name="validationLevel">
        ///  A NETLOGON_VALIDATION_INFO_CLASS enumerated type, 
        ///  that contains the validation
        ///  level requested by the client.
        /// </param>
        /// <param name="validationInformation">
        ///  A pointer to a NETLOGON_VALIDATION structure, 
        ///  that describes the user validation information
        ///  returned to the client. The type of the NETLOGON_VALIDATION
        ///  used is determined by the value of the ValidationLevel
        ///  parameter.
        /// </param>
        /// <param name="authoritative">
        ///  A pointer to a char value representing a Boolean condition.
        ///  FALSE is indicated by the value 0x00; TRUE SHOULD be
        ///  indicated by the value 0x01 and MAY also be indicated
        ///  by any nonzero value. Windows uses the value of 0x01
        ///  as the representation of TRUE and 0x00 for FALSE. This
        ///  Boolean value indicates whether the validation information
        ///  is final. This field is necessary because the request
        ///  might be forwarded through multiple servers. A value
        ///  of TRUE indicates that the validation information is
        ///  final and MUST remain unchanged.
        /// </param>
        /// <param name="extraFlags">
        ///  A pointer to a set of bit flags that specify delivery
        ///  settings. A flag is TRUE (or set) if its value is equal
        ///  to 1. The value is constructed from zero or more bit
        ///  flags from the following table.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NtStatus NetrLogonSamLogonWithFlags(
            string logonServer,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            ref _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            _NETLOGON_LOGON_INFO_CLASS logonLevel,
            _NETLOGON_LEVEL? logonInformation,
            _NETLOGON_VALIDATION_INFO_CLASS validationLevel,
            out _NETLOGON_VALIDATION? validationInformation,
            out byte? authoritative,
            ref NrpcNetrLogonSamLogonExtraFlags? extraFlags)
        {
            uint? flags = (uint?)extraFlags;

            NtStatus status = rpc.NetrLogonSamLogonWithFlags(
                logonServer,
                computerName,
                authenticator,
                ref returnAuthenticator,
                logonLevel,
                logonInformation,
                validationLevel,
                out validationInformation,
                out authoritative,
                ref flags);

            context.ConnectionStatus = status;
            extraFlags = (NrpcNetrLogonSamLogonExtraFlags?)flags;
            return status;
        }


        /// <summary>
        ///  The NetrServerGetTrustInfo method Supported in windows_xp
        ///  and windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, windows_server_7. returns an information
        ///  block from a specified server. The information includes
        ///  encrypted current and previous passwords for a particular
        ///  account and additional trust data. The account name
        ///  requested MUST be the name used when the secure channel
        ///  was created, unless the method is called on a PDC by
        ///  a domain controller, in which case it can be any valid
        ///  account name. Opnum: 46 
        /// </summary>
        /// <param name="trustedDcName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="accountName">
        ///  The null-terminated Unicode string that contains the
        ///  name of the client account in the domain.
        /// </param>
        /// <param name="secureChannelType">
        ///  A NETLOGON_SECURE_CHANNEL_TYPE enumerated value, 
        ///  that indicates the type of the
        ///  secure channel being established by this call.
        /// </param>
        /// <param name="computerName">
        ///  The null-terminated Unicode string that contains the
        ///  NetBIOS name of the client computer, for which the
        ///  trust information MUST be returned.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, 
        ///  that contains the client authenticator.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, 
        ///  that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="encryptedNewOwfPassword">
        ///  A pointer to an ENCRYPTED_NT_OWF_PASSWORD structure,
        ///  that contains the NTOWFv1
        ///  (as specified in NTLM v1 Authentication in [MS-NLMP])
        ///  of the current password, encrypted as specified
        ///  in [MS-SAMR], Encrypting an NT Hash or LM
        ///  Hash Value with a Specified Key. The session key is
        ///  the specified 16-byte key that is used to derive its
        ///  keys via the 16-byte value process, as specified in
        ///  [MS-SAMR].
        /// </param>
        /// <param name="encryptedOldOwfPassword">
        ///  A pointer to an ENCRYPTED_NT_OWF_PASSWORD structure,
        ///  that contains the NTOWFv1
        ///  (as specified in NTLM v1 Authentication in [MS-NLMP])
        ///  of the old password, encrypted as specified
        ///  in [MS-SAMR], Encrypting an NT Hash or LM
        ///  Hash Value with a Specified Key. The session key is
        ///  the specified 16-byte key that is used to derive its
        ///  keys via the 16-byte value process, as specified in
        ///  [MS-SAMR].
        /// </param>
        /// <param name="trustInfo">
        ///  A pointer to an NL_GENERIC_RPC_DATA structure, 
        ///  that contains a block of generic RPC data
        ///  with trust information for the specified server.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NtStatus NetrServerGetTrustInfo(
            string trustedDcName,
            string accountName,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            out _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            out _NT_OWF_PASSWORD? encryptedNewOwfPassword,
            out _NT_OWF_PASSWORD? encryptedOldOwfPassword,
            out _NL_GENERIC_RPC_DATA? trustInfo)
        {
            NtStatus status = rpc.NetrServerGetTrustInfo(
                trustedDcName,
                accountName,
                secureChannelType,
                computerName,
                authenticator,
                out returnAuthenticator,
                out encryptedNewOwfPassword,
                out encryptedOldOwfPassword,
                out trustInfo);

            context.ConnectionStatus = status;
            return status;
        }


        /// <summary>
        ///  OpnumUnused47 method. Opnum: 47 
        /// </summary>
        /// <param name="contextHandle">
        ///  A primitive RPC handle that identifies a particular
        ///  client/server binding.
        /// </param>
        public NtStatus OpnumUnused47(IntPtr contextHandle)
        {
            return rpc.OpnumUnused47(contextHandle);
        }


        /// <summary>
        /// The DsrUpdateReadOnlyServerDnsRecords method will allow an RODC to send a control 
        /// command to a normal (writable) DC for site-specific and CName types of DNS records 
        /// update. For registration, site-specific records should be for the site in which 
        /// RODC resides. For the types of DNS records, see [MS-ADTS] section 7.3.2. Opnum: 48
        /// </summary>
        /// <param name="ServerName">
        /// The custom RPC binding handle (as specified in section 3.5.5.1) that represents 
        /// the connection to the normal (writable) DC.
        /// </param>
        /// <param name="ComputerName">
        /// A null-terminated Unicode string that contains the client computer NetBIOS name.
        /// </param>
        /// <param name="Authenticator">
        /// A pointer to a NETLOGON_AUTHENTICATOR structure (as specified in section 2.2.1.1.5) 
        /// that contains the client authenticator that will be used to authenticate the client.
        /// </param>
        /// <param name="ReturnAuthenticator">
        /// A pointer to a NETLOGON_AUTHENTICATOR structure that contains the server return 
        /// authenticator.
        /// </param>
        /// <param name="SiteName">
        /// A pointer to a null-terminated Unicode string that contains the site name where 
        /// the RODC resides.
        /// </param>
        /// <param name="DnsTtl">
        /// The Time To Live value, in seconds, for DNS records.
        /// </param>
        /// <param name="DnsNames">
        /// A pointer to an NL_DNS_NAME_INFO_ARRAY (section 2.2.1.2.6) structure that contains 
        /// an array of NL_DNS_NAME_INFO structures.
        /// </param>
        public NtStatus DsrUpdateReadOnlyServerDnsRecords(
            string ServerName,
            string ComputerName,
            _NETLOGON_AUTHENTICATOR? Authenticator,
            out _NETLOGON_AUTHENTICATOR? ReturnAuthenticator,
            string SiteName,
            uint DnsTtl,
            ref _NL_DNS_NAME_INFO_ARRAY? DnsNames)
        {
            NtStatus status = rpc.DsrUpdateReadOnlyServerDnsRecords(
                ServerName,
                ComputerName,
                Authenticator,
                out ReturnAuthenticator,
                SiteName,
                DnsTtl,
                ref DnsNames);

            context.ConnectionStatus = status;
            return status;
        }


        /// <summary>
        /// When an RODC receives either the NetrServerAuthenticate3 method or the 
        /// NetrLogonGetDomainInfo method with updates requested, it invokes this method 
        /// on a normal (writable) DC to update to a client's computer account object in 
        /// Active Directory.
        /// </summary>
        /// <param name="PrimaryName">
        /// The custom RPC binding handle, as specified in section 3.5.5.1.
        /// </param>
        /// <param name="ChainedFromServerName">
        /// The null-terminated Unicode string that contains the name of the read-only 
        /// DC that issues the request.
        /// </param>
        /// <param name="ChainedForClientName">
        /// The null-terminated Unicode string that contains the name of the client 
        /// computer that called NetrServerAuthenticate3 or NetrLogonGetDomainInfo on 
        /// the RODC.
        /// </param>
        /// <param name="Authenticator">
        /// A pointer to a NETLOGON_AUTHENTICATOR structure that contains the client 
        /// authenticator.
        /// </param>
        /// <param name="ReturnAuthenticator">
        /// A pointer to a NETLOGON_AUTHENTICATOR structure that contains the server 
        /// return authenticator.
        /// </param>
        /// <param name="dwInVersion">
        /// One of the NL_IN_CHAIN_SET_CLIENT_ATTRIBUTES union types selected based on 
        /// the value of the pmsgIn field. The value MUST be 1.
        /// </param>
        /// <param name="pmsgIn">
        /// A pointer to an NL_IN_CHAIN_SET_CLIENT_ATTRIBUTES_V1 structure that contains 
        /// the values to update on the client's computer account object in Active 
        /// Directory on the normal (writable) DC.
        /// </param>
        /// <param name="pdwOutVersion">
        /// A pointer to one of the NL_OUT_CHAIN_SET_CLIENT_ATTRIBUTES union types selected 
        /// based on the value of the pmsgIn field. The value MUST be 1.
        /// </param>
        /// <param name="pmsgOut">
        /// A pointer to an NL_OUT_CHAIN_SET_CLIENT_ATTRIBUTES_V1 structure that contains 
        /// information on the client workstation and the writable domain controller. For 
        /// how it is populated by the server, see below.
        /// </param>
        public NtStatus NetrChainSetClientAttributes(
            string PrimaryName,
            string ChainedFromServerName,
            string ChainedForClientName,
            _NETLOGON_AUTHENTICATOR? Authenticator,
            ref _NETLOGON_AUTHENTICATOR? ReturnAuthenticator,
            uint dwInVersion,
            NL_IN_CHAIN_SET_CLIENT_ATTRIBUTES? pmsgIn,
            ref uint? pdwOutVersion,
            ref NL_OUT_CHAIN_SET_CLIENT_ATTRIBUTES? pmsgOut)
        {
            NtStatus status = rpc.NetrChainSetClientAttributes(
                PrimaryName,
                ChainedFromServerName,
                ChainedForClientName,
                Authenticator,
                ref ReturnAuthenticator,
                dwInVersion,
                pmsgIn,
                ref pdwOutVersion,
                ref pmsgOut);

            context.ConnectionStatus = status;
            return status;
        }

        #endregion


        #region NRPC Unbind method

        /// <summary>
        /// RPC unbind.
        /// </summary>
        public void Unbind()
        {
            rpc.Unbind();
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Dispose method.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        static Mutex disposeMutex = new Mutex();

        /// <summary>
        /// Dispose method.
        /// </summary>
        /// <param name="disposing">
        /// True to release both managed and unmanaged resources.<para/>
        /// False to release unmanaged resources only.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Release managed resources.
                disposeMutex.WaitOne();
                if (rpc != null)
                {
                    try
                    {
                        rpc.Unbind();
                    }
                    catch
                    {
                        //Ignore the exception thrown by Unbind.
                    }
                    rpc.Dispose();
                    rpc = null;
                }
                disposeMutex.ReleaseMutex();
            }

            // Release unmanaged resources.
        }


        /// <summary>
        /// finalizer
        /// </summary>
        ~NrpcClient()
        {
            Dispose(false);
        }

        #endregion
    }
}
