// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// Utility class encapsulate common code.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Change little endian byte array to unsigned BigInteger.
        /// </summary>
        /// <param name="input">input byte array data.</param>
        /// <returns>BigInteger data</returns>
        public static BigInteger LoadBytesLittleEndian(byte[] input)
        {
            byte[] tempInput = new byte[input.Length];
            Array.Copy(input, tempInput, input.Length);

            Array.Reverse(tempInput);
            Array.Resize(ref tempInput, tempInput.Length + 1);

            return new BigInteger(tempInput);
        }
    }
}
