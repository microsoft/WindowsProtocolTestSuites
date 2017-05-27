// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    public partial class ServerDataModel
    {
        private X509Certificate2 _adfsSigningCertificate;
        private X509Certificate2 _adfsEncryptionCertificate;
        private X509Certificate2 _adfsCertificate;
        private X509Certificate2 _webAppCertificate;
        private string           _pfxPassword;

        public X509Certificate2 AdfsSigningCertificate
        {
            get { return _adfsSigningCertificate; }
        }

        public X509Certificate2 AdfsEncryptionCertificate
        {
            get { return _adfsEncryptionCertificate; }
        }

        public X509Certificate2 AdfsCertificate
        {
            get { return _adfsCertificate; }
        }

        public X509Certificate2 WebAppCertificate
        {
            get { return _webAppCertificate; }
        }

        /// <summary>
        /// Initialize certificates used by the server.
        /// </summary>
        private void InitialServerCertificates()
        {
            _pfxPassword               = EnvironmentConfig.PfxPassword;
            // load all certificates from PFX files
            _adfsCertificate           = new X509Certificate2(EnvironmentConfig.ADFSCert,    _pfxPassword, X509KeyStorageFlags.Exportable);
            _webAppCertificate         = new X509Certificate2(EnvironmentConfig.WebAppCert,  _pfxPassword, X509KeyStorageFlags.Exportable);
            _adfsSigningCertificate    = new X509Certificate2(EnvironmentConfig.SignCert,    _pfxPassword, X509KeyStorageFlags.Exportable);
            _adfsEncryptionCertificate = new X509Certificate2(EnvironmentConfig.EncryptCert, _pfxPassword, X509KeyStorageFlags.Exportable);
        }
    }
}
