// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// The BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO structure is used with 
    /// the BCryptEncrypt and BCryptDecrypt functions to contain additional 
    /// information related to authenticated cipher modes.
    /// </summary>
    internal struct BcryptAuthenticatedCipherModeInfo
    {
        /// <summary>
        /// The size, in bytes, of this structure.
        /// </summary>
        internal uint cbSize;
        /// <summary>
        /// The version number of the structure.
        /// BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO_VERSION 1
        /// </summary>
        internal uint dwInfoVersion;
        /// <summary>
        /// A pointer to a buffer that contains a nonce.
        /// </summary>
        internal IntPtr pbNonce;
        /// <summary>
        /// The size, in bytes, of the buffer pointed to by the pbNonce member. 
        /// </summary>
        internal uint cbNonce;
        /// <summary>
        /// A pointer to a buffer that contains the authenticated data. 
        /// </summary>
        internal IntPtr pbAuthData;
        /// <summary>
        /// The size, in bytes, of the buffer pointed to by the pbAuthData member. 
        /// </summary>
        internal uint cbAuthData;
        /// <summary>
        /// BCryptEncrypt: The buffer will receive the authentication tag.
        /// BCryptDecrypt: The buffer contains the authentication tag to be checked against.
        /// </summary>
        internal IntPtr pbTag;
        /// <summary>
        /// The size, in bytes, of the pbTag buffer. 
        /// </summary>
        internal uint cbTag;
        /// <summary>
        /// A pointer to a buffer that stores the partially computed MAC 
        /// between calls to BCryptEncrypt and BCryptDecrypt when chaining 
        /// encryption or decryption.
        /// </summary>
        internal IntPtr pbMacContext;
        /// <summary>
        /// The size, in bytes, of the buffer pointed to by the pbMacContext member.
        /// </summary>
        internal uint cbMacContext;
        /// <summary>
        /// The length, in bytes, of additional authenticated data (AAD) 
        /// to be used by the BCryptEncrypt and BCryptDecrypt functions.
        /// </summary>
        internal uint cbAAD;
        /// <summary>
        /// The length, in bytes, of the payload data that was encrypted or decrypted. 
        /// </summary>
        internal ulong cbData;
        /// <summary>
        /// This flag is used when chaining BCryptEncrypt or BCryptDecrypt function calls. 
        /// If calls are not being chained, this member must be set to zero.
        /// BCRYPT_AUTH_MODE_CHAIN_CALLS_FLAG 0x00000001
        /// BCRYPT_AUTH_MODE_IN_PROGRESS_FLAG 0x00000002
        /// </summary>
        internal uint dwFlags;
    }
}
