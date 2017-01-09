// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Kile
{
    /// <summary>
    /// Kerberos server SecurityContext configuration
    /// </summary>
    [CLSCompliant(false)]
    public class KileServerSecurityConfig : SecurityConfig
    {
        /// <summary>
        /// Encrypt key of the service principle.
        /// </summary>
        private string encryptionKey;

        /// <summary>
        /// Encrypt key of the service principle.
        /// </summary>
        public string EncryptionKey
        {
            get
            {
                return this.encryptionKey;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public KileServerSecurityConfig(string key)
            : base(SecurityPackageType.Kerberos)
        {
            this.encryptionKey = key;
        }
    }
}
