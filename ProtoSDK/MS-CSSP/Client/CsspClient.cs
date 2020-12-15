// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.IO;
using System.Runtime.Versioning;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cssp
{
    /// <summary>
    /// Cssp client.
    /// </summary>
    [SupportedOSPlatform("Windows")]
    public class CsspClient
    {
        public CsspClient(Stream lowerLayerStream)
        {
            if (!OperatingSystem.IsWindows())
            {
                throw new PlatformNotSupportedException("The implementation of CsspClient is only supported on Windows.");
            }

            stream = new CsspClientStream(lowerLayerStream);
        }

        #region Public methods
        /// <summary>
        /// Authenticate as client.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        public void Authenticate(string domain, string principal, string userName, string password)
        {
            stream.Authenticate(domain, principal, userName, password);
        }

        /// <summary>
        /// Get data IO stream.
        /// </summary>
        /// <returns>The stream.</returns>
        public Stream GetStream()
        {
            return stream;
        }
        #endregion

        private CsspClientStream stream;
    }
}
