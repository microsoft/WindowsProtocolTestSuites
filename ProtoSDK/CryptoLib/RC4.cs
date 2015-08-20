// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Cryptography;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// An abstract class which all RC4 Algorithm implementation will inherit.
    /// </summary>
    public abstract class RC4 : SymmetricAlgorithm
    {
        /// <summary>
        /// Generate default RC4 instance
        /// </summary>
        /// <returns>default RC4 instance</returns>
        public new static RC4 Create()
        {
            return new RC4CryptoServiceProvider();
        }
    }
}
