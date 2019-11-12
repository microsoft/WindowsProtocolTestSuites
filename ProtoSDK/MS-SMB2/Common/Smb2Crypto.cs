// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2.Packets;
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
        /// Sign, compress and encrypt for Single or Compound packet.
        /// </summary>
        public static Smb2Packet SignCompressAndEncrypt(Smb2Packet originalPacket, Dictionary<ulong, Smb2CryptoInfo> cryptoInfoTable, Smb2CompressionInfo compressioninfo, Smb2Role role)
        {
            ulong sessionId;
            bool isCompound = false;
            bool notEncryptNotSign = false;
            bool notEncrypt = false;
            var compressedPacket = originalPacket;

            if (originalPacket is Smb2SinglePacket)
            {
                Smb2SinglePacket singlePacket = originalPacket as Smb2SinglePacket;
                sessionId = singlePacket.Header.SessionId;
                // [MS-SMB2] Section 3.2.4.1.8, the request being sent is SMB2 NEGOTIATE, 
                // or the request being sent is SMB2 SESSION_SETUP with the SMB2_SESSION_FLAG_BINDING bit set in the Flags field, 
                // the client MUST NOT encrypt the message
                if (sessionId == 0 ||
                    (singlePacket.Header.Command == Smb2Command.NEGOTIATE && (singlePacket is Smb2NegotiateRequestPacket)))
                {
                    notEncryptNotSign = true;
                }
                else if ((singlePacket.Header.Command == Smb2Command.SESSION_SETUP && (singlePacket is Smb2SessionSetupRequestPacket) &&
                    (singlePacket as Smb2SessionSetupRequestPacket).PayLoad.Flags == SESSION_SETUP_Request_Flags.SESSION_FLAG_BINDING))
                {
                    notEncrypt = true;
                }
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

            if (sessionId == 0 || notEncryptNotSign || !cryptoInfoTable.ContainsKey(sessionId))
            {
                if (originalPacket is Smb2CompressiblePacket)
                {
                    compressedPacket = Smb2Compression.Compress(originalPacket as Smb2CompressiblePacket, compressioninfo, role);
                }
                return compressedPacket;
            }

            Smb2CryptoInfo cryptoInfo = cryptoInfoTable[sessionId];

            #region Encrypt
            // Try to encrypt the message whenever the encryption is supported or not except for sesstion setup. 
            // If it's not supported, do it for negative test.
            // For compound packet, the encryption is done for the entire message.
            if (!notEncrypt)
            {
                if (originalPacket is Smb2CompressiblePacket)
                {
                    compressedPacket = Smb2Compression.Compress(originalPacket as Smb2CompressiblePacket, compressioninfo, role);
                }
                var encryptedPacket = Encrypt(sessionId, cryptoInfo, role, compressedPacket, originalPacket);
                if (encryptedPacket != null)
                {
                    return encryptedPacket;
                }
            }
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
            if (originalPacket is Smb2CompressiblePacket)
            {
                compressedPacket = Smb2Compression.Compress(originalPacket as Smb2CompressiblePacket, compressioninfo, role);
            }
            return compressedPacket;
        }

        public static byte[] Decrypt(byte[] bytes, Dictionary<ulong, Smb2CryptoInfo> cryptoInfoTable, Smb2Role role, out Transform_Header transformHeader)
        {
            var encryptedPacket = new Smb2EncryptedPacket();

            int consumedLen;
            int expectedLen;

            encryptedPacket.FromBytes(bytes, out consumedLen, out expectedLen);

            transformHeader = encryptedPacket.Header;

            // For client: If the Flags/EncryptionAlgorithm in the SMB2 TRANSFORM_HEADER is not 0x0001, the client MUST discard the message.
            // For server: If the Flags/EncryptionAlgorithm in the SMB2 TRANSFORM_HEADER is not 0x0001, the server MUST disconnect the connection.
            if (transformHeader.Flags != TransformHeaderFlags.Encrypted)
            {
                throw new InvalidOperationException(
                    String.Format(
                        "Flags/EncryptionAlgorithm field is invalid for encrypted message. Expected value 0x0001, actual {0}.",
                        (ushort)transformHeader.Flags
                    )
                );
            }

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
                // Auth data is Transform_Header start from Nonce, excluding ProtocolId and Signature.
                var authData = Smb2Utility.MarshalStructure(transformHeader).Skip((int)Marshal.OffsetOf<Transform_Header>("Nonce")).ToArray();
                return bcrypt.Decrypt(encryptedPacket.EncryptdData, transformHeader.Nonce.ToByteArray().Take(nonceLength).ToArray(), authData, transformHeader.Signature);
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

        private static Smb2EncryptedPacket Encrypt(ulong sessionId, Smb2CryptoInfo cryptoInfo, Smb2Role role, Smb2Packet packet, Smb2Packet packetBeforCompression)
        {
            Packet_Header header;
            if (packetBeforCompression is Smb2SinglePacket)
            {
                header = (packetBeforCompression as Smb2SinglePacket).Header;
            }
            else if (packetBeforCompression is Smb2CompoundPacket)
            {
                header = (packetBeforCompression as Smb2CompoundPacket).Packets[0].Header;
            }
            else
            {
                throw new InvalidOperationException("Unsupported SMB2 packet type!");
            }

            // Encrypt all messages after session setup if global encryption enabled.
            // Encrypt all messages after tree connect if global encryption disabled but share encryption enabled.
            if ((cryptoInfo.EnableSessionEncryption
                 || (cryptoInfo.EnableTreeEncryption.Contains(header.TreeId)
                     && header.Command != Smb2Command.TREE_CONNECT
                     )
                 )
             )
            {
                using (var bcrypt = new BCryptAlgorithm("AES"))
                {
                    byte[] originalBinary = packet.ToBytes();
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

                    var encryptedPacket = new Smb2EncryptedPacket();
                    encryptedPacket.Header = transformHeader;
                    encryptedPacket.EncryptdData = output;

                    return encryptedPacket;
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