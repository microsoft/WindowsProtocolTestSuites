// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Sspi
{
    /// <summary>
    /// The CERT_CONTEXT structure contains both the encoded and decoded representations of a certificate. A 
    /// certificate context returned by one of the functions defined in Wincrypt.h must be freed by calling the 
    /// CertFreeCertificateContext function. 
    /// http://msdn.microsoft.com/en-us/library/aa377189(v=VS.85).aspx
    /// </summary>
    public struct CertContext
    {
        /// <summary>
        /// Type of encoding used. It is always acceptable to specify both the certificate and message encoding 
        /// types by combining them with a bitwise-OR operation.
        /// </summary>
        public uint CertEncodingType;

        /// <summary>
        /// A pointer to a buffer that contains the encoded certificate.
        /// </summary>
        public byte[] CertEncoded;

        /// <summary>
        /// The address of a CERT_INFO structure that contains the certificate information.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr pCertInfo;

        /// <summary>
        /// A handle to the certificate store that contains the certificate context.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr hCertStore;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="certContext">Cert context</param>    
        internal CertContext(SspiCertContext certContext)
        {
            this.CertEncodingType = certContext.dwCertEncodingType;
            this.CertEncoded = new byte[certContext.cbCertEncoded];
            Marshal.Copy(certContext.pbCertEncoded, this.CertEncoded, 0, this.CertEncoded.Length);
            this.pCertInfo = certContext.pCertInfo;
            this.hCertStore = certContext.hCertStore;
        }
    }
}
