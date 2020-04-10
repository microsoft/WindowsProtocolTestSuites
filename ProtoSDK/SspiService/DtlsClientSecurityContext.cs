using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
using System;
using System.Security.Permissions;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.SspiService
{
    /// <summary>
    /// SecurityContext used by DTLS client.
    /// Supports DTLS 1.0
    /// Invokes InitializeSecurityContext function of SSPI
    /// </summary>
    public class DtlsClientSecurityContext : SspiClientSecurityContext
    {
        protected new IDtlsClientSecurityContext Context;

        /// <summary>
        /// Indicates if more fragments need to be output
        /// </summary>
        public bool HasMoreFragments
        {
            get { return this.Context.HasMoreFragments; }
        }

        /// <summary>
        /// The SecPkgContext_StreamSizes structure indicates the sizes of the various stream components for use with the 
        /// message support functions.
        /// </summary>
        public SecurityPackageContextStreamSizes StreamSizes
        {
            get
            {
                return this.Context.StreamSizes;
            }
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
        [SecurityPermission(SecurityAction.Demand)]
        public DtlsClientSecurityContext(
            SecurityPackageType packageType,
            CertificateCredential clientCredential,
            string serverPrincipal,
            ClientSecurityContextAttribute contextAttributes,
            SecurityTargetDataRepresentation targetDataRep)
        {
            if (clientCredential == null) clientCredential = new CertificateCredential(null);
            this.packageType = packageType;
            this.serverPrincipalName = serverPrincipal;
            this.securityContextAttributes = contextAttributes;
            this.targetDataRepresentaion = targetDataRep;

            this.Context = new Sspi.DtlsClientSecurityContext(packageType, clientCredential, serverPrincipal, contextAttributes, targetDataRep) as IDtlsClientSecurityContext;
        }

        /// <summary>
        /// Initialize SecurityContext with a server token.
        /// </summary>
        /// <param name="serverToken">Server Token</param>
        /// <exception cref="SspiException">If Initialize fail, this exception will be thrown.</exception>
        [SecurityPermission(SecurityAction.Demand)]
        public override void Initialize(byte[] serverToken)
        {
            this.Context.Initialize(serverToken);
        }
    }
}
