// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    /// <summary>
    /// sasl security header, which describes the encrypted data format with SASL.<para/>
    /// sasl packet is formatted as LENGTH|SIGNATURE|MESSAGE.
    /// </summary>
    internal class AdtsLdapSaslSecurityHeader
    {
        #region Fields

        /// <summary>
        /// an int value that indicates the size of length.
        /// </summary>
        public const int SIZE_OF_LENGTH = 4;

        /// <summary>
        /// an int value that indicates the length of sasl security packet.<para/>
        /// including the SignatureLength and the SecurityDataLength.
        /// </summary>
        public int Length;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public AdtsLdapSaslSecurityHeader()
        {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="signature">
        /// a bytes array that contains the signature.
        /// </param>
        /// <param name="securityData">
        /// a bytes array that contains the security data.
        /// </param>
        public AdtsLdapSaslSecurityHeader(byte[] signature, byte[] securityData)
        {
            this.Length = signature.Length + securityData.Length;
        }

        #endregion

        #region Methods

        /// <summary>
        /// a bool value that indicates whether the packet is a valid sasl packet.<para/>
        /// the length must be larger than zero,<para/>
        /// the signature and securityData must not be null,<para/>
        /// the length must be the sum of signature and securityData length.
        /// </summary>
        public bool IsValid(int bufferLength)
        {
            return this.Length > 0
                && bufferLength == SIZE_OF_LENGTH + this.Length;
        }


        /// <summary>
        /// unmarshal from bytes, which is formatted as LENGTH|SIGNATURE|MESSAGE.
        /// </summary>
        /// <param name="data">
        /// a bytes array that contains the ssl/tls header
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when data is null.
        /// </exception>
        public void FromBytes(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            // if the data does not contains the length, return.
            if (data.Length < SIZE_OF_LENGTH)
            {
                return;
            }

            // decode length from data.
            this.Length = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(data, 0));
        }


        /// <summary>
        /// marshal to bytes, which is formatted as LENGTH|SIGNATURE|MESSAGE.
        /// </summary>
        /// <returns>
        /// a 4-bytes array that contains the header data.
        /// </returns>
        public byte[] ToBytes()
        {
            return BitConverter.GetBytes(IPAddress.HostToNetworkOrder(this.Length));
        }

        #endregion
    }
}