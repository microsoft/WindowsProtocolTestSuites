// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp;
using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Spng
{
    public class SpngClientSecurityContext : ClientSecurityContext, IDisposable
    {
        /// <summary>
        /// Dispose status identifier.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Spng client role.
        /// </summary>
        private SpngClient client;

        /// <summary>
        /// Specific security context, e.g. NlmpSecurityContext or KileSecurityContext.
        /// </summary>
        private ClientSecurityContext securityMechContext;

        /// <summary>
        /// Indicate whether the authentication is completed.
        /// </summary>
        private bool needContinueProcessing;

        /// <summary>
        /// SecurityPackage type of SPNG, i.e. Negotiate.
        /// </summary>
        private SecurityPackageType packageType;

        /// <summary>
        /// Spng token.
        /// </summary>
        private byte[] token;

        /// <summary>
        /// Internal security mechanism sequence number.
        /// </summary>
        private uint sequenceNumber;

        /// <summary>
        /// Identifier of whether need to compute MechListMIC
        /// </summary>
        private bool needMechListMic;

        /// <summary>
        /// Security configuration list
        /// </summary>
        private SecurityConfig[] securityConfigList;

        private SecurityConfig CurrentSecurityConfig;

        /// <summary>
        /// The session key.
        /// </summary>
        public override byte[] SessionKey
        {
            get
            {
                if (this.SecurityMechContext != null)
                {
                    return this.SecurityMechContext.SessionKey;
                }
                return null;
            }
        }

        /// <summary>
        /// Specific security context, e.g. NlmpSecurityContext or KileSecurityContext.
        /// </summary>
        public ClientSecurityContext SecurityMechContext
        {
            get
            {
                return this.securityMechContext;
            }
        }

        /// <summary>
        /// Indicate whether the authentication is completed.
        /// </summary>
        public override bool NeedContinueProcessing
        {
            get
            {
                return this.needContinueProcessing;
            }
        }

        /// <summary>
        /// SecurityPackage type of SPNG, i.e. Negotiate.
        /// </summary>
        public override SecurityPackageType PackageType
        {
            get
            {
                return this.packageType;
            }
        }

        /// <summary>
        /// The security token of SPNG.
        /// </summary>
        public override byte[] Token
        {
            get
            {
                return this.token;
            }
        }

        /// <summary>
        /// Gets or sets sequence number for Verify, Encrypt and Decrypt message.
        /// For Digest SSP, it must be 0.
        /// </summary>
        public override uint SequenceNumber
        {
            get
            {
                return this.sequenceNumber;
            }
            set
            {
                this.sequenceNumber = value;
            }
        }

        /// <summary>
        /// Queries the sizes of the structures used in the per-message functions.
        /// </summary>
        public override SecurityPackageContextSizes ContextSizes
        {
            get
            {
                if (this.SecurityMechContext != null)
                {
                    return this.SecurityMechContext.ContextSizes;
                }
                return new SecurityPackageContextSizes();
            }
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="attributes">the attributes for server</param>
        /// <param name="nlmpConfig">
        /// the config for nlmp client, this param maybe null. if null, spng does not support nlmp.
        /// </param>
        /// <param name="kileConfig">
        /// the config for kile client, this param maybe null. if null, spng does not support Kerberos.
        /// </param>
        /// <exception cref="ArgumentException">at least one of nlmpConfig and kileConfig must not be null</exception>
        public SpngClientSecurityContext(
            ClientSecurityContextAttribute attributes,
            NlmpClientSecurityConfig nlmpConfig,
            KerberosClientSecurityConfig kileConfig)
        {
            InitializeClientSecurityContext(attributes, nlmpConfig, kileConfig, null);
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="attributes">the attributes for server</param>
        /// <param name="nlmpConfig">
        /// the config for nlmp client, this param maybe null. if null, spng does not support nlmp.
        /// </param>
        /// <param name="kileConfig">
        /// the config for kile client, this param maybe null. if null, spng does not support Kerberos.
        /// </param>
        /// <param name="sspiConfig">
        /// the config for sspi client, this param maybe null. if null, spng does not support sspi.
        /// </param>
        /// <exception cref="ArgumentException">
        /// at least one of nlmpConfig, kileConfig and sspiConfig must not be null
        /// </exception>
        public SpngClientSecurityContext(
            ClientSecurityContextAttribute attributes,
            NlmpClientSecurityConfig nlmpConfig,
            KerberosClientSecurityConfig kileConfig,
            SspiClientSecurityConfig sspiConfig)
        {
            InitializeClientSecurityContext(attributes, nlmpConfig, kileConfig, sspiConfig);
        }


        /// <summary>
        /// initialize the client security context.
        /// </summary>
        /// <param name="attributes">the attributes for server</param>
        /// <param name="nlmpConfig">
        /// the config for nlmp client, this param maybe null. if null, spng does not support nlmp.
        /// </param>
        /// <param name="kileConfig">
        /// the config for kile client, this param maybe null. if null, spng does not support Kerberos.
        /// </param>
        /// <param name="sspiConfig">
        /// the config for sspi client, this param maybe null. if null, spng does not support sspi.
        /// </param>
        /// <exception cref="ArgumentException">
        /// at least one of nlmpConfig, kileConfig and sspiConfig must not be null
        /// </exception>
        private void InitializeClientSecurityContext(
            ClientSecurityContextAttribute attributes,
            NlmpClientSecurityConfig nlmpConfig,
            KerberosClientSecurityConfig kileConfig,
            SspiClientSecurityConfig sspiConfig)
        {
            if (nlmpConfig == null && kileConfig == null && sspiConfig == null)
            {
                throw new ArgumentException("at least one of nlmpConfig, kileConfig and sspiConfig must not be null");
            }

            List<SecurityConfig> configList = new List<SecurityConfig>();

            // build the mech types
            List<MechType> mechTypes = new List<MechType>();
            if (kileConfig != null)
            {
                configList.Add(kileConfig);
                mechTypes.Add(new MechType(Consts.KerbOidInt));
            }
            if (nlmpConfig != null)
            {
                configList.Add(nlmpConfig);
                mechTypes.Add(new MechType(Consts.NlmpOidInt));
            }
            if (sspiConfig != null)
            {
                configList.Add(sspiConfig);
                switch (sspiConfig.SecurityType)
                {
                    case SecurityPackageType.Ntlm:
                        mechTypes.Add(new MechType(Consts.NlmpOidInt));
                        break;

                    case SecurityPackageType.Kerberos:
                        mechTypes.Add(new MechType(Consts.KerbOidInt));
                        break;

                    default:
                        mechTypes.Add(new MechType(Consts.UnknownOidInt));
                        break;
                }
            }

            // build a spng config
            SpngClientSecurityConfig spngConfig =
                new SpngClientSecurityConfig(attributes, new MechTypeList(mechTypes.ToArray()));

            // add spng config to config list.
            configList.Insert(0, spngConfig);

            // initialize the spng client
            this.client = new SpngClient(spngConfig);
            this.needContinueProcessing = true;
            this.packageType = SecurityPackageType.Negotiate;
            this.securityConfigList = configList.ToArray();
        }


        /// <summary>
        /// Initialize the context from a token, and generate a new token.
        /// </summary>
        /// <param name="inToken">the token from server. "inToken" must be null when invoked first time.</param>
        /// <exception cref="ArgumentNullException">Except invoked at the first time, the "inToken" MUST not be null
        /// when invoking Initialize.</exception>
        /// <exception cref="InvalidOperationException">The internal state is invalid when invoking Initialize.</exception>
        /// <exception cref="InvalidOperationException">Invalid MechListMic</exception>
        public override void Initialize(byte[] inToken)
        {
            try
            {
                switch (this.client.Context.NegotiationState)
                {
                    case SpngNegotiationState.Initial:
                        SpngNegotiationInitial(inToken);
                        break;

                    case SpngNegotiationState.RequestMic:
                        SpngNegotiationRequestMic(inToken);
                        break;

                    case SpngNegotiationState.AcceptIncomplete:
                        SpngNegotiationAcceptIncomplete(inToken);
                        break;

                    case SpngNegotiationState.AcceptCompleted:
                        throw new InvalidOperationException("Authentication completed!");

                    case SpngNegotiationState.SspiNegotiation:
                        if (securityMechContext != null)
                        {
                            securityMechContext.Initialize(inToken);
                        }
                        break;

                    default: // MUST be SpngNegotiationState.Reject
                        throw new InvalidOperationException("Authentication rejected!");
                }
            }
            catch (Exception ex)
            {
                if (securityMechContext is NlmpClientSecurityContext)
                {
                    throw ex;
                }
                SwitchToNTLMSSP(null); // try use NTLM
            }
        }


        #region Sign/Verify/Decrypt/Encrypt
        /// <summary>
        /// Sign data according SecBuffers.
        /// </summary>
        /// <param name="securityBuffers">SecurityBuffer array.</param>
        /// <exception cref="InvalidOperationException">Thrown if this operation is invoked 
        /// before the authentication process completed.</exception>
        /// <exception cref="SspiException">If encrypt fail, this exception will be thrown.</exception>
        public override void Sign(params SecurityBuffer[] securityBuffers)
        {
            if (this.needContinueProcessing)
            {
                throw new InvalidOperationException(
                    "This operation MUST be invoked after the authentication process completed.");
            }
            else
            {
                // Throw SspiException if failed
                securityMechContext.Sign(securityBuffers);
            }
        }


        /// <summary>
        /// Verify the signature containing in the SecurityBuffer.
        /// </summary>
        /// <param name="securityBuffers">SecBuffer array.</param>
        /// <returns>True if the signature matches the signed message, otherwise false.</returns>
        /// <exception cref="InvalidOperationException">Thrown if this operation is invoked 
        /// before the authentication process completed.</exception>
        /// <exception cref="SspiException">If encrypt fail, this exception will be thrown.</exception>
        public override bool Verify(params SecurityBuffer[] securityBuffers)
        {
            if (this.needContinueProcessing)
            {
                throw new InvalidOperationException(
                    "This operation MUST be invoked after the authentication process completed.");
            }
            else
            {
                // Throw SspiException if failed
                return securityMechContext.Verify(securityBuffers);
            }
        }


        /// <summary>
        /// Encrypts Message. User decides what SecBuffers are used.
        /// </summary>
        /// <param name="securityBuffers">SecBuffers.</param>
        /// <exception cref="InvalidOperationException">Thrown if this operation is invoked 
        /// before the authentication process completed.</exception>
        /// <exception cref="SspiException">If encrypt fail, this exception will be thrown.</exception>
        public override void Encrypt(params SecurityBuffer[] securityBuffers)
        {
            if (this.needContinueProcessing)
            {
                throw new InvalidOperationException(
                    "This operation MUST be invoked after the authentication process completed.");
            }
            else
            {
                // Throw SspiException if failed
                securityMechContext.Encrypt(securityBuffers);
            }
        }


        /// <summary>
        /// Decrypt the token containing in the SecurityBuffer. This function is also invoked by DecryptMessage.
        /// </summary>
        /// <param name="securityBuffers">SecBuffer.Encrypted data will be filled in SecBuffers.</param>
        /// <exception cref="InvalidOperationException">Thrown if this operation is invoked 
        /// before the authentication process completed.</exception>
        /// <exception cref="SspiException">If encrypt fail, this exception will be thrown.</exception>
        public override bool Decrypt(params SecurityBuffer[] securityBuffers)
        {
            if (this.needContinueProcessing)
            {
                throw new InvalidOperationException(
                    "This operation MUST be invoked after the authentication process completed.");
            }
            else
            {
                // Throw SspiException if failed
                return securityMechContext.Decrypt(securityBuffers);
            }
        }
        #endregion


        /// <summary>
        /// Initialize the securityMechContext based on the security package type
        /// </summary>
        /// <param name="mechType">security mechanism type</param>
        /// <param name="inToken">the input security token</param>
        /// <exception cref="InvalidOperationException">Thrown if could not find the configuration.</exception>
        /// <exception cref="InvalidOperationException">Thrown when security configuration is unknown</exception>
        private void InitializeSecurityContext(MechType mechType, byte[] inToken)
        {
            SpngClientContext clientContext = this.client.Context as SpngClientContext;
            SecurityPackageType authType = SpngUtility.ConvertMechType(mechType);
            CurrentSecurityConfig = SpngUtility.GetSecurityConfig(this.securityConfigList, authType);

            if (CurrentSecurityConfig == null)
            {
                throw new InvalidOperationException("Missing configuration for " + authType.ToString());
            }

            if (securityMechContext != null)
            {
                // re-enter. Nothing need to do
                return;
            }

            if (CurrentSecurityConfig.GetType() == typeof(KerberosClientSecurityConfig))
            {
                KerberosClientSecurityConfig kileConfig = CurrentSecurityConfig as KerberosClientSecurityConfig;

                securityMechContext = new KerberosClientSecurityContext(
                    kileConfig.ServiceName,
                    kileConfig.ClientCredential,
                    KerberosAccountType.User,
                    kileConfig.KdcIpAddress,
                    kileConfig.KdcPort,
                    kileConfig.TransportType,
                    kileConfig.SecurityAttributes);
            }
            else if (CurrentSecurityConfig.GetType() == typeof(NlmpClientSecurityConfig))
            {
                NlmpClientSecurityConfig nlmpConfig = CurrentSecurityConfig as NlmpClientSecurityConfig;

                NlmpClientCredential cred = new NlmpClientCredential(
                     nlmpConfig.TargetName,
                     nlmpConfig.DomainName,
                     nlmpConfig.AccountName,
                     nlmpConfig.Password);
                securityMechContext = new NlmpClientSecurityContext(cred, nlmpConfig.SecurityAttributes);
            }
            else if (CurrentSecurityConfig.GetType() == typeof(SspiClientSecurityConfig))
            {
                throw new InvalidOperationException("Only support Kerberos security config and NTLM security config");
            }
            else
            {
                throw new InvalidOperationException("unknown security config");
            }
        }

        /// <summary>
        /// Retrieve the internal security token and the supported MechTypeList from InitialNegToken2
        /// </summary>
        /// <param name="inToken">InitialNegToken2 buffer array</param>
        /// <param name="serverMechList">The supported MechTypeList of server side</param>
        /// <returns>The internal security token</returns>
        private byte[] UnwrapInitialNegToken2(byte[] inToken, out MechTypeList serverMechList)
        {
            SpngInitialNegToken2 initNegToken2 = new SpngInitialNegToken2();
            initNegToken2.FromBytes(inToken);

            serverMechList = initNegToken2.MechList;

            if (initNegToken2.MechToken == null || initNegToken2.MechToken.Length == 0)
                return null;

            return initNegToken2.MechToken;
        }

        /// <summary>
        /// Generate an InitialNegToken byte array, and insert the internal security token
        /// </summary>
        /// <param name="inToken">the internal security token</param>
        /// <returns>the generated InitialNegToken byte array</returns>
        private byte[] WrapInitialNegToken(byte[] inToken)
        {
            SpngInitialNegToken initNegToken = client.CreateInitialNegToken(
                SpngPayloadType.NegInit,
                new NegState(NegState.accept_incomplete),
                inToken,
                null);
            return initNegToken.ToBytes();
        }


        /// <summary>
        /// Retrieve the internal security token from NegotiationToken
        /// </summary>
        /// <param name="inToken">NegotiationToken byte array</param>
        /// <returns>the internal security token</returns>
        private byte[] UnwrapNegotiationToken(byte[] inToken)
        {
            byte[] mic = null;

            return UnwrapNegotiationToken(inToken, out mic);
        }


        /// <summary>
        /// Retrieve the internal security token from NegotiationToken
        /// </summary>
        /// <param name="inToken">NegotiationToken byte array</param>
        /// <param name="mechListMic">the byte-array formatted MechListMIC that contains in the inner payload</param>
        /// <returns>the internal security token</returns>
        private byte[] UnwrapNegotiationToken(byte[] inToken, out byte[] mechListMic)
        {
            mechListMic = null;

            SpngNegotiationToken negToken = new SpngNegotiationToken();
            negToken.FromBytes(inToken);

            if (negToken != null)
            {
                mechListMic = negToken.MechListMIC;
            }

            if (negToken.MechToken == null || negToken.MechToken.Length == 0)
                return null;

            return negToken.MechToken;
        }


        /// <summary>
        /// Generate a NegotiationToken byte array, and insert the internal security token
        /// </summary>
        /// <param name="payloadType">the internal payload type</param>
        /// <param name="inToken">the internal security token</param>
        /// <returns>the generated NegotiationToken byte array</returns>
        private byte[] WrapNegotiationToken(SpngPayloadType payloadType, byte[] inToken)
        {
            byte[] mechListMic = null;

            if (this.needMechListMic &&
                securityMechContext != null && !securityMechContext.NeedContinueProcessing)
            {
                mechListMic = this.client.GenerateMechListMIC(securityMechContext);
            }

            SpngNegotiationToken negToken = client.CreateNegotiationToken(payloadType,
                new NegState(NegState.accept_incomplete),
                inToken,
                mechListMic);

            return negToken.ToBytes();
        }

        private void SpngNegotiationInitial(byte[] inToken)
        {
            byte[] securityToken = null;
            MechTypeList serverMechList = null;

            if ((inToken == null) || (inToken.Length == 0)) // Client Initiation Mode
            {
                InitializeSecurityContext(this.client.Config.MechList.Elements[0], securityToken);
                this.client.Context.NegotiationState = SpngNegotiationState.AcceptIncomplete;
            }
            else // Server Initiation Mode
            {
                try
                {
                    securityToken = UnwrapInitialNegToken2(inToken, out serverMechList);
                    this.client.NegotiateMechType(serverMechList);
                    if (this.client.Context.NegotiationState == SpngNegotiationState.Reject)
                    {
                        //Negotiation failed. Do not need to throw exception in this case.
                        return;
                    }
                }
                catch
                {
                    // check if reauth token
                    SpngNegotiationToken negToken = new SpngNegotiationToken();
                    negToken.FromBytes(inToken);
                    this.client.Context.NegotiatedMechType = negToken.SupportedMechType;    // try use preview MechType to do Re-Initialize
                    securityToken = null;
                }

                InitializeSecurityContext(this.client.Context.NegotiatedMechType, securityToken);
            }

            if (this.client.Context.NegotiationState == SpngNegotiationState.AcceptIncomplete) // server prefered mechtype can find from local support mechtype list.
            {
                try
                {
                    securityMechContext.Initialize(securityToken);
                }
                catch (Exception ex)
                {
                    if (securityMechContext is NlmpClientSecurityContext)
                    {
                        throw ex;
                    }
                    SwitchToNTLMSSP(securityToken); // try use NTLM
                }
            }
            else
            {
                securityMechContext.Initialize(null);
            }

            UpdateNegotiationToken(securityToken);
        }

        private void SpngNegotiationRequestMic(byte[] inToken)
        {
            byte[] securityToken = null;
            if ((inToken == null) || (inToken.Length == 0))
            {
                throw new ArgumentNullException("inToken");
            }
            securityToken = UnwrapNegotiationToken(inToken);
            this.needMechListMic = true;
            this.client.Context.NegotiationState = SpngNegotiationState.AcceptIncomplete;

            securityMechContext.Initialize(securityToken);
            if (securityMechContext.Token != null)
            {
                this.token = WrapNegotiationToken(SpngPayloadType.NegResp, securityMechContext.Token);
            }
        }

        private void SpngNegotiationAcceptIncomplete(byte[] inToken)
        {
            byte[] securityToken = null;
            bool isNeedWrap = true;
            if ((inToken == null) || (inToken.Length == 0))
            {
                throw new ArgumentNullException("inToken");
            }

            byte[] mechListMic = null;
            if (CurrentSecurityConfig.GetType() == typeof(NlmpClientSecurityConfig))
            {
                NlmpClientSecurityConfig nlmpConfig = CurrentSecurityConfig as NlmpClientSecurityConfig;
                if ((nlmpConfig.SecurityAttributes & ClientSecurityContextAttribute.DceStyle) == ClientSecurityContextAttribute.DceStyle)
                {
                    isNeedWrap = false;
                }
            }

            securityToken = isNeedWrap ? UnwrapNegotiationToken(inToken, out mechListMic) : inToken;

            if (!securityMechContext.NeedContinueProcessing)
            {
                this.needContinueProcessing = false;
                this.client.Context.NegotiationState = SpngNegotiationState.AcceptCompleted;
                this.token = null;

                if (isNeedWrap && mechListMic != null && !this.client.VerifyMechListMIC(securityMechContext, mechListMic))
                {
                    throw new InvalidOperationException("Invalid MechListMic");
                }
            }
            else
            {
                securityMechContext.Initialize(securityToken);
                if (!securityMechContext.NeedContinueProcessing)
                {
                    this.needMechListMic = true;
                }
                this.token = isNeedWrap ? WrapNegotiationToken(SpngPayloadType.NegResp, securityMechContext.Token) : securityMechContext.Token;
            }
        }

        private void SwitchToNTLMSSP(byte[] securityToken)
        {
            securityMechContext = null; // try use NTLM
            this.client.Config.MechList = new MechTypeList(new MechType[] { this.client.Config.MechList.Elements[1] });
            InitializeSecurityContext(this.client.Config.MechList.Elements[0], null);
            //this.client.Context.NegotiationState = SpngNegotiationState.AcceptIncomplete;
            securityMechContext.Initialize(securityToken);

            UpdateNegotiationToken(securityToken);
        }

        private void UpdateNegotiationToken(byte[] securityToken)
        {
            if (this.client.Context.NegotiationState == SpngNegotiationState.SspiNegotiation)
            {
                //SSPI negotiation already has an SPNG wrapper.
                this.token = securityToken;
            }
            else
            {
                bool isNeedWrap = true;
                if (CurrentSecurityConfig.GetType() == typeof(NlmpClientSecurityConfig))
                {
                    NlmpClientSecurityConfig nlmpConfig = CurrentSecurityConfig as NlmpClientSecurityConfig;
                    if ((nlmpConfig.SecurityAttributes & ClientSecurityContextAttribute.DceStyle) == ClientSecurityContextAttribute.DceStyle)
                    {
                        isNeedWrap = false;
                    }
                }
                this.token = isNeedWrap ? WrapInitialNegToken(securityMechContext.Token) : securityMechContext.Token;
            }
        }

        #region IDispose
        /// <summary>
        /// Release the managed and unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Release all resources
        /// </summary>
        /// <param name="disposing">Indicate user or GC calling this method</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    securityMechContext = null;
                }

                disposed = true;
            }
        }

        public override object QueryContextAttributes(string contextAttribute)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Deconstructor
        /// </summary>
        ~SpngClientSecurityContext()
        {
            Dispose(false);
        }
        #endregion
    }
}
