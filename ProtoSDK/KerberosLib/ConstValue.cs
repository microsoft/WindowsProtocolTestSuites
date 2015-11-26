// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /// <summary>
    /// Define const values used in this project.
    /// </summary>
    public static class KerberosConstValue
    {
        #region transport
        /// <summary>
        /// The default transport buffer size.
        /// </summary>
        public const int TRANSPORT_BUFFER_SIZE = 1500;

        /// <summary>
        /// The timeout exception.
        /// </summary>
        public const string TIMEOUT_EXCEPTION = "It has been timeout when receiving packets.";

        /// <summary>
        /// Default value for max connections set on server.
        /// </summary>
        public const int MAX_CONNECTIONS = 10;

        /// <summary>
        /// Default value for kdc port.
        /// </summary>
        public const int KDC_PORT = 88;

        /// <summary>
        /// Default value for Kpassword port
        /// </summary>
        public const int KPASSWORD_PORT = 464;

        #endregion transport

        #region AS request
        /// <summary>
        /// The version number of KerberosV5.
        /// </summary>
        public const int KERBEROSV5 = 5;

        /// <summary>
        /// The till time in AS request.
        /// </summary>
        public const string TGT_TILL_TIME = "20370913024805Z";

        /// <summary>
        /// The rtime in AS request.
        /// </summary>
        public const string TGT_RTIME = "20370913024805Z";

        /// <summary>
        /// The service principle name of Kerberos.
        /// </summary>
        public const string KERBEROS_SNAME = "krbtgt";

        /// <summary>
        /// The service principle name of Kerberos for Password change.
        /// </summary>
        public const string KERBEROS_KADMIN_SNAME = "kadmin/changepw";

        /// <summary>
        /// The default timeout value.
        /// </summary>
        public static readonly TimeSpan TIMEOUT_DEFAULT = new TimeSpan(0, 0, 2);

        /// <summary>
        /// Timeout in seconds for SMB2 connection over transport
        /// </summary>
        public static readonly TimeSpan TIMEOUT_FOR_SMB2AP = new TimeSpan(0, 2, 0);
        #endregion AS request

        #region AP request
        /// <summary>
        /// The key length of AES256_CTS_HMAC_SHA1_96.
        /// </summary>
        public const int AES_KEY_LENGTH = 32;

        /// <summary>
        /// The version number of KerberosV5.
        /// </summary>
        public const int AUTHENTICATOR_CHECKSUM_LENGTH = 16;

        /// <summary>
        /// The default size of AuthCheckSum.
        /// </summary>
        public const int AUTH_CHECKSUM_SIZE = 24;

        /// <summary>
        /// The default value of sequence number.
        /// </summary>
        public const int SEQUENCE_NUMBER_DEFAULT = 0x123;

        /// <summary>
        /// The Authorization Data Type AD-AUTH-DATA-AP-OPTIONS has an ad-type of 143
        /// and ad-data of KERB_AP_OPTIONS_CBT (0x4000).
        /// </summary>
        public const int KERB_AP_OPTIONS_CBT = 0x4000;
        #endregion AP request

        #region token
        /// <summary>
        /// Filler field in wrap token for [rfc4121].
        /// </summary>
        public const int TOKEN_FILLER_1_BYTE = 0xFF;

        /// <summary>
        /// Filler field in wrap token for [rfc1964].
        /// </summary>
        public const int TOKEN_FILLER_2_BYTE = 0xFFFF;

        /// <summary>
        /// The size of confounder in wrap token for [rfc1964] and [rfc4757].
        /// </summary>
        public const int CONFOUNDER_SIZE = 8;

        /// <summary>
        /// The size of checksum in wrap and mic token for [rfc4757].
        /// </summary>
        public const int CHECKSUM_SIZE_RFC1964 = 8;

        /// <summary>
        /// The size of sequence number in wrap and mic token for [rfc4757].
        /// </summary>
        public const int SEQUENCE_NUMBER_SIZE = 8;

        /// <summary>
        /// The first 8 byte-size of header in wrap and mic token for [rfc4757].
        /// </summary>
        public const int HEADER_FIRST_8_BYTE_SIZE = 8;

        /// <summary>
        /// The signature key in wrap and mic token for [rfc4757].
        /// </summary>
        public const string SIGNATURE_KEY = "signaturekey\0";

        /// <summary>
        /// The fortybits in wrap and mic token for [rfc4757].
        /// </summary>
        public const string FORTY_BITS = "fortybits\0";

        /// <summary>
        /// The tag of kerberos.
        /// </summary>
        public const byte KERBEROS_TAG = 0x60;

        /// <summary>
        /// Default max token size
        /// </summary>
        public const ushort MAX_TOKEN_SIZE = 12288;

        /// <summary>
        /// Default max signature size
        /// </summary>
        public const byte MAX_SIGNATURE_SIZE = 50;

        /// <summary>
        /// Default max signature size
        /// </summary>
        public const byte SECURITY_TRAILER_SIZE = 76;

        /// <summary>
        /// Integral size of the messages
        /// </summary>
        public const byte BLOCK_SIZE = 8;

        /// <summary>
        /// The kerberos oid (1.2.840.113554.1.2.2).
        /// </summary>
        public static byte[] GetKerberosOid()
        {
            return (byte[])KerberosOid.Clone();
        }

        public static int[] GetKerberosOidInt()
        {
            return (int[])KerberosOidInt.Clone();
        }

        private readonly static byte[] KerberosOid =
            new byte[] { 0x06, 0x09, 0x2a, 0x86, 0x48, 0x86, 0xf7, 0x12, 0x01, 0x02, 0x02 };

        private readonly static int[] KerberosOidInt = { 1, 2, 840, 113554, 1, 2, 2 };

        /// <summary>
        /// The kerberos oid (1.2.840.48018.1.2.2).
        /// </summary>
        public static byte[] GetMsKerberosOid()
        {
            return (byte[])MsKerberosOid.Clone();
        }

        public static int[] GetMsKerberosOidInt()
        {
            return (int[])MsKerberosOidInt.Clone();
        }

        private readonly static byte[] MsKerberosOid =
            new byte[] { 0x06, 0x09, 0x2a, 0x86, 0x48, 0x82, 0xf7, 0x12, 0x01, 0x02, 0x02 };

        private readonly static int[] MsKerberosOidInt = { 1, 2, 840, 48018, 1, 2, 2 };

        /// <summary>
        /// This bytes is encoded ASN.1 value of the corresponding OID.
        /// </summary>
        public static byte[] GetSpnegoOidPkt()
        {
            return (byte[])SpnegoOidPkt.Clone();
        }

        public static int[] GetSpngOidInt()
        {
            return (int[])SpngOidInt.Clone();
        }
        private readonly static byte[] SpnegoOidPkt =
            new byte[] { 0x06, 0x06, 0x2b, 0x06, 0x01, 0x05, 0x05, 0x02 };

        private readonly static int[] SpngOidInt = { 1, 3, 6, 1, 5, 5, 2 };

        public enum OidPkt
        {
            MSKerberosToken,
            KerberosToken
        }

        public enum GSSToken
        {
            GSSAPI,
            GSSSPNG
        }
        #endregion token

        #region encryption and decryption
        /// <summary>
        /// (8 bits) The length of byte in bits
        /// </summary>
        public const int BYTE_SIZE = 8;

        /// <summary>
        /// (8 bytes = 64 bits) Size of DES encryption block
        /// </summary>
        public const int DES_BLOCK_SIZE = 8;
        #endregion encryption and decryption
    }
}
