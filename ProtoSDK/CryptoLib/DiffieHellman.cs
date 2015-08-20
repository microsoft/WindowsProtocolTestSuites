// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// Diffie-Hellman key exchange algorithm.
    /// </summary>
    public abstract class DiffieHellman
    {
        
        #region Public methods

        /// <summary>
        /// Gets Key exchange data by DH group.
        /// </summary>
        /// <returns>Key data</returns>
        public abstract byte[] GenerateKeyData();


        /// <summary>
        /// Generates a key with key exchange data.
        /// </summary>
        /// <param name="keyData">Key exchange data.</param>
        /// <exception cref="ArgumentNullException">Raised when keyData is null.</exception>
        /// <exception cref="StackException">Raised when exponent or prime is not initialized.</exception>
        /// <returns>Generated key.</returns>
        public abstract byte[] GenerateKey(byte[] keyData);
        
        #endregion Public methods
    }
}
