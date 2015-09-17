// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Sspi
{
    /// <summary>
    /// Abstract base class for client SecurityContext, SecurityContext used by client must be derived from this class.
    /// </summary>
    public abstract class ClientSecurityContext : SecurityContext
    {
        /// <summary>
        /// Initialize SecurityContext by the server token.
        /// </summary>
        /// <param name="token">Server token</param>
        /// <exception cref="SspiException">If initialize fail, this exception will be thrown.</exception>
        public abstract void Initialize(byte[] token);
    }
}
