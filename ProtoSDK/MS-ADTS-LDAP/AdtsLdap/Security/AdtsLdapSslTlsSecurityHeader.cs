// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    /// <summary>
    /// the ssl/tls transport header, which is used to decode the SSL/TLS header.
    /// </summary>
    internal class AdtsLdapSslTlsSecurityHeader
    {
        #region Fields

        /// <summary>
        /// a byte value that indicates the SSL/TLS hand shake content type.
        /// </summary>
        private const byte CONTENT_TYPE_HANDSHAKE = 0x16;

        /// <summary>
        /// an int value that indicates the size of header.
        /// </summary>
        public const int SIZE_OF_HEADER = 5;

        /// <summary>
        /// a byte value that indicates the content type.
        /// </summary>
        public byte ContentType;

        /// <summary>
        /// a byte value that indicates the major version.
        /// </summary>
        public byte MajorVersion;

        /// <summary>
        /// a byte value that indicates the minor version.
        /// </summary>
        public byte MinorVersion;

        /// <summary>
        /// a ushort value that indicates the length of payload.
        /// </summary>
        public ushort Length;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public AdtsLdapSslTlsSecurityHeader()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// get a bool value that specifies whether the data is ssl handshake data.
        /// </summary>
        public bool IsSslHandShake
        {
            get
            {
                return this.ContentType == CONTENT_TYPE_HANDSHAKE;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// a bool value that indicates whether the ssl packet is a valid sasl packet.<para/>
        /// the length must larger than zero<para/>
        /// the buffer length must be larger or equal to SIZE plus length.
        /// </summary>
        public bool IsValid(int bufferLength)
        {
            return this.Length > 0
                    && bufferLength >= SIZE_OF_HEADER + this.Length;
        }


        /// <summary>
        /// unmarshal from bytes.
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

            // data is not enough, return.
            if (data.Length < SIZE_OF_HEADER)
            {
                return;
            }

            this.ContentType = data[0];
            this.MajorVersion = data[1];
            this.MinorVersion = data[2];
            this.Length = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(data, 3));
        }

        #endregion
    }
}