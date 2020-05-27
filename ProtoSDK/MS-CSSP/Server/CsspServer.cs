// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cssp
{
    /// <summary>
    /// Cssp server.
    /// </summary>
    public class CsspServer
    {
        public CsspServer(Stream lowerLayerStream)
        {
            stream = new CsspServerStream(lowerLayerStream);
        }

        #region Public methods
        /// <summary>
        /// Authenticate as server.
        /// </summary>
        /// <param name="x509Cert">The certificate.</param>
        /// <param name="principal">The principal.</param>
        public void Authenticate(X509Certificate x509Cert, string principal)
        {
            stream.Authenticate(x509Cert, principal);
        }

        /// <summary>
        /// Get the data IO stream.
        /// </summary>
        /// <returns>The stream.</returns>
        public Stream GetStream()
        {
            return stream;
        }
        #endregion

        private CsspServerStream stream;
    }
}
