// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Cryptography;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// The abstract LM hash class, every LM hash implementation should inherit this class
    /// </summary>
    public abstract class LMHash : HashAlgorithm
    {
        /// <summary>
        /// Generate a default lm hash instance
        /// </summary>
        /// <returns>The default lm hash instance</returns>
        public new static LMHash Create()
        {
            return new LMHashManaged();
        }
    }
}
