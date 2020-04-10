using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
using System;

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
        /// Qulity of protection
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
        /// Whether to continue process.
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
        /// <returns>Security Context</returns>
        protected void AcquireCredentialsHandle<T>(T accountCredential)
        {
            this.UseNativeSSP(accountCredential);
        }

        public override bool Decrypt(params SecurityBuffer[] securityBuffers)
        {
            return this.Context.Decrypt(securityBuffers);
        }

        public override void Encrypt(params SecurityBuffer[] securityBuffers)
        {
            this.Context.Encrypt(securityBuffers);
        }

        public override void Initialize(byte[] token)
        {
            this.Context.Initialize(token);
        }

        public override void Sign(params SecurityBuffer[] securityBuffers)
        {
            this.Context.Sign(securityBuffers);
        }

        public override bool Verify(params SecurityBuffer[] securityBuffers)
        {
            return this.Context.Verify(securityBuffers);
        }

        public override object QueryContextAttributes(string contextAttribute)
        {
            return this.Context.QueryContextAttributes(contextAttribute);
        }

        #endregion

        #region

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

        public void Dispose()
        {
            this.Context = null;
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
