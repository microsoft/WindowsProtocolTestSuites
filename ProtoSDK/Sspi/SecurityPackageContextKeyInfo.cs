// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Sspi
{
    /// <summary>
    /// The SecPkgContext_KeyInfo structure contains information about the session keys used in a security context.
    /// http://msdn.microsoft.com/en-us/library/aa380086(v=VS.85).aspx
    /// </summary>
    public struct SecurityPackageContextKeyInfo
    {
        /// <summary>
        /// Pointer to a null-terminated string that contains the name, if available, of the algorithm used for 
        /// generating signatures, for example "MD5" or "SHA-2".
        /// </summary>
        public string SignatureAlgorithmName;

        /// <summary>
        /// Pointer to a null-terminated string that contains the name, if available, of the algorithm used for 
        /// encrypting messages. Reserved for future use.
        /// </summary>
        public string EncryptAlgorithmName;

        /// <summary>
        /// Specifies the effective key length, in bits, for the session key. This is typically 40, 56, or 128 bits.
        /// </summary>
        public uint KeySize;

        /// <summary>
        /// Specifies the algorithm identifier (ALG_ID) used for generating signatures, if available.
        /// </summary>
        public uint SignatureAlgorithm;

        /// <summary>
        /// Specifies the algorithm identifier (ALG_ID) used for encrypting messages. Reserved for future use.
        /// </summary>
        public uint EncryptAlgorithm;

        /// <summary>
        /// Constructor. Convert SspiSecurityPackageContextKeyInfo to SecurityPackageContextKeyInfo.
        /// </summary>
        /// <param name="contextKeyInfo"></param>
        internal SecurityPackageContextKeyInfo(SspiSecurityPackageContextKeyInfo contextKeyInfo)
        {
            this.SignatureAlgorithmName = Marshal.PtrToStringUni(contextKeyInfo.sEncryptAlgorithmName);
            this.EncryptAlgorithmName = Marshal.PtrToStringUni(contextKeyInfo.sSignatureAlgorithmName);
            this.KeySize = contextKeyInfo.KeySize;
            this.SignatureAlgorithm = contextKeyInfo.SignatureAlgorithm;
            this.EncryptAlgorithm = contextKeyInfo.EncryptAlgorithm;
        }
    }
}
