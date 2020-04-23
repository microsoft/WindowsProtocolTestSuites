// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib
{
    /// <summary>
    /// Abstract base class for server SecurityContext, SecurityContext used by the server must be derived from this class.
    /// </summary>
    public abstract class ServerSecurityContext : SecurityContext
    {
        /// <summary>
        /// Accept client token.
        /// </summary>
        /// <param name="token">Client token</param>
        /// <exception cref="SspiException">If accept fail, this exception will be thrown.</exception>
        public abstract void Accept(byte[] token);
    }
}
