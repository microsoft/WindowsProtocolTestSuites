using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr
{
    class CertificateApi
    {
        [DllImport("Crypt32.dll", CharSet = CharSet.Unicode)]
        private static extern bool CertSerializeCertificateStoreElement(
            IntPtr pCertContext,
            uint dwFlags,
            [MarshalAs(UnmanagedType.LPArray,ArraySubType = UnmanagedType.U1)]
            byte[] pbElement,
            ref uint pcbElement
            );

        private const uint CRYPT_STRING_BASE64 = 0x00000001;

        [DllImport("Crypt32.dll", CharSet = CharSet.Unicode)]
        private static extern bool CryptBinaryToString(
            [MarshalAs(UnmanagedType.LPArray,ArraySubType = UnmanagedType.U1)]
            byte[] pbBinary,
            uint cbBinary,
            uint dwFlags,
            [MarshalAs(UnmanagedType.LPArray,ArraySubType = UnmanagedType.U2)]
            char[] pszString,
            ref uint pcchString
            );

        /// <summary>
        /// Encode the certificate according to the windows implementation.
        /// </summary>
        /// <param name="certificate">The certificate to be encoded.</param>
        /// <returns></returns>
        public static byte[] EncodeCertificate(X509Certificate2 certificate)
        {
            uint cbSerialized = 0;
            bool bRet;
            bRet = CertSerializeCertificateStoreElement(certificate.Handle, 0, null, ref cbSerialized);
            byte[] serialized = new byte[cbSerialized];
            bRet = CertSerializeCertificateStoreElement(certificate.Handle, 0, serialized, ref cbSerialized);

            uint cbCrypted = 0;

            bRet = CryptBinaryToString(serialized, cbSerialized, CRYPT_STRING_BASE64, null, ref cbCrypted);
            char[] crypted = new char[cbCrypted];
            bRet = CryptBinaryToString(serialized, cbSerialized, CRYPT_STRING_BASE64, crypted, ref cbCrypted);

            var result = new byte[crypted.Length * 2];
            for (int i = 0; i < crypted.Length; i++)
            {
                result[2 * i + 0] = (byte)((crypted[i] & 0x00ff) >> 0);
                result[2 * i + 1] = (byte)((crypted[i] & 0xff00) >> 8);
            }

            return result;
        }
    }
}
