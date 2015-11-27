// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2.Common
{
    public static class Smb2Crypto
    {
        public enum CryptoOperationType
        {
            Encrypt,
            Decrypt
        }
        /// <summary>
        /// Sign and encrypt for Single or Compound packet.
        /// </summary>
        public static byte[] SignAndEncrypt(Smb2Packet originalPacket, Dictionary<ulong, Smb2CryptoInfo> cryptoInfoTable, Smb2Role role)
        {
            ulong sessionId;
            bool isCompound = false;

            if (originalPacket is Smb2SinglePacket)
            {
                sessionId = (originalPacket as Smb2SinglePacket).Header.SessionId;
            }
            else if (originalPacket is Smb2CompoundPacket)
            {
                isCompound = true;
                // The subsequent request in compound packet should use the SessionId of the first request for encryption
                sessionId = (originalPacket as Smb2CompoundPacket).Packets[0].Header.SessionId;
            }
            else
            {
                throw new NotImplementedException(string.Format("Signing and encryption are not implemented for packet: {0}", originalPacket.ToString()));
            }

            // Signing and encryption not required if session id is not available yet
            if (sessionId == 0 || !cryptoInfoTable.ContainsKey(sessionId))
                return originalPacket.ToBytes();

            Smb2CryptoInfo cryptoInfo = cryptoInfoTable[sessionId];

            #region Encrypt
            // Try to encrypt the message whenever the encryption is supported or not. 
            // If it's not supported, do it for negative test.
            // For compound packet, the encryption is done for the entire message.
            byte[] encryptedBinary = Encrypt(sessionId, cryptoInfo, role, originalPacket);
            if (encryptedBinary != null)
                return encryptedBinary;
            #endregion

            #region Sign
            if (cryptoInfo.EnableSessionSigning)
            {
                if (isCompound)
                {
                    // Calculate signature for every packet in the chain
                    foreach (Smb2SinglePacket packet in (originalPacket as Smb2CompoundPacket).Packets)
                    {
                        // If the packet is the first one in the chain or the unralated one, use its own SessionId for sign and encrypt
                        // If it's not the first one and it's the related one, use the SessionId of the first request for sign and encrypt
                        if (!packet.Header.Flags.HasFlag(Packet_Header_Flags_Values.FLAGS_RELATED_OPERATIONS))
                        {
                            sessionId = packet.Header.SessionId;
                            cryptoInfo = cryptoInfoTable[sessionId];
                        }

                        packet.Header.Signature = Sign(cryptoInfo, packet.ToBytes());
                    }
                }
                else
                {
                    (originalPacket as Smb2SinglePacket).Header.Signature = Sign(cryptoInfo, originalPacket.ToBytes());
                }
            }
            #endregion

            return originalPacket.ToBytes();
        }


        public static byte[] Decrypt(byte[] bytes, Dictionary<ulong, Smb2CryptoInfo> cryptoInfoTable, Smb2Role role)
        {
            Transform_Header transformHeader = Smb2Utility.UnmarshalStructure<Transform_Header>(bytes);

            if (transformHeader.SessionId == 0 || !cryptoInfoTable.ContainsKey(transformHeader.SessionId))
                throw new InvalidOperationException("Invalid SessionId in TRANSFORM_HEADER.");

            Smb2CryptoInfo cryptoInfo = cryptoInfoTable[transformHeader.SessionId];

            using (var bcrypt = new BCryptAlgorithm("AES"))
            {
                int nonceLength = 0;
                BCryptCipherMode mode = BCryptCipherMode.NotAvailable;
                GetCryptoParams(cryptoInfo, CryptoOperationType.Decrypt, out mode, out nonceLength);
                bcrypt.Mode = mode;
                bcrypt.Key = role == Smb2Role.Server ? cryptoInfo.ServerInKey : cryptoInfo.ServerOutKey;
                return bcrypt.Decrypt(bytes.Skip(52).ToArray(), transformHeader.Nonce.ToByteArray().Take(nonceLength).ToArray(), bytes.Skip(20).Take(32).ToArray(), transformHeader.Signature);
            }
        }

        private static byte[] Sign(Smb2CryptoInfo cryptoInfo, byte[] original)
        {
            if (Smb2Utility.IsSmb2Family(cryptoInfo.Dialect))
            {
                // [MS-SMB2] 3.1.4.1 
                // 3. If Connection.Dialect is "2.002" or "2.100", the sender MUST compute a 32-byte hash using HMAC-SHA256 over the entire message, 
                HMACSHA256 hmacSha = new HMACSHA256(cryptoInfo.SigningKey);
                return hmacSha.ComputeHash(original);
            }
            else
            {
                // [MS-SMB2] 3.1.4.1 
                // 2. If Connection.Dialect belongs to the SMB 3.x dialect family, the sender MUST compute a 16-byte hash using AES-128-CMAC over the entire message
                return AesCmac128.ComputeHash(cryptoInfo.SigningKey, original);
            }
        }

        private static byte[] Encrypt(ulong sessionId, Smb2CryptoInfo cryptoInfo, Smb2Role role, Smb2Packet originalPacket)
        {
            Packet_Header header;
            if (originalPacket is Smb2SinglePacket)
            {
                header = (originalPacket as Smb2SinglePacket).Header;
            }
            else
            {
                header = (originalPacket as Smb2CompoundPacket).Packets[0].Header;
            }

            // Encrypt all messages after session setup if global encryption enabled.
            // Encrypt all messages after tree connect if global encryption disabled but share encryption enabled.
            if (header.Command != Smb2Command.NEGOTIATE
             && header.Command != Smb2Command.SESSION_SETUP
             && (cryptoInfo.EnableSessionEncryption
                 || (cryptoInfo.EnableTreeEncryption.Contains(header.TreeId)
                     && header.Command != Smb2Command.TREE_CONNECT
                     )
                 )
             )
            {
                using (var bcrypt = new BCryptAlgorithm("AES"))
                {
                    byte[] originalBinary = originalPacket.ToBytes();
                    Transform_Header transformHeader = new Transform_Header
                    {
                        ProtocolId = Smb2Consts.ProtocolIdInTransformHeader,
                        OriginalMessageSize = (uint)originalBinary.Length,
                        SessionId = sessionId,
                        Signature = new byte[16]
                    };

                    if (cryptoInfo.Dialect == DialectRevision.Smb311)
                    {
                        transformHeader.Flags = TransformHeaderFlags.Encrypted;
                    }
                    else
                    {
                        transformHeader.EncryptionAlgorithm = EncryptionAlgorithm.ENCRYPTION_AES128_CCM;
                    }

                    byte[] tag;

                    int nonceLength = 0;
                    BCryptCipherMode mode = BCryptCipherMode.NotAvailable;
                    GetCryptoParams(cryptoInfo, CryptoOperationType.Encrypt, out mode, out nonceLength);
                    bcrypt.Mode = mode;
                    bcrypt.Key = role == Smb2Role.Server ? cryptoInfo.ServerOutKey : cryptoInfo.ServerInKey;
                    // The reserved field (5 bytes for CCM, 4 bytes for GCM) must be set to zero.
                    byte[] nonce = new byte[16];
                    Buffer.BlockCopy(Guid.NewGuid().ToByteArray(), 0, nonce, 0, nonceLength);
                    transformHeader.Nonce = new Guid(nonce);

                    byte[] output = bcrypt.Encrypt(
                        originalBinary,
                        transformHeader.Nonce.ToByteArray().Take(nonceLength).ToArray(),
                        // Use the fields including and after Nonce field as auth data
                        Smb2Utility.MarshalStructure(transformHeader).Skip(20).ToArray(),
                        // Signature is 16 bytes in length
                        16,
                        out tag);
                    transformHeader.Signature = tag;
                    return Smb2Utility.MarshalStructure(transformHeader).Concat(output).ToArray();
                }
            }

            // Return null if the message is not required to be encrypted.
            return null;
        }

        private static void GetCryptoParams(Smb2CryptoInfo cryptoInfo, CryptoOperationType operationType, out BCryptCipherMode mode, out int nonceLength)
        {
            if (cryptoInfo.CipherId == EncryptionAlgorithm.ENCRYPTION_AES128_CCM)
            {
                mode = BCryptCipherMode.CCM;
                nonceLength = Smb2Consts.AES128CCM_Nonce_Length;
            }
            else if (cryptoInfo.CipherId == EncryptionAlgorithm.ENCRYPTION_AES128_GCM)
            {
                mode = BCryptCipherMode.GCM;
                nonceLength = Smb2Consts.AES128GCM_Nonce_Length;
            }
            else
            {
                throw new InvalidOperationException(String.Format(
                    "No valid encryption algorithm is set in Smb2CryptoInfo when trying to {0}", operationType));
            }
        }
    }
}
