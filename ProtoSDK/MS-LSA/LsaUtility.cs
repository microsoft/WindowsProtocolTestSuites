// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa
{
    /// <summary>
    /// LSA utility class.
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public static class LsaUtility
    {
        /// <summary>
        /// LSA RPC interface UUID.
        /// </summary>
        public static readonly Guid LSA_RPC_INTERFACE_UUID = new Guid("12345778-1234-ABCD-EF00-0123456789AB");

        /// <summary>
        /// LSA RPC interface major version.
        /// </summary>
        [CLSCompliant(false)]
        public const ushort LSA_RPC_INTERFACE_MAJOR_VERSION = 0;

        /// <summary>
        /// LSA RPC interface minor version.
        /// </summary>
        [CLSCompliant(false)]
        public const ushort LSA_RPC_INTERFACE_MINOR_VERSION = 0;

        /// <summary>
        /// MS-LSA uses the following well-known endpoint. 
        /// This endpoint is a named pipe for RPC over SMB.
        /// </summary>
        public const string LSA_RPC_OVER_NP_WELLKNOWN_ENDPOINT = @"\PIPE\lsarpc";

        /// <summary>
        /// The length of TrustedDomainAuthorizedBlob.randomData in bytes.
        /// </summary>
        private const uint  BLOB_AUTH_RANDOM_LENGTH= 512;

        /// <summary>
        /// The length of data to encrypt or decrypt using DES-ECB-LM.
        /// </summary>
        private const uint CRYPT_BLOCK_LENGTH = 8;
        
        /// <summary>
        /// The length of key to encrypt or decrypt using DES-ECB-LM.
        /// </summary>
        private const uint CRYPT_KEY_LENGTH = 7;

        /// <summary>
        /// The version to encrypt or decrypt using DES-ECB-LM.
        /// </summary>
        private const uint CRYPT_VERSION = 1;

        #region Retrieve LSA dynamic TCP endpoint of a server

        /// <summary>
        /// Retrieve LSA dynamic TCP endpoint of a server.
        /// </summary>
        /// <param name="serverName">Server name.</param>
        /// <returns>TCP endpoints</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when serverName is null.
        /// </exception>
        [CLSCompliant(false)]
        public static ushort[] QueryLsaTcpEndpoint(string serverName)
        {
            if (serverName == null)
            {
                throw new ArgumentNullException("serverName");
            }

            return RpceEndpointMapper.QueryDynamicTcpEndpointByInterface(
                serverName,
                LSA_RPC_INTERFACE_UUID,
                LSA_RPC_INTERFACE_MAJOR_VERSION,
                LSA_RPC_INTERFACE_MINOR_VERSION);
        }

        #endregion


        /// <summary>
        /// The four high-order bits in ACCESS_MASK values are translated by the responder into specific
        /// ACCESS_MASK values using the following tables, depending on the type of the object that the
        /// operation is performed on. For numeric values of the symbolic names used in this table, refer to
        /// section 2.2.1.1.2 for policy objects, section 2.2.1.1.3 for account objects, section 2.2.1.1.4 for
        /// secret objects, and section 2.2.1.1.5 for trusted domain objects. 
        /// </summary>
        /// <param name="accessMask">ACCESS_MASK value to be translated</param>
        /// <param name="opnum">Then opnum of which method the access mask applies on.</param>
        /// <returns>The result of translation to accessMask</returns>
        [CLSCompliant(false)]
        public static ACCESS_MASK TranslateAccessMask(ACCESS_MASK accessMask, LsaMethodOpnums opnum)
        {
            switch(opnum)
            {
                case LsaMethodOpnums.LsarOpenPolicy:
                case LsaMethodOpnums.LsarOpenPolicy2:
                    accessMask = TranslateAccessMaskForPolicy(accessMask);
                    break;
                case LsaMethodOpnums.LsarCreateAccount:
                case LsaMethodOpnums.LsarOpenAccount:
                    accessMask = TranslateAccessMaskForAccount(accessMask);
                    break;
                case LsaMethodOpnums.LsarCreateSecret:
                case LsaMethodOpnums.LsarOpenSecret:
                    accessMask = TranslateAccessMaskForSecret(accessMask);
                    break;
                case LsaMethodOpnums.LsarCreateTrustedDomain:
                case LsaMethodOpnums.LsarCreateTrustedDomainEx:
                case LsaMethodOpnums.LsarCreateTrustedDomainEx2:
                case LsaMethodOpnums.LsarOpenTrustedDomain:
                case LsaMethodOpnums.LsarOpenTrustedDomainByName:
                    accessMask = TranslateAccessMaskForTrustedDomain(accessMask);
                    break;
            }
            return accessMask;
        }


        /// <summary>
        /// Translate access mask for policy object.
        /// </summary>
        /// <param name="accessMask">ACCESS_MASK value to be translated</param>
        /// <returns>The result of translation to accessMask</returns>
        private static ACCESS_MASK TranslateAccessMaskForPolicy(ACCESS_MASK accessMask)
        {
            switch (accessMask)
            {
                case ACCESS_MASK.ValueToBeTranslated0x80000000:
                    accessMask = ACCESS_MASK.POLICY_VIEW_AUDIT_INFORMATION
                        | ACCESS_MASK.POLICY_GET_PRIVATE_INFORMATION
                        | ACCESS_MASK.READ_CONTROL;
                    break;
                case ACCESS_MASK.ValueToBeTranslated0x40000000:
                    accessMask = ACCESS_MASK.POLICY_TRUST_ADMIN
                        | ACCESS_MASK.POLICY_CREATE_ACCOUNT
                        | ACCESS_MASK.POLICY_CREATE_SECRET
                        | ACCESS_MASK.POLICY_CREATE_PRIVILEGE
                        | ACCESS_MASK.POLICY_SET_DEFAULT_QUOTA_LIMITS
                        | ACCESS_MASK.POLICY_SET_AUDIT_REQUIREMENTS
                        | ACCESS_MASK.POLICY_AUDIT_LOG_ADMIN
                        | ACCESS_MASK.POLICY_SERVER_ADMIN
                        | ACCESS_MASK.READ_CONTROL;
                    break;
                case ACCESS_MASK.ValueToBeTranslated0x20000000:
                    accessMask = ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION
                        | ACCESS_MASK.POLICY_LOOKUP_NAMES
                        | ACCESS_MASK.READ_CONTROL;
                    break;
                case ACCESS_MASK.ValueToBeTranslated0x10000000:
                    accessMask = ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION
                        | ACCESS_MASK.POLICY_VIEW_AUDIT_INFORMATION
                        | ACCESS_MASK.POLICY_GET_PRIVATE_INFORMATION
                        | ACCESS_MASK.POLICY_TRUST_ADMIN
                        | ACCESS_MASK.POLICY_CREATE_ACCOUNT
                        | ACCESS_MASK.POLICY_CREATE_SECRET
                        | ACCESS_MASK.POLICY_CREATE_PRIVILEGE
                        | ACCESS_MASK.POLICY_SET_DEFAULT_QUOTA_LIMITS
                        | ACCESS_MASK.POLICY_SET_AUDIT_REQUIREMENTS
                        | ACCESS_MASK.POLICY_AUDIT_LOG_ADMIN
                        | ACCESS_MASK.POLICY_SERVER_ADMIN
                        | ACCESS_MASK.POLICY_LOOKUP_NAMES
                        | ACCESS_MASK.DELETE
                        | ACCESS_MASK.READ_CONTROL
                        | ACCESS_MASK.WRITE_DAC
                        | ACCESS_MASK.WRITE_OWNER;
                    break;
            }
            return accessMask;
        }


        /// <summary>
        /// Translate access mask for account object.
        /// </summary>
        /// <param name="accessMask">ACCESS_MASK value to be translated</param>
        /// <returns>The result of translation to accessMask</returns>
        private static ACCESS_MASK TranslateAccessMaskForAccount(ACCESS_MASK accessMask)
        {
            switch (accessMask)
            {
                case ACCESS_MASK.ValueToBeTranslated0x80000000:
                    accessMask = ACCESS_MASK.ACCOUNT_VIEW
                        | ACCESS_MASK.READ_CONTROL;
                    break;
                case ACCESS_MASK.ValueToBeTranslated0x40000000:
                    accessMask = ACCESS_MASK.ACCOUNT_ADJUST_PRIVILEGES
                        | ACCESS_MASK.ACCOUNT_ADJUST_QUOTAS
                        | ACCESS_MASK.ACCOUNT_ADJUST_SYSTEM_ACCESS
                        | ACCESS_MASK.READ_CONTROL;
                    break;
                case ACCESS_MASK.ValueToBeTranslated0x20000000:
                    accessMask = ACCESS_MASK.READ_CONTROL;
                    break;
                case ACCESS_MASK.ValueToBeTranslated0x10000000:
                    accessMask = ACCESS_MASK.ACCOUNT_VIEW
                        | ACCESS_MASK.ACCOUNT_ADJUST_PRIVILEGES
                        | ACCESS_MASK.ACCOUNT_ADJUST_QUOTAS
                        | ACCESS_MASK.ACCOUNT_ADJUST_SYSTEM_ACCESS
                        | ACCESS_MASK.DELETE
                        | ACCESS_MASK.READ_CONTROL
                        | ACCESS_MASK.WRITE_DAC
                        | ACCESS_MASK.WRITE_OWNER;
                    break;
            }
            return accessMask;
        }


        /// <summary>
        /// Translate access mask for secret object.
        /// </summary>
        /// <param name="accessMask">ACCESS_MASK value to be translated</param>
        /// <returns>The result of translation to accessMask</returns>
        private static ACCESS_MASK TranslateAccessMaskForSecret(ACCESS_MASK accessMask)
        {
            switch (accessMask)
            {
                case ACCESS_MASK.ValueToBeTranslated0x80000000:
                    accessMask = ACCESS_MASK.SECRET_QUERY_VALUE
                        | ACCESS_MASK.READ_CONTROL;
                    break;
                case ACCESS_MASK.ValueToBeTranslated0x40000000:
                    accessMask = ACCESS_MASK.SECRET_SET_VALUE
                        | ACCESS_MASK.READ_CONTROL;
                    break;
                case ACCESS_MASK.ValueToBeTranslated0x20000000:
                    accessMask = ACCESS_MASK.READ_CONTROL;
                    break;
                case ACCESS_MASK.ValueToBeTranslated0x10000000:
                    accessMask = ACCESS_MASK.SECRET_QUERY_VALUE
                        | ACCESS_MASK.SECRET_SET_VALUE
                        | ACCESS_MASK.DELETE
                        | ACCESS_MASK.READ_CONTROL
                        | ACCESS_MASK.WRITE_DAC
                        | ACCESS_MASK.WRITE_OWNER;
                    break;
            }
            return accessMask;
        }


        /// <summary>
        /// Translate access mask for trusted domain object.
        /// </summary>
        /// <param name="accessMask">ACCESS_MASK value to be translated</param>
        /// <returns>The result of translation to accessMask</returns>
        private static ACCESS_MASK TranslateAccessMaskForTrustedDomain(ACCESS_MASK accessMask)
        {
            switch (accessMask)
            {
                case ACCESS_MASK.ValueToBeTranslated0x80000000:
                    accessMask = ACCESS_MASK.TRUSTED_QUERY_DOMAIN_NAME
                        | ACCESS_MASK.READ_CONTROL;
                    break;
                case ACCESS_MASK.ValueToBeTranslated0x40000000:
                    accessMask = ACCESS_MASK.TRUSTED_SET_CONTROLLERS
                        | ACCESS_MASK.TRUSTED_SET_POSIX
                        | ACCESS_MASK.READ_CONTROL;
                    break;
                case ACCESS_MASK.ValueToBeTranslated0x20000000:
                    accessMask = ACCESS_MASK.TRUSTED_QUERY_CONTROLLERS
                        | ACCESS_MASK.TRUSTED_QUERY_POSIX
                        | ACCESS_MASK.READ_CONTROL;
                    break;
                case ACCESS_MASK.ValueToBeTranslated0x10000000:
                    accessMask = ACCESS_MASK.TRUSTED_QUERY_DOMAIN_NAME
                        | ACCESS_MASK.TRUSTED_QUERY_CONTROLLERS
                        | ACCESS_MASK.TRUSTED_SET_CONTROLLERS
                        | ACCESS_MASK.TRUSTED_QUERY_POSIX
                        | ACCESS_MASK.TRUSTED_SET_POSIX
                        | ACCESS_MASK.TRUSTED_SET_AUTH
                        | ACCESS_MASK.TRUSTED_QUERY_AUTH
                        | ACCESS_MASK.DELETE
                        | ACCESS_MASK.READ_CONTROL
                        | ACCESS_MASK.WRITE_DAC
                        | ACCESS_MASK.WRITE_OWNER;
                    break;
            }
            return accessMask;
        }


        /// <summary>
        /// Create the _LSAPR_TRUSTED_DOMAIN_AUTH_BLOB using Rc4 authentication algorithm.
        /// </summary>
        /// <param name="currentOutgoingAuthInfos">Current outgoing authInfos</param>
        /// <param name="previousOutgoingAuthInfos">Previous outgoing authInfos</param>
        /// <param name="currentIncomingAuthInfos">Current incoming authInfos</param>
        /// <param name="previousIncomingAuthInfos">Previous incoming authInfos</param>
        /// <param name="sessionKey">Session key from smb transport</param>
        /// <returns>_LSAPR_TRUSTED_DOMAIN_AUTH_BLOB encrypted by Rc4</returns>
        [CLSCompliant(false)]
        public static _LSAPR_TRUSTED_DOMAIN_AUTH_BLOB CreateTrustedDomainAuthorizedBlob(
            _LSAPR_AUTH_INFORMATION[] currentOutgoingAuthInfos,
            _LSAPR_AUTH_INFORMATION[] previousOutgoingAuthInfos,
            _LSAPR_AUTH_INFORMATION[] currentIncomingAuthInfos,
            _LSAPR_AUTH_INFORMATION[] previousIncomingAuthInfos,
            byte[] sessionKey)
        {
            _LSAPR_TRUSTED_DOMAIN_AUTH_BLOB retVal= new _LSAPR_TRUSTED_DOMAIN_AUTH_BLOB();
            LsaTrustedDomainAuthBlob blob = new LsaTrustedDomainAuthBlob();
            Random random = new Random();
            blob.randomData = new byte[BLOB_AUTH_RANDOM_LENGTH]; //leads with 512 bytes of random data
            random.NextBytes(blob.randomData);
            blob.CurrentOutgoingAuthInfos = MarshalAuthInfos(currentOutgoingAuthInfos);
            blob.PreviousOutgoingAuthInfos = MarshalAuthInfos(previousOutgoingAuthInfos);
            blob.CurrentIncomingAuthInfos = MarshalAuthInfos(currentIncomingAuthInfos);
            blob.PreviousIncomingAuthInfos = MarshalAuthInfos(previousIncomingAuthInfos);

            blob.CountOutgoingAuthInfos = (uint)(currentOutgoingAuthInfos == null ? 0 : currentOutgoingAuthInfos.Length);
            //blob.ByteOffsetCurrentOutgoingAuthInfo is the sum of sizeof CountOutgoingAuthInfos, 
            //sizeof ByteOffsetCurrentOutgoingAuthInfo ,sizeof ByteOffsetCurrentOutgoingAuthInfo;
            blob.ByteOffsetCurrentOutgoingAuthInfo = (uint)(Marshal.SizeOf(blob.CountOutgoingAuthInfos) + Marshal.SizeOf(
                blob.ByteOffsetCurrentOutgoingAuthInfo) + Marshal.SizeOf(blob.ByteOffsetPreviousOutgoingAuthInfo));
            blob.ByteOffsetPreviousOutgoingAuthInfo = (uint)(blob.ByteOffsetCurrentOutgoingAuthInfo
                + blob.CurrentOutgoingAuthInfos.Length);
            blob.CountIncomingAuthInfos = (uint)(currentIncomingAuthInfos == null ? 0 : currentIncomingAuthInfos.Length);
            //same as blob.ByteOffsetCurrentOutgoingAuthInfo
            blob.ByteOffsetCurrentIncomingAuthInfo = (uint)(Marshal.SizeOf(blob.CountIncomingAuthInfos) + Marshal.SizeOf(
                blob.ByteOffsetCurrentIncomingAuthInfo) + Marshal.SizeOf(blob.ByteOffsetPreviousIncomingAuthInfo));
            blob.ByteOffsetPreviousIncomingAuthInfo = (uint)(blob.ByteOffsetCurrentIncomingAuthInfo
                + blob.CurrentIncomingAuthInfos.Length);
            blob.OutgoingAuthInfoSize = (uint)(blob.ByteOffsetPreviousOutgoingAuthInfo
                + blob.PreviousOutgoingAuthInfos.Length);
            blob.IncomingAuthInfoSize = (uint)(blob.ByteOffsetPreviousIncomingAuthInfo
                + blob.PreviousIncomingAuthInfos.Length);
            
            byte[] input = ArrayUtility.ConcatenateArrays(
                blob.randomData,
                TypeMarshal.ToBytes(blob.CountOutgoingAuthInfos),
                TypeMarshal.ToBytes(blob.ByteOffsetCurrentOutgoingAuthInfo),
                TypeMarshal.ToBytes(blob.ByteOffsetPreviousOutgoingAuthInfo),
                blob.CurrentOutgoingAuthInfos,
                blob.PreviousOutgoingAuthInfos,
                TypeMarshal.ToBytes(blob.CountIncomingAuthInfos),
                TypeMarshal.ToBytes(blob.ByteOffsetCurrentIncomingAuthInfo),
                TypeMarshal.ToBytes(blob.ByteOffsetPreviousIncomingAuthInfo),
                blob.CurrentIncomingAuthInfos,
                blob.PreviousIncomingAuthInfos,
                TypeMarshal.ToBytes(blob.OutgoingAuthInfoSize),
                TypeMarshal.ToBytes(blob.IncomingAuthInfoSize));

            using (RC4 rc4 = RC4.Create())
            {
                rc4.Key = sessionKey;
                retVal.AuthBlob = rc4.CreateEncryptor().TransformFinalBlock(input, 0, input.Length);
            }
            retVal.AuthSize = (uint)(retVal.AuthBlob.Length);

            return retVal;
        }


        /// <summary>
        /// Marshal an array of _LSAPR_AUTH_INFORMATION, alignment is 4
        /// </summary>
        /// <param name="authInfos">a array of _LSAPR_AUTH_INFORMATION</param>
        /// <returns>byte array</returns>
        private static byte[] MarshalAuthInfos(_LSAPR_AUTH_INFORMATION[] authInfos)
        {
            byte[] returnValue = null;
            if (authInfos == null || authInfos.Length == 0)
            {
                return new byte[0];
            }

            foreach (_LSAPR_AUTH_INFORMATION authInfo in authInfos)
            {
                returnValue = ArrayUtility.ConcatenateArrays(
                    returnValue,
                    TypeMarshal.ToBytes(authInfo),
                    //alignment is 4 bytes
                    new byte[((authInfo.AuthInfoLength + 3) & ~3) - authInfo.AuthInfoLength]);
            }
            return returnValue;
        }


        /// <summary>
        /// The encrypt_secret routine is used to encrypt a clear text value into cipher text prior to transmission.
        /// </summary>
        /// <param name="input">a _RPC_UNICODE_STRING struct</param>
        /// <param name="sessionKey">session key from smb transport</param>
        /// <param name="output">encrypted _RPC_UNICODE_STRING struct</param>
        [CLSCompliant(false)]
        public static void EncryptSecret(_RPC_UNICODE_STRING input, byte[] sessionKey, out _RPC_UNICODE_STRING output)
        {
            output = new _RPC_UNICODE_STRING();
            uint blockLength = CRYPT_BLOCK_LENGTH; //encrypt every 8 bytes
            uint keyLength = CRYPT_KEY_LENGTH; //used to compute output key
            uint keyIndex = 0;  //the key index of sessionKey

            byte[] buffer = new byte[blockLength];
            uint version = CRYPT_VERSION; //the version must be 1, defined in TD.

            //copy input.Length and version to buffer, each of them occupys 4 bytes
            //It's the encrypted data's header, when decrypting, the length and version would be decoded in 
            //the same format.
            Array.Copy(BitConverter.GetBytes(input.Length), buffer, sizeof(ushort));
            Array.Copy(BitConverter.GetBytes(version), 0, buffer, 4, sizeof(uint));

            byte[] outputBuffer = new byte[buffer.Length];
            DesEcbLmEncode(buffer, sessionKey, out outputBuffer);

            byte[] inputBuffer = new byte[input.Length];
            Buffer.BlockCopy(input.Buffer, 0, inputBuffer, 0, input.Length);

            keyIndex = AdvanceKey(keyIndex);
            uint remaining = input.Length;
            uint count = 0;

            while (remaining > blockLength)
            {
                DesEcbLmEncode(ArrayUtility.SubArray(inputBuffer, (int)(count * blockLength), (int)blockLength),
                    ArrayUtility.SubArray(sessionKey, (int)keyIndex, (int)keyLength), out buffer);
                outputBuffer = ArrayUtility.ConcatenateArrays(outputBuffer, buffer);
                keyIndex = AdvanceKey(keyIndex);
                count++;
                remaining -= blockLength;
            }
            if (remaining > 0)
            {
                byte[] temp = new byte[blockLength];
                Buffer.BlockCopy(inputBuffer, (int)(count * blockLength), temp, 0, (int)remaining);
                DesEcbLmEncode(temp, ArrayUtility.SubArray(sessionKey, (int)keyIndex, (int)keyLength),
                    out buffer);
                outputBuffer = ArrayUtility.ConcatenateArrays(outputBuffer, buffer);
            }
            output.Length = (ushort)outputBuffer.Length;
            output.MaximumLength = (ushort)outputBuffer.Length;
            //output.Length is the length in bytes of output.Buffer
            output.Buffer = new ushort[output.Length / sizeof(ushort)];
            Buffer.BlockCopy(outputBuffer, 0, output.Buffer, 0, output.Length);
        }


        /// <summary>
        /// The decrypt_secret routine is used to decrypt a cipher text value into clear text after receipt.
        /// </summary>
        /// <param name="input">encrypted _RPC_UNICODE_STRING struct</param>
        /// <param name="sessionKey">session key from smb transport</param>
        /// <param name="output">decrypted _RPC_UNICODE_STRING struct</param>
        /// <exception cref="ArgumentException">thrown ArgumentException if version mismatch</exception>
        [CLSCompliant(false)]
        public static void DecryptSecret(_RPC_UNICODE_STRING input, byte[] sessionKey, out _RPC_UNICODE_STRING output)
        {
            output = new _RPC_UNICODE_STRING();
            uint blockLength = CRYPT_BLOCK_LENGTH; //encrypt every 8 bytes
            uint keyLength = CRYPT_KEY_LENGTH; //used to compute output key
            uint keyIndex = 0;  //the key index of sessionKey

            byte[] buffer = new byte[blockLength];
            byte[] inputBuffer = new byte[input.Length];

            Buffer.BlockCopy(input.Buffer, 0, inputBuffer, 0, input.Length);
            DesEcbLmDecode(ArrayUtility.SubArray(inputBuffer, 0, (int)blockLength), sessionKey, out buffer);

            //output.Length and version are combined in 8 bytes as the header, each of them occupys 4 bytes.
            output.Length = TypeMarshal.ToStruct<ushort>(buffer);
            uint version = TypeMarshal.ToStruct<uint>(ArrayUtility.SubArray(buffer, 4, sizeof(uint)));

            byte[] outputBuffer = new byte[0];

            if (version != CRYPT_VERSION)
            {
                throw new ArgumentException("failed in checking version.");
            }

            //get the next 7 bytes in sessionKey as the key of DesEcbLm
            //if not enough,
            keyIndex = AdvanceKey(keyIndex);
            uint remaining = input.Length - blockLength;
            uint count = 1;

            while (remaining > blockLength)
            {
                DesEcbLmDecode(ArrayUtility.SubArray(inputBuffer, (int)(count * blockLength), (int)blockLength),
                    ArrayUtility.SubArray(sessionKey, (int)keyIndex, (int)keyLength), out buffer);
                outputBuffer = ArrayUtility.ConcatenateArrays(outputBuffer, buffer);
                keyIndex = AdvanceKey(keyIndex);
                count++;
                remaining -= blockLength;
            }
            if (remaining > 0)
            {
                byte[] temp = new byte[blockLength];
                Buffer.BlockCopy(inputBuffer, (int)(count * blockLength), temp, 0, (int)remaining);
                DesEcbLmDecode(temp, ArrayUtility.SubArray(sessionKey, (int)keyIndex, (int)keyLength),
                    out buffer);
                outputBuffer = ArrayUtility.ConcatenateArrays(outputBuffer, buffer);
            }

            output.Buffer = new ushort[outputBuffer.Length / sizeof(ushort)];
            output.MaximumLength = (ushort)outputBuffer.Length;
            Buffer.BlockCopy(outputBuffer, 0, output.Buffer, 0, outputBuffer.Length);
        }


        /// <summary>
        /// Computer key index in session key for EncryptSecret and DecryptSecret
        /// </summary>
        /// <param name="pos">last key index</param>
        /// <returns>current key index</returns>
        internal static uint AdvanceKey(uint pos)
        {
            uint keyBlockLen = CRYPT_KEY_LENGTH; //key circle length
            uint overRun, currPos, nextPos;
            uint sessionKeyLength = 16;

            pos = pos + keyBlockLen;
            currPos = pos;
            nextPos = pos + keyBlockLen;

            //
            if (nextPos > sessionKeyLength)
            {
                overRun = nextPos - sessionKeyLength;
                currPos = keyBlockLen - overRun;
            }
            return currPos;
        }


        /// <summary>
        /// DES_ECB_LM encoder
        /// </summary>
        /// <param name="input">plain text</param>
        /// <param name="encryptionKey">session key from smb transport</param>
        /// <param name="output">encrypted text</param>
        public static void DesEcbLmEncode(byte[] input, byte[] encryptionKey, out byte[] output)
        {
            byte[] outputKey = GenerateOutputKey(encryptionKey);
            using (DES des = DES.Create())
            {
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.None;

                des.Key = outputKey;
                output = des.CreateEncryptor().TransformFinalBlock(input, 0, input.Length);
            }
        }


        /// <summary>
        /// DES_ECB_LM decoder
        /// </summary>
        /// <param name="input">encrypted text</param>
        /// <param name="encryptionKey">session key from smb tranpsort</param>
        /// <param name="output">plain text</param>
        public static void DesEcbLmDecode(byte[] input, byte[] encryptionKey, out byte[] output)
        {
            byte[] outputKey = GenerateOutputKey(encryptionKey);
            using (DES des = DES.Create())
            {
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.None;

                des.Key = outputKey;
                output = des.CreateDecryptor().TransformFinalBlock(input, 0, input.Length);
            }
        }


        /// <summary>
        /// Generate outputKey for DES_ECB_LM algorithm
        /// </summary>
        /// <param name="encryptionKey">the key for encryption</param>
        /// <returns>output key for DES_ECB_LM algorithm</returns>
        public static byte[] GenerateOutputKey(byte[] encryptionKey)
        {
            byte[] inputKey = ArrayUtility.SubArray(encryptionKey, 0, 7);
            byte[] outputKey = new byte[8];

            outputKey[0] = (byte)(inputKey[0] >> 0x01);
            outputKey[1] = (byte)(((inputKey[0] & 0x01) << 6) | (inputKey[1] >> 2));
            outputKey[2] = (byte)(((inputKey[1] & 0x03) << 5) | (inputKey[2] >> 3));
            outputKey[3] = (byte)(((inputKey[2] & 0x07) << 4) | (inputKey[3] >> 4));
            outputKey[4] = (byte)(((inputKey[3] & 0x0F) << 3) | (inputKey[4] >> 5));
            outputKey[5] = (byte)(((inputKey[4] & 0x1F) << 2) | (inputKey[5] >> 6));
            outputKey[6] = (byte)(((inputKey[5] & 0x3F) << 1) | (inputKey[6] >> 7));
            outputKey[7] = (byte)(inputKey[6] & 0x7F);

            for (int i = 0; i < 8; i++)
            {
                outputKey[i] = (byte)((outputKey[i] << 1) & 0xfe);
            }

            for (int i = 0; i < 8; i++)
            {
                if (BitCount(outputKey[i]) % 2 == 0)
                    outputKey[i] = (byte)(outputKey[i] | 0x01);
                else
                    outputKey[i] = (byte)(outputKey[i] & 0xfe);
            }
            return outputKey;
        }


        /// <summary>
        /// Count the number of 1 bit in integer
        /// </summary>
        /// <param name="x">integer</param>
        /// <returns>the number of 1 bit</returns>
        internal static int BitCount(int x)
        {
            x = (x & 0x55555555) + ((x >> 1) & 0x55555555); // 0-2 in 2 bits 
            x = (x & 0x33333333) + ((x >> 2) & 0x33333333); // 0-4 in 4 bits 
            x = (x & 0x0f0f0f0f) + ((x >> 4) & 0x0f0f0f0f); // 0-8 in 8 bits 
            x = (x & 0x00ff00ff) + ((x >> 8) & 0x00ff00ff); // 0-16 in 16 bits 
            x = (x & 0x0000ffff) + ((x >> 16) & 0x0000ffff); // 0-31 in 32 bits 
            return x; 
        }


        /// <summary>
        /// Creates an instance of request stub upon opnum received
        /// </summary>
        /// <param name="opnum"> opnum received</param>
        /// <returns>an instance of request stub.</returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode")]
        internal static LsaRequestStub CreateLsaRequestStub(LsaMethodOpnums opnum)
        {
            LsaRequestStub requestStub = null;
            switch (opnum)
            {

                case LsaMethodOpnums.LsarClose:
                    requestStub = new LsarCloseRequest();
                    break;

                case LsaMethodOpnums.Opnum1NotUsedOnWire:
                    requestStub = new Opnum1NotUsedOnWireRequest();
                    break;

                case LsaMethodOpnums.LsarEnumeratePrivileges:
                    requestStub = new LsarEnumeratePrivilegesRequest();
                    break;

                case LsaMethodOpnums.LsarQuerySecurityObject:
                    requestStub = new LsarQuerySecurityObjectRequest();
                    break;

                case LsaMethodOpnums.LsarSetSecurityObject:
                    requestStub = new LsarSetSecurityObjectRequest();
                    break;

                case LsaMethodOpnums.Opnum5NotUsedOnWire:
                    requestStub = new Opnum5NotUsedOnWireRequest();
                    break;

                case LsaMethodOpnums.LsarOpenPolicy:
                    requestStub = new LsarOpenPolicyRequest();
                    break;

                case LsaMethodOpnums.LsarQueryInformationPolicy:
                    requestStub = new LsarQueryInformationPolicyRequest();
                    break;

                case LsaMethodOpnums.LsarSetInformationPolicy:
                    requestStub = new LsarSetInformationPolicyRequest();
                    break;

                case LsaMethodOpnums.Opnum9NotUsedOnWire:
                    requestStub = new Opnum9NotUsedOnWireRequest();
                    break;

                case LsaMethodOpnums.LsarCreateAccount:
                    requestStub = new LsarCreateAccountRequest();
                    break;

                case LsaMethodOpnums.LsarEnumerateAccounts:
                    requestStub = new LsarEnumerateAccountsRequest();
                    break;

                case LsaMethodOpnums.LsarCreateTrustedDomain:
                    requestStub = new LsarCreateTrustedDomainRequest();
                    break;

                case LsaMethodOpnums.LsarEnumerateTrustedDomains:
                    requestStub = new LsarEnumerateTrustedDomainsRequest();
                    break;

                case LsaMethodOpnums.LsarLookupNames:
                    requestStub = new LsarLookupNamesRequest();
                    break;

                case LsaMethodOpnums.LsarLookupSids:
                    requestStub = new LsarLookupSidsRequest();
                    break;

                case LsaMethodOpnums.LsarCreateSecret:
                    requestStub = new LsarCreateSecretRequest();
                    break;

                case LsaMethodOpnums.LsarOpenAccount:
                    requestStub = new LsarOpenAccountRequest();
                    break;

                case LsaMethodOpnums.LsarEnumeratePrivilegesAccount:
                    requestStub = new LsarEnumeratePrivilegesAccountRequest();
                    break;

                case LsaMethodOpnums.LsarAddPrivilegesToAccount:
                    requestStub = new LsarAddPrivilegesToAccountRequest();
                    break;

                case LsaMethodOpnums.LsarRemovePrivilegesFromAccount:
                    requestStub = new LsarRemovePrivilegesFromAccountRequest();
                    break;

                case LsaMethodOpnums.Opnum21NotUsedOnWire:
                    requestStub = new Opnum21NotUsedOnWireRequest();
                    break;

                case LsaMethodOpnums.Opnum22NotUsedOnWire:
                    requestStub = new Opnum22NotUsedOnWireRequest();
                    break;

                case LsaMethodOpnums.LsarGetSystemAccessAccount:
                    requestStub = new LsarGetSystemAccessAccountRequest();
                    break;

                case LsaMethodOpnums.LsarSetSystemAccessAccount:
                    requestStub = new LsarSetSystemAccessAccountRequest();
                    break;

                case LsaMethodOpnums.LsarOpenTrustedDomain:
                    requestStub = new LsarOpenTrustedDomainRequest();
                    break;

                case LsaMethodOpnums.LsarQueryInfoTrustedDomain:
                    requestStub = new LsarQueryInfoTrustedDomainRequest();
                    break;

                case LsaMethodOpnums.LsarSetInformationTrustedDomain:
                    requestStub = new LsarSetInformationTrustedDomainRequest();
                    break;

                case LsaMethodOpnums.LsarOpenSecret:
                    requestStub = new LsarOpenSecretRequest();
                    break;

                case LsaMethodOpnums.LsarSetSecret:
                    requestStub = new LsarSetSecretRequest();
                    break;

                case LsaMethodOpnums.LsarQuerySecret:
                    requestStub = new LsarQuerySecretRequest();
                    break;

                case LsaMethodOpnums.LsarLookupPrivilegeValue:
                    requestStub = new LsarLookupPrivilegeValueRequest();
                    break;

                case LsaMethodOpnums.LsarLookupPrivilegeName:
                    requestStub = new LsarLookupPrivilegeNameRequest();
                    break;

                case LsaMethodOpnums.LsarLookupPrivilegeDisplayName:
                    requestStub = new LsarLookupPrivilegeDisplayNameRequest();
                    break;

                case LsaMethodOpnums.LsarDeleteObject:
                    requestStub = new LsarDeleteObjectRequest();
                    break;

                case LsaMethodOpnums.LsarEnumerateAccountsWithUserRight:
                    requestStub = new LsarEnumerateAccountsWithUserRightRequest();
                    break;

                case LsaMethodOpnums.LsarEnumerateAccountRights:
                    requestStub = new LsarEnumerateAccountRightsRequest();
                    break;

                case LsaMethodOpnums.LsarAddAccountRights:
                    requestStub = new LsarAddAccountRightsRequest();
                    break;

                case LsaMethodOpnums.LsarRemoveAccountRights:
                    requestStub = new LsarRemoveAccountRightsRequest();
                    break;

                case LsaMethodOpnums.LsarQueryTrustedDomainInfo:
                    requestStub = new LsarQueryTrustedDomainInfoRequest();
                    break;

                case LsaMethodOpnums.LsarSetTrustedDomainInfo:
                    requestStub = new LsarSetTrustedDomainInfoRequest();
                    break;

                case LsaMethodOpnums.LsarDeleteTrustedDomain:
                    requestStub = new LsarDeleteTrustedDomainRequest();
                    break;

                case LsaMethodOpnums.LsarStorePrivateData:
                    requestStub = new LsarStorePrivateDataRequest();
                    break;

                case LsaMethodOpnums.LsarRetrievePrivateData:
                    requestStub = new LsarRetrievePrivateDataRequest();
                    break;

                case LsaMethodOpnums.LsarOpenPolicy2:
                    requestStub = new LsarOpenPolicy2Request();
                    break;

                case LsaMethodOpnums.LsarGetUserName:
                    requestStub = new LsarGetUserNameRequest();
                    break;

                case LsaMethodOpnums.LsarQueryInformationPolicy2:
                    requestStub = new LsarQueryInformationPolicy2Request();
                    break;

                case LsaMethodOpnums.LsarSetInformationPolicy2:
                    requestStub = new LsarSetInformationPolicy2Request();
                    break;

                case LsaMethodOpnums.LsarQueryTrustedDomainInfoByName:
                    requestStub = new LsarQueryTrustedDomainInfoByNameRequest();
                    break;

                case LsaMethodOpnums.LsarSetTrustedDomainInfoByName:
                    requestStub = new LsarSetTrustedDomainInfoByNameRequest();
                    break;

                case LsaMethodOpnums.LsarEnumerateTrustedDomainsEx:
                    requestStub = new LsarEnumerateTrustedDomainsExRequest();
                    break;

                case LsaMethodOpnums.LsarCreateTrustedDomainEx:
                    requestStub = new LsarCreateTrustedDomainExRequest();
                    break;

                case LsaMethodOpnums.Opnum52NotUsedOnWire:
                    requestStub = new Opnum52NotUsedOnWireRequest();
                    break;

                case LsaMethodOpnums.LsarQueryDomainInformationPolicy:
                    requestStub = new LsarQueryDomainInformationPolicyRequest();
                    break;

                case LsaMethodOpnums.LsarSetDomainInformationPolicy:
                    requestStub = new LsarSetDomainInformationPolicyRequest();
                    break;

                case LsaMethodOpnums.LsarOpenTrustedDomainByName:
                    requestStub = new LsarOpenTrustedDomainByNameRequest();
                    break;

                case LsaMethodOpnums.Opnum56NotUsedOnWire:
                    requestStub = new Opnum56NotUsedOnWireRequest();
                    break;

                case LsaMethodOpnums.LsarLookupSids2:
                    requestStub = new LsarLookupSids2Request();
                    break;

                case LsaMethodOpnums.LsarLookupNames2:
                    requestStub = new LsarLookupNames2Request();
                    break;

                case LsaMethodOpnums.LsarCreateTrustedDomainEx2:
                    requestStub = new LsarCreateTrustedDomainEx2Request();
                    break;

                case LsaMethodOpnums.Opnum60NotUsedOnWire:
                    requestStub = new Opnum60NotUsedOnWireRequest();
                    break;

                case LsaMethodOpnums.Opnum61NotUsedOnWire:
                    requestStub = new Opnum61NotUsedOnWireRequest();
                    break;

                case LsaMethodOpnums.Opnum62NotUsedOnWire:
                    requestStub = new Opnum62NotUsedOnWireRequest();
                    break;

                case LsaMethodOpnums.Opnum63NotUsedOnWire:
                    requestStub = new Opnum63NotUsedOnWireRequest();
                    break;

                case LsaMethodOpnums.Opnum64NotUsedOnWire:
                    requestStub = new Opnum64NotUsedOnWireRequest();
                    break;

                case LsaMethodOpnums.Opnum65NotUsedOnWire:
                    requestStub = new Opnum65NotUsedOnWireRequest();
                    break;

                case LsaMethodOpnums.Opnum66NotUsedOnWire:
                    requestStub = new Opnum66NotUsedOnWireRequest();
                    break;

                case LsaMethodOpnums.Opnum67NotUsedOnWire:
                    requestStub = new Opnum67NotUsedOnWireRequest();
                    break;

                case LsaMethodOpnums.LsarLookupNames3:
                    requestStub = new LsarLookupNames3Request();
                    break;

                case LsaMethodOpnums.Opnum69NotUsedOnWire:
                    requestStub = new Opnum69NotUsedOnWireRequest();
                    break;

                case LsaMethodOpnums.Opnum70NotUsedOnWire:
                    requestStub = new Opnum70NotUsedOnWireRequest();
                    break;

                case LsaMethodOpnums.Opnum71NotUsedOnWire:
                    requestStub = new Opnum71NotUsedOnWireRequest();
                    break;

                case LsaMethodOpnums.Opnum72NotUsedOnWire:
                    requestStub = new Opnum72NotUsedOnWireRequest();
                    break;

                case LsaMethodOpnums.LsarQueryForestTrustInformation:
                    requestStub = new LsarQueryForestTrustInformationRequest();
                    break;

                case LsaMethodOpnums.LsarSetForestTrustInformation:
                    requestStub = new LsarSetForestTrustInformationRequest();
                    break;

                case LsaMethodOpnums.Opnum75NotUsedOnWire:
                    requestStub = new Opnum75NotUsedOnWireRequest();
                    break;

                case LsaMethodOpnums.LsarLookupSids3:
                    requestStub = new LsarLookupSids3Request();
                    break;

                case LsaMethodOpnums.LsarLookupNames4:
                    requestStub = new LsarLookupNames4Request();
                    break;
            }

            return requestStub;
        }
    }
}
