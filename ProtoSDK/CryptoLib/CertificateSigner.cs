// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// CryptAPI Wrapper of RSA signature.
    /// </summary>
    public class CertificateSigner
    {
        /// <summary>
        /// To store certificate used by signature.
        /// </summary>
        private X509Certificate rsaCertificate;

        /// <summary>
        /// Flags
        /// </summary>
        private CryptSignHashFlag flags;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="certificate">The certificate to sign</param>
        public CertificateSigner(X509Certificate certificate)
            : this(certificate, CryptSignHashFlag.None)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="certificate">The certificate to sign</param>
        /// <param name="flags">Flags</param>
        public CertificateSigner(X509Certificate certificate, CryptSignHashFlag flags)
        {
            this.rsaCertificate = certificate;
            this.flags = flags;
        }

        /// <summary>
        /// Flags used by CryptSignHash
        /// </summary>
        public CryptSignHashFlag Flags
        {
            get
            {
                return this.flags;
            }

            set
            {
                this.flags = value;
            }
        }


        /// <summary>
        /// To compute signature
        /// </summary>
        /// <param name="rgbHash">the hash value to sign</param>
        /// <param name="hashType">Hash type</param>
        /// <returns>the signature</returns>
        /// <exception cref="ArgumentNullException">If rgbHash is null, this exception will be thrown.</exception>
        /// <exception cref="NullReferenceException">If Certificate is set to null in constructor,
        /// this exception will be thrown</exception>
        /// <exception cref="SEHException">If Invoking CryptAPI failed, this exception will be thrown.</exception>
        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public byte[] SignHash(byte[] rgbHash, AlgId hashType)
        {
            if (rgbHash == null)
            {
                throw new ArgumentNullException("rgbHash");
            }

            if (this.rsaCertificate == null)
            {
                throw new NullReferenceException("Certificate can't be null.");
            }

            IntPtr pCertContext = rsaCertificate.Handle;
            IntPtr hCryptProv = IntPtr.Zero;
            uint keySpec = 0;
            bool freeProv = false;

            bool result = NativeMethods.CryptAcquireCertificatePrivateKey(
                pCertContext,
                (uint)CryptAcquireContextTypes.CRYPT_SILENT,
                IntPtr.Zero,
                out hCryptProv,
                out keySpec,
                out freeProv);
            if (!result)
            {
                throw new SEHException("Fail to invoke CryptAcquireCertificatePrivateKey failed.");
            }

            IntPtr hHashSend = IntPtr.Zero;
            IntPtr hHash = IntPtr.Zero;

            result = NativeMethods.CryptCreateHash(hCryptProv, hashType, IntPtr.Zero, 0, ref hHash);
            if (!result)
            {
                NativeMethods.CryptReleaseContext(hCryptProv, 0);
                throw new SEHException("Fail to invoke CryptCreateHash failed.");
            }
            hHashSend = Marshal.AllocHGlobal(rgbHash.Length);
            Marshal.Copy(rgbHash, 0, hHashSend, rgbHash.Length);

            result = NativeMethods.CryptSetHashParam(hHash, HashParameters.HP_HASHVAL, hHashSend, 0);
            if (!result)
            {
                NativeMethods.CryptReleaseContext(hCryptProv, 0);
                Marshal.FreeHGlobal(hHashSend);
                NativeMethods.CryptDestroyHash(hHash);
                throw new SEHException("Fail to invoke CryptSetHashParam failed.");
            }

            IntPtr pbSignature = IntPtr.Zero;
            uint signatureSize = 0;

            result = NativeMethods.CryptSignHash(hHash, NativeMethods.AT_SIGNATURE, IntPtr.Zero, this.flags, IntPtr.Zero, ref signatureSize);
            if (!result)
            {
                NativeMethods.CryptReleaseContext(hCryptProv, 0);
                Marshal.FreeHGlobal(hHashSend);
                NativeMethods.CryptDestroyHash(hHash);
                throw new SEHException("Fail to invoke CryptSignHash failed.");
            }

            pbSignature = Marshal.AllocHGlobal((int)signatureSize);
            result = NativeMethods.CryptSignHash(hHash, NativeMethods.AT_SIGNATURE, IntPtr.Zero, this.flags, pbSignature, ref signatureSize);
            if (!result)
            {
                Marshal.FreeHGlobal(pbSignature);
                NativeMethods.CryptReleaseContext(hCryptProv, 0);
                Marshal.FreeHGlobal(hHashSend);
                NativeMethods.CryptDestroyHash(hHash);
                throw new SEHException("Fail to invoke CryptSignHash failed.");
            }

            byte[] signature = new byte[signatureSize];
            Marshal.Copy(pbSignature, signature, 0, signature.Length);
            //The byte order of signature should be reversed, don't know the reason.
            Array.Reverse(signature);
            Marshal.FreeHGlobal(pbSignature);
            Marshal.FreeHGlobal(hHashSend);
            NativeMethods.CryptReleaseContext(hCryptProv, 0);
            NativeMethods.CryptDestroyHash(hHash);
            return signature;
        }
    }
}
