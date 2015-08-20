// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// BCryptAlgorithm provides encrypting and decrypting support
    /// based on Cryptography API Next Generation (CNG).<para/>
    /// The CNG API provides a set of functions that perform basic 
    /// cryptographic operations such as creating hashes or 
    /// encrypting and decrypting data.<para/>
    /// Symmetric cipher algorithms are supported, such as: 
    /// RC2, RC4, DES, AES, etc. 
    /// Please call BCryptAlgorithm.Algorithms to get the full list.
    /// </summary>
    public class BCryptAlgorithm : IDisposable
    {
        /// <summary>
        /// Get a full list of symmetric cipher algorithms.
        /// </summary>
        /// <exception cref="CryptographicException">
        /// Throw when error occurred in underlayer operation.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static string[] Algorithms
        {
            get
            {
                string[] algorithms;
                uint algCount;
                IntPtr algList;
                IntPtr algListFirst;

                uint status = NativeMethods.BCryptEnumAlgorithms(
                    NativeMethods.BCRYPT_CIPHER_OPERATION,
                    out algCount,
                    out algList,
                    0);
                if (status != (uint)NtStatus.STATUS_SUCCESS)
                {
                    throw new CryptographicException(GetErrorNameFromCode(status));
                }

                algListFirst = algList;
                algorithms = new string[algCount];

                for (int i = 0; i < algCount; i++)
                {
                    NativeMethods.BCRYPT_ALGORITHM_IDENTIFIER algIdentifier =
                        (NativeMethods.BCRYPT_ALGORITHM_IDENTIFIER)Marshal.PtrToStructure(
                            algList,
                            typeof(NativeMethods.BCRYPT_ALGORITHM_IDENTIFIER));

                    algorithms[i] = algIdentifier.pszName;

                    if (IntPtr.Size == sizeof(Int32))
                    {
                        algList = new IntPtr(
                            algList.ToInt32() +
                            Marshal.SizeOf(typeof(NativeMethods.BCRYPT_ALGORITHM_IDENTIFIER)));
                    }
                    else if (IntPtr.Size == sizeof(Int64))
                    {
                        algList = new IntPtr(
                            algList.ToInt64() +
                            Marshal.SizeOf(typeof(NativeMethods.BCRYPT_ALGORITHM_IDENTIFIER)));
                    }
                    else
                    {
                        throw new NotSupportedException("Platform is neither 32 bits nor 64 bits.");
                    }
                }

                status = NativeMethods.BCryptFreeBuffer(algListFirst);
                //Ignore the status.

                return algorithms;
            }
        }


        /// <summary>
        /// Get error's name from code.
        /// </summary>
        /// <param name="status">error code.</param>
        /// <returns>error name</returns>
        private static string GetErrorNameFromCode(uint status)
        {
            string name = Enum.GetName(typeof(NtStatus), status);
            if (string.IsNullOrEmpty(name))
            {
                name = string.Format("0x{0:x}", status);
            }
            return name;
        }


        /// <summary>
        /// algorithm name
        /// </summary>
        private string name;

        /// <summary>
        /// algorithm handle
        /// </summary>
        private IntPtr hAlgorithm;

        /// <summary>
        /// key handle
        /// </summary>
        private IntPtr hKey;

        /// <summary>
        /// key
        /// </summary>
        private byte[] key;

        /// <summary>
        /// iv
        /// </summary>
        private byte[] iv;

        /// <summary>
        /// Indicate if the instance is disposed.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Initialize an instance of BCryptAlgorithm.
        /// </summary>
        /// <param name="algorithm">
        /// Algorithm name. Example: "AES".
        /// </param>
        /// <exception cref="CryptographicException">
        /// Throw when error occurred in underlayer operation.
        /// </exception>
        public BCryptAlgorithm(string algorithm)
        {
            uint status = NativeMethods.BCryptOpenAlgorithmProvider(
                out hAlgorithm,
                algorithm,
                null,
                0);
            if (status != (uint)NtStatus.STATUS_SUCCESS)
            {
                throw new CryptographicException(GetErrorNameFromCode(status));
            }

            name = algorithm;
        }


        /// <summary>
        /// Set key to algorithm
        /// </summary>
        /// <param name="value">key</param>
        private void BCryptSetKey(byte[] value)
        {
            uint status;

            if (hKey != IntPtr.Zero)
            {
                status = NativeMethods.BCryptDestroyKey(hKey);
                if (status != (uint)NtStatus.STATUS_SUCCESS)
                {
                    throw new CryptographicException(GetErrorNameFromCode(status));
                }
                hKey = IntPtr.Zero;
            }

            status = NativeMethods.BCryptGenerateSymmetricKey(
                hAlgorithm,
                out hKey,
                null,
                0,
                value,
                (uint)value.Length,
                0);
            if (status != (uint)NtStatus.STATUS_SUCCESS)
            {
                throw new CryptographicException(GetErrorNameFromCode(status));
            }
        }


        /// <summary>
        /// Retrieve int32 property value from algorithm or key
        /// </summary>
        /// <param name="hObject">algorithm or key handle</param>
        /// <param name="property">property name</param>
        /// <returns>property value</returns>
        private static int BCryptGetPropertyInt32(
            IntPtr hObject, 
            string property)
        {
            uint cbInt32 = 4;
            byte[] pbInt32Buf = new byte[cbInt32];

            uint status = NativeMethods.BCryptGetProperty(
                hObject,
                property,
                pbInt32Buf,
                cbInt32,
                out cbInt32,
                0);
            if (status != (uint)NtStatus.STATUS_SUCCESS)
            {
                throw new CryptographicException(GetErrorNameFromCode(status));
            }

            return BitConverter.ToInt32(pbInt32Buf, 0);
        }


        /// <summary>
        /// Retrieve int32 array property value from algorithm or key
        /// </summary>
        /// <param name="hObject">algorithm or key handle</param>
        /// <param name="property">property name</param>
        /// <returns>property value</returns>
        private static int[] BCryptGetPropertyInt32Array(
            IntPtr hObject, 
            string property)
        {
            uint cbData;
            byte[] pbDataBuf;

            uint status = NativeMethods.BCryptGetProperty(
                hObject,
                property,
                null,
                0,
                out cbData,
                0);
            if (status != (uint)NtStatus.STATUS_SUCCESS)
            {
                throw new CryptographicException(GetErrorNameFromCode(status));
            }

            pbDataBuf = new byte[cbData];

            status = NativeMethods.BCryptGetProperty(
                hObject,
                property,
                pbDataBuf,
                cbData,
                out cbData,
                0);
            if (status != (uint)NtStatus.STATUS_SUCCESS)
            {
                throw new CryptographicException(GetErrorNameFromCode(status));
            }

            int[] result = new int[cbData / sizeof(int)];
            for (int i = 0, offset = 0; offset < cbData; i++, offset += sizeof(int))
            {
                result[i] = BitConverter.ToInt32(pbDataBuf, offset);
            }

            return result;
        }


        /// <summary>
        /// Retrieve byte property value from algorithm or key
        /// </summary>
        /// <param name="hObject">algorithm or key handle</param>
        /// <param name="property">property name</param>
        /// <returns>property value</returns>
        private static byte[] BCryptGetPropertyByteArray(
            IntPtr hObject,
            string property)
        {
            uint cbData;
            byte[] pbData;

            uint status = NativeMethods.BCryptGetProperty(
                hObject,
                property,
                null,
                0,
                out cbData,
                0);
            if (status != (uint)NtStatus.STATUS_SUCCESS)
            {
                throw new CryptographicException(GetErrorNameFromCode(status));
            }

            pbData = new byte[cbData];

            status = NativeMethods.BCryptGetProperty(
                hObject,
                NativeMethods.BCRYPT_CHAINING_MODE,
                pbData,
                cbData,
                out cbData,
                0);
            if (status != (uint)NtStatus.STATUS_SUCCESS)
            {
                throw new CryptographicException(GetErrorNameFromCode(status));
            }

            return pbData;
        }


        /// <summary>
        /// Set int32 property value from algorithm or key
        /// </summary>
        /// <param name="hObject">algorithm or key handle</param>
        /// <param name="property">property name</param>
        /// <param name="value">property value</param>
        private static void BCryptSetPrpoertyInt32(
            IntPtr hObject, 
            string property, 
            int value)
        {
            byte[] pbInt32Buf = BitConverter.GetBytes(value);

            uint status = NativeMethods.BCryptSetProperty(
                hObject,
                property,
                pbInt32Buf,
                (uint)pbInt32Buf.Length,
                0);
            if (status != (uint)NtStatus.STATUS_SUCCESS)
            {
                throw new CryptographicException(GetErrorNameFromCode(status));
            }
        }


        /// <summary>
        /// Set byte array property value from algorithm or key
        /// </summary>
        /// <param name="hObject">algorithm or key handle</param>
        /// <param name="property">property name</param>
        /// <param name="value">property value</param>
        private static void BCryptSetPropertyByteArray(
            IntPtr hObject,
            string property,
            byte[] value)
        {
            uint status = NativeMethods.BCryptSetProperty(
                hObject,
                property,
                value,
                (uint)value.Length,
                0);
            if (status != (uint)NtStatus.STATUS_SUCCESS)
            {
                throw new CryptographicException(GetErrorNameFromCode(status));
            }
        }


        /// <summary>
        /// Check if key exists
        /// </summary>
        private void ValidateKeyExists()
        {
            if (hKey == IntPtr.Zero)
            {
                throw new InvalidOperationException("Please set Key first.");
            }
        }


        /// <summary>
        /// Gets the algorithm name.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }


        /// <summary>
        /// The key lengths that are supported by the algorithm.
        /// </summary>
        /// <exception cref="CryptographicException">
        /// Throw when error occurred in underlayer operation.
        /// </exception>
        public int[] KeyLengthList
        {
            get
            {
                return BCryptGetPropertyInt32Array(
                    hAlgorithm,
                    NativeMethods.BCRYPT_KEY_LENGTHS);
            }
        }


        /// <summary>
        /// The size, in bits, of the key value of a symmetric key provider.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Throw when Key is not set.
        /// </exception>
        /// <exception cref="CryptographicException">
        /// Throw when error occurred in underlayer operation.
        /// </exception>
        public int KeyLength
        {
            get
            {
                ValidateKeyExists();
                return BCryptGetPropertyInt32(
                    hKey,
                    NativeMethods.BCRYPT_KEY_LENGTH);
            }
        }


        /// <summary>
        /// A list of the block lengths supported by an encryption algorithm.
        /// </summary>
        /// <exception cref="CryptographicException">
        /// Throw when error occurred in underlayer operation.
        /// </exception>
        public int[] BlockSizeList
        {
            get
            {
                return BCryptGetPropertyInt32Array(
                    hAlgorithm,
                    NativeMethods.BCRYPT_BLOCK_SIZE_LIST);
            }
        }


        /// <summary>
        /// The size, in bytes, of a cipher block for the algorithm.
        /// </summary>
        /// <exception cref="CryptographicException">
        /// Throw when error occurred in underlayer operation.
        /// </exception>
        public int BlockSize
        {
            get
            {
                return BCryptGetPropertyInt32(
                    hAlgorithm,
                    NativeMethods.BCRYPT_BLOCK_LENGTH);
            }
            set
            {
                BCryptSetPrpoertyInt32(
                    hAlgorithm,
                    NativeMethods.BCRYPT_BLOCK_LENGTH,
                    value);
            }
        }


        /// <summary>
        /// The key of the algorithm.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Throw when Key is null.
        /// </exception>
        /// <exception cref="CryptographicException">
        /// Throw when error occurred in underlayer operation.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public byte[] Key
        {
            get
            {
                return key;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                BCryptSetKey(value);

                key = value;
            }
        }


        /// <summary>
        /// Contains the initialization vector (IV) for a key.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Throw when IV is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Throw when Key is not set.
        /// </exception>
        /// <exception cref="CryptographicException">
        /// Throw when error occurred in underlayer operation.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public byte[] IV
        {
            get
            {
                return iv;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                ValidateKeyExists();

                BCryptSetPropertyByteArray(
                    hKey,
                    NativeMethods.BCRYPT_INITIALIZATION_VECTOR,
                    value);

                iv = value;
            }
        }


        /// <summary>
        /// The chaining mode of the algorithm provider.
        /// </summary>
        /// <exception cref="CryptographicException">
        /// Throw when error occurred in underlayer operation.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public BCryptCipherMode Mode
        {
            get
            {
                BCryptCipherMode mode;

                byte[] chainModeBuf = BCryptGetPropertyByteArray(
                    hAlgorithm, 
                    NativeMethods.BCRYPT_CHAINING_MODE);

                string chainMode = Encoding.Unicode.GetString(chainModeBuf);
                chainMode = chainMode.Substring(0, chainMode.IndexOf('\0'));

                switch (chainMode)
                {
                    case NativeMethods.BCRYPT_CHAIN_MODE_NA:
                        mode = BCryptCipherMode.NotAvailable;
                        break;
                    case NativeMethods.BCRYPT_CHAIN_MODE_CBC:
                        mode = BCryptCipherMode.CBC;
                        break;
                    case NativeMethods.BCRYPT_CHAIN_MODE_ECB:
                        mode = BCryptCipherMode.ECB;
                        break;
                    case NativeMethods.BCRYPT_CHAIN_MODE_CFB:
                        mode = BCryptCipherMode.CFB;
                        break;
                    case NativeMethods.BCRYPT_CHAIN_MODE_CCM:
                        mode = BCryptCipherMode.CCM;
                        break;
                    case NativeMethods.BCRYPT_CHAIN_MODE_GCM:
                        mode = BCryptCipherMode.GCM;
                        break;
                    default:
                        throw new CryptographicException("Unknown chain mode - " + chainMode);
                }

                return mode;
            }
            set
            {
                byte[] chainModeBuf;
                string chainMode;

                switch (value)
                {
                    case BCryptCipherMode.NotAvailable:
                        chainMode = NativeMethods.BCRYPT_CHAIN_MODE_NA;
                        break;
                    case BCryptCipherMode.CBC:
                        chainMode = NativeMethods.BCRYPT_CHAIN_MODE_CBC;
                        break;
                    case BCryptCipherMode.ECB:
                        chainMode = NativeMethods.BCRYPT_CHAIN_MODE_ECB;
                        break;
                    case BCryptCipherMode.CFB:
                        chainMode = NativeMethods.BCRYPT_CHAIN_MODE_CFB;
                        break;
                    case BCryptCipherMode.CCM:
                        chainMode = NativeMethods.BCRYPT_CHAIN_MODE_CCM;
                        break;
                    case BCryptCipherMode.GCM:
                        chainMode = NativeMethods.BCRYPT_CHAIN_MODE_GCM;
                        break;
                    default:
                        throw new CryptographicException("Unknown chain mode - " + value);
                }

                chainModeBuf = Encoding.Unicode.GetBytes(chainMode);

                BCryptSetPropertyByteArray(
                    hAlgorithm,
                    NativeMethods.BCRYPT_CHAINING_MODE, 
                    chainModeBuf);
            }
        }


        /// <summary>
        /// Encrypts a block of data.
        /// </summary>
        /// <param name="input">
        /// The input to be encrypted. 
        /// </param>
        /// <returns>
        /// Encrypted output.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Throw when input is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Throw when Key is not set.
        /// </exception>
        /// <exception cref="CryptographicException">
        /// Throw when error occurred in underlayer operation.
        /// </exception>
        public byte[] Encrypt(byte[] input)
        {
            byte[] tag;
            return Encrypt(input, null, null, 0, out tag);
        }

        /// <summary>
        /// Encrypts a block of data with cipher mode info specified.
        /// </summary>
        /// <param name="input">
        /// The input to be encrypted. 
        /// </param>
        /// <param name="nonce">
        /// Nonce.
        /// </param>
        /// <param name="authData">
        /// Auth data.
        /// </param>
        /// <param name="tagLength">
        /// Length of tag.
        /// </param>
        /// <param name="tag">
        /// Tag.
        /// </param>
        /// <returns>
        /// Encrypted output.
        /// </returns>
        public byte[] Encrypt(byte[] input, byte[] nonce, byte[] authData, int tagLength, out byte[] tag)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            ValidateKeyExists();

            tag = new byte[tagLength];

            IntPtr pbInfo = IntPtr.Zero;

            BcryptAuthenticatedCipherModeInfo info = new BcryptAuthenticatedCipherModeInfo();

            try
            {
                if (nonce != null || authData != null || tagLength > 0)
                {
                    info = new BcryptAuthenticatedCipherModeInfo
                    {
                        cbSize = (uint)Marshal.SizeOf(typeof(BcryptAuthenticatedCipherModeInfo)),
                        dwInfoVersion = 1,
                    };

                    if (nonce != null)
                    {
                        info.cbNonce = (uint)nonce.Length;
                        info.pbNonce = Marshal.AllocHGlobal(nonce.Length);
                        Marshal.Copy(nonce, 0, info.pbNonce, nonce.Length);
                    }

                    if (authData != null)
                    {
                        info.cbAuthData = (uint)authData.Length;
                        info.pbAuthData = Marshal.AllocHGlobal(authData.Length);
                        Marshal.Copy(authData, 0, info.pbAuthData, authData.Length);
                    }

                    if (tagLength > 0)
                    {
                        info.cbTag = (uint)tagLength;
                        info.pbTag = Marshal.AllocHGlobal(tagLength);
                    }

                    info.pbMacContext = IntPtr.Zero;
                    info.cbMacContext = 0;
                    info.cbAAD = 0;
                    info.cbData = 0;
                    info.dwFlags = 0;

                    pbInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BcryptAuthenticatedCipherModeInfo)));
                    Marshal.StructureToPtr(info, pbInfo, false);
                }

                uint cbOutput;
                uint status = NativeMethods.BCryptEncrypt(
                    hKey,
                    input,
                    (uint)input.Length,
                    pbInfo,
                    null,
                    0,
                    null,
                    0,
                    out cbOutput,
                    0);
                if (status != (uint)NtStatus.STATUS_SUCCESS)
                {
                    throw new CryptographicException(GetErrorNameFromCode(status));
                }

                byte[] output = new byte[cbOutput];
                status = NativeMethods.BCryptEncrypt(
                    hKey,
                    input,
                    (uint)input.Length,
                    pbInfo,
                    null,
                    0,
                    output,
                    cbOutput,
                    out cbOutput,
                    0);
                if (status != (uint)NtStatus.STATUS_SUCCESS)
                {
                    throw new CryptographicException(GetErrorNameFromCode(status));
                }

                return output;
            }
            finally
            {
                if (pbInfo != IntPtr.Zero)
                {
                    Marshal.Copy(info.pbTag, tag, 0, tagLength);

                    Marshal.FreeHGlobal(info.pbNonce);
                    Marshal.FreeHGlobal(info.pbAuthData);
                    Marshal.FreeHGlobal(info.pbTag);
                    Marshal.FreeHGlobal(pbInfo);
                }
            }
        }

        /// <summary>
        /// Encrypts a block of data.
        /// </summary>
        /// <param name="input">
        /// The input to be encrypted. 
        /// </param>
        /// <returns>
        /// Encrypted output.
        /// </returns>
        public byte[] Decrypt(byte[] input)
        {
            return Decrypt(input, null, null, null);
        }

        /// <summary>
        /// Encrypts a block of data with cipher mode info specified.
        /// </summary>
        /// <param name="input">
        /// The input to be encrypted. 
        /// </param>
        /// <param name="nonce">
        /// Nonce.
        /// </param>
        /// <param name="authData">
        /// Auth data.
        /// </param>
        /// <param name="tag">
        /// Tag.
        /// </param>
        /// <returns>
        /// Encrypted output.
        /// </returns>
        public byte[] Decrypt(byte[] input, byte[] nonce, byte[] authData, byte[] tag)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            ValidateKeyExists();

            IntPtr pbInfo = IntPtr.Zero;

            BcryptAuthenticatedCipherModeInfo info = new BcryptAuthenticatedCipherModeInfo();

            try
            {
                if (nonce != null || authData != null || tag != null)
                {
                    info = new BcryptAuthenticatedCipherModeInfo
                    {
                        cbSize = (uint)Marshal.SizeOf(typeof(BcryptAuthenticatedCipherModeInfo)),
                        dwInfoVersion = 1,
                    };

                    if (nonce != null)
                    {
                        info.cbNonce = (uint)nonce.Length;
                        info.pbNonce = Marshal.AllocHGlobal(nonce.Length);
                        Marshal.Copy(nonce, 0, info.pbNonce, nonce.Length);
                    }

                    if (authData != null)
                    {
                        info.cbAuthData = (uint)authData.Length;
                        info.pbAuthData = Marshal.AllocHGlobal(authData.Length);
                        Marshal.Copy(authData, 0, info.pbAuthData, authData.Length);
                    }

                    if (authData != null)
                    {
                        info.cbTag = (uint)tag.Length;
                        info.pbTag = Marshal.AllocHGlobal(tag.Length);
                        Marshal.Copy(tag, 0, info.pbTag, tag.Length);
                    }

                    info.pbMacContext = IntPtr.Zero;
                    info.cbMacContext = 0;
                    info.cbAAD = 0;
                    info.cbData = 0;
                    info.dwFlags = 0;

                    pbInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BcryptAuthenticatedCipherModeInfo)));
                    Marshal.StructureToPtr(info, pbInfo, false);
                }

                uint cbOutput;
                uint status = NativeMethods.BCryptDecrypt(
                    hKey,
                    input,
                    (uint)input.Length,
                    pbInfo,
                    null,
                    0,
                    null,
                    0,
                    out cbOutput,
                    0);
                if (status != (uint)NtStatus.STATUS_SUCCESS)
                {
                    throw new CryptographicException(GetErrorNameFromCode(status));
                }

                byte[] output = new byte[cbOutput];
                status = NativeMethods.BCryptDecrypt(
                    hKey,
                    input,
                    (uint)input.Length,
                    pbInfo,
                    null,
                    0,
                    output,
                    cbOutput,
                    out cbOutput,
                    0);
                if (status != (uint)NtStatus.STATUS_SUCCESS)
                {
                    throw new CryptographicException(GetErrorNameFromCode(status));
                }

                return output;
            }
            finally
            {
                if (pbInfo != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(info.pbNonce);
                    Marshal.FreeHGlobal(info.pbAuthData);
                    Marshal.FreeHGlobal(info.pbTag);
                    Marshal.FreeHGlobal(pbInfo);
                }
            }
        }


        #region IDisposable Members

        /// <summary>
        /// Dispose method.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Dispose method.
        /// </summary>
        /// <param name="disposing">
        /// True to release both managed and unmanaged resources.<para/>
        /// False to release unmanaged resources only.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Release managed resources.
            }

            uint status;
            // Release unmanaged resources.
            if (hKey != IntPtr.Zero)
            {
                status = NativeMethods.BCryptDestroyKey(hKey);
                if (status == (uint)NtStatus.STATUS_SUCCESS)
                {
                    hKey = IntPtr.Zero;
                }
            }
            if (hAlgorithm != IntPtr.Zero)
            {
                status = NativeMethods.BCryptCloseAlgorithmProvider(hAlgorithm, 0);
                if (status == (uint)NtStatus.STATUS_SUCCESS)
                {
                    hAlgorithm = IntPtr.Zero;
                }
            }

            disposed = true;
        }


        /// <summary>
        /// finalizer
        /// </summary>
        ~BCryptAlgorithm()
        {
            Dispose(false);
        }

        #endregion
    }
}
