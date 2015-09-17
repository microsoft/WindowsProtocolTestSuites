// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp
{
    /// <summary>
    /// NTLM client SecurityContext configuration
    /// </summary>
    public class NlmpClientSecurityConfig : SecurityConfig
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
        /// Constructor
        /// </summary>
        /// <param name="account">Client account credential.</param>
        /// <param name="target">Target name.</param>
        public NlmpClientSecurityConfig(
                AccountCredential account,
                string target)
            : base(SecurityPackageType.Ntlm)
        {
            this.clientCredential = account;
            this.targetName = target;
        }
    }
}
