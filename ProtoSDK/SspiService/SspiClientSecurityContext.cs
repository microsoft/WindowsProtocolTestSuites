// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp;
using Microsoft.Protocols.TestTools.StackSdk.Security.Spng;
using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
using System;
using System.Net;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.SspiService
{
    public class SspiClientSecurityContext : ClientSecurityContext, IDisposable
    {
        #region fields
        protected ClientSecurityContext Context;

        /// <summary>
        /// Server's principal name.
        /// </summary>
        protected string serverPrincipalName;

        /// <summary>
        /// Security package type.
        /// </summary>
        protected SecurityPackageType packageType;

        /// <summary>
        /// Quality of protection
        /// </summary>
        private SECQOP_WRAP qualityOfProtection;

        /// <summary>
        /// Bit flags that indicate requests for the context.
        /// </summary>
        protected ClientSecurityContextAttribute securityContextAttributes;

        /// <summary>
        /// The data representation, such as byte ordering, on the target.
        /// </summary>
        protected SecurityTargetDataRepresentation targetDataRepresentaion;

        #endregion

        #region properties
        /// <summary>
        /// Gets server principal name
        /// </summary>
        public virtual string ServerPrincipalName
        {
            get
            {
                return this.serverPrincipalName;
            }
        }

        /// <summary>
        /// Package type
        /// </summary>
        public override SecurityPackageType PackageType
        {
            get
            {
                return this.packageType;
            }
        }

        /// <summary>
        /// Whether need continue processing.
        /// </summary>
        public override bool NeedContinueProcessing
        {
            get
            {
                return this.Context.NeedContinueProcessing;
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
                return this.Context.SequenceNumber;
            }
            set
            {
                this.Context.SequenceNumber = value;
            }
        }

        /// <summary>
        /// The session Key
        /// </summary>
        public override byte[] SessionKey
        {
            get
            {
                return this.Context.SessionKey;
            }
        }

        /// <summary>
        /// The token returned by Sspi.
        /// </summary>
        public override byte[] Token
        {
            get
            {
                return this.Context.Token;
            }
        }

        /// <summary>
        /// Queries the sizes of the structures used in the per-message functions.
        /// </summary>
        public override SecurityPackageContextSizes ContextSizes
        {
            get
            {
                return this.Context.ContextSizes;
            }
        }

        /// <summary>
        /// Package-specific flags that indicate the quality of protection. A security package can use this parameter
        /// to enable the selection of cryptographic algorithms.
        /// </summary>
        public SECQOP_WRAP QualityOfProtection
        {
            get
            {
                return this.qualityOfProtection;
            }
            set
            {
                this.qualityOfProtection = value;
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        protected SspiClientSecurityContext()
        {

        }

        /// <summary>
        /// Constructor with client credential, principal of server, ContextAttributes and TargetDataRep.
        /// </summary>
        /// <param name="packageType">Specifies the name of the security package with which these credentials will be used
        /// </param>
        /// <param name="clientCredential">Client account credential, if null, use default user account</param>
        /// <param name="serverPrincipal">Server principal name</param>
        /// <param name="contextAttributes">Context attributes</param>
        /// <param name="targetDataRep">The data representation, such as byte ordering, on the target. 
        /// This parameter can be either SECURITY_NATIVE_DREP or SECURITY_NETWORK_DREP.</param>
        public SspiClientSecurityContext(
            SecurityPackageType packageType,
            AccountCredential clientCredential,
            string serverPrincipal,
            ClientSecurityContextAttribute contextAttributes,
            SecurityTargetDataRepresentation targetDataRep)
        {
            this.packageType = packageType;
            this.serverPrincipalName = serverPrincipal;
            this.securityContextAttributes = contextAttributes;
            this.targetDataRepresentaion = targetDataRep;

            this.AcquireCredentialsHandle(clientCredential);
        }

        /// <summary>
        /// Constructor with client credential, principal of server, ContextAttributes and TargetDataRep.
        /// </summary>
        /// <param name="packageType">Specifies the name of the security package with which these credentials will be used
        /// </param>
        /// <param name="clientCredential">Client account credential</param>
        /// <param name="serverPrincipal">Server principal name</param>
        /// <param name="contextAttributes">Context attributes</param>
        /// <param name="targetDataRep">The data representation, such as byte ordering, on the target. 
        /// This parameter can be either SECURITY_NATIVE_DREP or SECURITY_NETWORK_DREP.</param>
        public SspiClientSecurityContext(
            SecurityPackageType packageType,
            CertificateCredential clientCredential,
            string serverPrincipal,
            ClientSecurityContextAttribute contextAttributes,
            SecurityTargetDataRepresentation targetDataRep)
        {
            this.packageType = packageType;
            this.serverPrincipalName = serverPrincipal;
            this.securityContextAttributes = contextAttributes;
            this.targetDataRepresentaion = targetDataRep;

            this.AcquireCredentialsHandle(clientCredential);
        }

        #region public Methods

        /// <summary>
        /// AcquireCredentialsHandle
        /// </summary>
        /// <param name="accountCredential"></param>
        protected void AcquireCredentialsHandle<T>(T accountCredential)
        {
            if (!string.IsNullOrEmpty(this.serverPrincipalName))
            {
                this.serverPrincipalName = GetServicePrincipalName(this.serverPrincipalName);
            }

            switch (packageType)
            {
                case SecurityPackageType.Ntlm:
                    {
                        if (accountCredential is AccountCredential)
                        {
                            var credential = accountCredential as AccountCredential;
                            this.Context = new NlmpClientSecurityContext(
                                new NlmpClientCredential(
                                    this.serverPrincipalName,
                                    credential.DomainName,
                                    credential.AccountName,
                                    credential.Password),
                                this.securityContextAttributes
                           );
                        }
                        else
                        {
                            throw new NotSupportedException("NTLM only support AccountCredential, Please provide an AccountCredential and try again.");
                        }
                        return;
                    }
                case SecurityPackageType.Kerberos:
                    {
                        if (accountCredential is AccountCredential)
                        {
                            var credential = accountCredential as AccountCredential;
                            IPAddress kdcIpAddress = (string.IsNullOrEmpty(KerberosContext.KDCComputerName) ? credential.DomainName : KerberosContext.KDCComputerName).ParseIPAddress();
                            this.Context = KerberosClientSecurityContext.CreateClientSecurityContext(
                                this.serverPrincipalName,
                                credential,
                                KerberosAccountType.User,
                                kdcIpAddress,
                                KerberosContext.KDCPort,
                                TransportType.TCP,
                                this.securityContextAttributes
                            );
                        }
                        else
                        {
                            throw new NotSupportedException("Kerberos only support AccountCredential, Please provide an AccountCredential and try again.");
                        }
                        return;
                    }
                case SecurityPackageType.Negotiate:
                    {
                        if (accountCredential is AccountCredential)
                        {
                            if (this.securityContextAttributes == ClientSecurityContextAttribute.None)
                            {
                                this.securityContextAttributes = ClientSecurityContextAttribute.MutualAuth; // MS-SPNG 3.3.3 The client MUST request Mutual Authentication services
                            }

                            var credential = accountCredential as AccountCredential;
                            NlmpClientSecurityConfig nlmpSecurityConfig = new NlmpClientSecurityConfig(credential, this.serverPrincipalName, this.securityContextAttributes);

                            IPAddress kdcIpAddress = (string.IsNullOrEmpty(KerberosContext.KDCComputerName) ? credential.DomainName : KerberosContext.KDCComputerName).ParseIPAddress();
                            KerberosClientSecurityConfig kerberosSecurityConfig = new KerberosClientSecurityConfig(
                                credential,
                                credential.AccountName,
                                this.serverPrincipalName,
                                kdcIpAddress,
                                KerberosContext.KDCPort,
                                this.securityContextAttributes,
                                TransportType.TCP
                            );

                            this.Context = new SpngClientSecurityContext(this.securityContextAttributes, nlmpSecurityConfig, kerberosSecurityConfig);
                        }
                        else
                        {
                            throw new NotSupportedException("Negotiate SSP only support AccountCredential, Please provide an AccountCredential and try again.");
                        }
                        return;
                    }
                    //case SecurityPackageType.Schannel:
                    //    throw new NotImplementedException();
                    //case SecurityPackageType.CredSsp:
                    //    throw new NotImplementedException();
                    //default:
                    //    throw new NotImplementedException();
            }

            this.UseNativeSSP(accountCredential);
        }

        /// <summary>
        /// This takes the given SecBuffers, which are used by SSPI method DecryptMessage.
        /// </summary>
        /// <param name="securityBuffers">SecBuffer.Encrypted data will be filled in SecBuffers.</param>
        /// <returns>If successful, returns true, otherwise false.</returns>
        /// <exception cref="SspiException">If decrypt fail, this exception will be thrown.</exception>
        public override bool Decrypt(params SecurityBuffer[] securityBuffers)
        {
            return this.Context.Decrypt(securityBuffers);
        }

        /// <summary>
        /// Encrypts Message. User decides what SecBuffers are used.
        /// </summary>
        /// <param name="securityBuffers">SecBuffers.</param>
        /// <exception cref="SspiException">If encrypt fail, this exception will be thrown.</exception>
        public override void Encrypt(params SecurityBuffer[] securityBuffers)
        {
            this.Context.Encrypt(securityBuffers);
        }

        /// <summary>
        /// Initialize SecurityContext with a server token.
        /// </summary>
        /// <param name="serverToken">Server Token</param>
        /// <exception cref="SspiException">If Initialize fail, this exception will be thrown.</exception>
        public override void Initialize(byte[] token)
        {
            this.Context.Initialize(token);
        }

        /// <summary>
        /// Sign data according SecBuffers.
        /// </summary>
        /// <param name="securityBuffers">SecurityBuffer array</param>
        public override void Sign(params SecurityBuffer[] securityBuffers)
        {
            this.Context.Sign(securityBuffers);
        }

        /// <summary>
        /// Verify Data according to SecBuffers.
        /// </summary>
        /// <param name="securityBuffers">SecBuffer array</param>
        /// <returns>True if the signature matches the signed 
        public override bool Verify(params SecurityBuffer[] securityBuffers)
        {
            return this.Context.Verify(securityBuffers);
        }

        /// <summary>
        /// Query context attribute by Sspi QueryContextAttributes method.
        /// </summary>
        /// <param name="contextAttribute">Attribute name same as msdn: 
        /// http://msdn.microsoft.com/en-us/library/aa379326(VS.85).aspx</param>
        /// <returns>The attribute value</returns>
        /// <exception cref="SspiException">If QueryContextAttributes fail, this exception will be thrown.</exception>
        public override object QueryContextAttributes(string contextAttribute)
        {
            return this.Context.QueryContextAttributes(contextAttribute);
        }

        #endregion

        #region

        /// <summary>
        /// Use Native SSP to do auth
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="accountCredential"></param>
        private void UseNativeSSP<T>(T accountCredential)
        {
            if (accountCredential is AccountCredential)
            {
                this.Context = new Sspi.SspiClientSecurityContext(
                    packageType,
                    accountCredential as AccountCredential,
                    this.ServerPrincipalName,
                    securityContextAttributes,
                    targetDataRepresentaion
               );
            }
            else if (accountCredential is CertificateCredential)
            {
                this.Context = new Sspi.SspiClientSecurityContext(
                    packageType,
                    accountCredential as CertificateCredential,
                    this.ServerPrincipalName,
                    securityContextAttributes,
                    targetDataRepresentaion
               );
            }
        }

        private static string GetServicePrincipalName(string serverName)
        {
            try
            {
                if (!serverName.Contains("/")) // check SPN is valid
                {
                    // If the server name is an IP address. No need to query DNS.
                    // The server may not support kerberos. Use NTLM instead.
                    IPAddress address;
                    if (IPAddress.TryParse(serverName, out address))
                    {
                        return null;
                    }

                    // sometimes uplayer only provider hostname as serverPrincipalName for example "PDC.contoso.com", so here it'll become "HOST/PDC.contoso.com" to let it as a valid SPN.
                    // [TODO]: find a way to determin which service descriptor the uplayer used then set correct service descriptor, ldap/host/rpc/nfs/imap/pop/http...
                    serverName = $"HOST/{serverName}"; // use host as default service descriptor
                }
                return serverName;
            }
            catch
            {
                // For workgroup environment, it will use NTLM authentication method
                return serverName;
            }
        }

        /// <summary>
        /// Dispose Client Security Context
        /// </summary>
        public void Dispose()
        {
            this.Context = null;
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
