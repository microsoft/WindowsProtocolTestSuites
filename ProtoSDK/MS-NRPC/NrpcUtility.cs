// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Messages;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using System.Globalization;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc
{
    /// <summary>
    /// NRPC utility class.
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public static class NrpcUtility
    {
        /// <summary>
        /// NETLOGON RPC interface UUID.
        /// </summary>
        public static readonly Guid NETLOGON_RPC_INTERFACE_UUID = new Guid("12345678-1234-ABCD-EF00-01234567CFFB");

        /// <summary>
        /// NETLOGON RPC interface major version.
        /// </summary>
        public const ushort NETLOGON_RPC_INTERFACE_MAJOR_VERSION = 1;

        /// <summary>
        /// NETLOGON RPC interface minor version.
        /// </summary>
        public const ushort NETLOGON_RPC_INTERFACE_MINOR_VERSION = 0;


        /// <summary>
        /// MS-NRPC uses the following well-known endpoint. 
        /// This endpoint is a named pipe for RPC over SMB.
        /// </summary>
        public const string NETLOGON_RPC_OVER_NP_WELLKNOWN_ENDPOINT = @"\PIPE\NETLOGON";


        // NETLOGON session key length, in bytes.
        internal const int NETLOGON_SESSION_KEY_LENGTH = 16;

        // NETLOGON credential length, in bytes.
        internal const int NL_CREDENTIAL_LENGTH = 8;

        // NL_AUTH_SIGNATURE flags field length, in bytes.
        internal const int NL_AUTH_SIGNATURE_FLAGS_LENGTH = 2;

        // NL_AUTH_SIGNATURE sequence field number length, in bytes.
        internal const int NL_AUTH_SIGNATURE_SEQNUM_LENGTH = 8;

        // NL_AUTH_SIGNATURE checksum field length, in bytes.
        internal const int NL_AUTH_SIGNATURE_CHECKSUM_LENGTH = 8;

        // NL_AUTH_SIGNATURE confounder field length, in bytes.
        internal const int NL_AUTH_SIGNATURE_CONFOUNDER_LENGTH = 8;

        // NL_AUTH_SHA2_SIGNATURE dummy field length, in bytes.
        internal const int NL_AUTH_SHA2_SIGNATURE_DUMMY_LENGTH = 24;

        // LM_OWF_PASSWORD/NT_OWF_PASSWORD length
        internal const int OWF_PASSWORD_LENGTH = 16;


        #region Exception helper methods

        /// <summary>
        /// Throw exception when method returns an error status.
        /// </summary>
        /// <param name="method">method name</param>
        /// <param name="status">method return status</param>
        internal static void ThrowExceptionOnStatus(string method, int status)
        {
            string name = Enum.GetName(typeof(NtStatus), (uint)status);
            string message = string.Format(CultureInfo.InvariantCulture, 
                "{0} failed with status {1} (0x{2:x}).",
                method,
                name ?? "UNKNOWN",
                status);
            throw new InvalidOperationException(message);
        }


        /// <summary>
        /// Throw exception when signature, credential or authentication validation failed.
        /// </summary>
        /// <param name="method">method name</param>
        internal static void ThrowExceptionOnValidationFail(string method)
        {
            string message = string.Format(CultureInfo.InvariantCulture, 
                "{0} failed to validate returned authenticator or credential.",
                method);
            throw new InvalidOperationException(message);
        }

        #endregion


        #region Compute session-key, credential and authenticator methods

        /// <summary>
        /// InitLMKey defined in [MS-NRPC] section 3.1.4.4.2.
        /// </summary>
        /// <param name="keyIn">KeyIn, 7 bytes.</param>
        /// <returns>KeyOut, 8 bytes.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when keyIn is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the size of keyIn is not 7 bytes.
        /// </exception>
        public static byte[] InitLMKey(byte[] keyIn)
        {
            if (keyIn == null)
            {
                throw new ArgumentNullException("keyIn");
            }
            if (keyIn.Length != 7)
            {
                throw new ArgumentException("keyIn must be a 7-bytes array", "keyIn");
            }


            //InitLMKey(KeyIn, KeyOut)
            //KeyOut[0] = KeyIn[0] >> 0x01;
            //KeyOut[1] = ((KeyIn[0]&0x01)<<6) | (KeyIn[1]>>2);
            //KeyOut[2] = ((KeyIn[1]&0x03)<<5) | (KeyIn[2]>>3);
            //KeyOut[3] = ((KeyIn[2]&0x07)<<4) | (KeyIn[3]>>4);
            //KeyOut[4] = ((KeyIn[3]&0x0F)<<3) | (KeyIn[4]>>5);
            //KeyOut[5] = ((KeyIn[4]&0x1F)<<2) | (KeyIn[5]>>6);
            //KeyOut[6] = ((KeyIn[5]&0x3F)<<1) | (KeyIn[6]>>7);
            //KeyOut[7] = KeyIn[6] & 0x7F;
            //for( int i=0; i<8; i++ ){
            //  KeyOut[i] = (KeyOut[i] << 1) & 0xfe;
            //}


            byte[] keyOut = new byte[8];

            keyOut[0] = (byte)(keyIn[0] >> 1);
            keyOut[1] = (byte)(((keyIn[0] & 0x01) << 6) | (keyIn[1] >> 2));
            keyOut[2] = (byte)(((keyIn[1] & 0x03) << 5) | (keyIn[2] >> 3));
            keyOut[3] = (byte)(((keyIn[2] & 0x07) << 4) | (keyIn[3] >> 4));
            keyOut[4] = (byte)(((keyIn[3] & 0x0F) << 3) | (keyIn[4] >> 5));
            keyOut[5] = (byte)(((keyIn[4] & 0x1F) << 2) | (keyIn[5] >> 6));
            keyOut[6] = (byte)(((keyIn[5] & 0x3F) << 1) | (keyIn[6] >> 7));
            keyOut[7] = (byte)(keyIn[6] & 0x7F);
            for (int i = 0; i < 8; i++)
            {
                keyOut[i] = (byte)((keyOut[i] << 1) & 0xfe);
            }

            return keyOut;
        }


        /// <summary>
        /// Compute the Session Key.
        /// </summary>
        /// <param name="algorithm">
        /// Algorithm to compute the session key.
        /// </param>
        /// <param name="sharedSecret">
        /// An even-numbered sequence of bytes, with no embedded zero values, 
        /// that is a plain-text secret (password) shared between the client and the server.
        /// </param>
        /// <param name="clientChallenge">
        /// A byte array that contains the client challenge.
        /// </param>
        /// <param name="serverChallenge">
        /// A byte array that contains the server challenge response.
        /// </param>
        /// <returns>Session key</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when sharedSecret, clientChallenge or serverChallenge parameter 
        /// passed to the method is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when computation algorithm is not supported.
        /// </exception>
        public static byte[] ComputeSessionKey(
            NrpcComputeSessionKeyAlgorithm algorithm,
            string sharedSecret,
            byte[] clientChallenge,
            byte[] serverChallenge)
        {
            if (sharedSecret == null)
            {
                throw new ArgumentNullException("sharedSecret");
            }
            if (clientChallenge == null)
            {
                throw new ArgumentNullException("clientChallenge");
            }
            if (serverChallenge == null)
            {
                throw new ArgumentNullException("serverChallenge");
            }


            byte[] m4ss;
            byte[] sessionKey;

            using (MD4 md4 = MD4.Create())
            {
                md4.Initialize();
                m4ss = md4.ComputeHash(Encoding.Unicode.GetBytes(sharedSecret));
            }

            switch (algorithm)
            {
                case NrpcComputeSessionKeyAlgorithm.HMACSHA256:
                    //ComputeSessionKey(SharedSecret, ClientChallenge, ServerChallenge)
                    //M4SS := MD4(UNICODE(SharedSecret))
                    //CALL SHA256Reset(HashContext, M4SS, sizeof(M4SS));
                    //CALL SHA256Input(HashContext, ClientChallenge, sizeof(ClientChallenge));
                    //CALL SHA256FinalBits (HashContext, ServerChallenge, sizeof(ServerChallenge));
                    //CALL SHA256Result(HashContext, SessionKey);
                    //SET SessionKey to lower 16 bytes of the SessionKey;

                    using (HMACSHA256 hmacSha256 = new HMACSHA256(m4ss))
                    {
                        byte[] buffer = ArrayUtility.ConcatenateArrays(clientChallenge, serverChallenge);
                        byte[] hash = hmacSha256.ComputeHash(buffer);
                        sessionKey = ArrayUtility.SubArray(hash, 0, NETLOGON_SESSION_KEY_LENGTH);
                    }

                    break;

                case NrpcComputeSessionKeyAlgorithm.MD5:
                    //SET zeroes to 4 bytes of 0
                    //ComputeSessionKey(SharedSecret, ClientChallenge, ServerChallenge)
                    //M4SS := MD4(UNICODE(SharedSecret))
                    //CALL MD5Init(md5context)
                    //CALL MD5Update(md5context, zeroes, [4 bytes])
                    //CALL MD5Update(md5context, ClientChallenge, [8 bytes])
                    //CALL MD5Update(md5context, ServerChallenge, [8 bytes])
                    //CALL MD5Final(md5context)
                    //CALL HMAC_MD5(md5context.digest, md5context.digest length,
                    //M4SS, length of M4SS, output)
                    //SET Session-Key to output
                    byte[] digest;
                    using (MD5 md5 = MD5.Create())
                    {
                        byte[] buffer = ArrayUtility.ConcatenateArrays(
                            new byte[4],
                            clientChallenge,
                            serverChallenge);
                        digest = md5.ComputeHash(buffer);
                    }
                    using (HMACMD5 hmacMd5 = new HMACMD5(m4ss))
                    {
                        sessionKey = hmacMd5.ComputeHash(digest);
                    }
                    break;

                case NrpcComputeSessionKeyAlgorithm.DES:
                    //ComputeSessionKey(SharedSecret, ClientChallenge, ServerChallenge)
                    //M4SS := MD4(UNICODE(SharedSecret))
                    //SET sum to ClientChallenge + ServerChallenge
                    //SET k1 to lower 7 bytes of the M4SS
                    //SET k2 to upper 7 bytes of the M4SS
                    //CALL DES_ECB(sum, k1, &output1)
                    //CALL DES_ECB(output1, k2, &output2)
                    //SET Session-Key to output2


                    byte[] sum = BitConverter.GetBytes(
                        BitConverter.ToUInt64(clientChallenge, 0) +
                        BitConverter.ToUInt64(serverChallenge, 0));

                    byte[] k1 = ArrayUtility.SubArray(m4ss, 0, 7);
                    byte[] k2 = ArrayUtility.SubArray(m4ss, m4ss.Length - 7, 7);

                    using (DES des = DES.Create())
                    {
                        des.Mode = CipherMode.ECB;
                        des.Padding = PaddingMode.None;

                        des.Key = InitLMKey(k1);
                        byte[] output1 = des.CreateEncryptor().TransformFinalBlock(sum, 0, sum.Length);

                        des.Key = InitLMKey(k2);
                        byte[] output2 = des.CreateEncryptor().TransformFinalBlock(output1, 0, output1.Length);

                        sessionKey = output2;
                    }

                    //The key produced without AES and strong-key support negotiated is 64 bits 
                    //and is padded to 128 bits with zeros in the most-significant bits.
                    sessionKey = ArrayUtility.ConcatenateArrays(
                        sessionKey,
                        new byte[NETLOGON_SESSION_KEY_LENGTH - sessionKey.Length]);

                    break;


                default:
                    throw new ArgumentException(
                        "Specified session key computation algorithm is not valid.",
                        "algorithm");
            }

            return sessionKey;
        }


        /// <summary>
        /// Compute the Netlogon Credential.
        /// </summary>
        /// <param name="algorithm">
        /// Algorithm to compute the Netlogon authenticator. 
        /// </param>
        /// <param name="input">
        /// A byte array that contains the input.
        /// </param>
        /// <param name="sessionKey">
        /// Session Key.
        /// </param>
        /// <returns>Netlogon Credential</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when input or sessionKey parameter 
        /// passed to the method is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when computation algorithm is not supported. 
        /// Thrown when session key length is incorrect.
        /// </exception>
        public static byte[] ComputeNetlogonCredential(
            NrpcComputeNetlogonCredentialAlgorithm algorithm,
            byte[] input,
            byte[] sessionKey)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            if (sessionKey == null)
            {
                throw new ArgumentNullException("sessionKey");
            }
            if (sessionKey.Length != NETLOGON_SESSION_KEY_LENGTH)
            {
                throw new ArgumentException("Session key length is incorrect.", "sessionKey");
            }

            byte[] credential;

            switch (algorithm)
            {
                case NrpcComputeNetlogonCredentialAlgorithm.AES128:
                    //ComputeNetlogonCredential(Input, Sk, Output)
                    //SET IV = 0
                    //CALL AesEncrypt(Input, Sk, IV, Output)

                    using (BCryptAlgorithm aes = new BCryptAlgorithm("AES"))
                    {
                        aes.Mode = BCryptCipherMode.CFB;
                        aes.Key = sessionKey;
                        aes.IV = new byte[aes.BlockSize]; //AES128's IV is 128 bits.
                        credential = aes.Encrypt(input);
                    }

                    break;


                case NrpcComputeNetlogonCredentialAlgorithm.DESECB:
                    //ComputeNetlogonCredential(Input, Sk, Output)
                    //SET k1 to bytes(0, 6, Sk)
                    //CALL InitLMKey(k1, k3)
                    //SET k2 to bytes(7, 13, Sk)
                    //CALL InitLMKey(k2, k4)
                    //CALL DES_ECB(Input, k3, &output1)
                    //CALL DES_ECB(output1, k4, &output2)
                    //SET Output to output2

                    byte[] k1 = ArrayUtility.SubArray(sessionKey, 0, 7);
                    byte[] k2 = ArrayUtility.SubArray(sessionKey, 7, 7);

                    using (DES des = DES.Create())
                    {
                        des.Mode = CipherMode.ECB;
                        des.Padding = PaddingMode.None;

                        des.Key = InitLMKey(k1);
                        byte[] output1 = des.CreateEncryptor().TransformFinalBlock(input, 0, input.Length);

                        des.Key = InitLMKey(k2);
                        byte[] output2 = des.CreateEncryptor().TransformFinalBlock(output1, 0, output1.Length);

                        credential = output2;
                    }

                    break;


                default:
                    throw new ArgumentException(
                        "Specified netlogon credential computation algorithm is not valid.",
                        "algorithm");
            }

            return credential;
        }


        /// <summary>
        /// Compute the Netlogon Authenticator at client-side.
        /// </summary>
        /// <param name="algorithm">
        /// Algorithm to compute the authenticator.
        /// </param>
        /// <param name="time">
        /// Current time on client.
        /// </param>
        /// <param name="clientStoredCredential">
        /// A byte array containing the credential that is created by the client and 
        /// received by the server and that is used during computation and 
        /// verification of the Netlogon authenticator.
        /// </param>
        /// <param name="sessionKey">
        /// Session Key.
        /// </param>
        /// <returns>Netlogon Authenticator</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when clientStoredCredential or sessionKey is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when session key length is incorrect.
        /// </exception>
        public static _NETLOGON_AUTHENTICATOR ComputeClientNetlogonAuthenticator(
            NrpcComputeNetlogonCredentialAlgorithm algorithm,
            DateTime time,
            ref byte[] clientStoredCredential,
            byte[] sessionKey)
        {
            if (clientStoredCredential == null)
            {
                throw new ArgumentNullException("clientStoredCredential");
            }
            if (sessionKey == null)
            {
                throw new ArgumentNullException("sessionKey");
            }
            if (sessionKey.Length != NETLOGON_SESSION_KEY_LENGTH)
            {
                throw new ArgumentException("Session key length is incorrect.", "sessionKey");
            }

            //SET TimeNow = current time;
            //SET ClientAuthenticator.Timestamp = TimeNow;
            //SET ClientStoredCredential = ClientStoredCredential + TimeNow;
            //CALL ComputeNetlogonCredential(ClientStoredCredential, Session-Key, ClientAuthenticator.Credential);

            //Timestamp is an integer value that contains the time of day at 
            //which the client constructed this authentication credential, 
            //represented as the number of elapsed seconds since 00:00:00 of January 1, 1970 (UTC).

            DateTime time19700101 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            _NETLOGON_AUTHENTICATOR authenticator = new _NETLOGON_AUTHENTICATOR();

            authenticator.Timestamp = (uint)(
                (time.ToUniversalTime() - time19700101).Ticks / TimeSpan.TicksPerSecond);

            //In each of the addition operations previously performed, 
            //the least-significant 4 bytes of the credential are added 
            //with the 4-byte time stamp value (or the constant 1), 
            //and overflow is ignored. This leaves the most-significant 
            //4 bytes of the credential unmodified.
            uint credential = BitConverter.ToUInt32(clientStoredCredential, 0);
            credential += authenticator.Timestamp;
            byte[] buf = BitConverter.GetBytes(credential);
            Array.Copy(buf, clientStoredCredential, buf.Length);

            authenticator.Credential = new _NETLOGON_CREDENTIAL();
            authenticator.Credential.data = ComputeNetlogonCredential(
                algorithm,
                clientStoredCredential,
                sessionKey);

            return authenticator;
        }


        /// <summary>
        /// Validates the client Netlogon authenticator by incrementing the Netlogon credential 
        /// in the Netlogon authenticator by one, performs the computation described in section 
        /// 3.1.4.4, Netlogon Credential Computation, and stores the new Netlogon credential. 
        /// The server returns a Netlogon authenticator that contains the new Netlogon credential 
        /// to the client.
        /// </summary>
        /// <param name="clientAuthenticator">
        /// Authenticator from client to validate.
        /// </param>
        /// <param name="algorithm">
        /// Algorithm to validate the authenticator.
        /// </param>
        /// <param name="serverStoredCredential">
        /// A byte array containing the credential that is maintained by 
        /// the server and that is used during computation and 
        /// verification of the Netlogon authenticator.
        /// </param>
        /// <param name="sessionKey">Session Key</param>
        /// <returns>Return true if validate successfully; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when serverStoredCredential or sessionKey is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when Credential.data field of clientAuthenticator is null. 
        /// Thrown when session key length is incorrect.
        /// </exception>
        public static bool ValidateClientNetlogonAuthenticator(
            _NETLOGON_AUTHENTICATOR clientAuthenticator,
            NrpcComputeNetlogonCredentialAlgorithm algorithm,
            ref byte[] serverStoredCredential,
            byte[] sessionKey)
        {
            if (serverStoredCredential == null)
            {
                throw new ArgumentNullException("serverStoredCredential");
            }
            if (sessionKey == null)
            {
                throw new ArgumentNullException("sessionKey");
            }
            if (sessionKey.Length != NETLOGON_SESSION_KEY_LENGTH)
            {
                throw new ArgumentException("Session key length is incorrect.", "sessionKey");
            }
            if (clientAuthenticator.Credential.data == null)
            {
                throw new ArgumentException("clientAuthenticator is invalid.", "clientAuthenticator");
            }

            //SET ServerStoredCredential = ServerStoredCredential +
            //      ClientAuthenticator.Timestamp;
            //CALL ComputeNetlogonCredential(ServerStoredCredential,
            //      Session-Key, TempCredential);
            //IF TempCredential != ClientAuthenticator.Credential
            //THEN return access denied error

            //In each of the addition operations previously performed, 
            //the least-significant 4 bytes of the credential are added 
            //with the 4-byte time stamp value (or the constant 1), 
            //and overflow is ignored. This leaves the most-significant 
            //4 bytes of the credential unmodified.
            uint credential = BitConverter.ToUInt32(serverStoredCredential, 0);
            credential += clientAuthenticator.Timestamp;
            byte[] buf = BitConverter.GetBytes(credential);
            Array.Copy(buf, serverStoredCredential, buf.Length);

            byte[] tempCredential = ComputeNetlogonCredential(
                algorithm,
                serverStoredCredential,
                sessionKey);

            return ArrayUtility.CompareArrays(tempCredential, clientAuthenticator.Credential.data);
        }


        /// <summary>
        /// Compute the Netlogon Authenticator at server-side.
        /// </summary>
        /// <param name="algorithm">
        /// Algorithm to compute the authenticator.
        /// </param>
        /// <param name="serverStoredCredential">
        /// A byte array containing the credential that is created by the server and 
        /// received by the client and that is used during computation and 
        /// verification of the Netlogon authenticator.
        /// </param>
        /// <param name="sessionKey">
        /// Session Key.
        /// </param>
        /// <returns>Netlogon Authenticator</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when serverStoredCredential or sessionKey is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when session key length is incorrect.
        /// </exception>
        public static _NETLOGON_AUTHENTICATOR ComputeServerNetlogonAuthenticator(
            NrpcComputeNetlogonCredentialAlgorithm algorithm,
            ref byte[] serverStoredCredential,
            byte[] sessionKey)
        {
            if (serverStoredCredential == null)
            {
                throw new ArgumentNullException("serverStoredCredential");
            }
            if (sessionKey == null)
            {
                throw new ArgumentNullException("sessionKey");
            }
            if (sessionKey.Length != NETLOGON_SESSION_KEY_LENGTH)
            {
                throw new ArgumentException("Session key length is incorrect.", "sessionKey");
            }

            //SET ServerStoredCredential = ServerStoredCredential + 1;
            //CALL ComputeNetlogonCredential(ServerStoredCredential,
            //Session-Key, ServerAuthenticator.Credential);

            _NETLOGON_AUTHENTICATOR authenticator = new _NETLOGON_AUTHENTICATOR();

            //In each of the addition operations previously performed, 
            //the least-significant 4 bytes of the credential are added 
            //with the 4-byte time stamp value (or the constant 1), 
            //and overflow is ignored. This leaves the most-significant 
            //4 bytes of the credential unmodified.
            uint credential = BitConverter.ToUInt32(serverStoredCredential, 0);
            credential += 1;
            byte[] buf = BitConverter.GetBytes(credential);
            Array.Copy(buf, serverStoredCredential, buf.Length);

            authenticator.Credential = new _NETLOGON_CREDENTIAL();
            authenticator.Credential.data = ComputeNetlogonCredential(
                algorithm,
                serverStoredCredential,
                sessionKey);

            return authenticator;
        }


        /// <summary>
        /// Validates the returned Netlogon authenticator by 
        /// incrementing its stored Netlogon credential by one, 
        /// encrypting the result with the session key using 
        /// the algorithm described in section Netlogon Credential Computation, 
        /// and comparing the results.
        /// </summary>
        /// <param name="serverAuthenticator">
        /// Server returned authenticator to validate.
        /// </param>
        /// <param name="algorithm">
        /// Algorithm to validate the authenticator.
        /// </param>
        /// <param name="clientStoredCredential">
        /// A byte array containing the credential that is created by the client and 
        /// received by the server and that is used during computation and 
        /// verification of the Netlogon authenticator.
        /// </param>
        /// <param name="sessionKey">Session Key</param>
        /// <returns>Return true if validate successfully; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when clientStoredCredential or sessionKey is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when Credential.data field of serverAuthenticator is null. 
        /// Thrown when session key length is incorrect.
        /// </exception>
        public static bool ValidateServerNetlogonAuthenticator(
            _NETLOGON_AUTHENTICATOR serverAuthenticator,
            NrpcComputeNetlogonCredentialAlgorithm algorithm,
            ref byte[] clientStoredCredential,
            byte[] sessionKey)
        {
            if (clientStoredCredential == null)
            {
                throw new ArgumentNullException("clientStoredCredential");
            }
            if (sessionKey == null)
            {
                throw new ArgumentNullException("sessionKey");
            }
            if (sessionKey.Length != NETLOGON_SESSION_KEY_LENGTH)
            {
                throw new ArgumentException("Session key length is incorrect.", "sessionKey");
            }
            if (serverAuthenticator.Credential.data == null)
            {
                throw new ArgumentException("serverAuthenticator is invalid.", "serverAuthenticator");
            }

            //SET ClientStoredCredential = ClientStoredCredential + 1;
            //CALL ComputeNetlogonCredential(ClientStoredCredential, Session-Key, TempCredential);
            //IF TempCredential != ServerAuthenticator.Credential
            //THEN return abort

            //In each of the addition operations previously performed, 
            //the least-significant 4 bytes of the credential are added 
            //with the 4-byte time stamp value (or the constant 1), 
            //and overflow is ignored. This leaves the most-significant 
            //4 bytes of the credential unmodified.
            uint credential = BitConverter.ToUInt32(clientStoredCredential, 0);
            credential += 1;
            byte[] buf = BitConverter.GetBytes(credential);
            Array.Copy(buf, clientStoredCredential, buf.Length);

            byte[] tempCredential = ComputeNetlogonCredential(
                algorithm,
                clientStoredCredential,
                sessionKey);

            return ArrayUtility.CompareArrays(tempCredential, serverAuthenticator.Credential.data);
        }


        // For client behavior test, we should have following 2 methods:
        // ValidateClientNetlogonAuthenticator
        // ComputeServerNetlogonAuthenticator

        /// <summary>
        ///  Calculates the server/client credential based on negotiate flags
        /// </summary>
        /// <param name="SessionKey">the session key</param>
        /// <param name="negotiateFlags">the negotiate flags</param>
        /// <param name="ClientOrServerChallenge">The client challenge or server challenge</param>
        /// <returns>The credential computed</returns>
        static internal byte[] ComputeNetlogonCredential(byte[] SessionKey, uint negotiateFlags, byte[] ClientOrServerChallenge)
        {
            if (SessionKey == null)
            {
                throw new InvalidOperationException("Session Key is null, unable to calculate server Credential");
            }

            if (ClientOrServerChallenge == null)
            {
                throw new InvalidOperationException("ServerChallenge is null, unable to calculate server Credential");
            }

            if ((negotiateFlags & (uint)NrpcNegotiateFlags.SupportsAESAndSHA2) != 0)
            {
                return NrpcUtility.ComputeNetlogonCredential(NrpcComputeNetlogonCredentialAlgorithm.AES128,
                    ClientOrServerChallenge, SessionKey);
            }
            else
            {
                return NrpcUtility.ComputeNetlogonCredential(NrpcComputeNetlogonCredentialAlgorithm.DESECB,
                    ClientOrServerChallenge, SessionKey);
            }
        }
        #endregion


        #region Integrity / confidentiality methods

        /// <summary>
        /// Encrypt a byte array. 
        /// Many data in NRPC struct is required to be encrypted.
        /// </summary>
        /// <param name="isAesNegotiated">
        /// Is AES negotiated during secure channel setup. 
        /// If AES is not negotiated, use RC4 to encrypt.
        /// </param>
        /// <param name="sessionKey">
        /// Session key.
        /// </param>
        /// <param name="buffer">
        /// Buffer to encrypt.
        /// </param>
        /// <returns>
        /// Encrypted buffer.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when sessionKey or buffer is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when session key length is incorrect.
        /// </exception>
        public static byte[] EncryptBuffer(
            bool isAesNegotiated,
            byte[] sessionKey,
            byte[] buffer)
        {
            if (sessionKey == null)
            {
                throw new ArgumentNullException("sessionKey");
            }
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if (sessionKey.Length != NETLOGON_SESSION_KEY_LENGTH)
            {
                throw new ArgumentException("Session key length is incorrect.", "sessionKey");
            }

            if (isAesNegotiated)
            {
                using (BCryptAlgorithm aes = new BCryptAlgorithm("AES"))
                {
                    aes.Mode = BCryptCipherMode.CFB;
                    aes.Key = sessionKey;
                    aes.IV = new byte[aes.BlockSize];
                    return aes.Encrypt(buffer);
                }
            }
            else
            {
                using (RC4 rc4 = RC4.Create())
                {
                    rc4.Key = sessionKey;
                    return rc4.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length);
                }
            }
        }


        /// <summary>
        /// Decrypt a byte array. 
        /// Many data in NRPC struct is required to be encrypted.
        /// </summary>
        /// <param name="isAesNegotiated">
        /// Is AES negotiated during secure channel setup. 
        /// If AES is not negotiated, use RC4 to decrypt.
        /// </param>
        /// <param name="sessionKey">
        /// Session key.
        /// </param>
        /// <param name="buffer">
        /// Buffer to decrypt.
        /// </param>
        /// <returns>
        /// Decrypted buffer.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when sessionKey or buffer is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when session key length is incorrect.
        /// </exception>
        public static byte[] DecryptBuffer(
            bool isAesNegotiated,
            byte[] sessionKey,
            byte[] buffer)
        {
            if (sessionKey == null)
            {
                throw new ArgumentNullException("sessionKey");
            }
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if (sessionKey.Length != NETLOGON_SESSION_KEY_LENGTH)
            {
                throw new ArgumentException("Session key length is incorrect.", "sessionKey");
            }

            if (isAesNegotiated)
            {
                using (BCryptAlgorithm aes = new BCryptAlgorithm("AES"))
                {
                    aes.Mode = BCryptCipherMode.CFB;
                    aes.Key = sessionKey;
                    aes.IV = new byte[aes.BlockSize];
                    return aes.Decrypt(buffer);
                }
            }
            else
            {
                using (RC4 rc4 = RC4.Create())
                {
                    rc4.Key = sessionKey;
                    return rc4.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length);
                }
            }
        }


        /// <summary>
        /// Indicates the computation of an N-byte cryptographic-strength random number.
        /// </summary>
        /// <param name="size">The size of nonce</param>
        /// <returns>The random nonce</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when size is negative.
        /// </exception>
        public static byte[] GenerateNonce(int size)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(
                    "size",
                    size,
                    "size must be equal or greater than zero.");
            }

            byte[] buffer = new byte[size];

            Random random = new Random();
            random.NextBytes(buffer);

            return buffer;
        }


        /// <summary>
        /// Compute copy sequence number defined in MS-NRPC section 3.3.4.2.1.
        /// </summary>
        /// <param name="sequenceNumber">Sequence Number</param>
        /// <param name="isClientSideOutbound">Is client side outbound message.</param>
        /// <returns>CopySeqNumber</returns>
        private static byte[] ComputeCopySequenceNumber(ulong sequenceNumber, bool isClientSideOutbound)
        {
            //Assume byte(n, l) returns byte n of the 32-bit number l.
            //The n parameter is limited to 0..3. The least significant
            //byte is 0, the most significant byte is 3.
            //SET CopySeqNumber[0] to byte(3, ClientSequenceNumber.LowPart)
            //SET CopySeqNumber[1] to byte(2, ClientSequenceNumber.LowPart)
            //SET CopySeqNumber[2] to byte(1, ClientSequenceNumber.LowPart)
            //SET CopySeqNumber[3] to byte(0, ClientSequenceNumber.LowPart)
            //SET CopySeqNumber[4] to byte(3, ClientSequenceNumber.HighPart)
            //SET CopySeqNumber[5] to byte(2, ClientSequenceNumber.HighPart)
            //SET CopySeqNumber[6] to byte(1, ClientSequenceNumber.HighPart)
            //SET CopySeqNumber[7] to byte(0, ClientSequenceNumber.HighPart)
            //Set CopySeqNumber[4] to CopySeqNumber[4] OR 0x80

            byte[] sequenceNumberLowPart = BitConverter.GetBytes((uint)(sequenceNumber & 0xFFFFFFFF));
            byte[] sequenceNumberHighPart = BitConverter.GetBytes((uint)(sequenceNumber >> 32));
            byte[] copySeqNumber = new byte[NL_AUTH_SIGNATURE_SEQNUM_LENGTH];
            copySeqNumber[0] = sequenceNumberLowPart[3];
            copySeqNumber[1] = sequenceNumberLowPart[2];
            copySeqNumber[2] = sequenceNumberLowPart[1];
            copySeqNumber[3] = sequenceNumberLowPart[0];
            copySeqNumber[4] = sequenceNumberHighPart[3];
            copySeqNumber[5] = sequenceNumberHighPart[2];
            copySeqNumber[6] = sequenceNumberHighPart[1];
            copySeqNumber[7] = sequenceNumberHighPart[0];

            if (isClientSideOutbound)
            {
                copySeqNumber[4] = (byte)(copySeqNumber[4] | 0x80);
            }

            return copySeqNumber;
        }


        /// <summary>
        /// Compute encryption key when AES is negotiated
        /// </summary>
        /// <param name="sessionKey">session key</param>
        /// <returns>encryption key</returns>
        private static byte[] ComputeEncryptionKeyWhenAesIsNegotiated(byte[] sessionKey)
        {
            //The AES key used MUST be derived using the following algorithm:
            //FOR (I=0; I < Key Length; I++)
            //EncryptionKey [I] = SessionKey[I] XOR 0xf0
            byte[] aesEncryptionKey = new byte[sessionKey.Length];
            for (int i = 0; i < sessionKey.Length; i++)
            {
                aesEncryptionKey[i] = (byte)(sessionKey[i] ^ 0xf0);
            }
            return aesEncryptionKey;
        }


        /// <summary>
        /// Compute encryption key when AES is not negotiated
        /// </summary>
        /// <param name="sessionKey">session key</param>
        /// <param name="copySequenceNumber">CopySequenceNumber</param>
        /// <returns>encryption key</returns>
        private static byte[] ComputeEncryptionKeyWhenAesIsNotNegotiated(
            byte[] sessionKey,
            byte[] copySequenceNumber)
        {
            //The RC4 key used MUST be derived using the following algorithm
            //SET zeroes to 4 bytes of 0
            //FOR (I=0; I < Key Length; I++)
            //XorKey [I] = SessionKey[I] XOR 0xf0
            //CALL hmac_md5(zeroes, [4 bytes], XorKey, size of XorKey, TmpData)
            //CALL hmac_md5(CopySeqNumber, size of CopySeqNumber, TmpData,
            //size of TmpData, EncryptionKey)
            byte[] xorKey = new byte[sessionKey.Length];
            for (int i = 0; i < sessionKey.Length; i++)
            {
                xorKey[i] = (byte)(sessionKey[i] ^ 0xf0);
            }

            byte[] tmpData;
            byte[] rc4EncryptionKey;
            using (HMACMD5 hmacMd5 = new HMACMD5(xorKey))
            {
                tmpData = hmacMd5.ComputeHash(new byte[4]);
            }
            using (HMACMD5 hmacMd5 = new HMACMD5(tmpData))
            {
                rc4EncryptionKey = hmacMd5.ComputeHash(copySequenceNumber);
            }

            return rc4EncryptionKey;
        }


        /// <summary>
        /// Compute signature when AES is negotiated
        /// </summary>
        /// <param name="nlAuthSha2Sign">NL_AUTH_SHA2_SIGNATURE struct</param>
        /// <param name="sessionKey">session key</param>
        /// <param name="requestConfidentiality">confidentiality is required or not</param>
        /// <param name="plainText">plain-text message to make signature</param>
        /// <returns>signature</returns>
        private static byte[] ComputeSignatureWhenAesIsNegotiated(
            NL_AUTH_SHA2_SIGNATURE nlAuthSha2Sign,
            byte[] sessionKey,
            bool requestConfidentiality,
            byte[] plainText)
        {
            //If AES is negotiated, then a signature MUST be computed using 
            //the following algorithm:
            //CALL SHA256Reset(&HashContext, Sk, sizeof(Sk));
            //CALL SHA256Input(HashContext, NL_AUTH_SHA2_SIGNATURE, [8 bytes]);
            //IF Confidentiality requested
            //CALL SHA256Input(HashContext, Confounder, [8 bytes]);
            //CALL SHA256FinalBits(HashContext, Message, size of Message;
            //CALL SHA256Result(HashContext, output);
            //SET Signature to output
            //Note: In the first call to SHA256Input, only the first 8-bytes
            //of the NL_AUTH_SHA2_SIGNATURE structure are used.
            byte[] signature;

            byte[] buf = ArrayUtility.ConcatenateArrays(
                BitConverter.GetBytes((ushort)nlAuthSha2Sign.SignatureAlgorithm),
                BitConverter.GetBytes((ushort)nlAuthSha2Sign.SealAlgorithm),
                BitConverter.GetBytes((ushort)nlAuthSha2Sign.Pad),
                BitConverter.GetBytes((ushort)nlAuthSha2Sign.Flags));

            if (requestConfidentiality)
            {
                buf = ArrayUtility.ConcatenateArrays(
                    buf,
                    nlAuthSha2Sign.Confounder);
            }

            buf = ArrayUtility.ConcatenateArrays(
                buf,
                plainText);

            using (HMACSHA256 hmacSha256 = new HMACSHA256(sessionKey))
            {
                signature = hmacSha256.ComputeHash(buf);
            }

            //After the signature is computed, the signature MUST be truncated, 
            //with only the first 8 bytes being copied into the Checksum field 
            //of NL_AUTH_SHA2_SIGNATURE.
            signature = ArrayUtility.SubArray(signature, 0, NL_AUTH_SIGNATURE_CHECKSUM_LENGTH);

            return signature;
        }


        /// <summary>
        /// Compute signature when AES is not negotiated
        /// </summary>
        /// <param name="nlAuthSign">NL_AUTH_SIGNATURE struct</param>
        /// <param name="sessionKey">session key</param>
        /// <param name="requestConfidentiality">confidentiality is required or not</param>
        /// <param name="plainText">plain-text message to make signature</param>
        /// <returns>signature</returns>
        private static byte[] ComputeSignatureWhenAesIsNotNegotiated(
            NL_AUTH_SIGNATURE nlAuthSign,
            byte[] sessionKey,
            bool requestConfidentiality,
            byte[] plainText)
        {
            //If AES is not negotiated, a signature MUST be computed using 
            //the following algorithm:
            //SET zeroes to 4 bytes of 0
            //CALL MD5Init(md5context)
            //CALL MD5Update(md5context, zeroes, [4 bytes])
            //CALL MD5Update(md5context, NL_AUTH_SIGNATURE, [8 bytes])
            //IF Confidentiality requested
            //CALL MD5Update(md5context, Confounder, [8 bytes])
            //CALL MD5Update(md5context, Message, size of Message)
            //CALL MD5Final(md5context)
            //CALL HMAC_MD5(md5context.digest, md5context.digest length,
            //Session-Key, size of Session Key, output)
            //SET Signature to output
            //Note: In the second call to MD5Update, only the first 8-bytes
            //of the NL_AUTH_SIGNATURE structure are used.
            byte[] signature;

            byte[] buf = ArrayUtility.ConcatenateArrays(
                new byte[4],
                BitConverter.GetBytes((ushort)nlAuthSign.SignatureAlgorithm),
                BitConverter.GetBytes((ushort)nlAuthSign.SealAlgorithm),
                BitConverter.GetBytes((ushort)nlAuthSign.Pad),
                nlAuthSign.Flags);

            if (requestConfidentiality)
            {
                buf = ArrayUtility.ConcatenateArrays(
                    buf,
                    nlAuthSign.Confounder);
            }

            buf = ArrayUtility.ConcatenateArrays(
                buf,
                plainText);

            using (MD5 md5 = MD5.Create())
            {
                signature = md5.ComputeHash(buf);
            }
            using (HMACMD5 hmacMd5 = new HMACMD5(sessionKey))
            {
                signature = hmacMd5.ComputeHash(signature);
            }

            //After the signature is computed, the signature MUST be truncated, 
            //with only the first 8 bytes being copied into the Checksum field 
            //of NL_AUTH_SIGNATURE.
            signature = ArrayUtility.SubArray(signature, 0, NL_AUTH_SIGNATURE_CHECKSUM_LENGTH);

            return signature;
        }


        /// <summary>
        /// initial netlogon signature token when AES is negotiated
        /// </summary>
        /// <param name="sequenceNumber">sequence number</param>
        /// <param name="sessionKey">session key</param>
        /// <param name="requestConfidentiality">confidentiality is required or not</param>
        /// <param name="isClientSideOutbound">Is client side outbound message.</param>
        /// <param name="securityBuffers">
        /// Security buffers, contains input plain-text; output cipher-text and signature.
        /// </param>
        private static void InitialNetlogonSignatureTokenWhenAesIsNegotiated(
            ref ulong sequenceNumber,
            byte[] sessionKey,
            bool requestConfidentiality,
            bool isClientSideOutbound,
            SecurityBuffer[] securityBuffers)
        {
            byte[] plainText;
            byte[] cipherText;
            byte[] token;
            NL_AUTH_SHA2_SIGNATURE nlAuthSha2Sign = new NL_AUTH_SHA2_SIGNATURE();

            //The SignatureAlgorithm first byte MUST be set to 0x13, 
            //and the second byte MUST be set to 0x00.
            nlAuthSha2Sign.SignatureAlgorithm =
                NL_AUTH_SHA2_SIGNATURE_SignatureAlgorithm_Values.HMACSHA256;

            if (requestConfidentiality)
            {
                //If the Confidentiality option (section 3.3.1) is 
                //requested from the application, then the SealAlgorithm 
                //first byte MUST be set to 0x1A, the second byte MUST 
                //be set to 0x00, and the Confounder MUST be filled with 
                //cryptographically random data.
                nlAuthSha2Sign.SealAlgorithm =
                    NL_AUTH_SHA2_SIGNATURE_SealAlgorithm_Values.AES128;
                nlAuthSha2Sign.Confounder = GenerateNonce(NL_AUTH_SIGNATURE_CONFOUNDER_LENGTH);
            }
            else
            {
                //If the Confidentiality option (section 3.3.1) is not 
                //requested, then the SealAlgorithm MUST be filled with 
                //two bytes of 0xff, and the Confounder is not included in the token.
                nlAuthSha2Sign.SealAlgorithm =
                    NL_AUTH_SHA2_SIGNATURE_SealAlgorithm_Values.NotEncrypted;
                nlAuthSha2Sign.Confounder = null;
            }

            //The Pad MUST be filled with 0xff bytes.
            nlAuthSha2Sign.Pad = NL_AUTH_SHA2_SIGNATURE_Pad_Values.V1;

            //The Flags MUST be filled with 0x00 bytes.
            nlAuthSha2Sign.Flags = Flags_Values.V1;

            //The SequenceNumber MUST be computed using the following algorithm.
            nlAuthSha2Sign.SequenceNumber = ComputeCopySequenceNumber(sequenceNumber, isClientSideOutbound);

            //The ClientSequenceNumber MUST be incremented by 1.
            sequenceNumber += 1;

            //Compute signature
            plainText = ConcatenateSecurityBuffersForChecksum(securityBuffers);
            nlAuthSha2Sign.Checksum = ComputeSignatureWhenAesIsNegotiated(
                nlAuthSha2Sign,
                sessionKey,
                requestConfidentiality,
                plainText);

            //If the Confidentiality option is requested, the data and the Confounder 
            //field MUST be encrypted. If AES is negotiated then the server MUST use 
            //the AES-128 algorithm using the SessionKey with an initialization 
            //vector constructed by concatenating the sequence number with 
            //itself twice (thus getting 16 bytes of data)
            if (requestConfidentiality)
            {
                byte[] aesEncryptionKey = ComputeEncryptionKeyWhenAesIsNegotiated(sessionKey);

                using (BCryptAlgorithm aes = new BCryptAlgorithm("AES"))
                {
                    aes.Mode = BCryptCipherMode.CFB;
                    aes.Key = aesEncryptionKey;
                    aes.IV = ArrayUtility.ConcatenateArrays(
                        nlAuthSha2Sign.SequenceNumber,
                        nlAuthSha2Sign.SequenceNumber);
                    nlAuthSha2Sign.Confounder = aes.Encrypt(nlAuthSha2Sign.Confounder);

                    plainText = SspiUtility.ConcatenateReadWriteSecurityBuffers(
                        securityBuffers,
                        SecurityBufferType.Data);
                    cipherText = aes.Encrypt(plainText);
                }

                SspiUtility.UpdateSecurityBuffers(securityBuffers, SecurityBufferType.Data, cipherText);
            }
            else
            {
                cipherText = null;
            }

            //The SequenceNumber MUST be encrypted.
            //If AES is negotiated, then the server MUST use the AES-128 
            //algorithm using the SessionKey with an initialization vector 
            //constructed by concatenating the first 8 bytes of the 
            //checksum with itself twice (thus getting 16 bytes of data)
            using (BCryptAlgorithm aes = new BCryptAlgorithm("AES"))
            {
                aes.Mode = BCryptCipherMode.CFB;
                aes.Key = sessionKey;
                aes.IV = ArrayUtility.ConcatenateArrays(
                    nlAuthSha2Sign.Checksum, // Checksum is only 8 bytes
                    nlAuthSha2Sign.Checksum);
                nlAuthSha2Sign.SequenceNumber = aes.Encrypt(nlAuthSha2Sign.SequenceNumber);
            }

            nlAuthSha2Sign.Dummy = GenerateNonce(NL_AUTH_SHA2_SIGNATURE_DUMMY_LENGTH); // 24 == size of dummy

            token = TypeMarshal.ToBytes(nlAuthSha2Sign);
            SspiUtility.UpdateSecurityBuffers(securityBuffers, SecurityBufferType.Token, token);
        }

        /// <summary>
        /// initial netlogon signature token when AES is not negotiated
        /// </summary>
        /// <param name="sequenceNumber">sequence number</param>
        /// <param name="sessionKey">session key</param>
        /// <param name="requestConfidentiality">confidentiality is required or not</param>
        /// <param name="isClientSideOutbound">Is client side outbound message.</param>
        /// <param name="securityBuffers">
        /// Security buffers, contains input plain-text; output cipher-text and signature.
        /// </param>
        private static void InitialNetlogonSignatureTokenWhenAesIsNotNegotiated(
            ref ulong sequenceNumber,
            byte[] sessionKey,
            bool requestConfidentiality,
            bool isClientSideOutbound,
            SecurityBuffer[] securityBuffers)
        {
            byte[] plainText;
            byte[] cipherText;
            byte[] token;
            NL_AUTH_SIGNATURE nlAuthSign = new NL_AUTH_SIGNATURE();

            //The SignatureAlgorithm first byte MUST be set to 0x77 
            //and the second byte MUST be set to 0x00.
            nlAuthSign.SignatureAlgorithm = SignatureAlgorithm_Values.HMACMD5;

            if (requestConfidentiality)
            {
                //If the Confidentiality option (section 3.3.1) is requested 
                //from the application, then the SealAlgorithm first byte MUST 
                //be set to 0x7A, the second byte MUST be set to 0x00, and 
                //the Confounder MUST be filled with cryptographically random data.
                nlAuthSign.SealAlgorithm = SealAlgorithm_Values.RC4;
                nlAuthSign.Confounder = GenerateNonce(NL_AUTH_SIGNATURE_CONFOUNDER_LENGTH);
            }
            else
            {
                //If the Confidentiality option is not requested, then the 
                //SealAlgorithm MUST be filled with two bytes of value 0xff, 
                //and the Confounder is not included in the token.
                nlAuthSign.SealAlgorithm = SealAlgorithm_Values.NotEncrypted;
                nlAuthSign.Confounder = null;
            }

            //The Pad MUST be filled with 0xff bytes.
            nlAuthSign.Pad = Pad_Values.V1;

            //The Flags MUST be filled with 0x00 bytes.
            nlAuthSign.Flags = new byte[NL_AUTH_SIGNATURE_FLAGS_LENGTH];

            //The SequenceNumber MUST be computed using the following algorithm.
            nlAuthSign.SequenceNumber = ComputeCopySequenceNumber(sequenceNumber, isClientSideOutbound);

            //The ClientSequenceNumber MUST be incremented by 1.
            sequenceNumber += 1;

            //Compute signature
            plainText = ConcatenateSecurityBuffersForChecksum(securityBuffers);
            nlAuthSign.Checksum = ComputeSignatureWhenAesIsNotNegotiated(
                nlAuthSign,
                sessionKey,
                requestConfidentiality,
                plainText);

            //If the Confidentiality option is requested, the data and the Confounder 
            //field MUST be encrypted. If AES is not negotiated, it MUST use the 
            //RC4 algorithm.
            if (requestConfidentiality)
            {
                byte[] rc4EncryptionKey = ComputeEncryptionKeyWhenAesIsNotNegotiated(
                    sessionKey,
                    nlAuthSign.SequenceNumber);

                using (RC4 rc4 = RC4.Create())
                {
                    rc4.Key = rc4EncryptionKey;
                    nlAuthSign.Confounder = rc4.CreateEncryptor().TransformFinalBlock(
                        nlAuthSign.Confounder,
                        0,
                        nlAuthSign.Confounder.Length);

                    plainText = SspiUtility.ConcatenateReadWriteSecurityBuffers(
                        securityBuffers,
                        SecurityBufferType.Data);
                    cipherText = rc4.CreateEncryptor().TransformFinalBlock(
                        plainText,
                        0,
                        plainText.Length);
                }

                SspiUtility.UpdateSecurityBuffers(securityBuffers, SecurityBufferType.Data, cipherText);
            }
            else
            {
                cipherText = null;
            }

            //The SequenceNumber MUST be encrypted. 
            //If AES is not negotiated, it MUST use the RC4 algorithm.
            //The RC4 key MUST be derived as follows:
            //SET zeroes to 4 bytes of 0
            //CALL hmac_md5(zeroes, [4 bytes], SessionKey, size of SessionKey, TmpData)
            //CALL hmac_md5(Checksum, size of Checksum, TmpData, size of TmpData, EncryptionKey)
            byte[] tmpData;
            byte[] sequenceNumberEncryptionKey;
            using (HMACMD5 hmacMd5 = new HMACMD5(sessionKey))
            {
                tmpData = hmacMd5.ComputeHash(new byte[4]);
            }
            using (HMACMD5 hmacMd5 = new HMACMD5(tmpData))
            {
                sequenceNumberEncryptionKey = hmacMd5.ComputeHash(nlAuthSign.Checksum);
            }
            using (RC4 rc4 = RC4.Create())
            {
                rc4.Key = sequenceNumberEncryptionKey;
                nlAuthSign.SequenceNumber = rc4.CreateEncryptor().TransformFinalBlock(
                    nlAuthSign.SequenceNumber,
                    0,
                    nlAuthSign.SequenceNumber.Length);
            }

            token = TypeMarshal.ToBytes(nlAuthSign);
            SspiUtility.UpdateSecurityBuffers(securityBuffers, SecurityBufferType.Token, token);
        }


        /// <summary>
        /// validate netlogon signature token when AES is negotiated
        /// </summary>
        /// <param name="sequenceNumber">sequence number</param>
        /// <param name="sessionKey">session key</param>
        /// <param name="requestConfidentiality">confidentiality is required or not</param>
        /// <param name="isClientSideOutbound">Is client side outbound message.</param>
        /// <param name="securityBuffers">
        /// Security buffer, contains plain-text if requestConfidentiality is false; 
        /// or cipher-text if requestConfidentiality is true; 
        /// and signature.
        /// </param>
        /// <returns>true if validate success; otherwise, false.</returns>
        private static bool ValidateNetlogonSignatureTokenWhenAesIsNegotiated(
            ref ulong sequenceNumber,
            byte[] sessionKey,
            bool requestConfidentiality,
            bool isClientSideOutbound,
            SecurityBuffer[] securityBuffers)
        {
            byte[] plainText;
            byte[] cipherText;
            byte[] token;

            //If AES is negotiated, a server receives an NL_AUTH_SHA2_SIGNATURE structure
            token = SspiUtility.ConcatenateSecurityBuffers(securityBuffers, SecurityBufferType.Token);
            NL_AUTH_SHA2_SIGNATURE nlAuthSha2Sign = TypeMarshal.ToStruct<NL_AUTH_SHA2_SIGNATURE>(token);

            //The SignatureAlgorithm bytes MUST be verified to ensure:
            //If AES is negotiated, the first byte is set to 0x13.
            //The second byte is set to 0x00
            if (nlAuthSha2Sign.SignatureAlgorithm != NL_AUTH_SHA2_SIGNATURE_SignatureAlgorithm_Values.HMACSHA256)
            {
                return false;
            }

            if (requestConfidentiality)
            {
                //If the Confidentiality option is requested from the application, 
                //then the SealAlgorithm MUST be verified to ensure that 
                //if AES is negotiated, the first byte is set to 0x1A; 
                //The second byte is set to 0x00.
                if (nlAuthSha2Sign.SealAlgorithm != NL_AUTH_SHA2_SIGNATURE_SealAlgorithm_Values.AES128)
                {
                    return false;
                }
            }
            else
            {
                //If the Confidentiality option is not requested, 
                //then the SealAlgorithm MUST be verified to contain all 0xff bytes.
                if (nlAuthSha2Sign.SealAlgorithm != NL_AUTH_SHA2_SIGNATURE_SealAlgorithm_Values.NotEncrypted)
                {
                    return false;
                }
            }

            //The Pad MUST be verified to contain all 0xff bytes.
            if (nlAuthSha2Sign.Pad != NL_AUTH_SHA2_SIGNATURE_Pad_Values.V1)
            {
                return false;
            }

            //The Flags data MAY be<86> disregarded.

            //The SequenceNumber MUST be decrypted. 
            //If AES is negotiated, then the server MUST use the AES128 algorithm 
            //and a key of SessionKey and an initialization vector constructed 
            //by concatenating the checksum with itself (thus getting 16 bytes of data). 
            using (BCryptAlgorithm aes = new BCryptAlgorithm("AES"))
            {
                aes.Mode = BCryptCipherMode.CFB;
                aes.Key = sessionKey;
                aes.IV = ArrayUtility.ConcatenateArrays(
                    nlAuthSha2Sign.Checksum,
                    nlAuthSha2Sign.Checksum);
                nlAuthSha2Sign.SequenceNumber = aes.Decrypt(nlAuthSha2Sign.SequenceNumber);
            }

            //A local copy of SequenceNumber MUST be computed using the following algorithm.
            byte[] copySequenceNumber = ComputeCopySequenceNumber(sequenceNumber, isClientSideOutbound);

            //The SequenceNumber MUST be compared to CopySeqNumber. 
            if (!ArrayUtility.CompareArrays(
                    copySequenceNumber,
                    nlAuthSha2Sign.SequenceNumber))
            {
                return false;
            }

            //ServerSequenceNumber MUST be incremented.
            sequenceNumber += 1;

            if (requestConfidentiality)
            {
                //If the Confidentiality option is requested, 
                //the Confounder and the data MUST be decrypted.
                byte[] aesEncryptionKey = ComputeEncryptionKeyWhenAesIsNegotiated(sessionKey);

                using (BCryptAlgorithm aes = new BCryptAlgorithm("AES"))
                {
                    aes.Mode = BCryptCipherMode.CFB;
                    aes.Key = aesEncryptionKey;
                    aes.IV = ArrayUtility.ConcatenateArrays(
                        nlAuthSha2Sign.SequenceNumber,
                        nlAuthSha2Sign.SequenceNumber);
                    nlAuthSha2Sign.Confounder = aes.Decrypt(nlAuthSha2Sign.Confounder);

                    cipherText = SspiUtility.ConcatenateReadWriteSecurityBuffers(
                        securityBuffers,
                        SecurityBufferType.Data);
                    plainText = aes.Decrypt(cipherText);
                }

                SspiUtility.UpdateSecurityBuffers(securityBuffers, SecurityBufferType.Data, plainText);
            }

            //Compute signature
            plainText = ConcatenateSecurityBuffersForChecksum(securityBuffers);
            byte[] checksum = ComputeSignatureWhenAesIsNegotiated(
                nlAuthSha2Sign,
                sessionKey,
                requestConfidentiality,
                plainText);

            //The first 8 bytes of the computed signature MUST be compared to the checksum.
            if (checksum != null
                && checksum.Length >= NL_AUTH_SIGNATURE_CHECKSUM_LENGTH
                && nlAuthSha2Sign.Checksum != null
                && nlAuthSha2Sign.Checksum.Length >= NL_AUTH_SIGNATURE_CHECKSUM_LENGTH)
            {
                for (int i = 0; i < NL_AUTH_SIGNATURE_CHECKSUM_LENGTH; i++)
                {
                    if (checksum[i] != nlAuthSha2Sign.Checksum[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// validate netlogon signature token when AES is not negotiated
        /// </summary>
        /// <param name="sequenceNumber">sequence number</param>
        /// <param name="sessionKey">session key</param>
        /// <param name="requestConfidentiality">confidentiality is required or not</param>
        /// <param name="isClientSideOutbound">Is client side outbound message.</param>
        /// <param name="securityBuffers">
        /// Security buffer, contains plain-text if requestConfidentiality is false; 
        /// or cipher-text if requestConfidentiality is true; 
        /// and signature.
        /// </param>
        /// <returns>true if validate success; otherwise, false.</returns>
        private static bool ValidateNetlogonSignatureTokenWhenAesIsNotNegotiated(
            ref ulong sequenceNumber,
            byte[] sessionKey,
            bool requestConfidentiality,
            bool isClientSideOutbound,
            SecurityBuffer[] securityBuffers)
        {
            byte[] plainText;
            byte[] cipherText;
            byte[] token;

            //If AES is not negotiated, a server receives a NL_AUTH_SIGNATURE structure.
            token = SspiUtility.ConcatenateSecurityBuffers(securityBuffers, SecurityBufferType.Token);
            NL_AUTH_SIGNATURE nlAuthSign = TypeMarshal.ToStruct<NL_AUTH_SIGNATURE>(token);

            //The SignatureAlgorithm bytes MUST be verified to ensure:
            //If AES is not negotiated, the first byte is set to 0x77.
            //The second byte is set to 0x00
            if (nlAuthSign.SignatureAlgorithm != SignatureAlgorithm_Values.HMACMD5)
            {
                return false;
            }

            if (requestConfidentiality)
            {
                //If the Confidentiality option is requested from the application, 
                //then the SealAlgorithm MUST be verified to ensure that 
                //if AES is not negotiated, the first byte is set to 0x7A. 
                //The second byte is set to 0x00.
                if (nlAuthSign.SealAlgorithm != SealAlgorithm_Values.RC4)
                {
                    return false;
                }
            }
            else
            {
                //If the Confidentiality option is not requested, 
                //then the SealAlgorithm MUST be verified to contain all 0xff bytes.
                if (nlAuthSign.SealAlgorithm != SealAlgorithm_Values.NotEncrypted)
                {
                    return false;
                }
            }

            //The Pad MUST be verified to contain all 0xff bytes.
            if (nlAuthSign.Pad != Pad_Values.V1)
            {
                return false;
            }

            //The Flags data MAY be<86> disregarded.

            //The SequenceNumber MUST be decrypted. 
            //If AES is not negotiated, then the server MUST use the RC4 algorithm. 
            //The RC4key MUST be derived as follows.
            //SET zeroes to 4 bytes of 0
            //CALL hmac_md5(zeroes, [4 bytes], SessionKey, size of SessionKey, TmpData)
            //CALL hmac_md5(Checksum, size of Checksum, TmpData, size of TmpData, DecryptionKey)
            byte[] tmpData;
            byte[] sequenceNumberDecryptionKey;
            using (HMACMD5 hmacMd5 = new HMACMD5(sessionKey))
            {
                tmpData = hmacMd5.ComputeHash(new byte[4]);
            }
            using (HMACMD5 hmacMd5 = new HMACMD5(tmpData))
            {
                sequenceNumberDecryptionKey = hmacMd5.ComputeHash(nlAuthSign.Checksum);
            }
            using (RC4 rc4 = RC4.Create())
            {
                rc4.Key = sequenceNumberDecryptionKey;
                nlAuthSign.SequenceNumber = rc4.CreateDecryptor().TransformFinalBlock(
                    nlAuthSign.SequenceNumber,
                    0,
                    nlAuthSign.SequenceNumber.Length);
            }

            //A local copy of SequenceNumber MUST be computed using the following algorithm.
            byte[] copySequenceNumber = ComputeCopySequenceNumber(sequenceNumber, isClientSideOutbound);

            //The SequenceNumber MUST be compared to CopySeqNumber. 
            if (!ArrayUtility.CompareArrays(
                copySequenceNumber,
                nlAuthSign.SequenceNumber))
            {
                return false;
            }

            //ServerSequenceNumber MUST be incremented.
            sequenceNumber += 1;

            //If the Confidentiality option is requested, 
            //the Confounder and the data MUST be decrypted.
            if (requestConfidentiality)
            {
                byte[] rc4DecryptionKey = ComputeEncryptionKeyWhenAesIsNotNegotiated(
                    sessionKey,
                    nlAuthSign.SequenceNumber);

                using (RC4 rc4 = RC4.Create())
                {
                    rc4.Key = rc4DecryptionKey;
                    nlAuthSign.Confounder = rc4.CreateDecryptor().TransformFinalBlock(
                        nlAuthSign.Confounder,
                        0,
                        nlAuthSign.Confounder.Length);

                    cipherText = SspiUtility.ConcatenateReadWriteSecurityBuffers(
                        securityBuffers,
                        SecurityBufferType.Data);
                    plainText = rc4.CreateDecryptor().TransformFinalBlock(
                        cipherText,
                        0,
                        cipherText.Length);
                }

                SspiUtility.UpdateSecurityBuffers(securityBuffers, SecurityBufferType.Data, plainText);
            }

            //Compute signature
            plainText = ConcatenateSecurityBuffersForChecksum(securityBuffers);
            byte[] checksum = ComputeSignatureWhenAesIsNotNegotiated(
                nlAuthSign,
                sessionKey,
                requestConfidentiality,
                plainText);

            //The first 8 bytes of the computed signature MUST be compared to the checksum.
            if (checksum != null
                && checksum.Length >= NL_AUTH_SIGNATURE_CHECKSUM_LENGTH
                && nlAuthSign.Checksum != null
                && nlAuthSign.Checksum.Length >= NL_AUTH_SIGNATURE_CHECKSUM_LENGTH)
            {
                for (int i = 0; i < NL_AUTH_SIGNATURE_CHECKSUM_LENGTH; i++)
                {
                    if (checksum[i] != nlAuthSign.Checksum[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// Concatenate security buffers for computing checksum.
        /// </summary>
        /// <param name="securityBuffers">The security buffers.</param>
        /// <returns>Concatenated security buffers.</returns>
        private static byte[] ConcatenateSecurityBuffersForChecksum(SecurityBuffer[] securityBuffers)
        {
            byte[] buf = new byte[0];
            for (int i = 0; i < securityBuffers.Length; i++)
            {
                SecurityBufferType securityBufferType = securityBuffers[i].BufferType;
                if ((securityBufferType & ~SecurityBufferType.AttrMask) == SecurityBufferType.Data
                    && (securityBufferType & SecurityBufferType.ReadOnly) == 0
                    && securityBuffers[i].Buffer != null)
                {
                    buf = ArrayUtility.ConcatenateArrays(buf, securityBuffers[i].Buffer);
                }
            }
            return buf;
        }


        /// <summary>
        /// Compute an Initial Netlogon Signature Token, 
        /// and encrypt data at the same time.
        /// </summary>
        /// <param name="isAesNegotiated">
        /// Is AES negotiated to initial netlogon signature token.
        /// </param>
        /// <param name="sequenceNumber">
        /// Sequence Number.<para/>
        /// The Sequence Number MUST be incremented by 1, during the signing.
        /// </param>
        /// <param name="sessionKey">
        /// Session Key.
        /// </param>
        /// <param name="requestConfidentiality">
        /// Indicates whether the data should be encrypted.
        /// </param>
        /// <param name="isClientSideOutbound">
        /// Is client side outbound message.
        /// </param>
        /// <param name="securityBuffers">
        /// Security buffers, contains input plain-text; output cipher-text and signature.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when sessionKey or securityBuffers is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when session key length is incorrect.
        /// </exception>
        public static void InitialNetlogonSignatureToken(
            bool isAesNegotiated,
            ref ulong sequenceNumber,
            byte[] sessionKey,
            bool requestConfidentiality,
            bool isClientSideOutbound,
            params SecurityBuffer[] securityBuffers)
        {
            if (sessionKey == null)
            {
                throw new ArgumentNullException("sessionKey");
            }
            if (securityBuffers == null || securityBuffers.Length == 0)
            {
                throw new ArgumentNullException("securityBuffers");
            }
            if (sessionKey.Length != NETLOGON_SESSION_KEY_LENGTH)
            {
                throw new ArgumentException("Session key length is incorrect.", "sessionKey");
            }
            for (int i = 0; i < securityBuffers.Length; i++)
            {
                if (securityBuffers[i] == null)
                {
                    throw new ArgumentNullException("securityBuffers");
                }
            }

            if (isAesNegotiated)
            {
                InitialNetlogonSignatureTokenWhenAesIsNegotiated(
                    ref sequenceNumber,
                    sessionKey,
                    requestConfidentiality,
                    isClientSideOutbound,
                    securityBuffers);
            }
            else
            {
                InitialNetlogonSignatureTokenWhenAesIsNotNegotiated(
                    ref sequenceNumber,
                    sessionKey,
                    requestConfidentiality,
                    isClientSideOutbound,
                    securityBuffers);
            }
        }


        /// <summary>
        /// Validate a Netlogon Signature Token, and decrypt.
        /// </summary>
        /// <param name="isAesNegotiated">
        /// Is AES negotiated to validate netlogon signature token.
        /// </param>
        /// <param name="sequenceNumber">
        /// Sequence Number.
        /// </param>
        /// <param name="sessionKey">
        /// Session Key.
        /// </param>
        /// <param name="requestConfidentiality">
        /// Indicates whether the data was encrypted.
        /// </param>
        /// <param name="isClientSideOutbound">
        /// Is client side outbound message.
        /// </param>
        /// <param name="securityBuffers">
        /// Security buffer, contains plain-text if requestConfidentiality is false; 
        /// or cipher-text if requestConfidentiality is true; 
        /// and signature.
        /// </param>
        /// <returns>Returns true if verify successfully; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when sessionKey or securityBuffers is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when session key length is incorrect.
        /// </exception>
        public static bool ValidateNetlogonSignatureToken(
            bool isAesNegotiated,
            ref ulong sequenceNumber,
            byte[] sessionKey,
            bool requestConfidentiality,
            bool isClientSideOutbound,
            params SecurityBuffer[] securityBuffers)
        {
            if (sessionKey == null)
            {
                throw new ArgumentNullException("sessionKey");
            }
            if (securityBuffers == null || securityBuffers.Length == 0)
            {
                throw new ArgumentNullException("securityBuffers");
            }
            if (sessionKey.Length != NETLOGON_SESSION_KEY_LENGTH)
            {
                throw new ArgumentException("Session key length is incorrect.", "sessionKey");
            }
            for (int i = 0; i < securityBuffers.Length; i++)
            {
                if (securityBuffers[i] == null)
                {
                    throw new ArgumentNullException("securityBuffers");
                }
            }

            bool result;

            if (isAesNegotiated)
            {
                result = ValidateNetlogonSignatureTokenWhenAesIsNegotiated(
                    ref sequenceNumber,
                    sessionKey,
                    requestConfidentiality,
                    isClientSideOutbound,
                    securityBuffers);
            }
            else
            {
                result = ValidateNetlogonSignatureTokenWhenAesIsNotNegotiated(
                    ref sequenceNumber,
                    sessionKey,
                    requestConfidentiality,
                    isClientSideOutbound,
                    securityBuffers);
            }

            return result;
        }

        #endregion


        #region Retrieve NRPC dynamic TCP endpoint of a server

        /// <summary>
        /// Retrieve NRPC dynamic TCP endpoint of a server.
        /// </summary>
        /// <param name="serverName">Server name.</param>
        /// <returns>TCP endpoints</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when serverName is null.
        /// </exception>
        public static ushort[] QueryNrpcTcpEndpoint(string serverName)
        {
            if (serverName == null)
            {
                throw new ArgumentNullException("serverName");
            }

            return RpceEndpointMapper.QueryDynamicTcpEndpointByInterface(
                serverName,
                NETLOGON_RPC_INTERFACE_UUID,
                NETLOGON_RPC_INTERFACE_MAJOR_VERSION,
                NETLOGON_RPC_INTERFACE_MINOR_VERSION);
        }

        #endregion


        #region CreateXxxx helper methods

        /// <summary>
        /// Create a STRING struct.
        /// </summary>
        /// <param name="buf">
        /// Buffer to be included in the string. 
        /// If it is null, Length, MaximumLength is 0 and Buffer is NULL in created STRING struct.
        /// </param>
        /// <returns>A STRING struct.</returns>
        public static _STRING CreateString(byte[] buf)
        {
            _STRING str = new _STRING();
            if (buf != null)
            {
                str.Length = (ushort)buf.Length;
                str.MaximumLength = (ushort)buf.Length;
            }
            str.Buffer = buf;
            return str;
        }


        /// <summary>
        /// Create a _CYPHER_BLOCK 2-elements array.
        /// </summary>
        /// <param name="cypherText">
        /// Cypher-Text to be contained in _CYPHER_BLOCK. 
        /// The length must be 16 bytes.
        /// </param>
        /// <returns>A _CYPHER_BLOCK 2-elements array.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when cypherText is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the length of cypherText is incorrect.
        /// </exception>
        public static _CYPHER_BLOCK[] CreateCypherBlocks(byte[] cypherText)
        {
            if (cypherText == null)
            {
                throw new ArgumentNullException("cypherText");
            }
            if (cypherText.Length != NL_CREDENTIAL_LENGTH * 2)
            {
                throw new ArgumentException("Length of cypherText is incorrect.", "cypherText");
            }

            //Divide cypherText into 2 8-bytes byte array.
            _CYPHER_BLOCK[] cypherBlocks = new _CYPHER_BLOCK[2];
            cypherBlocks[0].data = ArrayUtility.SubArray(cypherText, 0, NL_CREDENTIAL_LENGTH);
            cypherBlocks[1].data = ArrayUtility.SubArray(cypherText, NL_CREDENTIAL_LENGTH, NL_CREDENTIAL_LENGTH);
            return cypherBlocks;
        }


        /// <summary>
        /// Create a _NETLOGON_LOGON_IDENTITY_INFO struct.
        /// </summary>
        /// <param name="parameterControl">ParameterControl.</param>
        /// <param name="domainName">Domain name.</param>
        /// <param name="userName">User name.</param>
        /// <param name="workstation">Workstation.</param>
        /// <returns>A _NETLOGON_LOGON_IDENTITY_INFO struct.</returns>
        public static _NETLOGON_LOGON_IDENTITY_INFO CreateNetlogonIdentityInfo(
            NrpcParameterControlFlags parameterControl,
            string domainName,
            string userName,
            string workstation)
        {
            _NETLOGON_LOGON_IDENTITY_INFO identityInfo = new _NETLOGON_LOGON_IDENTITY_INFO();
            identityInfo.LogonDomainName = DtypUtility.ToRpcUnicodeString(domainName);
            identityInfo.UserName = DtypUtility.ToRpcUnicodeString(userName);
            identityInfo.Workstation = DtypUtility.ToRpcUnicodeString(workstation);
            identityInfo.Reserved = DtypUtility.ToOldLargeInteger(0);
            identityInfo.ParameterControl = (uint)parameterControl;
            return identityInfo;
        }


        /// <summary>
        /// Extract domain names from _DOMAIN_NAME_BUFFER.
        /// </summary>
        /// <param name="buffer">A struct of _DOMAIN_NAME_BUFFER.</param>
        /// <returns>A string collection what contains domain names in the buffer.</returns>
        public static ReadOnlyCollection<string> ExtractDomainNames(_DOMAIN_NAME_BUFFER buffer)
        {
            List<string> nameList = new List<string>();

            if (buffer.DomainNames != null)
            {
                //The Unicode string buffer that contains the list of trusted domains, 
                //in MULTI-SZ format. MULTI-SZ format is a UTF-16 string composed of one or more substrings. 
                //Each substring is separated from adjacent substrings by the UTF-16 null character, 0x0000. 
                //After the final substring, the MULTI-SZ format string is terminated by two UTF-16 null characters.
                //For example, if there are three trusted domains, DOMAIN1, DOMAIN2, and DOMAIN3, 
                //the DomainNames string buffer would have the following form:
                //DOMAIN1<null>DOMAIN2<null>DOMAIN3<null><null>
                foreach (string name in Encoding.Unicode.GetString(buffer.DomainNames).Split('\0'))
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        nameList.Add(name);
                    }
                }
            }

            return nameList.AsReadOnly();
        }


        /// <summary>
        /// compute _NT_OWF_PASSWORD that is an ENCRYPTED_NT_OWF_PASSWORD structure, contains the NTOWFv1 (as specified
        /// in NTLM v1 
        /// Authentication in [MS-NLMP] section 3.3.1) of the current password, encrypted as specified in 
        /// [MS-SAMR] section 2.2.11.1.1, Encrypting an NT Hash or LM Hash Value with a specified key. 
        /// The session key is the specified 16-byte key that is used to derive the password's keys. The 
        /// specified 16-byte key uses the 16-byte value process, as specified in [MS-SAMR] section 2.2.11.1.4.
        /// </summary>
        /// <param name="password">password used to do the compute </param>
        /// <param name="key">The key used to do encryption, here is sessionkey</param>
        /// <returns>_NT_OWF_PASSWORD that contains encrypted NTOWFv1 of password </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when password is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown when key is null.
        /// </exception>
        public static _NT_OWF_PASSWORD ComputeEncryptedNtOwfv1Password(string password, byte[] key)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            byte[] ntowfv1Password = NlmpUtility.NtOWF(NlmpVersion.v1, null, null, password);

            byte[] encryptOwfPassword = EncryptOwfPassword(ntowfv1Password, key);

            _NT_OWF_PASSWORD ntowfPassword = new _NT_OWF_PASSWORD();
            ntowfPassword.data = CreateCypherBlocks(encryptOwfPassword);

            return ntowfPassword;
        }


        /// <summary>
        /// encrypt the owf of password as showed in 2.2.11.1 of TD of MS-SAMR
        /// </summary>
        /// <param name="data">owf of the password to be encrypted</param>
        /// <param name="key">key used for encryption</param>
        /// <returns>encrypted the owf of password</returns>
        private static byte[] EncryptOwfPassword(byte[] data, byte[] key)
        {
            if (data.Length != OWF_PASSWORD_LENGTH)
            {
                throw new ArgumentException("Length of data is incorrect.", "data");
            }

            if (key.Length != NETLOGON_SESSION_KEY_LENGTH)
            {
                throw new ArgumentException("Length of key is incorrect.", "key");
            }

            // call the method of SAMR
            _ENCRYPTED_LM_OWF_PASSWORD encryptedLmOwfPassword = SamrCryptography.EncryptBlockWithKey(data, key);

            return encryptedLmOwfPassword.data;
        }
        #endregion

        #region Create Nrpc Request Messages upon opnum
        /// <summary>
        /// Creates an instance of request stub upon opnum received
        /// </summary>
        /// <param name="opnum"> opnum received</param>
        /// <returns>an instance of request stub.</returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        internal static NrpcRequestStub CreateNrpcRequestStub(NrpcMethodOpnums opnum)
        {
            NrpcRequestStub requestStub = null;
            switch (opnum)
            {
                case NrpcMethodOpnums.NetrLogonUasLogon:
                    requestStub = new NrpcNetrLogonUasLogonRequest();
                    break;

                case NrpcMethodOpnums.NetrLogonUasLogoff:
                    requestStub = new NrpcNetrLogonUasLogoffRequest();
                    break;

                case NrpcMethodOpnums.NetrLogonSamLogon:
                    requestStub = new NrpcNetrLogonSamLogonRequest();
                    break;

                case NrpcMethodOpnums.NetrLogonSamLogoff:
                    requestStub = new NrpcNetrLogonSamLogoffRequest();
                    break;

                case NrpcMethodOpnums.NetrServerReqChallenge:
                    requestStub = new NrpcNetrServerReqChallengeRequest();
                    break;

                case NrpcMethodOpnums.NetrServerAuthenticate:
                    requestStub = new NrpcNetrServerAuthenticateRequest();
                    break;

                case NrpcMethodOpnums.NetrServerPasswordSet:
                    requestStub = new NrpcNetrServerPasswordSetRequest();
                    break;

                case NrpcMethodOpnums.NetrDatabaseDeltas:
                    requestStub = new NrpcNetrDatabaseDeltasRequest();
                    break;

                case NrpcMethodOpnums.NetrDatabaseSync:
                    requestStub = new NrpcNetrDatabaseSyncRequest();
                    break;

                case NrpcMethodOpnums.NetrAccountDeltas:
                    requestStub = new NrpcNetrAccountDeltasRequest();
                    break;

                case NrpcMethodOpnums.NetrAccountSync:
                    requestStub = new NrpcNetrAccountSyncRequest();
                    break;

                case NrpcMethodOpnums.NetrGetDCName:
                    requestStub = new NrpcNetrGetDCNameRequest();
                    break;

                case NrpcMethodOpnums.NetrLogonControl:
                    requestStub = new NrpcNetrLogonControlRequest();
                    break;

                case NrpcMethodOpnums.NetrGetAnyDCName:
                    requestStub = new NrpcNetrGetAnyDCNameRequest();
                    break;

                case NrpcMethodOpnums.NetrLogonControl2:
                    requestStub = new NrpcNetrLogonControl2Request();
                    break;

                case NrpcMethodOpnums.NetrServerAuthenticate2:
                    requestStub = new NrpcNetrServerAuthenticate2Request();
                    break;

                case NrpcMethodOpnums.NetrDatabaseSync2:
                    requestStub = new NrpcNetrDatabaseSync2Request();
                    break;

                case NrpcMethodOpnums.NetrDatabaseRedo:
                    requestStub = new NrpcNetrDatabaseRedoRequest();
                    break;

                case NrpcMethodOpnums.NetrLogonControl2Ex:
                    requestStub = new NrpcNetrLogonControl2ExRequest();
                    break;

                case NrpcMethodOpnums.NetrEnumerateTrustedDomains:
                    requestStub = new NrpcNetrEnumerateTrustedDomainsRequest();
                    break;

                case NrpcMethodOpnums.DsrGetDcName:
                    requestStub = new NrpcDsrGetDcNameRequest();
                    break;

                case NrpcMethodOpnums.NetrLogonGetCapabilities:
                    requestStub = new NrpcNetrLogonGetCapabilitiesRequest();
                    break;

                case NrpcMethodOpnums.NetrLogonSetServiceBits:
                    requestStub = new NrpcNetrLogonSetServiceBitsRequest();
                    break;

                case NrpcMethodOpnums.NetrLogonGetTrustRid:
                    requestStub = new NrpcNetrLogonGetTrustRidRequest();
                    break;

                case NrpcMethodOpnums.NetrLogonComputeServerDigest:
                    requestStub = new NrpcNetrLogonComputeServerDigestRequest();
                    break;

                case NrpcMethodOpnums.NetrLogonComputeClientDigest:
                    requestStub = new NrpcNetrLogonComputeClientDigestRequest();
                    break;

                case NrpcMethodOpnums.NetrServerAuthenticate3:
                    requestStub = new NrpcNetrServerAuthenticate3Request();
                    break;

                case NrpcMethodOpnums.DsrGetDcNameEx:
                    requestStub = new NrpcDsrGetDcNameExRequest();
                    break;

                case NrpcMethodOpnums.DsrGetSiteName:
                    requestStub = new NrpcDsrGetSiteNameRequest();
                    break;

                case NrpcMethodOpnums.NetrLogonGetDomainInfo:
                    requestStub = new NrpcNetrLogonGetDomainInfoRequest();
                    break;

                case NrpcMethodOpnums.NetrServerPasswordSet2:
                    requestStub = new NrpcNetrServerPasswordSet2Request();
                    break;

                case NrpcMethodOpnums.NetrServerPasswordGet:
                    requestStub = new NrpcNetrServerPasswordGetRequest();
                    break;

                case NrpcMethodOpnums.NetrLogonSendToSam:
                    requestStub = new NrpcNetrLogonSendToSamRequest();
                    break;

                case NrpcMethodOpnums.DsrAddressToSiteNamesW:
                    requestStub = new NrpcDsrAddressToSiteNamesWRequest();
                    break;

                case NrpcMethodOpnums.DsrGetDcNameEx2:
                    requestStub = new NrpcDsrGetDcNameEx2Request();
                    break;

                case NrpcMethodOpnums.NetrLogonGetTimeServiceParentDomain:
                    requestStub = new NrpcNetrLogonGetTimeServiceParentDomainRequest();
                    break;

                case NrpcMethodOpnums.NetrEnumerateTrustedDomainsEx:
                    requestStub = new NrpcNetrEnumerateTrustedDomainsExRequest();
                    break;

                case NrpcMethodOpnums.DsrAddressToSiteNamesExW:
                    requestStub = new NrpcDsrAddressToSiteNamesExWRequest();
                    break;

                case NrpcMethodOpnums.DsrGetDcSiteCoverageW:
                    requestStub = new NrpcDsrGetDcSiteCoverageWRequest();
                    break;

                case NrpcMethodOpnums.NetrLogonSamLogonEx:
                    requestStub = new NrpcNetrLogonSamLogonExRequest();
                    break;

                case NrpcMethodOpnums.DsrEnumerateDomainTrusts:
                    requestStub = new NrpcDsrEnumerateDomainTrustsRequest();
                    break;

                case NrpcMethodOpnums.DsrDeregisterDnsHostRecords:
                    requestStub = new NrpcDsrDeregisterDnsHostRecordsRequest();
                    break;

                case NrpcMethodOpnums.NetrServerTrustPasswordsGet:
                    requestStub = new NrpcNetrServerTrustPasswordsGetRequest();
                    break;

                case NrpcMethodOpnums.DsrGetForestTrustInformation:
                    requestStub = new NrpcDsrGetForestTrustInformationRequest();
                    break;

                case NrpcMethodOpnums.NetrGetForestTrustInformation:
                    requestStub = new NrpcNetrGetForestTrustInformationRequest();
                    break;

                case NrpcMethodOpnums.NetrLogonSamLogonWithFlags:
                    requestStub = new NrpcNetrLogonSamLogonWithFlagsRequest();
                    break;

                case NrpcMethodOpnums.NetrServerGetTrustInfo:
                    requestStub = new NrpcNetrServerGetTrustInfoRequest();
                    break;

                case NrpcMethodOpnums.OpnumUnused47:
                    requestStub = new NrpcOpnumUnused47Request();
                    break;

                case NrpcMethodOpnums.DsrUpdateReadOnlyServerDnsRecords:
                    requestStub = new NrpcDsrUpdateReadOnlyServerDnsRecordsRequest();
                    break;

                case NrpcMethodOpnums.NetrChainSetClientAttributes:
                    requestStub = new NrpcNetrChainSetClientAttributesRequest();
                    break;

                default:
                    throw new InvalidOperationException("Unknown opnum encountered");
            };

            return requestStub;
        }
        #endregion
    }
}
