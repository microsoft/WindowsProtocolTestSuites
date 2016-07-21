// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Security.Cryptography;
using System.Collections.Generic;

using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Samr
{
    /// <summary>
    /// Password type, encrypt with Oem or Unicode password.
    /// </summary>
    public enum PasswordType
    {
        /// <summary>
        /// An OEM code page character set (as opposed to UTF-16)
        /// </summary>
        Oem,
        /// <summary>
        /// Unicode character set.
        /// </summary>
        Unicode,
    }

    /// <summary>
    /// SamrCryptography class is used to encrypt parameters of SAMR RPC methods
    /// </summary>
    public class SamrCryptography
    {
        #region Member variables
        //Store existing password
        private string existingPwd;
        //Store new password.
        private string newPwd;

        // Underived key size
        private const int underivedKeySize = 7;
        // Encrypted password buffer
        private const int encryptedPwdSize = 512;
        // Length of password
        private const int pwdLenSize = 4;
        // Size of password encryption key
        private const int pwdEncryptionKeySize = 16;

        #endregion Member variables


        #region Helper Methods
        /// <summary>
        /// Encrypts input with DES ECB mode
        /// </summary>
        /// <param name="input">The data to be encrypted</param>
        /// <param name="key">The key used to encrypt the data</param>
        /// <returns>
        /// The encrypted data, samrDesEncryptedDataSize bytes are returned
        /// </returns>
        private static byte[] DesEcbEncrypt(byte[] input, byte[] key)
        {
            // SAMR DES(ECB mode) encrypted data size
            const int samrDesEncryptedDataSize = 8;

            DESCryptoServiceProvider desCSP = DES.Create() as DESCryptoServiceProvider;
            desCSP.Mode = CipherMode.ECB;
            desCSP.Key = key;

            // Get crypto transform
            ICryptoTransform crpto = desCSP.CreateEncryptor();
            // Do encryption
            byte[] encryptedData = crpto.TransformFinalBlock(input, 0, input.Length);
            byte[] output = new byte[samrDesEncryptedDataSize];
            Array.Copy(encryptedData, 0, output, 0, output.Length);

            return output;
        }


        /// <summary>
        /// Splits a given block into two blocks with specified block size
        /// </summary>
        /// <param name="block">The array to be splitted</param>
        /// <param name="block1Size">Size of block1</param>
        /// <param name="block2Size">Size of block2</param>
        /// <param name="block1">The splitted first block, starts at index 0</param>
        /// <param name="block2">The splitted second block, starts right after block1</param>
        private static void SplitBlock(byte[] block, int block1Size, int block2Size,
            out byte[] block1, out byte[] block2)
        {
            block1 = new byte[block1Size];
            block2 = new byte[block2Size];
            // Copy block1Size bytes to block1
            Array.Copy(block, 0, block1, 0, block1.Length);
            // Copy the consecutive block2Size bytes to block2
            Array.Copy(block, block1.Length, block2, 0, block2.Length);
        }


        /// <summary>
        /// Merges two given blocks to one block. The data of blocks are concatenated
        /// </summary>
        /// <param name="block1">First block to be merged</param>
        /// <param name="block2">Second block to be merged</param>
        /// <returns>The merged block</returns>
        private static byte[] MergeBlocks(byte[] block1, byte[] block2)
        {
            byte[] outBlock = new byte[block1.Length + block2.Length];
            Array.Copy(block1, outBlock, block1.Length);
            Array.Copy(block2, 0, outBlock, block1.Length, block2.Length);

            return outBlock;
        }


        /// <summary>
        /// Transforms a underivedKeySize-byte key to a 8-byte key
        /// </summary>
        /// <param name="inputKey">The input underivedKeySize-byte key</param>
        /// <returns>The derived 8-byte key</returns>
        private static byte[] TransformKey(byte[] inputKey)
        {
            // The bit mask whose least-significant bit is 0
            const byte bitMask = 0xfe;

            const int bitsInOneByte = 8;

            // The following transform algorithm is defined in MS-SAMR TD Section 2.2.11.1.2
            byte[] outputKey = new byte[8];
            outputKey[0] = (byte)(inputKey[0] >> 0x01);
            outputKey[1] = (byte)(((inputKey[0] & 0x01) << 6) | (inputKey[1] >> 2));
            outputKey[2] = (byte)(((inputKey[1] & 0x03) << 5) | (inputKey[2] >> 3));
            outputKey[3] = (byte)(((inputKey[2] & 0x07) << 4) | (inputKey[3] >> 4));
            outputKey[4] = (byte)(((inputKey[3] & 0x0F) << 3) | (inputKey[4] >> 5));
            outputKey[5] = (byte)(((inputKey[4] & 0x1F) << 2) | (inputKey[5] >> 6));
            outputKey[6] = (byte)(((inputKey[5] & 0x3F) << 1) | (inputKey[6] >> 7));
            outputKey[7] = (byte)(inputKey[6] & 0x7F);

            // The inputKey is expanded to 8 bytes by inserting a 0-bit after every 
            // seventh bit
            for (int i = 0; i < outputKey.Length; i++)
            {
                outputKey[i] = (byte)((outputKey[i] << 1) & bitMask);
            }

            //Let the least-significant bit of each byte of OutputKey be a parity bit. 
            //That is, if the sum of the preceding seven bits is odd, the eighth bit is 0; 
            //otherwise, the eighth bit is 1. The processing starts at the leftmost bit of OutputKey.
            bool odd = false;
            byte[] bitMaskArray = new byte[bitsInOneByte] { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };
            for (int j = 0; j < outputKey.Length; ++j)
            {
                for (int t = 0; t < bitsInOneByte - 1; ++t)
                {
                    if ((outputKey[j] & bitMaskArray[t]) == bitMaskArray[t])
                    {
                        odd = !odd;
                    }
                }
                if (odd == false)
                {
                    outputKey[j] = (byte)(outputKey[j] | bitMaskArray[bitsInOneByte - 1]);
                }
                else
                {
                    //clear the most right bit
                    outputKey[j] = (byte)(outputKey[j] & bitMask);
                }
                odd = false;
            }
            return outputKey;
        }


        /// <summary>
        /// Splits both the block and the key to 2 halves, then transforms the keys, and encrypts the blocks 
        /// with the transformed keys
        /// </summary>
        /// <param name="block">The input block to be encrypted</param>
        /// <param name="key">The encryption key</param>
        /// <returns>The encrypted data</returns>
        public static _ENCRYPTED_LM_OWF_PASSWORD EncryptBlockWithKey(byte[] block, byte[] key)
        {
            // The block size for DES ECB encryption
            const int samrEncryptionBlockSize = 8;

            _ENCRYPTED_LM_OWF_PASSWORD encryptedPwd = new _ENCRYPTED_LM_OWF_PASSWORD();
            byte[] block1 = null;
            byte[] block2 = null;
            byte[] key1 = null;
            byte[] key2 = null;

            // Split blocks into 2 blocks with each has the length samrEncryptionBlockSize
            SplitBlock(block, samrEncryptionBlockSize, samrEncryptionBlockSize, out block1, out block2);
            // Split keys into 2 underivedKeySize size blocks
            SplitBlock(key, underivedKeySize, underivedKeySize, out key1, out key2);

            // Derive keys
            byte[] transformedKey1 = TransformKey(key1);
            byte[] transformedKey2 = TransformKey(key2);

            // Do encryption
            byte[] encryptedBlock1 = DesEcbEncrypt(block1, transformedKey1);
            byte[] encryptedBlock2 = DesEcbEncrypt(block2, transformedKey2);
            // Concatenate the encrypted blocks
            encryptedPwd.data = MergeBlocks(encryptedBlock1, encryptedBlock2);

            return encryptedPwd;
        }


        /// <summary>
        /// Encrypts input data with RC4
        /// </summary>
        /// <param name="input">The input data to be encrypted</param>
        /// <param name="inputOffset">Offset of input from which the data will be encrypted </param>
        /// <param name="inputLength">Length of data to be encrypted</param>
        /// <param name="key">The encryption key</param>
        private static byte[] RC4Encrypt(byte[] input, int inputOffset, int inputLength, byte[] key)
        {
            RC4CryptoServiceProvider rc4 = new RC4CryptoServiceProvider();
            ICryptoTransform crypto = rc4.CreateEncryptor(key, null);
            byte[] output = crypto.TransformFinalBlock(input, inputOffset, inputLength);

            return output;
        }


        /// <summary>
        /// Gets a password encrypted with session key.
        /// </summary>
        /// <param name="password">The password to be encrypted</param>
        /// <param name="sessionKey">The session key used for encryption.</param>
        /// <exception cref="ArgumentNullException">Raised when session key is null</exception>
        /// <returns>The encrypted password.</returns>
        private _SAMPR_ENCRYPTED_USER_PASSWORD GetPasswordEncryptedWithSessionKey(
            string password,
            byte[] sessionKey)
        {
            if (sessionKey == null)
            {
                throw new ArgumentNullException("sessionKey");
            }

            _SAMPR_ENCRYPTED_USER_PASSWORD encryptedPwd = new _SAMPR_ENCRYPTED_USER_PASSWORD();
            encryptedPwd.Buffer = new byte[encryptedPwdSize + pwdLenSize];

            // Get new password bytes
            byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
            // Copy password bytes to the tail of the encryptedPwdSize bytes of buffer
            Array.Copy(passwordBytes, 0, encryptedPwd.Buffer,
                encryptedPwdSize - passwordBytes.Length, passwordBytes.Length);
            // Set password length for the last pwdLenSize bytes of the (encryptedPwdSize + pwdLenSize) bytes
            byte[] lengthBytes = BitConverter.GetBytes(passwordBytes.Length);
            Array.Copy(lengthBytes, 0, encryptedPwd.Buffer, encryptedPwdSize, pwdLenSize);

            // Do RC4 encryption
            encryptedPwd.Buffer = RC4Encrypt(encryptedPwd.Buffer, 0, encryptedPwd.Buffer.Length, sessionKey);

            return encryptedPwd;
        }
        #endregion Helper Methods


        #region Public Crypto Methods
        /// <summary>
        /// Constructor with existing password and new password.
        /// </summary>
        /// <param name="existingPassword">Existing password. Must have at least 8 characters.</param>
        /// <param name="newPassword">New password. Must have at least 8 characters.</param>
        /// <exception cref="ArgumentException">Raised if existingPassword or newPassword has less than 8 characters
        /// </exception>
        /// <exception cref="ArgumentNullException">Raised if existingPassword or newPassword is null</exception>
        public SamrCryptography(string existingPassword, string newPassword)
        {
            if (existingPassword != null)
            {
                this.existingPwd = existingPassword;
            }
            else
            {
                throw new ArgumentNullException("existingPassword");
            }

            if (newPassword != null)
            {
                this.newPwd = newPassword;
            }
            else
            {
                throw new ArgumentNullException("newPassword");
            }
        }


        /// <summary>
        /// Gets LM hash of the target user's existing password (as presented by the client) encrypted according 
        /// to the specification of ENCRYPTED_LM_OWF_PASSWORD (section 2.2.3.3),  where the key is the LM hash 
        /// of the new password for the target user.
        /// SamrChangePasswordUser/SamrOemChangePasswordUser2
        /// </summary>
        /// <returns>
        /// LM hash of existing password encrypted with LM hash of new password
        /// </returns>
        /// CA1024: Use properties where appropriate, suppressed as the returning structure is generated by PAC
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public _ENCRYPTED_LM_OWF_PASSWORD GetOldLmEncryptedWithNewLm()
        {
            // compute LM hash of existing password
            byte[] oldLmowf = GetHashWithLMOWFv1(existingPwd);
            // compute LM hash of new password
            byte[] newLmowf = GetHashWithLMOWFv1(newPwd);

            return EncryptBlockWithKey(oldLmowf, newLmowf);
        }


        /// <summary>
        /// Validate the hash of the target user's existing password (as presented by the client) encrypted according 
        /// to the specification of ENCRYPTED_LM_OWF_PASSWORD (section 2.2.3.3),  where the key is the LM hash 
        /// of the new password for the target user.
        /// </summary>
        /// <param name="target"> the target to be validate</param>
        /// <returns>validate result</returns>
        /// CA1024: Use properties where appropriate, suppressed as the returning structure is generated by PAC
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public bool ValidateOldLmEncryptedWithNewLm(_ENCRYPTED_LM_OWF_PASSWORD target)
        {
            _ENCRYPTED_LM_OWF_PASSWORD expected = GetOldLmEncryptedWithNewLm();
            bool isSame = ObjectUtility.DeepCompare(expected, target);
            return isSame;
        }


        /// <summary>
        /// Gets LM hash of the target user's new password (as presented by the client) encrypted according to 
        /// the specification of ENCRYPTED_LM_OWF_PASSWORD (section 2.2.3.3), where the key is the LM hash of 
        /// the existing password for the target user.
        /// SamrChangePasswordUser
        /// </summary>
        /// <returns>
        /// LM hash of new password encrypted with LM hash of existing password
        /// </returns>
        /// CA1024: Use properties where appropriate, suppressed as the returning structure is generated by PAC
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public _ENCRYPTED_LM_OWF_PASSWORD GetNewLmEncryptedWithOldLm()
        {
            // compute LM hash of existing password
            byte[] oldLmowf = GetHashWithLMOWFv1(existingPwd);
            // compute LM hash of new password
            byte[] newLmowf = GetHashWithLMOWFv1(newPwd);

            return EncryptBlockWithKey(newLmowf, oldLmowf);
        }


        /// <summary>
        /// Validate the LM hash of the target user's new password (as presented by the client) encrypted according to 
        /// the specification of ENCRYPTED_LM_OWF_PASSWORD (section 2.2.3.3), where the key is the LM hash of 
        /// the existing password for the target user.
        /// </summary>
        /// <param name="target"> the target to be validate</param>
        /// <returns>validate result</returns>
        ///  CA1024: Use properties where appropriate, suppressed as the returning structure is generated by PAC
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public bool ValidateNewLmEncryptedWithOldLm(_ENCRYPTED_LM_OWF_PASSWORD target)
        {
            _ENCRYPTED_LM_OWF_PASSWORD expected = GetNewLmEncryptedWithOldLm();
            bool isSame = ObjectUtility.DeepCompare(expected, target);
            return isSame;
        }


        /// <summary>
        /// Gets NT hash of the target user's existing password (as presented by the client) encrypted according 
        /// to the specification of ENCRYPTED_NT_OWF_PASSWORD (section 2.2.3.3), where the key is the NT hash 
        /// of the new password for the target user.
        /// SamrChangePasswordUser/SamrOemChangePasswordUser2
        /// </summary>
        /// <returns>
        /// NT hash of existing password encrypted with NT hash of new password
        /// </returns>
        /// CA1024: Use properties where appropriate, suppressed as the returning structure is generated by PAC
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public _ENCRYPTED_LM_OWF_PASSWORD GetOldNtEncryptedWithNewNt()
        {
            // compute NT hash of existing password
            byte[] oldNtowf = GetHashWithNTOWFv1(existingPwd);
            // compute NT hash of new password
            byte[] newNtowf = GetHashWithNTOWFv1(newPwd);

            return EncryptBlockWithKey(oldNtowf, newNtowf);
        }


        /// <summary>
        /// Validate the NT hash of the target user's existing password (as presented by the client) encrypted according 
        /// to the specification of ENCRYPTED_NT_OWF_PASSWORD (section 2.2.3.3), where the key is the NT hash 
        /// of the new password for the target user.
        /// </summary>
        /// <param name="target"> the target to be validate</param>
        /// <returns>validate result</returns>
        ///  CA1024: Use properties where appropriate, suppressed as the returning structure is generated by PAC
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public bool ValidateOldNtEncryptedWithNewNt(_ENCRYPTED_LM_OWF_PASSWORD target)
        {
            _ENCRYPTED_LM_OWF_PASSWORD expected = GetOldNtEncryptedWithNewNt();
            bool isSame = ObjectUtility.DeepCompare(expected, target);
            return isSame;
        }


        /// <summary>
        /// Gets NT hash of the target user's new password (as presented by the client) encrypted according to 
        /// the specification of ENCRYPTED_NT_OWF_PASSWORD, where the key is the NT hash of the existing 
        /// password for the target user.
        /// SamrChangePasswordUser
        /// </summary>
        /// <returns>NT hash of new password encrypted with NT hash of existing password.</returns>
        /// CA1024: Use properties where appropriate, suppressed as the returning structure is generated by PAC
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public _ENCRYPTED_LM_OWF_PASSWORD GetNewNtEncryptedWithOldNt()
        {
            // compute NT hash of existing password
            byte[] oldNtowf = GetHashWithNTOWFv1(existingPwd);
            // compute NT hash of new password
            byte[] newNtowf = GetHashWithNTOWFv1(newPwd);

            return EncryptBlockWithKey(newNtowf, oldNtowf);
        }


        /// <summary>
        /// Validate the NT hash of the target user's new password (as presented by the client) encrypted according to 
        /// the specification of ENCRYPTED_NT_OWF_PASSWORD, where the key is the NT hash of the existing 
        /// password for the target user.
        /// SamrChangePasswordUser
        /// </summary>
        /// <param name="target"> the target to be validate</param>
        /// <returns>validate result</returns>
        ///  CA1024: Use properties where appropriate, suppressed as the returning structure is generated by PAC
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public bool ValidateNewNtEncryptedWithOldNt(_ENCRYPTED_LM_OWF_PASSWORD target)
        {
            _ENCRYPTED_LM_OWF_PASSWORD expected = GetNewNtEncryptedWithOldNt();
            bool isSame = ObjectUtility.DeepCompare(expected, target);
            return isSame;
        }


        /// <summary>
        /// Gets NT hash of the target user's new password (as presented by the client) encrypted according to 
        /// the specification of ENCRYPTED_NT_OWF_PASSWORD, where the key is the LM hash of the new password 
        /// for the target user.
        /// SamrChangePasswordUser
        /// </summary>
        /// <returns>NT hash of new password encrypted with LM hash of new password</returns>
        /// CA1024: Use properties where appropriate, suppressed as the returning structure is generated by PAC
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public _ENCRYPTED_LM_OWF_PASSWORD GetNewNtEncryptedWithNewLm()
        {
            // compute NT hash of new password
            byte[] newNtowf = GetHashWithNTOWFv1(newPwd);
            // compute LM hash of new password
            byte[] newLmowf = GetHashWithLMOWFv1(newPwd);

            return EncryptBlockWithKey(newNtowf, newLmowf);
        }


        /// <summary>
        /// Validate the NT hash of the target user's new password (as presented by the client) encrypted according to 
        /// the specification of ENCRYPTED_NT_OWF_PASSWORD, where the key is the LM hash of the new password 
        /// for the target user.
        /// SamrChangePasswordUser
        /// </summary>
        /// <param name="target"> the target to be validate</param>
        /// <returns>validate result</returns>
        ///  CA1024: Use properties where appropriate, suppressed as the returning structure is generated by PAC
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public bool ValidateNewNtEncryptedWithNewLm(_ENCRYPTED_LM_OWF_PASSWORD target)
        {
            _ENCRYPTED_LM_OWF_PASSWORD expected = GetNewNtEncryptedWithNewLm();
            bool isSame = ObjectUtility.DeepCompare(expected, target);
            return isSame;
        }


        /// <summary>
        /// Gets LM hash of the target user's new password (as presented by the client) encrypted according to 
        /// the specification of ENCRYPTED_LM_OWF_PASSWORD, where the key is the NT hash of the new password 
        /// for the target user.
        /// SamrChangePasswordUser
        /// </summary>
        /// <returns>LM hash of new password encrypted with NT hash of new password</returns>
        /// CA1024: Use properties where appropriate, suppressed as the returning structure is generated by PAC
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public _ENCRYPTED_LM_OWF_PASSWORD GetNewLmEncryptedWithNewNt()
        {
            // compute LM hash of new password
            byte[] newLmowf = GetHashWithLMOWFv1(newPwd);
            // compute NT hash of new password
            byte[] newNtowf = GetHashWithNTOWFv1(newPwd);

            return EncryptBlockWithKey(newLmowf, newNtowf);
        }


        /// <summary>
        /// Validate the LM hash of the target user's new password (as presented by the client) encrypted according to 
        /// the specification of ENCRYPTED_LM_OWF_PASSWORD, where the key is the NT hash of the new password 
        /// for the target user.
        /// SamrChangePasswordUser
        /// </summary>
        /// <param name="target"> the target to be validate</param>
        /// <returns>validate result</returns>
        ///  CA1024: Use properties where appropriate, suppressed as the returning structure is generated by PAC
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public bool ValidateNewLmEncryptedWithNewNt(_ENCRYPTED_LM_OWF_PASSWORD target)
        {
            _ENCRYPTED_LM_OWF_PASSWORD expected = GetNewLmEncryptedWithNewNt();
            bool isSame = ObjectUtility.DeepCompare(expected, target);
            return isSame;
        }


        /// <summary>
        /// Gets LM hash the target user's existing password (as presented by the client) encrypted according to 
        /// the specification of ENCRYPTED_LM_OWF_PASSWORD, where the key is the NT hash of the clear text 
        /// password obtained from decrypting NewPasswordEncryptedWithOldNt.
        /// SamrUnicodeChangePasswordUser2
        /// </summary>
        /// <returns>
        /// LM hash of existing password encrypted with NT hash of new password</returns>
        /// CA1024: Use properties where appropriate, suppressed as the returning structure is generated by PAC
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public _ENCRYPTED_LM_OWF_PASSWORD GetOldLmOwfPasswordEncryptedWithNewNt()
        {
            // compute LM hash of existing password
            byte[] oldLmowf = GetHashWithLMOWFv1(existingPwd);
            // compute NT hash of new password
            byte[] newNtowf = GetHashWithNTOWFv1(newPwd);

            return EncryptBlockWithKey(oldLmowf, newNtowf);
        }


        /// <summary>
        /// Validate the LM hash the target user's existing password (as presented by the client) encrypted according to 
        /// the specification of ENCRYPTED_LM_OWF_PASSWORD, where the key is the NT hash of the clear text 
        /// password obtained from decrypting NewPasswordEncryptedWithOldNt.
        /// SamrUnicodeChangePasswordUser2
        /// </summary>
        /// <param name="target"> the target to be validate</param>
        /// <returns>validate result</returns>
        ///  CA1024: Use properties where appropriate, suppressed as the returning structure is generated by PAC
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public bool ValidateOldLmOwfPasswordEncryptedWithNewNt(_ENCRYPTED_LM_OWF_PASSWORD target)
        {
            _ENCRYPTED_LM_OWF_PASSWORD expected = GetOldLmOwfPasswordEncryptedWithNewNt();
            bool isSame = ObjectUtility.DeepCompare(expected, target);
            return isSame;
        }


        /// <summary>
        /// Computes an NT hash value with a password using NTOWFv1.
        /// </summary>
        /// <param name="password">A password to compute hash</param>
        /// <exception cref="ArgumentNullException">Raised if password is null</exception>
        /// <returns>An NT hash value</returns>
        public static byte[] GetHashWithNTOWFv1(string password)
        {
            if (password == null)
            {
                // Will be changed to StackSdkException after StackSdkException class is updated.
                throw new ArgumentNullException("password");
            }

            // Get MD4 CSP
            MD4 md4 = MD4CryptoServiceProvider.Create();
            md4.Initialize();

            // Get password byte array
            byte[] passwordBuffer = Encoding.Unicode.GetBytes(password);

            return md4.ComputeHash(passwordBuffer);
        }


        /// <summary>
        /// Computes an NT hash value with a password LMOWFv1
        /// </summary>
        /// <param name="password">A password to compute hash</param>
        /// <exception cref="ArgumentNullException">Raised if password is null</exception>
        /// <returns>An LM hash value.</returns>
        public static byte[] GetHashWithLMOWFv1(string password)
        {
            if (password == null)
            {
                // Will be changed to StackSdkException after StackSdkException class is updated.
                throw new ArgumentNullException("password");
            }

            LMHash lmHashAlg = LMHashManaged.Create();
            return lmHashAlg.ComputeHash(Encoding.ASCII.GetBytes(password));
        }


        /// <summary>
        /// Gets NT hash of the new password (as presented by the client) encrypted according to the
        /// specification of ENCRYPTED_NT_OWF_PASSWORD where the key is the User ID.
        /// SamrSetDSRMPassword
        /// </summary>
        /// <param name="userId">the target user's id</param>
        /// <returns>Encrypted password with userid.</returns>
        /// CA1024: Use properties where appropriate, suppressed as the returning structure is generated by PAC
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public _ENCRYPTED_LM_OWF_PASSWORD GetEncryptedNtOwfPasswordWithUserId(uint userId)
        {
            byte[] userIdBytes = BitConverter.GetBytes(userId);

            // Compute NT hash of new password
            byte[] newNtowf = GetHashWithNTOWFv1(newPwd);

            // The byte indices of key derived from userIdBytes, see TD Section 2.2.11.1.3
            int[] keyIndices = { 0, 1, 2, 3, 0, 1, 2, 3, 0, 1, 2, 3, 0, 1 };
            // Get key
            byte[] key = new byte[keyIndices.Length];
            for (int i = 0; i < key.Length; i++)
            {
                key[i] = userIdBytes[keyIndices[i]];
            }

            return EncryptBlockWithKey(newNtowf, key);
        }


        /// <summary>
        /// Validate the NT hash of the new password (as presented by the client) encrypted according to the
        /// specification of ENCRYPTED_NT_OWF_PASSWORD where the key is the User ID.
        /// SamrSetDSRMPassword
        /// </summary>
        /// <param name="userId">the target user's id</param>
        /// <param name="target"> the target to be validate</param>
        /// <returns>validate result</returns>
        ///  CA1024: Use properties where appropriate, suppressed as the returning structure is generated by PAC
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public bool ValidateEncryptedNtOwfPasswordWithUserId(uint userId, _ENCRYPTED_LM_OWF_PASSWORD target)
        {
            _ENCRYPTED_LM_OWF_PASSWORD expected = GetEncryptedNtOwfPasswordWithUserId(userId);
            bool isSame = ObjectUtility.DeepCompare(expected, target);
            return isSame;
        }


        /// <summary>
        /// Gets clear text password encrypted according to the specification of SAMPR_ENCRYPTED_USER_PASSWORD 
        /// (section 2.2.7.21), where the key is the LM hash of the existing password for the target user (as 
        /// presented by the client). The clear text password MUST be encoded in an OEM code page character set.
        /// SamrOemChangePasswordUser2/SamrUnicodeChangePasswordUser2
        /// </summary>
        /// <returns>Encrypted new password with LM hash of existing password.</returns>
        /// CA1024: Use properties where appropriate, suppressed as the returning structure is generated by PAC
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public _SAMPR_ENCRYPTED_USER_PASSWORD GetNewPasswordEncryptedWithOldLm(PasswordType passwordType)
        {
            _SAMPR_ENCRYPTED_USER_PASSWORD encryptedPwd = new _SAMPR_ENCRYPTED_USER_PASSWORD();
            encryptedPwd.Buffer = new byte[encryptedPwdSize + pwdLenSize];

            Encoding targetEncoding;
            if (passwordType == PasswordType.Oem)
            {
                // Windows OEM encoding is ASCII
                targetEncoding = Encoding.ASCII;
            }
            else if (passwordType == PasswordType.Unicode)
            {
                targetEncoding = Encoding.Unicode;
            }
            else
            {
                throw new InvalidOperationException("Invalid password type");
            }

            // Get new password bytes
            byte[] newPwdBytes = targetEncoding.GetBytes(newPwd);
            // Copy password bytes to the tail of the encryptedPwdSize bytes of buffer
            Array.Copy(newPwdBytes, 0, encryptedPwd.Buffer, encryptedPwdSize - newPwdBytes.Length,
                newPwdBytes.Length);
            // Set password length for the last pwdLenSize bytes of the (encryptedPwdSize + pwdLenSize) bytes
            byte[] lengthBytes = BitConverter.GetBytes(newPwdBytes.Length);
            Array.Copy(lengthBytes, 0, encryptedPwd.Buffer, encryptedPwdSize, lengthBytes.Length);

            // Get LM hash of existing password
            byte[] oldLmowf = GetHashWithLMOWFv1(existingPwd);
            // Do RC4 encryption
            encryptedPwd.Buffer = RC4Encrypt(encryptedPwd.Buffer, 0, encryptedPwd.Buffer.Length, oldLmowf);

            return encryptedPwd;
        }


        /// <summary>
        /// Validate clear text password encrypted according to the specification of SAMPR_ENCRYPTED_USER_PASSWORD 
        /// (section 2.2.7.21), where the key is the LM hash of the existing password for the target user (as 
        /// presented by the client). The clear text password MUST be encoded in an OEM code page character set.
        /// SamrOemChangePasswordUser2/SamrUnicodeChangePasswordUser2
        /// </summary>
        /// <param name="passwordType">the type of the password</param>
        /// <param name="target"> the target to be validate</param>
        /// <returns>validate result</returns>
        ///  CA1024: Use properties where appropriate, suppressed as the returning structure is generated by PAC
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public bool ValidateNewPasswordEncryptedWithOldLm(PasswordType passwordType, _SAMPR_ENCRYPTED_USER_PASSWORD target)
        {
            _SAMPR_ENCRYPTED_USER_PASSWORD expected = GetNewPasswordEncryptedWithOldLm(passwordType);
            bool isSame = ObjectUtility.DeepCompare(expected, target);
            return isSame;
        }


        /// <summary>
        /// Gets clear text password encrypted according to the specification of SAMPR_ENCRYPTED_USER_PASSWORD 
        /// (section 2.2.7.21), where the key is the NT hash of the existing password for the target user.
        /// SamrUnicodeChangePasswordUser2
        /// </summary>
        /// <returns>Encrypted new password with NT hash of old password.</returns>
        /// CA1024: Use properties where appropriate, suppressed as the returning structure is generated by PAC
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public _SAMPR_ENCRYPTED_USER_PASSWORD GetNewPasswordEncryptedWithOldNt()
        {
            _SAMPR_ENCRYPTED_USER_PASSWORD encryptedPwd = new _SAMPR_ENCRYPTED_USER_PASSWORD();
            encryptedPwd.Buffer = new byte[encryptedPwdSize + pwdLenSize];

            // Get new password bytes
            byte[] newPwdBytes = Encoding.Unicode.GetBytes(newPwd);
            // Copy password bytes to the tail of the encryptedPwdSize bytes of buffer
            Array.Copy(newPwdBytes, 0, encryptedPwd.Buffer,
                encryptedPwdSize - newPwdBytes.Length, newPwdBytes.Length);
            // Set password length for the last pwdLenSize bytes of the (encryptedPwdSize + pwdLenSize) bytes
            byte[] lengthBytes = BitConverter.GetBytes(newPwdBytes.Length);
            Array.Copy(lengthBytes, 0, encryptedPwd.Buffer, encryptedPwdSize, pwdLenSize);

            // Get NT hash of existing password
            byte[] oldNtowf = GetHashWithNTOWFv1(existingPwd);
            // Do RC4 encryption
            encryptedPwd.Buffer = RC4Encrypt(encryptedPwd.Buffer, 0, encryptedPwd.Buffer.Length, oldNtowf);

            return encryptedPwd;
        }


        /// <summary>
        /// Validate the clear text password encrypted according to the specification of SAMPR_ENCRYPTED_USER_PASSWORD 
        /// (section 2.2.7.21), where the key is the NT hash of the existing password for the target user.
        /// SamrUnicodeChangePasswordUser2
        /// </summary>
        /// <param name="target"> the target to be validate</param>
        /// <returns>validate result</returns>
        ///  CA1024: Use properties where appropriate, suppressed as the returning structure is generated by PAC
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public bool ValidateNewPasswordEncryptedWithOldNt(_SAMPR_ENCRYPTED_USER_PASSWORD target)
        {
            _SAMPR_ENCRYPTED_USER_PASSWORD expected = GetNewPasswordEncryptedWithOldNt();
            bool isSame = ObjectUtility.DeepCompare(expected, target);
            return isSame;
        }


        /// <summary>
        /// Gets a SAMPR_ENCRYPTED_USER_PASSWORD_NEW structure carries an encrypted string.
        /// SamrSetInformationUser2
        /// </summary>
        /// <param name="clearSalt">A byte array of random value</param>
        /// <param name="sessionKey">The session key used for encryption.</param>
        /// <exception cref="ArgumentNullException">Raised when clearSalt or sessionKey is null</exception>
        /// <returns>Encrypted existing password with clearsalt.</returns>
        /// CA1024: Use properties where appropriate, suppressed as the returning structure is generated by PAC
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public _SAMPR_ENCRYPTED_USER_PASSWORD_NEW GetPasswordEncryptedWithSalt(byte[] clearSalt, byte[] sessionKey)
        {
            if (clearSalt == null)
            {
                // Will be changed to StackSdkException after StackSdkxception class is updated
                throw new ArgumentNullException("clearSalt");
            }

            if (sessionKey == null)
            {
                throw new ArgumentNullException("sessionKey");
            }

            byte[] digestData = MergeBlocks(clearSalt, sessionKey);

            // Compute digest for clearSalt and sessionKey, this digest is the key for RC4 encryption
            MD5 md5 = MD5.Create();
            byte[] digest = md5.ComputeHash(digestData);

            _SAMPR_ENCRYPTED_USER_PASSWORD_NEW encryptedPwd = new _SAMPR_ENCRYPTED_USER_PASSWORD_NEW();
            encryptedPwd.Buffer = new byte[encryptedPwdSize + pwdLenSize + pwdEncryptionKeySize];

            // Copy password bytes to the tail of the encryptedPwdSize bytes of buffer
            byte[] existingPwdBuffer = Encoding.Unicode.GetBytes(existingPwd);
            Array.Copy(existingPwdBuffer, 0, encryptedPwd.Buffer,
                encryptedPwdSize - existingPwdBuffer.Length, existingPwdBuffer.Length);

            // Set password length for the last pwdLenSize bytes of (the encryptedPwdSize + pwdLenSize) bytes
            byte[] lengthBuffer = BitConverter.GetBytes(existingPwdBuffer.Length);
            Array.Copy(lengthBuffer, 0, encryptedPwd.Buffer, encryptedPwdSize, lengthBuffer.Length);
            // Copy clear salt to the last pwdEncryptionKeySize bytes of the buffer
            Array.Copy(clearSalt, 0, encryptedPwd.Buffer, encryptedPwdSize + pwdLenSize, clearSalt.Length);
            // Do RC4 encryption, the last pwdEncryptionKeySize bytes are intact
            byte[] rc4Result = RC4Encrypt(encryptedPwd.Buffer, 0,
                encryptedPwd.Buffer.Length - pwdEncryptionKeySize, digest);
            Array.Copy(rc4Result, encryptedPwd.Buffer, rc4Result.Length);

            return encryptedPwd;
        }


        /// <summary>
        /// Validate the  SAMPR_ENCRYPTED_USER_PASSWORD_NEW structure carries an encrypted string.
        /// SamrSetInformationUser2
        /// </summary>
        /// <param name="clearSalt">A byte array of random value</param>
        /// <param name="sessionKey">The session key used for encryption.</param>
        /// <param name="target"> the target to be validate</param>
        /// <returns>validate result</returns>
        ///  CA1024: Use properties where appropriate, suppressed as the returning structure is generated by PAC
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public bool ValidatePasswordEncryptedWithSalt(byte[] clearSalt, byte[] sessionKey, _SAMPR_ENCRYPTED_USER_PASSWORD_NEW target)
        {
            _SAMPR_ENCRYPTED_USER_PASSWORD_NEW expected = GetPasswordEncryptedWithSalt(clearSalt, sessionKey);
            bool isSame = ObjectUtility.DeepCompare(expected, target);
            return isSame;
        }


        /// <summary>
        /// Gets a random byte array for clear salt
        /// </summary>
        /// <param name="size">Size of clear salt to be generated</param>
        /// <returns>A clear salt.</returns>
        public byte[] GetClearSalt(uint size)
        {
            byte[] salt = new byte[size];
            Random rand = new Random();
            rand.NextBytes(salt);

            return salt;
        }


        /// <summary>
        /// Gets LM hash of the existing password (as presented by the client) encrypted according to the
        /// specification of ENCRYPTED_NT_OWF_PASSWORD where the key is the 16-byte SMB session key established by
        /// the underlying authentication protocol(either Kerberos or NTLM).
        /// </summary>
        /// <param name="sessionKey">The session key used for encryption.</param>
        /// <exception cref="ArgumentNullException">Raised when session key is null</exception>
        /// <returns>The encrypted password.</returns>
        public _ENCRYPTED_LM_OWF_PASSWORD GetOldLmOwfPasswordEncryptedWithSessionKey(byte[] sessionKey)
        {
            if (sessionKey == null)
            {
                throw new ArgumentNullException("sessionKey");
            }
            byte[] oldLmOwf = GetHashWithLMOWFv1(this.existingPwd);
            return EncryptBlockWithKey(oldLmOwf, sessionKey);
        }


        /// <summary>
        /// Validate the LM hash of the existing password (as presented by the client) encrypted according to the
        /// specification of ENCRYPTED_NT_OWF_PASSWORD where the key is the 16-byte SMB session key established by
        /// the underlying authentication protocol(either Kerberos or NTLM).
        /// </summary>
        /// <param name="sessionKey">The session key used for encryption.</param>
        /// <param name="target"> the target to be validate</param>
        /// <returns>validate result</returns>
        public bool ValidateOldLmOwfPasswordEncryptedWithSessionKey(byte[] sessionKey, _ENCRYPTED_LM_OWF_PASSWORD target)
        {
            _ENCRYPTED_LM_OWF_PASSWORD expected = GetOldLmOwfPasswordEncryptedWithSessionKey(sessionKey);
            bool isSame = ObjectUtility.DeepCompare(expected, target);
            return isSame;
        }


        /// <summary>
        /// Gets LM hash of the new password (as presented by the client) encrypted according to the
        /// specification of ENCRYPTED_NT_OWF_PASSWORD where the key is the 16-byte SMB session key established by
        /// the underlying authentication protocol(either Kerberos or NTLM).
        /// </summary>
        /// <param name="sessionKey">The session key used for encryption.</param>
        /// <exception cref="ArgumentNullException">Raised when session key is null</exception>
        /// <returns>The encrypted password.</returns>
        public _ENCRYPTED_LM_OWF_PASSWORD GetNewLmOwfPasswordEncryptedWithSessionKey(byte[] sessionKey)
        {
            if (sessionKey == null)
            {
                throw new ArgumentNullException("sessionKey");
            }
            byte[] newLmOwf = GetHashWithLMOWFv1(this.newPwd);
            return EncryptBlockWithKey(newLmOwf, sessionKey);
        }


        /// <summary>
        /// Validate the LM hash of the new password (as presented by the client) encrypted according to the
        /// specification of ENCRYPTED_NT_OWF_PASSWORD where the key is the 16-byte SMB session key established by
        /// the underlying authentication protocol(either Kerberos or NTLM).
        /// </summary>
        /// <param name="sessionKey">The session key used for encryption.</param>
        /// <param name="target"> the target to be validate</param>
        /// <returns>validate result</returns>
        public bool ValidateNewLmOwfPasswordEncryptedWithSessionKey(byte[] sessionKey, _ENCRYPTED_LM_OWF_PASSWORD target)
        {
            _ENCRYPTED_LM_OWF_PASSWORD expected = GetNewLmOwfPasswordEncryptedWithSessionKey(sessionKey);
            bool isSame = ObjectUtility.DeepCompare(expected, target);
            return isSame;
        }


        /// <summary>
        /// Gets NT hash of the existing password (as presented by the client) encrypted according to the
        /// specification of ENCRYPTED_NT_OWF_PASSWORD where the key is the 16-byte SMB session key established by
        /// the underlying authentication protocol(either Kerberos or NTLM).
        /// </summary>
        /// <param name="sessionKey">The session key used for encryption.</param>
        /// <exception cref="ArgumentNullException">Raised when session key is null</exception>
        /// <returns>The encrypted password.</returns>
        public _ENCRYPTED_LM_OWF_PASSWORD GetOldNtOwfPasswordEncryptedWithSessionKey(byte[] sessionKey)
        {
            if (sessionKey == null)
            {
                throw new ArgumentNullException("sessionKey");
            }
            byte[] oldNtOwf = GetHashWithNTOWFv1(this.existingPwd);
            return EncryptBlockWithKey(oldNtOwf, sessionKey);
        }


        /// <summary>
        /// Validate the NT hash of the existing password (as presented by the client) encrypted according to the
        /// specification of ENCRYPTED_NT_OWF_PASSWORD where the key is the 16-byte SMB session key established by
        /// the underlying authentication protocol(either Kerberos or NTLM).
        /// </summary>
        /// <param name="sessionKey">The session key used for encryption.</param>
        /// <param name="target"> the target to be validate</param>
        /// <returns>validate result</returns>
        public bool ValidateOldNtOwfPasswordEncryptedWithSessionKey(byte[] sessionKey, _ENCRYPTED_LM_OWF_PASSWORD target)
        {
            _ENCRYPTED_LM_OWF_PASSWORD expected = GetOldNtOwfPasswordEncryptedWithSessionKey(sessionKey);
            bool isSame = ObjectUtility.DeepCompare(expected, target);
            return isSame;
        }


        /// <summary>
        /// Gets NT hash of the new password (as presented by the client) encrypted according to the
        /// specification of ENCRYPTED_NT_OWF_PASSWORD where the key is the 16-byte SMB session key established by
        /// the underlying authentication protocol(either Kerberos or NTLM).
        /// </summary>
        /// <param name="sessionKey">The session key used for encryption.</param>
        /// <exception cref="ArgumentNullException">Raised when session key is null</exception>
        /// <returns>The encrypted password.</returns>
        public _ENCRYPTED_LM_OWF_PASSWORD GetNewNtOwfPasswordEncryptedWithSessionKey(byte[] sessionKey)
        {
            if (sessionKey == null)
            {
                throw new ArgumentNullException("sessionKey");
            }
            byte[] newNtOwf = GetHashWithNTOWFv1(this.newPwd);
            return EncryptBlockWithKey(newNtOwf, sessionKey);
        }


        /// <summary>
        /// Validate the NT hash of the new password (as presented by the client) encrypted according to the
        /// specification of ENCRYPTED_NT_OWF_PASSWORD where the key is the 16-byte SMB session key established by
        /// the underlying authentication protocol(either Kerberos or NTLM).
        /// </summary>
        /// <param name="sessionKey">The session key used for encryption.</param>
        /// <param name="target"> the target to be validate</param>
        /// <returns>validate result</returns>
        public bool ValidateNewNtOwfPasswordEncryptedWithSessionKey(byte[] sessionKey, _ENCRYPTED_LM_OWF_PASSWORD target)
        {
            _ENCRYPTED_LM_OWF_PASSWORD expected = GetNewNtOwfPasswordEncryptedWithSessionKey(sessionKey);
            bool isSame = ObjectUtility.DeepCompare(expected, target);
            return isSame;
        }


        /// <summary>
        /// Gets the new password encrypted with session key.
        /// </summary>
        /// <param name="sessionKey">The session key used for encryption.</param>
        /// <exception cref="ArgumentNullException">Raised when session key is null</exception>
        /// <returns>The encrypted new password.</returns>
        public _SAMPR_ENCRYPTED_USER_PASSWORD GetNewPasswordEncryptedWithSessionKey(byte[] sessionKey)
        {
            return GetPasswordEncryptedWithSessionKey(newPwd, sessionKey);
        }


        /// <summary>
        /// Validate the new password encrypted with session key.
        /// </summary>
        /// <param name="sessionKey">The session key used for encryption.</param>
        /// <param name="target"> the target to be validate</param>
        /// <returns>validate result</returns>
        public bool ValidateNewPasswordEncryptedWithSessionKey(byte[] sessionKey, _SAMPR_ENCRYPTED_USER_PASSWORD target)
        {
            _SAMPR_ENCRYPTED_USER_PASSWORD expected = GetNewPasswordEncryptedWithSessionKey(sessionKey);
            bool isSame = ObjectUtility.DeepCompare(expected, target);
            return isSame;
        }


        /// <summary>
        /// Gets the existing password encrypted with session key.
        /// </summary>
        /// <param name="sessionKey">The session key used for encryption.</param>
        /// <exception cref="ArgumentNullException">Raised when session key is null</exception>
        /// <returns>The encrypted existing password.</returns>
        public _SAMPR_ENCRYPTED_USER_PASSWORD GetOldPasswordEncryptedWithSessionKey(byte[] sessionKey)
        {
            return GetPasswordEncryptedWithSessionKey(existingPwd, sessionKey);
        }


        /// <summary>
        /// Validate the existing password encrypted with session key.
        /// </summary>
        /// <param name="sessionKey">The session key used for encryption.</param>
        /// <param name="target"> the target to be validate</param>
        /// <returns>validate result</returns>
        public bool ValidateOldPasswordEncryptedWithSessionKey(byte[] sessionKey, _SAMPR_ENCRYPTED_USER_PASSWORD target)
        {
            _SAMPR_ENCRYPTED_USER_PASSWORD expected = GetOldPasswordEncryptedWithSessionKey(sessionKey);
            bool isSame = ObjectUtility.DeepCompare(expected, target);
            return isSame;
        }
        #endregion Public Crypto Methods
    }
}
