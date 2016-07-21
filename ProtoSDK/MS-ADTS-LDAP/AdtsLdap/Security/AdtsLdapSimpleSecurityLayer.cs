// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    /// <summary>
    /// the simple security layer, over simple bind,<para/>
    /// which provides simple authentication, and no message security.
    /// </summary>
    public class AdtsLdapSimpleSecurityLayer : AdtsLdapSecurityLayer
    {
        #region Methods

        /// <summary>
        /// return the data directly.
        /// </summary>
        /// <param name="data">
        /// a bytes data that contains the data to be encoded with security provider.
        /// </param>
        /// <returns>
        /// a bytes data that contains the encoded data.
        /// </returns>
        public override byte[] Encode(byte[] data)
        {
            return data;
        }


        /// <summary>
        /// return the data directly.
        /// </summary>
        /// <param name="data">
        /// a bytes data that contains the data to be decoded with security provider.
        /// </param>
        /// <returns>
        /// a bytes data that contains the decoded data.
        /// </returns>
        public override byte[] Decode(byte[] data)
        {
            return data;
        }

        #endregion
    }
}