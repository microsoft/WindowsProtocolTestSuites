// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp
{
    /// <summary>
    /// NTLM server SecurityContext configuration
    /// </summary>
    public class NlmpServerSecurityConfig : SecurityConfig
    {
        /// <summary>
        /// Client account credential
        /// </summary>
        private AccountCredential clientCredential;

        /// <summary>
        /// Target name
        /// </summary>
        private string targetName;

        /// <summary>
        /// Negotiation flags
        /// </summary>
        private NegotiateTypes negotiateflags;

        /// <summary>
        /// Identifier of whether joined domain
        /// </summary>
        private bool isDomainJoined;

        /// <summary>
        /// Netbios Domain Name
        /// </summary>
        private string netbiosDomainName;

        /// <summary>
        /// Netbios Machine Name
        /// </summary>
        private string netbiosMachineName;

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
        /// Target name
        /// </summary>
        public string TargetName
        {
            get
            {
                return this.targetName;
            }
        }

        /// <summary>
        /// Netbios domain name
        /// </summary>
        public string NetbiosDomainName
        {
            get
            {
                return this.netbiosDomainName;
            }
        }

        /// <summary>
        /// Netbios machine name
        /// </summary>
        public string NetbiosMachineName
        {
            get
            {
                return this.netbiosMachineName;
            }
        }

        /// <summary>
        /// Identifier of whether joined in a domain or not
        /// </summary>
        public bool IsDomainJoined
        {
            get
            {
                return this.isDomainJoined;
            }
        }

        /// <summary>
        /// Negotiation flags
        /// </summary>
        public NegotiateTypes NegotiateFlags
        {
            get
            {
                return this.negotiateflags;
            }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="flags">Negotiation flags.</param>
        /// <param name="clientCredential">Client account credential.</param>
        /// <param name="isDomainJoined">Joined in a domain or not</param>
        /// <param name="netbiosDomainName">Netbios domain name.</param>
        /// <param name="netbiosMachineName">Netbios machine name.</param>
        public NlmpServerSecurityConfig(
            NegotiateTypes flags,
            NlmpClientCredential clientCredential,
            bool isDomainJoined,
            string netbiosDomainName,
            string netbiosMachineName)
            : base(SecurityPackageType.Ntlm)
        {
            this.negotiateflags = flags;
            this.clientCredential = clientCredential;
            this.isDomainJoined = isDomainJoined;
            this.netbiosDomainName = netbiosDomainName;
            this.netbiosMachineName = netbiosMachineName;
            this.targetName = clientCredential.TargetName;
        }
    }
}
