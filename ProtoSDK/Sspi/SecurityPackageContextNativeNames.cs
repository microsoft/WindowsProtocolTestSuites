// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Sspi
{
    /// <summary>
    /// The SecPkgContext_NativeNames structure returns the client and server principal names from the outbound ticket.
    /// This structure is valid only for client outbound tickets.
    /// http://msdn.microsoft.com/en-us/library/aa380090(v=VS.85).aspx
    /// </summary>
    public struct SecurityPackageContextNativeNames
    {
        /// <summary>
        /// Pointer to a null-terminated string that represents the principal name for the client in the outbound 
        /// ticket. This string should never be NULL when querying a security context negotiated with Kerberos.
        /// </summary>
        public string ClientName;

        /// <summary>
        /// Pointer to a null-terminated string that represents the principal name for the server in the outbound
        /// ticket. This string should never be NULL when querying a security context negotiated with Kerberos.
        /// </summary>
        public string ServerName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contextNativeNames">ContextNativeNames</param>    
        internal SecurityPackageContextNativeNames(SspiSecurityPackageContextNativeNames contextNativeNames)
        {
            this.ClientName = Marshal.PtrToStringUni(contextNativeNames.sClientName);
            this.ServerName = Marshal.PtrToStringUni(contextNativeNames.sServerName);
        }
    }
}
