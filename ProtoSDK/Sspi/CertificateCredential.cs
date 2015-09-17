// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Sspi
{
    /// <summary>
    /// User Credential with a certificate.
    /// </summary>
    public class CertificateCredential : ICredential
    {
        /// <summary>
        /// Store a certificate
        /// </summary>
        protected X509Certificate certificate;

        /// <summary>
        /// Constructor.
        /// Find the certificate in store by certificate name.If more than 1 certificate are found, 
        /// get the first.
        /// </summary>
        /// <param name="storeLocation">StoreLocation of certificate</param>
        /// <param name="storeName">StoreName of certificate</param>
        /// <param name="certificateName">Subject name of certificate.</param>
        public CertificateCredential(StoreLocation storeLocation, StoreName storeName, string certificateName)
        {
            X509Store store = new X509Store(storeName, storeLocation);

            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            X509Certificate2Collection certificates = store.Certificates.Find(X509FindType.FindBySubjectName, 
                certificateName, 
                false);

            if (certificates.Count > 0)
            {
                this.certificate = certificates[0];
            }
            store.Close();
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="certificate">A X509Certificate</param>
        public CertificateCredential(X509Certificate certificate)
        {
            this.certificate = certificate;
        }


        /// <summary>
        /// Gets stored X509Certificate.
        /// </summary>
        public X509Certificate Certificate
        {
            get
            {
                return this.certificate;
            }
        }
    }
}
