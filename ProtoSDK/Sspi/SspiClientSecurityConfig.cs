// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Sspi
{
    /// <summary>
    /// SSPI client SecurityContext configuration
    /// </summary>
    public class SspiClientSecurityConfig : SecurityConfig
    {
        /// <summary>
        /// Client account credential
        /// </summary>
        private AccountCredential clientCredential;

        /// <summary>
        /// Server principal
        /// </summary>
        private string serverPrincipal;

        /// <summary>
        /// Client SecurityContext Attributes
        /// </summary>
        private ClientSecurityContextAttribute securityAttributes;

        /// <summary>
        /// The data representation, such as byte ordering, on the target.
        /// </summary>
        private SecurityTargetDataRepresentation targetDataRep;

        /// <summary>
        /// Domain Name of the client account
        /// </summary>
        public string DomainName
        {
            get
            {
                if (this.clientCredential != null)
                {
                    return this.clientCredential.DomainName;
                }
                return null;
            }
        }

        /// <summary>
        /// Account name of the client
        /// </summary>
        public string AccountName
        {
            get
            {
                if (this.clientCredential != null)
                {
                    return this.clientCredential.AccountName;
                }
                return null;
            }
        }

        /// <summary>
        /// Password of the client
        /// </summary>
        public string Password
        {
            get
            {
                if (this.clientCredential != null)
                {
                    return this.clientCredential.Password;
                }
                return null;
            }
        }

        /// <summary>
        /// Client account credential
        /// </summary>
        public AccountCredential ClientCredential
        {
            get
            {
                return this.clientCredential;
            }
        }

        /// <summary>
        /// Server principal
        /// </summary>
        public string ServerPrincipal
        {
            get
            {
                return this.serverPrincipal;
            }
        }

        /// <summary>
        /// Client SecurityContext Attributes
        /// </summary>
        public ClientSecurityContextAttribute SecurityAttributes
        {
            get
            {
                return this.securityAttributes;
            }
        }

        /// <summary>
        /// The data representation, such as byte ordering, on the target. 
        /// This parameter can be either SECURITY_NATIVE_DREP or SECURITY_NETWORK_DREP.
        /// </summary>
        public SecurityTargetDataRepresentation TargetDataRep
        {
            get
            {
                return this.targetDataRep;
            }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="clientCredential">Client account credential.</param>
        /// <param name="serverPrincipal">Server principal.</param>
        /// <param name="contextAttributes">Client security context attributes.</param>
        /// <param name="targetDataRep">The data representation, such as byte ordering, on the target.</param>
        public SspiClientSecurityConfig(
            AccountCredential clientCredential,
            string serverPrincipal,
            ClientSecurityContextAttribute contextAttributes,
            SecurityTargetDataRepresentation targetDataRep)
            : base(SecurityPackageType.Unknown)
        {
            this.clientCredential = clientCredential;
            this.serverPrincipal = serverPrincipal;
            this.securityAttributes = contextAttributes;
            this.targetDataRep = targetDataRep;
        }
    }
}
