using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
using System;
using System.Security.Permissions;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.SspiService
{
    /// <summary>
    /// Security context, which used by DTLS server.
    /// Accept client token to get server token.
    /// </summary>
    public class DtlsServerSecurityContext : SspiServerSecurityContext
    {
        protected new IDtlsServerSecurityContext Context;

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
        /// Constructor
        /// </summary>
        /// <param name="packageType">Specifies the name of the security package with which these credentials will be used
        /// </param>
        /// <param name="serverCredential">The credential of server, if null, use default user account.</param>
        /// <param name="serverPrincipal">Server principal name</param>
        /// <param name="contextAttributes">Bit flags that specify the attributes required by the server to establish 
        /// the context</param>
        /// <param name="targetDataRep">The data representation, such as byte ordering, on the target. This parameter 
        /// can be either SECURITY_NATIVE_DREP or SECURITY_NETWORK_DREP.</param>
        [SecurityPermission(SecurityAction.Demand)]
        public DtlsServerSecurityContext(
            SecurityPackageType packageType,
            CertificateCredential serverCredential,
            string serverPrincipal,
            ServerSecurityContextAttribute contextAttributes,
            SecurityTargetDataRepresentation targetDataRep)
        {
            this.packageType = packageType;
            this.serverPrincipalName = serverPrincipal;
            this.securityContextAttributes = contextAttributes;
            this.targetDataRepresentaion = targetDataRep;

            this.Context = new Sspi.DtlsServerSecurityContext(packageType, serverCredential, serverPrincipal, contextAttributes, targetDataRep);
        }

        /// <summary>
        /// Accept client token.
        /// </summary>
        /// <param name="clientToken">Token of client</param>
        /// <exception cref="SspiException">If Accept fail, this exception will be thrown.</exception>
        [SecurityPermission(SecurityAction.Demand)]
        public override void Accept(byte[] clientToken)
        {
            this.Context.Accept(clientToken);
        }
    }
}
