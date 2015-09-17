// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp
{
    /// <summary>
    /// the utility of Nlmp common utility functions.
    /// </summary>
    public static class NlmpUtility
    {
        #region Fields and Consts

        /// <summary>
        /// Specifies the maximum size of the signature created by the MakeSignature function. This member must
        /// be zero if integrity services are not requested or available.
        /// </summary>
        internal const uint NLMP_MAX_SIGNATURE_SIZE = 16;

        /// <summary>
        /// Size of the security trailer to be appended to messages. This member should be zero if the relevant 
        /// services are not requested or available.
        /// </summary>
        internal const uint NLMP_SECURITY_TRAILER_SIZE = 0x10;

        /// <summary>
        /// the size of the signature that defined in NlmpHeader.
        /// </summary>
        internal const int NLMP_HEADER_SIGNATURE_SIZE = 8;

        /// <summary>
        /// the size of the messageType that defined in NlmpHeader.
        /// </summary>
        private const int NLMP_HEADER_MESSAGETYPE_SIZE = 4;

        /// <summary>
        /// the size of the NlmpHeader structure.
        /// </summary>
        internal const int NLMP_HEADER_SIZE = NLMP_HEADER_SIGNATURE_SIZE + NLMP_HEADER_MESSAGETYPE_SIZE;

        /// <summary>
        /// the constant size of the Negotiate packet.
        /// this size does not include the payload length which is variant.
        /// </summary>
        internal const int NEGOTIATE_MESSAGE_CONST_SIZE =
            4 + // NegotiateFlags
            8 + // DomainNameFields
            8 + // WorkstationFields
            8   // Version
            ;

        /// <summary>
        /// the constant size of the Challenge packet.
        /// this size does not include the payload length which is variant.
        /// </summary>
        internal const int CHALLENGE_MESSAGE_CONST_SIZE =
            8 + // TargetNameFields
            4 + // NegotiateFlags
            8 + // ServerChallenge
            8 + // Reserved
            8 + // TargetInfoFields
            8   // Version
            ;

        /// <summary>
        /// the constant size of the Authenticate packet.
        /// this size does not include the payload length which is variant.
        /// </summary>
        internal const int AUTHENTICATE_MESSAGE_CONST_SIZE =
            8 + // LmChallengeResponseFields
            8 + // NtChallengeResponseFields
            8 + // DomainNameFields
            8 + // UserNameFields
            8 + // WorkstationFields
            8 + // EncryptedRandomSessionKeyFields
            4 + // NegotiateFlags
            8   // Version
            + AUTHENTICATE_MESSAGE_MIC_CONST_SIZE;

        /// <summary>
        /// A 256-bit random number created at computer startup to identify the calling machine.
        /// </summary>
        internal const int MACHINE_ID_SIZE = 32;

        /// <summary>
        /// the const size of mic in authenticate message, MIC.
        /// </summary>
        internal const int AUTHENTICATE_MESSAGE_MIC_CONST_SIZE = 16;

        /// <summary>
        /// the encrypt salt for lm key generation.
        /// </summary>
        private const string LM_KEY_ENCRYPT_SALT = "KGS!@#$%";

        /// <summary>
        /// the mode of client when key is generate.
        /// </summary>
        private const string KEY_MODE_CLIENT = "Client";

        /// <summary>
        /// the mode of server when key is generate.
        /// </summary>
        private const string KEY_MODE_SERVER = "Server";

        /// <summary>
        /// session key to client-to-server sealing key magic constant 
        /// </summary>
        private const string CLIENT_TO_SERVER_SEALING_KEY_MAGIC =
            "session key to client-to-server sealing key magic constant\0";

        /// <summary>
        /// session key to server-to-client sealing key magic constant
        /// </summary>
        private const string SERVER_TO_CLIENT_SEALING_KEY_MAGIC =
            "session key to server-to-client sealing key magic constant\0";

        /// <summary>
        /// session key to client-to-server signing key magic constant
        /// </summary>
        private const string CLIENT_TO_SERVER_SIGNING_KEY_MAGIC =
            "session key to client-to-server signing key magic constant\0";

        /// <summary>
        /// session key to server-to-client signing key magic constant
        /// </summary>
        private const string SERVER_TO_CLIENT_SIGNING_KEY_MAGIC =
            "session key to server-to-client signing key magic constant\0";

        /// <summary>
        /// this field stores the next value of handle.
        /// when the RC4Init(H, K) are invoked, the handle must be updated. sdk just increase it
        /// normally.
        /// </summary>
        private static int nextHandle;

        /// <summary>
        /// the map between a handle and a key.
        /// RC4Init(H, K) will add a pair to this map, like rc4HandleKeyMap[H] = K. then, RC4(H, D) is equal to
        /// RC4(K, D) in the same security context.
        /// </summary>
        private static Dictionary<int, ICryptoTransform> rc4HandleKeyMap = new Dictionary<int, ICryptoTransform>();

        #endregion

        #region Public Methods

        /// <summary>
        /// An auxiliary function that is used to manage AV pairs in NTLM messages.
        /// </summary>
        /// <param name="avPairs">the avPairs collection hold the avPairs.this param can not be null.</param>
        /// <param name="avId"> A 16-bit unsigned integer that defines the information type in the Value field.</param>
        /// <param name="avLen"> A 16-bit unsigned integer that defines the length, in bytes, of Value.</param>
        /// <param name="value"> 
        /// A variable-length byte-array that contains the value defined for this AV pair entry. This param can be null.
        /// </param>
        /// <exception cref="ArgumentNullException">the param avPairs can not be null</exception>
        public static void AddAVPair(
            ICollection<AV_PAIR> avPairs,
            AV_PAIR_IDs avId,
            ushort avLen,
            byte[] value
            )
        {
            if (avPairs == null)
            {
                throw new ArgumentNullException("avPairs");
            }

            AV_PAIR pair = new AV_PAIR();
            pair.AvId = avId;
            pair.AvLen = avLen;
            if (value != null)
            {
                pair.Value = ArrayUtility.SubArray<byte>(value, 0, value.Length);
            }

            // if contains the end avPair, insert this new item before it.
            if (AvPairContains(avPairs, AV_PAIR_IDs.MsvAvEOL))
            {
                AV_PAIR endPair = new AV_PAIR();
                endPair.AvId = AV_PAIR_IDs.MsvAvEOL;
                avPairs.Remove(endPair);

                avPairs.Add(pair);

                avPairs.Add(endPair);
            }
            else
            {
                avPairs.Add(pair);
            }
        }


        /// <summary>
        /// Indicates the left-to-right concatenation of the string parameters, from the first string to the Nnth. Any
        /// numbers are converted to strings and all numeric conversions to strings retain all digits, even 
        /// no significant ones. The result is a string.
        /// </summary>
        /// <param name="strs">strings to concatenation</param>
        /// <returns>return the concatenation of string array</returns>
        public static string ConcatenationOf(
            params string[] strs
            )
        {
            if (strs == null)
            {
                return string.Empty;
            }

            string ret = "";

            foreach (string item in strs)
            {
                if (item == null)
                {
                    continue;
                }
                ret += item;
            }

            return ret;
        }


        /// <summary>
        /// Indicates the left-to-right concatenation of the string parameters, from the first string to the Nnth. Any
        /// numbers are converted to strings and all numeric conversions to strings retain all digits, even 
        /// no significant ones. The result is a string.
        /// </summary>
        /// <param name="bytes">the bytes array to concatenation</param>
        /// <returns>return the concatenation of bytes array</returns>
        public static byte[] ConcatenationOf(
            params byte[][] bytes
            )
        {
            if (bytes == null)
            {
                return null;
            }

            int size = 0;
            foreach (byte[] item in bytes)
            {
                if (item == null)
                {
                    continue;
                }
                size += item.Length;
            }

            byte[] ret = new byte[size];
            int currentIndex = 0;
            foreach (byte[] item in bytes)
            {
                if (item == null)
                {
                    continue;
                }
                Array.Copy(item, 0, ret, currentIndex, item.Length);
                currentIndex += item.Length;
            }

            return ret;
        }


        /// <summary>
        /// Indicates the computation of an N-byte cryptographic-strength random number.
        /// </summary>
        /// <param name="size">the size of nonce</param>
        /// <returns>the random nonce</returns>
        /// <exception cref="ArgumentException">the size can not be negative</exception>
        public static byte[] Nonce(
            int size
            )
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException("size", size, "size can not be negative");
            }

            byte[] buffer = new byte[size];

            Random random = new Random();
            random.NextBytes(buffer);

            return buffer;
        }


        /// <summary>
        /// Indicates the 2-byte little-endian byte order encoding of the Unicode UTF-16 representation of string. 
        /// The Byte Order Mark (BOM) is not sent over the wire.
        /// </summary>
        /// <param name="str">the string to get Unicode</param>
        /// <returns>the Unicode encoding bytes of string</returns>
        /// <remarks>this method is defined in section: 6 Appendix A: Cryptographic Operations Reference</remarks>
        /// <exception cref="ArgumentNullException">str can not be null</exception>
        public static byte[] Unicode(
            string str
            )
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            return StringGetBytes(str, true);
        }


        /// <summary>
        /// Indicates the uppercase representation of string.
        /// </summary>
        /// <param name="str">the string to get uppercase</param>
        /// <returns>the uppercase of string</returns>
        /// <exception cref="ArgumentNullException">str can not be null</exception>
        /// <remarks>this method is defined in section: 6 Appendix A: Cryptographic Operations Reference</remarks>
        public static string UpperCase(
            string str
            )
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            return str.ToUpper();
        }


        /// <summary>
        /// Indicates the creation of a byte array of length N. Each byte in the array is initialized to the value 
        /// zero.
        /// </summary>
        /// <remarks>
        /// this function is defined in TD section: 6 Appendix A: Cryptographic Operations Reference.
        /// </remarks>
        /// <param name="size">the size of zero array</param>
        /// <returns>the zero bytes array</returns>
        /// <exception cref="ArgumentException">the size can not be negative</exception>
        public static byte[] Z(
            int size
            )
        {
            if (size < 0)
            {
                throw new ArgumentException("size can not be negative", "size");
            }

            return new byte[size];
        }


        /// <summary>
        /// Indicates the retrieval of the current time as a 64-bit value, represented as the number of 100-nanosecond
        /// ticks elapsed since midnight of January 1st, 1601 (UTC).
        /// </summary>
        /// <returns>return current time</returns>
        public static ulong CurrentTime()
        {
            return (ulong)DateTime.Now.ToFileTimeUtc();
        }


        /// <summary>
        /// An auxiliary function that returns an operating system version-specific value
        /// </summary>
        /// <returns>current version</returns>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1024:UsePropertiesWhereAppropriate"
            )]
        public static VERSION GetVersion()
        {
            VERSION version = new VERSION();

            // For Windows XP SP2 and Windows Server 2003, 
            // the value of this field is WINDOWS_MAJOR_VERSION_5.
            if (Environment.OSVersion.Version.Major <= 5)
            {
                version.ProductMajorVersion = VERSION_MAJOR.WINDOWS_MAJOR_VERSION_5;
            }
            // For Windows Vista, Windows Server 2008, Windows 7, and Windows Server 2008 R2, 
            // the value of this field is WINDOWS_MAJOR_VERSION_6.
            else
            {
                version.ProductMajorVersion = VERSION_MAJOR.WINDOWS_MAJOR_VERSION_6;
            }

            // For Windows XP SP2 and Windows Server 2003, 
            // the value of this field is WINDOWS_MAJOR_VERSION_5.
            if (Environment.OSVersion.Version.Major == 5)
            {
                // For Windows XP SP2, the value of this field is WINDOWS_MINOR_VERSION_1.
                if (Environment.OSVersion.Version.Minor == 1)
                {
                    version.ProductMinorVersion = VERSION_MINOR.WINDOWS_MINOR_VERSION_1;
                }
                // For Windows Server 2003, the value of this field is WINDOWS_MINOR_VERSION_2.
                else
                {
                    version.ProductMinorVersion = VERSION_MINOR.WINDOWS_MINOR_VERSION_2;
                }
            }
            // For Windows Vista, Windows Server 2008, Windows 7, and Windows Server 2008 R2, 
            // the value of this field is WINDOWS_MINOR_VERSION_0.
            else
            {
                version.ProductMinorVersion = VERSION_MINOR.WINDOWS_MINOR_VERSION_0;
            }

            version.ProductBuild = (ushort)Environment.OSVersion.Version.Build;

            // For Windows XP SP2, Windows Server 2003, Windows Vista, Windows Server 2008, 
            // Windows 7, and Windows Server 2008 R2 the value of this field is NTLMSSP_REVISION_W2K3.
            // NTLMSSP_REVISION_W2K3 = 0x0F
            version.NTLMRevisionCurrent = VERSION_REVERSION.NTLMSSP_REVISION_W2K3;

            return version;
        }


        /// <summary>
        /// get the time from AV_PAIRs, from the node which is MsvAvTimestamp.
        /// if AV_PAIRs does not contains it, return the current time.
        /// </summary>
        /// <param name="challengeTargetInfo">
        /// the targetinfo of challenge message.if this param is null, return the system current time.if this param is
        /// not null, return the avpair which hole the MsvAvTimestamp.
        /// </param>
        /// <returns>the current time</returns>
        public static ulong GetTime(
            ICollection<AV_PAIR> challengeTargetInfo
            )
        {
            if (challengeTargetInfo != null)
            {
                foreach (AV_PAIR pair in challengeTargetInfo)
                {
                    if (pair.AvId == AV_PAIR_IDs.MsvAvTimestamp)
                    {
                        return BitConverter.ToUInt64(pair.Value, 0);
                    }
                }
            }

            return CurrentTime();
        }


        /// <summary>
        /// hold the results of calling LMOWF().
        /// </summary>
        /// <param name="version">the version of NTLM</param>
        /// <param name="domain">the domain of user</param>
        /// <param name="userName">the name of user</param>
        /// <param name="password">the password of user</param>
        /// <returns>the results of calling LMOWF().</returns>
        public static byte[] GetResponseKeyLm(
            NlmpVersion version,
            string domain,
            string userName,
            string password
            )
        {
            if (IsNtlmV1(version))
            {
                return GetResponseKeyLmV1(password);
            }
            else
            {
                return GetResponseKeyLmV2(domain, userName, password);
            }
        }


        /// <summary>
        /// Retrieve the user's LM response key from the server database (directory or local database).
        /// </summary>
        /// <param name="domain">the domain of user</param>
        /// <param name="userName">the name of user</param>
        /// <param name="password">the password of user</param>
        /// <param name="version">the version of NTLM</param>
        /// <returns>the results of calling LMOWF().</returns>
        public static byte[] LmGetKey(
            NlmpVersion version,
            string domain,
            string userName,
            string password
            )
        {
            return GetResponseKeyLm(version, domain, userName, password);
        }


        /// <summary>
        /// Computes a one-way function of the user's password to use as the response key. NTLM v1 and NTLM v2 define
        /// separate LMOWF() functions in the NTLM v1 authentication and NTLM v2 authentication sections, respectively.
        /// </summary>
        /// <param name="domain">the domain of user</param>
        /// <param name="userName">the name of user</param>
        /// <param name="password">the password of user</param>
        /// <param name="version">the version of NTLM</param>
        /// <returns>the results of calling LMOWF().</returns>
        public static byte[] LmOWF(
            NlmpVersion version,
            string domain,
            string userName,
            string password
            )
        {
            return LmGetKey(version, domain, userName, password);
        }


        /// <summary>
        /// hold the results of calling NTOWF().
        /// </summary>
        /// <param name="version">the version of NTLM</param>
        /// <param name="domain">the domain of user</param>
        /// <param name="userName">the name of user</param>
        /// <param name="password">the password of user</param>
        /// <returns>the result of calling NTOWF().</returns>
        public static byte[] GetResponseKeyNt(
            NlmpVersion version,
            string domain,
            string userName,
            string password
            )
        {
            if (IsNtlmV1(version))
            {
                return GetResponseKeyNtV1(password);
            }
            else
            {
                return GetResponseKeyNtV2(domain, userName, password);
            }
        }


        /// <summary>
        /// Retrieve the user's NT response key from the server database.
        /// </summary>
        /// <param name="domain">the domain of user</param>
        /// <param name="userName">the name of user</param>
        /// <param name="password">the password of user</param>
        /// <param name="version">the version of NTLM</param>
        /// <returns>the result of calling NTOWF().</returns>
        public static byte[] NtGetKey(
            NlmpVersion version,
            string domain,
            string userName,
            string password
            )
        {
            return GetResponseKeyNt(version, domain, userName, password);
        }


        /// <summary>
        /// Computes a one-way function of the user's password to use as the response key. NTLM v1 and NTLM v2 define
        /// separate NTOWF() functions in the NTLM v1 authentication and NTLM v2 authentication sections, respectively.
        /// </summary>
        /// <param name="domain">the domain of user</param>
        /// <param name="userName">the name of user</param>
        /// <param name="password">the password of user</param>
        /// <param name="version">the version of NTLM</param>
        /// <returns>the result of calling NTOWF().</returns>
        public static byte[] NtOWF(
            NlmpVersion version,
            string domain,
            string userName,
            string password
            )
        {
            return NtGetKey(version, domain, userName, password);
        }


        /// <summary>
        /// A function that computes the NT response, LM responses, and key exchange key from the response keys and 
        /// challenge.
        /// </summary>
        /// <param name="negotiateFlags">The client sets negotiateFlags to indicate options it supports.</param>
        /// <param name="responseKeyNT">hold the results of calling NTOWF().</param>
        /// <param name="responseKeyLM">hold the results of calling LMOWF().</param>
        /// <param name="serverChallenge">The 8-byte challenge message generated by the server.</param>
        /// <param name="clientChallenge">The 8-byte challenge message generated by the client.</param>
        /// <param name="time">The 8-byte little-endian time in GMT.</param>
        /// <param name="serverName">The TargetInfo field structure of the CHALLENGE_MESSAGE payload.</param>
        /// <param name="ntChallengeResponse">The NT response to the server challenge. Computed by the client.</param>
        /// <param name="lmChallengeResponse">The LM response to the server challenge. Computed by the client.</param>
        /// <param name="sessionBaseKey">A session key calculated from the user's password.</param>
        /// <param name="version">the version of NTLM</param>
        public static void ComputeResponse(
            NlmpVersion version,
            NegotiateTypes negotiateFlags,
            byte[] responseKeyNT,
            byte[] responseKeyLM,
            ulong serverChallenge,
            ulong clientChallenge,
            ulong time,
            byte[] serverName,
            out byte[] ntChallengeResponse,
            out byte[] lmChallengeResponse,
            out byte[] sessionBaseKey
            )
        {
            // requests usage of the NTLM v1 session security protocol.
            if (IsNtlmV1(version))
            {
                ComputeResponseV1(negotiateFlags, responseKeyNT,
                    responseKeyLM, serverChallenge, clientChallenge,
                    out ntChallengeResponse, out lmChallengeResponse, out sessionBaseKey);
            }
            else
            {
                ComputeResponseV2(responseKeyNT,
                    responseKeyLM, serverChallenge, clientChallenge, time, serverName,
                    out ntChallengeResponse, out lmChallengeResponse, out sessionBaseKey);
            }
        }


        /// <summary>
        /// A function that computes the NT response, LM responses, and key exchange key from the response keys and 
        /// challenge.version 1.
        /// </summary>
        /// <param name="negotiateFlags">The client sets negotiateFlags to indicate options it supports.</param>
        /// <param name="responseKeyNT">hold the results of calling NTOWF().</param>
        /// <param name="responseKeyLM">hold the results of calling LMOWF().</param>
        /// <param name="serverChallenge">The 8-byte challenge message generated by the server.</param>
        /// <param name="clientChallenge">The 8-byte challenge message generated by the client.</param>
        /// <param name="ntChallengeResponse">The NT response to the server challenge. Computed by the client.</param>
        /// <param name="lmChallengeResponse">The LM response to the server challenge. Computed by the client.</param>
        /// <param name="sessionBaseKey">A session key calculated from the user's password.</param>
        public static void ComputeResponseV1(
            NegotiateTypes negotiateFlags,
            byte[] responseKeyNT,
            byte[] responseKeyLM,
            ulong serverChallenge,
            ulong clientChallenge,
            out byte[] ntChallengeResponse,
            out byte[] lmChallengeResponse,
            out byte[] sessionBaseKey
            )
        {
            ntChallengeResponse = null;
            lmChallengeResponse = null;
            sessionBaseKey = null;

            if (IsLm(negotiateFlags))
            {
                lmChallengeResponse = DESL(responseKeyLM, SecurityUInt64GetBytes(serverChallenge));
            }
            else if (IsExtendedSessionSecurity(negotiateFlags))
            {
                byte[] source = MD5(
                    ConcatenationOf(SecurityUInt64GetBytes(serverChallenge), SecurityUInt64GetBytes(clientChallenge)));

                // use the first 8 bytes as the data of DESL.
                ntChallengeResponse = DESL(responseKeyNT, ArrayUtility.SubArray<byte>(source, 0, 8));
                // fill the last 16 bytes with zero.
                lmChallengeResponse = ConcatenationOf(SecurityUInt64GetBytes(clientChallenge), Z(16));
            }
            // NTLM with none extended session security.
            else
            {
                ntChallengeResponse = DESL(responseKeyNT, SecurityUInt64GetBytes(serverChallenge));
                if (IsNtOnly(negotiateFlags))
                {
                    lmChallengeResponse = ArrayUtility.SubArray<byte>(
                        ntChallengeResponse, 0, ntChallengeResponse.Length);
                }
                else
                {
                    lmChallengeResponse = DESL(responseKeyLM, SecurityUInt64GetBytes(serverChallenge));
                }
            }

            sessionBaseKey = MD4(responseKeyNT);
        }


        /// <summary>
        /// A function that computes the NT response, LM responses, and key exchange key from the response keys and 
        /// challenge.version 2.
        /// </summary>
        /// <param name="responseKeyNT">hold the results of calling NTOWF().</param>
        /// <param name="responseKeyLM">hold the results of calling LMOWF().</param>
        /// <param name="serverChallenge">The 8-byte challenge message generated by the server.</param>
        /// <param name="clientChallenge">The 8-byte challenge message generated by the client.</param>
        /// <param name="time">The 8-byte little-endian time in GMT.</param>
        /// <param name="serverName">The TargetInfo field structure of the CHALLENGE_MESSAGE payload.</param>
        /// <param name="ntChallengeResponse">The NT response to the server challenge. Computed by the client.</param>
        /// <param name="lmChallengeResponse">The LM response to the server challenge. Computed by the client.</param>
        /// <param name="sessionBaseKey">A session key calculated from the user's password.</param>
        public static void ComputeResponseV2(
            byte[] responseKeyNT,
            byte[] responseKeyLM,
            ulong serverChallenge,
            ulong clientChallenge,
            ulong time,
            byte[] serverName,
            out byte[] ntChallengeResponse,
            out byte[] lmChallengeResponse,
            out byte[] sessionBaseKey
            )
        {
            ntChallengeResponse = null;
            lmChallengeResponse = null;
            sessionBaseKey = null;

            // the current version of the challenge response type. This MUST be 0x01.
            byte[] responserVersion = new byte[] { 0x01 };
            // the maximum supported version of the challenge response type. This MUST be 0x01.
            byte[] hiResponserVersion = new byte[] { 0x01 };

            byte[] resonseData = ConcatenationOf(
                responserVersion,
                hiResponserVersion,
                // 48 bit reserved zero bytes
                Z(6),
                SecurityUInt64GetBytes(time),
                SecurityUInt64GetBytes(clientChallenge),
                // 32 bit reserved zero bytes.
                Z(4),
                serverName,
                // 32 bit reserved zero bytes.
                Z(4)
                );

            byte[] ntProofStr = HMAC_MD5(
                responseKeyNT, ConcatenationOf(SecurityUInt64GetBytes(serverChallenge), resonseData));

            ntChallengeResponse = ConcatenationOf(ntProofStr, resonseData);
            lmChallengeResponse = ConcatenationOf(
                HMAC_MD5(responseKeyLM,
                    ConcatenationOf(SecurityUInt64GetBytes(serverChallenge), SecurityUInt64GetBytes(clientChallenge))
                ),
                SecurityUInt64GetBytes(clientChallenge));

            sessionBaseKey = HMAC_MD5(responseKeyNT, ntProofStr);
        }


        /// <summary>
        /// The client's encrypted random session key.
        /// </summary>
        /// <param name="negotiateFlags">The client sets negotiateFlags to indicate options it supports.</param>
        /// <param name="sessionBaseKey">A session key calculated from the user's password.</param>
        /// <param name="lmChallengeResponse">The LM response to the server challenge. Computed by the client.</param>
        /// <param name="responseKeyLM">hold the results of calling LMOWF().</param>
        /// <param name="serverChallenge">The 8-byte challenge message generated by the server.</param>
        /// <param name="encryptedRandomSessionKey">the encrypted random session key.</param>
        /// <param name="exportedSessionKey">
        /// A 128-bit (16-byte) session key used to derive ClientSigningKey, ClientSealingKey, ServerSealingKey, and 
        /// ServerSigningKey.
        /// </param>
        /// <param name="keyExchangeKey">hold the results of calling KXKEY.</param>
        /// <param name="version">the version of NTLM</param>
        public static void GetEncryptedRandomSessionKey(
            NlmpVersion version,
            NegotiateTypes negotiateFlags,
            byte[] sessionBaseKey,
            byte[] lmChallengeResponse,
            byte[] responseKeyLM,
            ulong serverChallenge,
            out byte[] encryptedRandomSessionKey,
            out byte[] keyExchangeKey,
            out byte[] exportedSessionKey
            )
        {
            encryptedRandomSessionKey = null;
            keyExchangeKey = null;
            exportedSessionKey = null;

            keyExchangeKey = KXKey(
                version, sessionBaseKey, lmChallengeResponse, serverChallenge, negotiateFlags, responseKeyLM);
            if (IsKeyExch(negotiateFlags))
            {
                // set the exportedSessionKey to a random 16 bytes.
                exportedSessionKey = Nonce(16);
                encryptedRandomSessionKey = RC4(keyExchangeKey, exportedSessionKey);
            }
            else
            {
                exportedSessionKey = keyExchangeKey;
                encryptedRandomSessionKey = Nil();
            }
        }


        /// <summary>
        /// get the message integrity. when connectionless, negotiate is null.
        /// </summary>
        /// <param name="exportedSessionKey">
        /// A 128-bit (16-byte) session key used to derive ClientSigningKey, ClientSealingKey, ServerSealingKey, and 
        /// ServerSigningKey.
        /// </param>
        /// <param name="negotiate">the negotiate message</param>
        /// <param name="challenge">the challenge message</param>
        /// <param name="authenticate">the authenticate message, the mic field should be null.</param>
        /// <returns>return the mic</returns>
        /// <exception cref="System.ArgumentException">the mic of authenticate should be 0.</exception>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1011:ConsiderPassingBaseTypesAsParameters"
            )]
        public static byte[] GetMic(
            byte[] exportedSessionKey,
            NlmpNegotiatePacket negotiate,
            NlmpChallengePacket challenge,
            NlmpAuthenticatePacket authenticate
            )
        {
            if (authenticate.Payload.MIC == null)
            {
                throw new ArgumentException(
                    "mic of authenticate must be a 16 bytes array. actual value is null!", "authenticate");
            }
            foreach (byte bit in authenticate.Payload.MIC)
            {
                // each byte of the mic must be 0.
                if (bit != 0)
                {
                    throw new ArgumentException("the mic of authenticate should be null!", "authenticate");
                }
            }

            if (negotiate == null)
            {
                return HMAC_MD5(
                    exportedSessionKey,
                    ConcatenationOf(challenge.ToBytes(), authenticate.ToBytes()));
            }
            else
            {
                return HMAC_MD5(
                    exportedSessionKey,
                    ConcatenationOf(negotiate.ToBytes(), challenge.ToBytes(), authenticate.ToBytes()));
            }
        }


        /// <summary>
        /// Produces a key exchange key from the session base key, LM response and server challenge as defined in the 
        /// sections KXKEY, SIGNKEY, and SEALKEY.
        /// </summary>
        /// <param name="sessionBaseKey">A session key calculated from the user's password.</param>
        /// <param name="lmChallengeResponse">The LM response to the server challenge. Computed by the client.</param>
        /// <param name="serverChallenge">The 8-byte challenge message generated by the server.</param>
        /// <param name="negotiateFlags">Defined in section 3.1.1.</param>
        /// <param name="responseKeyLM">hold the results of calling LMOWF().</param>
        /// <param name="version">the version of NTLM</param>
        /// <returns>KeyExchangeKey - The Key Exchange Key.</returns>
        public static byte[] KXKey(
            NlmpVersion version,
            byte[] sessionBaseKey,
            byte[] lmChallengeResponse,
            ulong serverChallenge,
            NegotiateTypes negotiateFlags,
            byte[] responseKeyLM
            )
        {
            if (IsNtlmV1(version))
            {
                if (IsExtendedSessionSecurity(negotiateFlags))
                {
                    return KXKeyV1ExtendSecurity(sessionBaseKey, lmChallengeResponse, serverChallenge);
                }
                else
                {
                    return KXKeyV1(sessionBaseKey, lmChallengeResponse, negotiateFlags, responseKeyLM);
                }
            }
            else
            {
                return KXKeyV2(sessionBaseKey);
            }
        }


        /// <summary>
        /// Produces an encryption key from the session key as defined in sections KXKEY, SIGNKEY, and SEALKEY.
        /// </summary>
        /// <param name="negotiateFlags">Defined in section 3.1.1.</param>
        /// <param name="randomSessionKey">A randomly generated session key.</param>
        /// <param name="mode">
        /// An enum that defines the local machine performing the computation. Mode always takes the value "Client" 
        /// or "Server.
        /// </param>
        /// <returns>the generated seal key</returns>
        public static byte[] SealKey(
            NegotiateTypes negotiateFlags,
            byte[] randomSessionKey,
            string mode
            )
        {
            byte[] sealKey = null;

            // initialize the randomSessionKey1 to 16 bytes zero.
            byte[] randomSessionKey1 = new byte[7];
            // copy the first 7 bytes to randomSessionKey1
            Array.Copy(randomSessionKey, randomSessionKey1, randomSessionKey1.Length);

            // initialize the randomSessionKey2 to 16 bytes zero.
            byte[] randomSessionKey2 = new byte[5];
            // copy the first 5 bytes to randomSessionKey2.
            Array.Copy(randomSessionKey, randomSessionKey2, randomSessionKey2.Length);

            if (IsExtendedSessionSecurity(negotiateFlags))
            {
                if (IsNegotiate128(negotiateFlags))
                {
                    sealKey = randomSessionKey;
                }
                else if (IsNegotiate56(negotiateFlags))
                {
                    sealKey = randomSessionKey1;
                }
                else
                {
                    sealKey = randomSessionKey2;
                }

                if (mode == KEY_MODE_CLIENT)
                {
                    byte[] magic = StringGetBytes(CLIENT_TO_SERVER_SEALING_KEY_MAGIC, false);

                    sealKey = MD5(ConcatenationOf(sealKey, magic));
                }
                else if (mode == KEY_MODE_SERVER)
                {
                    byte[] magic = StringGetBytes(SERVER_TO_CLIENT_SEALING_KEY_MAGIC, false);

                    sealKey = MD5(ConcatenationOf(sealKey, magic));
                }
                else
                {
                    throw new ArgumentException(string.Format("the mode:{0} is not defined!", mode), "mode");
                }
            }
            else if (IsLmKey(negotiateFlags))
            {
                if (IsNegotiate56(negotiateFlags))
                {
                    // add 0xA0 to the end of randomSessionKey1
                    sealKey = ConcatenationOf(randomSessionKey1, new byte[] { 0xA0 });
                }
                else
                {
                    // add 0xE5, 0x38, 0xB0 to the end of randomSessionKey2
                    sealKey = ConcatenationOf(randomSessionKey2, new byte[] { 0xE5, 0x38, 0xB0 });
                }
            }
            else
            {
                sealKey = randomSessionKey;
            }

            return sealKey;
        }


        /// <summary>
        /// Produces a signing key from the session key as defined in sections KXKEY, SIGNKEY, and SEALKEY.
        /// </summary>
        /// <param name="negotiateFlags">Defined in section 3.1.1.</param>
        /// <param name="randomSessionKey">A randomly generated session key.</param>
        /// <param name="mode">
        /// An enum that defines the local machine performing the computation. Mode always takes the value "Client" 
        /// or "Server.
        /// </param>
        /// <returns>the generated sign key</returns>
        public static byte[] SignKey(
            NegotiateTypes negotiateFlags,
            byte[] randomSessionKey,
            string mode
            )
        {
            byte[] signKey = null;

            if (IsExtendedSessionSecurity(negotiateFlags))
            {
                if (mode == KEY_MODE_CLIENT)
                {
                    byte[] magic = StringGetBytes(CLIENT_TO_SERVER_SIGNING_KEY_MAGIC, false);

                    signKey = MD5(ConcatenationOf(randomSessionKey, magic));
                }
                else
                {
                    byte[] magic = StringGetBytes(SERVER_TO_CLIENT_SIGNING_KEY_MAGIC, false);

                    signKey = MD5(ConcatenationOf(randomSessionKey, magic));
                }
            }
            else
            {
                signKey = Nil();
            }

            return signKey;
        }


        /// <summary>
        /// Sign message
        /// </summary>
        /// <param name="negotiateFlags">the negotiate negotiateFlags</param>
        /// <param name="handle">
        /// The handle to a key state structure corresponding to the current state of the SealingKey
        /// </param>
        /// <param name="signingKey">The key used to sign the message.</param>
        /// <param name="seqNum">A 4-byte sequence number</param>
        /// <param name="message">The message being sent between the client and server.</param>
        /// <param name="version">the version of NTLM</param>
        /// <returns>Signed message</returns>
        public static byte[] Sign(
            NlmpVersion version,
            NegotiateTypes negotiateFlags,
            int handle,
            byte[] signingKey,
            ref int seqNum,
            byte[] message
            )
        {
            return ConcatenationOf(message, Mac(version, negotiateFlags, handle, signingKey, ref seqNum, message));
        }


        /// <summary>
        /// to seal message
        /// </summary>
        /// <param name="negotiateFlags">the negotiate negotiateFlags</param>
        /// <param name="handle">
        /// The handle to a key state structure corresponding to the current state of the SealingKey
        /// </param>
        /// <param name="signingKey">The key used to sign the message.</param>
        /// <param name="seqNum">A 4-byte sequence number</param>
        /// <param name="message">The message being sent between the client and server.</param>
        /// <param name="sealedMessage">The encrypted message</param>
        /// <param name="signature">The checksum of the Sealed message</param>
        /// <param name="version">the version of NTLM</param>
        public static void Seal(
            NlmpVersion version,
            NegotiateTypes negotiateFlags,
            int handle,
            byte[] signingKey,
            ref int seqNum,
            byte[] message,
            out byte[] sealedMessage,
            out byte[] signature
            )
        {
            sealedMessage = RC4(handle, message);
            signature = Mac(version, negotiateFlags, handle, signingKey, ref seqNum, message);
        }


        /// <summary>
        /// to unseal message
        /// </summary>
        /// <param name="negotiateFlags">the negotiate negotiateFlags</param>
        /// <param name="handle">
        /// The handle to a key state structure corresponding to the current state of the SealingKey
        /// </param>
        /// <param name="signingKey">The key used to sign the message.</param>
        /// <param name="seqNum">A 4-byte sequence number</param>
        /// <param name="sealedMessage">The encrypted message</param>
        /// <param name="message">The message being sent between the client and server.</param>
        /// <param name="signature">The checksum of the Sealed message</param>
        /// <param name="version">the version of NTLM</param>
        public static void UnSeal(
            NlmpVersion version,
            NegotiateTypes negotiateFlags,
            int handle,
            byte[] signingKey,
            ref int seqNum,
            byte[] sealedMessage,
            out byte[] message,
            out byte[] signature
            )
        {
            message = RC4(handle, sealedMessage);
            signature = Mac(version, negotiateFlags, handle, signingKey, ref seqNum, message);
        }


        /// <summary>
        /// the mac function
        /// </summary>
        /// <param name="negotiateFlags">the negotiate negotiateFlags</param>
        /// <param name="handle">
        /// The handle to a key state structure corresponding to the current state of the SealingKey
        /// </param>
        /// <param name="signingKey">The key used to sign the message.</param>
        /// <param name="seqNum">A 4-byte sequence number</param>
        /// <param name="message">The message being sent between the client and server.</param>
        /// <param name="version">the version of NTLM</param>
        /// <returns>the signature</returns>
        public static byte[] Mac(
            NlmpVersion version,
            NegotiateTypes negotiateFlags,
            int handle,
            byte[] signingKey,
            ref int seqNum,
            byte[] message
            )
        {
            // requests usage of the NTLM v1 session security protocol.
            if (IsNtlmV1(version) && !IsExtendedSessionSecurity(negotiateFlags))
            {
                NTLMSSP_MESSAGE_SIGNATURE signature = new NTLMSSP_MESSAGE_SIGNATURE();
                MacV1(negotiateFlags, handle, ref seqNum, message, ref signature);
                return StructGetBytes(signature);
            }
            else
            {
                NTLMSSP_MESSAGE_SIGNATURE_Extended signature = new NTLMSSP_MESSAGE_SIGNATURE_Extended();
                MacV2(negotiateFlags, handle, signingKey, ref seqNum, message, ref signature);
                return StructGetBytes(signature);
            }
        }


        /// <summary>
        /// decode nlmp packet from bytes data
        /// </summary>
        /// <param name="nlmpPacketBytesData">the nlmp packet data</param>
        /// <returns>the decoded nlmp packet</returns>
        /// <exception cref="InvalidOperationException">
        /// if the data is corrupt, we can not decode the packet,we will throw this exception.
        /// </exception>
        public static NlmpPacket DecodePacket(
            byte[] nlmpPacketBytesData
            )
        {
            if (nlmpPacketBytesData == null)
            {
                return null;
            }

            // decode the message header
            NlmpEmptyPacket empty = new NlmpEmptyPacket(nlmpPacketBytesData);

            NlmpPacket packet = null;

            switch (empty.Header.MessageType)
            {
                case MessageType_Values.NEGOTIATE:
                    packet = new NlmpNegotiatePacket(nlmpPacketBytesData);
                    break;
                case MessageType_Values.CHALLENGE:
                    packet = new NlmpChallengePacket(nlmpPacketBytesData);
                    break;
                case MessageType_Values.AUTHENTICATE:
                    packet = new NlmpAuthenticatePacket(nlmpPacketBytesData);
                    break;
            }

            if (packet == null)
            {
                throw new InvalidOperationException(
                    string.Format("the message type of packet is invalid! MessageType = {0}",
                    empty.Header.MessageType));
            }

            return packet;
        }


        /// <summary>
        /// is the nlmp version is NTLM v2
        /// </summary>
        /// <param name="version">the version of NTLM</param>
        /// <returns>if the version 2 is selected, return true; otherwise, false.</returns>
        public static bool IsNtlmV2(
            NlmpVersion version
            )
        {
            return version == NlmpVersion.v2;
        }


        /// <summary>
        /// convert collection of AV_PAIR to bytes.
        /// </summary>
        /// <param name="avPairs">the collection of AV_PAIR</param>
        /// <returns>the bytes of AV_PAIR collection</returns>
        public static byte[] AvPairCollectionGetBytes(
            ICollection<AV_PAIR> avPairs
            )
        {
            if (avPairs == null)
            {
                return new byte[0];
            }
            List<byte> targetInfoBytes = new List<byte>();
            foreach (AV_PAIR pair in avPairs)
            {
                targetInfoBytes.AddRange(NlmpUtility.StructGetBytes(pair));
            }
            return targetInfoBytes.ToArray();
        }


        /// <summary>
        /// get the bytes of string
        /// </summary>
        /// <param name="data">the string data</param>
        /// <param name="isUnicode">if the encoding is Unicode</param>
        /// <returns>the encoded bytes</returns>
        public static byte[] StringGetBytes(
            string data,
            bool isUnicode
            )
        {
            if (isUnicode)
            {
                return Encoding.Unicode.GetBytes(data);
            }
            else
            {
                return Encoding.ASCII.GetBytes(data);
            }
        }


        /// <summary>
        /// The RC4 Encryption Algorithm. To obtain this stream cipher that is licensed by RSA Data Security, Inc.,
        /// contact this company. Indicates the encryption of data item D with the current session or message key
        /// state, using the RC4 algorithm. H is the handle to a key state structure initialized by RC4INIT.
        /// </summary>
        /// <param name="handle">the handle which hold the rc4 context</param>
        /// <param name="data">the data to encrypt</param>
        /// <returns>the result of RC4 algorithm</returns>
        public static byte[] RC4(
            int handle,
            byte[] data
            )
        {
            if (!rc4HandleKeyMap.ContainsKey(handle))
            {
                throw new ArgumentException(string.Format("the handle({0}) have not map to key", handle), "handle");
            }

            return rc4HandleKeyMap[handle].TransformFinalBlock(data, 0, data.Length);
        }


        /// <summary>
        /// Indicates the encryption of data item D with the key K using the RC4 algorithm.
        /// </summary>
        /// <param name="key">the key to encrypt the data</param>
        /// <param name="data">the data to encrypt</param>
        /// <returns>the result of RC4 algorithm</returns>
        public static byte[] RC4(
            byte[] key,
            byte[] data
            )
        {
            Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic.RC4 rc4 =
                Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic.RC4.Create();

            rc4.Key = key;

            return rc4.CreateEncryptor(rc4.Key, rc4.IV).TransformFinalBlock(data, 0, data.Length);
        }


        /// <summary>
        /// Initialization of the RC4 key and handle to a key state structure for the session.
        /// </summary>
        /// <param name="key">the key to initilize to handle</param>
        /// <param name="handle">the handle which hold the rc4 context</param>
        public static void RC4Init(
            int handle,
            byte[] key
            )
        {
            Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic.RC4 rc4 =
              Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic.RC4.Create();

            rc4.Key = key;

            ICryptoTransform transform = rc4.CreateEncryptor(rc4.Key, rc4.IV);

            rc4HandleKeyMap[handle] = transform;
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// A zero-length string.
        /// </summary>
        /// <returns>return a zero-length string</returns>
        private static byte[] Nil()
        {
            return StringGetBytes("", false);
        }


        /// <summary>
        /// convert the des key from 7bytes length array to 8 bytes length array.
        /// </summary>
        /// <param name="bytes">the 7 length password</param>
        /// <returns>a 8 bytes key array</returns>
        /// <exception cref="ArgumentException">the length of input bytes must be 7</exception>
        private static byte[] ConvertDESKeyFrom7BytesTo8Bytes(
            byte[] bytes
            )
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            // the des key to convert must be 7 bytes.
            if (bytes.Length != 7)
            {
                throw new ArgumentException(
                    string.Format("the length of input bytes must be 7, actually {0}", bytes.Length), "bytes");
            }

            return LMHashManaged.GenerateDesKey(bytes, 0);
        }


        /// <summary>
        /// the version 1 of LMOWF
        /// </summary>
        /// <param name="password">the password of user</param>
        /// <returns>the version 1 LM response key</returns>
        private static byte[] GetResponseKeyLmV1(
            string password
            )
        {
            // copy password array data to a 14 length array
            byte[] upperCasePassword = StringGetBytes(UpperCase(password), false);
            // copy the first 14 bytest of upperCasePassword to the passwordArray. if not enough, padding with 0.
            byte[] passwordArray = new byte[14];
            Array.Copy(upperCasePassword, passwordArray, Math.Min(passwordArray.Length, upperCasePassword.Length));

            // store the first 7 bytes to passwordKey1.
            byte[] passwordKey1 = ArrayUtility.SubArray<byte>(passwordArray, 0, 7);

            // store the second 7 bytes to passwordKey2.
            byte[] passwordKey2 = ArrayUtility.SubArray<byte>(passwordArray, 7, 7);

            // the encrypt salt
            byte[] salt = StringGetBytes(LM_KEY_ENCRYPT_SALT, false);

            // the des result
            byte[] des1 = DES(ConvertDESKeyFrom7BytesTo8Bytes(passwordKey1), salt);
            byte[] des2 = null;

            // if the password length is less than 8, the passwordKey2 is set to byte[7]{0}
            // the DES will throw exception:
            // CryptographicException: Specified key is a known weak key for 'DES' and cannot be used.
            // so, we directly set the des2 value here.
            if (password.Length < 8)
            {
                des2 = new byte[] { 0xaa, 0xd3, 0xb4, 0x35, 0xb5, 0x14, 0x04, 0xee };
            }
            // encrypt the password key when its length is not less than 8.
            else
            {
                des2 = DES(ConvertDESKeyFrom7BytesTo8Bytes(passwordKey2), salt);
            }

            return ConcatenationOf(
                ArrayUtility.SubArray<byte>(des1, 0, 8), // just take the fist 8 bytes of des1.
                ArrayUtility.SubArray<byte>(des2, 0, 8)  // just take the fist 8 bytes of des2.
                );
        }


        /// <summary>
        /// the version 2 of LMOWF
        /// </summary>
        /// <param name="domain">the domain of user</param>
        /// <param name="userName">the name of user</param>
        /// <param name="password">the password of user</param>
        /// <returns>the version 2 of LM response key</returns>
        private static byte[] GetResponseKeyLmV2(
            string domain,
            string userName,
            string password
            )
        {
            return GetResponseKeyNtV2(domain, userName, password);
        }


        /// <summary>
        /// the version 1 of NTOWF().
        /// </summary>
        /// <param name="password">the password of user</param>
        /// <returns>the version 1 of NT response key</returns>
        private static byte[] GetResponseKeyNtV1(
            string password
            )
        {
            return MD4(Unicode(password));
        }


        /// <summary>
        /// the version 2 of NTOWF().
        /// </summary>
        /// <param name="domain">the domain of user</param>
        /// <param name="userName">the name of user</param>
        /// <param name="password">the password of user</param>
        /// <returns>the version 2 of NT response key</returns>
        private static byte[] GetResponseKeyNtV2(
            string domain,
            string userName,
            string password
            )
        {
            byte[] key = MD4(Unicode(password));
            string data = ConcatenationOf(UpperCase(userName), domain);

            // userName and domain must convert to Unicode byte array
            return HMAC_MD5(key, StringGetBytes(data, true));
        }


        /// <summary>
        /// Produces a key exchange key from the session base key, LM response and server challenge as defined in the 
        /// sections KXKEY, SIGNKEY, and SEALKEY.
        /// </summary>
        /// <param name="sessionBaseKey">A session key calculated from the user's password.</param>
        /// <returns>KeyExchangeKey - The Key Exchange Key.</returns>
        private static byte[] KXKeyV2(
            byte[] sessionBaseKey
            )
        {
            return sessionBaseKey;
        }


        /// <summary>
        /// Produces a key exchange key from the session base key, LM response and server challenge as defined in the 
        /// sections KXKEY, SIGNKEY, and SEALKEY.
        /// </summary>
        /// <param name="sessionBaseKey">A session key calculated from the user's password.</param>
        /// <param name="lmChallengeResponse">The LM response to the server challenge. Computed by the client.</param>
        /// <param name="serverChallenge">The 8-byte challenge message generated by the server.</param>
        /// <returns>KeyExchangeKey - The Key Exchange Key.</returns>
        private static byte[] KXKeyV1ExtendSecurity(
            byte[] sessionBaseKey,
            byte[] lmChallengeResponse,
            ulong serverChallenge
            )
        {
            return HMAC_MD5(
                sessionBaseKey,
                ConcatenationOf(
                    SecurityUInt64GetBytes(serverChallenge),
                    ArrayUtility.SubArray<byte>(lmChallengeResponse, 0, 8) // just use the first 8 bytes of lmChallengeResponse
                ));
        }


        /// <summary>
        /// Produces a key exchange key from the session base key, LM response and server challenge as defined in the 
        /// sections KXKEY, SIGNKEY, and SEALKEY.
        /// </summary>
        /// <param name="sessionBaseKey">A session key calculated from the user's password.</param>
        /// <param name="lmChallengeResponse">The LM response to the server challenge. Computed by the client.</param>
        /// <param name="negotiateFlags">Defined in section 3.1.1.</param>
        /// <param name="responseKeyLM">hold the results of calling LMOWF().</param>
        /// <returns>KeyExchangeKey - The Key Exchange Key.</returns>
        /// <exception cref="ArgumentNullException">responseKeyLM must not be null!</exception>
        private static byte[] KXKeyV1(
            byte[] sessionBaseKey,
            byte[] lmChallengeResponse,
            NegotiateTypes negotiateFlags,
            byte[] responseKeyLM
            )
        {
            if (responseKeyLM == null)
            {
                throw new ArgumentNullException("responseKeyLM");
            }

            byte[] keyExchangeKey = null;

            // use the fist 7 bytes as the session base key.
            byte[] sessionBaseKey1 = ArrayUtility.SubArray<byte>(sessionBaseKey, 0, 7);

            // use the first 8 bytes as the lm challenge response.
            byte[] lmChallengeResponse1 = ArrayUtility.SubArray<byte>(lmChallengeResponse, 0, 8);

            // copy the responseKeyLM to LMOWF1.
            byte[] LMOWF1 = ArrayUtility.SubArray<byte>(responseKeyLM, 0, responseKeyLM.Length);

            if (IsLmKey(negotiateFlags))
            {
                keyExchangeKey = ConcatenationOf(
                    DES(
                        ConvertDESKeyFrom7BytesTo8Bytes(sessionBaseKey1), lmChallengeResponse1
                    ),
                    DES(
                        ConvertDESKeyFrom7BytesTo8Bytes(
                    // fill the first element with sessionBaseKey[7] and others with 0xBD.
                    // see TD: 3.4.5.1 KXKEY
                            new byte[] { sessionBaseKey[7], 0xBD, 0xBD, 0xBD, 0xBD, 0xBD, 0xBD }
                        ),
                        lmChallengeResponse1
                    )
                );
            }
            else
            {
                if ((negotiateFlags & NegotiateTypes.NTLMSSP_REQUEST_NON_NT_SESSION_KEY)
                    == NegotiateTypes.NTLMSSP_REQUEST_NON_NT_SESSION_KEY)
                {
                    // fill the last 8 bytes with zero.
                    keyExchangeKey = ConcatenationOf(LMOWF1, Z(8));
                }
                else
                {
                    keyExchangeKey = sessionBaseKey;
                }
            }

            return keyExchangeKey;
        }


        /// <summary>
        /// the mac function. if connectionless mode, we do not update the seqnum, (application supplied SeqNum)
        /// </summary>
        /// <param name="negotiateFlags">the negotiate negotiateFlags</param>
        /// <param name="handle">
        /// The handle to a key state structure corresponding to the current state of the SealingKey
        /// </param>
        /// <param name="seqNum">A 4-byte sequence number</param>
        /// <param name="message">The message being sent between the client and server.</param>
        /// <param name="signature">the signature of mac</param>
        /// <returns>the signature</returns>
        private static void MacV1(
            NegotiateTypes negotiateFlags,
            int handle, ref int seqNum, byte[] message,
            ref NTLMSSP_MESSAGE_SIGNATURE signature
            )
        {
            signature.Version = SignatureVersion_Values.V1;
            signature.Checksum = BytesToSecurityUInt32(CRC32(message));
            signature.RandomPad = BytesToSecurityUInt32(RC4(handle, SecurityUInt32GetBytes(signature.RandomPad)));
            signature.Checksum = BytesToSecurityUInt32(RC4(handle, SecurityUInt32GetBytes(signature.Checksum)));
            signature.SeqNum = BytesToSecurityUInt32(RC4(handle, SecurityUInt32GetBytes(0x00)));

            if (!IsConnectionless(negotiateFlags))
            {
                signature.SeqNum ^= (uint)seqNum;
                seqNum++;
            }
            else
            {
                signature.SeqNum ^= (uint)seqNum;
            }

            signature.RandomPad = 0;
        }


        /// <summary>
        /// the mac function
        /// </summary>
        /// <param name="negotiateFlags">the negotiate negotiateFlags</param>
        /// <param name="handle">
        /// The handle to a key state structure corresponding to the current state of the SealingKey
        /// </param>
        /// <param name="signingKey">The key used to sign the message.</param>
        /// <param name="seqNum">A 4-byte sequence number</param>
        /// <param name="message">The message being sent between the client and server.</param>
        /// <param name="signature">the signature of mac</param>
        /// <returns>the signature</returns>
        private static void MacV2(
            NegotiateTypes negotiateFlags,
            int handle,
            byte[] signingKey,
            ref int seqNum,
            byte[] message,
            ref NTLMSSP_MESSAGE_SIGNATURE_Extended signature
            )
        {
            signature.Version = ExtendedSignatureVersion_Values.V1;

            byte[] hmacBytes = HMAC_MD5(signingKey, ConcatenationOf(SecurityUInt32GetBytes((uint)seqNum), message));
            if (IsKeyExch(negotiateFlags))
            {
                // use the first 8 bytes of hmacBytes as the data of RC4.
                byte[] rc4Bytes = RC4(handle, ArrayUtility.SubArray<byte>(hmacBytes, 0, 8));
                hmacBytes = rc4Bytes;
            }
            // checksumBytes is the first 8bytes of hmacBytes.
            byte[] checksumBytes = ArrayUtility.SubArray<byte>(hmacBytes, 0, 8);

            signature.Checksum = BytesToSecurityUInt64(checksumBytes);

            signature.SeqNum = (uint)seqNum;
            seqNum++;
        }


        /// <summary>
        /// indicates whether the sdk is connectionless. if the connectionless flag is set in negotiateFlags, return
        /// true.
        /// </summary>
        /// <param name="negotiateFlags">the negotiateFlags of NegotiateFlags</param>
        /// <returns>if the connectionless flag is set in the negotiateFlags, return true; otherwise, false.</returns>
        internal static bool IsConnectionless(
            NegotiateTypes negotiateFlags
            )
        {
            return (negotiateFlags & NegotiateTypes.NTLMSSP_NEGOTIATE_DATAGRAM)
                            == NegotiateTypes.NTLMSSP_NEGOTIATE_DATAGRAM;
        }


        /// <summary>
        /// If set, requests 56-bit encryption.
        /// </summary>
        /// <param name="negotiateFlags">the negotiateFlags of NegotiateFlags</param>
        /// <returns>if the negotiate 56 flag is set in the negotiateFlags, return true; otherwise, false.</returns>
        private static bool IsNegotiate56(
            NegotiateTypes negotiateFlags
            )
        {
            return (negotiateFlags & NegotiateTypes.NTLMSSP_NEGOTIATE_56)
                                == NegotiateTypes.NTLMSSP_NEGOTIATE_56;
        }


        /// <summary>
        /// If set, requests 128-bit session key negotiation.
        /// </summary>
        /// <param name="negotiateFlags">the negotiateFlags of NegotiateFlags</param>
        /// <returns>if the negotiate 128 flag is set in the negotiateFlags, return true; otherwise, false.</returns>
        private static bool IsNegotiate128(
            NegotiateTypes negotiateFlags
            )
        {
            return (negotiateFlags & NegotiateTypes.NTLMSSP_NEGOTIATE_128)
                                == NegotiateTypes.NTLMSSP_NEGOTIATE_128;
        }


        /// <summary>
        /// If set, requests session key negotiation for message signatures.
        /// </summary>
        /// <param name="negotiateFlags">the negotiateFlags of NegotiateFlags</param>
        /// <returns>if the sign flag is set in the negotiateFlags, return true; otherwise, false.</returns>
        private static bool IsSign(
            NegotiateTypes negotiateFlags
            )
        {
            return (negotiateFlags & NegotiateTypes.NTLMSSP_NEGOTIATE_SIGN)
                                == NegotiateTypes.NTLMSSP_NEGOTIATE_SIGN;
        }


        /// <summary>
        /// If set, requests session key negotiation for message confidentiality.
        /// </summary>
        /// <param name="negotiateFlags">the negotiateFlags of NegotiateFlags</param>
        /// <returns>if the seal flag is set in the negotiateFlags, return true; otherwise, false.</returns>
        private static bool IsSeal(
            NegotiateTypes negotiateFlags
            )
        {
            return (negotiateFlags & NegotiateTypes.NTLMSSP_NEGOTIATE_SEAL)
                                == NegotiateTypes.NTLMSSP_NEGOTIATE_SEAL;
        }


        /// <summary>
        /// If set, requests an explicit key exchange.
        /// </summary>
        /// <param name="negotiateFlags">the negotiateFlags of NegotiateFlags</param>
        /// <returns>if the key exch flag is set in the negotiateFlags, return true; otherwise, false.</returns>
        internal static bool IsKeyExch(
            NegotiateTypes negotiateFlags
            )
        {
            if (!IsSign(negotiateFlags) && !IsSeal(negotiateFlags))
            {
                return false;
            }

            return (negotiateFlags & NegotiateTypes.NTLMSSP_NEGOTIATE_KEY_EXCH)
                            == NegotiateTypes.NTLMSSP_NEGOTIATE_KEY_EXCH;
        }


        /// <summary>
        /// If set, requests LAN Manager(LM) session key computation.
        /// </summary>
        /// <param name="negotiateFlags">the negotiateFlags of NegotiateFlags</param>
        /// <returns>if the Lm key flag is set in the negotiateFlags, return true; otherwise, false.</returns>
        private static bool IsLmKey(
            NegotiateTypes negotiateFlags
            )
        {
            return (negotiateFlags & NegotiateTypes.NTLMSSP_NEGOTIATE_LM_KEY)
                            == NegotiateTypes.NTLMSSP_NEGOTIATE_LM_KEY;
        }


        /// <summary>
        /// If set, requests usage of the NTLMv2 session security.
        /// </summary>
        /// <param name="negotiateFlags">the negotiateFlags of NegotiateFlags</param>
        /// <returns>if the extended security flag is set in the negotiateFlags, return true; otherwise, false.</returns>
        private static bool IsExtendedSessionSecurity(
            NegotiateTypes negotiateFlags
            )
        {
            return (negotiateFlags & NegotiateTypes.NTLMSSP_NEGOTIATE_EXTENDED_SESSIONSECURITY)
                            == NegotiateTypes.NTLMSSP_NEGOTIATE_EXTENDED_SESSIONSECURITY;
        }


        /// <summary>
        /// If set, LM authentication is not allowed and only NT authentication is used.
        /// </summary>
        /// <param name="negotiateFlags">the negotiateFlags of NegotiateFlags</param>
        /// <returns>if the NTOnly flag is set in the negotiateFlags, return true; otherwise, false.</returns>
        private static bool IsNtOnly(
            NegotiateTypes negotiateFlags
            )
        {
            return (negotiateFlags & NegotiateTypes.NTLMSSP_NEGOTIATE_NT_ONLY)
                            == NegotiateTypes.NTLMSSP_NEGOTIATE_NT_ONLY;
        }


        /// <summary>
        /// indicates whether the NTLMv1 is selected.
        /// </summary>
        /// <param name="version">the version of NTLM</param>
        /// <returns>if the version 1 is selected, return true; otherwise, false.</returns>
        internal static bool IsNtlmV1(
            NlmpVersion version
            )
        {
            return version == NlmpVersion.v1;
        }


        /// <summary>
        /// get the 8bytes ulong array
        /// </summary>
        /// <param name="data">the data of ulong</param>
        /// <returns>get the ulong bytes</returns>
        private static byte[] SecurityUInt64GetBytes(
            ulong data
            )
        {
            return BitConverter.GetBytes(data);
        }


        /// <summary>
        /// Indicates a 32-bit CRC calculated over M.
        /// </summary>
        /// <param name="data">the data to encrypt using CRC32</param>
        /// <returns>the result of CRC32 algorithm</returns>
        /// <exception cref="ArgumentNullException">data must not be null</exception>
        private static byte[] CRC32(
            byte[] data
            )
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            return Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic.Crc32.ComputeChecksum(data);
        }


        /// <summary>
        /// Indicates the encryption of an 8-byte data item D with the 7-byte key K using the Data Encryption Standard
        /// (DES) algorithm in Electronic Codebook (ECB) mode. The result is 8 bytes in length.
        /// </summary>
        /// <param name="key">the key using DES to encrypt</param>
        /// <param name="data">the data to encrypt</param>
        /// <returns>the result of DES algorithm</returns>
        /// <exception cref="ArgumentException">the key must be 8 bytes array</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.Cryptography", "CA5351:DESCannotBeUsed")]
        private static byte[] DES(
            byte[] key,
            byte[] data
            )
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            // the length of des key must be 8.
            if (key.Length != 8)
            {
                throw new ArgumentException(
                    string.Format("the length of key must be 8 bytes, actually {0} bytes", key.Length), "key");
            }

            DES des = System.Security.Cryptography.DES.Create();
            des.Mode = CipherMode.ECB;
            des.Key = key;

            return des.CreateEncryptor(des.Key, des.IV).TransformFinalBlock(data, 0, data.Length);
        }


        /// <summary>
        /// Indicates the encryption of an 8-byte data item D with the 16-byte key K using the Data Encryption 
        /// Standard Long (DESL) algorithm. The result is 24 bytes in length. DESL(K, D) is computed as follows.
        /// </summary>
        /// <param name="key">the key using for DESL</param>
        /// <param name="data">the data to encrypt with DESL</param>
        /// <returns>the result of DESL algorithm</returns>
        public static byte[] DESL(
            byte[] key,
            byte[] data
            )
        {
            // the fist 7 bytes is used as the key1 of DESL.
            byte[] key1 = ArrayUtility.SubArray<byte>(key, 0, 7);
            // the second 7 bytes is used as the key2 of DESL.
            byte[] key2 = ArrayUtility.SubArray<byte>(key, 7, 7);
            // the key3 is the last 2 bytes of key and 5 bytes zero.
            byte[] key3 = ConcatenationOf(ArrayUtility.SubArray<byte>(key, 14, 2), Z(5));

            byte[] des1 = DES(ConvertDESKeyFrom7BytesTo8Bytes(key1), data);
            byte[] des2 = DES(ConvertDESKeyFrom7BytesTo8Bytes(key2), data);
            byte[] des3 = DES(ConvertDESKeyFrom7BytesTo8Bytes(key3), data);

            return ConcatenationOf(
                ArrayUtility.SubArray<byte>(des1, 0, 8), // get the first 8 bytes of des1.
                ArrayUtility.SubArray<byte>(des2, 0, 8), // get the first 8 bytes of des2.
                ArrayUtility.SubArray<byte>(des3, 0, 8)  // get the first 8 bytes of des3.
                );
        }


        /// <summary>
        /// Indicates the computation of a 16-byte HMAC-keyed MD5 message digest of the byte string M using the key K.
        /// </summary>
        /// <param name="key">the key to encrypt with md5</param>
        /// <param name="data">the data encrypt with md5</param>
        /// <returns>the result of HMAC_MD5 algorithm</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.Cryptography", "CA5350:MD5CannotBeUsed")]
        private static byte[] HMAC_MD5(
            byte[] key,
            byte[] data
            )
        {
            return new HMACMD5(key).ComputeHash(data);
        }


        /// <summary>
        /// Indicates the computation of an MD4 message digest of the null-terminated byte string M ([RFC1320]).
        /// </summary>
        /// <param name="data">the data to encrypt with data</param>
        /// <returns>the result of MD4 algorithm</returns>
        private static byte[] MD4(
            byte[] data
            )
        {
            Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic.MD4 md4
                = Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic.MD4.Create();

            md4.Initialize();

            return md4.ComputeHash(data);
        }


        /// <summary>
        /// Indicates the computation of an MD5 message digest of the null-terminated byte string M ([RFC1321]).
        /// </summary>
        /// <param name="data">the data to encrypt with md5</param>
        /// <returns>the result of MD5 algorithm</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.Cryptography", "CA5350:MD5CannotBeUsed")]
        internal static byte[] MD5(
            byte[] data
            )
        {
            return System.Security.Cryptography.MD5.Create().ComputeHash(data);
        }


        #endregion

        #region Internal Methods from TD

        /// <summary>
        /// is the AvPairs contains the avId.
        /// </summary>
        /// <param name="avPairs">the container of AvPairs.</param>
        /// <param name="avId">the target avId</param>
        /// <returns>if avPairs contains avId, return true;, otherwise, return false.</returns>
        /// <exception cref="ArgumentNullException">avPairs can not be null!</exception>
        internal static bool AvPairContains(
            ICollection<AV_PAIR> avPairs,
            AV_PAIR_IDs avId
            )
        {
            if (avPairs == null)
            {
                throw new ArgumentNullException("avPairs");
            }

            foreach (AV_PAIR pair in avPairs)
            {
                if (pair.AvId == avId)
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// get the value of id
        /// </summary>
        /// <param name="avPairs">the container of AvPairs.</param>
        /// <param name="avId">the target avId</param>
        /// <returns>the value of avId</returns>
        internal static byte[] AvPairGetValue(
            ICollection<AV_PAIR> avPairs,
            AV_PAIR_IDs avId
            )
        {
            foreach (AV_PAIR pair in avPairs)
            {
                if (pair.AvId == avId)
                {
                    return pair.Value;
                }
            }

            return null;
        }


        /// <summary>
        /// update the avPairs of the avId.
        /// </summary>
        /// <param name="avPairs">the container of AvPairs.</param>
        /// <param name="avId">the target avId</param>
        /// <param name="newAvLen">the new avLen.</param>
        /// <param name="newValue">the new value</param>
        internal static void UpdateAvPair(
            ICollection<AV_PAIR> avPairs,
            AV_PAIR_IDs avId,
            ushort newAvLen,
            byte[] newValue
            )
        {
            List<AV_PAIR> swapPairs = new List<AV_PAIR>();

            foreach (AV_PAIR pair in avPairs)
            {
                if (pair.AvId == avId)
                {
                    AddAVPair(swapPairs, avId, newAvLen, newValue);
                }
                else
                {
                    swapPairs.Add(pair);
                }
            }

            avPairs.Clear();

            foreach (AV_PAIR pair in swapPairs)
            {
                avPairs.Add(pair);
            }
        }


        /// <summary>
        /// generate the next handle, using for the RC4Init(H, K) and RC4(H, D).
        /// </summary>
        internal static int NextHandle
        {
            get
            {
                return ++nextHandle;
            }
        }


        /// <summary>
        /// is the version flag is set in the negotiateFlags.
        /// </summary>
        /// <param name="negotiateFlags">the negotiateFlags of NegotiateFlags</param>
        /// <returns>if the version flag is set in the negotiateFlags, return true; otherwise, false.</returns>
        internal static bool IsVersionRequired(
            NegotiateTypes negotiateFlags
            )
        {
            return (negotiateFlags & NegotiateTypes.NTLMSSP_NEGOTIATE_VERSION)
                == NegotiateTypes.NTLMSSP_NEGOTIATE_VERSION;
        }


        /// <summary>
        /// is the Unicode flag is set in the negotiateFlags.
        /// </summary>
        /// <param name="negotiateFlags">the negotiateFlags of NegotiateFlags</param>
        /// <returns>if the Unicode flag is set in the negotiateFlags, return true; otherwise, false.</returns>
        internal static bool IsUnicode(
            NegotiateTypes negotiateFlags
            )
        {
            return (negotiateFlags & NegotiateTypes.NTLMSSP_NEGOTIATE_UNICODE)
                == NegotiateTypes.NTLMSSP_NEGOTIATE_UNICODE;
        }


        /// <summary>
        /// is the domainName flag is set in the negotiateFlags.
        /// </summary>
        /// <param name="negotiateFlags">the negotiateFlags of NegotiateFlags</param>
        /// <returns>if the domainName flag is set in the negotiateFlags, return true; otherwise, false.</returns>
        internal static bool IsDomainNameSupplied(
            NegotiateTypes negotiateFlags
            )
        {
            return (negotiateFlags & NegotiateTypes.NTLMSSP_NEGOTIATE_OEM_DOMAIN_SUPPLIED)
                == NegotiateTypes.NTLMSSP_NEGOTIATE_OEM_DOMAIN_SUPPLIED;
        }


        /// <summary>
        /// is the DomainType flag is set in the negotiateFlags.
        /// </summary>
        /// <param name="negotiateFlags">the negotiateFlags of NegotiateFlags</param>
        /// <returns>if the DomainType flag is set in the negotiateFlags, return true; otherwise, false.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal static bool IsDomainType(
            NegotiateTypes negotiateFlags
            )
        {
            return (negotiateFlags & NegotiateTypes.NTLMSSP_TARGET_TYPE_DOMAIN)
                == NegotiateTypes.NTLMSSP_TARGET_TYPE_DOMAIN;
        }


        /// <summary>
        /// is the ServerType flag is set in the negotiateFlags.
        /// </summary>
        /// <param name="negotiateFlags">the negotiateFlags of NegotiateFlags</param>
        /// <returns>if the ServerType flag is set in the negotiateFlags, return true; otherwise, false.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal static bool IsServerType(
            NegotiateTypes negotiateFlags
            )
        {
            return (negotiateFlags & NegotiateTypes.NTLMSSP_TARGET_TYPE_SERVER)
                == NegotiateTypes.NTLMSSP_TARGET_TYPE_SERVER;
        }


        /// <summary>
        /// is the Workstation flag is set in the negotiateFlags.
        /// </summary>
        /// <param name="negotiateFlags">the negotiateFlags of NegotiateFlags</param>
        /// <returns>if the Workstation flag is set in the negotiateFlags, return true; otherwise, false.</returns>
        internal static bool IsWorkstationSupplied(
            NegotiateTypes negotiateFlags
            )
        {
            return (negotiateFlags & NegotiateTypes.NTLMSSP_NEGOTIATE_OEM_WORKSTATION_SUPPLIED)
                == NegotiateTypes.NTLMSSP_NEGOTIATE_OEM_WORKSTATION_SUPPLIED;
        }


        /// <summary>
        /// whether the nlmp version is LM.
        /// </summary>
        /// <param name="negotiateFlags">the negotiateFlags of NegotiateFlags</param>
        /// <returns>if the LmKey flag is set in the negotiateFlags, return true; otherwise, false.</returns>
        internal static bool IsLm(
            NegotiateTypes negotiateFlags
            )
        {
            if (IsLmKey(negotiateFlags) && !IsNtOnly(negotiateFlags) && !IsExtendedSessionSecurity(negotiateFlags))
            {
                throw new NotSupportedException("the LM version of NLMP is not supported!");
            }

            return false;
        }


        /// <summary>
        /// convert bytes to collection of AV_PAIR.
        /// </summary>
        /// <param name="avPairsBytes">the bytes of AV_PAIR collection</param>
        /// <returns>the collection of AV_PAIR</returns>
        internal static ICollection<AV_PAIR> BytesGetAvPairCollection(
            byte[] avPairsBytes
            )
        {
            if (avPairsBytes == null)
            {
                return null;
            }

            Collection<AV_PAIR> pairs = new Collection<AV_PAIR>();

            int start = 0;

            while (start < avPairsBytes.Length)
            {
                AV_PAIR_IDs avId = (AV_PAIR_IDs)BytesToStruct<ushort>(avPairsBytes, ref start);
                ushort avLen = BytesToStruct<ushort>(avPairsBytes, ref start);

                byte[] value = null;
                if (avLen > 0)
                {
                    value = ReadBytes(avPairsBytes, ref start, avLen);
                }

                AddAVPair(pairs, avId, avLen, value);
            }

            return pairs;
        }


        /// <summary>
        /// read size bytes from start of data.
        /// </summary>
        /// <param name="data">the bytes</param>
        /// <param name="start">the start index</param>
        /// <param name="size">the length of bytes</param>
        /// <returns>the readed bytes</returns>
        /// <exception cref="ArgumentNullException">data must not be null!</exception>
        internal static byte[] ReadBytes(
            byte[] data,
            ref int start,
            int size
            )
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            int consumedSize = 0;
            byte[] ret = ReadBytes(data, start, size, out consumedSize);

            if (consumedSize > 0)
            {
                start += consumedSize;
            }

            return ret;
        }


        /// <summary>
        /// read size bytes from start of data.
        /// </summary>
        /// <param name="data">the bytes</param>
        /// <param name="start">the start index</param>
        /// <param name="size">the length of bytes</param>
        /// <param name="consumedSize">the consumed size</param>
        /// <returns>the readed bytes</returns>
        internal static byte[] ReadBytes(
            byte[] data,
            int start,
            int size,
            out int consumedSize
            )
        {
            consumedSize = 0;

            if (start + size > data.Length)
            {
                return null;
            }

            if (size == 0)
            {
                return null;
            }

            byte[] ret = ArrayUtility.SubArray<byte>(data, start, size);

            consumedSize = size;

            return ret;
        }


        /// <summary>
        /// convert bytes to struct
        /// </summary>
        /// <typeparam name="T">the type of struct</typeparam>
        /// <param name="structInBytes">the source data</param>
        /// <param name="start">
        /// the start of data to convert.
        /// if success, add start with the length of structure.
        /// </param>
        /// <returns>the object</returns>
        internal static T BytesToStruct<T>(
            byte[] structInBytes,
            ref int start
            )
        {
            int consumedSize = 0;

            T obj = BytesToStruct<T>(structInBytes, start, out consumedSize);

            if (consumedSize > 0)
            {
                start += consumedSize;
            }

            return obj;
        }


        /// <summary>
        /// convert bytes to struct
        /// </summary>
        /// <typeparam name="T">the type of struct</typeparam>
        /// <param name="structInBytes">the source data</param>
        /// <param name="start">the start of data to convert.</param>
        /// <param name="consumedSize">
        /// set the consumedSize size to 0, if success, set to the length of structure.
        /// </param>
        /// <returns>the object</returns>
        internal static T BytesToStruct<T>(
            byte[] structInBytes,
            int start,
            out int consumedSize
            )
        {
            // set to 0
            consumedSize = 0;

            // the object to convert
            T obj = default(T);

            // get size of type
            int size = Marshal.SizeOf(typeof(T));

            // is the data is enough
            if (start + size > structInBytes.Length)
            {
                return obj;
            }

            // alloc memory for structure
            IntPtr intPtr = Marshal.AllocHGlobal(size);

            // copy data to memory
            Marshal.Copy(structInBytes, start, intPtr, size);

            // convert data to structure
            obj = (T)Marshal.PtrToStructure(intPtr, typeof(T));

            // free the alloced memory
            Marshal.FreeHGlobal(intPtr);

            // update the comsumed size of data
            consumedSize = size;

            return obj;
        }


        /// <summary>
        /// get the bytes of struct
        /// </summary>
        /// <param name="objs">the objs to get bytes</param>
        /// <returns>marshal struct to bytes</returns>
        internal static byte[] StructGetBytes(
            params object[] objs
            )
        {
            // have no objects
            if (objs == null || objs.Length == 0)
            {
                return null;
            }

            // the list store the bytes
            List<byte> list = new List<byte>();

            // more than one objects
            if (objs.Length > 1)
            {
                foreach (object item in objs)
                {
                    list.AddRange(StructGetBytes(item));
                }
                return list.ToArray();
            }

            // only one object
            object obj = objs[0];
            if (obj == null)
            {
                return null;
            }

            Type type = obj.GetType();

            // user defined type
            if (!type.FullName.StartsWith("System."))
            {
                FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

                if (fields == null)
                {
                    return null;
                }

                // convert all field of type to bytes
                foreach (FieldInfo field in fields)
                {
                    byte[] array = StructGetBytes(field.GetValue(obj));
                    if (array != null)
                    {
                        list.AddRange(array);
                    }
                }

                return list.ToArray();
            }

            // system definied type
            if (type.FullName == "System.Byte")
            {
                list.Add((byte)obj);
            }
            else if (type.FullName == "System.Byte[]")
            {
                list.AddRange((byte[])obj);
            }
            else if (type.FullName == "System.Int16")
            {
                list.AddRange(BitConverter.GetBytes((short)obj));
            }
            else if (type.FullName == "System.UInt16")
            {
                list.AddRange(BitConverter.GetBytes((ushort)obj));
            }
            else if (type.FullName == "System.Int32")
            {
                list.AddRange(BitConverter.GetBytes((int)obj));
            }
            else if (type.FullName == "System.UInt32")
            {
                list.AddRange(BitConverter.GetBytes((uint)obj));
            }
            else if (type.FullName == "System.Int64")
            {
                list.AddRange(BitConverter.GetBytes((long)obj));
            }
            else if (type.FullName == "System.UInt64")
            {
                list.AddRange(BitConverter.GetBytes((ulong)obj));
            }

            return list.ToArray();
        }


        /// <summary>
        /// get the 4bytes uint array
        /// </summary>
        /// <param name="data">the data of uint</param>
        /// <returns>get the uint bytes</returns>
        internal static byte[] SecurityUInt32GetBytes(
            uint data
            )
        {
            return BitConverter.GetBytes(data);
        }


        /// <summary>
        /// get the 4bytes uint from bytes
        /// </summary>
        /// <param name="data">the bytes contains the uint</param>
        /// <returns>the uint value</returns>
        /// <returns>build a uint from bytes</returns>
        internal static uint BytesToSecurityUInt32(
            byte[] data
            )
        {
            return BitConverter.ToUInt32(data, 0);
        }


        /// <summary>
        /// get the 8bytes ulong from bytes
        /// </summary>
        /// <param name="data">the bytes contains the ulong</param>
        /// <returns>the ulong value</returns>
        /// <returns>build a ulong from bytes</returns>
        internal static ulong BytesToSecurityUInt64(
            byte[] data
            )
        {
            return BitConverter.ToUInt64(data, 0);
        }


        /// <summary>
        /// update the sealing key for connectionless mode.<para/>
        /// In connectionless mode, messages can arrive out of order.<para/>
        /// Because of this, the sealing key MUST be reset for every message.<para/>
        /// Rekeying with the same sealing key for multiple messages would not maintain message security.<para/>
        /// Therefore, a per-message sealing key, SealingKey', is computed as the MD5 hash of the original
        /// sealing key and the message sequence number.<para/>
        /// The resulting SealingKey' value is used to reinitialize the key state structure prior to invoking
        /// the following SIGN, SEAL, and MAC algorithms. To compute the SealingKey' and initialize the key
        /// state structure identified by the Handle parameter, use the following: <para/>
        /// SealingKey' = MD5(ConcatenationOf(SealingKey, SequenceNumber)) <para/>
        /// RC4Init(Handle, SealingKey')
        /// </summary>
        /// <param name="negotiateFlags">the negotiate flags</param>
        /// <param name="handle">the handle for RC4Init(H, K)</param>
        /// <param name="sealingKey">the sealing key for RC4Init(H, K)</param>
        /// <param name="sequenceNumber">the sequence number</param>
        internal static void UpdateSealingKeyForConnectionlessMode(
            NegotiateTypes negotiateFlags, int handle, byte[] sealingKey, uint sequenceNumber)
        {
            if (NlmpUtility.IsConnectionless(negotiateFlags))
            {
                NlmpUtility.RC4Init(handle,
                    NlmpUtility.MD5(NlmpUtility.ConcatenationOf(sealingKey, BitConverter.GetBytes(sequenceNumber))));
            }
        }


        #endregion

        #region Internal Methods For Gss Api

        /// <summary>
        /// this takes the given byte array, encrypts it, and returns another byte array containing the encrypted 
        /// message.the format of the returned byte array is as follows: |MESSAGE|
        /// </summary>
        /// <param name="version">the version of Nlmp, this value can be NTLMv1 or NTLMv2.</param>
        /// <param name="negotiateFlags">the negotiate negotiateFlags, indicates the capabilities of server/client</param>
        /// <param name="handle">
        /// the handle of encrypt key, this value can be ClientHandle/ServerHandle in Context.
        /// </param>
        /// <param name="key">the key used to encrypt the message</param>
        /// <param name="seqNum">the sequence number, this value will be modified in this function</param>
        /// <param name="securityBuffers">
        /// the security buffer array to encrypt.<para/>
        /// it can contain none or some data security buffer, that are combine to one message to encrypt.<para/>
        /// it can contain none or some token security buffer, in which the signature will be stored.
        /// </param>
        /// <exception cref="ArgumentNullException">the securityBuffers must not be null</exception>
        internal static void GssApiEncrypt(
            NlmpVersion version, NegotiateTypes negotiateFlags, int handle, byte[] key,
            ref uint seqNum, params SecurityBuffer[] securityBuffers)
        {
            if (securityBuffers == null)
            {
                throw new ArgumentNullException("securityBuffers");
            }

            byte[] encryptedMessage = null;
            byte[] signature = null;

            int sequenceNumber = (int)seqNum;

            // seal all non-readonly data
            byte[] nonReadonlyData =
                SspiUtility.ConcatenateReadWriteSecurityBuffers(securityBuffers, SecurityBufferType.Data);
            encryptedMessage = RC4(handle, nonReadonlyData);

            // generate the signature with all data, including readonly data.
            byte[] bothReadonlyAndReadWriteMessage =
                SspiUtility.ConcatenateSecurityBuffers(securityBuffers, SecurityBufferType.Data);
            signature = Mac(version, negotiateFlags, handle, key, ref sequenceNumber, bothReadonlyAndReadWriteMessage);

            seqNum = (uint)sequenceNumber;

            SspiUtility.UpdateSecurityBuffers(securityBuffers, SecurityBufferType.Data, encryptedMessage);

            byte[] signatureToVerify =
                SspiUtility.ConcatenateSecurityBuffers(securityBuffers, SecurityBufferType.Token);
            if (signatureToVerify != null && signatureToVerify.Length > 0)
            {
                SspiUtility.UpdateSecurityBuffers(securityBuffers, SecurityBufferType.Token, signature);
            }
        }


        /// <summary>
        /// this takes the given byte array, decrypts it, and returns the original, unencrypted byte array. The given 
        /// byte array is assumed to be of the format: |ENCRYPTED_MESSAGE|
        /// </summary>
        /// <param name="version">the version of Nlmp, this value can be NTLMv1 or NTLMv2.</param>
        /// <param name="negotiateFlags">the negotiate negotiateFlags, indicates the capabilities of server/client</param>
        /// <param name="handle">
        /// the handle of decrypt key, this value can be ClientHandle/ServerHandle in Context.
        /// </param>
        /// <param name="key">the key used to decrypt the message</param>
        /// <param name="seqNum">the sequence number, this value will be modified in this function</param>
        /// <param name="securityBuffers">
        /// the security buffer array to decrypt.<para/>
        /// it can contain none or some data security buffer, that are combine to one message to decrypt.<para/>
        /// it can contain none or some token security buffer, in which the signature is stored.
        /// </param>
        /// <returns>the encrypt result, if verify, it's the verify result.</returns>
        /// <exception cref="ArgumentNullException">the securityBuffers must not be null</exception>
        internal static bool GssApiDecrypt(
            NlmpVersion version, NegotiateTypes negotiateFlags, int handle, byte[] key,
            ref uint seqNum, params SecurityBuffer[] securityBuffers)
        {
            if (securityBuffers == null)
            {
                throw new ArgumentNullException("securityBuffers");
            }

            byte[] decryptedMessage = null;

            byte[] signature = null;

            int sequenceNumber = (int)seqNum;

            // seal all non-readonly data
            byte[] nonReadonlyData =
                SspiUtility.ConcatenateReadWriteSecurityBuffers(securityBuffers, SecurityBufferType.Data);
            decryptedMessage = RC4(handle, nonReadonlyData);

            // update the decrypted message to security buffers
            SspiUtility.UpdateSecurityBuffers(securityBuffers, SecurityBufferType.Data, decryptedMessage);

            // generate the signature with all data, including readonly data.
            byte[] bothReadonlyAndReadWriteMessage =
                SspiUtility.ConcatenateSecurityBuffers(securityBuffers, SecurityBufferType.Data);
            signature = Mac(version, negotiateFlags, handle, key, ref sequenceNumber, bothReadonlyAndReadWriteMessage);

            seqNum = (uint)sequenceNumber;

            byte[] signatureToVerify =
                SspiUtility.ConcatenateSecurityBuffers(securityBuffers, SecurityBufferType.Token);

            if (signatureToVerify != null && signatureToVerify.Length > 0
                && !ArrayUtility.CompareArrays<byte>(signature, signatureToVerify))
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        /// <summary>
        /// This takes the given byte array, signs it, and returns another byte array containing the signature. The 
        /// format of the returned byte array is as follows: |SIGNATURE|
        /// </summary>
        /// <param name="version">the version of Nlmp, this value can be NTLMv1 or NTLMv2.</param>
        /// <param name="negotiateFlags">the negotiate negotiateFlags, indicates the capabilities of server/client</param>
        /// <param name="handle">
        /// the handle of sign key, this value can be ClientHandle/ServerHandle in Context.
        /// </param>
        /// <param name="key">the key used to sign the message</param>
        /// <param name="seqNum">the sequence number, this value will be modified in this function</param>
        /// <param name="securityBuffers">
        /// the security buffer array to sign.<para/>
        /// it can contain none or some data security buffer, that are combine to one message to sign.<para/>
        /// it must contain token security buffer, in which the signature will be stored.
        /// </param>
        /// <exception cref="ArgumentNullException">the securityBuffers must not be null</exception>
        /// <exception cref="ArgumentException">securityBuffers must contain signature to store signature</exception>
        internal static void GssApiSign(
            NlmpVersion version, NegotiateTypes negotiateFlags, int handle, byte[] key,
            ref uint seqNum, params SecurityBuffer[] securityBuffers)
        {
            if (securityBuffers == null)
            {
                throw new ArgumentNullException("securityBuffers");
            }

            // signature security buffer
            byte[] signatureSecurityBuffer =
                SspiUtility.ConcatenateReadWriteSecurityBuffers(securityBuffers, SecurityBufferType.Token);

            if (signatureSecurityBuffer == null || signatureSecurityBuffer.Length <= 0)
            {
                throw new ArgumentException(
                    "securityBuffers must contain signature to store signature", "securityBuffers");
            }

            // get the combined message
            byte[] message = SspiUtility.ConcatenateSecurityBuffers(securityBuffers, SecurityBufferType.Data);

            byte[] signedMessage = null;
            byte[] signature = null;

            int sequenceNumber = (int)seqNum;

            signedMessage = NlmpUtility.Sign(
                version,
                negotiateFlags,
                handle,
                key,
                ref sequenceNumber,
                message);

            seqNum = (uint)sequenceNumber;

            // get the signature
            signature = new byte[signedMessage.Length - message.Length];
            Array.Copy(signedMessage, message.Length, signature, 0, signature.Length);

            SspiUtility.UpdateSecurityBuffers(securityBuffers, SecurityBufferType.Token, signature);
        }


        /// <summary>
        /// this takes the given byte array and verifies it using the SSPI VerifySignature method. 
        /// </summary>
        /// <param name="version">the version of Nlmp, this value can be NTLMv1 or NTLMv2.</param>
        /// <param name="negotiateFlags">the negotiate negotiateFlags, indicates the capabilities of server/client</param>
        /// <param name="handle">
        /// the handle of sign key, this value can be ClientHandle/ServerHandle in Context.
        /// </param>
        /// <param name="key">the key used to sign the message</param>
        /// <param name="seqNum">the sequence number, this value will be modified in this function</param>
        /// <param name="securityBuffers">
        /// the security buffer array to verify.<para/>
        /// it can contain none or some data security buffer, that are combine to one message to verify.<para/>
        /// it must contain token security buffer, in which the signature is stored.
        /// </param>
        /// <returns>the verify result. if true, verify success.</returns>
        /// <exception cref="ArgumentNullException">the securityBuffers must not be null</exception>
        /// <exception cref="ArgumentException">securityBuffers must contain signature to verify</exception>
        internal static bool GssApiVerify(
            NlmpVersion version, NegotiateTypes negotiateFlags, int handle, byte[] key,
            ref uint seqNum, params SecurityBuffer[] securityBuffers)
        {
            if (securityBuffers == null)
            {
                throw new ArgumentNullException("securityBuffers");
            }

            byte[] message = SspiUtility.ConcatenateSecurityBuffers(securityBuffers, SecurityBufferType.Data);
            byte[] signature = SspiUtility.ConcatenateSecurityBuffers(securityBuffers, SecurityBufferType.Token);

            if (signature == null || signature.Length <= 0)
            {
                throw new ArgumentException("securityBuffers must contain signature to verify", "securityBuffers");
            }

            int sequenceNumber = (int)seqNum;

            // sign message
            byte[] expectedSignedMessage = NlmpUtility.Sign(
                version,
                negotiateFlags,
                handle,
                key,
                ref sequenceNumber,
                message);

            seqNum = (uint)sequenceNumber;

            byte[] expectedSignature = new byte[expectedSignedMessage.Length - message.Length];
            Array.Copy(expectedSignedMessage, message.Length, expectedSignature, 0, expectedSignature.Length);

            // verify signature.
            return ArrayUtility.CompareArrays<byte>(signature, expectedSignature);
        }


        #endregion
    }
}
