// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Kile
{
    /// <summary>
    /// Define const values used in this project.
    /// </summary>
    internal static class ConstValue
    {
        #region transport
        /// <summary>
        /// The default transport buffer size.
        /// </summary>
        internal const int TRANSPORT_BUFFER_SIZE = 1500;

        /// <summary>
        /// The timeout exception.
        /// </summary>
        internal const string TIMEOUT_EXCEPTION = "It has been timeout when receiving packets.";

        /// <summary>
        /// Default value for max connections set on server.
        /// </summary>
        internal const int MAX_CONNECTIONS = 10;

        /// <summary>
        /// Default value for kdc port.
        /// </summary>
        internal const int KDC_PORT = 88;

        #endregion transport

        #region AS request
        /// <summary>
        /// The version number of KerberosV5.
        /// </summary>
        internal const int KERBEROSV5 = 5;

        /// <summary>
        /// The till time in AS request.
        /// </summary>
        internal const string TGT_TILL_TIME = "20370913024805Z";

        /// <summary>
        /// The rtime in AS request.
        /// </summary>
        internal const string TGT_RTIME = "20370913024805Z";

        /// <summary>
        /// The service principle name of Kerberos.
        /// </summary>
        internal const string KERBEROS_SNAME = "krbtgt";

        /// <summary>
        /// The default timeout value.
        /// </summary>
        internal static readonly TimeSpan TIMEOUT_DEFAULT = new TimeSpan(0, 0, 2);
        #endregion AS request

        #region AP request
        /// <summary>
        /// The key length of AES256_CTS_HMAC_SHA1_96.
        /// </summary>
        internal const int AES_KEY_LENGTH = 32;

        /// <summary>
        /// The version number of KerberosV5.
        /// </summary>
        internal const int AUTHENTICATOR_CHECKSUM_LENGTH = 16;

        /// <summary>
        /// The default size of AuthCheckSum.
        /// </summary>
        internal const int AUTH_CHECKSUM_SIZE = 24;

        /// <summary>
        /// The default value of sequence number.
        /// </summary>
        internal const int SEQUENCE_NUMBER_DEFAULT = 0x123;

        /// <summary>
        /// The Authorization Data Type AD-AUTH-DATA-AP-OPTIONS has an ad-type of 143
        /// and ad-data of KERB_AP_OPTIONS_CBT (0x4000).
        /// </summary>
        internal const int KERB_AP_OPTIONS_CBT = 0x4000;
        #endregion AP request

        #region token
        /// <summary>
        /// Filler field in wrap token for [rfc4121].
        /// </summary>
        internal const int TOKEN_FILLER_1_BYTE = 0xFF;

        /// <summary>
        /// Filler field in wrap token for [rfc1964].
        /// </summary>
        internal const int TOKEN_FILLER_2_BYTE = 0xFFFF;

        /// <summary>
        /// The size of confounder in wrap token for [rfc1964] and [rfc4757].
        /// </summary>
        internal const int CONFOUNDER_SIZE = 8;

        /// <summary>
        /// The size of checksum in wrap and mic token for [rfc4757].
        /// </summary>
        internal const int CHECKSUM_SIZE_RFC1964 = 8;

        /// <summary>
        /// The size of sequence number in wrap and mic token for [rfc4757].
        /// </summary>
        internal const int SEQUENCE_NUMBER_SIZE = 8;

        /// <summary>
        /// The first 8 byte-size of header in wrap and mic token for [rfc4757].
        /// </summary>
        internal const int HEADER_FIRST_8_BYTE_SIZE = 8;

        /// <summary>
        /// The signaturekey in wrap and mic token for [rfc4757].
        /// </summary>
        internal const string SIGNATURE_KEY = "signaturekey\0";

        /// <summary>
        /// The fortybits in wrap and mic token for [rfc4757].
        /// </summary>
        internal const string FORTY_BITS = "fortybits\0";

        /// <summary>
        /// The tag of kerberos.
        /// </summary>
        internal const byte KERBEROS_TAG = 0x60;

        /// <summary>
        /// Default max token size
        /// </summary>
        internal const ushort MAX_TOKEN_SIZE = 12288;

        /// <summary>
        /// Default max signature size
        /// </summary>
        internal const byte MAX_SIGNATURE_SIZE = 50;

        /// <summary>
        /// Default max signature size
        /// </summary>
        internal const byte SECURITY_TRAILER_SIZE = 76;

        /// <summary>
        /// Integral size of the messages
        /// </summary>
        internal const byte BLOCK_SIZE = 8;

        /// <summary>
        /// The kerberos oid (1.2.840.113554.1.2.2).
        /// </summary>
        internal readonly static byte[] KERBEROS_OID =
            new byte[] { 0x06, 0x09, 0x2a, 0x86, 0x48, 0x86, 0xf7, 0x12, 0x01, 0x02, 0x02 };
        #endregion token

        #region encryption and decryption
        /// <summary>
        /// (8 bits) The length of byte in bits
        /// </summary>
        internal const int BYTE_SIZE = 8;

        /// <summary>
        /// (8 bytes = 64 bits) Size of DES encryption block
        /// </summary>
        internal const int DES_BLOCK_SIZE = 8;
        #endregion encryption and decryption
    }
}
