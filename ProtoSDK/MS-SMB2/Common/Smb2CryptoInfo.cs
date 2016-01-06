// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    public class Smb2CryptoInfo
    {
        internal DialectRevision Dialect;
        internal byte[] SessionKey;
        internal byte[] SigningKey;
        internal byte[] ServerInKey;
        internal byte[] ServerOutKey;
        internal byte[] ApplicationKey;
        internal EncryptionAlgorithm CipherId;

        internal bool DisableVerifySignature;

        internal bool EnableSessionSigning;
        internal bool EnableSessionEncryption;
        internal List<uint> EnableTreeEncryption = new List<uint>();
        
        /// <summary>
        /// Generate encrypt/decrypt and signing keys according to the smb2 dialect, cryptographicKey(session key), PreauthIntegrityHashValue and existing cryptInfo
        /// If the param cryptoInfo is not null, then the param cipherId will be ignored. 
        /// The binding session should use the same cipherId as the master session.
        /// </summary>
        /// <param name="dialect">Smb2 dialect</param>
        /// <param name="cryptographicKey">The key to derive new encrypt/decrypt/signing key </param>
        /// <param name="enableSigning">True if signing is enabled, otherwise false</param>
        /// <param name="enableEncryption">True if encrypt is enabled, otherwise false</param>
        /// <param name="cryptoInfo">The existing cryptoInfo. This is for the binding session</param>        
        /// <param name="preauthIntegrityHashValue">The preauthentication integrity hash value, for smb dialect 311 only </param>
        /// <param name="cipherId">The ID of the cipher that was negotiated for this connection</param>
        public Smb2CryptoInfo(
            DialectRevision dialect, 
            byte[] cryptographicKey, 
            bool enableSigning, 
            bool enableEncryption, 
            bool disableVerifySignature,
            Smb2CryptoInfo cryptoInfo = null,
            byte[] preauthIntegrityHashValue = null,
            EncryptionAlgorithm cipherId = EncryptionAlgorithm.ENCRYPTION_AES128_CCM)
        {
            if (dialect >= DialectRevision.Smb311 && dialect != DialectRevision.Smb2Unknown && preauthIntegrityHashValue == null)
            {
                if (cryptoInfo == null && enableEncryption) // For the alternative channel, the cryptoinfo should be the same as the main channel.
                {
                    throw new ArgumentNullException("For SMB 3.11, null preauthIntegrityHashValue is not allowed when encryption is enabled.");
                }

                if(enableSigning) // For the alternative channel, the signingkey should be recalculated.
                {
                    throw new ArgumentNullException("For SMB 3.11, null preauthIntegrityHashValue is not allowed when signing is enabled.");
                }
            }

            DisableVerifySignature = disableVerifySignature;
            Dialect = dialect;

            SessionKey = cryptographicKey;

            // TD indicates that when signing the message the protocol uses
            // the first 16 bytes of the cryptographic key for this authenticated context. 
            // If the cryptographic key is less than 16 bytes, it is right-padded with zero bytes.
            if (SessionKey.Length < 16)
                SessionKey = SessionKey.Concat(new byte[16 - SessionKey.Length]).ToArray();

            else if (SessionKey.Length > 16)
                SessionKey = SessionKey.Take(16).ToArray();

            if (Dialect == DialectRevision.Smb2002 || Dialect == DialectRevision.Smb21)
                SigningKey = SessionKey;

            else if (Dialect == DialectRevision.Smb30 || Dialect == DialectRevision.Smb302)
                SigningKey = SP8001008KeyDerivation.CounterModeHmacSha256KeyDerive(
                                SessionKey,
                                Encoding.ASCII.GetBytes("SMB2AESCMAC\0"),
                                Encoding.ASCII.GetBytes("SmbSign\0"),
                                128);
            else // dialect == SMB311
            {                
                SigningKey = SP8001008KeyDerivation.CounterModeHmacSha256KeyDerive(
                                SessionKey,
                                Encoding.ASCII.GetBytes("SMBSigningKey\0"),
                                preauthIntegrityHashValue,
                                128);
            }
            if (cryptoInfo == null)
            {
                if (dialect >= DialectRevision.Smb311)
                {
                    CipherId = cipherId;
                }
                else
                {
                    // for pre SMB 3.11 dialects, use AES-128-CCM for encryption
                    CipherId = EncryptionAlgorithm.ENCRYPTION_AES128_CCM;
                }
                ServerInKey = SP8001008KeyDerivation.CounterModeHmacSha256KeyDerive(
                                SessionKey,
                                // If Connection.Dialect is "3.1.1", the case-sensitive ASCII string "SMBC2SCipherKey" as the label; 
                                // otherwise, the case-sensitive ASCII string "SMB2AESCCM" as the label.
                                Dialect == DialectRevision.Smb311 ? Encoding.ASCII.GetBytes("SMBC2SCipherKey\0") : Encoding.ASCII.GetBytes("SMB2AESCCM\0"),
                                // If Connection.Dialect is "3.1.1", Session.PreauthIntegrityHashValue as the context; 
                                // otherwise, the case-sensitive ASCII string "ServerIn " as context for the algorithm (note the blank space at the end).
                                Dialect == DialectRevision.Smb311 ? preauthIntegrityHashValue : Encoding.ASCII.GetBytes("ServerIn \0"),
                                128);

                ServerOutKey = SP8001008KeyDerivation.CounterModeHmacSha256KeyDerive(
                                SessionKey,
                                // If Connection.Dialect is "3.100", the case-sensitive ASCII string "SMBS2CCipherKey" as the label; 
                                // otherwise, the case-sensitive ASCII string "SMB2AESCCM" as the label.
                                Dialect == DialectRevision.Smb311 ? Encoding.ASCII.GetBytes("SMBS2CCipherKey\0") : Encoding.ASCII.GetBytes("SMB2AESCCM\0"),
                                // If Connection.Dialect is "3.100", Session.PreauthIntegrityHashValue as the context; 
                                // otherwise, the case-sensitive ASCII string "ServerOut" as context for the algorithm.
                                Dialect == DialectRevision.Smb311 ? preauthIntegrityHashValue : Encoding.ASCII.GetBytes("ServerOut\0"),
                                128);

                ApplicationKey = SP8001008KeyDerivation.CounterModeHmacSha256KeyDerive(
                                SessionKey,
                                // If Connection.Dialect is "3.1.1", the case-sensitive ASCII string "SMBAppKey" as the label; 
                                // otherwise, the case-sensitive ASCII string "SMB2APP" as the label.
                                Dialect == DialectRevision.Smb311 ? Encoding.ASCII.GetBytes("SMBAppKey\0") : Encoding.ASCII.GetBytes("SMB2APP\0"),
                                // If Connection.Dialect is "3.1.1", Session.PreauthIntegrityHashValue as the context; 
                                // otherwise, the case-sensitive ASCII string "SmbRpc" as context for the algorithm.
                                Dialect == DialectRevision.Smb311 ? preauthIntegrityHashValue : Encoding.ASCII.GetBytes("SmbRpc\0"),
                                128);
            }
            else
            {
                // Reuse encryption/decryption key that is generated in first channel's session setup 
                // when send encrypted request in alternative channels
                CipherId = cryptoInfo.CipherId;
                ServerInKey = cryptoInfo.ServerInKey;
                ServerOutKey = cryptoInfo.ServerOutKey;
                ApplicationKey = cryptoInfo.ApplicationKey;
            }

            EnableSessionSigning = enableSigning;
            EnableSessionEncryption = enableEncryption;
        }
    }
}
