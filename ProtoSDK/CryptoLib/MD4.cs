// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Cryptography;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// An abstract class which every MD4 algorithm implementation will inherit.
    /// </summary>
    public abstract class MD4 : HashAlgorithm
    {
        protected MD4()
        {
            HashSizeValue = 128;
        }

        /// <summary>
        /// Generate a default MD4 instance
        /// </summary>
        /// <returns>a default MD4 instance</returns>
        public new static MD4 Create()
        {
            return new MD4Managed();
        }
    }
}
