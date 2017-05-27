// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Pac
{
    /// <summary>
    /// Configurations used to construct a PacUtility instance.
    /// </summary>
    public class PacConfig
    {
        /// <summary>
        /// The key used to encrypt credential information.
        /// </summary>
        public byte[] CredentialKey
        {
            get
            {
                return null;
            }
            private set
            {
            }
        }

        /// <summary>
        /// The IV used to encrypt credential information.
        /// </summary>
        public byte[] CredentialIV
        {
            get
            {
                return null;
            }
            private set
            {
            }
        }

        /// <summary>
        /// The key used to generate server signature.
        /// </summary>
        public byte[] ServerSignKey
        {
            get
            {
                return null;
            }
            private set
            {
            }
        }

        /// <summary>
        /// The key used to generate KDC signature.
        /// </summary>
        public byte[] KdcSignKey
        {
            get
            {
                return null;
            }
            private set
            {
            }
        }


        /// <summary>
        /// Creates an instance of PacConfig class using the specified keys and buffer size.
        /// </summary>
        /// <param name="credentialkey">The key used to encrypt credential information.</param>
        /// <param name="credentialIV">The IV used to encrypt credential information.</param>
        /// <param name="serverSignkey">The key used to generate server signature.</param>
        /// <param name="kdcSignkey">The key used to generate KDC signature.</param>
        public PacConfig(
            byte[] credentialkey, 
            byte[] credentialIV, 
            byte[] serverSignkey, 
            byte[] kdcSignkey)
        {
        }
    }
}
